using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Azolution.Entities.Sale;
using Laps.Stock.DataService.DataService;
using LapsUtility;
using SmsService;
using Solaric.GenerateCode;
using Utilities;
using Utilities.Common;

namespace Laps.Sale.SaleDataService.DataService
{
    public class SaleInactiveDataService
    {
        public bool CheckInstallment(string invoice)
        {
            try
            {
                var tempQuery = string.Format(@"Select * From Sale_Installment_Temp where SInvoice='{0}' and Status=1", invoice);
                var tempIms = Data<Installment>.DataSource(tempQuery);

                var query = string.Format(@"Select * From Sale_Installment where SInvoice='{0}' and Status=1", invoice);
                var ims = Data<Installment>.DataSource(query);
                return ims.Count <= tempIms.Count + 1;
            }
            catch (Exception)
            {
                throw new Exception("Error! While Installment Checking");
            }
        }

        public bool CheckDP(int saleId)
        {
            try
            {
                string sql = string.Format(@"select * from Sale where SaleId={0} and IsDownPayCollected='TRUE'", saleId);
                var data = Data<Azolution.Entities.Sale.Sale>.DataSource(sql);
                if (data.Any())
                {
                    return true;
                }
                else
                {
                    throw new Exception("This Sale not eligible for Inactive due to DP is not collected yet");
                }
            }
            catch (Exception)
            {
                throw new Exception("Error! While Check downpayment status");
            }
        }

        public void SetSaleStateToBooked(int saleId, CommonConnection connection)
        {
            try
            {

                string sql = string.Format(@"Update Sale set State={1}, IsActive=0,IsDownPayCollected=0 where SaleId={0}", saleId, SaleStates.State.SaveAsBooked);
                connection.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                throw new Exception("Error! While Update Sale State to Booked");
            }
        }

