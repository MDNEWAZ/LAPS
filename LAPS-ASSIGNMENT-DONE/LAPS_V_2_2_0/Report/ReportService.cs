using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Report;
using Azolution.Entities.Sale;
using ReportDataService;

namespace Report
{
    public class ReportService : IReportService
    {
        private readonly ReportDataService.ReportDataService _dataService;

        public ReportService()
        {
            _dataService = new ReportDataService.ReportDataService();
        }

        public List<SaleReport> GetSalesData(ReportParam reportParam)
        {
            try
            {
                var condition1 = GetInnerCondition(reportParam);
                var condion2 = GetOuterCondition(reportParam);
                var data = _dataService.GetSalesReport(condition1, condion2);
                data.ForEach(d =>
                {
                    d.FilterOrganogram = "Filter By: Sale & Collection, " + reportParam.SearchParam;
                    d.StartDate = reportParam.StartDate;
                    d.EndDate = reportParam.EndDate;
                });
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<BranchWiseSaleReport> GetBranchWiseSalesData(ReportParam param)
        {
            try
            {
                var data = _dataService.GetBranchWiseSalesData(GetConditionForCompanyWiseEmployee(param));
                data.ForEach(d =>
                {
                    d.StratDate = param.StartDate;
                    d.EndDate = param.EndDate;
                });
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }




        public List<Employee> GetEmployeeDetailsByCompany(ReportParam reportParam)
        {
            try
            {
                var data = _dataService.GetEmployeeDetailsByCompany(GetConditionForCompanyWiseEmployee(reportParam));
                return data;

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        private string GetConditionForCompanyWiseEmployee(ReportParam param)
        {
            try
            {
                var condition = "";
                if (param.Company > 0)
                {
                    condition = "Where Education.Company=" + param.Company;
                }
                return condition;
            }
            catch (Exception)
            {
                throw new Exception(" Error! While creating SQL Query Condition");
            }
        }
        public List<BranchWiseCollectionReport> GetBranchWiseCollectionData(ReportParam param)
        {
            try
            {
                var data = _dataService.GetBranchWiseCollectionData(GetConditionForCompanyWiseEmployee(param));
                data.ForEach(d =>
                {
                    d.StartDate = param.StartDate;
                    d.EndDate = param.EndDate;
                });
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<RepresentatorSalesReport> GetRepresentatorSalesData(ReportParam param)
        {
            try
            {
                var data = _dataService.GetRepresentatorSalesData(GetConditionForRepSalesReport(param), GetDateConditionForRepSalesReport(param));
                data.ForEach(d =>
                {
                    d.StartDate = param.StartDate;
                    d.EndDate = param.EndDate;
                });
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<PackageWiseSalesReport> GetPackageWiseSalesData(ReportParam param)
        {
            try
            {
                var data = _dataService.GetPackageWiseSalesData(GetConditionForPackageWiseSalesReport(param));
                data.ForEach(d =>
                {
                    d.Start = param.StartDate;
                    d.End = param.EndDate;
                });
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<StockSummaryReport> GetStockSummary()
        {
            try
            {
                return _dataService.GetStockSummary();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Employee> GetEmployeesReport()
        {
            try
            {
                return _dataService.GetEmployeesReport();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<StockDetailsReport> GetStockDetails(ReportParam param)
        {
            try
            {
                var data = _dataService.GetStockDetails(GetConditionForStockDetailsReport(param), param);
                data.ForEach(d =>
                {
                    d.StartDate = param.StartDate;
                    d.EndDate = param.EndDate;
                });
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<RepCommsionReport> GetCommisionReport(ReportParam param)
        {
            try
            {
                var condition = "";
                if (param.StartDate != DateTime.MinValue && param.EndDate != DateTime.MinValue)
                {
                    condition = string.Format(@" and (Convert(Date,EntryDate,105)between '{0}' and '{1}')", param.StartDate, param.EndDate);
                }
                var data = _dataService.GetCommisionReport(condition);
                data.ForEach(d =>
                {
                    d.StartDate = param.StartDate;
                    d.EndDate = param.EndDate;
                });
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private string GetConditionForStockDetailsReport(ReportParam param)
        {
            try
            {
                var condition = "";
                if (param.ZoneId > 0)
                {
                    condition = "Where  (stock.CompanyId=" + param.ZoneId;
                }
                if (param.RegionId > 0)
                {
                    if (condition == "")
                    {
                        condition = "Where (stock.CompanyId=" + param.RegionId;
                    }
                    else
                    {
                        condition += " or stock.CompanyId=" + param.RegionId;
                    }
                }
                if (param.BranchId > 0)
                {
                    if (condition == "")
                    {
                        condition = "Where stock.BranchId=" + param.BranchId;
                    }
                    else
                    {
                        if (param.RegionId > 0 || param.ZoneId > 0)
                        {
                            condition += ") and stock.BranchId=" + param.BranchId;
                        }
                        else
                        {
                            condition += " and stock.BranchId=" + param.BranchId;
                        }
                    }
                }
                else
                {
                    if (condition != "" && (param.RegionId > 0 || param.ZoneId > 0))
                    {
                        condition += ")";
                    }
                }
                if (param.StartDate != DateTime.MinValue && param.EndDate != DateTime.MinValue)
                {
                    if (condition != "")
                    {
                        condition += string.Format(@" and (Convert(Date,stock.EntryDate,105)between '{0}' and '{1}')", param.StartDate, param.EndDate);
                    }
                    else
                    {
                        condition = string.Format(@" where (Convert(Date,stock.EntryDate,105)between '{0}' and '{1}')", param.StartDate, param.EndDate);
                    }
                }
                return condition;
            }
            catch (Exception)
            {
                throw new Exception(" Error! While creating SQL Query Condition");
            }
        }

        private string GetConditionForPackageWiseSalesReport(ReportParam param)
        {
            try
            {
                var condition = "";
                if (param.PackageId > 0)
                {
                    condition = "Where Sale.ModelId=" + param.PackageId;
                }
                if (param.StartDate != DateTime.MinValue && param.EndDate != DateTime.MinValue)
                {
                    if (condition != "")
                    {
                        condition += string.Format(@" and  (Convert(Date,Sale.EntryDate,105)between '{0}' and '{1}')", param.StartDate, param.EndDate);
                    }
                    else
                    {
                        condition = string.Format(@" where  Sale.State = 5 and (Convert(Date,Sale.EntryDate,105)between '{0}' and '{1}')", param.StartDate, param.EndDate);
                    }
                }
                if (condition == "")
                {
                    condition = @" where Sale.State = 5 and sale.CustomerId in( select CustomerId from Sale_Customer where Sale_Customer.IsActive=1)";
                }
                else
                {
                    condition += @" and Sale.State = 5 and sale.CustomerId in( select CustomerId from Sale_Customer where Sale_Customer.IsActive=1)";
                }
                return condition;
            }
            catch (Exception)
            {
                throw new Exception(" Error! While creating SQL Query Condition");
            }
        }

        private string GetConditionForRepSalesReport(ReportParam param)
        {
            try
            {
                var condition = "";
                if (!string.IsNullOrEmpty(param.SaleRep))
                {
                    if (param.SaleRep != "0" && param.SaleRep != "-1")
                    {
                        //  condition = "Where Sale.SalesRepId='" + param.SaleRep + "'";   //previous Code
                        condition = "Where tblSale.SalesRepId='" + param.SaleRep + "'";
                    }
                }

                if (param.BranchId > 0)   //New Code for Branch Id
                {
                    if (condition == "")
                    {
                        condition = "Where tblSale.BranchId=" + param.BranchId;
                    }
                    else
                    {
                        if (param.SaleRep != "0" && param.SaleRep != "-1")
                        {
                            condition += " and tblSale.BranchId=" + param.BranchId;
                        }
                        else
                        {
                            condition += " and tblSale.BranchId=" + param.BranchId;
                        }
                    }
                }

                //if (param.StartDate != DateTime.MinValue && param.EndDate != DateTime.MinValue)   //Previous Sale.EntryDate
                //{
                //    if (condition != "")
                //    {
                //        condition += string.Format(@"  and Convert(Date,Sale.EntryDate,105) between '{0}' and '{1}')", param.StartDate, param.EndDate);
                //    }
                //    else
                //    {
                //        condition = string.Format(@" and Convert(Date,Sale.EntryDate,105) between '{0}' and '{1}')", param.StartDate, param.EndDate);
                //    }
                //}
                return condition;
            }
            catch (Exception)
            {
                throw new Exception(" Error! While creating SQL Query Condition");
            }
        }

        private string GetDateConditionForRepSalesReport(ReportParam param)
        {
            var condition2 = "";

            if (param.StartDate != DateTime.MinValue && param.EndDate != DateTime.MinValue)   //Previous Sale.EntryDate
            {
                if (condition2 != "")
                {
                    condition2 += string.Format(@"  and Convert(Date,Sale.EntryDate,105) between '{0}' and '{1}'", param.StartDate.ToString("MM/dd/yyyy"), param.EndDate.ToString("MM/dd/yyyy"));
                }
                else
                {
                    condition2 = string.Format(@" and Convert(Date,Sale.EntryDate,105) between '{0}' and '{1}'", param.StartDate.ToString("MM/dd/yyyy"), param.EndDate.ToString("MM/dd/yyyy"));
                }
            }

            return condition2;
        }

        private string GetConditionForBranchWiseReport(ReportParam param)
        {
            try
            {
                var condition = "";
                if (param.BranchId > 0)
                {
                    condition = "Where Sale.BranchId=" + param.BranchId;
                }
                if (param.StartDate != DateTime.MinValue && param.EndDate != DateTime.MinValue)
                {
                    if (condition != "")
                    {
                        condition += string.Format(@"  and (Convert(Date,Sale.EntryDate,105)between '{0}' and '{1}')", param.StartDate, param.EndDate);
                    }
                    else
                    {
                        condition = string.Format(@" where  (Convert(Date,Sale.EntryDate,105)between '{0}' and '{1}')", param.StartDate, param.EndDate);
                    }
                }
                return condition;
            }
            catch (Exception)
            {
                throw new Exception(" Error! While creating SQL Query Condition");
            }
        }

        private string GetOuterCondition(ReportParam reportParam)
        {
            try
            {
                string condition = "";
                if (reportParam.StartDate != DateTime.MinValue && reportParam.EndDate != DateTime.MinValue)
                {
                    condition = string.Format(@" and (Convert(Date,EntryDate,105)between '{0}' and '{1}')", reportParam.StartDate, reportParam.EndDate);
                }

                if (reportParam.SaleRep != "All" && !string.IsNullOrEmpty(reportParam.SaleRep))
                {
                    condition += string.Format(@" and (SalesRepId='{0}')", reportParam.SalesRepId);
                }
                if (reportParam.Package != "All" && !string.IsNullOrEmpty(reportParam.Package))
                {
                    condition += string.Format(@" and (ModelId='{0}')", reportParam.PackageId);
                }
                return condition;
            }
            catch (Exception)
            {
                throw new Exception(" Error! While creating SQL Query");
            }
        }

        private string GetInnerCondition(ReportParam reportParam)
        {
            try
            {

                //string condition = "";
                //if (reportParam.Region == "All")
                //{
                //    condition = " where CompanyType='Region'";
                //}
                //else
                //{
                //    if (!string.IsNullOrEmpty(reportParam.Region))
                //    {
                //        condition = string.Format(" where (CompanyId={0} and CompanyType='Region')", reportParam.RegionId);
                //    }

                //}

                //if (reportParam.Zone == "All")
                //{
                //    condition = condition == ""
                //        ? condition + " where (CompanyType='Zone')"
                //        : condition + " or (CompanyType='Zone')";
                //}
                //else
                //{
                //    if (!string.IsNullOrEmpty(reportParam.Zone))
                //    {
                //        condition = condition == ""
                //            ? condition + string.Format(" where (CompanyId={0} and CompanyType='Zone')", reportParam.ZoneId)
                //            : condition + string.Format(" and (CompanyId={0} and CompanyType='Zone')", reportParam.ZoneId);
                //    }

                //}

                //new Condition Apply

                string condition = "";
                if (reportParam.Region == "All")
                {
                    if (reportParam.ZoneId < 0 && reportParam.BranchId <= 0)
                    {
                        condition = " where COMPANYID in(select CompanyId from company where CompanyType='Region')";

                    }
                    else if (reportParam.ZoneId == 0 && reportParam.BranchId == 0)
                    {
                        condition = " where COMPANYID in(select CompanyId from company where CompanyType='Region')";

                    }

                    else
                    {
                        condition = " where COMPANYID in(select CompanyId from company where CompanyType='Region'";

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(reportParam.Region))
                    {
                        condition = string.Format(" where COMPANYID in(select CompanyId from company where (CompanyId={0} or MotherId = {0} and CompanyType='Region')", reportParam.RegionId);
                    }

                }

                if (reportParam.Zone == "All")
                {

                    condition = condition == ""
                       ? condition + " where COMPANYID in(select CompanyId from company)"
                       : condition + " or (CompanyType='Zone'))";
                }
                else
                {
                    if (!string.IsNullOrEmpty(reportParam.Zone))
                    {
                        condition = condition == ""
                            ? condition + string.Format(" where COMPANYID in(select CompanyId from company where (CompanyId={0} or MotherId = {0} or CompanyType='Zone'))", reportParam.ZoneId)
                            : condition + string.Format("  and (CompanyId={0} or MotherId = {0} or CompanyType='Zone'))", reportParam.ZoneId);
                    }

                }

                if (reportParam.BranchId > 0)
                {
                    condition = string.Format(" where BranchId = {0}", reportParam.BranchId);

                }
                else
                {
                    if (condition != "")
                        if (reportParam.RegionId > 0 && reportParam.ZoneId < 0)
                        {
                            condition = condition + ")";
                        }

                    //if (reportParam.RegionId <= 0 && reportParam.ZoneId <= 0)
                    //{
                    //    condition = condition + ")";
                    //}

                }

                return condition;
            }
            catch (Exception)
            {
                throw new Exception(" Error! While creating SQL Query");
            }
        }

        public List<CustomerStatusReport> GetCustomerStatusReport(ReportParam param, Due dueObjForCustomer, int reportType)
        {
            try
            {
                var data = new List<CustomerStatusReport>();
                if (param.CustomerId > 0 && reportType == 1) // 1 For Single Customer
                {
                    data = _dataService.GetCustomerStatusReportByCustomerId(GetConditionForCustomerStatusReport(param, dueObjForCustomer), param);
                }
                else    // 2 for Multiple Customer
                {
                    data = _dataService.GetCustomerStatusReport(GetConditionForCustomerStatusReport(param, dueObjForCustomer), param);
                }

                data.ForEach(d =>
                {
                    d.FilterOrganogram = "Filer By: Customer Status Report, " + param.SearchParam;
                    d.StartDate = param.StartDate;
                    d.EndDate = param.EndDate;
                });
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private string GetConditionForCustomerStatusReport(ReportParam param, Due dueObjForCustomer)
        {
            try
            {
                var condition = "";
                if (param.ZoneId > 0)
                {
                    condition = "Where  (tblTemp3.CompanyId=" + param.ZoneId;
                }
                if (param.RegionId > 0)
                {
                    if (condition == "")
                    {
                        condition = "Where (tblTemp3.CompanyId=" + param.RegionId;
                    }
                    else
                    {
                        condition += " or tblTemp3.CompanyId=" + param.RegionId;
                    }
                }
                if (param.BranchId > 0)
                {
                    if (condition == "")
                    {
                        condition = "Where tblTemp3.BranchId=" + param.BranchId;
                    }
                    else
                    {
                        if (param.RegionId > 0 || param.ZoneId > 0)
                        {
                            condition += ") and tblTemp3.BranchId=" + param.BranchId;
                        }
                        else
                        {
                            condition += " and tblTemp3.BranchId=" + param.BranchId;
                        }
                    }
                }
                else
                {
                    if (condition != "" && (param.RegionId > 0 || param.ZoneId > 0))
                    {
                        condition += ")";
                    }
                }
                if (param.StartDate != DateTime.MinValue && param.EndDate != DateTime.MinValue)
                {
                    if (condition != "")
                    {
                        condition += string.Format(@" and (Convert(Date,tblTemp3.EntryDate,105)between '{0}' and '{1}')", param.StartDate.ToString("MM/dd/yyyy"), param.EndDate.ToString("MM/dd/yyyy"));
                    }
                    else
                    {
                        condition = string.Format(@" where (Convert(Date,tblTemp3.EntryDate,105)between '{0}' and '{1}')", param.StartDate.ToString("MM/dd/yyyy"), param.EndDate.ToString("MM/dd/yyyy"));
                    }
                }

                if (param.Package != "All" && !string.IsNullOrEmpty(param.Package))   //Package Or  Model
                {
                    if (condition != "")
                    {
                        condition += string.Format(@" and tblTemp3.ModelId='{0}'", param.PackageId);
                    }
                    else
                    {
                        condition = string.Format(@" where tblTemp3.ModelId='{0}'", param.PackageId);
                    }
                }

                if (param.CustomerCode != "All" && !string.IsNullOrEmpty(param.CustomerCode))   //Customer Id
                {
                    if (condition != "")
                    {
                        condition += string.Format(@" and tblTemp3.CustomerCode='{0}'", param.CustomerCode);
                    }
                    else
                    {
                        condition = string.Format(@" where tblTemp3.CustomerCode='{0}'", param.CustomerCode);
                    }
                }

                if (dueObjForCustomer != null && (dueObjForCustomer.Color != "All" && !string.IsNullOrEmpty(dueObjForCustomer.Color))) //Status Red, Orange, 
                {
                    if (param.DueId == -3)//Waiting for Release DueId = -3
                    {
                        condition += string.Format(@" and tblTemp3.TotalDuePercent < 1 And tblTemp3.IsRelease != 1");
                    }
                    else if (param.DueId == -2)//Released  DueID = -2
                    {
                        condition += string.Format(@" and tblTemp3.TotalDuePercent < 1 And tblTemp3.IsRelease = 1");
                    }
                    else  //Color
                    {
                        condition += string.Format(@" and rt.Color ='{0}'", dueObjForCustomer.Color);
                    }

                }
                return condition;
            }
            catch (Exception)
            {
                throw new Exception(" Error! While creating SQL Query Condition");
            }
        }

        public List<TransactionReport> GetTransactionReport(ReportParam param)
        {
            try
            {
                var data = _dataService.GetTransactionReport(GetConditionForTransactionReport(param), param);
                data.ForEach(d =>
                {
                    d.FilterOrganogram = "Filer By: Transaction Report, " + param.SearchParam;
                    d.StartDate = param.StartDate;
                    d.EndDate = param.EndDate;
                });
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private string GetConditionForTransactionReport(ReportParam param)
        {
            try
            {
                var condition = "";
                if (param.ZoneId > 0)
                {
                    condition = "Where  (Sale.CompanyId=" + param.ZoneId;
                }
                if (param.RegionId > 0)
                {
                    if (condition == "")
                    {
                        condition = "Where (Sale.CompanyId=" + param.RegionId;
                    }
                    else
                    {
                        condition += " or Sale.CompanyId=" + param.RegionId;
                    }
                }
                if (param.BranchId > 0)
                {
                    if (condition == "")
                    {
                        condition = "Where Sale.BranchId=" + param.BranchId;
                    }
                    else
                    {
                        if (param.RegionId > 0 || param.ZoneId > 0)
                        {
                            condition += ") and Sale.BranchId=" + param.BranchId;
                        }
                        else
                        {
                            condition += " and Sale.BranchId=" + param.BranchId;
                        }
                    }
                }
                else
                {
                    if (condition != "" && (param.RegionId > 0 || param.ZoneId > 0))
                    {
                        condition += ")";
                    }
                }
                if (param.StartDate != DateTime.MinValue && param.EndDate != DateTime.MinValue)
                {
                    if (condition != "")
                    {
                        condition += string.Format(@" and (Convert(Date,Sale_Collection.PayDate,105)between '{0}' and '{1}')", param.StartDate, param.EndDate);
                    }
                    else
                    {
                        condition = string.Format(@" where (Convert(Date,Sale_Collection.PayDate,105)between '{0}' and '{1}')", param.StartDate, param.EndDate);
                    }
                }
                return condition;
            }
            catch (Exception)
            {
                throw new Exception(" Error! While creating SQL Query Condition");
            }
        }
    }
}
