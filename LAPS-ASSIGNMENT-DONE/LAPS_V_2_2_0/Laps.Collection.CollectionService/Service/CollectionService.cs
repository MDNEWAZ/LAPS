using System;
using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Laps.AdminSettings.Service.Service;
using Laps.Collection.CollectionService.Interface;
using DataService.DataService;
using Laps.Product.DataService.DataService;
using Laps.Sale.SaleDataService.DataService;
using SmsService;
using Solaric.GenerateCode;
using Utilities;


namespace Laps.Collection.CollectionService.Service
{
    public class CollectionService : ICollectionRepository
    {
        CollectionDataService.DataService.CollectionDataService _collectionDataService = new CollectionDataService.DataService.CollectionDataService();
        public string GetPaymentAndCollect(PaymentReceivedInfo paymentInfoObj, int userId)
        {
            var connection = new CommonConnection();
            string res = "";
            connection.BeginTransaction();
            try
            {
                string initialCode = string.Empty;

                var installmentReceicveList = new List<Azolution.Entities.Sale.Collection>();
                var salesInvoiceNumber = paymentInfoObj.SaleInvoice;
                var nowReceiveAmount = paymentInfoObj.ReceiveAmount; // need to calculate for total due
                decimal remainingAmount = nowReceiveAmount;
                var transactionType = paymentInfoObj.TransactionType;

                var transactionId = paymentInfoObj.TransectionId;
                var customerMobileNo = paymentInfoObj.Phone;
                var customerSmsMobileNo = paymentInfoObj.Phone2;

                var branchMobileNo = paymentInfoObj.BranchSmsMobileNumber;
                decimal totalDue = 0;
                var payDate = paymentInfoObj.PayDate;

                var customerIsActive = _collectionDataService.CustomerIsActiveBySaleId(paymentInfoObj.SaleInvoice, connection);

                var duplicateTxnIdCount = _collectionDataService.CheckDuplicateTransactionId(transactionId ?? ""); //New Code for check duplicate

                if (duplicateTxnIdCount == 0)
                {
                    if (customerIsActive == 1)
                    {
                        //---------
                        string brCode = "";
                        string cusCode = "";
                        _collectionDataService.GetCompanyCodeAndBranchCodeFromItemSLNo(paymentInfoObj.SaleInvoice, ref brCode, ref cusCode);

                        remainingAmount = DownPaymentCollectionProcess(paymentInfoObj, salesInvoiceNumber, transactionType, transactionId, payDate, remainingAmount, installmentReceicveList, connection, ref initialCode);


                        remainingAmount = InstallmentCollectionProcess(remainingAmount, salesInvoiceNumber, connection, totalDue, transactionType, transactionId, payDate, customerMobileNo, customerSmsMobileNo, branchMobileNo, installmentReceicveList, paymentInfoObj.SaleTypeId);

                        if (remainingAmount > 1) //To handle somethings value like 0.40 
                        {
                            res = "Over";
                        }
                        else
                        {
                            res = _collectionDataService.CollectPaymentAndUpdatePaymentStatus(installmentReceicveList, connection, userId, initialCode, paymentInfoObj, brCode, cusCode);
                            connection.CommitTransaction();

                            if (res == "Success")
                            {
                                if (paymentInfoObj.SaleTypeId == 1)
                                {
                                    res = SendSmsByCustomerRating(paymentInfoObj.SaleInvoice, paymentInfoObj.SaleId, paymentInfoObj.ReceiveAmount, connection, null);
                                }

                            }
                        }
                    }
                    else
                    {
                        res = "Customer " + "(" + "Invoice Number: " + salesInvoiceNumber + ")" + " Is Inactive";
                    }

                }
                else
                {
                    res = transactionId + " Is duplicate Transaction ID. Transaction ID must be Unique.";
                }
            }
            catch (Exception exception)
            {
                res = exception.Message;
                connection.RollBack();
            }
            finally
            {
                connection.Close();
            }
            return res;
        }

