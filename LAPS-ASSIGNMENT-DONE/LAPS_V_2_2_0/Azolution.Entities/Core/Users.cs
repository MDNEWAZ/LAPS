using System;
using System.Collections.Generic;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class Users
    {
        public Users(){}


        public int UserId { get; set; }
        public int CompanyId { get; set; }
        
        public string LoginId { get; set; }
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int FailedLoginNo { get; set; }
        public bool IsActive { get; set; }
        public int IsNotify { get; set; }
        public bool IsExpired { get; set; }
        public string IsFirstLogin { get; set; }
        public string CompanyName { get; set; }
        public string FullLogoPath { get; set; }
        public bool LogHourEnable { get; set; }
        public int FiscalYearStart { get; set; }

        public DateTime LicenseExpiryDate { get; set; }
        public int LicenseUserNo { get; set; }
        public int TotalCount { get; set; }

        public int ShiftId { get; set; }
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int DepartmentId { get; set; }
        public string Theme { get; set; }

        public List<GroupMember> GroupMembers { get; set; }

        public int RootCompanyId { get; set; }
        public string CompanyType { get; set; }
        public int CompanyStock { get; set; }

        public int ChangedCompanyId { get; set; }
        public int ChangedBranchId { get; set; }
        public string ChangedBranchCode { get; set; }
        public int ChangedCompanyStock { get; set; }
        public string ChangedCompanyType { get; set; }
        public int ChangedRootCompanyId { get; set; }

        public List<Company> CompanyList { get; set; }

        public int IsViewer { get; set; }
    }
}
