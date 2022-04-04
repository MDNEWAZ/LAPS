using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Report;
using Azolution.Entities.Sale;
using DataService.DataService;
using Laps.Customer.Service.Service;

using Laps.SaleRepresentative.DataService.DataService;
using SmsService;
using Solaric.GenerateCode;
using Utilities;

namespace Laps.Collection.CollectionDataService.DataService
{
    public class CollectionDataService
    {
        SqlCommand _aCommand;
        SqlDataAdapter _adapter;
        readonly string _connection = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        public string CollectPaymentAndUpdatePaymentStatus(List<Azolution.Entities.Sale.Collection> objCollection, CommonConnection connection, int userId, string initialCode, PaymentReceivedInfo paymentReceivedInfo,string brCode, string cusCode)
        {
            var res = "";
            var sql = "";

            try
            {

                var updateDate = DateTime.Now.ToString("dd-MMM-yyyy");

                SaveCollection(connection, objCollection, userId, paymentReceivedInfo);

                StringBuilder stringBuilder = new StringBuilder();
                foreach (var collection in objCollection)
                {
                    if (collection.InstallmentId > 0)
                    {
                        sql = string.Format(@" Update Sale_Installment Set Status={0},Updated='{1}' Where InstallmentId={2};", collection.PaymentStatus, updateDate, collection.InstallmentId);
                        stringBuilder.Append(sql);
                    }
                }
                if (stringBuilder.ToString() != "")
                {
                    sql = "Begin \r\n" + stringBuilder + "\r\n End;";
                    connection.ExecuteNonQuery(sql);
                }

                #region Save Lisence

                GenerateLisenceAndSendSMS(objCollection, connection, initialCode, paymentReceivedInfo,brCode,cusCode);

                #endregion
                res = Operation.Success.ToString();

            }
            catch (Exception exception)
            {
                // connection.RollBack();
                throw new Exception("Exception During Collection Data Save: " + exception.Message);

            }


            return res;
        }

