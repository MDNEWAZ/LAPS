using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class LicenseInfo
    {
        public int LType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public decimal DownPay { get; set; }
        public string Code { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal DueAmount { get; set; }

       
    }
}
