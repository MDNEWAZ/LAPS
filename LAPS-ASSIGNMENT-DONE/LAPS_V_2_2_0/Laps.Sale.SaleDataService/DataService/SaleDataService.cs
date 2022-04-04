using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Laps.AdminSettings.Service.Service;
using Laps.Collection.CollectionDataService.DataService;
using Laps.SaleRepresentative.DataService.DataService;
using Laps.SaleRepresentative.Service.Service;
using Laps.Stock.DataService.DataService;
using SmsService;
using Utilities;

namespace Laps.Sale.SaleDataService.DataService
{
    public class SaleDataService
    {
        SqlCommand _aCommand;
        SqlDataAdapter _adapter;
        readonly string _connection = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        private readonly SqlConnection _aConnection;

        public SaleDataService()
        {
            _aConnection = new SqlConnection(_connection);
        }

        public GridEntity<Azolution.Entities.Sale.Sale> GetAllSale(GridOptions options, int userId, string condition)
        {
            var query = string.Format("SELECT S.Invoice,S.EntryDate, Convert(Datetime,S.EntryDate,111) EntryDate2,S.Price,S.Installment,S.ProductNo, SC.CustomerCode, SC.Name,SC.Phone,SP.ProductName,SP.Model FROM Sale S " +
                    "LEFT JOIN Sale_Product SP ON SP.ModelId=S.ModelId LEFT JOIN Sale_Customer SC ON SC.CustomerId=S.CustomerId " +
                    "WHERE S.SaleUserId={0} {1}", userId, condition);
            var data = Kendo<Azolution.Entities.Sale.Sale>.Grid.GenericDataSource(options, query, "EntryDate DESC");
            return data;
        }

        public object GetSaleType()
        {
            var query = string.Format(@"SELECT TypeId, [Type] FROM [Sale_AllType] WHERE [IsActive]=1 AND Flag='Sale'");
            var data = Kendo<AllType>.Combo.DataSource(query);
            return data;
        }


        public string SaveSale(SalesParam salesParam, int state)
        {
            var connectionAfterReturnId = new CommonConnection(IsolationLevel.ReadCommitted);
            var collectionDataService = new CollectionDataService();
            connectionAfterReturnId.BeginTransaction();
            try
            {
                var saleUserId = salesParam.User.UserId;
                salesParam.SaleInfo.SaleUserId = saleUserId;
                salesParam.SaleInfo.State = state;

                var customerId = SaveCustomerInfo(salesParam.CustomerInfo, connectionAfterReturnId, salesParam.User);
                var saleId = SaveSaleInfo(salesParam.SaleInfo, connectionAfterReturnId, customerId, salesParam.User, salesParam.CustomerInfo.IsCustomerForMultipeSale,salesParam.DiscountInfo.IsApprovedSpecialDiscount);
                List<SalesItem> previousSalesitems = GetPreviousSalesItem(saleId);  
                SaveSalesProductItem(saleId, salesParam.ItemInfoLis, salesParam.ItemDetailsInfoList, connectionAfterReturnId);
                if (salesParam.DiscountInfo != null)
                {
                    SaveDiscountInfo(salesParam.SaleInfo, saleId, salesParam.DiscountInfo, connectionAfterReturnId); 
                }
             
                //Stock will be adjusted to Final Sale New on 23-07-2016  (Previous Update Stock Balance)
               // UpdateStockBalance(salesParam.SaleInfo, salesParam.ItemInfoLis, salesParam.User, connectionAfterReturnId, previousSalesitems);

                if (salesParam.SaleInfo.SaleTypeId == 2)
                {
                    // SaveCashPayment(salesParam.SaleInfo, connectionAfterReturnId, saleUserId);
                }

                //if (salesParam.SaleInfo.SaleTypeId == 1)
                //{
                    salesParam.DownPayCollection.SaleInvoice = salesParam.SaleInfo.Invoice;
                    collectionDataService.DownpaymentCollection(salesParam.DownPayCollection, saleUserId, connectionAfterReturnId, saleId);
               // }
                connectionAfterReturnId.CommitTransaction();
                return Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                connectionAfterReturnId.RollBack();
                return ex.Message;
            }
            finally
            {
                connectionAfterReturnId.Close();
            }
        }



        private void SaveDiscountInfo(Azolution.Entities.Sale.Sale aSale, int saleId, Discount objDiscountInfo, CommonConnection connectionAfterReturnId)
        {
            string sql = "";
            try
            {
                if (objDiscountInfo != null)
                {
                    if (objDiscountInfo.DiscountId == 0)
                    {
                        sql = string.Format(@"Insert Into Discount(SaleId,InvoiceNo,DiscountOptionId,DiscountTypeCode,DiscountAmount,EntryDate,IsApprovedSpecialDiscount,UserId) 
                       Values({0},'{1}',{2},'{3}',{4},'{5}',{6},{7})", saleId, aSale.Invoice, objDiscountInfo.DiscountOptionId, objDiscountInfo.DiscountTypeCode, objDiscountInfo.DiscountAmount, DateTime.Now, 0, 0);


                    }
                    else
                    {
                        sql = string.Format(@"Update Discount Set SaleId={0},InvoiceNo='{1}',DiscountOptionId={2},DiscountTypeCode='{3}',DiscountAmount={4} Where DiscountId={5}", saleId, aSale.Invoice, objDiscountInfo.DiscountOptionId, objDiscountInfo.DiscountTypeCode, objDiscountInfo.DiscountAmount, objDiscountInfo.DiscountId);
                    }

                    connectionAfterReturnId.ExecuteNonQuery(sql);
                }

            }
            catch (Exception)
            {
                throw new Exception("Failed To Save Discount Info!");
            }

        }

        public List<SalesItem> GetPreviousSalesItem(int saleId)
        {
            string sql = string.Format(@"Select * From SalesItem Where SaleId={0}", saleId);
            var data = Data<SalesItem>.DataSource(sql);
            return data;
        }

        private void SaveDownPaymentCollection(Azolution.Entities.Sale.Sale aSale, Azolution.Entities.Sale.Collection objDownPayCollection, int saleUserId, CommonConnection connectionAfterReturnId)
        {
            if (objDownPayCollection.ReceiveAmount > 0)
            {
                var entryDate = DateTime.Now;
                var insertQuery = string.Format(@" Insert Into Sale_Collection(SaleInvoice,InstallmentNo,TransectionType,TransectionId,
                                    ReceiveAmount,PayDate,EntryDate,InstallmentId,Flag,IsActive,CollectedBy,DueAmount,CollectionType,DueDate) 
                        Values('{0}',{1},{2},'{3}',{4},'{5}','{6}',{7},{8},{9},{10},{11},{12},'{13}');",
                    aSale.Invoice, 0, 1, 0, objDownPayCollection.ReceiveAmount, objDownPayCollection.PayDate, entryDate, 0, 1, 1, saleUserId, objDownPayCollection.DueAmount, 3, "");
            }
        }

        private void SaveCashPayment(Azolution.Entities.Sale.Sale aSale, CommonConnection connectionAfterReturnId, int userId)
        {
            CollectionDataService _collectionDataService = new CollectionDataService();
            _collectionDataService.SaveCashPayment(aSale, connectionAfterReturnId, userId);

        }

