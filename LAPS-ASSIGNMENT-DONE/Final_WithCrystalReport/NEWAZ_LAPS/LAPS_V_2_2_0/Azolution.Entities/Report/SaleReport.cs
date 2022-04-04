using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Report
{
    public class SaleReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CompanyName { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public int TotalSalesUnit { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public decimal TotalCollectionAmount { get; set; }
        public decimal TotalIncentive { get; set; }
        public string FilterOrganogram { get; set; }
        public string BranchName { get; set; }

    }
}
