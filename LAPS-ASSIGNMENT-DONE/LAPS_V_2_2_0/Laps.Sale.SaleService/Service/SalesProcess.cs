using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using DataService.DataService;
using Laps.AdminSettings.Service.Service;
using Laps.Collection.CollectionDataService.DataService;
using Laps.Collection.CollectionService.Interface;
using Laps.Collection.CollectionService.Service;
using Laps.Product.DataService.DataService;
using Laps.Product.Service.Service;
using Laps.Sale.SaleService.Interface;
using Laps.Stock.DataService.DataService;
using Laps.Stock.Service.Service;
using LapsUtility;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Solaric.GenerateCode;
using Utilities;

namespace Laps.Sale.SaleService.Service
{
    public class SalesProcess : ISalesProcess
    {
        private readonly SaleDataService.DataService.SaleDataService _saleDataService;
        private readonly CollectionService _collectionService;
        private readonly SaleService _saleService;

        public SalesProcess()
        {
            _saleDataService = new SaleDataService.DataService.SaleDataService();
            _collectionService = new CollectionService();
            _saleService = new SaleService();
        }
        public string SaveAsBooked(SalesParam salesParam, bool iSAtumated)
        {
            CheckIsUnRecognized(salesParam);
            return Booked(salesParam);

        }


        private void CheckIsUnRecognized(SalesParam salesParam)
        {
            if (salesParam.DiscountInfo.DiscountTypeCode == "02" && salesParam.DiscountInfo.IsApprovedSpecialDiscount == 0)
            {
                salesParam.SaleInfo.State = 3;
                salesParam.SaleInfo.TempState = 2;
                salesParam.SaleInfo.IsSpecialDiscount = 1;
                salesParam.SaleInfo.TypeOfUnRecognized = 1;
            }
            else if (salesParam.DiscountInfo.DiscountTypeCode == "02" && salesParam.DiscountInfo.IsApprovedSpecialDiscount == 1)
            {
                salesParam.SaleInfo.State = 2;
                salesParam.SaleInfo.TempState = 3;
                salesParam.SaleInfo.IsSpecialDiscount = 1;
                salesParam.SaleInfo.TypeOfUnRecognized = 1;
            }
            else
            {
                salesParam.SaleInfo.State = 2;
                salesParam.SaleInfo.TempState = 0;
                salesParam.SaleInfo.IsSpecialDiscount = 0;

            }

            if (salesParam.DownPayCollection.ReceiveAmount < salesParam.SaleInfo.DownPay || salesParam.DownPayCollection.ReceiveAmount > salesParam.SaleInfo.DownPay)
            {
                salesParam.SaleInfo.State = 3;
                salesParam.SaleInfo.TempState = 2;
                salesParam.SaleInfo.TypeOfUnRecognized = 2;
            }
        }
        private string Booked(SalesParam salesParam)
        {
            if (!CheckDuplicateInvoiceNo(salesParam.SaleInfo))
            {
                return _saleDataService.SaveSale(salesParam, 2);
            }
            else
            {
                return Operation.Exists.ToString();
            }

        }


