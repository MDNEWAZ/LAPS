using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class OperationMode
    {
        public int OperationModeId { get; set; }
        public int AutoOperation { get; set; }
        public int ManualOperation { get; set; }
        public int AutoSale { get; set; }
        public int AutoCollection { get; set; }
        public int ManualSale { get; set; }
        public int ManualCollection { get; set; }
        public int AutoInventoryChecking { get; set; }
        public int ManualInventoryChecking { get; set; }
    }
}
