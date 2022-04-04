using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
   public class SMSSent
    {
        public int ID { get; set; }
        public string SMSText { get; set; }
        public string MobileNumber { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public int Status { get; set; }
        public int ReplyFor { get; set; }
        public string SimNumber { get; set; }
        public int NoOfTry { get; set; }
        public int MessageReference { get; set; }
    }
}
