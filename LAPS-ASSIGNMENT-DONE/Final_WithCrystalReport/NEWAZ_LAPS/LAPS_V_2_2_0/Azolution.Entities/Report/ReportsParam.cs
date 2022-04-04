using System;
using System.Collections.Generic;

namespace Azolution.Entities.Report
{
    public class ReportsParam<T>
    {
        
        public string RptFileName { get; set; }
        public Int32 HrRecordId { get; set; }
        public string ReportTitle { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public Int32 MenuId { get; set; }
        public Boolean IsPassParamToCr { get; set; }
        public string ReportType { get; set; }
        public Int32 CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Int32 ShiftId { get; set; }
        public string ShiftName { get; set; }
        public List<T> DataSource { get; set; }

        public DateTime AttendanceDate { get; set; }


        public string StartYear { get; set; }
        public string FullFiscalYear { get; set; }

        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeType { get; set; }


        //Added for Seniority List Report
        public int SeniorityBy { get; set; }

        public string SeniorityByName { get; set; }

        public int CNF_AGENCY_TYPE { get; set; }
        public int WFStateId { get; set; }
        
        //For Asset Return Report

        public int ReceivingGoodsConditionId { get; set; }
        public int AssessmentTypeId { get; set; }
        public int StatusId { get; set; }
        public int CertificateTypeId { get; set; }
        public int DesignationId { get; set; }
        

        //for manpower statistics

        public int Gender { get; set; }
        public int MeritialStatus { get; set; }

        public string PrintUserName { get; set; }


    }
}