        public string Sold(List<Azolution.Entities.Sale.Sale> salesList, Users user)
        {


            string rv = "";
            string rvalue = "";
            CommonConnection connection = new CommonConnection();
            var connectionAfterReturnId = new CommonConnection(IsolationLevel.ReadCommitted);

            connection.BeginTransaction();
            connectionAfterReturnId.BeginTransaction();

            try
            {
                DefaultSettingsService _defaultSettingsService  = new DefaultSettingsService();
                StockService _stockService = new StockService();
                ProductService _productService = new ProductService();

                var newSaleObjList = new List<Azolution.Entities.Sale.Sale>();
                var downpayDueSaleList = new List<Azolution.Entities.Sale.Sale>();
                foreach (var sale in salesList)
                {
                    var res = CheckIsDPCollected(sale.Invoice, sale.SaleId);
                    if (res)
                    {
                        newSaleObjList.Add(sale);
                    }
                    else
                    {
                        downpayDueSaleList.Add(sale);

                        // throw new Exception("Down payment is not collected for invoice:" + sale.Invoice);
                    }

                }

                if (newSaleObjList.Any())
                {

                    GetLicenseAndSendSms(newSaleObjList, connection);
                    _saleDataService.SaveFinalSale(newSaleObjList, SaleStates.State.Sold, connection);


                    var modeOperation = _defaultSettingsService.GetOperationModeSettings();  //New Code For Reduce Stock Branch Type = 1,2,4 (Retail Sale)
                    if (modeOperation.ManualInventoryChecking == 1)
                    {  
                                //reduce Inventory; ReduceStock Branch
                        _stockService.ReduceStockInventoryForBranchbyBranchIdAndModelId(newSaleObjList,user,connection);
                        _stockService.ReduceStockInventoryForBranchbyBranchIdAndModelIdFromRemoteMySql(newSaleObjList,user);

                    }


                    foreach (var saleObj in newSaleObjList) //Stock Balance Reduce
                    {
                        if (saleObj.AProduct.TypeId != 3)
                        {
                            List<SalesItem> previousSalesitems = _saleDataService.GetPreviousSalesItem(saleObj.SaleId);
                            List<SalesItem> itemInfoList = _productService.GetProductItemInfoBySaleId(saleObj.SaleId);
                            _saleDataService.UpdateStockBalance(saleObj, itemInfoList, user, connectionAfterReturnId, previousSalesitems);
                        }
                    }

                    connection.CommitTransaction();
                    connectionAfterReturnId.CommitTransaction();

                    rv = Operation.Success.ToString();
                }
                if (downpayDueSaleList.Any())
                {
                    rvalue = "DPNotFound";
                }


            }
            catch (Exception exception)
            {
                connection.RollBack();
                connectionAfterReturnId.RollBack();
                rv = exception.Message;
            }
            finally
            {
                connection.Close();
                connectionAfterReturnId.Close();
            }


            return rv + rvalue;

        }

        private bool CheckIsCashPaymentCollected(string invoice, int saleId)
        {
            return _saleDataService.CheckIsCashPaymentCollected(invoice, saleId);
        }

        private void GetLicenseAndSendSms(List<Azolution.Entities.Sale.Sale> newSaleObjList, CommonConnection connection)
        {
            CustomerDataService _customerDataService = new CustomerDataService();
            CollectionDataService _collectionDataService = new CollectionDataService();

            if (newSaleObjList.Any())
            {
                foreach (var sale in newSaleObjList)
                {
                    //if (sale.SaleTypeId == 1)
                    //{

                    if (sale.AProduct.PackageType == 1)
                    {
                        var totalReceivedAmt = _collectionDataService.GetTotalReceivedAmount(sale.Invoice);
                        var duePercent = _collectionDataService.GetDuePercentByInvoiceNo(sale.Invoice);
                        List<Due> customerRating = _customerDataService.GetCustomerRatingByCompanyId(sale.ACustomer.CompanyId);

                        if (customerRating.Any())
                            foreach (var due in customerRating)
                            {
                                var duePercentage = Convert.ToDecimal(duePercent.TotalDuePercentTillDate);

                                if (Convert.ToDecimal(due.FromDue) <= duePercentage &&
                                    Convert.ToDecimal(due.ToDue) >= duePercentage)
                                {
                                    if (due.AAllType.TypeId != 4) //4 = red customer
                                    {
                                        sale.IsRedCustomer = 0; //Not Red Customer
                                        //sale.ALicense.IsSMSSent = 1;
                                    }
                                    else
                                    {
                                        sale.IsRedCustomer = 1; //Red Customer
                                        // sale.ALicense.IsSMSSent = 0;
                                    }
                                    sale.ReceiveAmount = totalReceivedAmt;
                                    _saleDataService.GetInitialLisenceAndSendSms(sale, connection);

                                }
                            }
                        // }
                        //else
                        //{
                        //    _saleDataService.GetReleaseLisenceAndSendSms(sale, connection);
                        //}
                    }
                }
            }
        }

        public string MakeUnRecognized(SaleSummary sale)
        {
            int typeOfUnrecognized = GetUnrecognizedType(sale);
            if (_saleDataService.SaveAsUnRecognized(sale.SaleId, 3, sale.State, typeOfUnrecognized, sale.Comments))
            {
                return Operation.Success.ToString();
            }
            return Operation.Failed.ToString();
        }