        private decimal InstallmentCollectionProcess(decimal remainingAmount, string salesInvoiceNumber, CommonConnection connection, decimal totalDue, int transactionType, string transactionId, DateTime payDate, string customerMobileNo, string customerSmsMobileNo, string branchMobileNo, List<Azolution.Entities.Sale.Collection> installmentReceicveList, int saleTypeId)
        {
            //for Installment collection

            if (remainingAmount > 0)
            {
                var data = _collectionDataService.GetAllInstalmentByInvoiceNo(salesInvoiceNumber, connection);

                for (var i = 0; i < data.Count; i++)
                {
                    if (remainingAmount > 0)
                    {
                        //Check if my payment exists

                        var dueAmount = data[i].DueAmount;
                        totalDue = totalDue + dueAmount;
                        var obj = new Azolution.Entities.Sale.Collection();

                        if (data[i].Status != 1)
                        {
                            //means unpaid/Partial paid
                            obj.SaleInvoice = data[i].SInvoice;
                            obj.InstallmentId = data[i].InstallmentId;
                            obj.InstallmentNo = data[i].Number;
                            obj.PaymentType = transactionType;
                            // previously used Transaction type as Payment type, so i also use same
                            obj.TransectionId = transactionId;
                            obj.CollectionType = saleTypeId == 1 ? 1 : 2;
                            obj.PayDate = payDate;
                            obj.ACustomer = new Customer
                            {
                                Phone = customerMobileNo,
                                Phone2 = customerSmsMobileNo,
                                BranchSmsMobileNumber = branchMobileNo
                            };

                            obj.DueDate = Convert.ToDateTime(data[i].DueDate);

                            if (data[i].Number == data.Count)
                            {
                                obj.FinalInstallment = 1;
                            }

                            if (data[i].Status == 2)
                            {
                                // 2 means partial paid

                                if (remainingAmount >= dueAmount)
                                {
                                    // check if customer have enough amount to pay full due

                                    //obj.ReceiveAmount = data[i].ReceiveAmount + dueAmount; //Previous receive amount + due amount
                                    obj.ReceiveAmount = dueAmount; //Previous receive amount + due amount
                                    obj.DueAmount = 0;
                                    obj.PaymentStatus = 1; // means full paid this installemnt

                                    remainingAmount = remainingAmount - dueAmount; //cut balance
                                }
                                else
                                {
                                    // if not able to pay full amount then cut remaining amount and left other as due

                                    //obj.ReceiveAmount = data[i].ReceiveAmount + remainingAmount;
                                    obj.ReceiveAmount = remainingAmount;
                                    obj.DueAmount = dueAmount - remainingAmount;
                                    obj.PaymentStatus = 2; //means partially paid

                                    remainingAmount = remainingAmount - dueAmount;
                                }

                                installmentReceicveList.Add(obj); // push created object
                            }

                            else if (data[i].Status == 0)
                            {
                                //means unpaid
                                var dueAmmountToCheck = dueAmount - 1; //This is to consider 1 tk

                                if (remainingAmount >= dueAmmountToCheck)
                                {
                                    // check if customer have enough amount to pay full due
                                    obj.ReceiveAmount = dueAmount;
                                    obj.PaymentStatus = 1; // means full paid this installemnt

                                    remainingAmount = remainingAmount - dueAmount; //cut balance
                                }
                                else
                                {
                                    // if not able to pay full amount then cut remaining amount and left other as due

                                    obj.ReceiveAmount = remainingAmount;
                                    obj.DueAmount = dueAmount - remainingAmount;

                                    obj.PaymentStatus = 2; //means partially paid

                                    remainingAmount = remainingAmount - dueAmount;
                                }
                                installmentReceicveList.Add(obj); // push created object
                            } //End of unpaid check
                        } //End of unpaid/partial paid condition
                    } //End of remaining amount
                } //End Of loop
            }
            return remainingAmount;
        }

