using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Report
{
    public class PackageWiseSalesReport
    {
        public string PackageName { get; set; }
        public string ZoneRegionBranch { get; set; }
        public int NoOfPacUnit { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public decimal TotalCollectionAmount { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}
