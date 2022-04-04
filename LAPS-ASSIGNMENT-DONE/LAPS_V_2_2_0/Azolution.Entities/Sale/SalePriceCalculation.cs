using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SalePriceCalculation
    {
        public decimal TotalPrice { get; set; }
        public decimal NetPrice { get; set; }
        public decimal DownPayment { get; set; }
    }
}
