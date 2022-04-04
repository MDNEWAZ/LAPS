using System;
using System.Collections.Generic;
using Azolution.Entities.HumanResource;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string FullLogoPath { get; set; }
        public string PrimaryContact { get; set; }
        public int FiscalYearStart { get; set; }
        public int Flag { get; set; }
        public int MotherId { get; set; }
        public int TotalCount { get; set; }
        public List<Branch> Branches { get; set; }
        public int IsActive { get; set; }

        public string CompanyType { get; set; }
        public int CompanyStock { get; set; }

        public int RootCompanyId { get; set; }
        public Branch Branch{ get; set; }

    }
}