        private decimal DownPaymentCollectionProcess(PaymentReceivedInfo paymentInfoObj, string salesInvoiceNumber, int transactionType, string transactionId, DateTime payDate, decimal remainingAmount, List<Azolution.Entities.Sale.Collection> installmentReceicveList, CommonConnection connection, ref string initialCode)
        {
            var downpaymentData = _collectionDataService.GetDownpaymentByInvoiceNo(paymentInfoObj.SaleInvoice);
            var allCollectionData = _collectionDataService.GetAllReceiveAmountByInvoice(paymentInfoObj.SaleInvoice);
            //for downpayment Collection--------------------

            if (downpaymentData != null)
            {
                if (downpaymentData.ReceiveAmount != downpaymentData.DownPay)
                {
                    var objDownpay = new Azolution.Entities.Sale.Collection();
                    objDownpay.SaleInvoice = salesInvoiceNumber;
                    objDownpay.PaymentType = transactionType;
                    // previously used Transaction type as Payment type, so i also use same
                    objDownpay.TransectionId = transactionId;
                    objDownpay.CollectionType = 3; //means downpayment
                    objDownpay.PayDate = payDate;

                    if (downpaymentData.ReceiveAmount == 0)
                    {
                        if (remainingAmount >= downpaymentData.DownPay)
                        {
                            objDownpay.ReceiveAmount = downpaymentData.DownPay;
                            objDownpay.DueAmount = 0;
                            objDownpay.PaymentStatus = 1;
                            remainingAmount = remainingAmount - downpaymentData.DownPay;
                        }
                        else
                        {
                            objDownpay.ReceiveAmount = remainingAmount;
                            objDownpay.DueAmount = downpaymentData.DownPay - remainingAmount;
                            objDownpay.PaymentStatus = 2;
                            remainingAmount = 0;
                        }
                    }
                    else
                    {
                        //if ReceiveAmount != 0

                        if (remainingAmount >= downpaymentData.DueAmount)
                        {
                            objDownpay.ReceiveAmount = downpaymentData.DueAmount;
                            objDownpay.DueAmount = 0;
                            objDownpay.PaymentStatus = 1;


                            remainingAmount = remainingAmount - downpaymentData.DueAmount;
                        }
                        else
                        {
                            objDownpay.ReceiveAmount = remainingAmount;
                            objDownpay.DueAmount = downpaymentData.DueAmount - remainingAmount;
                            objDownpay.PaymentStatus = 2;

                            remainingAmount = 0;
                        }
                    } //End of ReceiveAmount != 0

                    installmentReceicveList.Add(objDownpay); // push created object to save or update data

                    initialCode = GenerateInitialLisence(paymentInfoObj, connection);

                    if (downpaymentData.DownPay <= (paymentInfoObj.ReceiveAmount + downpaymentData.ReceiveAmount))
                    {
                        _collectionDataService.UpdateSaleForDPCollection(paymentInfoObj.SaleInvoice, connection);
                    }

                }

                else
                {
                    if (paymentInfoObj.SaleTypeId == 2)
                    {
                        if (downpaymentData.DownPay == 0 && downpaymentData.ReceiveAmount == 0)
                        {
                            initialCode = GenerateInitialLisence(paymentInfoObj, connection);

                            if (paymentInfoObj.ReceiveAmount + allCollectionData.ReceiveAmount >= allCollectionData.Price)
                            {
                                _collectionDataService.UpdateSaleForDPCollection(paymentInfoObj.SaleInvoice, connection);
                            }

                        }
                    }
                    else
                    {
                        if (downpaymentData.DownPay == 0 && downpaymentData.ReceiveAmount == 0)
                        {
                            initialCode = GenerateInitialLisence(paymentInfoObj, connection);
                            _collectionDataService.UpdateSaleForDPCollection(paymentInfoObj.SaleInvoice, connection);
                        }
                    }
                }
            }
            return remainingAmount;
        }

