using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;
using Utilities;

namespace Azolution.Dashboard.DataService
{
    public class DashboardDataService
    {
        public List<PendingCollectionChart> GetAllPendingCollectionsForDashBoard(object user, string topParam, string groupParam, string whereCondition, DateTime? fromDate, DateTime? toDate)
        {
            //This mathod works for Monthly business trend
            try
            {

                var currentDate = DateTime.Now;

                var startMonth = currentDate.AddMonths(-6);
                // var startDate = "1/" + startMonth.Month + "/" + startMonth.Year;
                var startDate = startMonth.Month + "/1/" + startMonth.Year;
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

                string query = string.Format(@"Select SUM(tblAll.SalesPrice) as SalesPrice, SUM(tblAll.InstallmentReceiveAmount) as InstallmentReceiveAmount,SUM(tblAll.DownPaymentReceiveAmount) as DownPaymentReceiveAmount, WarantyStartMonth,SUM(tblAll.OutStanding) as OutStanding from (
select tblFinal.RowNumber,tblFinal.CompanyId,tblFinal.BranchId,tblFinal.SalesPrice,tblFinal.InstallmentReceiveAmount,tblFinal.DownPaymentReceiveAmount,tblFinal.WarantyStartMonth,(tblFinal.SalesPrice-tblFinal.InstallmentReceiveAmount-tblFinal.DownPaymentReceiveAmount) as OutStanding --tblFinal.SalesPrice-tblFinal.ReceiveAmount+Sum(isnull(tbl4.OutStanding,0)) as OutStanding 

from (
Select CompanyId,BranchId,Sum(SalesPrice) as SalesPrice,Sum(InstallmentReceiveAmount) as InstallmentReceiveAmount,Sum(DownPaymentReceiveAmount) as DownPaymentReceiveAmount,WarantyStartMonth,(Sum(SalesPrice)-Sum(InstallmentReceiveAmount)-Sum(DownPaymentReceiveAmount)) as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth desc) AS RowNumber from (
select * from (
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,sum(isnull(Sale.DownPay + Sale.NetPrice,0)) as SalesPrice,0 as InstallmentReceiveAmount,0 as DownPaymentReceiveAmount,DATENAME(MM, Sale.WarrantyStartDate) + ' ' + CAST(YEAR(Sale.WarrantyStartDate) AS VARCHAR(4)) as WarantyStartMonth
from Sale_Customer
left outer join Sale on Sale.CustomerId = Sale_Customer.CustomerId and Cast(WarrantyStartDate as date) between '{0}' and '{1}' and Sale.IsActive =1
where Sale.WarrantyStartDate is not null
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, Sale.WarrantyStartDate) + ' ' + CAST(YEAR(Sale.WarrantyStartDate) AS VARCHAR(4))) as tbl1
union all
select * from (
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,0 as SalesPrice,Sum(ReceiveAmount) as InstallmentReceiveAmount,0 as DownPaymentReceiveAmount,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4)) as PayDate
from Sale_Collection
inner join Sale on Sale.Invoice = Sale_Collection.SaleInvoice 
inner join Sale_Customer on Sale_Customer.CustomerId = Sale.CustomerId
where Cast(PayDate as date) between '{0}' and '{1}' and CollectionType in (1,2)
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4))
union all
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,0 as SalesPrice,0 as InstallmentReceiveAmount,Sum(ReceiveAmount) as DownPaymentReceiveAmount,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4)) as PayDate
from Sale_Collection
inner join Sale on Sale.Invoice = Sale_Collection.SaleInvoice  and Sale.IsActive =1 and Sale.State = 5
inner join Sale_Customer on Sale_Customer.CustomerId = Sale.CustomerId
where Cast(PayDate as date) between '{0}' and '{1}' and CollectionType in (3)
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4))

) as tbl2 ) as tbl3 group by CompanyId,BranchId,WarantyStartMonth) tblFinal
left outer join (
Select CompanyId,BranchId,Sum(SalesPrice) as SalesPrice,Sum(InstallmentReceiveAmount) as InstallmentReceiveAmount,Sum(DownPaymentReceiveAmount) as DownPaymentReceiveAmount,WarantyStartMonth,(Sum(SalesPrice)-Sum(InstallmentReceiveAmount)-Sum(DownPaymentReceiveAmount)) as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth desc) AS RowNumber from (
select * from (
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,sum(isnull(Sale.DownPay + Sale.NetPrice,0)) as SalesPrice,0 as InstallmentReceiveAmount,0 as DownPaymentReceiveAmount,DATENAME(MM, Sale.WarrantyStartDate) + ' ' + CAST(YEAR(Sale.WarrantyStartDate) AS VARCHAR(4)) as WarantyStartMonth
from Sale_Customer
left outer join Sale on Sale.CustomerId = Sale_Customer.CustomerId and Cast(WarrantyStartDate as date) between '{0}' and '{1}'
where Sale.WarrantyStartDate is not null
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, Sale.WarrantyStartDate) + ' ' + CAST(YEAR(Sale.WarrantyStartDate) AS VARCHAR(4))) as tbl1
union all
select * from (
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,0 as SalesPrice,Sum(ReceiveAmount) as InstallmentReceiveAmount,0 as DownPaymentReceiveAmount,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4)) as PayDate
from Sale_Collection
inner join Sale on Sale.Invoice = Sale_Collection.SaleInvoice
inner join Sale_Customer on Sale_Customer.CustomerId = Sale.CustomerId
where Cast(PayDate as date) between '{0}' and '{1}' and CollectionType in (1,2)
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4))
union all
Select Sale_Customer.CompanyId,Sale_Customer.BranchId,0 as SalesPrice,0 as InstallmentReceiveAmount,Sum(ReceiveAmount) as DownPaymentReceiveAmount,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4)) as PayDate
from Sale_Collection
inner join Sale on Sale.Invoice = Sale_Collection.SaleInvoice and Sale.IsActive =1 and Sale.State = 5
inner join Sale_Customer on Sale_Customer.CustomerId = Sale.CustomerId
where Cast(PayDate as date) between '{0}' and '{1}' and CollectionType in (3)
group by Sale_Customer.CompanyId,Sale_Customer.BranchId,DATENAME(MM, PayDate) + ' ' + CAST(YEAR(PayDate) AS VARCHAR(4))

) as tbl2 ) as tbl3 group by CompanyId,BranchId,WarantyStartMonth
) tbl4 on tbl4.RowNumber < tblFinal.RowNumber and tbl4.CompanyId = tblFinal.CompanyId 
group by tblFinal.RowNumber,tblFinal.CompanyId,tblFinal.BranchId,tblFinal.SalesPrice,tblFinal.InstallmentReceiveAmount,tblFinal.DownPaymentReceiveAmount,tblFinal.WarantyStartMonth,tblFinal.OutStanding
 ) tblAll {3}
 
 group by WarantyStartMonth 
  --order by WarantyStartMonth desc 