        public void GenerateLisenceAndSendSMS(List<Azolution.Entities.Sale.Collection> objCollection, CommonConnection connection, string initialCode, PaymentReceivedInfo paymentReceivedInfo,string brCode,string cusCode)
        {
            string previousCode = string.Empty;
            string oldNumber = initialCode;

            var codeGenerator = new LicenceService();
            try
            {
                foreach (var collection in objCollection)
                {


                    if (collection.CollectionType != 3)
                    {
                        //if (collection.PaymentStatus != 2)
                        //{
                        if (collection.FinalInstallment != 1)
                        {
                            var mobileNo = collection.ACustomer.Phone == null ? collection.ACustomer.BranchSmsMobileNumber : collection.ACustomer.Phone;
                            // If customer mobile no. not exists then it take branch Mobile No.
                            var lType = 0; //1= by default it will be renewal lisence type for Code generator param

                            //if (paymentReceivedInfo.SaleTypeId == 1)
                            //{
                            if (oldNumber == "")
                            {
                                previousCode = GetPreviousCode(collection.DueDate, collection.SaleInvoice, connection);
                            }
                            else
                            {
                                previousCode = oldNumber;
                            }
                            //}
                            var newLisenceNo = string.Empty;


                            //string brCode = "";
                            //string cusCode = "";
                            //GetCompanyCodeAndBranchCodeFromItemSLNo(collection.SaleInvoice, ref brCode, ref cusCode);

                            //for new customer only
                            string customerCodeParam = cusCode == "" ? paymentReceivedInfo.CustomerCode.Substring(paymentReceivedInfo.CustomerCode.Length - 4) : cusCode;
                            // string branchCodeParam = paymentReceivedInfo.BranchCode.Substring(paymentReceivedInfo.BranchCode.Length - 4);

                            //if (paymentReceivedInfo.SaleTypeId == 1)
                            //{
                            lType = 1;
                            if (paymentReceivedInfo.IsCustomerUpgraded == 1)//for old customer
                            {
                                if (collection.DueAmount == 0)
                                {
                                    newLisenceNo = codeGenerator.GenerateLicence(mobileNo, previousCode, lType);                                    
                                }
                            }
                            else // for new customer
                            {
                                if (collection.DueAmount == 0)
                                {
                                    newLisenceNo = codeGenerator.GenerateLicence(customerCodeParam, previousCode, lType);                                    
                                }
                            }
                            // }
                         
                            oldNumber = newLisenceNo;

                            License lisenceObj = new License();
                            lisenceObj.IssueDate = collection.DueDate;
                            lisenceObj.Number = newLisenceNo;
                          
                            lisenceObj.LType = 2;

                            lisenceObj.MobileNumber = mobileNo;
                            lisenceObj.EntryDate = DateTime.Now;
                            lisenceObj.SaleInvoice = collection.SaleInvoice;

                            DateTime dueMonthYear = Convert.ToDateTime(collection.DueDate.Month + "/" + collection.DueDate.Year);
                            DateTime currentMonthYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                            DateTime licenseIssueDateForNextMonth = new DateTime(collection.DueDate.Year, collection.DueDate.Month, 1).AddMonths(1);

                            if (licenseIssueDateForNextMonth < currentMonthYear)
                            {
                                //lisenceObj.IsSMSSent = 1;
                                lisenceObj.IsSMSSent = 0;
                                // this license is for past due month , for that sms will not be sent ever.
                                lisenceObj.Status = 0;
                            }
                            else
                            {
                                lisenceObj.Status = 1;
                                lisenceObj.IsSMSSent = 0;
                            }
                            if (collection.DueAmount == 0)
                            {
                                SaveLisenceInfo(lisenceObj, connection);                                
                            }

                        }
                    }

                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to save Lisence Info");
            }
        }

        public void GetCompanyCodeAndBranchCodeFromItemSLNo(string invoiceNo, ref string brCode, ref string cusCode)
        {
            CustomerDataService _customerDataService = new CustomerDataService();
            string itemCode = "OP";

            var slaesItemDetails = GetItemDetailsByInvoiceNo(invoiceNo, itemCode);
            var customerInfo = _customerDataService.GetCustomerInfobyInvoiceNo(invoiceNo);

            if (customerInfo.ProductId != "")
            {
                var itemSlNo = customerInfo.ProductId.Trim();

                if (itemSlNo != "" && itemSlNo.Length == 8)
                {
                    brCode = itemSlNo.Substring(0, 4);
                    cusCode = itemSlNo.Substring(itemSlNo.Length - 4);
                }
            }
            if (slaesItemDetails != null)
            {
                var itemSlNo = slaesItemDetails != null && slaesItemDetails.ItemSLNo == null ? "" : slaesItemDetails.ItemSLNo.Trim();

                if (itemSlNo != "" && itemSlNo.Length == 8)
                {
                    brCode = itemSlNo.Substring(0, 4);
                    cusCode = itemSlNo.Substring(itemSlNo.Length - 4);
                }
            }
            
        }
        public SalesItemDetails GetItemDetailsByInvoiceNo(string invoice, string itemCode)
        {
            string sql = string.Format(@"Select SPI.ModelId,SPI.ItemId,SPI.ItemName,IsPriceApplicable,ItemCode,ItemSLNo From Sale_Product_Items SPI
            inner join SalesItem on SalesItem.ItemId=SPI.ItemId
            left join SalesItemDetails on SalesItem.SalesItemId=SalesItemDetails.SalesItemId
            inner join Sale on Sale.SaleId=SalesItem.SaleId
            Where Sale.Invoice='{0}' And ItemCode='{1}'", invoice, itemCode);
            var data = Data<SalesItemDetails>.DataSource(sql).SingleOrDefault();
            return data;
        }
        public string SaveCollection(CommonConnection connection, List<Azolution.Entities.Sale.Collection> collectionList, int userId, PaymentReceivedInfo payReceiveInfo)
        {
            string res = "";
            try
            {

                StringBuilder stringBuilder = new StringBuilder();

                string insertQuery = "";
                var entryDate = DateTime.Now;
                var payDate = "";
                foreach (var objCollection in collectionList)
                {
                    if (payReceiveInfo.TypeId == 3)
                    {
                        payDate = payReceiveInfo.SaleDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        payDate = objCollection.PayDate.ToString("MM/dd/yyyy");
                    }

                    var dueDate = objCollection.DueDate == DateTime.MinValue ? null : objCollection.DueDate.ToString("MM/dd/yyyy");

                    insertQuery = string.Format(@" Insert Into Sale_Collection(SaleInvoice,InstallmentNo,TransectionType,TransectionId,
                                    ReceiveAmount,PayDate,EntryDate,InstallmentId,Flag,IsActive,CollectedBy,DueAmount,CollectionType,DueDate) 
                        Values('{0}',{1},{2},'{3}',{4},'{5}','{6}',{7},{8},{9},{10},{11},{12},'{13}');",
                   objCollection.SaleInvoice, objCollection.InstallmentNo, objCollection.PaymentType, objCollection.TransectionId,
                    objCollection.ReceiveAmount, payDate, entryDate, objCollection.InstallmentId, 1, 1, userId, objCollection.DueAmount, objCollection.CollectionType, dueDate);

                    stringBuilder.Append(insertQuery);
                }

                if (stringBuilder.ToString() != "")
                {
                    insertQuery = "Begin \r\n" + stringBuilder + "\r\n End;";
                    connection.ExecuteNonQuery(insertQuery);
                }

                res = Operation.Success.ToString();

            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public void SaveLisenceInfo(License lisenceObj, CommonConnection connection)
        {
            try
            {
                var issueDate = new DateTime(lisenceObj.IssueDate.Year, lisenceObj.IssueDate.Month, lisenceObj.IssueDate.Day);
                if (lisenceObj.LType != 3)
                {
                    issueDate =issueDate.AddDays(30);

                    var issueMonth = lisenceObj.IssueDate.Month;
                    var newMonth = issueDate.Month;
                    if (issueMonth == newMonth)
                    {
                        issueDate = new DateTime(issueDate.Year, issueDate.Month, issueDate.Day).AddDays(1);
                    }
                }

                string isLisenceGeneratedQuery = string.Format(@"Select * from Sale_License
                where IssueDate = '{0}' and Number = {1} And  SaleInvoice='{2}'", issueDate, lisenceObj.Number, lisenceObj.SaleInvoice);

                var isLisenceGenerated = connection.Data<License>(isLisenceGeneratedQuery);

                if (isLisenceGenerated.Count == 0)
                {
                    string query = string.Format(@"INSERT INTO Sale_License([Number],[LType],[IssueDate],[EntryDate] ,[Updated],[ModelId],[SaleInvoice],[IsActive],IsSentSMS,ReleaseVarifiedBy)
                                        VALUES('{0}',{1},'{2}','{3}','{4}',{5},'{6}',{7},{8},{9})",
                                       lisenceObj.Number, lisenceObj.LType, issueDate, DateTime.Now, "",
                                       0, lisenceObj.SaleInvoice, lisenceObj.Status, lisenceObj.IsSMSSent, lisenceObj.VarificationType);
                    connection.ExecuteNonQuery(query);
                }



            }
            catch (Exception ex)
            {

                if (!ex.Message.Contains("UNIQUE"))//(SaleInvoice With LisenceNumber) UNIQUE
                {
                    throw new Exception(ex.Message);
                }
            }


        }
        public List<AllType> GetPaymentType()
        {
            try
            {
                const string sql = " select * from Sale_AllType where Flag='Collection' And IsActive=1";
                var data = Kendo<AllType>.Combo.DataSource(sql);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public int CheckIsAlreadyPaid(Azolution.Entities.Sale.Collection objCollection)
        {
            try
            {
                string sql = string.Format(@" Select * From Sale_Installment Where SInvoice={0} and Number={1} and Status=1", objCollection.SaleInvoice, objCollection.InstallmentNo);
                var data = Data<Azolution.Entities.Sale.Collection>.DataSource(sql);
                return data.Count;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public Installment GetNextInstallmentByInvoiceNo(int invoiceNo)
        {
            try
            {
                string sql = string.Format(@"Select * from  Sale_Installment  where SInvoice={0} AND Number =(Select MIN(Number) Number from Sale_Installment Where Status=0 and  SInvoice={0})", invoiceNo);
                var data = Data<Installment>.GenericDataSource(sql);
                return data.SingleOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public GridEntity<RCollection> GetAllCollection(GridOptions options, string companies)
        {
            string sql = string.Format(@"Select tblCollection.*,SC.Name,SC.Phone,SC.Phone2,SC.NID,SC.ProductId,
                SC.ReferenceId,SC.CustomerCode,SC.IsStaff,BRANCHNAME,BranchSmsMobileNumber As BranchSmsMobNo ,BranchCode,SC.IsUpgraded  From (
                Select tSale.*,SUM(isnull(Sale_Collection.ReceiveAmount,0)) as PaidAmount,
                (NetPrice+DownPay-SUM(isnull(Sale_Collection.ReceiveAmount,0))) as DueAmount
                From (
                Select Invoice,CustomerId,sum(NetPrice)NetPrice,sum(DownPay) DownPay,sum((NetPrice+DownPay)) as Amount,BranchId,Sale.SaleTypeId From Sale --and Sale.IsRelease <>1
                where 
                Sale.CompanyId in ({0}) and Sale.State in (4,5)
                group by Sale.Invoice,Sale.CustomerId,Sale.BranchId,Sale.SaleTypeId) tSale 
                left outer join Sale_Collection on Sale_Collection.SaleInvoice = tSale.Invoice 
                group by tSale.Invoice,NetPrice, DownPay,Amount,CustomerId,tSale.BranchId,tSale.SaleTypeId) tblCollection
                inner join Sale_Customer SC on SC.CustomerId=tblCollection.CustomerId and SC.IsActive=1
                left outer join Branch on Branch.BRANCHID = tblCollection.BranchId", companies);

            var data = Kendo<RCollection>.Grid.GenericDataSource(options, sql, "CustomerId");
            return data;
        }



        public List<PendingCollectionChart> GetAllPendingCollections(Users userId, string topParam, string groupParam, string whereCondition, DateTime? fromDate, DateTime? toDate)
        {
            //This mathod works for Monthly business trend
            try
            {


                //var currentDate = DateTime.Now;
                //var startMonth = currentDate.AddMonths(-12);
                //var startDate = "1/" + startMonth.Month + "/" + startMonth.Year;

                var currentDate = DateTime.Now;

                var startMonth = currentDate.AddMonths(-6);
                var startDate = "1/" + startMonth.Month + "/" + startMonth.Year;
                currentDate = currentDate.AddDays(1);
                var lastDate = currentDate.Month + "/" + currentDate.Day + "/" + currentDate.Year;
                if (fromDate != null)
                {
                    startDate = fromDate.ToString();
                }
                if (toDate != null)
                {
                    lastDate = toDate.ToString();
                }

                string query = string.Format(@"Select SUM(tblAll.SalesPrice) as SalesPrice, SUM(tblAll.ReceiveAmount) as ReceiveAmount, WarantyStartMonth,SUM(tblAll.OutStanding) as OutStanding from (
select tblFinal.RowNumber,tblFinal.CompanyId,tblFinal.BranchId,tblFinal.SalesPrice,tblFinal.ReceiveAmount,tblFinal.WarantyStartMonth,tblFinal.SalesPrice-tblFinal.ReceiveAmount as OutStanding --tblFinal.SalesPrice-tblFinal.ReceiveAmount+Sum(isnull(tbl4.OutStanding,0)) as OutStanding 

from (
Select CompanyId,BranchId,Sum(SalesPrice) as SalesPrice,Sum(ReceiveAmount) as ReceiveAmount,WarantyStartMonth,(Sum(SalesPrice)-Sum(ReceiveAmount)) as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth desc) AS RowNumber from (
select * from (
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,sum(isnull(Sale.DownPay + Sale.NetPrice,0)) as SalesPrice,0 as ReceiveAmount,DATENAME(MM, Sale.WarrantyStartDate) + ' ' + CAST(YEAR(Sale.WarrantyStartDate) AS VARCHAR(4)) as WarantyStartMonth
from Sale_Customer
left outer join Sale on Sale.CustomerId = Sale_Customer.CustomerId and Cast(WarrantyStartDate as datetime) between '{0}' and '{1}' and Sale.IsActive =1
where Sale.WarrantyStartDate is not null
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, Sale.WarrantyStartDate) + ' ' + CAST(YEAR(Sale.WarrantyStartDate) AS VARCHAR(4))) as tbl1
union all
select * from (
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,0 as SalesPrice,Sum(ReceiveAmount) as ReceiveAmount,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4)) as PayDate
from Sale_Collection
inner join Sale on Sale.Invoice = Sale_Collection.SaleInvoice  and Sale.IsActive =1
inner join Sale_Customer on Sale_Customer.CustomerId = Sale.CustomerId
where PayDate between '{0}' and '{1}'
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4))) as tbl2 ) as tbl3 group by CompanyId,BranchId,WarantyStartMonth) tblFinal
left outer join (
Select CompanyId,BranchId,Sum(SalesPrice) as SalesPrice,Sum(ReceiveAmount) as ReceiveAmount,WarantyStartMonth,(Sum(SalesPrice)-Sum(ReceiveAmount)) as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth desc) AS RowNumber from (
select * from (
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,sum(isnull(Sale.DownPay + Sale.NetPrice,0)) as SalesPrice,0 as ReceiveAmount,DATENAME(MM, Sale.WarrantyStartDate) + ' ' + CAST(YEAR(Sale.WarrantyStartDate) AS VARCHAR(4)) as WarantyStartMonth
from Sale_Customer
left outer join Sale on Sale.CustomerId = Sale_Customer.CustomerId and Cast(WarrantyStartDate as datetime) between '{0}' and '{1}'
where Sale.WarrantyStartDate is not null
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, Sale.WarrantyStartDate) + ' ' + CAST(YEAR(Sale.WarrantyStartDate) AS VARCHAR(4))) as tbl1
union all
select * from (
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,0 as SalesPrice,Sum(ReceiveAmount) as ReceiveAmount,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4)) as PayDate
from Sale_Collection
inner join Sale on Sale.Invoice = Sale_Collection.SaleInvoice
inner join Sale_Customer on Sale_Customer.CustomerId = Sale.CustomerId
where PayDate between '{0}' and '{1}'
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4))) as tbl2 ) as tbl3 group by CompanyId,BranchId,WarantyStartMonth
) tbl4 on tbl4.RowNumber < tblFinal.RowNumber and tbl4.CompanyId = tblFinal.CompanyId 
group by tblFinal.RowNumber,tblFinal.CompanyId,tblFinal.BranchId,tblFinal.SalesPrice,tblFinal.ReceiveAmount,tblFinal.WarantyStartMonth,tblFinal.OutStanding
 ) tblAll {3}
 
 group by WarantyStartMonth 
  --order by WarantyStartMonth desc 
order by CONVERT(datetime, WarantyStartMonth, 110)  ", startDate, lastDate, topParam, whereCondition, groupParam);
                return Data<PendingCollectionChart>.DataSource(query);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<PendingCollectionChart> GetMonthWiseCollectionData(int userId, string topParam, string groupParam, string whereCondition, DateTime? fromDate, DateTime? toDate)
        {
            try
            {


                var currentDate = DateTime.Now;
                var startMonth = currentDate.AddMonths(-6);
                var startDate = "1/" + startMonth.Month + "/" + startMonth.Year;
                var lastDate = currentDate.Month + "/" + currentDate.Day + "/" + currentDate.Year;
                //var  startDate = new DateTime(startMonth.Year,startMonth.Month,1).ToString("MM/dd/yyyy");
                //var lastDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).ToString("MM/dd/yyyy");

                if (fromDate != null)
                {
                    startDate = fromDate.ToString();
                }
                if (toDate != null)
                {
                    lastDate = toDate.ToString();
                }
                //var dateTo = Convert.ToDateTime(lastDate);



                string query = string.Format(@"
Select tblFinal.ReceiveAmount,Sum(tblJoinForOutStanding.OutStanding) as OutStanding, DATENAME(MM, tblFinal.WarantyStartMonth) + ' ' + CAST(YEAR(tblFinal.WarantyStartMonth) AS VARCHAR(4)) as WarantyStartMonth from (


select ReceiveAmount,WarantyStartMonth,ReceiveAmount as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (

Select sum(isnull(ReceiveAmount,0)) as ReceiveAmount,
convert(datetime,(cast(YEAR(Sale_Collection.PayDate) as varchar(4)) + '-' + cast(Month(Sale_Collection.PayDate) as varchar(4)) + '-' + '01'),101) as WarantyStartMonth
 from Sale_Collection
left outer join Sale on Sale.Invoice = Sale_Collection.SaleInvoice and Sale.IsActive = 1
where Sale_Collection.PayDate is not null and Cast(PayDate as datetime) between '{0}' and '{1}'
{3}
group by convert(datetime,(cast(YEAR(PayDate) as varchar(4)) + '-' + cast(Month(PayDate) as varchar(4)) + '-' + '01'),101)) tbl1) tblFinal


left outer join (

select ReceiveAmount,WarantyStartMonth,ReceiveAmount as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (

select ReceiveAmount,WarantyStartMonth,ReceiveAmount as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (

Select sum(isnull(ReceiveAmount,0)) as ReceiveAmount,
convert(datetime,(cast(YEAR(Sale_Collection.PayDate) as varchar(4)) + '-' + cast(Month(Sale_Collection.PayDate) as varchar(4)) + '-' + '01'),101) as WarantyStartMonth
 from Sale_Collection
left outer join Sale on Sale.Invoice = Sale_Collection.SaleInvoice and Sale.IsActive = 1
where Sale_Collection.PayDate is not null and Cast(PayDate as datetime) between '{0}' and '{1}'
{3}
group by convert(datetime,(cast(YEAR(PayDate) as varchar(4)) + '-' + cast(Month(PayDate) as varchar(4)) + '-' + '01'),101)) tbl1) tblFinal
)
 tblJoinForOutStanding on  tblJoinForOutStanding.RowNumber<=tblFinal.RowNumber
 group by tblFinal.ReceiveAmount,tblFinal.WarantyStartMonth,tblFinal.ReceiveAmount,tblFinal.OutStanding,tblFinal.RowNumber ",
        startDate, lastDate, topParam, whereCondition, groupParam);
                return Data<PendingCollectionChart>.DataSource(query);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<PendingCollectionChart> GetMonthWiseSalesData(int userId, string topParam, string groupParam, string whereCondition, DateTime? fromDate, DateTime? toDate)
        {
            try
            {


                //var currentDate = DateTime.Now;
                //var startMonth = currentDate.AddMonths(-6);
                //var startDate = "1/" + startMonth.Month + "/" + startMonth.Year;
                //if (fromDate != null)
                //{
                //    startDate = fromDate.ToString();
                //}
                //if (toDate != null)
                //{
                //    toDate = toDate;
                //}

                var currentDate = DateTime.Now;
                var startMonth = currentDate.AddMonths(-6);
                var startDate = "1/" + startMonth.Month + "/" + startMonth.Year;
                var lastDate = currentDate.Month + "/" + currentDate.Day + "/" + currentDate.Year;
                //var startDate = new DateTime(startMonth.Year, startMonth.Month, 1).ToString("dd/MM/yyyy");
                //var lastDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).ToString("dd/MM/yyyy");



                if (fromDate != null)
                {
                    startDate = fromDate.ToString();
                }
                if (toDate != null)
                {
                    lastDate = toDate.ToString();
                }

                string query = string.Format(@"Select tblFinal.SalesPrice, DATENAME(MM, tblFinal.WarantyStartMonth) + ' ' + CAST(YEAR(tblFinal.WarantyStartMonth) AS VARCHAR(4)) as WarantyStartMonth,Sum(tblJoinForOutStanding.OutStanding) as OutStanding
from (
Select SalesPrice,WarantyStartMonth,SalesPrice as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (
Select sum(isnull(Sale.DownPay + Sale.NetPrice,0)) as SalesPrice,
convert(datetime,(cast(YEAR(Sale.WarrantyStartDate) as varchar(4)) + '-' + cast(Month(Sale.WarrantyStartDate) as varchar(4)) + '-' + '01'),101) as WarantyStartMonth
from Sale
where Sale.WarrantyStartDate is not null and Sale.IsActive=1 and Cast(WarrantyStartDate as datetime) between '{0}' and '{1}'
{3}
group by convert(datetime,(cast(YEAR(Sale.WarrantyStartDate) as varchar(4)) + '-' + cast(Month(Sale.WarrantyStartDate) as varchar(4)) + '-' + '01'),101)) tbl1) tblFinal

left outer join (Select * from (

Select SalesPrice,WarantyStartMonth,SalesPrice as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (
Select sum(isnull(Sale.DownPay + Sale.NetPrice,0)) as SalesPrice,
convert(datetime,(cast(YEAR(Sale.WarrantyStartDate) as varchar(4)) + '-' + cast(Month(Sale.WarrantyStartDate) as varchar(4)) + '-' + '01'),101) as WarantyStartMonth
from Sale
where Sale.WarrantyStartDate is not null and Sale.IsActive=1 and Cast(WarrantyStartDate as datetime) between '{0}' and '{1}'
{3}
group by convert(datetime,(cast(YEAR(Sale.WarrantyStartDate) as varchar(4)) + '-' + cast(Month(Sale.WarrantyStartDate) as varchar(4)) + '-' + '01'),101)) tbl1) tblFinal) 
tblJoinForOutStanding on  tblJoinForOutStanding.RowNumber<=tblFinal.RowNumber
group by tblFinal.SalesPrice,tblFinal.WarantyStartMonth,tblFinal.SalesPrice,tblFinal.OutStanding,tblFinal.RowNumber
", startDate, lastDate, topParam, whereCondition, groupParam);
                return Data<PendingCollectionChart>.DataSource(query);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public GridEntity<RCollection> GetTenRedCustomer(GridOptions options, string invoice, int userId)
        {
            GridEntity<RCollection> gridEntity = null;
            try
            {
                var aRCollections = new List<RCollection>();
                using (var conn = new SqlConnection(_connection))
                {
                    conn.Open();
                    var cmd = new SqlCommand("[GetPendingCollecton_ByCustomer]", conn) { CommandType = CommandType.StoredProcedure };
                    SqlParameter[] param = {
                                               new SqlParameter("@userId", userId), 
                                                 new SqlParameter("@invoice", invoice),
                        
                        };
                    cmd.Parameters.AddRange(param);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var aRCollection = new RCollection
                            {
                                Name = rdr["Name"].ToString(),
                                Address = rdr["Address"].ToString(),
                                Phone = rdr["Phone"].ToString(),
                                Amount = (decimal)rdr["Amount"],
                                DueAmount = (decimal)rdr["DueAmount"],
                                DuePercent = rdr["DuePercent"].ToString()
                            };

                            aRCollections.Add(aRCollection);
                            gridEntity = new GridEntity<RCollection>
                            {
                                Items = aRCollections,
                                TotalCount = aRCollections.Count
                            };
                        }
                    }
                }
                return gridEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Installment> GetAllInstalmentByInvoiceNo(string invoiceNo, CommonConnection connection)
        {
            string sql = string.Format(@"Select SI.InstallmentId,SI.SInvoice, SS.Number, SS.DueDate, SS.Status, SI.Amount, SUM(ISNULL(SC.ReceiveAmount,0))ReceiveAmount ,(Max(SI.Amount)-SUM(ISNULL(SC.ReceiveAmount,0))) DueAmount from Sale_Installment SI
                left join Sale_Collection SC on SC.InstallmentId=SI.InstallmentId
                left join (Select * from Sale_Installment I Where I.SInvoice='{0}') SS on SS.InstallmentId=SI.InstallmentId
                Where SI.SInvoice='{0}' 
                group by SI.InstallmentId, SS.Number, SS.DueDate,SS.Status,SI.Amount,SI.SInvoice", invoiceNo);

            // var data = Data<Installment>.GenericDataSource(sql);
            var data = connection.Data<Installment>(sql);
            return data;
        }

        public Azolution.Entities.Sale.Collection GetDownpaymentBySaleId(int saleId, CommonConnection connection)
        {
            string sql = string.Format(@"
Select tbl.SaleInvoice,isnull(tbl.ReceiveAmount,0)ReceiveAmount,isnull(Sale.DownPay,0)DownPay,isnull((Sale.DownPay-ReceiveAmount),0) DueAmount from Sale 
Left outer  join (
Select Sale_Collection.SaleInvoice,SUM(Sale_Collection.ReceiveAmount) as ReceiveAmount from Sale_Collection 
where SaleInvoice=(Select top 1 Invoice from Sale where SaleId={0}) and Sale_Collection.CollectionType = 3
group by SaleInvoice) tbl  on tbl.SaleInvoice = (Select top 1 Invoice from Sale where SaleId={0})
Where Sale.SaleId={0}", saleId);
            // var data = Data<Azolution.Entities.Sale.Collection>.GenericDataSource(sql).SingleOrDefault();
            var data = connection.Data<Azolution.Entities.Sale.Collection>(sql).SingleOrDefault();
            return data;
        }


        public string GetPreviousCode(DateTime dueDate, string saleInvoice, CommonConnection connection)
        {
            //var issueDate = dueDate.AddMonths(-1).ToString("yyyy/MM/dd");//worked
            var issueDate = dueDate.ToString("yyyy/MM/dd");

            //            string sql = string.Format(@"Select * From Sale_License Where SaleInvoice='{0}' 
            //            and DATENAME(MM, IssueDate) + ' ' + CAST(YEAR(IssueDate) AS VARCHAR(4)) = DATENAME(MM, dateadd(MM,-1,'{1}')) + ' ' + CAST(YEAR('{1}') AS VARCHAR(4))", saleInvoice, issueDate);
            // License data = connection.Data<License>.GenericDataSource(sql).SingleOrDefault();

            string sql = string.Format(@"Select * From Sale_License Where SaleInvoice='{0}' 
            and DATENAME(MM, IssueDate) + ' ' + CAST(YEAR(IssueDate) AS VARCHAR(4)) = DATENAME(MM, '{1}') + ' ' + CAST(YEAR('{1}') AS VARCHAR(4))", saleInvoice, issueDate);

            License data = connection.Data<License>(sql).SingleOrDefault();
            return data.Number;
        }
        public string GetPreviousCodeForReleaseLicense(ReleaseLisenceGenerate releaseObj)
        {
            string rv = "";

            var issueDate = releaseObj.DueDate.AddMonths(0).ToString("yyyy/MM/dd");
            string sql = string.Format(@"Select * From Sale_License Where SaleInvoice='{0}' 
            and DATENAME(MM, IssueDate) + ' ' + CAST(YEAR(IssueDate) AS VARCHAR(4)) = DATENAME(MM, '{1}') + ' ' + CAST(YEAR('{1}') AS VARCHAR(4))",
                releaseObj.SInvoice, issueDate);

            License data = Data<License>.DataSource(sql).SingleOrDefault();
            rv = data.Number;

            return rv;
        }



        public RatingCalculation GetDuePercentByInvoiceNo(string sInvoiceNo)
        {

            string sql = string.Format(@"Select tblTemp2.*,
             SUM(isnull(stcCur.ReceiveAmount,0)) as PaymentTillDate, 
            case when SUM(isnull(stcCur.ReceiveAmount,0)) > 0 then 
            (SUM(isnull(stcCur.ReceiveAmount,0))/RequiredReceiveAmountTillDate)*100
            else 0 end as TotalReceivePercentTillDate,
            (RequiredReceiveAmountTillDate-SUM(isnull(stcCur.ReceiveAmount,0))) as DueAmountTillDate
            ,case when OutStandingAmount > 0 then ((RequiredReceiveAmountTillDate-SUM(isnull(stcCur.ReceiveAmount,0)))*100)/OutStandingAmount else 0 end  as TotalDuePercentTillDate,
            SUM(isnull(stcCur.ReceiveAmount,0))ReceiveAmountTillDate
            from (
            Select tblTemp1.*, (Sum(ISNULL(SIC.Amount,0))+DownPay) as RequiredReceiveAmountTillDate 
            from (
            Select tSale.*,SUM(isnull(Sale_Collection.ReceiveAmount,0)) as PaidAmount,(NetPrice+DownPay-SUM(isnull(Sale_Collection.ReceiveAmount,0))) as OutStandingAmount,
            (SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100 as TotalReceivePercent,
            100-((SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100) as TotalDuePercent From (
            Select Invoice,sum(NetPrice)NetPrice,sum(DownPay) DownPay,sum((NetPrice+DownPay)) as Amount From Sale
            where Invoice = '{0}'  group by Sale.Invoice) tSale 
            left outer join Sale_Collection on Sale_Collection.SaleInvoice = tSale.Invoice 
            group by tSale.Invoice,NetPrice, DownPay,Amount) as tblTemp1
            left outer join Sale_Installment SIC on SIC.SInvoice = tblTemp1.Invoice and SIC.DueDate <= GETDATE()
            group by tblTemp1.Invoice,NetPrice,DownPay,tblTemp1.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent) tblTemp2
            left outer join Sale_Collection stcCur on stcCur.SaleInvoice = tblTemp2.Invoice and stcCur.DueDate<=GETDATE()
            group by tblTemp2.Invoice,NetPrice,DownPay,tblTemp2.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,RequiredReceiveAmountTillDate", sInvoiceNo);
            var data = Data<RatingCalculation>.DataSource(sql).SingleOrDefault();
            return data;
        }




        public List<License> SendSmsByCustomerRating(string condition, CommonConnection connection, Users user)
        {
            string sql = string.Format(@"Select SaleInvoice,LicenseId,Number,IssueDate,IsActive,IsSentSMS,LType From Sale_License
                        {0}", condition);
            //var data = Data<License>.GenericDataSource(sql);
            var data = connection.Data<License>(sql);
            return data;
        }

        public void UpdateAllLicensebySaleInvoice(string saleInvoice, CommonConnection connection)
        {
            string sql = string.Format(@"Update Sale_License Set IsActive=0,IsSentSMS=1 Where SaleInvoice = '{0}' and LType != 1", saleInvoice);
           connection.ExecuteNonQuery(sql);

        }


        public void SendSMSandUpdateLisenceData(License lisenceObj, string saleInvoice, DateTime issueDate, CommonConnection connection, decimal totalPayment, decimal totalDue, string simNumber, Azolution.Entities.Sale.Sale customerInfo)
        {
            StringBuilder qBuilder = new StringBuilder();
         
            try
            {
                var sql = "";
                var isDate = new DateTime(issueDate.Year, issueDate.Month, 1).AddDays(-1).ToString("MM/dd/yyyy");//To update previous Lisence info
              
                SmsManager _smsManager = new SmsManager();
                LicenseInfo licenseInfo = new LicenseInfo();
                licenseInfo.LType = 2;
                licenseInfo.ReceivedAmount = Math.Round(totalPayment,2);
                licenseInfo.DueAmount = Math.Round(totalDue,2);
                licenseInfo.Code = lisenceObj.Number;
                
                string smsText = _smsManager.GetSmsText(licenseInfo);

                qBuilder.Append(string.Format(@"Update Sale_License Set IsActive=0,IsSentSMS=1 Where SaleInvoice='{0}' and LType != 1 and
                    IssueDate <= '{1}';", saleInvoice, isDate));//Set Inactive to previous license


                var isSentSms = 0;
                if (lisenceObj.LType != 1)
                {
                    SaveSMS(lisenceObj.MobileNumber, qBuilder, smsText, simNumber);
                    isSentSms = 1;
                }
                else
                {
                    isSentSms = 0;
                }

                var isSmsEligible = customerInfo.ACustomer.IsSmsEligible;
                if (isSmsEligible == 1)//for Sending SMS copy to BM
                {
                    var generalSms = new GeneralSms();
                    generalSms.SmsType = 6;//BM SMS
                    generalSms.license = new LicenseInfo();
                    generalSms.license.LType = 2;
                    generalSms.license.Code = lisenceObj.Number;
                    generalSms.license.CustomerCode = customerInfo.ACustomer.CustomerCode;
                    generalSms.license.ReceivedAmount = Math.Round(customerInfo.ReceiveAmount,2);

                    var mobileNo = customerInfo.ACustomer.BranchSmsMobileNumber;
                    smsText = _smsManager.GetGeneralSmsText(generalSms);

                    SaveSMS(mobileNo, qBuilder, smsText, simNumber);

                }

                qBuilder.Append(string.Format(@"Update Sale_License Set IsActive=1, IsSentSMS={2} Where SaleInvoice='{0}' and LType != 1 and IssueDate='{1}';", saleInvoice, issueDate, isSentSms));//set active to present lisence



                if (qBuilder.ToString() != "")
                {
                    sql = "Begin " + qBuilder + " End;";
                    connection.ExecuteNonQuery(sql);
                }

            }
            catch (Exception)
            {
                throw new Exception("Failed to Send SMS !");
            }


        }

        private static void SaveSMS(string mobileNo, StringBuilder qBuilder, string smsText, string simNumber)
        {
            qBuilder.Append(
                string.Format(
                    "INSERT INTO SMSSent(SMSText,MobileNumber,[RequestDateTime],[Status],ReplyFor,SimNumber) VALUES(N'{0}','{1}','{2}',{3},{4},'{5}');",
                    smsText, mobileNo, DateTime.Now, 0, 0, simNumber));
        }

        #region Dashboard Data








        public GridEntity<RatingCalculation> GetDuePercentAndCustomerInfoForDashboardGrid(GridOptions options, string condition, string condition2, string orderby)
        {
            #region sql query


            string sql = string.Format(@"

Select tblTemp3.* from (Select tblTemp2.*, case when SaleTypeId=2 then PaidAmount else SUM(isnull(stcCur.ReceiveAmount,0)) end as PaymentTillDate, 
case when SaleTypeId=2 then PaidAmount else 
case when SUM(isnull(stcCur.ReceiveAmount,0)) > 0 then 
(SUM(isnull(stcCur.ReceiveAmount,0))/ReceiveAmountTillDate)*100
else 0 end end as TotalReceivePercentTillDate,
case when SaleTypeId=2 then 0 else
(100 - case when SUM(isnull(stcCur.ReceiveAmount,0)) > 0 then 
(SUM(isnull(stcCur.ReceiveAmount,0))/ReceiveAmountTillDate)*100
else 0 end) end as TotalDuePercentTillDateOld,
case when OutStandingAmount > 0 then ((ReceiveAmountTillDate-SUM(isnull(stcCur.ReceiveAmount,0)))*100)/OutStandingAmount else 0 end  as TotalDuePercentTillDate,
(ReceiveAmountTillDate-SUM(isnull(stcCur.ReceiveAmount,0))) as DueAmountTillDate
from (
Select tblTemp1.*, (Sum(ISNULL(SIC.Amount,0))+DownPay) as ReceiveAmountTillDate 
from (

Select Invoice,NetPrice,DownPay,(NetPrice+DownPay) as Amount,SUM(isnull(Sale_Collection.ReceiveAmount,0)) as PaidAmount,
(NetPrice+DownPay-SUM(isnull(Sale_Collection.ReceiveAmount,0))) as OutStandingAmount,
(SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100 as TotalReceivePercent,
100-((SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100) as TotalDuePercent,
sc.Name,sc.Address,sc.Phone,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,sc.CompanyId

from Sale 
left outer join Sale_Collection on Sale_Collection.SaleInvoice = Sale.Invoice 
left outer join Sale_Customer sc on Sale.CustomerId = sc.CustomerId and Sale.IsActive = 1
left outer join Sale_Product SP on Sale.ModelId = SP.ModelId
{0}
group by Invoice,NetPrice,DownPay,sc.Name,sc.Address,sc.Phone,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,sc.CompanyId) as tblTemp1
left outer join Sale_Installment SIC on SIC.SInvoice = tblTemp1.Invoice and SIC.DueDate <= GETDATE()
group by tblTemp1.Invoice,NetPrice,DownPay,tblTemp1.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,tblTemp1.Name,tblTemp1.Address,tblTemp1.Phone,tblTemp1.CustomerCode,tblTemp1.ProductName,tblTemp1.Model,SaleTypeId,tblTemp1.IsRelease,tblTemp1.CompanyId) tblTemp2
left outer join Sale_Collection stcCur on stcCur.SaleInvoice = tblTemp2.Invoice and stcCur.DueDate<=GETDATE()
group by tblTemp2.Invoice,NetPrice,DownPay,tblTemp2.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,ReceiveAmountTillDate,tblTemp2.Name,tblTemp2.Address,tblTemp2.Phone,tblTemp2.CustomerCode,tblTemp2.ProductName,tblTemp2.Model,SaleTypeId,tblTemp2.IsRelease,tblTemp2.CompanyId) tblTemp3 {1}", condition, condition2);

            #endregion
            //var data = Data<RatingCalculation>.GenericDataSource(sql).SingleOrDefault();
            var data = Kendo<RatingCalculation>.Grid.GenericDataSource(options, sql, orderby);
            return data;
        }
        #endregion

        public void SaveCashPayment(Azolution.Entities.Sale.Sale aSale, CommonConnection connectionAfterReturnId, int userId)
        {
            string insertQuery = string.Format(@" Insert Into Sale_Collection(SaleInvoice,InstallmentNo,TransectionType,TransectionId,
                                    ReceiveAmount,PayDate,EntryDate,InstallmentId,Flag,IsActive,CollectedBy,DueAmount,CollectionType) 
                        Values('{0}',{1},{2},'{3}',{4},'{5}','{6}',{7},{8},{9},{10},{11},{12});",
                   aSale.Invoice, 0, 1, 1, aSale.Price, DateTime.Now, DateTime.Now, 0, 1, 1, userId, 0, 2);

            connectionAfterReturnId.ExecuteNonQuery(insertQuery);
        }

        public GridEntity<ReleaseLisenceGenerate> GetCustomerForReleaseLisenceGridData(GridOptions options, string condition)
        {

            string sql = string.Format(@"select tbl3.* from (
            Select tbl2.*,Sale.SaleId,Sale.CustomerId,isnull(Sale.IsRelease,0)IsRelease,SC.CustomerCode,SC.IsUpgraded As IsCustomerUpgraded ,BranchCode,SP.PackageType,
             SC.Name,SC.Phone,SC.Phone2,SC.CompanyId,SC.BranchId,IIM ,BranchSmsMobileNumber,IsSmsEligible ,Sale.State 
             from (Select tbl.*
            From(
            select tmpIns.*,Sale_Installment.InstallmentId,Sale_Installment.Status,Sale_Installment.DueDate
             from(Select SInvoice,MAX(Sale_Installment.Number)Number from Sale_Installment
            group by SInvoice) tmpIns
             inner join Sale_Installment on 
            tmpIns.SInvoice=Sale_Installment.SInvoice and tmpIns.Number=Sale_Installment.Number --and Sale_Installment.Status=1
            )tbl 
            inner join Sale_Installment SI on tbl.SInvoice = SI.SInvoice and tbl.Number = SI.Number --And SI.Status=1 
            )tbl2
            inner join Sale on Sale.Invoice = tbl2.SInvoice 
            left outer join Sale_Customer SC on  SC.CustomerId = Sale.CustomerId
            left outer join Sale_Product SP on SP.ModelId= Sale.ModelId
            left outer join Branch on Branch.BRANCHID=SC.BranchId) tbl3
            left outer join (
            Select tSale.*,SUM(isnull(Sale_Collection.ReceiveAmount,0)) as PaidAmount,(NetPrice+DownPay-SUM(isnull(Sale_Collection.ReceiveAmount,0))) as OutStandingAmount,
            case when SUM(isnull(Sale_Collection.ReceiveAmount,0)) = 0 then 0 else (SUM(isnull(Sale_Collection.ReceiveAmount,0))/(isnull(NetPrice,0)+isnull(DownPay,0)))*100 end as TotalReceivePercent,
            100-case when SUM(isnull(Sale_Collection.ReceiveAmount,0)) = 0 then 0 else ((SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100) end as TotalDuePercent From (
            Select Invoice,sum(NetPrice)NetPrice,sum(DownPay) DownPay,sum((NetPrice+DownPay)) as Amount From Sale
           -- where Invoice = '10-15-1767'  
            group by Sale.Invoice) tSale 
            left outer join Sale_Collection on Sale_Collection.SaleInvoice = tSale.Invoice 
            group by tSale.Invoice,NetPrice, DownPay,Amount)tb on tb.Invoice = SInvoice

             where tbl3.PackageType =1 and tbl3.State=5 and tbl3.IsRelease != 1 and tb.TotalDuePercent <= 1 {0}", condition);
            var data = Kendo<ReleaseLisenceGenerate>.Grid.GenericDataSource(options, sql, "Name");
            return data;
        }

        #region backup GenerateReleaseLicenseAndSendSms
        //public string GenerateReleaseLicenseAndSendSms(ReleaseLisenceGenerate strobjReleaseLicenseInfo, int varificationType)
        //{
        //    CommonConnection connection = new CommonConnection();
        //    connection.BeginTransaction();
        //    try
        //    {
        //        LicenceService codeGenerator = new LicenceService();

        //        var previousCode = GetPreviousCodeForReleaseLicense(strobjReleaseLicenseInfo.DueDate, strobjReleaseLicenseInfo.SInvoice);
        //        var lType = 2;//to generate release type license
        //        var releaseLicenseCode = "";
        //        if (strobjReleaseLicenseInfo.IsCustomerUpgraded == 1)
        //        {
        //            releaseLicenseCode = codeGenerator.GenerateLicence(strobjReleaseLicenseInfo.Phone, previousCode, lType);
        //        }
        //        else
        //        {

        //            string brCode = "";
        //            string cusCode = "";
        //            GetCompanyCodeAndBranchCodeFromItemSLNo(strobjReleaseLicenseInfo.SInvoice, ref brCode, ref cusCode);

        //            string customerCodeParam = cusCode == "" ? strobjReleaseLicenseInfo.CustomerCode.Substring(strobjReleaseLicenseInfo.CustomerCode.Length - 4) : cusCode;
        //            string branchCodeParam = brCode == "" ? strobjReleaseLicenseInfo.BranchCode.Substring(strobjReleaseLicenseInfo.BranchCode.Length - 4) : brCode;

        //            releaseLicenseCode = codeGenerator.GenerateLicence(customerCodeParam, branchCodeParam, lType);
        //        }


        //        var objLicense = new License();
        //        objLicense.Number = releaseLicenseCode;
        //        objLicense.IssueDate = strobjReleaseLicenseInfo.DueDate;
        //        objLicense.SaleInvoice = strobjReleaseLicenseInfo.SInvoice;
        //        objLicense.LType = 3;//for Release License
        //        objLicense.IsSMSSent = 1;
        //        objLicense.Status = 1;
        //        objLicense.MobileNumber = strobjReleaseLicenseInfo.Phone;

        //        objLicense.VarificationType = varificationType;

        //        SaveLisenceInfo(objLicense, connection);

        //        UpdateSaleWhenGetReleasedLicense(objLicense, connection);
        //        connection.CommitTransaction();


        //        SendSMSandUpdateForReleaseLicense(objLicense, strobjReleaseLicenseInfo.SInvoice, strobjReleaseLicenseInfo.DueDate);




        //    }
        //    catch (Exception ex)
        //    {
        //        connection.RollBack();
        //        return ex.Message;

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return "Success";

        //}
        #endregion

        public string GenerateReleaseLicenseAndSendSms(ReleaseLisenceGenerate strobjReleaseLicenseInfo, int varificationType, string simNumber)
        {
            CommonConnection connection = new CommonConnection();
            connection.BeginTransaction();
            try
            {
                LicenceService codeGenerator = new LicenceService();

               
                var lType = 2;//to generate release type license
                var releaseLicenseCode = "";
                var previousCode = "";
                if (strobjReleaseLicenseInfo.IsCustomerUpgraded == 1)
                {
                     previousCode = GetPreviousCodeForReleaseLicense(strobjReleaseLicenseInfo);
                    releaseLicenseCode = codeGenerator.GenerateLicence(strobjReleaseLicenseInfo.Phone, previousCode, lType);
                }
                else
                {

                    string brCode = "";
                    string cusCode = "";
                    GetCompanyCodeAndBranchCodeFromItemSLNo(strobjReleaseLicenseInfo.SInvoice, ref brCode, ref cusCode);

                    string customerCodeParam = cusCode == "" ? strobjReleaseLicenseInfo.CustomerCode.Substring(strobjReleaseLicenseInfo.CustomerCode.Length - 4) : cusCode;
                    string branchCodeParam = brCode == "" ? strobjReleaseLicenseInfo.BranchCode.Substring(strobjReleaseLicenseInfo.BranchCode.Length - 4) : brCode;

                    previousCode = GetPreviousCodeForReleaseLicenseV2(strobjReleaseLicenseInfo, customerCodeParam, branchCodeParam, codeGenerator);
                   // releaseLicenseCode = codeGenerator.GenerateLicence(customerCodeParam, branchCodeParam, lType);
                    releaseLicenseCode = codeGenerator.GenerateLicence(customerCodeParam, previousCode, lType);
                }


                var objLicense = new License();
                objLicense.Number = releaseLicenseCode;
                objLicense.IssueDate = strobjReleaseLicenseInfo.DueDate;
                objLicense.SaleInvoice = strobjReleaseLicenseInfo.SInvoice;
                objLicense.LType = 3;//for Release License
                objLicense.IsSMSSent = 1;
                objLicense.Status = 1;
                objLicense.MobileNumber = strobjReleaseLicenseInfo.Phone2;

                objLicense.VarificationType = varificationType;

                SaveLisenceInfo(objLicense, connection);

                UpdateSaleWhenGetReleasedLicense(objLicense, connection);
                connection.CommitTransaction();


                SendSMSandUpdateForReleaseLicense(objLicense, strobjReleaseLicenseInfo, simNumber);


            }
            catch (Exception ex)
            {
                connection.RollBack();
                return ex.Message;

            }
            finally
            {
                connection.Close();
            }
            return "Success";

        }

        private string GetPreviousCodeForReleaseLicenseV2(ReleaseLisenceGenerate strobjReleaseLicenseInfo, string customerCode, string branchCode, LicenceService codeGenerator)
        {
            string previousCode = "";

            var initialCode = codeGenerator.GenerateLicence(customerCode, branchCode, 0);
            previousCode = initialCode;

            int count = Convert.ToInt32(branchCode.Substring(strobjReleaseLicenseInfo.BranchCode.Length - 2));

            if (count > 0)
            {
                for (int i = 1; i < count; i++)
                {
                    previousCode = codeGenerator.GenerateLicence(customerCode, previousCode, 1);
                } 
            }
            return previousCode;
        }

        private void UpdateSaleWhenGetReleasedLicense(License objLicense, CommonConnection connection)
        {
            try
            {
                string sqlQuery = string.Format(@"Update Sale set IsRelease=1 where Invoice='{0}'", objLicense.SaleInvoice);
                connection.ExecuteNonQuery(sqlQuery);
            }
            catch (Exception)
            {

                throw new Exception("Failed to update Sale when Released license");
            }
        }
        public void SendSMSandUpdateForReleaseLicense(License lisenceObj,ReleaseLisenceGenerate releaseObj, string simNumber)
        {
            StringBuilder qBuilder = new StringBuilder();
            CommonConnection connection = new CommonConnection();
            try
            {
                var sql = "";
            
                SmsManager _smsManager = new SmsManager();
                LicenseInfo licenseInfo = new LicenseInfo();
                licenseInfo.LType = 3;
                licenseInfo.Code = lisenceObj.Number;

                string smsText = _smsManager.GetSmsText(licenseInfo);

                qBuilder.Append(string.Format(@"Update Sale_License Set IsActive=0,IsSentSMS=1 Where SaleInvoice='{0}' and
                    IssueDate <= '{1}' and Number != '{2}';", releaseObj.SInvoice, releaseObj.DueDate, lisenceObj.Number));//Set Inactive to previous license

                SaveSMS(lisenceObj.MobileNumber, qBuilder, smsText, simNumber);


                var isSmsEligible = releaseObj.IsSmsEligible;
                if (isSmsEligible == 1)//for Sending SMS copy to BM
                {
                    var generalSms = new GeneralSms();
                    generalSms.SmsType = 6;//BM SMS
                    generalSms.license = new LicenseInfo();
                    generalSms.license.LType = 3;
                    generalSms.license.Code = lisenceObj.Number;
                    generalSms.license.CustomerCode = releaseObj.CustomerCode;
                    generalSms.license.ReceivedAmount =0;

                    var mobileNo = "";

                    smsText = _smsManager.GetGeneralSmsText(generalSms);

                    SaveSMS(mobileNo, qBuilder, smsText, simNumber);
                }

                if (qBuilder.ToString() != "")
                {
                    sql = "Begin " + qBuilder + " End;";
                    connection.ExecuteNonQuery(sql);
                }

            }
            catch (Exception)
            {
                throw new Exception("Failed to Send SMS !");
            }
            finally
            {
                connection.Close();
            }


        }

        public GridEntity<CustomerWithLicenseCode> GetCustomerWithCodeSummary(GridOptions options, string condition)
        {


            string query = string.Format(@"Select Sale_Customer.Name,Sale_Customer.Phone,Sale_Customer.Phone2,Sale_License.IssueDate,
            Sale_License.Number,Sale_Customer.CustomerCode,Sale_License.LType,Sale.Invoice As SaleInvoice,Sale.DownPay,Branch.BranchCode
             from Sale_License
            left outer join Sale on Sale.Invoice = Sale_License.SaleInvoice
            left outer join Sale_Product SP on SP.ModelId=Sale.ModelId
            left outer join Sale_Customer on Sale_Customer.CustomerId = Sale.CustomerId
            left outer join Branch on Branch.BRANCHID = Sale_Customer.BranchId
            {0}", condition);
            return Kendo<CustomerWithLicenseCode>.Grid.DataSource(options, query, "Name");

        }
        public string ResendLicenseCodeSms(CustomerWithLicenseCode objResendSms)
        {
            string res = "";
            string smsText = "";
            decimal totalPayment = 0;
            decimal totalDue = 0;
            decimal downpay = 0;
            CommonConnection connection = new CommonConnection();
            try
            {

                if (objResendSms.LType != 3)
                {
                    var duePercent = GetDuePercentByInvoiceNo(objResendSms.SaleInvoice);
                    totalPayment = duePercent.PaidAmount;
                    totalDue = duePercent.OutStandingAmount;
                    downpay = objResendSms.DownPay;
                }
                SmsManager _smsManager = new SmsManager();
                LicenseInfo licenseInfo = new LicenseInfo();
                CustomerService _customerService = new CustomerService();
                SalesRepresentatorDataService _representatorDatsService = new SalesRepresentatorDataService();

                licenseInfo.LType = objResendSms.LType;
                licenseInfo.Code = objResendSms.Number;
                licenseInfo.ReceivedAmount = totalPayment;
                licenseInfo.DueAmount = totalDue;
                licenseInfo.CustomerName = objResendSms.Name;
                licenseInfo.CustomerCode = objResendSms.CustomerCode;
                licenseInfo.DownPay = downpay;

                smsText = _smsManager.GetSmsText(licenseInfo);

                string simNumber = objResendSms.SimNumber;

                string query = string.Format(@"INSERT INTO SMSSent(SMSText,MobileNumber,[RequestDateTime],[Status],ReplyFor,SimNumber) VALUES(N'{0}','{1}','{2}',{3},{4},'{5}')",
                    smsText, objResendSms.Phone2, objResendSms.IssueDate, 0, 0, simNumber);
                connection.ExecuteNonQuery(query);


                var getSaleInfo = _customerService.GetCustomerAndSaleInfoByCustomerCode(string.Format(" Where CustomerCode = '{0}'", objResendSms.CustomerCode));

                //Dealer SMS Sent If eligible
                if (getSaleInfo!= null && getSaleInfo.ACustomer.TypeId == 3 && getSaleInfo.SalesRepId != null && objResendSms.LType == 1) //type = 3, Dealer Sale
                {
                    var repSaleInfo = _representatorDatsService.GetAllSalesRepresentatorById(getSaleInfo.SalesRepId);
                    if (repSaleInfo.IsSalesRepSmsSent == 1)
                    {
                        string sql = "";
                        var generalSms = new GeneralSms();
                        generalSms.SmsType = 7;//Dealer SMS for Special Package (Initial license for Dealer), Type = 7
                        generalSms.license = new LicenseInfo();
                        generalSms.license.LType = 1;
                        generalSms.license.Code = objResendSms.Number;
                        generalSms.license.CustomerCode = objResendSms.CustomerCode;
                        generalSms.license.DownPay = downpay;

                        smsText = _smsManager.GetGeneralSmsText(generalSms);

                          sql = string.Format(@"INSERT INTO SMSSent(SMSText,MobileNumber,[RequestDateTime],[Status],ReplyFor,SimNumber) VALUES(N'{0}','{1}','{2}',{3},{4},'{5}')",
                    smsText, repSaleInfo.SalesRepSmsMobNo, objResendSms.IssueDate, 0, 0, simNumber);
                        connection.ExecuteNonQuery(sql);
                    }
                }

                res = "Success";

            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                connection.Close();

            }
            return res;
        }

        public void DownpaymentCollection(PaymentReceivedInfo objDownPayCollection, int userId, CommonConnection connection, int saleId)
        {
            //temporary downpayment collection

            string sql = "";
            var entryDate = DateTime.Now;

            try
            {
                if (objDownPayCollection.CollectionId == 0)
                {
                    sql = string.Format(@"Insert Into DownpamentCollection_tmp (SaleInvoice,ReceiveAmount,CollectionType,PaymentType,TransectionId,PayDate,EntryDate,SaleId) 
                        Values('{0}',{1},{2},{3},'{4}','{5}','{6}',{7})", objDownPayCollection.SaleInvoice, objDownPayCollection.ReceiveAmount, objDownPayCollection.CollectionType, objDownPayCollection.PaymentType,
                           objDownPayCollection.TransectionId, objDownPayCollection.PayDate, entryDate, saleId);
                }
                else
                {
                    sql = string.Format(@"Update DownpamentCollection_tmp Set SaleInvoice='{0}',ReceiveAmount={1},CollectionType={2},PaymentType={3},TransectionId='{4}',PayDate='{5}',EntryDate='{6}' 
                        Where CollectionId = {7}", objDownPayCollection.SaleInvoice, objDownPayCollection.ReceiveAmount, objDownPayCollection.CollectionType, objDownPayCollection.PaymentType,
                           objDownPayCollection.TransectionId, objDownPayCollection.PayDate, entryDate, objDownPayCollection.CollectionId);
                }

                connection.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {

                throw new Exception("Failed to Save Down Payment Info");
            }

        }
        public int CustomerIsActiveBySaleId(string invoice, CommonConnection connection)
        {
            // CommonConnection connection  = new CommonConnection();
            string sql = string.Format(@"Select top 1 Sale_Customer.IsActive from Sale_Customer
            left outer join Sale on Sale.CustomerId = Sale_Customer.CustomerId
            where Sale.Invoice='{0}'", invoice);
            return connection.GetScaler(sql);
        }

        public Azolution.Entities.Sale.Collection GetDownpaymentByInvoiceNo(string invoiceNo)
        {

            string sql = string.Format(@" Select tblSale.Invoice,isnull(tblCollection.ReceiveAmount,0)ReceiveAmount,tblSale.DownPay,
                         (isnull(tblSale.DownPay,0)- isnull(tblCollection.ReceiveAmount,0))DueAmount From (
                         Select SUM(isnull(ReceiveAmount,0))ReceiveAmount,SaleInvoice 
                         From Sale_Collection Where SaleInvoice='{0}' And CollectionType=3
                         group by SaleInvoice) tblCollection
                         full outer join (
                         Select SUM(DownPay)DownPay,Invoice From Sale Where Invoice='{0}'
                         group by Invoice) tblSale on tblSale.Invoice=tblCollection.SaleInvoice", invoiceNo);


            var data = Data<Azolution.Entities.Sale.Collection>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public Azolution.Entities.Sale.Collection GetAllReceiveAmountByInvoice(string invoiceNo)
        {
            string sql = string.Format(@"Select tblSale.Invoice,isnull(tblCollection.ReceiveAmount,0)ReceiveAmount,tblSale.Price,
                         (isnull(tblSale.Price,0)- isnull(tblCollection.ReceiveAmount,0))DueAmount From (
                         Select SUM(isnull(ReceiveAmount,0))ReceiveAmount,SaleInvoice 
                         From Sale_Collection Where SaleInvoice='{0}' 
                         group by SaleInvoice) tblCollection
                         full outer join (
                         Select SUM(Price)Price,Invoice From Sale Where Invoice='{0}'
                         group by Invoice) tblSale on tblSale.Invoice=tblCollection.SaleInvoice", invoiceNo);


            var data = Data<Azolution.Entities.Sale.Collection>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public Azolution.Entities.Sale.Sale GetExistingSalesInformation(int customerId)
        {
            var query = string.Format(@"SELECT * from Sale where CustomerId={0} And (IIM is NULL or IIM =0) and IsRelease=0", customerId);
            var data = Data<Azolution.Entities.Sale.Sale>.GenericDataSource(query);
            return data.FirstOrDefault();
        }

        public Installment GetInstallmentInfo(string saleInvoice, int saleTypeId)
        {
            string condition = "";
            if (saleTypeId == 1)
            {
                condition = " And Status=0";
            }
            else
            {
                condition = " And Status <> 1";
            }
            var sql = string.Format(@" Select Top 1 * From Sale_Installment Where SInvoice='{0}' {1}", saleInvoice,condition);
            var data = Data<Installment>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public string GetExistInitialCode(string saleInvoice, string dueDate, string initialCode)
        {
            string sql = string.Format(@" Select * From Sale_License Where SaleInvoice='{0}' And IssueDate='{1}' And Number='{2}'", saleInvoice, dueDate, initialCode);
            var data = Data<License>.DataSource(sql).SingleOrDefault();
            return data != null ? data.Number : string.Empty;
        }

        public void UpdateSaleForDPCollection(string saleInvoice, CommonConnection connection)
        {
            try
            {
                string sql = string.Format(@"Update Sale Set IsDownPayCollected ='True' Where Invoice='{0}' And (IsDownPayCollected='false' or IsDownPayCollected is null )", saleInvoice);
                connection.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                throw new Exception("Error ! Duril Update Sale For DP Collection");
            }
        }

        public decimal GetTotalReceivedAmount(string invoice)
        {
            string sql = string.Format(@"Select SUM(ReceiveAmount)ReceiveAmount From Sale_Collection
                        Where SaleInvoice='{0}'",invoice);

            var receivedAmt = Data<CollectionInfo>.DataSource(sql).SingleOrDefault().ReceiveAmount;
            return receivedAmt;
        }

        public GridEntity<CollectionDto> GetCollectionHistoryByInvoice(GridOptions options, string invoiceNo)
        {
            string query = string.Format(@"Select C.SaleInvoice,C.InstallmentNo,C.TransectionId,C.TransectionType,C.ReceiveAmount,
                C.DueAmount,C.PayDate,C.DueDate,C.EntryDate,C.CollectionType,
                Case when C.CollectionType=3 then 'Down Pay' else 'Installment' end CollectionTypeName
                From Sale_Collection C Where SaleInvoice='{0}'", invoiceNo);
            return Kendo<CollectionDto>.Grid.DataSource(options, query, "InstallmentNo");
        }

        public Product GetPackageType(string saleInvoice)
        {
            string sql = string.Format(@"Select SP.PackageType From Sale
            inner join Sale_Product SP on SP.ModelId=Sale.ModelId
            Where Invoice='{0}'", saleInvoice);
            var data = Data<Product>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public int CheckDuplicateTransactionId(string txtId)
        {
            var conn = new CommonConnection();
            var query = "";
            query = string.Format(@"Select COUNT(*) from Sale_Collection where TransectionId = '{0}' and TransectionId != '' and TransectionId is not null ", txtId.Trim());
            var dataCount = conn.GetScaler(query);
            return dataCount;
        }
    }
}

