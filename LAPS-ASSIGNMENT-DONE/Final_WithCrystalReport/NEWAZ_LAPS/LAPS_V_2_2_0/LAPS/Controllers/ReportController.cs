using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.Report;
using Azolution.Entities.Sale;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using Laps.SaleRepresentative.Service.Interface;
using Laps.SaleRepresentative.Service.Service;
using Report;
using Utilities;

namespace LAPS.Controllers
{
    public class ReportController : Controller
    {
        #region Sales & Collection Report Param

        public ActionResult SalesReport()
        {
            return View();
        }

        public ActionResult GetParamDataSource()
        {
            ICompanyRepository companyRepository = new CompanyService();
            if (Session["CurrentUser"] != null)
            {
                var user = ((Users) (Session["CurrentUser"]));
                var companyId = user.CompanyId;
                List<Company> companyList = companyRepository.GetMotherCompany(companyId).ToList();
                var data = new ReportParamDataSource(companyList);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        public ActionResult GetAllRepresentatorCombo()
        {
            ISalesRepresentatorRepository salesRepresentatorRepository = new SalesRepresentatorService();
            return Json(salesRepresentatorRepository.GetAllSalesRepresentatorCombo(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sales & Collection Report

        public ActionResult ShowSaleOrCollectionReport(ReportParam param)
        {
            try
            {
                var data = new ReportData<SaleReport>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetSalesData(param);
                data.RptName = "rptSales.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region Branch Wise Sale Report

        public ActionResult BranchWiseSaleDetails()
        {
            return View("../Reports/SaleReport/BranchWiseSaleDetails");
        }

        public ActionResult ShowBranchSaleReport(ReportParam param)
        {
            try
            {
                var data = new ReportData<BranchWiseSaleReport>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetBranchWiseSalesData(param);
                data.RptName = "rptBranchWiseSaleReport.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region Company Wise Employee Details Report

        public ActionResult EmployeeDetailsByCompany()
        {
            return View("../Reports/CompanyWiseEmployee");
        }
        public ActionResult CompanyWiseEmployeeDetails(ReportParam param)
        {
            try
            {

                var data = new ReportData<Employee>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetEmployeeDetailsByCompany(param);
                data.RptName = "rptCompanyWiseEmployeeInfo.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw  new Exception(e.Message);
            }
        }

        #endregion

        #region Branch Wise Collection

        public ActionResult BranchWiseCollection()
        {
            return View("../Reports/BranchWiseCollection");
        }

        public ActionResult ShowBranchCollectionReport(ReportParam param)
        {
            try
            {
                var data = new ReportData<BranchWiseCollectionReport>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetBranchWiseCollectionData(param);
                data.RptName = "rptBranchWiseCollectionReport.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region Representator Sales Report

        public ActionResult RepresentatorSalesSummary()
        {
            return View("../Reports/SaleReport/RepresentatorSalesSummary");
        }

        public ActionResult GetRepresentatorSalesData(ReportParam param)
        {
            try
            {
                var data = new ReportData<RepresentatorSalesReport>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetRepresentatorSalesData(param);
               // data.RptName = "rptRepresentatorSalesReport.rpt";
                data.RptName = "rptRepresentatorSalesReportModified.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion


        #region Package wisee Sales Report

        public ActionResult PackageWiseSalesSummary()
        {
            return View("../Reports/SaleReport/PackageWiseSalesSummary");
        }

        public ActionResult PackageWiseSalesData(ReportParam param)
        {
            try
            {
                var data = new ReportData<PackageWiseSalesReport>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetPackageWiseSalesData(param);
                data.RptName = "rptPackageWiseSalesReport.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion


        #region Stock Summary Report

        public ActionResult StockSummary()
        {
            return View("../Reports/StockSummary");
        }


        public ActionResult GetStockSummary()
        {
            try
            {
                var data = new ReportData<StockSummaryReport>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetStockSummary();
                data.RptName = "rptStockSummaryReport.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region Stock Report

        public ActionResult StockDetails()
        {
            return View("../Reports/StockDetails");
        }


        public ActionResult GetStockDetails(ReportParam param)
        {
            try
            {
                var data = new ReportData<StockDetailsReport>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetStockDetails(param);
                if (param.StockType == 2)
                {
                    data.RptName = "rptStockBranchDetailsReport.rpt";
                }
                else
                {
                    data.RptName = "rptStockDetailsReport.rpt";
                }
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region Sales Representator Commision Report

        public ActionResult CommisionReport()
        {
            return View("../Reports/CommisionReport");
        }


        public ActionResult GetCommisionReport(ReportParam param)
        {
            try
            {
                var data = new ReportData<RepCommsionReport>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetCommisionReport(param);
                data.RptName = "rptCommisionReport.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region Customer Status Report

        public ActionResult CustomerStatusReport()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("../Reports/CustomerStatusReport");
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        public ActionResult GetCustomerStatusReport(ReportParam param, Due dueObjForCustomer)
        {
            try
            {
                var data = new ReportData<CustomerStatusReport>();
                IReportService reportService = new ReportService();
                if (param.CustomerId > 0)
                {
                    data.DataSource = reportService.GetCustomerStatusReport(param, dueObjForCustomer,1);  //1 for Single Customer
                    data.RptName = "rptCustomerStatusReportForSingleCustomer.rpt";
                    Session["report"] = data;
                }
                else
                {
                    data.DataSource = reportService.GetCustomerStatusReport(param, dueObjForCustomer,2);  //2 for all Customer
                    data.RptName = "rptCustomerStatusReport.rpt";
                    Session["report"] = data;
                }

                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion


        #region Transaction Report

        public ActionResult TransactionReport()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("../Reports/TransactionReport");
                
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        public ActionResult GetTransactionReport(ReportParam param)
        {
            try
            {
                var data = new ReportData<TransactionReport>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetTransactionReport(param);
                data.RptName = "rptTransactionReport.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region EmployeeInfo Summary Report

        public ActionResult EmployeeInfoSummary()
        {
            return View("../Reports/EmployeeSummaryReport");
        }


        public ActionResult GetEmployeeInfoSummary()
        {
            try
            {
                var data = new ReportData<Employee>();
                IReportService reportService = new ReportService();
                data.DataSource = reportService.GetEmployeesReport();
                data.RptName = "rptEmployeeeSummaryReport.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion
    }

    public class ReportParamDataSource
    {
        public List<Company> Zone { private set; get; }
        public List<Company> Region { private set; get; }


        public ReportParamDataSource(List<Company> companies)
        {
            Zone = companies.Where(c => c.CompanyType == "Zone").ToList();
            Region = companies.Where(c => c.CompanyType == "Region").ToList();
        }
    }

}
