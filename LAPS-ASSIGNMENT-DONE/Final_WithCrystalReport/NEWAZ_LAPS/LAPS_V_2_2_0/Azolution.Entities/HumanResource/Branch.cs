using System;
using System.Collections.Generic;

namespace Azolution.Entities.HumanResource
{
    [Serializable]
    public class Branch
    {
        public Branch()
        {
           
        }

        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchSmsMobileNumber { get; set; }
        public string BranchDescription { get; set; }

        public List<Department> Departments { get; set; } 
        public int TotalCount { get; set; }

        public int IsActive { get; set; }
        public int IsUpgraded { get; set; }
        public int IsSmsEligible { get; set; }
        public string NewBranchCode
        {
            get { return IsUpgraded == 0 ? "12" + this.BranchCode : null; }
        }

        public int CompanyStock { get; set; }
        public string CompanyType { get; set; }
    }
}
