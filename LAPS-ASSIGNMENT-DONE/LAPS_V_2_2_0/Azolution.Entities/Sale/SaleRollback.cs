using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SaleRollback
    {
        public string CustomerCode { get; set; }
        public int CustomerId { get; set; }
        public string BranchCode { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string InvoiceNo { get; set; }
        public int SaleId { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public int ChangedCompanyId { get; set; }
        public int ChangedBranchId { get; set; }
        public int ModelId { get; set; }
        public int IsCustomerUpgraded { get; set; }
        public string ReferenceId { get; set; }
        public DateTime FirstPayDate { get; set; }

        public int IsStaff { get; set; }
        public string StaffId { get; set; }
        public string SalesRepId { get; set; }

        public decimal DownPay { get; set; }
        public string ItemSLNo { get; set; }

        public string SimNumber { get; set; }
    }
}
