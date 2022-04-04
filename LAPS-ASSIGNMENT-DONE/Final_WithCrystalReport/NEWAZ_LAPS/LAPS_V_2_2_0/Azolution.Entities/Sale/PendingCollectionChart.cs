using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class PendingCollectionChart
    {
        public decimal SalesPrice { get; set; }
        public decimal ReceiveAmount { get; set; }
        public string WarantyStartMonth { get; set; }
        public decimal OutStanding { get; set; }

        public decimal InstallmentReceiveAmount { get; set; }
        public decimal DownPaymentReceiveAmount { get; set; }
    }
}
