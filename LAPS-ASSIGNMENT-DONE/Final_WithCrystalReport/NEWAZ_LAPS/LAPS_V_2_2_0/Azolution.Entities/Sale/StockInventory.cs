using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class StockInventoryDealer
    {
        public int StockDealerID { get; set; }
        public int DealerID { get; set; }
        public int StockDealerQty { get; set; }
        public int ItemID { get; set; }
        public DateTime StockDealerUpdate { get; set; }
        public int StockDealerStatus { get; set; }
        public int StockDealerLastQty { get; set; }
    }

    public class StockInventoryBranch
    {
        public int StockBranchID { get; set; }
        public int BranchID { get; set; }
        public int StockBranchQty { get; set; }
        public int ItemID { get; set; }
        public DateTime StockBranchUpdate { get; set; }
        public int StockBranchStatus { get; set; }
        public int StockBranchLastQty { get; set; }
    }

}
