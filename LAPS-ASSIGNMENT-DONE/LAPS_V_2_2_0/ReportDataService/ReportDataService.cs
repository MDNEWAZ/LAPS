using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Report;
using Utilities;

namespace ReportDataService
{
    public class ReportDataService
    {
        public List<int> GetCompanies(string condition)
        {
            string sql = string.Format(@"select CompanyId from company where {0}", condition);
            var companies = Data<Company>.DataSource(sql);
            return (from com in companies select com.CompanyId).ToList();

        }

        public List<SaleReport> GetSalesReport(string condition1, string condion2)
        {
            try
            {
                //                string sql = string.Format(@"select tbl1.*,tbl2.ReceiveAmount as TotalCollectionAmount from (
                //                                        select tmp.CompanyId,Company.CompanyName,Company.CompanyName as Region,Company.CompanyName as Zone,Branch.BRANCHNAME,COUNT(tmp.SaleId)TotalSalesUnit,SUM(tmp.Price) as TotalSalesAmount,
                //                                        (select IncentiveAmount from IncentiveSettings where NumberOfSale=COUNT(tmp.SaleId)
                //                                        ) as TotalIncentive from
                //                                        (select Sale.* from 
                //                                        (select distinct Sale.Invoice from Sale inner join Sale_Customer on Sale.CustomerId=Sale_Customer.CustomerId  where Sale_Customer.IsActive=1
                //                                        )invTbl inner join Sale on invTbl.Invoice=Sale.Invoice 
                //                                        where BranchId in (
                //                                        select branchId from Branch where COMPANYID in(select CompanyId from company {0})
                //                                        ){1}
                //                                        ) tmp 
                //                                        inner join Company on tmp.CompanyId=Company.CompanyId 
                //                                        inner join Branch on tmp.BranchId=Branch.BRANCHID
                //                                        group by Company.CompanyName,tmp.CompanyId,Branch.BRANCHNAME)tbl1
                //                                        inner join 
                //                                        (SELECT colTbl.CompanyId,SUM(ReceiveAmount)ReceiveAmount
                //                                        FROM Sale_Collection inner join (select Sale.Invoice,Sale.CompanyId from Sale
                //                                        where BranchId in (
                //                                        select branchId from Branch where COMPANYID in(select CompanyId from company {0})
                //                                        ){1})colTbl
                //                                        on Sale_Collection.SaleInvoice=colTbl.Invoice
                //                                        group by colTbl.CompanyId)tbl2
                //                                        on tbl1.CompanyId=tbl2.CompanyId", condition1,condion2);


                //New Query on 2-May-2016
                string sql = string.Format(@"select tbl1.*,tbl2.ReceiveAmount as TotalCollectionAmount from (
                                        select tmp.CompanyId,Company.CompanyName,Company.CompanyName as Region,Company.CompanyName as Zone,Branch.BRANCHNAME,Branch.BRANCHID,COUNT(tmp.SaleId)TotalSalesUnit,SUM(tmp.Price) as TotalSalesAmount,tmp.State,
                                        (select IncentiveAmount from IncentiveSettings where NumberOfSale=COUNT(tmp.SaleId) and IsActive = 1
                                        ) as TotalIncentive from
                                        (select Sale.* from 
                                        (select distinct Sale.Invoice from Sale inner join Sale_Customer on Sale.CustomerId=Sale_Customer.CustomerId  where Sale_Customer.IsActive=1 and Sale.State = 5
                                        )invTbl inner join Sale on invTbl.Invoice=Sale.Invoice 
                                        where BranchId in (
                                        select branchId from Branch
                                       --where COMPANYID in(select CompanyId from company 
                                        {0})
                                        {1}
                                        ) tmp 
                                        inner join Company on tmp.CompanyId=Company.CompanyId 
                                        inner join Branch on tmp.BranchId=Branch.BRANCHID
                                        group by Company.CompanyName,tmp.CompanyId,Branch.BRANCHNAME,Branch.BRANCHID,tmp.State)tbl1
                                        Right join 
                                        (SELECT colTbl.CompanyId,SUM(ReceiveAmount)ReceiveAmount,BranchId
                                        FROM Sale_Collection inner join (select Sale.Invoice,Sale.CompanyId,sale.BranchId,Sale.State from Sale
                                        where BranchId in (
                                        select branchId from Branch 
                                       --where COMPANYID in(select CompanyId from company
                                        {0})
                                        {1})colTbl
                                        on Sale_Collection.SaleInvoice=colTbl.Invoice
                                        group by colTbl.CompanyId,BranchId)tbl2
                                        --on tbl1.CompanyId=tbl2.CompanyId
                                        on tbl1.BRANCHID=tbl2.BranchId ", condition1, condion2);


                return Data<SaleReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<BranchWiseSaleReport> GetBranchWiseSalesData(string condition)
        {
            try
            {
                string sql = string.Format(@"SELECT Branch.BRANCHNAME,cust.CustomerCode as CustomerId,cust.Name as CustomerName,cust.Phone as CustomerMobile,Sale.Price as NetPrice
                            ,pro.ProductName as PackageName,(Case when Sale.SaleTypeId=1 then 'Installment' when Sale.SaleTypeId=2 then 'Cash' end)SaleType,
                            Sale.EntryDate as SaleDate,sRepType.SalesRepTypeName as RepType
                            FROM Sale inner join Branch on Sale.BranchId=Branch.BRANCHID
                            inner join Sale_Customer as cust on Sale.CustomerId=cust.CustomerId and cust.IsActive=1  and Sale.State = 5
                            inner join Sale_Product as pro on Sale.ModelId=pro.ModelId
                            left join SalesRepresentator sReps on Sale.SalesRepId=sReps.SalesRepId
                            left join SalesRepresentatorType sRepType on sReps.SalesRepType=sRepType.SalesRepTypeId {0}", condition);
                return Data<BranchWiseSaleReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<Employee> GetEmployeeDetailsByCompany(string condition)
        {
            try
            {
                string sql = string.Format(@"Select t.EmployeeID,EmployeeName,Company,education.Exam,education.Result, education.year
                from
                (Select Education.EmployeeId,Exam,Result,Year From Education
                right join NewEmployee on NewEmployee.EmployeeID = Education.EmployeeID
                right Join Experience on NewEmployee.EmployeeID= Experience.EmployeeID 
                ) as education
                left join
                (Select NewEmployee.EmployeeID,EmployeeName,Company 
                from NewEmployee 
                right Join Experience on NewEmployee.EmployeeID= Experience.EmployeeID 
                )t on education.EmployeeId=t.EmployeeID{0}", condition);
                return Data<Employee>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<BranchWiseCollectionReport> GetBranchWiseCollectionData(string condition)
        {
            try
            {
                string sql = string.Format(@"select custTbl.*,ISNULL(InsTbl1.PaidIns,0) as NoOfPaidInstallment,ISNULL(CollectTbl.ReceiveAmount,0) as LastCollection,ISNULL(CollectTbl.TotalCollection,0) as TotalCollection
                            ,CONVERT(date,CollectTbl.PayDate,105)CollectionDate,CollectTbl.TransectionId as TranactionId,CONVERT(date,CollectTbl.DueDate,105)DueDate
                            ,(custTbl.InsAmount-ISNULL(InsTbl1.Amount,0) )TotalDueAmount
                             from 
                            (SELECT Branch.BRANCHNAME,Sale.Invoice,Sale.NetPrice as InsAmount,cust.CustomerCode as CustomerId,cust.Name as CustomerName,cust.Phone as CustomerMobile,Sale.Installment as TotalInstallment
                            FROM Sale inner join Branch on Sale.BranchId=Branch.BRANCHID
                            inner join Sale_Customer as cust on Sale.CustomerId=cust.CustomerId  and cust.IsActive=1 and Sale.State = 5 {0})custTbl
                            left join (select SInvoice,COUNT(*)PaidIns,SUM(Amount)Amount from Sale_Installment where Status=1 group by SInvoice) as InsTbl1
                            on custTbl.Invoice=InsTbl1.SInvoice
                            right join 
                            (Select SaleInvoice,MAX(InstallmentNo)InstallmentNo,MIN(ReceiveAmount)ReceiveAmount,SUM(ReceiveAmount)TotalCollection,MAX(DueAmount)DueAmount,MAX(PayDate)PayDate
                           ,MAX(TransectionId)TransectionId,MAX(DueDate)DueDate
                            from Sale_Collection group by SaleInvoice)as CollectTbl
                            on custTbl.Invoice=CollectTbl.SaleInvoice", condition);
                return Data<BranchWiseCollectionReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<RepresentatorSalesReport> GetRepresentatorSalesData(string condition, string condition2)
        {
            try
            {
                //Previous Code for Sales Representator
                //                string sql = string.Format(@"SELECT Branch.BRANCHNAME,rep.SalesRepId,rep.SalesRepCode as RepName,rep.SalesRepSMSMobNo as MobileNo
                //                            ,count(Sale.SaleId)TotalSalesUnit,SUM(Sale.Price)SalesAmount
                //                            FROM Sale 
                //                            inner join (select distinct Invoice from Sale inner join Sale_Customer on Sale.CustomerId=Sale_Customer.CustomerId  
                //                            {0})tblTemp on Sale.invoice=tblTemp.Invoice and Sale.State = 5
                //                            inner join Branch on Sale.BranchId=Branch.BRANCHID
                //                            inner join SalesRepresentator as rep on Sale.SalesRepId=rep.SalesRepId {0}
                //                            group by rep.SalesRepId,Branch.BRANCHNAME,rep.SalesRepCode,rep.SalesRepSMSMobNo", condition);


                string sql = string.Format(@"Select tblSale.SalesRepId,Rate,TotalSalesUnit,SalesAmount,DpCollection,tblSale.Model,tblSale.ModelId,TotalInstallColl,tblSale.TypeId,tblSale.BRANCHNAME,tblSale.BranchId from 
--TotalRepSale
( Select Price as Rate, SUM(Price) as SalesAmount,COUNT(Sale.SaleId) as TotalSalesUnit,SUM(DownPay)DpCollection,Sale.SalesRepId ,Sale.ModelId, Sale_Product.Model,Sale_product.TypeId,Branch.BRANCHNAME,SalesRepresentator.BranchId
 from Sale inner join Sale_Product on Sale.ModelId = Sale_Product.ModelId
 inner join Sale_Customer on Sale.CustomerId = Sale_Customer.CustomerId
 inner join SalesRepresentator on Sale.SalesRepId = SalesRepresentator.SalesRepId
 inner join Branch on Branch.BRANCHID = SalesRepresentator.BranchId 
 where Sale.State = 5 {1}
  group by Sale.SalesRepId,Sale.ModelId,Sale_product.TypeId,Sale_Product.Model,Price,BRANCHNAME,SalesRepresentator.BranchId
) tblSale

Left join
(Select SUM(ReceiveAmount) as TotalInstallColl,Sale.SalesRepId,Sale.ModelId,Sale_Product.TypeId,Sale_Product.Model,Branch.BRANCHNAME,SalesRepresentator.BranchId
 from Sale_Collection inner join Sale on Sale.Invoice = Sale_Collection.SaleInvoice
 inner join Sale_Product on Sale.ModelId = Sale_Product.ModelId
  inner join Sale_Customer on Sale.CustomerId = Sale_Customer.CustomerId
inner join SalesRepresentator on Sale.SalesRepId = SalesRepresentator.SalesRepId 
inner join Branch on Branch.BRANCHID = SalesRepresentator.BranchId
where Sale_Collection.InstallmentNo > 0 and Sale.State = 5 {1}
 group by Sale.SalesRepId,Sale.ModelId, Sale_product.TypeId,Sale_Product.Model,BRANCHNAME,SalesRepresentator.BranchId) tblInsColl
  on tblSale.SalesRepId = tblInsColl.SalesRepId and tblSale.ModelId = tblInsColl.ModelId and tblSale.BranchId = tblInsColl.BranchId
  
  {0}", condition, condition2);
                return Data<RepresentatorSalesReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<PackageWiseSalesReport> GetPackageWiseSalesData(string condition)
        {
            try
            {
                string sql = string.Format(@"select pro.Model PackageName,sum(tmpSale.price)TotalSalesAmount,SUM(tmpSale.Unit)NoOfPacUnit,sum(tmpSale.ReceiveAmount)TotalCollectionAmount,Company.CompanyName ZoneRegionBranch from
                            (select saleTbl.*,ISNULL(collTbl.ReceiveAmount,0)ReceiveAmount from 
                            (Select Sale.Invoice,Sale.ModelId,SUM(Sale.DownPay+Sale.NetPrice)price,COUNT(*)Unit,Sale.CompanyId from Sale {0}
                            group by Sale.Invoice,Sale.CompanyId,Sale.ModelId)saleTbl
                            left join 
                            (select coll.SaleInvoice,SUM(coll.ReceiveAmount)ReceiveAmount from Sale_Collection as coll group by SaleInvoice)collTbl
                            on saleTbl.Invoice=collTbl.SaleInvoice)tmpSale
                            inner join Company on tmpSale.CompanyId=Company.CompanyId
                            inner join Sale_Product pro on tmpSale.ModelId= pro.ModelId
                            group by Company.CompanyName,tmpSale.ModelId,pro.Model", condition);
                return Data<PackageWiseSalesReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<StockSummaryReport> GetStockSummary()
        {
            try
            {
                //                string sql = string.Format(@"select Company.CompanyName,pro.Model,items.ItemName,ISNULL(stockTmp.stockQuantity,0)StockQuantity,
                //                            ISNULL( stockTmp.StockBalanceQty,0)StockBalanceQty,ISNULL(stockTmp.ReplaceQuantity,0)ReplaceQuantity from 
                //                            (select a.CompanyId,a.ModelId,a.ItemId,a.Quantity as stockQuantity,b.StockBalanceQty,c.Quantity ReplaceQuantity from   
                //                            (select ModelId,itemId,CompanyId,SUM(Quantity)Quantity from Sale_Stock stock group by ModelId,itemId,CompanyId) a
                //                            inner join 
                //                            (
                //                            select ModelId,itemId,CompanyId,SUM(StockBalanceQty)StockBalanceQty from 
                //                            (select a.* from  
                //                            (select EntryDate,ModelId,itemId,CompanyId,StockBalanceQty from StockBalance stockBal)a
                //                            inner join  
                //                            (select MAX(EntryDate)EntryDate, ModelId,itemId,CompanyId from StockBalance stockBal group by ModelId,itemId,CompanyId ) b
                //                            on a.EntryDate=b.EntryDate and a.CompanyId=b.CompanyId and a.ModelId=b.ModelId and a.ItemId=b.ItemId
                //                            union all  
                //                            select a.* from  
                //                            (select EntryDate,ModelId,itemId,CompanyId,StockBalanceQty from StockBalance_Replacement stockBal)a
                //                            left join  
                //                            (select MAX(EntryDate)EntryDate, ModelId,itemId,CompanyId from StockBalance_Replacement stockBal group by ModelId,itemId,CompanyId ) b
                //                            on a.EntryDate=b.EntryDate and a.CompanyId=b.CompanyId and a.ModelId=b.ModelId and a.ItemId=b.ItemId) stockBalance
                //                            group by ModelId,itemId,CompanyId
                //                            )b 
                //                            on a.CompanyId=b.CompanyId and a.ModelId=b.ModelId and a.ItemId=b.ItemId 
                //                            full outer join 
                //                            (
                //                              select ModelId,ItemId,CompanyId,SUM(Quantity)Quantity from (
                //                              select Replacement.ModelId, RefItemId as ItemId,Sale.CompanyId,1  as Quantity from Sale_Replacement Replacement 
                //                              left join Sale on Replacement.SaleId=Sale.SaleId 
                //                              )tmp group by ModelId,ItemId,CompanyId
                //                            )c 
                //                            on b.CompanyId=c.CompanyId and b.ModelId=b.ModelId and b.ItemId=b.ItemId) stockTmp
                //                            left join Company on stockTmp.CompanyId=Company.CompanyId
                //                            left join Sale_Product as pro on stockTmp.ModelId=pro.ModelId
                //                            left join Sale_Product_Items as items on stockTmp.ItemId=items.ItemId order by Company.CompanyName");


                string sql = string.Format(@"select Company.CompanyName,pro.Model,items.ItemName,ISNULL(stockTmp.stockQuantity,0)StockQuantity,ISNULL(stockTmp.TotalSale,0)SaleQuantity,
        ISNULL( stockTmp.StockBalanceQty,0)StockBalanceQty,ISNULL(stockTmp.ReplaceQuantity,0)ReplaceQuantity from 
        (select a.CompanyId,a.ModelId,a.ItemId,a.Quantity as stockQuantity,b.StockBalanceQty,c.Quantity ReplaceQuantity,TotalSale  from   
        (select ModelId,itemId,CompanyId,SUM(Quantity)Quantity from Sale_Stock stock group by ModelId,itemId,CompanyId) a
        inner join 
        (
        select ModelId,itemId,CompanyId,SUM(StockBalanceQty)StockBalanceQty,TotalSale from 
        (select a.*,t.TotalSale from  
        (select EntryDate,ModelId,itemId,CompanyId,StockBalanceQty from StockBalance stockBal)a
        inner join  
        (select MAX(EntryDate)EntryDate, ModelId,itemId,CompanyId from StockBalance stockBal group by ModelId,itemId,CompanyId ) b
        on a.EntryDate=b.EntryDate and a.CompanyId=b.CompanyId and a.ModelId=b.ModelId and a.ItemId=b.ItemId
        
         left join 
		(Select ModelId,SUM(ItemQuantity) as TotalSale,SalesItem.ItemId,Sale.CompanyId
		from Sale right join SalesItem on Sale.SaleId = SalesItem.SaleId and Sale.State = 5
		group by Sale.ModelId,SalesItem.ItemId, Sale.CompanyId)t 
		on t.CompanyId = a.CompanyId and t.ItemId = a.ItemId and t.ModelId = a.ModelId
       		    		
        union all  
        
        select a.*,t.TotalSale from  
        (select EntryDate,ModelId,itemId,CompanyId,StockBalanceQty from StockBalance_Replacement stockBal)a
        
        left join 
		(Select ModelId,SUM(ItemQuantity) as TotalSale,SalesItem.ItemId,Sale.CompanyId
		from Sale right join SalesItem on Sale.SaleId = SalesItem.SaleId and Sale.State = 5
		group by Sale.ModelId,SalesItem.ItemId, Sale.CompanyId)t 
		on t.CompanyId = a.CompanyId and t.ItemId = a.ItemId and t.ModelId = a.ModelId
        
        left join  
        (select MAX(EntryDate)EntryDate, ModelId,itemId,CompanyId from StockBalance_Replacement stockBal group by ModelId,itemId,CompanyId ) b
        on a.EntryDate=b.EntryDate and a.CompanyId=b.CompanyId and a.ModelId=b.ModelId and a.ItemId=b.ItemId) stockBalance
        group by ModelId,itemId,CompanyId,TotalSale
        )b 
        on a.CompanyId=b.CompanyId and a.ModelId=b.ModelId and a.ItemId=b.ItemId 
        
        full outer join 
        (
          select ModelId,ItemId,CompanyId,SUM(Quantity)Quantity from (
          select Replacement.ModelId, RefItemId as ItemId,Sale.CompanyId,1  as Quantity from Sale_Replacement Replacement 
          left join Sale on Replacement.SaleId=Sale.SaleId 
          )tmp group by ModelId,ItemId,CompanyId
        )c 
        on b.CompanyId=c.CompanyId and b.ModelId=b.ModelId and b.ItemId=b.ItemId) stockTmp
        left join Company on stockTmp.CompanyId=Company.CompanyId
        left join Sale_Product as pro on stockTmp.ModelId=pro.ModelId
        left join Sale_Product_Items as items on stockTmp.ItemId=items.ItemId order by Company.CompanyName");
                return Data<StockSummaryReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }


        public List<Employee> GetEmployeesReport()
        {
            try
            {
                string sql = string.Format(@"Select NE.EmployeeName, NE.Email, NE.Designation, EX.Company, Edu.Institute, Edu.Year, Edu.Exam, Edu.Result 
                from
                NewEmployee as NE
                left Join Experience as EX on NE.EmployeeID =EX.EmployeeID
                left Join Education as Edu on NE.EmployeeID =Edu.EmployeeID");
                return Data<Employee>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }
        public List<StockDetailsReport> GetStockDetails(string condition, ReportParam reportParam)
        {
            try
            {
                string sql = "";
                if (reportParam.StockType == 2)
                {
                    sql = string.Format(@"select Branch.BRANCHNAME, tmp.* from 
                        (select stock.CompanyId,stock.BranchId,DeliveryDate as ChalanDate,DeliveryChalanNo as ChalanNo,pro.Model,DeliveryOrderNo as DOnumbeer,item.ItemName,item.ItemModel
                        ,Quantity,QBInvoiceNo,case when StockCategoryId=1 then 'Sales' else 'Replacement' end as DeliveryType,Company.CompanyName as IssuedFrom
                        from Sale_Stock stock 
                        inner join Sale_Product pro on stock.ModelId=pro.ModelId
                        inner join Sale_Product_Items as item on stock.ItemId=item.ItemId
                        inner join Users on stock.EntryUserId=Users.UserId
                        inner join Company on Users.CompanyID=Company.CompanyId {0})tmp
                        inner join Branch ON tmp.BranchId=Branch.BRANCHID", condition);
                }
                else
                {
                    sql = string.Format(@"select Company.CompanyName,tmp.* from 
                        (select stock.CompanyId,stock.BranchId,DeliveryDate as ChalanDate,DeliveryChalanNo as ChalanNo,pro.Model,DeliveryOrderNo as DOnumbeer,item.ItemName,item.ItemModel
                        ,Quantity,QBInvoiceNo,case when StockCategoryId=1 then 'Sales' else 'Replacement' end as DeliveryType,Company.CompanyName as IssuedFrom
                        from Sale_Stock stock 
                        inner join Sale_Product pro on stock.ModelId=pro.ModelId
                        inner join Sale_Product_Items as item on stock.ItemId=item.ItemId
                        inner join Users on stock.EntryUserId=Users.UserId
                        inner join Company on Users.CompanyID=Company.CompanyId {0})tmp
                        inner join Company on tmp.CompanyId=Company.CompanyId", condition);
                }
                return Data<StockDetailsReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<RepCommsionReport> GetCommisionReport(string condition)
        {
            try
            {
                string sql = string.Format(@"select tmp.*,Commission.Amount as Commision,1 Unit from 
                            (select repType.SalesRepTypeId,repType.SalesRepTypeName,rep.SalesRepSMSMobNo as mobile,Branch.BRANCHNAME,Sale.BranchId,Sale.SalesRepId,SaleTypeId,(Sale.NetPrice+Sale.DownPay)price,Sale.WarrantyStartDate as saleDate from Sale
                            inner join SalesRepresentator rep on Sale.SalesRepId=rep.SalesRepId 
                            inner join SalesRepresentatorType repType on rep.SalesRepType =repType.SalesRepTypeId
                            inner join Branch on rep.BranchId=Branch.BRANCHID
                            where Sale.SalesRepId <> '' and Sale.State = 5 {0})tmp
                            inner join Commission on tmp.SalesRepTypeId=Commission.SaleRepTypeId and tmp.SaleTypeId=Commission.SaleType", condition);
                return Data<RepCommsionReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<CustomerStatusReport> GetCustomerStatusReport(string condition, ReportParam reportParam)
        {
            var startDate = reportParam.StartDate.ToString("MM/dd/yyyy");
            var endDate = reportParam.EndDate.ToString("MM/dd/yyyy");
            var condition2 = "";

            if (startDate != "" && endDate != "" && startDate != "01/01/0001" && endDate != "01/01/0001")
            {
                condition2 += string.Format(" where Cast(IssueDate as date) between '{0}' and '{1}'", startDate, endDate);
            }


            try
            {
                string sql = string.Format(@"
WITH e AS
(
        SELECT *,
            ROW_NUMBER() OVER
            (
                PARTITION BY SaleInvoice
                ORDER BY CONVERT(date, IssueDate, 105) DESC
            ) AS Recency
        FROM Sale_License 
)
SELECT distinct rt.Color, tblTemp3.*,Number as LicenseCode, IssueDate,'Last License' as License,
 case 
	when (tblTemp3.TotalDuePercent < 1 and tblTemp3.IsRelease != 1) then 'Waiting for Released' 
	when (tblTemp3.TotalDuePercent < 1 and tblTemp3.IsRelease = 1) then 'Released' 
	else rt.Color end as [Status]
FROM e 

Left join
--Select tblTemp3.* from 
(Select tblTemp2.*, case when SaleTypeId=2 then PaidAmount else SUM(isnull(stcCur.ReceiveAmount,0)) end as PaymentTillDate, 
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
sc.Name,sc.Address,sc.Phone,sc.Phone2,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,sc.CompanyId,Branch.BranchCode, Branch.BranchId, sale.ModelId, sale.EntryDate,
Branch.BRANCHNAME,Company.CompanyName, Company.CompanyName as Zone, Company.CompanyName as Region

from Sale 
left outer join Sale_Collection on Sale_Collection.SaleInvoice = Sale.Invoice 
left outer join Sale_Customer sc on Sale.CustomerId = sc.CustomerId and Sale.IsActive = 1 and sc.IsActive=1
left outer join Branch on Branch.BRANCHID= Sale.BranchId
Left outer join Company on Company.CompanyId = sc.CompanyId
left outer join Sale_Product SP on Sale.ModelId = SP.ModelId
 Where  sc.CompanyId in ( Select CompanyId from Company) 
 
group by Invoice,NetPrice,DownPay,sc.Name,sc.Address,sc.Phone,sc.Phone2,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,Branch.BranchCode,Branch.BranchId,Sale.ModelId,sale.EntryDate,sc.CompanyId, Branch.BRANCHNAME,Company.CompanyName,Color) as tblTemp1
left outer join Sale_Installment SIC on SIC.SInvoice = tblTemp1.Invoice and SIC.DueDate <= GETDATE()
group by tblTemp1.Invoice,NetPrice,DownPay,tblTemp1.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,tblTemp1.Name,tblTemp1.Address,tblTemp1.Phone,tblTemp1.Phone2,tblTemp1.CustomerCode,tblTemp1.ProductName,tblTemp1.Model,SaleTypeId,tblTemp1.IsRelease,tblTemp1.CompanyId,tblTemp1.BranchCode,tblTemp1.BranchId, tblTemp1.ModelId,tblTemp1.EntryDate,tblTemp1.BRANCHNAME,tblTemp1.CompanyName,Zone,Region) tblTemp2
left outer join Sale_Collection stcCur on stcCur.SaleInvoice = tblTemp2.Invoice and stcCur.DueDate<=GETDATE()
group by tblTemp2.Invoice,NetPrice,DownPay,tblTemp2.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,RequiredReceiveAmountTillDate,tblTemp2.Name,tblTemp2.Address,tblTemp2.Phone,tblTemp2.Phone2,tblTemp2.CustomerCode,tblTemp2.ProductName,tblTemp2.Model,SaleTypeId,tblTemp2.IsRelease,tblTemp2.BranchCode,tblTemp2.BranchId, tblTemp2.ModelId, tblTemp2.EntryDate,BRANCHNAME,tblTemp2.CompanyId,tblTemp2.CompanyName,Zone,Region) 
tblTemp3 on tblTemp3.Invoice = e.SaleInvoice

inner join (
Select CompanyId,FromDue,ToDue,Sale_AllType.Type as Color from Sale_Due
left outer join Sale_AllType on Sale_AllType.TypeId = Sale_Due.TypeId and Sale_AllType.Flag='Color' and Sale_AllType.IsActive = 1
where  Status = 1) rt on rt.CompanyId = tblTemp3.CompanyId and TotalDuePercentTillDate between FromDue and ToDue
{0} and Recency = 1 ", condition, condition2);
                return Data<CustomerStatusReport>.DataSource(sql);
            }
            catch (Exception)
            {

                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<TransactionReport> GetTransactionReport(string condition, ReportParam reportParam)
        {
            try
            {
                string sql = string.Format(@"Select Sale_Collection.*,Branch.BranchId,Branch.BranchCode,
			 Branch.BRANCHNAME,Company.CompanyId, Company.CompanyName, Company.CompanyName as Zone,
			 Company.CompanyName as Region,sc.CustomerId,sc.CustomerCode,sc.Name, sc.ReferenceId from Sale 
                left outer join Sale_Collection on Sale_Collection.SaleInvoice = Sale.Invoice 
                left outer join Sale_Customer sc on Sale.CustomerId = sc.CustomerId and Sale.IsActive = 1 and sc.IsActive=1
                left outer join Branch on Branch.BRANCHID= Sale.BranchId
                Left outer join Company on Company.CompanyId = Sale.CompanyId
                left outer join Sale_Product SP on Sale.ModelId = SP.ModelId {0}", condition);
                return Data<TransactionReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

        public List<CustomerStatusReport> GetCustomerStatusReportByCustomerId(string condition, ReportParam reportParam)
        {
            var startDate = reportParam.StartDate.ToString("MM/dd/yyyy");
            var endDate = reportParam.EndDate.ToString("MM/dd/yyyy");
            var condition2 = "";

            if (startDate != "" && endDate != "" && startDate != "01/01/0001" && endDate != "01/01/0001")
            {
                condition2 += string.Format(" and Cast(Sale_License.IssueDate as date) between '{0}' and '{1}'", startDate, endDate);
            }

            try
            {
                string sql = string.Format(@"Select  tblTemp3.*,'All License' as License,rt.Color,Sale_License.Number as LicenseCode, Sale_License.IssueDate,
 case 
	when (tblTemp3.TotalDuePercent < 1 and tblTemp3.IsRelease != 1) then 'Waiting for Released' 
	when (tblTemp3.TotalDuePercent < 1 and tblTemp3.IsRelease = 1) then 'Released' 
	else rt.Color end as [Status]
  from 
(Select tblTemp2.*, case when SaleTypeId=2 then PaidAmount else SUM(isnull(stcCur.ReceiveAmount,0)) end as PaymentTillDate, 
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
sc.Name,sc.Address,sc.Phone,sc.Phone2,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,sc.CompanyId,Branch.BranchCode, Branch.BranchId, sale.ModelId, sale.EntryDate,
Branch.BRANCHNAME,Company.CompanyName, Company.CompanyName as Zone, Company.CompanyName as Region

from Sale 
left outer join Sale_Collection on Sale_Collection.SaleInvoice = Sale.Invoice 
left outer join Sale_Customer sc on Sale.CustomerId = sc.CustomerId and Sale.IsActive = 1 and sc.IsActive=1
left outer join Branch on Branch.BRANCHID= Sale.BranchId
Left outer join Company on Company.CompanyId = sc.CompanyId
left outer join Sale_Product SP on Sale.ModelId = SP.ModelId
 Where  sc.CompanyId in ( Select CompanyId from Company) 
group by Invoice,NetPrice,DownPay,sc.Name,sc.Address,sc.Phone,sc.Phone2,sc.CustomerCode,SP.ProductName,SP.Model,SaleTypeId,Sale.IsRelease,Branch.BranchCode,Branch.BranchId,Sale.ModelId,sale.EntryDate,sc.CompanyId, Branch.BRANCHNAME,Company.CompanyName) as tblTemp1
left outer join Sale_Installment SIC on SIC.SInvoice = tblTemp1.Invoice and SIC.DueDate <= GETDATE()
group by tblTemp1.Invoice,NetPrice,DownPay,tblTemp1.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,tblTemp1.Name,tblTemp1.Address,tblTemp1.Phone,tblTemp1.Phone2,tblTemp1.CustomerCode,tblTemp1.ProductName,tblTemp1.Model,SaleTypeId,tblTemp1.IsRelease,tblTemp1.CompanyId,tblTemp1.BranchCode,tblTemp1.BranchId, tblTemp1.ModelId,tblTemp1.EntryDate,tblTemp1.BRANCHNAME,tblTemp1.CompanyName,Zone,Region) tblTemp2
left outer join Sale_Collection stcCur on stcCur.SaleInvoice = tblTemp2.Invoice and stcCur.DueDate<=GETDATE()
group by tblTemp2.Invoice,NetPrice,DownPay,tblTemp2.Amount,PaidAmount,OutStandingAmount,TotalReceivePercent,TotalDuePercent,RequiredReceiveAmountTillDate,tblTemp2.Name,tblTemp2.Address,tblTemp2.Phone,tblTemp2.Phone2,tblTemp2.CustomerCode,tblTemp2.ProductName,tblTemp2.Model,SaleTypeId,tblTemp2.IsRelease,tblTemp2.BranchCode,tblTemp2.BranchId, tblTemp2.ModelId, tblTemp2.EntryDate,BRANCHNAME,tblTemp2.CompanyId,tblTemp2.CompanyName,Zone,Region) 
tblTemp3 

inner join (
Select CompanyId,FromDue,ToDue,Sale_AllType.Type as Color from Sale_Due
left outer join Sale_AllType on Sale_AllType.TypeId = Sale_Due.TypeId and Sale_AllType.Flag='Color' and Sale_AllType.IsActive = 1
where  Status = 1) rt on rt.CompanyId = tblTemp3.CompanyId and TotalDuePercentTillDate between FromDue and ToDue
Right outer join Sale_License on Sale_License.SaleInvoice = tblTemp3.Invoice 
{0}", condition, condition2);
                return Data<CustomerStatusReport>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }

    }
}

