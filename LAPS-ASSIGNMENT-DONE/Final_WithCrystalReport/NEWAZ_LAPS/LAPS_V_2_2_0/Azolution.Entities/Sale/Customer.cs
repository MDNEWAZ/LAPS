using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.HumanResource;

namespace Azolution.Entities.Sale
{
    public class Customer
    {
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string CustomerCode { get; set; }
        public int TypeId { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Thana { get; set; }
        public string Dob { get; set; }
        public string NId { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string EntryDate { get; set; }
        public string Updated { get; set; }
        public int CustomerId { get; set; }
        public int PrantId { get; set; }
        public int IsActive { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string CompanyCode { get; set; }
        //public Branch aBranch { get; set; }
        public string BranchSmsMobileNumber { get; set; }
        public string BranchCode { get; set; }

        public int IsStaff { get; set; }
        public string StaffId { get; set; }
        public string Flag { get; set; }

        public int IsUpgraded { get; set; }
        public string ReferenceId { get; set; }

        public bool IsCustomerForMultipeSale { get; set; }
        public int IsSmsEligible { get; set; }
        public string ProductId { get; set; }
    }
}
