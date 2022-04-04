using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.HumanResource;
using Azolution.Entities.Sale;
using Laps.Stock.DataService.DataService;
using LapsUtility;
using Utilities;

namespace Laps.Replacement.DataService.DataService
{
    public class ReplacementDataService
    {

        StockDataService _stockDataService = new StockDataService();

        public string ReplaceProduct(Azolution.Entities.Sale.Replacement aReplacement, Users objUser)
        {
            var companyId = 0;
            var branchId = 0;

            var companyInfo = GetCompanyInfoByCompany(aReplacement.ACustomer.CompanyId);

            if (companyInfo != null && companyInfo.CompanyStock == 1)// update stock balance as company or branch wise
            {
                companyId = companyInfo.CompanyType == "MotherCompany" ? companyInfo.CompanyId : companyInfo.RootCompanyId;
                branchId = 0;
            }
            else
            {
                companyId = companyInfo.CompanyId;
                branchId = aReplacement.ACustomer.BranchId;
            }

            string rv = "";
            string sql = "";
            var connection = new CommonConnection();
            connection.BeginTransaction();
            try
            {

                var entryDate = DateTime.Now;
                if (aReplacement.ReplacementId == 0)
                {

                    var stock = _stockDataService.checkExistStockBalanceByItemId(aReplacement.RefItemId, companyId, branchId, StockCategoryType.Replacement);
                    if (stock != null)
                    {
                        if (stock.StockBalanceQty > 0)
                        {
                            SaveReplacement(aReplacement, objUser, entryDate, connection);
                            UpdateStockBalanceForReplacement(aReplacement.AProduct.ModelId, aReplacement.RefItemId, objUser, connection,StockCategoryType.Replacement,companyId,branchId);


                            connection.CommitTransaction();
                            rv = Operation.Success.ToString();

                        }
                        else
                        {
                            rv = "No enough Stock!";
                        }

                    }
                    else
                    {
                        rv = "No enough Stock!";
                    }
                }


            }
            catch (Exception exception)
            {
                rv = exception.Message;
                connection.RollBack();
            }
            finally
            {
                connection.Close();
            }
            return rv;
        }

        private Company GetCompanyInfoByCompany(int companyId)
        {
            string sql = string.Format(@"Select * From Company Where CompanyId={0}", companyId);
            var data = Data<Company>.DataSource(sql).SingleOrDefault();
            return data;
        }

