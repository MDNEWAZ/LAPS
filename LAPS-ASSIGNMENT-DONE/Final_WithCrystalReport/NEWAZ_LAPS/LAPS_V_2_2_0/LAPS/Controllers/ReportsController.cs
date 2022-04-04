using System.Collections.Generic;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Report;
using Azolution.Entities.Sale;
using Azolution.Reports.Service.Interface;
using Azolution.Reports.Service.Service;

namespace LAPS.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/
        private readonly IStatusRepository _statusService = new StatusService();
        private readonly IAuditTrailsReportRepository _repository = new AuditTrailsReportService();
        #region Index / Views
        
       

        public ActionResult AuditTrailsReport()
        {
            return View("AuditTrailsReport/AuditTrailsReport");
        }
        #endregion
         public void GenerateAuditTrailsReport(ReportsParam<AuditTrailsReportEntity> objReportParams)
         {
             IAuditTrailsReportRepository repository = new AuditTrailsReportService();
             objReportParams.ReportTitle = "Audit Trails Report";
             objReportParams.RptFileName = "rptAuditTrailsReport.rpt";
             var auditTrailsList = repository.GenerateAuditTrailsReport(objReportParams);
             objReportParams.DataSource = auditTrailsList;
             //If you want to pass any parameter to CR then Enable
             objReportParams.IsPassParamToCr = true;
             HttpContext.Session["ReportType"] = "AuditTrailsReport";
             HttpContext.Session["ReportParam"] = objReportParams;
         }
        
        public ActionResult SaleReport()
        {
           

            return View("SaleReport/Sale");
        }

        
        public void GenerateSaleReport()
        {
            //ISaleReportRepository aSaleRepository = new SaleReportService();
            //List<SaleReport> aSaleList = aSaleRepository.GetAllSaleReport();
            //HttpContext.Session["SaleReport"] = aSaleList;
            //HttpContext.Session["ReportType"] = "SaleReport";
        }
    }
}
