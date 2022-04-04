using System.Collections.Generic;
using Azolution.Entities.Report;
using Utilities;

namespace Azolution.Reports.DataService.DataService
{
    public class AuditTrailsReportDataService
    {
        public List<AuditTrailsReportEntity> GenerateAuditTrailsReport(string condition)
        {
            string query = string.Format("SELECT A.*,U.USERNAME FULLNAME,U.COMPANYID,U.BRANCHID,U.DEPARTMENTID,DEP.DEPARTMENTNAME,0 EMPLOYEETYPENAME,DESIGNATIONNAME FROM AUDIT_TRAIL A " +
                                       "LEFT OUTER JOIN USERS U on U.UserId=A.USER_ID LEFT OUTER JOIN DESIGNATION D on D.COMPANYID=U.CompanyID LEFT OUTER JOIN DEPARTMENT DEP on DEP.DEPARTMENTID=D.DEPARTMENTID {0}", condition);

//            string query = string.Format(@"SELECT A.*,FULLNAME,EMT.COMPANYID,EMT.BRANCHID,EMT.DEPARTMENTID,DEPARTMENTNAME ,EMPLOYEETYPENAME,DESIGNATIONNAME
//From AUDIT_TRAIL A
//LEFT OUTER JOIN EMPLOYEE EM ON EM.HRRECORDID = A.User_Id 
//LEFT OUTER JOIN EMPLOYMENT EMT ON EM.HRRECORDID = EMT.HRRECORDID
//LEFT OUTER JOIN DEPARTMENT on DEPARTMENT.DEPARTMENTID = EMT.DEPARTMENTID
//Left outer join EMPLOYEETYPE on EMPLOYEETYPE.EMPLOYEETYPEID = EMT.EMPLOYEETYPE
//LEFT OUTER JOIN DESIGNATION on DESIGNATION.DESIGNATIONID = EMT.DESIGNATIONID {0}", condition);

            return Data<AuditTrailsReportEntity>.DataSource(query);
        }
    }
}