        private string GenerateInitialLisence(PaymentReceivedInfo paymentInfoObj, CommonConnection connection)
        {
            string initialCode = string.Empty;

            SaleDataService _saleDataService = new SaleDataService();
            CustomerDataService _customerDataService = new CustomerDataService();
            string itemCode = "OP";

            var slaesItemDetails = _saleDataService.GetItemDetailsByInvoiceNo(paymentInfoObj.SaleInvoice, itemCode);
            var customerInfo = _customerDataService.GetCustomerInfobyInvoiceNo(paymentInfoObj.SaleInvoice);

            var itemSlNo = "";
            if (slaesItemDetails != null)
            {
                itemSlNo = slaesItemDetails != null && slaesItemDetails.ItemSLNo == null ? "" : slaesItemDetails.ItemSLNo.Trim();
            }

            if (customerInfo.ProductId != "")
            {
                itemSlNo = customerInfo.ProductId.Trim();
            }

            var installmentData = _collectionDataService.GetInstallmentInfo(paymentInfoObj.SaleInvoice, paymentInfoObj.SaleTypeId);

            var newSaleObj = _collectionDataService.GetExistingSalesInformation(paymentInfoObj.CustomerId);

            newSaleObj.ACustomer.CustomerCode = paymentInfoObj.CustomerCode;
            newSaleObj.ACustomer.Phone2 = paymentInfoObj.Phone2;
            newSaleObj.ACustomer.BranchCode = paymentInfoObj.BranchCode;
            newSaleObj.ACustomer.IsUpgraded = paymentInfoObj.IsCustomerUpgraded;
            newSaleObj.ACustomer.BranchSmsMobileNumber = paymentInfoObj.BranchSmsMobileNumber;
            newSaleObj.SaleTypeId = paymentInfoObj.SaleTypeId;
            newSaleObj.FirstPayDate = Convert.ToDateTime(installmentData.DueDate);
            newSaleObj.ItemSlNo = itemSlNo;
            new LicenceService().GetLicenseObjectNew(newSaleObj);

            initialCode = newSaleObj.ALicense.Number;

            var existInitialCode = _collectionDataService.GetExistInitialCode(paymentInfoObj.SaleInvoice, installmentData.DueDate, initialCode);
            if (existInitialCode == "")
            {
                string smsText = "";

                _saleDataService.SaveLicense(newSaleObj, connection, smsText);
            }


            return initialCode;
        }


        public GridEntity<CustomerWithLicenseCode> GetCustomerWithCodeSummary(GridOptions options, string customerCode, string smsMobileNumber)
        {
            string condition = "";
            if (customerCode != "0")
            {
                condition += condition == "" ? string.Format(@" CustomerCode = '{0}'", customerCode) : string.Format(@" and CustomerCode = '{0}'", customerCode);
            }
            if (smsMobileNumber != "0")
            {
                condition += condition == "" ? string.Format(@" Phone= '{0}'", smsMobileNumber) : string.Format(@" and Phone= '{0}'", smsMobileNumber);
            }
            if (condition != "")
            {
                condition = " Where SP.PackageType=1 and Sale.State=5  And" + condition;
            }
            return _collectionDataService.GetCustomerWithCodeSummary(options, condition);
        }

        public string ResendLicenseCodeSms(CustomerWithLicenseCode objResendSms)
        {
            PhoneSettingsService _phoneSettingsService = new PhoneSettingsService();
            objResendSms.SimNumber = _phoneSettingsService.GetRandomPhoneNumber();
            return _collectionDataService.ResendLicenseCodeSms(objResendSms);
        }

        public Azolution.Entities.Sale.Collection GetDownpaymentByInvoiceNo(string invoiceNo)
        {
            return _collectionDataService.GetDownpaymentByInvoiceNo(invoiceNo);
        }

        public GridEntity<CollectionDto> GetCollectionHistoryByInvoice(GridOptions options, string invoiceNo)
        {
            return _collectionDataService.GetCollectionHistoryByInvoice(options, invoiceNo);
        }

