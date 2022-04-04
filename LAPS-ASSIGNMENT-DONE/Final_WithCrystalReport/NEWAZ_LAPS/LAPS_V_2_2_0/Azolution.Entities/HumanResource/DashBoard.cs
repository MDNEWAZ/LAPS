using System;

namespace Azolution.Entities.HumanResource
{
    [Serializable]
    public class DashBoard
    {
        public DashBoard()
        {
        }
        public string FullName { get; set; }
        public string TelephoneExtension { get; set; }
        public int Status { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int MovementStatus { get; set; }
        public string Remarks { get; set; }
        public DateTime MovementDate { get; set; }
        public bool IsApproved { get; set; }
        public string OutTime { get; set; }
        public bool IsLate { get; set; }
        public bool IsAttendanceClearOut { get; set; }
        public int CompanyId { get; set; }
        public int HRRecordId { get; set; }
        public string StatusSummary { get; set; }
        public int UserId { get; set; }
        public string LoginTime { get; set; }
        public string updateTime { get; set; }
        public string ExpectedReturnTime { get; set; }
        public string LogOutTime { get; set; }
        public string Intime { get; set; }
        public string ClientName { get; set; }
        public int LeaveId { get; set; }
        public int DayOffId { get; set; }
        public int OnSiteClientId { get; set; }
        public int MovementType { get; set; }
        public int ReportTo { get; set; }
        public string CompanyName { get; set; }
        public int TotalCount { get; set; }
    }
}