        public string Draft(List<Azolution.Entities.Sale.Sale> objSalesObjList, Users user)
        {
            var connection = new CommonConnection();
            string rv = string.Empty;
            string sql = string.Empty;

            try
            {
                connection.BeginTransaction();
                IPaymentSchedule paymentSchedule = new PaymentSchedule();
                foreach (var sale in objSalesObjList)
                {
                    paymentSchedule.MakePaymentSchedule(sale.Installment, sale.SaleId, sale.Invoice, sale.NetPrice, sale.SaleTypeId == 2 ? sale.WarrantyStartDate : sale.FirstPayDate, sale.SaleTypeId, connection);
                    rv = _saleDataService.SaveSaleAsDraft(sale, connection);

                    connection.CommitTransaction();

                    DownpaymentCollect(sale, user, connection);
                }

            }
            catch (Exception)
            {
                rv = Operation.Failed.ToString();
                connection.RollBack();
            }
            finally
            {
                connection.Close();
            }

            return rv;
        }

        private void DownpaymentCollect(Azolution.Entities.Sale.Sale sale, Users user, CommonConnection connection)
        {
            CollectionService _collectionService = new CollectionService();
            PaymentReceivedInfo paymentReceivedInfo = new PaymentReceivedInfo();
            var rollbackedDP = _saleDataService.GetRollbackDownpayData(sale.Invoice);
            if (rollbackedDP != null)
            {
                paymentReceivedInfo.ReceiveAmount = rollbackedDP.ReceiveAmount;
                paymentReceivedInfo.SaleInvoice = rollbackedDP.SaleInvoice;
                paymentReceivedInfo.Phone = sale.ACustomer.Phone;
                paymentReceivedInfo.Phone2 = sale.ACustomer.Phone2;
                paymentReceivedInfo.BranchSmsMobileNumber = sale.ACustomer.BranchSmsMobileNumber;
                paymentReceivedInfo.CustomerId = sale.ACustomer.CustomerId;
                paymentReceivedInfo.CustomerCode = sale.ACustomer.CustomerCode;
                paymentReceivedInfo.BranchCode = sale.ACustomer.BranchCode;
                paymentReceivedInfo.IsCustomerUpgraded = sale.ACustomer.IsUpgraded;
                paymentReceivedInfo.SaleTypeId = sale.SaleTypeId;
                paymentReceivedInfo.PayDate = DateTime.Now;
                paymentReceivedInfo.TransactionType = 1;
                _collectionService.GetPaymentAndCollect(paymentReceivedInfo, user.UserId);

                _saleDataService.DeleteRollbackCollection_Temp(sale.Invoice, connection);
            }
        }

        private bool CheckIsDPCollected(string invoice, int saleId)
        {
            return _saleDataService.CheckIsDPCollected(invoice, saleId);
        }


        public static int GetUnrecognizedType(SaleSummary sale)
        {
            if (sale.ADiscount.DiscountTypeCode == "02" && sale.ADiscount.IsApprovedSpecialDiscount == 0)
            {
                return 1;
            }
            else if (sale.DownPay > sale.TempReceiveAmount)
            {
                return 2;
            }
            else if (sale.ADiscount.DiscountTypeCode == "02" && sale.DownPay <= sale.TempReceiveAmount)
            {
                return 3;
            }
            else
            {
                return 4;
            }

        }

        private bool CheckDuplicateInvoiceNo(Azolution.Entities.Sale.Sale aSale)
        {
            string condition = "";
            if (aSale.SaleId == 0)
            {
                condition = " Where Invoice = '" + aSale.Invoice + "' And CustomerId !=" + aSale.ACustomer.CustomerId;
            }
            if (aSale.SaleId > 0)
            {
                condition = " Where Invoice = '" + aSale.Invoice + "' And SaleId != " + aSale.SaleId + " And CustomerId !=" + aSale.ACustomer.CustomerId;
            }

            var res = _saleDataService.CheckExistInvoice(condition);
            return res;
        }

