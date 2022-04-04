using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Report
{
    public class BranchWiseSaleReport
    {
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BranchName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string PackageName { get; set; }
        public DateTime SaleDate { get; set; }
        public string SaleType { get; set; }
        public string RepType { get; set; }
        public decimal NetPrice { get; set; } 
    }
}
