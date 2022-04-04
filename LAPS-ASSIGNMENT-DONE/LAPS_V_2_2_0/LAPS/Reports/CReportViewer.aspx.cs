using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using AuditTrail.Entity.DataService;
using Azolution.Entities.Core;
using Azolution.Entities.Report;
using Azolution.Reports.Service.Interface;
using Azolution.Reports.Service.Service;
using CrystalDecisions.CrystalReports.Engine;

namespace LAPS.Reports
{
    public partial class ReportViewer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OpenSaleReport();
        }

        private void OpenSaleReport()
        {
            CReportViewer.Visible = true;
            var rDoc = new ReportDocument();
            ISaleReportRepository aSaleRepository = new SaleReportService();
            DataTable aTable = aSaleRepository.GetAllSaleReport();
            rDoc.Load(Server.MapPath("/Reports/Rpt/SaleCReport.rpt"));
            var aDataSet=new DataSet();
            aDataSet.Tables.Add(aTable);
            rDoc.SetDataSource(aTable);
            CReportViewer.HasCrystalLogo = false;
            CReportViewer.ReportSource = rDoc;
        }
    }
}