        public string SaveSaleForSpecialPackage(SalesParam salesParam, bool iSAtumated)  //D-type
        {

            CompanyService _companyService = new CompanyService();
            ICollectionRepository collRepository = new CollectionService();
            ISaleRepository saleRepository = new SaleService();

            var res = "";
            var dpCollPayment = "";

            if (!CheckDuplicateInvoiceNo(salesParam.SaleInfo))
            {
                var bookStageAction = _saleDataService.SaveSale(salesParam, 2);
                if (bookStageAction == "Success")
                {
                    var getsaleInfo = saleRepository.GetCustomerAndSaleInfoByCustomerCode(salesParam.CustomerInfo.CustomerCode, null);
                    salesParam.SaleInfo.SaleId = getsaleInfo.SaleId;
                    var draftAction = DraftedForSpecialPackage(getsaleInfo, salesParam.User);  //Previous salesParam.SaleInfo getsaleInfo

                    if (draftAction == "Success")
                    {
                        var getsaleInfoForPay = saleRepository.GetCustomerAndSaleInfoByCustomerCode(salesParam.CustomerInfo.CustomerCode, null);
                        var companyBranchInfo = _companyService.GetCompanyInfoByBranchCode(salesParam.SaleInfo.SalesRepId.Substring(0, 4));
                        var getPayment = GetPaymentReceiveAmountForSpecialPackage(getsaleInfoForPay, salesParam.User, companyBranchInfo.Branch.BranchSmsMobileNumber);   //Previous salesParam.SaleInfo for  getsaleInfoForPay 

                        //DownpaymentCollect baki;
                        getPayment.ProductId = salesParam.CustomerInfo.ProductId;
                        getPayment.TypeId = getsaleInfoForPay.ACustomer.TypeId;
                        getPayment.SaleDate = getsaleInfoForPay.WarrantyStartDate;
                        dpCollPayment = collRepository.GetPaymentAndCollect(getPayment, salesParam.User.UserId);

                        if (dpCollPayment == "Success")
                        {
                            var getsaleInfoForFinalSubmit = saleRepository.GetCustomerAndSaleInfoByCustomerCode(salesParam.CustomerInfo.CustomerCode, null);
                            salesParam.SaleInfo.SaleId = getsaleInfo.SaleId;
                            salesParam.CustomerInfo.CompanyId = getsaleInfo.ACustomer.CompanyId;
                            salesParam.SaleInfo.AProduct.PackageType = getsaleInfo.AProduct.PackageType;
                            salesParam.SaleInfo.ACustomer.CompanyId = companyBranchInfo.CompanyId;

                            var finalsold = FinalSoldForSpecialPackage(getsaleInfoForFinalSubmit, salesParam);  //Previous salesParam.SaleInfo getsaleInfoForPay
                            if (finalsold == "Success")
                            {
                                res = "Success";
                            }
                            else
                            {
                                res = Operation.Failed.ToString();
                            }

                        }

                    }
                }
            }
            else
            {
                return Operation.Exists.ToString();
            }
            return res;
        }   //Special Sale for D-Type (Dealer Sale) Or R-Type (Retail Sale)

        public PaymentReceivedInfo GetPaymentReceiveAmountForSpecialPackage(Azolution.Entities.Sale.Sale sale, Users user, string branchSmsMobileNo)
        {
            CollectionService _collectionService = new CollectionService();
            PaymentReceivedInfo paymentReceivedInfo = new PaymentReceivedInfo();
            SaleService _saleService = new SaleService();
            ProductDataService _productDataService = new ProductDataService();
            var customerInfo = _productDataService.GetCustomerInfoByInvoiceNo(sale.Invoice);

            if (sale != null)
            {
                paymentReceivedInfo.ReceiveAmount = sale.DownPay;
                paymentReceivedInfo.SaleInvoice = sale.Invoice;
                paymentReceivedInfo.Phone = customerInfo.ACustomer.Phone;
                paymentReceivedInfo.Phone2 = customerInfo.ACustomer.Phone2;
                paymentReceivedInfo.BranchSmsMobileNumber = branchSmsMobileNo;
                paymentReceivedInfo.CustomerId = customerInfo.ACustomer.CustomerId;
                paymentReceivedInfo.CustomerCode = customerInfo.ACustomer.CustomerCode;
                paymentReceivedInfo.BranchCode = sale.SalesRepId.Substring(0, 4);
                paymentReceivedInfo.IsCustomerUpgraded = customerInfo.ACustomer.IsUpgraded;
                paymentReceivedInfo.SaleTypeId = sale.SaleTypeId;
                paymentReceivedInfo.PayDate = DateTime.Now;
                paymentReceivedInfo.TransactionType = 1;

            }
            return paymentReceivedInfo;
        }
        public string DraftedForSpecialPackage(Azolution.Entities.Sale.Sale objSalesObj, Users user)
        {
            var connection = new CommonConnection();
            string rv = string.Empty;
            string sql = string.Empty;

            try
            {
                connection.BeginTransaction();
                IPaymentSchedule paymentSchedule = new PaymentSchedule();
                paymentSchedule.MakePaymentSchedule(objSalesObj.Installment, objSalesObj.SaleId, objSalesObj.Invoice, objSalesObj.NetPrice, objSalesObj.SaleTypeId == 2 ? objSalesObj.WarrantyStartDate : objSalesObj.FirstPayDate, objSalesObj.SaleTypeId, connection);
                rv = _saleDataService.SaveSaleAsDraft(objSalesObj, connection);

                connection.CommitTransaction();

                DownpaymentCollect(objSalesObj, user, connection);

            }
            catch (Exception)
            {
                rv = Operation.Failed.ToString();
                connection.RollBack();
            }
            finally
            {
                connection.Close();
            }

            return rv;
        }