        public void RolleBackInstallment(string invoice, CommonConnection connection)
        {
            try
            {
                var deleteQuery = string.Format(@"delete from Sale_Installment where SInvoice='{0}'", invoice);
                var rbQueury = string.Format(@"INSERT INTO [Sale_Installment]([SInvoice],[ProductNo],[Number] ,[Amount],[Status],[DueDate],[EntryDate],[Updated],[Flag])
                Select [SInvoice],[ProductNo],[Number] ,[Amount],[Status],[DueDate],[EntryDate],[Updated],[Flag] From Sale_Installment_Temp where Sale_Installment_Temp.SInvoice='{0}'", invoice);
                connection.ExecuteNonQuery(deleteQuery);
                // connection.ExecuteNonQuery(rbQueury);
            }
            catch (Exception)
            {
                throw new Exception("Error! While Rollback Installment Information");
            }
        }

        public void DeleteLicenseOfSale(string invoice, CommonConnection connection)
        {
            try
            {
                string sql = string.Format(@"Delete from  Sale_License where SaleInvoice='{0}' 
                and LicenseId=(select top 1 LicenseId from Sale_License where SaleInvoice='{0}' and LType=1 order by LicenseId desc)", invoice);
                connection.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                throw new Exception("Error! While Deleting Initial License");
            }
        }

        public List<Azolution.Entities.Sale.Sale> GetCustomerSalesInformation(string customerCode, int branchId, int companyId)
        {
            try
            {
                string query = string.Format(@"SELECT S.SaleId,S.Invoice,S.WarrantyStartDate,S.FirstPayDate,S.DownPay,S.SalesRepId,
                    S.Price,S.NetPrice,S.Installment,S.State,S.TempState,isnull(DPT.ReceiveAmount,0) ReceiveAmount,SC.CustomerId, 
                    SC.CustomerCode, SC.Name,SC.Phone,SC.Phone2,SC.IsUpgraded,SP.ProductName,SP.ModelId,SP.Model,S.SaleTypeId,S.CompanyId,Branch.BRANCHID,BranchCode,
                    s.IsActive,BranchSmsMobileNumber,(Select distinct IsLisenceRequired From Sale_Product_Items Where ModelId=S.ModelId and IsLisenceRequired=1)IsLisenceRequired,
                    Discount.DiscountTypeCode,Discount.IsApprovedSpecialDiscount,S.EntryDate
                    FROM Sale S 
                    LEFT JOIN Sale_Product SP ON SP.ModelId=S.ModelId 
                    LEFT JOIN Sale_Customer SC ON SC.CustomerId=S.CustomerId And SC.IsActive=1
                    Left Outer join Branch on Branch.BRANCHID = SC.BranchId
                    Left Outer join Discount on Discount.SaleId=S.SaleId
                    Left outer join DownpamentCollection_tmp DPT on DPT.SaleId=S.SaleId
                    where  S.IsRelease <> 1 And S.State <> 2 and S.CompanyId={0} and S.BranchId={1} and SC.customercode='{2}'", companyId, branchId, customerCode);
                var data = Data<Azolution.Entities.Sale.Sale>.GenericDataSource(query);


                if (data.Count > 1)
                {
                    var lastData = data.OrderByDescending(s => s.SaleId).FirstOrDefault();
                    data.RemoveAll(s => s.SaleId != lastData.SaleId);
                }


                return data;

            }
            catch (Exception)
            {
                throw new Exception("Error! While select Customer Sales Information");
            }
        }

        public bool CheckIsPackageAvailable(int modelId, int changedCompanyId)
        {
            var companyInfo = GetCompanyInf(changedCompanyId);

            string sql = string.Format(@"Select * From Sale_Product Where ModelId={0} and CompanyId={1}", modelId, companyInfo.RootCompanyId);
            var data = Data<Product>.DataSource(sql).SingleOrDefault();
            return data != null;
        }

        public string SaleBranchSwitch(SaleRollback rollbackInfo, Users user)
        {
            CommonConnection connection = new CommonConnection();
            string rv = "";
            try
            {
                connection.BeginTransaction();

                SaleDataService _saleDataService = new SaleDataService();
                string itemCode = "OP";
                var slaesItemDetails = _saleDataService.GetItemDetailsByInvoiceNo(rollbackInfo.InvoiceNo, itemCode);

                var itemSlNo = slaesItemDetails != null && slaesItemDetails.ItemSLNo == null ? "" : slaesItemDetails.ItemSLNo.Trim();
                rollbackInfo.ItemSLNo = itemSlNo;

                UpdateCustomerInformation(rollbackInfo, connection);
                UpdateSaleInformation(rollbackInfo, connection);
                UpdateStockBalance(rollbackInfo, connection, user);
                GenerateInitialLicenseAndSendSms(rollbackInfo, connection);

                connection.CommitTransaction();
                rv = Operation.Success.ToString();

            }
            catch (Exception exception)
            {
                connection.RollBack();
                rv = Operation.Failed.ToString();
            }
            finally
            {

                connection.Close();
            }
            return rv;
        }

        private void UpdateSaleInformation(SaleRollback rollbackInfo, CommonConnection connection)
        {
            string sql = string.Empty;
            sql = string.Format(@"Update Sale Set SalesRepId='{0}',CompanyId={1},BranchId={2} Where SaleId={3}", rollbackInfo.SalesRepId, rollbackInfo.ChangedCompanyId, rollbackInfo.ChangedBranchId, rollbackInfo.SaleId);
            connection.ExecuteNonQuery(sql);
        }

        private Branch GetBranchInfoByBranchId(int changedBranchId)
        {
            var sql = string.Empty;
            sql = string.Format(@"Select * From Branch Where BranchId={0}", changedBranchId);
            var branch = Data<Branch>.DataSource(sql).SingleOrDefault();
            return branch;
        }

        private void UpdateCustomerInformation(SaleRollback rollbackInfo, CommonConnection connection)
        {
            var branchInfo = GetBranchInfoByBranchId(rollbackInfo.ChangedBranchId);
            string branchCode = branchInfo.BranchCode.Substring(branchInfo.BranchCode.Length - 4);
            string customerCode = rollbackInfo.CustomerCode.Substring(rollbackInfo.CustomerCode.Length - 4);
            rollbackInfo.ReferenceId = branchCode + customerCode;

            string sql = string.Empty;
            sql = string.Format(@"Update Sale_Customer Set ReferenceId='{0}', CompanyId={1},BranchId={2},IsStaff={3},StaffId='{4}' Where CustomerCode='{5}'", rollbackInfo.ReferenceId, rollbackInfo.ChangedCompanyId, rollbackInfo.ChangedBranchId, rollbackInfo.IsStaff, rollbackInfo.StaffId, rollbackInfo.CustomerCode);
            connection.ExecuteNonQuery(sql);
        }
        private void UpdateStockBalance(SaleRollback rollbackInfo, CommonConnection connection, Users users)
        {
            StringBuilder qBuilder1 = new StringBuilder();
            StringBuilder qBuilder2 = new StringBuilder();

            string sql = "";
            var itemList = GetSalesItemInfo(rollbackInfo.SaleId);

            var oldCompanyData = GetCompanyInf(rollbackInfo.CompanyId);
            var companyId = 0;
            var branchId = 0;
            if (oldCompanyData.CompanyStock == 1)
            {
                companyId = oldCompanyData.RootCompanyId;
                branchId = 0;
            }
            else
            {
                companyId = oldCompanyData.CompanyId;
                branchId = rollbackInfo.BranchId;
            }

            StockUpdateProcess(rollbackInfo.ModelId, companyId, branchId, users, itemList, 1, qBuilder1); //1 = addition of stock


            var changedCompanyData = GetCompanyInf(rollbackInfo.ChangedCompanyId);
            if (changedCompanyData.CompanyStock == 1)
            {
                companyId = changedCompanyData.RootCompanyId;
                branchId = 0;
            }
            else
            {
                companyId = changedCompanyData.CompanyId;
                branchId = rollbackInfo.ChangedBranchId;
            }
            StockUpdateProcess(rollbackInfo.ModelId, companyId, branchId, users, itemList, 2, qBuilder2); //2 = deduction of stock


            if (qBuilder1.ToString() != "")
            {
                sql = "Begin " + qBuilder1 + " End;";
                connection.ExecuteNonQuery(sql);
            }

            if (qBuilder2.ToString() != "")
            {
                sql = "Begin " + qBuilder2 + " End;";
                connection.ExecuteNonQuery(sql);
            }

        }

        private static void StockUpdateProcess(int modelId, int companyId, int branchId, Users users, List<SalesItem> itemList, int adjustmentType, StringBuilder qBuilder)
        {
            StockDataService _stockDataService = new StockDataService();
            SaleDataService _saleDataService = new SaleDataService();
            foreach (var salesItem in itemList)
            {
                var stockbalance = new StockBalance();
                stockbalance = _stockDataService.CheckExistStock(modelId, salesItem.ItemId, companyId, branchId, StockCategoryType.Sale);
                if (stockbalance != null)
                {
                    var newStockbalance = new StockBalance();
                    newStockbalance.ModelId = modelId;
                    newStockbalance.ItemId = salesItem.ItemId;
                    newStockbalance.StockQuantity = stockbalance.StockBalanceQty;
                    if (adjustmentType == 1) // AdjustmentTypeId 1 for addition , 2 for deduction
                    {
                        newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty + salesItem.ItemQuantity);
                        newStockbalance.Type = 2; // TypeId 2 for addition/update
                    }
                    else
                    {
                        newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty - salesItem.ItemQuantity);
                        newStockbalance.Type = 3; // TypeId 3 for deduction/damage
                    }
                    newStockbalance.EntryDate = DateTime.Now;
                    newStockbalance.EntryUserId = users.UserId;

                    _stockDataService.SaveStockBalance(qBuilder, newStockbalance.ModelId, newStockbalance.ItemId, newStockbalance.StockQuantity, newStockbalance.StockBalanceQty, newStockbalance.EntryUserId,
                        companyId, branchId, stockbalance.Type, StockCategoryType.Sale);
                }
                else
                {
                    var newStockbalance = new StockBalance();
                    newStockbalance.ModelId = modelId;
                    newStockbalance.ItemId = salesItem.ItemId;
                    newStockbalance.StockQuantity = newStockbalance.StockBalanceQty;
                    if (adjustmentType == 1)
                    {
                        newStockbalance.StockBalanceQty = (newStockbalance.StockBalanceQty - salesItem.ItemQuantity);
                        newStockbalance.Type = 4; // TypeId 4 for deduction/Sale
                    }
                    else
                    {
                        newStockbalance.StockBalanceQty = (newStockbalance.StockBalanceQty + salesItem.ItemQuantity);
                        newStockbalance.Type = 2; // TypeId 2 for addition/update
                    }
                    newStockbalance.EntryDate = DateTime.Now;
                    newStockbalance.EntryUserId = users.UserId;
                    newStockbalance.CompanyId = companyId;
                    newStockbalance.BranchId = branchId;

                    _saleDataService.SaveStockBalanceQuery(qBuilder, newStockbalance);

                    // _stockDataService.SaveStockBalance(qBuilder, modelId, salesItem.ItemId, salesItem.ItemQuantity, salesItem.ItemQuantity, users.UserId, companyId, branchId, 1, StockCategoryType.Sale);
                }
            }

        }



        private List<SalesItem> GetSalesItemInfo(int saleId)
        {
            var sqlPackage = string.Empty;
            sqlPackage = string.Format(@"Select * From SalesItem Where SaleId={0}", saleId);
            var itemList = Data<SalesItem>.DataSource(sqlPackage);
            return itemList;
        }

        private static Company GetCompanyInf(int companyId)
        {
            string sqlCompany = string.Empty;
            sqlCompany = string.Format(@"Select * From Company Where CompanyId={0}", companyId);
            var companyData = Data<Company>.DataSource(sqlCompany).SingleOrDefault();
            return companyData;
        }

        private void GenerateInitialLicenseAndSendSms(SaleRollback rollbackInfo, CommonConnection connection)
        {

            DeleteLicenseOfSale(rollbackInfo.InvoiceNo, connection);

            var branchInfo = GetBranchInfoByBranchId(rollbackInfo.ChangedBranchId);
            rollbackInfo.BranchCode = branchInfo.BranchCode;


            var obj = new Azolution.Entities.Sale.Sale();
            obj.ACustomer = new Customer();
            obj.ALicense = new License();
            obj.AProduct = new Product();
            obj.ACustomer.BranchCode = rollbackInfo.BranchCode;
            obj.ACustomer.CustomerCode = rollbackInfo.CustomerCode;
            obj.ACustomer.IsUpgraded = rollbackInfo.IsCustomerUpgraded;
            obj.FirstPayDate = rollbackInfo.FirstPayDate;
            obj.ItemSlNo = rollbackInfo.ItemSLNo;
            var license = new LicenceService().GetLicenseObjectNew(obj);

            SmsManager _smsManager = new SmsManager();
            LicenseInfo licenseInfo = new LicenseInfo();
            licenseInfo.LType = 1;
            licenseInfo.Code = license.Number;
            licenseInfo.CustomerCode = rollbackInfo.CustomerCode;
            licenseInfo.DownPay = rollbackInfo.DownPay;

            string smsText = _smsManager.GetSmsText(licenseInfo);

            string simNumber = rollbackInfo.SimNumber;


            obj.ALicense.IssueDate = rollbackInfo.FirstPayDate;
            obj.Invoice = rollbackInfo.InvoiceNo;
            obj.AProduct.ModelId = rollbackInfo.ModelId;


            string query = string.Empty;

            query = string.Format("INSERT INTO SMSSent(SMSText,MobileNumber,[RequestDateTime],[Status],ReplyFor,SimNumber) VALUES(N'{0}','{1}','{2}',{3},{4},'{5}')",
                   smsText, rollbackInfo.Phone2, rollbackInfo.FirstPayDate, 0, 0, simNumber);
            connection.ExecuteNonQuery(query);


            var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");
            int IsSmsSent = 1;
            query = string.Format(@"INSERT INTO Sale_License([Number],[LType],[IssueDate],[EntryDate] ,[Updated],[ModelId],[SaleInvoice],[ProductNo],[Flag],[IsActive],IsSentSMS)
                                        VALUES('{0}',{1},'{2}','{3}','{4}',{5},'{6}','{7}',{8},{9},{10})",
                    obj.ALicense.Number, obj.ALicense.LType, obj.ALicense.IssueDate, entrydate, "",
                    obj.AProduct.ModelId, obj.Invoice, obj.AProduct.ProductNo, 0, 1, IsSmsSent);
            connection.ExecuteNonQuery(query);


        }


        public List<Azolution.Entities.Sale.Sale> GetSaleInformationByInvoice(string invoice)
        {
            string sql = string.Format(@"Select * From Sale Where Invoice='{0}'", invoice);
            var data = Data<Azolution.Entities.Sale.Sale>.DataSource(sql);
            return data;
        }

        public List<Azolution.Entities.Sale.Collection> GetAllCollectedDownPayByInvoice(string invoice)
        {
            string sql = string.Format(@"Select * From Sale_Collection Where CollectionType=3 And SaleInvoice='{0}' order by CollectionId", invoice);
            var data = Data<Azolution.Entities.Sale.Collection>.DataSource(sql);
            return data;
        }

        public void SaveCollectionIntoTemp(Azolution.Entities.Sale.Collection collectionObj, CommonConnection connection)
        {
            try
            {
                string sql = string.Format(@"Insert Into RollBackCollection_Temp(SaleInvoice,ReceiveAmount,PayDate) Values('{0}',{1},'{2}')", collectionObj.SaleInvoice, collectionObj.ReceiveAmount, collectionObj.PayDate);
                connection.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                throw new Exception("Error! While Save Collection Into Temp");
            }
        }

        public void DeleteAllCollectionData(List<Azolution.Entities.Sale.Collection> collectionList, CommonConnection connection)
        {
            try
            {
                StringBuilder qBuilder = new StringBuilder();
                string sql = "";

                foreach (var collection in collectionList)
                {
                    qBuilder.Append(string.Format(@"Delete From Sale_Collection Where CollectionId={0};", collection.CollectionId));
                }

                if (qBuilder.ToString() != "")
                {
                    sql = "Begin " + qBuilder + " End;";
                    connection.ExecuteNonQuery(sql);
                }
            }
            catch (Exception)
            {
                throw new Exception("Error! While Delete Collection Data");
            }
        }
    }
}
