using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class StockBalance
    {
        public int StockBalanceId { get; set; }
        public int ModelId { get; set; }
        public int ItemId { get; set; }
        public int StockQuantity { get; set; }
        public int StockBalanceQty { get; set; }
        public int Type { get; set; }
        public DateTime EntryDate { get; set; }
        public int EntryUserId { get; set; }
        public int UpdateUserId { get; set; }

        public int CompanyId { get; set; }
        public int BranchId { get; set; }
    }
}
