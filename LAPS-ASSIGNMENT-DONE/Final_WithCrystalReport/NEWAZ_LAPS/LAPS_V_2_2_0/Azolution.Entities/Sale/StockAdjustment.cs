using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class StockAdjustment
    {
        public int StockAdjustmentId { get; set; }
        public int ModelId { get; set; }
        public int ItemId { get; set; }
        public int AdjustmentQuantity { get; set; }
        public int StockBalanceQty { get; set; }
        public DateTime EntryDate { get; set; }
        public int EntryUserId { get; set; }
        public int UpdateUserId { get; set; }
        public string Description { get; set; }
        public int AdjustmentTypeId { get; set; }

        public int CompanyId { get; set; }
        public int BranchId { get; set; }
    }
}