        private int SaveSaleInfo(Azolution.Entities.Sale.Sale aSale, CommonConnection connectionAfterReturnId, int customerId, Users saleUser, bool isMultiplealeSaleRequest, int isApprovedSpecialDiscount)
        {

            try
            {
                var companyId = saleUser.ChangedCompanyId == 0 ? saleUser.CompanyId : saleUser.ChangedCompanyId;
                var branchId = saleUser.ChangedBranchId == 0 ? saleUser.BranchId : saleUser.ChangedBranchId;

                var isSpecialDiscount = 0;
                var installmentNo = aSale.Installment == 0 ? 1 : aSale.Installment;
                if (aSale.SaleTypeId == 2 || aSale.SaleTypeId == 1)
                {
                    if (aSale.IsSpecialDiscount == 1)
                    {
                        isSpecialDiscount = 1;
                        aSale.NetPrice = aSale.Price;
                        if (isApprovedSpecialDiscount != 1)
                        {
                            aSale.State = LapsUtility.SaleStates.State.Unrecognized;
                            aSale.TempState = LapsUtility.SaleStates.State.SaveAsBooked;
                        }
                    }

                }
                if ( aSale.IsSpecialDiscount == 1)
                {
                    isSpecialDiscount = 1;
                }

                var saleId = 0;
                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");
                var updatedate = DateTime.Now.ToString("dd-MMM-yyyy");
                if (aSale.SaleId == 0)
                {
                    if (!isMultiplealeSaleRequest)
                    {
                        saleId = SaveNewSale(aSale, connectionAfterReturnId, customerId, installmentNo, entrydate, companyId, branchId, isSpecialDiscount, saleId);
                    }
                    else
                    {
                        var margeSalesObject = MargeSalesObjectWithExisting(customerId, aSale);
                        saleId = SaveNewSale(margeSalesObject, connectionAfterReturnId, customerId, installmentNo, entrydate, companyId, branchId, isSpecialDiscount, saleId);
                    }
                }
                else
                {
                    saleId = UpdateSaleInformation(aSale, connectionAfterReturnId, installmentNo, updatedate, isSpecialDiscount, saleId);
                }

                return saleId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }

        private Azolution.Entities.Sale.Sale MargeSalesObjectWithExisting(int customerId, Azolution.Entities.Sale.Sale aSale)
        {
            try
            {
                var exitingSalesInfo = GetExistingSalesInformation(customerId);
                if (exitingSalesInfo != null)
                {
                    aSale.Invoice = exitingSalesInfo.Invoice;
                    aSale.IIM = exitingSalesInfo.Installment;
                }
              
                return aSale;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public Azolution.Entities.Sale.Sale GetExistingSalesInformation(int customerId)
        {
            try
            {
                var query = string.Format(@"SELECT * from Sale where CustomerId={0} and IsActive=1 and  IsRelease=0 and (IIM=0 or IIM is NULL)", customerId);
                var data = Data<Azolution.Entities.Sale.Sale>.DataSource(query);
                return data.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception("Error! While searching exiting Sales information");
            }
        }

        private static int UpdateSaleInformation(Azolution.Entities.Sale.Sale aSale, CommonConnection connectionAfterReturnId, int installmentNo,
            string updatedate, int isSpecialDiscount, int saleId)
        {
            var updateQuery =
                string.Format(
                    @"UPDATE [Sale] SET [Invoice] = '{0}',[ModelId] ={1} ,[Price] ={2} ,[DownPay] = {3},[NetPrice]={4},[Installment] = {5}" +
                    ",[WarrantyPeriod] = {6},[SaleTypeId] = {7},[ChallanNo] = '{8}',[FirstPayDate] = '{9}',[WarrantyStartDate] = '{10}'" +
                    ",[Updated] = '{11}',[UpdatedBy] ={12}, [CustomerId] ={13} ,[IsActive] ={14},SalesRepId='{15}',IsSpecialDiscount={16},State={17},TempState={18} WHERE [SaleId]={19} ",
                    aSale.Invoice, aSale.AProduct.ModelId, aSale.Price, aSale.DownPay, aSale.NetPrice,
                    installmentNo, aSale.WarrantyPeriod,
                    aSale.SaleTypeId, aSale.ChallanNo, aSale.FirstPayDate, aSale.WarrantyStartDate,
                    updatedate, aSale.SaleUserId, aSale.ACustomer.CustomerId, aSale.IsActive, aSale.SalesRepId,
                    isSpecialDiscount,aSale.State,aSale.TempState, aSale.SaleId);
            connectionAfterReturnId.ExecuteNonQuery(updateQuery);
            saleId = aSale.SaleId;
            return saleId;
        }

        private static int SaveNewSale(Azolution.Entities.Sale.Sale aSale, CommonConnection connectionAfterReturnId, int customerId, int installmentNo,
            string entrydate, int companyId, int branchId, int isSpecialDiscount, int saleId)
        {
            try
            {
                var insertQuery =
                    string.Format(@"INSERT INTO Sale(Invoice,ModelId,Price,DownPay,NetPrice,Installment,SaleTypeId,ChallanNo
                        ,FirstPayDate,WarrantyStartDate,EntryDate,CustomerId,SaleUserId,Flag,IsActive,CompanyId,BranchId,SalesRepId,ParentSaleId,IsSmsSale,state,TempState,IsSpecialDiscount,IIM,UnRecognizeType,SaleDate) 
                        VALUES('{0}',{1},{2},{3},{4},{5},{6},'{7}','{8}','{9}','{10}','{11}',{12},{13},{14},{15},{16},'{17}',{18},{19},{20},{21},{22},{23},{24},'{25}')",
                        aSale.Invoice, aSale.AProduct.ModelId, aSale.Price, aSale.DownPay, aSale.NetPrice,
                        installmentNo, aSale.SaleTypeId,
                        aSale.ChallanNo, aSale.FirstPayDate, aSale.WarrantyStartDate, entrydate,
                        customerId, aSale.SaleUserId, aSale.Flag, aSale.IsActive, companyId, branchId, aSale.SalesRepId,
                        aSale.ParentSaleId, aSale.IsSmsSale, aSale.State,aSale.TempState, isSpecialDiscount, aSale.IIM,aSale.TypeOfUnRecognized,aSale.WarrantyStartDate);
                saleId = connectionAfterReturnId.ExecuteAfterReturnId(insertQuery, "SaleId");
                return saleId;
            }
            catch (Exception e)
            {
                throw new Exception("Error during Saving New Sales Information");
            }
        }


        private void SaveSalesProductItem(int saleId, List<SalesItem> objItemInfoList, List<SalesItemInformation> objItemDetailsInfoList, CommonConnection connectionAfterReturnId)
        {
            string sql = "";
            try
            {
                if (saleId != 0)
                {
                    if (objItemInfoList.Any(s => s.SalesItemId == 0))
                    {
                        DeletePreviousSaleItemIfExist(saleId, connectionAfterReturnId);
                    }

                    if (objItemInfoList != null)
                    {
                        foreach (var salesItem in objItemInfoList)
                        {
                            var salesItemId = 0;

                            if (salesItem.SalesItemId == 0)
                            {

                                sql = string.Format(
                                    @" Insert Into SalesItem(SaleId,ItemId,ItemPrice,ItemQuantity) Values({0},{1},{2},{3})",
                                    saleId, salesItem.ItemId, salesItem.Price, salesItem.SalesQty);
                                salesItemId = connectionAfterReturnId.ExecuteAfterReturnId(sql, "SalesItemId");

                            }
                            else
                            {
                                sql =
                               string.Format(
                                   @" Update SalesItem Set ItemPrice={0},ItemQuantity={1} Where SalesItemId={2}",
                                    salesItem.Price, salesItem.SalesQty, salesItem.SalesItemId);
                                connectionAfterReturnId.ExecuteNonQuery(sql);

                                salesItemId = salesItem.SalesItemId;

                            }

                            if (salesItemId > 0)
                            {
                                SaveSalesItemDetailsInfo(salesItem.ItemId, salesItemId, objItemDetailsInfoList, connectionAfterReturnId);

                            }

                        }
                    }
                }


            }
            catch (Exception exception)
            {

                throw new Exception("Failed to save Item Information");
            }



        }

        private void DeletePreviousSaleItemIfExist(int saleId, CommonConnection connection)
        {
            string sql = "";
            StringBuilder qBuilder = new StringBuilder();
            sql = string.Format(@"Select * From SalesItem Where SaleId={0}", saleId);
            var itemData = Data<SalesItem>.DataSource(sql);
            if (itemData.Any())
            {
                foreach (var salesItem in itemData)
                {
                    qBuilder.Append(string.Format(@" Delete From SalesItemDetails Where SalesItemId={0}", salesItem.SalesItemId));

                }

                if (qBuilder.ToString() != "")
                {
                    sql = "Begin " + qBuilder + " End;";
                    connection.ExecuteNonQuery(sql);
                }

                sql = string.Format(@"Delete From SalesItem Where SaleId={0}", saleId);
                connection.ExecuteNonQuery(sql);

            }

        }

        private void SaveSalesItemDetailsInfo(int itemId, int salesItemId, List<SalesItemInformation> objItemDetailsInfoList, CommonConnection connection)
        {
            string sql = "";
            StringBuilder qBuilder = new StringBuilder();
            if (objItemDetailsInfoList != null)
            {
                foreach (var sItemInfo in objItemDetailsInfoList)
                {
                    var itemInfo = sItemInfo.ItemDetails;

                    if (sItemInfo.ItemId == itemId)
                    {
                        var id = sItemInfo.ItemId;

                        qBuilder.Append(string.Format(@"Delete From SalesItemDetails Where SalesItemId={0};", salesItemId));

                        if (itemInfo != null)
                        {
                            foreach (var item in itemInfo)
                            {
                                //var mfd = item.ItemManufactureDate == DateTime.MinValue
                                //    ? null
                                //    : item.ItemManufactureDate.ToString("MM/dd/yyyy");

                                qBuilder.Append(string.Format(@" Insert Into SalesItemDetails(SalesItemId,ItemId,ItemSLNo,ItemManufactureDate,ItemWarrantyPeriod) Values({0},{1},'{2}','{3}','{4}');", salesItemId, id, item.ItemSLNo, item.ItemManufactureDate, item.ItemWarrantyPeriod));

                            }
                        }


                        if (qBuilder.ToString() != "")
                        {
                            sql = "Begin " + qBuilder.ToString() + " End;";
                            connection.ExecuteNonQuery(sql);
                        }

                    }
                }
            }
        }

        public void UpdateStockBalance(Azolution.Entities.Sale.Sale aSale, List<SalesItem> objItemInfoList, Users saleUser, CommonConnection connection, List<SalesItem> previousSalesitems)
        {
            StockDataService _stockDataService = new StockDataService();
            try
            {
                var companyId = 0;
                var branchId = 0;
                if (saleUser.CompanyStock == 1)// update stock balance as company or branch wise
                {
                    companyId = saleUser.RootCompanyId;
                    branchId = 0;
                }
                else
                {
                    companyId = saleUser.ChangedCompanyId;
                    branchId = saleUser.ChangedBranchId;
                }

                //if (saleUser.ChangedCompanyId == 0)
                //{
                //    if (saleUser.CompanyType == "MotherCompany")
                //    {
                //        if (saleUser.CompanyStock == 1)
                //        {
                //            companyId = saleUser.CompanyId;
                //        }
                //        else
                //        {
                //            companyId = saleUser.CompanyId;
                //            branchId = saleUser.BranchId;
                //        }
                //    }
                //}
                //else
                //{
                //    if (saleUser.ChangedCompanyType == "MotherCompany")
                //    {
                //        if (saleUser.ChangedCompanyStock == 1)
                //        {
                //            companyId = saleUser.CompanyId;
                //        }
                //        else
                //        {
                //            companyId = saleUser.CompanyId;
                //            branchId = saleUser.BranchId;
                //        }
                //    }
                //    else
                //    {
                //        if (saleUser.ChangedCompanyStock == 1)
                //        {
                //            companyId = saleUser.RootCompanyId;
                //        }
                //        else
                //        {
                //            companyId = saleUser.ChangedCompanyId;
                //            branchId = saleUser.ChangedBranchId;
                //        }
                //    }
                //}



                if (objItemInfoList != null)
                {
                    var modelId = aSale.AProduct.ModelId;
                    string query = "";
                    StringBuilder qBuilder = new StringBuilder();
                    foreach (var itemInfo in objItemInfoList)
                    {
                        StockBalance stockbalance = _stockDataService.CheckExistStock(modelId, itemInfo.ItemId, companyId, branchId, 1);
                        if (stockbalance == null)
                        {
                            stockbalance = new StockBalance();
                        }

                        if (previousSalesitems != null && previousSalesitems.Count == 0)
                        {
                            //if (stockbalance != null)
                            //{
                            var newStockbalance = new StockBalance();
                            newStockbalance.ModelId = modelId;
                            newStockbalance.ItemId = itemInfo.ItemId;
                            newStockbalance.StockQuantity = stockbalance.StockBalanceQty;
                            newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty - itemInfo.SalesQty);
                            newStockbalance.Type = 4; // TypeId 4 for deduction/Sale

                            newStockbalance.EntryDate = DateTime.Now;
                            newStockbalance.EntryUserId = saleUser.UserId;
                            newStockbalance.CompanyId = companyId;
                            newStockbalance.BranchId = branchId;

                            SaveStockBalanceQuery(qBuilder, newStockbalance);

                            // }
                        }
                        else
                        {
                            //if (stockbalance != null)
                            // {
                            var newStockbalance = new StockBalance();
                            newStockbalance.ModelId = modelId;
                            newStockbalance.ItemId = itemInfo.ItemId;
                            newStockbalance.StockQuantity = stockbalance.StockBalanceQty;

                            newStockbalance.CompanyId = companyId;
                            newStockbalance.BranchId = branchId;

                            newStockbalance.EntryDate = DateTime.Now;
                            newStockbalance.EntryUserId = saleUser.UserId;

                            foreach (var preSalesitem in previousSalesitems)
                            {
                                if (preSalesitem.ItemId == itemInfo.ItemId)
                                {
                                    if (itemInfo.SalesQty > preSalesitem.ItemQuantity)
                                    {
                                        newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty -
                                                                           (itemInfo.SalesQty - preSalesitem.ItemQuantity));

                                        newStockbalance.Type = 4; // TypeId 4 for deduction/Sale
                                        SaveStockBalanceQuery(qBuilder, newStockbalance);
                                        //                                            

                                    }
                                    else if (itemInfo.SalesQty < preSalesitem.ItemQuantity)
                                    {
                                        newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty +
                                                                           (preSalesitem.ItemQuantity -
                                                                            itemInfo.SalesQty));

                                        newStockbalance.Type = 6; // TypeId 6 for add/Sales update
                                        SaveStockBalanceQuery(qBuilder, newStockbalance);

                                    }
                                    else
                                    {
                                        //nothing to do
                                    }


                                }
                            }




                            //  }
                        }

                    }

                    if (qBuilder.ToString() != "")
                    {
                        query = "Begin " + qBuilder.ToString() + " End;";
                        connection.ExecuteNonQuery(query);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Stock Update Failed.");
            }

        }

        public void SaveStockBalanceQuery(StringBuilder qBuilder, StockBalance newStockbalance)
        {
            qBuilder.Append(
                string.Format(@" Insert Into StockBalance(ModelId,ItemId,StockQuantity,StockBalanceQty,EntryDate,[Type],EntryUserId,CompanyId,BranchId) 
                                                    Values({0},{1},{2},{3},'{4}',{5},{6},{7},{8});", newStockbalance.ModelId,
                    newStockbalance.ItemId, newStockbalance.StockQuantity,
                    newStockbalance.StockBalanceQty,
                    newStockbalance.EntryDate, newStockbalance.Type, newStockbalance.EntryUserId, newStockbalance.CompanyId, newStockbalance.BranchId));
        }


        public void SaveLicense(Azolution.Entities.Sale.Sale aSale, CommonConnection connection, string smsText)
        {
            try
            {
                var query = "";
                int IsSmsSent = 0;

                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");
                var updatedate = DateTime.Now.ToString("dd-MMM-yyyy");
                // DateTime issueDate = Convert.ToDateTime(aSale.ALicense.IssueDate);
                var issueDate = aSale.ALicense.IssueDate.ToString("dd-MMM-yyyy");

                //if (aSale.SaleTypeId==2)
                //{
                //    query = string.Format("INSERT INTO SMSSent(SMSText,MobileNumber,[RequestDateTime],[Status],ReplyFor) VALUES('{0}','{1}','{2}',{3},{4})",
                //        smsText, aSale.ACustomer.Phone2, issueDate, 0, 0);
                //    connection.ExecuteNonQuery(query);
                //}


                //if (aSale.SaleTypeId == 1)
                //{
                //    IsSmsSent = 0; //is already sent a sms
                //}
                //else
                //{
                //    IsSmsSent = 1;
                //}

                IsSmsSent = 0;
                query = string.Format(@"INSERT INTO Sale_License([Number],[LType],[IssueDate],[EntryDate] ,[Updated],[ModelId],[SaleInvoice],[ProductNo],[Flag],[IsActive],IsSentSMS)
                                        VALUES('{0}',{1},'{2}','{3}','{4}',{5},'{6}','{7}',{8},{9},{10})",
                        aSale.ALicense.Number, aSale.ALicense.LType, aSale.ALicense.IssueDate, entrydate, "",
                        aSale.AProduct.ModelId, aSale.Invoice, aSale.AProduct.ProductNo, 0, 1, IsSmsSent);
                connection.ExecuteNonQuery(query);

            }
            catch (Exception)
            {
                throw new Exception("Failed to save Lisence Info.");
            }

        }

        private static string GetInvoice(string invoice)
        {
            if (string.IsNullOrEmpty(invoice)) return "";
            var lastinvoice = Regex.Split(invoice, @"\D+").Last().Trim();
            var date = DateTime.Now;
            var month = date.ToString("MM");
            var year = date.ToString("yy");
            return month + "-" + year + "-" + lastinvoice;
        }

        public GridEntity<Azolution.Entities.Sale.Sale> GetSalesDetailsInfoByCustomerId(GridOptions options, int customerId)
        {
            string sql = string.Format(@" Select SC.CustomerCode,SC.CustomerId,S.SaleId,S.Invoice, SP.ModelId,SP.ProductName,SP.Model,SP.TotalPrice,
                    SL.Number,SL.LType,SL.IsActive As Status,SL.IssueDate,AT.Type,AlT.Type As ProductType
                    From Sale S
                    Left outer join Sale_Customer SC on SC.CustomerId=S.CustomerId
                    Left outer join Sale_Product SP on SP.ModelId = S.ModelId
                    Left outer join(Select LType,Number,IsActive,SaleInvoice,IssueDate From Sale_License Where IsActive=1 and IsSentSMS=1)SL  on SL.SaleInvoice = S.Invoice
                    left outer join Sale_AllType AT on AT.TypeId = SL.LType And AT.Flag='Lisence' and AT.IsActive=1
                    left outer join Sale_AllType AlT on AlT.TypeId = SP.TypeId And AlT.Flag='Product' and AlT.IsActive=1
                    Where SC.CustomerId={0}", customerId);
            var data = Kendo<Azolution.Entities.Sale.Sale>.Grid.GenericDataSource(options, sql, "CustomerId");
            return data;
        }

        public GridEntity<Installment> GetInstallmentInfoByModelId(GridOptions options, string invoice)
        {
            string sql = string.Format(@"SELECT * FROM Sale_Installment Where SInvoice ='{0}'", invoice);
            var data = Kendo<Installment>.Grid.GenericDataSource(options, sql, "InstallmentId");
            return data;
        }

        public GridEntity<Installment> GetAllInstallmentByInvoiceNo(GridOptions options, string invoiceNo)
        {
            string sql = string.Format(@"Select SI.InstallmentId,SI.SInvoice, SS.Number, SS.DueDate, SS.Status, SI.Amount, 
                 SUM(ISNULL(SC.ReceiveAmount,0))ReceiveAmount ,(Max(SI.Amount)-SUM(ISNULL(SC.ReceiveAmount,0))) DueAmount from Sale_Installment SI
                left join Sale_Collection SC on SC.InstallmentId=SI.InstallmentId
                left join (Select * from Sale_Installment I Where I.SInvoice='{0}') SS on SS.InstallmentId=SI.InstallmentId
                Where SI.SInvoice='{0}' 
                group by SI.InstallmentId, SS.Number, SS.DueDate,SS.Status,SI.Amount,SI.SInvoice", invoiceNo);

            var data = Kendo<Installment>.Grid.GenericDataSource(options, sql, "InstallmentId");
            return data;
        }

        public GridEntity<SaleSummary> GetAllSaleByMonth(GridOptions options, string condition, Users user)
        {

            var query = string.Format(@"SELECT S.SaleId,S.Invoice,S.WarrantyStartDate, Convert(Datetime,S.WarrantyStartDate,111) EntryDate2,S.FirstPayDate,S.DownPay,S.SalesRepId,
                S.Price,S.Installment,S.ProductNo,S.State,S.TempState,
                 isnull(DPT.ReceiveAmount,0) TempReceiveAmount,
                isnull(tblColl.ReceiveAmount,0)ReceiveAmount,
                S.IIM, S.IsDownPayCollected, SC.ProductId,
                SC.CustomerCode, SC.Name,SC.Phone,SC.Phone2,SP.ProductName,SP.Model,SP.PackageType,S.SaleTypeId,S.CompanyId,Branch.BRANCHID,BranchCode,
                s.IsActive,BranchSmsMobileNumber,IsSmsEligible,(Select distinct IsLisenceRequired From Sale_Product_Items Where ModelId=S.ModelId and IsLisenceRequired=1)IsLisenceRequired,
                Discount.DiscountTypeCode,Discount.IsApprovedSpecialDiscount,SP.TypeId, S.ModelId, SP.ModelItemID
                FROM Sale S 
                LEFT JOIN Sale_Product SP ON SP.ModelId=S.ModelId 
                INNER JOIN Sale_Customer SC ON SC.CustomerId=S.CustomerId And SC.IsActive=1
                Left Outer join Branch on Branch.BRANCHID = SC.BranchId
                Left Outer join Discount on Discount.SaleId=S.SaleId
                Left outer join DownpamentCollection_tmp DPT on DPT.SaleId=S.SaleId
                left outer join (Select SaleInvoice,SUM(ReceiveAmount)ReceiveAmount From Sale_Collection
                group by SaleInvoice)tblColl on tblColl.SaleInvoice=S.Invoice
                Where State=4 {0}", condition);

            var data = Kendo<SaleSummary>.Grid.GenericDataSource(options, query, "S.EntryDate DESC");
            return data;
        }

        public string GetCompanyCode(int companyId)
        {
            try
            {
                string sql = string.Format("Select * from  Company C where C.CompanyId={0}", companyId);
                var data = Data<Company>.GenericDataSource(sql);
                var singleOrDefault = data.SingleOrDefault();
                if (singleOrDefault != null) return singleOrDefault.CompanyCode;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public GridEntity<SalesItem> GetSalesItemInfoBySaleId(GridOptions options, int saleId)
        {
//            string sql = string.Format(@"Select SalesItem.*,sp.ItemName,sp.ItemModel,sp.Price
//                    From SalesItem
//                    inner join Sale_Product_Items sp on sp.ItemId=SalesItem.ItemId
//                Where SalesItem.saleId={0}", saleId);

            string sql = string.Format(@"Select tbl1.*,tbl2.ItemSLNo From (Select SalesItem.*,sp.ItemName,sp.ItemModel,sp.Price
                From SalesItem
                inner join Sale_Product_Items sp on sp.ItemId=SalesItem.ItemId
                Where SalesItem.saleId={0})
                tbl1 
                left join (Select * From SalesItemDetails Where  ItemSLNo <>'')tbl2 on tbl1.SalesItemId=tbl2.SalesItemId
                ", saleId);
            var data = Kendo<SalesItem>.Grid.GenericDataSource(options, sql, "SalesItemId");
            return data;
        }

        public GridEntity<SalesItemInfoForGetData> GetSalesItemDataBySaleId(GridOptions options, int saleId)
        {

            string sql = string.Format(@"Select SalesItemId,SalesItem.SaleId,Sale_Product_Items.ItemName,Sale_Product_Items.ItemModel,Sale_Product_Items.Price,Sale_Product_Items.BundleQuantity,
                    SalesItem.ItemId,ItemPrice,ItemQuantity As SalesQty,Sale_Product_Items.IsLisenceRequired,Sale_Product_Items.IsPriceApplicable,Sale_Product.TotalPrice PackagePrice,Sale_Product.ModelId
                    From SalesItem
                    left outer join Sale_Product_Items on Sale_Product_Items.ItemId=SalesItem.ItemId
                    left outer join Sale on Sale.SaleId = SalesItem.SaleId
                    left outer join Sale_Product on Sale_Product.ModelId= Sale.ModelId
                    Where SalesItem.saleId={0}", saleId);
            var data = Kendo<SalesItemInfoForGetData>.Grid.GenericDataSource(options, sql, "SalesItemId");
            return data;
        }

        public List<SalesItemInformation> GetItemDetailsInformationBySaleId(int saleId)
        {
            //var saleItemsInfo = new SalesItemInformation();
            var saleItemsList = new List<SalesItemInformation>();
            string sql = string.Format(@"Select SalesItem.SalesItemId,SalesItem.ItemId,
            SalesItemDetails.ItemSLNo,
            SalesItemDetails.ItemWarrantyPeriod From SalesItem 
            Left outer join SalesItemDetails on SalesItemDetails.SalesItemId=SalesItem.SalesItemId
            Where SalesItem.SaleId={0}", saleId);

            var saleItems = Data<SalesItemInformation>.DataSource(@"Select SalesItem.SalesItemId,SalesItem.ItemId From SalesItem Where SalesItem.SaleId=" + saleId);

            var details = Data<SalesItemDetails>.DataSource(sql);
            foreach (var item in saleItems)
            {
                item.ItemDetails = details.Where(s => s.ItemId == item.ItemId).ToList();

            }



            return saleItems;
        }

        public bool CheckExistInvoice(string condition)
        {
            string sql = string.Format(@" Select * From Sale {0}", condition);
            var data = Data<Azolution.Entities.Sale.Sale>.DataSource(sql).SingleOrDefault();
            return data != null;
        }

        public int GetDefaultInstallmentNo(int companyId)
        {
            string sql = string.Format(@" Select DefaultInstallmentNo As Number from Sale_Interest where CompanyId = {0} and Status = 1", companyId);
            var data = Data<Installment>.DataSource(sql).SingleOrDefault();
            return data == null ? 0 : data.Number;
        }

        public Azolution.Entities.Sale.Collection GetDownPaymentCollectionInfoBySaleId(int saleId)
        {
            string sql = string.Format(@"Select DC.CollectionId,DC.SaleInvoice,DC.ReceiveAmount,DC.CollectionType,DC.PaymentType,DC.TransectionId,
                    DC.PayDate,DownPay From DownpamentCollection_tmp DC
                    left outer join Sale on Sale.SaleId = DC.SaleId
                    Where DC.SaleId={0}", saleId);
            var data = Data<Azolution.Entities.Sale.Collection>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public int SaveCustomerInfo(Customer objCustomerInfo, CommonConnection connection, Users saleUsers)
        {

            var companyId = saleUsers.ChangedCompanyId == 0 ? saleUsers.CompanyId : saleUsers.ChangedCompanyId;
            var branchId = saleUsers.ChangedBranchId == 0 ? saleUsers.BranchId : saleUsers.ChangedBranchId;

            var customerId = 0;
            string sql = "";
            var entryDate = DateTime.Now;
            if (objCustomerInfo.CustomerId == 0)
            {
                if (!IsCustomerExist(objCustomerInfo, saleUsers))
                {
                    sql = string.Format(
                        @"Insert Into Sale_Customer(Name,CustomerCode,NID,Phone,CompanyId,BranchId,IsActive,EntryDate,IsStaff,StaffId,Phone2,ReferenceId,ProductId) Values('{0}','{1}','{2}','{3}',{4},{5},{6},'{7}',{8},'{9}','{10}','{11}','{12}')",
                        objCustomerInfo.Name, objCustomerInfo.CustomerCode, objCustomerInfo.NId, objCustomerInfo.Phone, companyId, branchId, 1, entryDate, objCustomerInfo.IsStaff, objCustomerInfo.StaffId, objCustomerInfo.Phone2, objCustomerInfo.ReferenceId,objCustomerInfo.ProductId);
                    customerId = connection.ExecuteAfterReturnId(sql, "CustomerId");
                }
                else
                {
                    string condition = CreateConditionForExistCustomer(objCustomerInfo, saleUsers);
                    string query = string.Format(@"select * from Sale_Customer where {0}", condition);
                    var data = Data<Customer>.DataSource(query).FirstOrDefault();
                    if (data != null) customerId = data.CustomerId;
                    objCustomerInfo.IsCustomerForMultipeSale = true;

                }
            }
            else
            {
                sql = string.Format(
                       @"Update Sale_Customer Set Name='{0}',CustomerCode='{1}',NID='{2}',Phone='{3}',IsActive='{4}',UpdateDate='{5}',IsStaff={6},StaffId='{7}',Phone2='{8}',ReferenceId='{9}',ProductId = '{10}'  Where CustomerId={11}",
                       objCustomerInfo.Name, objCustomerInfo.CustomerCode, objCustomerInfo.NId, objCustomerInfo.Phone, 1, entryDate, objCustomerInfo.IsStaff, objCustomerInfo.StaffId, objCustomerInfo.Phone2, objCustomerInfo.ReferenceId,objCustomerInfo.ProductId, objCustomerInfo.CustomerId);
                connection.ExecuteNonQuery(sql);
                customerId = objCustomerInfo.CustomerId;

            }

            return customerId;
        }

        private bool IsCustomerExist(Customer objCustomerInfo, Users saleUser)
        {
            string condition = CreateConditionForExistCustomer(objCustomerInfo, saleUser);
            string query = string.Format(@"Select * from Sale_Customer where {0}", condition);
            return Data<Customer>.DataSource(query).Any();

        }

        private static string CreateConditionForExistCustomer(Customer objCustomerInfo, Users saleUser)
        {
            var companyId = saleUser.ChangedCompanyId == 0 ? saleUser.CompanyId : saleUser.ChangedCompanyId;
            var branchId = saleUser.ChangedBranchId == 0 ? saleUser.BranchId : saleUser.ChangedBranchId;
            string condition = "";
            if (!string.IsNullOrEmpty(objCustomerInfo.CustomerCode))
            {
                condition = "CustomerCode ='" + objCustomerInfo.CustomerCode + "'";
            }
            if (!string.IsNullOrEmpty(objCustomerInfo.Phone))
            {
                condition += " Or Phone ='" + objCustomerInfo.Phone + "'";
            }
            if (!string.IsNullOrEmpty(objCustomerInfo.NId))
            {
                condition += " Or NID ='" + objCustomerInfo.NId + "'";
            }
            if (condition != "")
            {
                condition = "(" + condition + ")";
            }
            if (companyId > 0 && branchId > 0)
            {
                condition += " And CompanyId=" + companyId + " And BranchId=" + branchId;
            }
            return condition;
        }

        public string SaveFinalSale(List<Azolution.Entities.Sale.Sale> objSalesObjList, int state, CommonConnection connection)
        {
            string rv = "";
            string query = "";
            //  var connection = new CommonConnection();
            var qBuilder = new StringBuilder();
            try
            {
                if (objSalesObjList != null)
                {
                    foreach (var sale in objSalesObjList)
                    {
                        qBuilder.Append(string.Format(@"Update Sale Set IsActive=1,State={0},TempState={1} Where SaleId={2};", state, sale.State, sale.SaleId));

                    }

                }

                if (qBuilder.ToString() != "")
                {
                    query = "Begin " + qBuilder + " End;";
                }

                connection.ExecuteNonQuery(query);
                rv = Operation.Success.ToString();


            }
            catch (Exception exception)
            {
                rv = exception.Message;
            }
          

            return rv;
        }

        public PaymentReceivedInfo GetDownPaymentTempDataBySaleId(int saleId, CommonConnection connection)
        {
            string sql = "";

            sql = string.Format(@"Select DC.*,SC.Phone,Branch.BranchSmsMobileNumber From DownpamentCollection_tmp DC
                Left outer join Sale on Sale.SaleId=DC.SaleId
                Left outer join Sale_Customer SC on SC.CustomerId = Sale.CustomerId
                Left outer join Branch on Branch.BRANCHID= Sale.BranchId
                Where DC.SaleId='{0}'", saleId);

            // var data = Data<PaymentReceivedInfo>.DataSource(sql);
            var data = connection.Data<PaymentReceivedInfo>(sql);

            return data.SingleOrDefault();
        }

        public void SaveInitialLicenseInfo(Azolution.Entities.Sale.Sale sale, Users user)
        {
            CommonConnection connection = new CommonConnection();
            var smsText = "Dear Customer,\nYour Initial License Code is : " + sale.ALicense.Number + "\nThanks,\nCustomer Service";
            try
            {
                SaveLicense(sale, connection, smsText);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Save License Info!");
            }
            finally
            {
                connection.Close();
            }

        }

        //Queryable Modified by Rubel on 21-07-2016 
        public Azolution.Entities.Sale.Sale GetCustomerAndSaleInfoByCustomerCode(string condition)
        {
//            string sql = string.Format(@"Select SC.CustomerCode,SC.CustomerId,SC.Name,SC.Phone,SC.CompanyId,SC.BranchId,
//                    Sale.SaleId,Sale.Invoice,Sale.WarrantyStartDate,Sale.ModelId
//                    From Sale_Customer SC
//                    Inner join Sale on Sale.CustomerId = SC.CustomerId 
//                    {0}", condition);

            string sql = string.Format(@"Select SC.CustomerCode,SC.CustomerId,SC.Name,SC.Phone,SC.Phone2,SC.CompanyId,SC.BranchId,Branch.IsSmsEligible,BranchCode,
                    Sale.SaleId,Sale.Invoice,Sale.WarrantyStartDate,Sale.ModelId,Sale.SaleTypeId,Sale.State,
                    Sale.TempState,Sale.SalesRepId,Sale.Installment,Sale.Price,Sale.NetPrice,Sale.DownPay,
                    Sale.FirstPayDate,
                     Sale_Product.DefaultInstallmentNo,DownPayPercent,PackageType,TypeId,ModelItemID, Convert(Datetime,Sale.WarrantyStartDate,111) EntryDate2,
              Sale.ProductNo,
                 isnull(DPT.ReceiveAmount,0) TempReceiveAmount,
                isnull(tblColl.ReceiveAmount,0)ReceiveAmount,
                Sale.IIM, Sale.IsDownPayCollected, 
                SC.CustomerCode, SC.Name,SC.Phone,SC.Phone2,Sale_Product.ProductName,Sale_Product.Model,Sale.CompanyId,Branch.BRANCHID,BranchCode,
                sale.IsActive,BranchSmsMobileNumber,IsSmsEligible,(Select distinct IsLisenceRequired From Sale_Product_Items Where ModelId=Sale.ModelId and IsLisenceRequired=1)IsLisenceRequired,
                Discount.DiscountTypeCode,Discount.IsApprovedSpecialDiscount,Sale_Product.TypeId, Sale_Product.ModelId,SC.ProductId
                    From Sale_Customer SC
                    Inner join Sale on Sale.CustomerId = SC.CustomerId 
                    inner join Sale_Product on Sale_Product.ModelId = Sale.ModelId
                    Left join Branch on Branch.BRANCHID = Sale.BranchId
                 Left Outer join Discount on Discount.SaleId=Sale.SaleId
                Left outer join DownpamentCollection_tmp DPT on DPT.SaleId=Sale.SaleId
                left outer join (Select SaleInvoice,SUM(ReceiveAmount)ReceiveAmount From Sale_Collection
                group by SaleInvoice)tblColl on tblColl.SaleInvoice=Sale.Invoice
                    {0}", condition);

            var data = Data<Azolution.Entities.Sale.Sale>.GenericDataSource(sql).SingleOrDefault();
            return data;
        }

        public bool IsDuplicateInvoiceNo(string invoice)
        {
            string sql = string.Format(@" Select * From Sale Where Invoice= '{0}'", invoice);
            var data = Data<Azolution.Entities.Sale.Sale>.DataSource(sql).SingleOrDefault();
            return data != null;
        }

        public bool SaveAsUnRecognized(int saleId, int state, int tempState, int typeOfUnrecognized, string comments)
        {

            CommonConnection connection = new CommonConnection();
            try
            {
                string sql = string.Format(@" Update Sale Set State={0}, TempState={1},UnRecognizeType={2}, Comments='{3}' Where Saleid={4}", state, tempState, typeOfUnrecognized,comments, saleId);
                connection.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error! During Sale make Unrecognized");
            }
            finally
            {
                connection.Close();
            }

        }

        public GridEntity<SaleSummary> GetAllBookedSale(GridOptions options, string condition, Users user)
        {
            var query = string.Format(@"SELECT S.SaleId,S.Invoice,S.WarrantyStartDate,S.FirstPayDate,S.DownPay,S.SalesRepId,S.Comments,SC.ProductId,Sale_AllType.[Type],
                S.Price,S.NetPrice,S.Installment,S.ProductNo,S.State,S.TempState,isnull(DPT.ReceiveAmount,0) ReceiveAmount,
                isnull(DPT.ReceiveAmount,0) TempReceiveAmount, 
                SC.CustomerCode,SC.CustomerId, SC.Name,SC.Phone,SC.Phone2,SC.ReferenceId,SC.IsUpgraded,SP.ProductName,SP.Model,SP.PackageType,S.SaleTypeId,S.CompanyId,Branch.BRANCHID,BranchCode,
                s.IsActive,BranchSmsMobileNumber,(Select distinct IsLisenceRequired From Sale_Product_Items Where ModelId=S.ModelId and IsLisenceRequired=1)IsLisenceRequired,
                Discount.DiscountTypeCode,Discount.IsApprovedSpecialDiscount,S.EntryDate,SP.TypeId,S.ModelId,SP.ModelItemID
                FROM Sale S 
                LEFT JOIN Sale_Product SP ON SP.ModelId=S.ModelId 
                INNER JOIN Sale_Customer SC ON SC.CustomerId=S.CustomerId And SC.IsActive=1
                Left Outer join Branch on Branch.BRANCHID = SC.BranchId
                Left Outer join Discount on Discount.SaleId=S.SaleId
                Left outer join DownpamentCollection_tmp DPT on DPT.SaleId=S.SaleId
                left outer join Sale_AllType on Sale_AllType.TypeId = SP.TypeId and Sale_AllType.Code = '00' and Sale_AllType.IsActive = 1
                Where S.IsActive=0 And S.State=2 {0}", condition);

            var data = Kendo<SaleSummary>.Grid.GenericDataSource(options, query, "EntryDate DESC");
            return data;
        }

        public GridEntity<SaleSummary> GetAllUnrecognizedSale(GridOptions options, string condition, Users user)
        {
            var query = string.Format(@"SELECT S.SaleId,S.Invoice,S.WarrantyStartDate,S.FirstPayDate,S.DownPay,S.SalesRepId,S.Comments,
                S.Price,S.Installment,S.ProductNo,S.State,S.TempState,isnull(DPT.ReceiveAmount,0) ReceiveAmount,  DPT.TransectionId,DPT.PayDate,  
                SC.CustomerCode, SC.Name,SC.Phone,SP.ProductName,SP.Model,SP.ModelId,S.SaleTypeId,S.CompanyId,Branch.BRANCHID,BranchCode,
                s.IsActive,BranchSmsMobileNumber,(Select distinct IsLisenceRequired From Sale_Product_Items Where ModelId=S.ModelId and IsLisenceRequired=1)IsLisenceRequired,
                Discount.DiscountTypeCode,Discount.IsApprovedSpecialDiscount,S.IsSpecialDiscount,S.EntryDate,S.UnRecognizeType As TypeOfUnRecognized
                FROM Sale S 
                LEFT JOIN Sale_Product SP ON SP.ModelId=S.ModelId 
                INNER JOIN Sale_Customer SC ON SC.CustomerId=S.CustomerId and SC.IsActive=1
                Left Outer join Branch on Branch.BRANCHID = SC.BranchId
                Left Outer join Discount on Discount.SaleId=S.SaleId
                Left outer join DownpamentCollection_tmp DPT on DPT.SaleId=S.SaleId
                Where S.IsActive=0 And S.State=3  and UnRecognizeType <>1 {0}", condition);//and Discount.DiscountTypeCode <> '02' 

            var data = Kendo<SaleSummary>.Grid.GenericDataSource(options, query, "EntryDate DESC");
            return data;
        }

        public string SaveUnrecognizedSale(UnRecognizeSale unrecogSaleObj)
        {
            string sql = "";
            string rv = "";
            CommonConnection connection = new CommonConnection();
            connection.BeginTransaction();
            try
            {
                if (unrecogSaleObj.TypeOfUnRecognized == 2)
                {
                    UpdateDPInformation(unrecogSaleObj, connection);
                }

                UpdateSale(unrecogSaleObj, connection);
                connection.CommitTransaction();
                rv = Operation.Success.ToString();
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

        private void UpdateDPInformation(UnRecognizeSale unrecogSaleObj, CommonConnection connection)
        {
            decimal newDpAmount = unrecogSaleObj.NewCollectedAmount;
          //  decimal netPrice = unrecogSaleObj.Price - unrecogSaleObj.NewCollectedAmount;

            string sql = string.Format(@"Update DownpamentCollection_tmp Set ReceiveAmount={0} Where SaleId={1}", newDpAmount, unrecogSaleObj.SaleId);
            connection.ExecuteNonQuery(sql);
        }

        private static void UpdateSale(UnRecognizeSale unrecogSaleObj, CommonConnection connection)
        {
            var sql = "";

            var state = unrecogSaleObj.TempState;
            var tempstate = unrecogSaleObj.State;
            var typeOfUnrecognize = unrecogSaleObj.TypeOfUnRecognized;

            if (unrecogSaleObj.TypeOfUnRecognized == 2)
            {
                
                if (unrecogSaleObj.IsSpecialDiscount == 1 && unrecogSaleObj.IsApprovedSpecialDiscount==0)
                {
                    state = 3;
                    tempstate = 3;
                    typeOfUnrecognize = 1;
                }

                decimal newDpAmount = unrecogSaleObj.NewCollectedAmount;
                decimal netPrice = unrecogSaleObj.Price - unrecogSaleObj.NewCollectedAmount;

                sql = string.Format(@"Update Sale Set State={0}, TempState={1},DownPay={2},NetPrice={3},UnRecognizeType={4} Where SaleId={5}",
                    state, tempstate, newDpAmount, netPrice,typeOfUnrecognize, unrecogSaleObj.SaleId);
            }
           
            else
            {
                if (unrecogSaleObj.IsSpecialDiscount == 1)
                {
                    state = 3;
                    tempstate = 3;
                    typeOfUnrecognize = 1;

                }
                sql = string.Format(@"Update Sale Set State={0}, TempState={1},UnRecognizeType={2} Where SaleId={3}", state, tempstate,typeOfUnrecognize,unrecogSaleObj.SaleId);
            }
            connection.ExecuteNonQuery(sql);
        }

        public string SaveSaleAsDraft(Azolution.Entities.Sale.Sale sale, CommonConnection connection)
        {
            var rv = "";
            try
            {
                if (sale != null)
                {
                    var quey = string.Format(@"Update Sale Set State=4, TempState={0} Where SaleId={1};", sale.State, sale.SaleId);
                    connection.ExecuteNonQuery(quey);
                    rv = Operation.Success.ToString();
                }
                return rv;
            }
            catch (Exception exception)
            {
                rv = exception.Message;
            }
            return rv;
        }

        public bool CheckIsDPCollected(string invoice, int saleId)
        {
            var res = false;
            string sql = string.Empty;
            sql = string.Format(@"Select * From Sale Where SaleId={0} And IsDownPayCollected='True'", saleId);
            var data = Data<Azolution.Entities.Sale.Sale>.DataSource(sql).Any();

            return data;

        }

        public void GetInitialLisenceAndSendSms(Azolution.Entities.Sale.Sale sale, CommonConnection connection)
        {
            if (sale.IsRedCustomer != 1)
            {
                StringBuilder qBuilder = new StringBuilder();
                string sql = string.Empty;

                string licenseQuery =
                string.Format(@"Select * From Sale_License Where SaleInvoice='{0}' And LType=1 And IsSentSMS !=1 And IsActive=1", sale.Invoice);
                var licenseData = Data<License>.DataSource(licenseQuery).SingleOrDefault();


                //For Generating Sms Text   Process 1 (Bangla)
                SmsManager _smsManager = new SmsManager();
                LicenseInfo licenseInfo = new LicenseInfo();
                SalesRepresentatorDataService _representatorDatsService = new SalesRepresentatorDataService();
               
                licenseInfo.LType = 1;
                licenseInfo.CustomerName = sale.ACustomer.Name;
                licenseInfo.CustomerCode = sale.ACustomer.CustomerCode;
                licenseInfo.DownPay = Math.Round(sale.DownPay,2);
                if (licenseData != null)
                {
                    licenseInfo.Code = licenseData.Number;
                    string smsText = _smsManager.GetSmsText(licenseInfo);

                    PhoneSettingsService _phoneSettingsService = new PhoneSettingsService();
                    string simNumber = _phoneSettingsService.GetRandomPhoneNumber();

                    qBuilder.Append(string.Format(@"Update Sale_License Set IsActive=1, IsSentSMS=1 Where SaleInvoice='{0}' and IssueDate='{1}';", sale.Invoice, licenseData.IssueDate));//set active to present lisence

                    SaveSMS(sale.ACustomer.Phone2, qBuilder, smsText, simNumber);

                    if (sale.ACustomer.IsSmsEligible == 1)//for Sending SMS copy to BM
                    {
                        var generalSms= new GeneralSms();
                        generalSms.SmsType = 6;//BM SMS
                        generalSms.license =new LicenseInfo();
                        generalSms.license.LType = 1;
                        generalSms.license.Code = licenseData.Number;
                        generalSms.license.CustomerCode = sale.ACustomer.CustomerCode;
                        generalSms.license.DownPay = Math.Round(sale.ReceiveAmount,2);

                        smsText = _smsManager.GetGeneralSmsText(generalSms);

                        SaveSMS(sale.ACustomer.BranchSmsMobileNumber, qBuilder, smsText, simNumber);

                    }

                    //Dealer SMS Sent If eligible
                    if (sale.ACustomer.TypeId == 3 && sale.SalesRepId != null) //type = 3, Dealer Sale
                    {
                        var repSaleInfo = _representatorDatsService.GetAllSalesRepresentatorById(sale.SalesRepId);
                        if (repSaleInfo.IsSalesRepSmsSent == 1)
                        {
                            var generalSms = new GeneralSms();
                            generalSms.SmsType = 7;//Dealer SMS for Special Package (Initial license for Dealer), Type = 7
                            generalSms.license = new LicenseInfo();
                            generalSms.license.LType = 1;
                            generalSms.license.Code = licenseData.Number;
                            generalSms.license.CustomerCode = sale.ACustomer.CustomerCode;
                            generalSms.license.DownPay = Math.Round(sale.ReceiveAmount,2);

                            smsText = _smsManager.GetGeneralSmsText(generalSms);

                            SaveSMS(repSaleInfo.SalesRepSmsMobNo, qBuilder, smsText, simNumber);
                        }
                    }
                }

                if (qBuilder.ToString() != "")
                {
                    sql = "Begin " + qBuilder.ToString() + " End;";
                    connection.ExecuteNonQuery(sql);
                }
            }
        }

        private static void SaveSMS(string mobileNo, StringBuilder qBuilder, string smsText, string simNumber)
        {
            qBuilder.Append(
                string.Format(
                    "INSERT INTO SMSSent(SMSText,MobileNumber,[RequestDateTime],[Status],ReplyFor,SimNumber) VALUES(N'{0}','{1}','{2}',{3},{4},'{5}');",
                    smsText, mobileNo, DateTime.Now, 0, 0, simNumber));
        }

        public bool CheckIsCashPaymentCollected(string invoice, int saleId)
        {
            var res = false;
            string sql = string.Empty;
            sql = string.Format(@"Select * From Sale inner join Sale_Collection on Sale_Collection.SaleInvoice=Sale.Invoice Where SaleId={0}", saleId);
            var data = Data<Azolution.Entities.Sale.Sale>.DataSource(sql).Any();

            return data;
        }

        public void GetReleaseLisenceAndSendSms(Azolution.Entities.Sale.Sale sale, CommonConnection connection)
        {
            if (sale.IsRedCustomer != 1)
            {
                StringBuilder qBuilder = new StringBuilder();
                string sql = string.Empty;

                string licenseQuery =
             string.Format(@"Select * From Sale_License Where SaleInvoice='{0}' And LType=3 And IsSentSMS !=1 And IsActive=1", sale.Invoice);
                var licenseData = Data<License>.DataSource(licenseQuery).SingleOrDefault();

                qBuilder.Append(string.Format(@"Update Sale_License Set IsActive=1, IsSentSMS=1 Where SaleInvoice='{0}' and IssueDate='{1}';", sale.Invoice, licenseData.IssueDate));//set active to present lisence

                if (qBuilder.ToString() != "")
                {
                    sql = "Begin " + qBuilder.ToString() + " End;";
                    connection.ExecuteNonQuery(sql);
                }
            }
        }

        public Azolution.Entities.Sale.Collection GetRollbackDownpayData(string invoice)
        {
            string sql = string.Format(@"Select * From RollBackCollection_Temp Where SaleInvoice='{0}'",invoice);
            var data = Data<Azolution.Entities.Sale.Collection>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public void DeleteRollbackCollection_Temp(string invoice, CommonConnection connection)
        {
            try
            {
                string sql = string.Format(@"Delete From RollBackCollection_Temp Where SaleInvoice ='{0}'",invoice);
                connection.ExecuteNonQuery(sql);
            }
            catch (Exception e)
            {
                throw new Exception("Error! While Delete Rollback Collection Temp Data");
            }
        }

        public SalesItemDetails GetItemDetailsByInvoiceNo(string  invoice, string itemCode)
        {
            string sql = string.Format(@"Select SPI.ModelId,SPI.ItemId,SPI.ItemName,IsPriceApplicable,ItemCode,ItemSLNo From Sale_Product_Items SPI
            inner join SalesItem on SalesItem.ItemId=SPI.ItemId
            left join SalesItemDetails on SalesItem.SalesItemId=SalesItemDetails.SalesItemId
            inner join Sale on Sale.SaleId=SalesItem.SaleId
            Where Sale.Invoice='{0}' And ItemCode='{1}'", invoice, itemCode);
            var data = Data<SalesItemDetails>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public int GetPackageWiseDefaultInstallmentNoByModelId(int modelId)
        {
            string sql = string.Format(@" Select DefaultInstallmentNo As Number from Sale_Product where ModelId = {0} and IsActive = 1", modelId);
            var data = Data<Installment>.DataSource(sql).SingleOrDefault();
            return data == null ? 0 : data.Number;
        }

  
    }
}
