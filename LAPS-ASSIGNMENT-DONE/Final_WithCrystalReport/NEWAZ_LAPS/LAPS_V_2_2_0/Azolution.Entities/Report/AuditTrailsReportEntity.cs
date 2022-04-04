using System;

namespace Azolution.Entities.Report
{
   public class AuditTrailsReportEntity
    {
        public string Requested_Url { get; set; }
        public int Audit_Id { get; set; }
        public int User_Id { get; set; }
        public string Client_User { get; set; }
        public string Client_Ip { get; set; }
        public string Shortdescription { get; set; }
        public string AudiT_Type { get; set; }
        public string Audit_Description { get; set; }
        public DateTime Action_Date { get; set; }

        public string COMPANYNAME { get; set; }
        public string BRANCHNAME { get; set; }
        public string DEPARTMENTNAME { get; set; }
        public string FULLNAME { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
