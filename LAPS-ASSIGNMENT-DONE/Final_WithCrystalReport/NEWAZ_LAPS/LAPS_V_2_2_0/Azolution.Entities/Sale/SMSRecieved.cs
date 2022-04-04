using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SMSRecieved
    {
        public Int64 ID { get; set; }
        public Int64 SMSIndex { get; set; }
        public DateTime RecievedDate { get; set; }
        public string SMSText { get; set; }
        public string FromMobileNumber { get; set; }
        public int Status { get; set; }
        public DateTime SystemDate { get; set; }

    }
}
