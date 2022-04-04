using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Report
{
    public class RepresentatorSalesReport
    {
        public string BranchName { get; set; }
        public string SalesRepId { get; set; }
        public string RepName { get; set; }
        public string MobileNo { get; set; }
        public int TotalSalesUnit { get; set; }
        public decimal SalesAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //New filed added on 23-07-2016

        public decimal Rate { get; set; }
        public decimal DpCollection { get; set; }
        public string Model { get; set; }
        public int ModelId { get; set; }
        public decimal TotalInstallColl { get; set; }
        public int TypeId { get; set; }
        public int BranchId { get; set; }
    }
}
