using System;
using System.Collections.Generic;
using System.Configuration;
using Azolution.Entities.Report;
using Azolution.Reports.DataService.DataService;
using Azolution.Reports.Service.Interface;

namespace Azolution.Reports.Service.Service
{
    public class AuditTrailsReportService : IAuditTrailsReportRepository
    {
        public List<AuditTrailsReportEntity> GenerateAuditTrailsReport(ReportsParam<AuditTrailsReportEntity> objReportParams)
        {
           var dataService = new AuditTrailsReportDataService();
           string condition = "";
           if (objReportParams.CompanyId != 0 && objReportParams.BranchId != 0)
           {
               condition = " U.COMPANYID='" + objReportParams.CompanyId + "' and U.branchid='" + objReportParams.BranchId +"' ";
           }
           if (objReportParams.DepartmentId != 0)
           {
               condition += "  and DEPARTMENT.departmentid='" + objReportParams.DepartmentId + "' ";
           }
           if (objReportParams.EmployeeId != 0)
           {
               condition += "  and  EM.HRRECORDID='" + objReportParams.EmployeeId + "'  ";
           }
           if (objReportParams.EmployeeType != 0)
           {
               condition += " and Employeetype='" + objReportParams.EmployeeType + "' ";
           }
           if (objReportParams.FromDate != DateTime.MinValue && objReportParams.ToDate != DateTime.MinValue)
           {

               if (condition != "")
               {
                   var connectionType = ConfigurationSettings.AppSettings["DataBaseType"];
                   if (connectionType == "SQL")
                   {
                       condition += " and ACTION_DATE between '" +
                                   objReportParams.FromDate.ToString("MM/dd/yyyy") + "' and '" +
                                   objReportParams.ToDate.ToString("MM/dd/yyyy") + "'";
                   }
                   else
                   {
                       condition += " and ACTION_DATE between to_date('" +
                                    objReportParams.FromDate.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') and to_date('" +
                                    objReportParams.ToDate.ToString("MM/dd/yyyy") + "', 'MM/dd/yyyy')  ";
                   }
               }
           }

           if (condition != "")
           {
               condition = " WHERE " + condition;
           }



            return dataService.GenerateAuditTrailsReport(condition);
        }
    }
}
