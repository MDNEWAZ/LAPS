using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Report
{
    public class RepCommsionReport
    {
        public string BranchName { get; set; }
        public string SalesRepId { get; set; }
        public string SalesRepTypeName { get; set; }
        public string Mobile { get; set; }
        public int Unit { get; set; }
        public decimal Price { get; set; }
        public decimal Commision { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SaleType { get; set; }

    }
}