order by CONVERT(datetime, WarantyStartMonth, 110)  ", startDate, lastDate, topParam, whereCondition, groupParam);
                var data = Data<PendingCollectionChart>.DataSource(query);
                return data;

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
                var startDate = startMonth.Month + "/1/" + startMonth.Year;
                var lastDate = currentDate.Month + "/" + currentDate.Day + "/" + currentDate.Year;

                if (fromDate != null)
                {
                    startDate = fromDate.ToString();
                }
                if (toDate != null)
                {
                    lastDate = toDate.ToString();
                }

                var firstDayOfYear = "01/01/" + Convert.ToDateTime(startDate).Year;

                string query = string.Format(@"

--with mycte1 AS ( SELECT CAST('{0}' AS DATETIME) DateValue  
                                --UNION ALL SELECT  DateValue + 1 FROM    mycte1  
                              --WHERE   DateValue + 1 <= CAST('{1}' AS DATETIME))                                                            

--select distinct 
--convert(datetime,(cast(YEAR(DateValue) as varchar(4)) + '-' + cast(Month(DateValue) as varchar(4)) + '-' + '01'),101) as DateValue,
--isnull(ReceiveAmount,0) as ReceiveAmount,isnull(OutStanding,0) as OutStanding,ISNULL(WarantyStartMonth,DATENAME(MM, DateValue) + ' ' + CAST(YEAR(DateValue) AS VARCHAR(4))) as WarantyStartMonth
--from mycte1
--left outer join (

Select tblFinal.ReceiveAmount as OutStanding,
--Sum(tblJoinForOutStanding.OutStanding) as OutStanding,
DATENAME(MM, tblFinal.WarantyStartMonth) + ' ' + CAST(YEAR(tblFinal.WarantyStartMonth) AS VARCHAR(4)) as WarantyStartMonth from (


select ReceiveAmount,WarantyStartMonth,ReceiveAmount as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (

Select sum(isnull(ReceiveAmount,0)) as ReceiveAmount,
convert(datetime,(cast(YEAR(Sale_Collection.PayDate) as varchar(4)) + '-' + cast(Month(Sale_Collection.PayDate) as varchar(4)) + '-' + '01'),101) as WarantyStartMonth
 from Sale_Collection
left outer join Sale on Sale.Invoice = Sale_Collection.SaleInvoice and Sale.IsActive = 1 and Sale.State = 5 {3}
where Sale_Collection.PayDate is not null and Cast(PayDate as date) between '{0}' and '{1}'

group by convert(datetime,(cast(YEAR(PayDate) as varchar(4)) + '-' + cast(Month(PayDate) as varchar(4)) + '-' + '01'),101)) tbl1) tblFinal


left outer join (

select ReceiveAmount,WarantyStartMonth,OutStanding as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (

select ReceiveAmount,WarantyStartMonth,case when ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) =1 then ReceiveAmount + isnull(OpeningAmount,0) else ReceiveAmount end as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (

Select sum(isnull(ReceiveAmount,0)) as ReceiveAmount,
convert(datetime,(cast(YEAR(Sale_Collection.PayDate) as varchar(4)) + '-' + cast(Month(Sale_Collection.PayDate) as varchar(4)) + '-' + '01'),101) as WarantyStartMonth
 from Sale_Collection
left outer join Sale on Sale.Invoice = Sale_Collection.SaleInvoice and Sale.IsActive = 1 and Sale.State = 5 {3}
where Sale_Collection.PayDate is not null and Cast(PayDate as date) between '{0}' and '{1}'

group by convert(datetime,(cast(YEAR(PayDate) as varchar(4)) + '-' + cast(Month(PayDate) as varchar(4)) + '-' + '01'),101)) tbl1
cross join (Select sum(isnull(ReceiveAmount,0)) as OpeningAmount
 from Sale_Collection
left outer join Sale on Sale.Invoice = Sale_Collection.SaleInvoice and Sale.IsActive = 1 and Sale.State = 5 {3}
where Sale_Collection.PayDate is not null and Cast(PayDate as date) between '{5}' and '{0}' ) tblop

) tblFinal
)
 tblJoinForOutStanding on  tblJoinForOutStanding.RowNumber<=tblFinal.RowNumber
 group by tblFinal.ReceiveAmount,tblFinal.WarantyStartMonth,tblFinal.ReceiveAmount,tblFinal.OutStanding,tblFinal.RowNumber

--) tblFinal on tblFinal.WarantyStartMonth = convert(datetime,(cast(YEAR(DateValue) as varchar(4)) + '-' + cast(Month(DateValue) as varchar(4)) + '-' + '01'),101)
--option (maxrecursion 0)
",
        startDate, lastDate, topParam, whereCondition, groupParam, firstDayOfYear);
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

                var currentDate = DateTime.Now;
                var startMonth = currentDate.AddMonths(-6);

                var startDate = startMonth.Month + "/1/" + startMonth.Year;
                var lastDate = currentDate.Month + "/" + currentDate.Day + "/" + currentDate.Year;

                if (fromDate != null)
                {
                    startDate = fromDate.ToString();
                }
                if (toDate != null)
                {
                    lastDate = toDate.ToString();
                }
                var firstDayOfYear = "01/01/" + Convert.ToDateTime(startDate).Year;

                string query = string.Format(@"Select tblFinal.SalesPrice as OutStanding, DATENAME(MM, tblFinal.WarantyStartMonth) + ' ' + CAST(YEAR(tblFinal.WarantyStartMonth) AS VARCHAR(4)) as WarantyStartMonth
--,Sum(tblJoinForOutStanding.OutStanding) as OutStanding
from (
Select SalesPrice,WarantyStartMonth,SalesPrice as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (
Select sum(isnull(Sale.DownPay + Sale.NetPrice,0)) as SalesPrice,
convert(datetime,(cast(YEAR(Sale.WarrantyStartDate) as varchar(4)) + '-' + cast(Month(Sale.WarrantyStartDate) as varchar(4)) + '-' + '01'),101) as WarantyStartMonth
from Sale
where Sale.WarrantyStartDate is not null and Sale.IsActive=1 and Sale.State = 5 and Cast(WarrantyStartDate as date) between '{0}' and '{1}'
{3}
group by convert(datetime,(cast(YEAR(Sale.WarrantyStartDate) as varchar(4)) + '-' + cast(Month(Sale.WarrantyStartDate) as varchar(4)) + '-' + '01'),101)) tbl1) tblFinal

left outer join (Select * from (

Select SalesPrice,WarantyStartMonth,case when ROW_NUMBER() over (ORDER BY WarantyStartMonth asc)=1 then SalesPrice+isnull(OpeningSales,0) else SalesPrice end as OutStanding,ROW_NUMBER() over (ORDER BY WarantyStartMonth asc) AS RowNumber from (
Select sum(isnull(Sale.DownPay + Sale.NetPrice,0)) as SalesPrice,
convert(datetime,(cast(YEAR(Sale.WarrantyStartDate) as varchar(4)) + '-' + cast(Month(Sale.WarrantyStartDate) as varchar(4)) + '-' + '01'),101) as WarantyStartMonth
from Sale
where Sale.WarrantyStartDate is not null and Sale.IsActive=1 and Sale.State = 5 and Cast(WarrantyStartDate as date) between '{0}' and '{1}'
{3}
group by convert(datetime,(cast(YEAR(Sale.WarrantyStartDate) as varchar(4)) + '-' + cast(Month(Sale.WarrantyStartDate) as varchar(4)) + '-' + '01'),101)) tbl1 
cross join (Select sum(isnull(Sale.DownPay + Sale.NetPrice,0)) as OpeningSales
from Sale
where Sale.WarrantyStartDate is not null and Sale.IsActive=1 and Cast(WarrantyStartDate as date) between '{5}' and '{0}'
{3}) tblOpening )
tblFinal) 
tblJoinForOutStanding on  tblJoinForOutStanding.RowNumber<=tblFinal.RowNumber
group by tblFinal.SalesPrice,tblFinal.WarantyStartMonth,tblFinal.SalesPrice,tblFinal.OutStanding,tblFinal.RowNumber
", startDate, lastDate, topParam, whereCondition, groupParam, firstDayOfYear);
                return Data<PendingCollectionChart>.DataSource(query);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public GridEntity<RatingCalculation> GetDuePercentAndCustomerInfoForDashboardGrid(GridOptions options, string condition, string condition2, string orderBy)
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
else 0 end) end as TotalDuePercentTillDate,

