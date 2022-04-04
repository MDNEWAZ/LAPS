using System;
using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.Report;
using Azolution.Entities.Sale;

namespace Report
{
    public interface IReportService
    {
        List<SaleReport> GetSalesData(ReportParam reportParam);
        List<BranchWiseSaleReport> GetBranchWiseSalesData(ReportParam param);

        List<Employee> GetEmployeeDetailsByCompany(ReportParam reportParam);
        List<BranchWiseCollectionReport> GetBranchWiseCollectionData(ReportParam reportParam);
        List<RepresentatorSalesReport> GetRepresentatorSalesData(ReportParam reportParam);
        List<PackageWiseSalesReport> GetPackageWiseSalesData(ReportParam reportParam);
        List<StockSummaryReport> GetStockSummary();

        List<Employee> GetEmployeesReport();

        List<StockDetailsReport> GetStockDetails(ReportParam reportParam);
        List<RepCommsionReport> GetCommisionReport(ReportParam reportParam);
        List<CustomerStatusReport> GetCustomerStatusReport(ReportParam reportParam,Due dueObjForCustomer, int reportType);
        List<TransactionReport> GetTransactionReport(ReportParam reportParam);
    }
}