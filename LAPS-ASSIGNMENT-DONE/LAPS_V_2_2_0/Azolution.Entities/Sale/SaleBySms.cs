using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SaleBySms
    {
        public string  CustomerName { get; set; }
        public string NationalId { get; set; }
        public string SmsMobNo { get; set; }
        public string PackageName { get; set; }
        public int ExtraLight { get; set; }
        public decimal DownPaymentAmount { get; set; }
        public int InstallmentNo { get; set; }
        public string BranchCode { get; set; }
        public string SalesRepId { get; set; }
        public string StaffId { get; set; }
        public int IsStaff { get; set; }
        public int IsMultipleSale { get; set; }
      
    }
}