        private static void SaveReplacement(Azolution.Entities.Sale.Replacement aReplacement, Users user, DateTime entryDate, CommonConnection connection)
        {
            string sql;

            sql = string.Format(@"Insert Into Sale_Replacement(SaleInvoice,RefItemSLNo,ReplacedItemSLNo,EntryDate,InstallmentDate,ReplacementDate,Updated,ModelId,
                    SaleId,CustomerId,IsActive,ReplacedBy,RefItemId,SalesItemId) Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9},{10},{11},{12},{13})",
                   aReplacement.SaleInvoice, aReplacement.RefItemSLNo, aReplacement.ReplacedItemSLNo, entryDate, aReplacement.InstallmentDate, aReplacement.ReplacementDate, entryDate,
                   aReplacement.AProduct.ModelId, aReplacement.SaleId, aReplacement.ACustomer.CustomerId, 1, user.UserId, aReplacement.RefItemId, aReplacement.SalesItemId);

            connection.ExecuteNonQuery(sql);
        }

        public void UpdateStockBalanceForReplacement(int modelId, int itemId, Users user, CommonConnection connection, int stockCategoryId,int companyId,int branchId)
        {

            StockDataService _stockDataService = new StockDataService();
            StockBalance stockbalance = _stockDataService.CheckExistStock(modelId, itemId, companyId, branchId, stockCategoryId);

            if (stockbalance != null)
            {
            var newStockbalance = new StockBalance();
            newStockbalance.ModelId = modelId;
            newStockbalance.ItemId = itemId;
            newStockbalance.StockQuantity = stockbalance.StockBalanceQty;
            newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty - 1);
            newStockbalance.Type = 5; // TypeId 5 for deduction/Replacement

            newStockbalance.EntryDate = DateTime.Now;
            newStockbalance.EntryUserId = user.UserId;

            SaveStockBalanceForReplacement(connection, newStockbalance, companyId, branchId);

             }
        }

        private static void SaveStockBalanceForReplacement(CommonConnection connection, StockBalance newStockbalance, int companyId, int branchId)
        {
            string sql =
                string.Format(@" Insert Into StockBalance_Replacement(ModelId,ItemId,StockQuantity,StockBalanceQty,EntryDate,[Type],EntryUserId,CompanyId,BranchId) 
                                                    Values({0},{1},{2},{3},'{4}',{5},{6},{7},{8})",
                    newStockbalance.ModelId,
                    newStockbalance.ItemId, newStockbalance.StockQuantity,
                    newStockbalance.StockBalanceQty,
                    newStockbalance.EntryDate, newStockbalance.Type, newStockbalance.EntryUserId, companyId, branchId);
            connection.ExecuteNonQuery(sql);
        }

        public GridEntity<Azolution.Entities.Sale.Replacement> GetReplacementInfoByInvoiceNo(GridOptions options, string invoiceNo)
        {
            var sql = string.Format(@"Select SR.*,SC.Name,SC.FatherName,SC.Gender,SC.NID,SC.Address,SC.District,SC.DOB,SC.Phone,SC.CustomerCode,
SP.Model, SP.ProductName, SP.Code, SL.LType,SL.Number,SP.Type
From Sale_Replacement SR 
left join Sale_Customer SC on SC.CustomerId=SR.CustomerId 
left join (Select SP.*, SAT.Type From Sale_Product SP 
left join Sale_AllType SAT on SAT.TypeId = SP.TypeId Where SAT.Flag='Product' And SAT.IsActive=1) SP ON SP.ModelId =SR.ModelId 
left join (Select * FROM Sale_License SL Where SL.IsActive=1 and SL.IsSentSMS=1) SL ON SL.SaleInvoice =SR.SaleInvoice 
LEFT JOIN Sale S ON S.SaleId=SR.SaleId Where S.Invoice='{0}'", invoiceNo);


            var data = Kendo<Azolution.Entities.Sale.Replacement>.Grid.GenericDataSource(options, sql, "ReplacementId");
            return data;
        }

        public List<Installment> GetInstallmentId(string invoiceNo)
        {
            try
            {
                string sql = string.Format("Select MIN(SI.InstallmentId) InstallmentId,MIN(SI.SInvoice) SInvoice, MIN(SI.Number) MinNumber, MAX(SI.Number) Number From Sale_Installment SI " +
                                           "LEFT JOIN (SELECT SR.SaleInvoice, SL.IssueDate FROM Sale S " +
                                           "LEFT JOIN Sale_Replacement SR ON SR.RefProductNo= S.ProductNo " +
                                           "LEFT JOIN Sale_License SL ON SL.ProductNo=SR.ReplacedNo Where SR.IsActive=1) SR ON SR.SaleInvoice=SI.SInvoice " +
                                           "WHERE SI.SInvoice='{0}'", invoiceNo);
                return Data<Installment>.GenericDataSource(sql);
                //if (data != null) return data.InstallmentId.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #region Customer Service Replacement

        public string SaveReplacementForCustomerService(ReplacementCs objReplacementCs, List<ReplaceItem> objItemList, BranchInfo companyBranchInfo, Users user)
        {
            string sql = "";
            string rv = "";
            CommonConnection connection = new CommonConnection();
            connection.BeginTransaction();
            try
            {
                if (objItemList.Any())
                {
                    SaveReplacementCs(objReplacementCs, objItemList, user, connection);

                    foreach (var replaceItem in objItemList)
                    {
                        UpdateStockBalanceForCS(objReplacementCs.ModelId, replaceItem.ItemId, companyBranchInfo, user, connection);
                    }

                    connection.CommitTransaction();
                    rv = Operation.Success.ToString();
                }


            }
            catch (Exception exception)
            {
                connection.RollBack();
                rv = exception.Message;
            }
            finally
            {
                connection.Close();
            }
            return rv;
        }

        private void SaveReplacementCs(ReplacementCs objReplacementCs, List<ReplaceItem> objItemList, Users user, CommonConnection connection)
        {
            StringBuilder qBuilder = new StringBuilder();
            string sql = string.Empty;
            try
            {
                if (objItemList.Any())
                {
                    foreach (var item in objItemList)
                    {
                        qBuilder.Append(string.Format(@"Insert Into Replacement_CustomerService ([ItemId],[ModelId],[ReplacedQty],[ReplacementDate],[BranchId],[EntryDate],[ReplacedBy])
                            Values({0},{1},{2},'{3}',{4},'{5}',{6});", item.ItemId, objReplacementCs.ModelId, item.ReplacedItemQty, objReplacementCs.ReplacementDate, objReplacementCs.BranchId, DateTime.Now, user.UserId));
                    }
                    if (qBuilder.ToString() != "")
                    {
                        sql = "Begin " + qBuilder + " End;";
                        connection.ExecuteNonQuery(sql);
                    }

                }
            }
            catch (Exception)
            {

                throw new Exception("Errro ! While Save Replacement Information");
            }
        }

        private void UpdateStockBalanceForCS(int modelId, int itemId, BranchInfo companyBranchInfo, Users user, CommonConnection connection)
        {
            var companyId = 0;
            var branchId = 0;

            if (companyBranchInfo.CompanyStock == 1)// update stock balance as company or branch wise
            {

                companyId = companyBranchInfo.CompanyType == "MotherCompany" ? companyBranchInfo.CompanyId : companyBranchInfo.RootCompanyId;
            }
            else
            {
                companyId = companyBranchInfo.CompanyId;
                branchId = companyBranchInfo.BranchId;
            }

            StockDataService _stockDataService = new StockDataService();
            StockBalance stockbalance = _stockDataService.CheckExistStock(modelId, itemId, companyId, branchId, StockCategoryType.Replacement);

            if (stockbalance != null)
            {
                var newStockbalance = new StockBalance();
                newStockbalance.ModelId = modelId;
                newStockbalance.ItemId = itemId;
                newStockbalance.StockQuantity = stockbalance.StockBalanceQty;
                newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty - 1);
                newStockbalance.Type = 5; // TypeId 5 for deduction/Replacement

                newStockbalance.EntryDate = DateTime.Now;
                newStockbalance.EntryUserId = user.UserId;

                SaveStockBalanceForReplacement(connection, newStockbalance, companyId, branchId);

            }
            else
            {
                throw new Exception("No enough stock for Replacement!");
            }
        }

        #endregion

        public BranchInfo GetBranchInfoByBranchId(int branchId)
        {
            string sql = @"Select * From Branch left join Company on Company.CompanyId=Branch.COMPANYID Where BranchId= " + branchId;
            var data = Data<BranchInfo>.DataSource(sql);
            return data.SingleOrDefault();
        }


        public CustomerPackage GetCustomerAndPackageInfoByCustomerCode(string customerCode, Users user)
        {

            string sqlCustomer = string.Format(@"Select * From Sale_Customer Where CustomerCode='{0}'", customerCode);
            var customerPackage = new CustomerPackage();
            customerPackage = Data<CustomerPackage>.DataSource(sqlCustomer).SingleOrDefault();

            string sqlpackage = string.Format(@"Select Sale.ModelId,SP.ProductName
                                        From Sale_Customer SC
                                        Inner join Sale on Sale.CustomerId = SC.CustomerId 
                                        inner join Sale_Product SP on SP.ModelId = Sale.ModelId 
                                        Where CustomerCode='{0}'", customerCode);

            var packageInfo = Data<PackageInfo>.GenericDataSource(sqlpackage);

            if (customerPackage != null) customerPackage.PackageInfos = packageInfo;
            return customerPackage;
        }

        public SalePackageInfo GetPackageSaleInfo(int modelId, string customerCode)
        {

            string sql = string.Format(@"Select SC.CustomerCode,SC.CustomerId,SC.Name,SC.CompanyId,SC.BranchId,
                Sale.ModelId,SP.ProductName,Sale.SaleId,Sale.WarrantyStartDate
                From Sale_Customer SC
                Inner join Sale on Sale.CustomerId = SC.CustomerId 
                inner join Sale_Product SP on SP.ModelId = Sale.ModelId 
                Where CustomerCode='{0}' and SP.ModelId={1}", customerCode, modelId);

            var data = Data<SalePackageInfo>.GenericDataSource(sql).SingleOrDefault();
            return data;
        }
    }
}
