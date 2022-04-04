using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Core
{
   public class SmsNotificationInfo
    {
       //SmsNotification
       public int SmsNotificationId { get; set; }
        public string SMSText { get; set; }
        public string MobileNumber { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public int Status { get; set; }
        public int ReplyFor { get; set; }
        public string SimNumber { get; set; }

       //SmsNotification user
        public int SmsNotificationUserId { get; set; }
        public int UserId { get; set; }
        public int ViewStatus { get; set; }
    }
}
