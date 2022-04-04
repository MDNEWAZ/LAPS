using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Report
{
    public class StockSummaryReport
    {
        public string CompanyName { get; set; }
        public string Model { get; set; }
        public string ItemName { get; set; }
        public int StockQuantity { get; set; }
        public int StockBalanceQty { get; set; }
        public int ReplaceQuantity { get; set; }
        public int SaleQuantity { get; set; }


    }
}
