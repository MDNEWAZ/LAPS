using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class CustomerWithLicenseCode
    {
        public string Number { get; set; }
        public DateTime IssueDate { get; set; }
        public string SaleInvoice { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string BranchCode { get; set; }
        public int LType { get; set; }

        public decimal DownPay { get; set; }
        public string SimNumber { get; set; }
    }
}
