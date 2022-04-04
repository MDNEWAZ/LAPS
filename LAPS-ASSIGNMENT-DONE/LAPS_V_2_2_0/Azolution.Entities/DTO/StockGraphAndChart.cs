using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
    public class StockGraphAndChart
    {
        public int ModelId { get; set; }
        public int ItemId { get; set; }
        public int StockBalanceQty { get; set; }
        public DateTime EntryDate { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }

        public string ModelItem { get; set; }
    }
}