        public string SendSmsByCustomerRating(string saleInvoice, int saleId, decimal receiveAmount, CommonConnection connection, Users user)
        {
            //  connection = new CommonConnection();
            try
            {
                string condition = "";
                if (saleInvoice != "")
                {
                    condition = " Where SaleInvoice='" + saleInvoice + "' And IsSentSMS != 1";
                }

                if (condition != "")
                {
                    condition = condition + " And IsActive=" + 1;
                }
                if (condition == "")
                {
                    condition = " Where IsActive= 1 And IsSentSMS != 1";
                }

                var lisenceData = _collectionDataService.SendSmsByCustomerRating(condition, connection, user);

                //Get active License info

                foreach (var license in lisenceData)
                {
                    var packageInfo = _collectionDataService.GetPackageType(license.SaleInvoice);

                    if (packageInfo != null && packageInfo.PackageType == 1)//code will not sent of item sale
                    {
                        var issueDate = license.IssueDate.AddMonths(-1);
                        //DateTime issueMonthYear = Convert.ToDateTime(license.IssueDate.Month + "/" + license.IssueDate.Year);
                        DateTime issueMonthYear = Convert.ToDateTime(issueDate.Month + "/" + issueDate.Year);

                        DateTime currentMonthYear = Convert.ToDateTime(DateTime.Now.Month + "/" + DateTime.Now.Year);

                        if (issueMonthYear == currentMonthYear)  //Previous Logic
                        {
                            if (license.LType != 1)//initial code will not sent
                            {
                                SendSmsAndUpdateLicenseByCustomerRating(receiveAmount, connection, license);
                            }

                        }

                    }
                }

                //Last License Code sent for renewal code if full paid (Code Added by Rubel on 26-07-2016)  New Logic
                if (lisenceData.Count > 0)
                {
                    var number = lisenceData.Last().Number;
                    var packageInfoLastLicense = _collectionDataService.GetPackageType(lisenceData.Last().SaleInvoice);

                    if (packageInfoLastLicense != null && packageInfoLastLicense.PackageType == 1)//code will not sent of item sale
                    {
                        if (lisenceData.Last().LType != 1)//initial code will not sent
                        {
                            SendSmsAndUpdateLicenseByCustomerRating(receiveAmount, connection, lisenceData.Last());
                            _collectionDataService.UpdateAllLicensebySaleInvoice(lisenceData.Last().SaleInvoice, connection);
                        }
                    }
                }

                //End NEw Code

                return "Success";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

        }

        private void SendSmsAndUpdateLicenseByCustomerRating(decimal receiveAmount, CommonConnection connection, License license)
        {
            ProductDataService _productDataService = new ProductDataService();

            Azolution.Entities.Sale.Sale customerInfo = _productDataService.GetCustomerInfoByInvoiceNo(license.SaleInvoice);
            CustomerDataService _customerDataService = new CustomerDataService();

            List<Due> customerRating = _customerDataService.GetCustomerRatingByCompanyId(customerInfo.ACustomer.CompanyId);

            var duePercent = _collectionDataService.GetDuePercentByInvoiceNo(license.SaleInvoice);

            var totalPayment = duePercent.PaidAmount;
            var totalDue = duePercent.OutStandingAmount;

            foreach (var due in customerRating)
            {
                var duePercentage = Convert.ToDecimal(duePercent.TotalDuePercentTillDate);


                if ((Convert.ToDecimal(due.FromDue) - 1) <= duePercentage &&
                    (Convert.ToDecimal(due.ToDue) + 1) >= duePercentage)
                {
                    if (due.AAllType.TypeId != 4) //4 = red customer
                    {
                        License lisenceObj = new License();
                        lisenceObj.IsSMSSent = 1;
                        lisenceObj.MobileNumber = customerInfo.ACustomer.Phone2 == null
                            ? customerInfo.ACustomer.BranchSmsMobileNumber
                            : customerInfo.ACustomer.Phone2;
                        lisenceObj.Number = license.Number;
                        lisenceObj.IssueDate = license.IssueDate;
                        lisenceObj.Status = 0;
                        lisenceObj.LType = license.LType;

                        PhoneSettingsService _phoneSettingsService = new PhoneSettingsService();
                        string simNumber = _phoneSettingsService.GetRandomPhoneNumber();
                        customerInfo.ReceiveAmount = receiveAmount;
                        _collectionDataService.SendSMSandUpdateLisenceData(lisenceObj, license.SaleInvoice, license.IssueDate,
                            connection, totalPayment, totalDue, simNumber, customerInfo);
                    }
                }
            }
        }

        public object GetPaymentType()
        {
            return _collectionDataService.GetPaymentType();
        }
        public object GetNextInstallmentByInvoiceNo(int invoiceNo)
        {
            return _collectionDataService.GetNextInstallmentByInvoiceNo(invoiceNo);
        }
        public object GetAllCollection(GridOptions options, string companies)
        {
            return _collectionDataService.GetAllCollection(options, companies);
        }

        public List<PendingCollectionChart> GetAllPendingCollections(Users user, int companyId, int branchId, bool isAdministrator, DateTime? fromDate, DateTime? toDate, string companies)
        {

            //here PendingCollections means month wise bussiness trend graph's data
            string topParam = "";

            string groupParam = "";

            string whereCondition = "";
            if (isAdministrator != true)
            {
                //topParam = "CompanyId,BranchId,";
                //groupParam = ",tblAll.CompanyId,tblAll.BranchId";
                //whereCondition = "where CompanyId=" + user.CompanyId + " and BranchId = " + user.BranchId;
                whereCondition = "where CompanyId in (" + companies + ") ";
            }
            else
            {


                if (isAdministrator == true && (companyId != 0 || branchId != 0))
                {
                    if (companyId != 0)
                    {
                        topParam = "CompanyId,";
                        groupParam = ",tblAll.CompanyId";
                        whereCondition = "where CompanyId=" + companyId;
                    }
                    if (branchId != 0)
                    {
                        topParam = "BranchId,";
                        groupParam = ",tblAll.BranchId";
                        whereCondition = "where BranchId=" + branchId;
                    }
                    if (companyId != 0 && branchId != 0)
                    {
                        topParam = "CompanyId,BranchId,";
                        groupParam = ",tblAll.CompanyId,tblAll.BranchId";
                        whereCondition = "where CompanyId=" + companyId + " and BranchId = " + branchId;
                    }
                }
                if (isAdministrator == true && (companyId == 0 && companies != ""))
                {
                    topParam = "CompanyId,";
                    groupParam = ",tblAll.CompanyId";

                    whereCondition = "where CompanyId in (" + companies + ")";

                }
            }

            //topParam = "CompanyId,BranchId,";

            //groupParam = ",tblAll.CompanyId,tblAll.BranchId";

            // whereCondition = "where CompanyId=2";

            return _collectionDataService.GetAllPendingCollections(user, topParam, groupParam, whereCondition, fromDate, toDate);
        }
        public List<PendingCollectionChart> GetMonthWiseCollectionData(int userId, int companyId, int branchId, DateTime? fromDate, DateTime? toDate, string companies)
        {
            string topParam = "";

            string groupParam = "";

            string whereCondition = "";

            if (companyId != 0 || branchId != 0)
            {
                if (companyId != 0)
                {
                    topParam = "CompanyId,";
                    groupParam = ",tblAll.CompanyId";
                    whereCondition = "and CompanyId=" + companyId;
                }
                if (branchId != 0)
                {
                    topParam = "BranchId,";
                    groupParam = ",tblAll.BranchId";
                    whereCondition = "and BranchId=" + branchId;
                }
                if (companyId != 0 && branchId != 0)
                {
                    topParam = "CompanyId,BranchId,";
                    groupParam = ",tblAll.CompanyId,tblAll.BranchId";
                    whereCondition = "and CompanyId=" + companyId + " and BranchId = " + branchId;
                }
            }
            else
            {
                topParam = "CompanyId,";
                groupParam = ",tblAll.CompanyId";
                whereCondition = "and CompanyId in (" + companies + " )";
            }

            return _collectionDataService.GetMonthWiseCollectionData(userId, topParam, groupParam, whereCondition, fromDate, toDate);
        }
        public List<PendingCollectionChart> GetMonthWiseSalesData(int userId, int companyId, int branchId, DateTime? fromDate, DateTime? toDate, string companies)
        {
            string topParam = "";

            string groupParam = "";

            string whereCondition = "";

            if (companyId != 0 || branchId != 0)
            {
                if (companyId != 0)
                {
                    topParam = "CompanyId,";
                    groupParam = ",tblAll.CompanyId";
                    whereCondition = "and  CompanyId=" + companyId;
                }
                if (branchId != 0)
                {
                    topParam = "BranchId,";
                    groupParam = ",tblAll.BranchId";
                    whereCondition = "and BranchId=" + branchId;
                }
                if (companyId != 0 && branchId != 0)
                {
                    topParam = "CompanyId,BranchId,";
                    groupParam = ",tblAll.CompanyId,tblAll.BranchId";
                    whereCondition = "and CompanyId=" + companyId + " and BranchId = " + branchId;
                }
            }
            else
            {
                topParam = "CompanyId,";
                groupParam = ",tblAll.CompanyId";
                whereCondition = "and CompanyId in (" + companies + " )";
            }

            return _collectionDataService.GetMonthWiseSalesData(userId, topParam, groupParam, whereCondition, fromDate, toDate);
        }

        public object GetTenRedCustomer(GridOptions options, string invoice, bool isAdministrator, Users objUser, int companyId, int branchId, string companies)
        {


            string condition = "";
            string companyAndbranchId = "";
            if (invoice != "0")
            {
                condition += condition == "" ? " SI.SInvoice=" + invoice : " and SI.SInvoice=" + invoice;
            }


            if (isAdministrator != true)
            {
                condition += condition == ""
                                ? " sc.CompanyId = " + objUser.CompanyId + " and sc.BranchId = " + objUser.BranchId
                                : " And sc.CompanyId = " + objUser.CompanyId + " and sc.BranchId = " + objUser.BranchId;
            }

            else if (isAdministrator == true && (companyId != 0 || branchId != 0))
            {
                if (companyId != 0)
                {
                    companyAndbranchId = "  sc.CompanyId =  " + companyId;
                }
                if (branchId != 0)
                {
                    companyAndbranchId = "  sc.BranchId =  " + branchId;
                }
                if (companyId != 0 && branchId != 0)
                {
                    companyAndbranchId = "  sc.CompanyId =" + companyId + " and sc.BranchId =  " + branchId;
                }
                condition += condition == "" ? companyAndbranchId : " and " + companyAndbranchId;
            }
            if (isAdministrator == true && (companyId == 0 && companies != ""))
            {
                condition += condition == ""
                             ? " sc.CompanyId in ( " + companies + " ) "
                             : " And sc.CompanyId in ( " + companies + " ) ";
            }
            if (condition != "")
            {
                condition = "Where " + condition;
            }
            string orderBy = "TotalDuePercentTillDate desc";
            string condition2 = "";
            return _collectionDataService.GetDuePercentAndCustomerInfoForDashboardGrid(options, condition, condition2, orderBy);

        }

        public List<Installment> GetAllInstalmentByInvoiceNo(string invoiceNo)
        {
            var connection = new CommonConnection();
            return _collectionDataService.GetAllInstalmentByInvoiceNo(invoiceNo, connection);
        }

        public Azolution.Entities.Sale.Collection GetDownpaymentByInvoiceId(int saleId)
        {
            var connection = new CommonConnection();
            return _collectionDataService.GetDownpaymentBySaleId(saleId, connection);
        }

        public string SendSmsByCustomerRatingWhenLogin(string saleInvoice, Users user)
        {
            CommonConnection connection = new CommonConnection();
            int saleId = 0;
            var res = SendSmsByCustomerRating(saleInvoice, saleId, 0, connection, user);
            connection.Close();
            return res;
        }

        public RatingCalculation GetDuePercentByInvoiceNo(string invoiceNo)
        {

            return _collectionDataService.GetDuePercentByInvoiceNo(invoiceNo);
        }

        public GridEntity<RatingCalculation> GetDueCollectionCustomerGridData(GridOptions options, bool isAdministrator, Users objUser, int companyId, int branchId, string companies)
        {
            string condition = "";
            if (isAdministrator != true)
            {

                condition = "Where sc.CompanyId = " + objUser.CompanyId + " and sc.BranchId = " + objUser.BranchId;
            }

            else if (isAdministrator == true && (companyId != 0 || branchId != 0))
            {
                if (companyId != 0)
                {
                    condition = " where sc.CompanyId =  " + companyId;
                }
                if (branchId != 0)
                {
                    condition = " where sc.BranchId =  " + branchId;
                }
                if (companyId != 0 && branchId != 0)
                {
                    condition = " where sc.CompanyId =" + companyId + " and sc.BranchId =  " + branchId;
                }
            }

            if (isAdministrator == true && (companyId == 0 && companies != ""))
            {
                condition += condition == ""
                             ? "Where sc.CompanyId in ( " + companies + " ) "
                             : " And sc.CompanyId in ( " + companies + " ) ";
            }
            string orderBy = "TotalDuePercentTillDate";
            string condition2 = "where tblTemp3.TotalDuePercentTillDate != 0";
            return _collectionDataService.GetDuePercentAndCustomerInfoForDashboardGrid(options, condition, condition2, orderBy);

        }

        public GridEntity<ReleaseLisenceGenerate> GetCustomerForReleaseLisenceGridData(GridOptions options, string companies, int branchId)
        {
            string condition = "";
            if (companies != "")
            {
                condition += " and tbl3.CompanyId in (" + companies + ")";
                //condition += " and tbl3.CompanyId in (" + companies + ")";
            }
            if (branchId != 0)
            {
                condition += " and tbl3.BranchId=" + branchId;
            }
            if (companies != "" && branchId != 0)
            {
                condition += " and tbl3.CompanyId in (" + companies + ")" + " and tbl3.BranchId=" + branchId;
            }

            return _collectionDataService.GetCustomerForReleaseLisenceGridData(options, condition);
        }

        public string GenerateReleaseLisenceFromRootUser(ReleaseLisenceGenerate strobjReleaseLicenseInfo, int varificationType)
        {
            PhoneSettingsService _phoneSettingsService = new PhoneSettingsService();
            string simNumber = _phoneSettingsService.GetRandomPhoneNumber();
            return _collectionDataService.GenerateReleaseLicenseAndSendSms(strobjReleaseLicenseInfo, varificationType, simNumber);
        }


        //public bool CollectDownpayment(List<Azolution.Entities.Sale.Sale> salesList, CommonConnection connection, Users user)
        //{
        //    SaleDataService _saleDataService = new SaleDataService();
        //    CollectionService _collectionService = new CollectionService();

        //    string res = "";
        //    bool returnValue = false;

        //    if (salesList.Any())
        //    {
        //        decimal remainingAmont = 0;


        //        foreach (var sale in salesList)
        //        {
        //            var downPaymentdata = _saleDataService.GetDownPaymentTempDataBySaleId(sale.SaleId, connection);
        //            if (downPaymentdata != null)
        //            {
        //                if (sale.DownPay < downPaymentdata.ReceiveAmount)//if receive amount greater than downpayment then it divide the amount and collect downpay first then save initial license then collect other amount
        //                {
        //                    remainingAmont = downPaymentdata.ReceiveAmount - sale.DownPay;
        //                    downPaymentdata.ReceiveAmount = sale.DownPay;
        //                    res = _collectionService.GetPaymentAndCollect(downPaymentdata, connection, user.UserId);
        //                }
        //                else
        //                {
        //                    res = _collectionService.GetPaymentAndCollect(downPaymentdata, connection, user.UserId);
        //                }

        //                if (remainingAmont > 0)
        //                {
        //                    downPaymentdata.ReceiveAmount = remainingAmont;
        //                    res = _collectionService.GetPaymentAndCollect(downPaymentdata, connection, user.UserId);
        //                    remainingAmont = 0;//This is to Clear the receive amount
        //                }

        //            }
        //        }
        //        if (res == "Success")
        //        {
        //            returnValue = true;
        //        }
        //    }

        //    return returnValue;
        //}
    }
}