        public string FinalSoldForSpecialPackage(Azolution.Entities.Sale.Sale salesObjInfo, SalesParam salesParam)
        {


            string rv = "";
            string rvalue = "";
            CommonConnection connection = new CommonConnection();
            var connectionAfterReturnId = new CommonConnection(IsolationLevel.ReadCommitted);

            connection.BeginTransaction();
            connectionAfterReturnId.BeginTransaction();

            try
            {
                StockService _stockService = new StockService();
                DefaultSettingsService _defaultSettingsService = new DefaultSettingsService();
                ProductService _productService = new ProductService();

                var newSaleObjList = new List<Azolution.Entities.Sale.Sale>();
                var downpayDueSaleList = new List<Azolution.Entities.Sale.Sale>();

                var res = CheckIsDPCollected(salesObjInfo.Invoice, salesObjInfo.SaleId);
                if (res)
                {
                    newSaleObjList.Add(salesObjInfo);
                }
                else
                {
                    downpayDueSaleList.Add(salesObjInfo);

                    // throw new Exception("Down payment is not collected for invoice:" + sale.Invoice);
                }


                if (newSaleObjList.Any())
                {

                    GetLicenseAndSendSms(newSaleObjList, connection);
                    _saleDataService.SaveFinalSale(newSaleObjList, SaleStates.State.Sold, connection);
                    
                    var modeOperation = _defaultSettingsService.GetOperationModeSettings();  //New Code For Reduce Stock Branch Type = 4 (Retail Sale)
                    if (modeOperation.ManualInventoryChecking == 1)
                    {
                                //reduce Inventory; ReduceStock Branch
                        _stockService.ReduceStockInventoryForDealerbyDealerIdAndModelId(newSaleObjList,salesParam.User, connection);
                        _stockService.ReduceStockInventoryForDealerbyDealerIdAndModelIdFromRemoteMySql(newSaleObjList, salesParam.User);

                    }


                    foreach (var saleObj in newSaleObjList) //Stock Reduce
                    {
                        if (saleObj.AProduct.TypeId == 3)
                        {
                            List<SalesItem> previousSalesitems = _saleDataService.GetPreviousSalesItem(saleObj.SaleId);
                            List<SalesItem> itemInfoList = _productService.GetProductItemInfoBySaleId(saleObj.SaleId);
                            _saleDataService.UpdateStockBalance(saleObj, itemInfoList, salesParam.User, connectionAfterReturnId, previousSalesitems);
                        }
                    }


                    connection.CommitTransaction();
                    connectionAfterReturnId.CommitTransaction();
                    rv = Operation.Success.ToString();
                }
                if (downpayDueSaleList.Any())
                {
                    rvalue = "DPNotFound";
                }


            }
            catch (Exception exception)
            {
                connection.RollBack();
                connectionAfterReturnId.RollBack();
                rv = exception.Message;
            }
            finally
            {
                connection.Close();
                connectionAfterReturnId.Close();
            }

            return rv + rvalue;

        }
    }
}
