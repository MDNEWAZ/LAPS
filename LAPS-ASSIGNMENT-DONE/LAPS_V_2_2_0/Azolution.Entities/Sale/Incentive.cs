using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class Incentive
    {
        public int IncentiveId { get; set; }
        public int NumberOfSale { get; set; }
        public decimal IncentiveAmount { get; set; }

        public int IsActive { get; set; }
    }
}
