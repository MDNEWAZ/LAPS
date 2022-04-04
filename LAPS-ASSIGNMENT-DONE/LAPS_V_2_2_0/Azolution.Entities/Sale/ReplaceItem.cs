using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class ReplaceItem
    {
        public int ItemId { get; set; }
        public int BundleQuantity { get; set; }
        public int ReplacedItemQty { get; set; }
    }
}
