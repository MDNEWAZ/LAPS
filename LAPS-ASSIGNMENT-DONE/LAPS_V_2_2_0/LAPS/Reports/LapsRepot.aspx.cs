using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Azolution.Reports.Service.Interface;
using Azolution.Reports.Service.Service;
using CrystalDecisions.CrystalReports.Engine;
using LAPS.Reports.Entity;
//using Microsoft.Ajax.Utilities;


namespace LAPS.Reports
{
    public partial class LapsRepot : System.Web.UI.Page
    {
        ReportDocument rDoc = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            LoadReaport();
        }

        private void LoadReaport()
        {
            try
            {
                var reportData = (dynamic)HttpContext.Current.Session["report"];
                Page.Title = reportData.PageTile;
                lapsCReportViewer.Visible = true;
                string path = null;
                if (reportData.DataSource.Count == 0)
                {
                    path = Server.MapPath("~/Reports/Rpt/Error.rpt");
                    rDoc.Load(path);
                }
                else
                {
                    try
                    {
                        path = Server.MapPath("~/Reports/Rpt/" + reportData.RptName);
                        rDoc.Load(path);
                        rDoc.SetDataSource(reportData.DataSource);


                    }
                    catch (Exception ex)
                    {
                        Response.Write("Report designer " + reportData.RptName + " not found lam" + ex.Message +  path);
                        return;
                    }

                }

                lapsCReportViewer.HasCrystalLogo = true;
                lapsCReportViewer.ReportSource = rDoc;

                // rDoc.PrintToPrinter(1, true, 0, 0);
                // HttpContext.Current.Session["report"] = null;

                //PrinterSettings settings = new PrinterSettings();

                //PrintDialog pdialog = new PrintDialog();
                //if (pdialog.ShowDialog() == DialogResult.OK)
                //{
                //    settings = pdialog.PrinterSettings;
                //}

                //rDoc.PrintToPrinter(settings, new PageSettings() { }, false);

            }
            catch (Exception e)
            {
                throw new Exception("Error! While Generating Report");
            }
        }

        protected void lapsCReportViewer_Unload(object sender, EventArgs e)
        {
            rDoc.Close();
            rDoc.Dispose();
            GC.Collect();
        }


    }
}