(ReceiveAmountTillDate-SUM(isnull(stcCur.ReceiveAmount,0))) as DueAmountTillDate
from (
Select tblTemp1.*, (Sum(ISNULL(SIC.Amount,0))+DownPay) as ReceiveAmountTillDate 
from (

Select Invoice,NetPrice,DownPay,(NetPrice+DownPay) as Amount,SUM(isnull(Sale_Collection.ReceiveAmount,0)) as PaidAmount,
(NetPrice+DownPay-SUM(isnull(Sale_Collection.ReceiveAmount,0))) as OutStandingAmount,
(SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100 as TotalReceivePercent,
100-((SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100) as TotalDuePercent,
sc.Name,sc.Address,sc.Phone,sc.Phone2,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,Branch.BranchCode,
AT.[Type] as ProductTypeName, SP.TypeId
from Sale 
left outer join Sale_Collection on Sale_Collection.SaleInvoice = Sale.Invoice 
left outer join Sale_Customer sc on Sale.CustomerId = sc.CustomerId and Sale.IsActive = 1 And sc.IsActive=1
left outer join Branch on Branch.BRANCHID= Sale.BranchId
left outer join Sale_Product SP on Sale.ModelId = SP.ModelId
left outer join Sale_AllType AT ON AT.TypeId=SP.TypeId and AT.Flag='Product' AND AT.IsActive=1
{0}
group by Invoice,NetPrice,DownPay,sc.Name,sc.Address,sc.Phone,sc.Phone2,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,Branch.BranchCode,AT.Type,SP.TypeId) as tblTemp1
left outer join Sale_Installment SIC on SIC.SInvoice = tblTemp1.Invoice and SIC.DueDate <= GETDATE()
group by tblTemp1.Invoice,NetPrice,DownPay,tblTemp1.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,tblTemp1.Name,tblTemp1.Address,tblTemp1.Phone,tblTemp1.Phone2,tblTemp1.CustomerCode,tblTemp1.ProductName,tblTemp1.Model,SaleTypeId,tblTemp1.IsRelease,tblTemp1.BranchCode,TypeId,ProductTypeName) tblTemp2
left outer join Sale_Collection stcCur on stcCur.SaleInvoice = tblTemp2.Invoice and stcCur.DueDate<=GETDATE()
group by tblTemp2.Invoice,NetPrice,DownPay,tblTemp2.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,ReceiveAmountTillDate,tblTemp2.Name,tblTemp2.Address,tblTemp2.Phone,tblTemp2.Phone2,tblTemp2.CustomerCode,tblTemp2.ProductName,tblTemp2.Model,SaleTypeId,tblTemp2.IsRelease,tblTemp2.BranchCode,TypeId,ProductTypeName) tblTemp3 {1}", condition, condition2);

            #endregion
            var data = Kendo<RatingCalculation>.Grid.GenericDataSource(options, sql, orderBy);
            return data;
        }

        public GridEntity<RatingCalculation> GetCusomerRatingInfoData(GridOptions options, string condition, string condition2, string orderBy)
        {
            #region sql query
//            string sql = string.Format(@"
//
//Select tblTemp3.* from (Select tblTemp2.*, case when SaleTypeId=2 then PaidAmount else SUM(isnull(stcCur.ReceiveAmount,0)) end as PaymentTillDate, 
//case when SaleTypeId=2 then PaidAmount else 
//case when SUM(isnull(stcCur.ReceiveAmount,0)) > 0 then 
//(SUM(isnull(stcCur.ReceiveAmount,0))/ReceiveAmountTillDate)*100
//else 0 end end as TotalReceivePercentTillDate,
//case when SaleTypeId=2 then 0 else
//(100 - case when SUM(isnull(stcCur.ReceiveAmount,0)) > 0 then 
//(SUM(isnull(stcCur.ReceiveAmount,0))/ReceiveAmountTillDate)*100
//else 0 end) end as TotalDuePercentTillDateOld,
//case when OutStandingAmount > 0 then ((ReceiveAmountTillDate-SUM(isnull(stcCur.ReceiveAmount,0)))*100)/OutStandingAmount else 0 end  as TotalDuePercentTillDate,
//(ReceiveAmountTillDate-SUM(isnull(stcCur.ReceiveAmount,0))) as DueAmountTillDate
//from (
//Select tblTemp1.*, (Sum(ISNULL(SIC.Amount,0))+DownPay) as ReceiveAmountTillDate 
//from (
//
//Select Invoice,NetPrice,DownPay,(NetPrice+DownPay) as Amount,SUM(isnull(Sale_Collection.ReceiveAmount,0)) as PaidAmount,
//(NetPrice+DownPay-SUM(isnull(Sale_Collection.ReceiveAmount,0))) as OutStandingAmount,
//(SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100 as TotalReceivePercent,
//100-((SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100) as TotalDuePercent,
//sc.Name,sc.Address,sc.Phone,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,sc.CompanyId
//
//from Sale 
//left outer join Sale_Collection on Sale_Collection.SaleInvoice = Sale.Invoice 
//left outer join Sale_Customer sc on Sale.CustomerId = sc.CustomerId and Sale.IsActive = 1 and sc.IsActive=1
//left outer join Sale_Product SP on Sale.ModelId = SP.ModelId
//{0}
//group by Invoice,NetPrice,DownPay,sc.Name,sc.Address,sc.Phone,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,sc.CompanyId) as tblTemp1
//left outer join Sale_Installment SIC on SIC.SInvoice = tblTemp1.Invoice and SIC.DueDate <= GETDATE()
//group by tblTemp1.Invoice,NetPrice,DownPay,tblTemp1.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,tblTemp1.Name,tblTemp1.Address,tblTemp1.Phone,tblTemp1.CustomerCode,tblTemp1.ProductName,tblTemp1.Model,SaleTypeId,tblTemp1.IsRelease,tblTemp1.CompanyId) tblTemp2
//left outer join Sale_Collection stcCur on stcCur.SaleInvoice = tblTemp2.Invoice and stcCur.DueDate<=GETDATE()
//group by tblTemp2.Invoice,NetPrice,DownPay,tblTemp2.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,ReceiveAmountTillDate,tblTemp2.Name,tblTemp2.Address,tblTemp2.Phone,tblTemp2.CustomerCode,tblTemp2.ProductName,tblTemp2.Model,SaleTypeId,tblTemp2.IsRelease,tblTemp2.CompanyId) tblTemp3 {1}", condition, condition2);

            string sql = string.Format(@"
Select tblTemp3.* from (Select tblTemp2.*, case when SaleTypeId=2 then PaidAmount else SUM(isnull(stcCur.ReceiveAmount,0)) end as PaymentTillDate, 
case when SaleTypeId=2 then PaidAmount else 
case when SUM(isnull(stcCur.ReceiveAmount,0)) > 0 then 
(SUM(isnull(stcCur.ReceiveAmount,0))/RequiredReceiveAmountTillDate)*100
else 0 end end as TotalReceivePercentTillDate,
case when SaleTypeId=2 then 0 else
(100 - case when SUM(isnull(stcCur.ReceiveAmount,0)) > 0 then 
(SUM(isnull(stcCur.ReceiveAmount,0))/RequiredReceiveAmountTillDate)*100
else 0 end) end as TotalDuePercentTillDateOld,
case when OutStandingAmount > 0 then ((RequiredReceiveAmountTillDate-SUM(isnull(stcCur.ReceiveAmount,0)))*100)/OutStandingAmount else 0 end  as TotalDuePercentTillDate,
(RequiredReceiveAmountTillDate-SUM(isnull(stcCur.ReceiveAmount,0))) as DueAmountTillDate,
 case when SaleTypeId=2 then PaidAmount else SUM(isnull(stcCur.ReceiveAmount,0)) end as ReceiveAmountTillDate
from (
Select tblTemp1.*, (Sum(ISNULL(SIC.Amount,0))+DownPay) as RequiredReceiveAmountTillDate 
from (
Select Invoice,NetPrice,DownPay,(NetPrice+DownPay) as Amount,SUM(isnull(Sale_Collection.ReceiveAmount,0)) as PaidAmount,
(NetPrice+DownPay-SUM(isnull(Sale_Collection.ReceiveAmount,0))) as OutStandingAmount,
(SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100 as TotalReceivePercent,
100-((SUM(isnull(Sale_Collection.ReceiveAmount,0))/(NetPrice+DownPay))*100) as TotalDuePercent,
sc.Name,sc.Address,sc.Phone,sc.Phone2,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,sc.CompanyId,Branch.BranchCode,
AT.[Type] as ProductTypeName, SP.TypeId
from Sale 
left outer join Sale_Collection on Sale_Collection.SaleInvoice = Sale.Invoice 
left outer join Sale_Customer sc on Sale.CustomerId = sc.CustomerId and Sale.IsActive = 1 and sc.IsActive=1
left outer join Branch on Branch.BRANCHID= Sale.BranchId
left outer join Sale_Product SP on Sale.ModelId = SP.ModelId
left outer join Sale_AllType AT ON AT.TypeId=SP.TypeId and AT.Flag='Product' AND AT.IsActive=1
 {0}
group by Invoice,NetPrice,DownPay,sc.Name,sc.Address,sc.Phone,sc.Phone2,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,sc.CompanyId,Branch.BranchCode,AT.Type,SP.TypeId) as tblTemp1
left outer join Sale_Installment SIC on SIC.SInvoice = tblTemp1.Invoice and SIC.DueDate <= GETDATE()
group by tblTemp1.Invoice,NetPrice,DownPay,tblTemp1.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,tblTemp1.Name,tblTemp1.Address,tblTemp1.Phone,tblTemp1.Phone2,tblTemp1.CustomerCode,tblTemp1.ProductName,tblTemp1.Model,SaleTypeId,tblTemp1.IsRelease,tblTemp1.CompanyId,tblTemp1.BranchCode,TypeId,ProductTypeName) tblTemp2
left outer join Sale_Collection stcCur on stcCur.SaleInvoice = tblTemp2.Invoice and stcCur.DueDate<=GETDATE()
group by tblTemp2.Invoice,NetPrice,DownPay,tblTemp2.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,RequiredReceiveAmountTillDate,tblTemp2.Name,tblTemp2.Address,tblTemp2.Phone,tblTemp2.Phone2,tblTemp2.CustomerCode,tblTemp2.ProductName,tblTemp2.Model,SaleTypeId,tblTemp2.IsRelease,tblTemp2.CompanyId,tblTemp2.BranchCode,TypeId,ProductTypeName) tblTemp3  {1}", condition, condition2);

            #endregion

            var data = Kendo<RatingCalculation>.Grid.GenericDataSource(options, sql, orderBy);
            return data;
        }
    }
}
