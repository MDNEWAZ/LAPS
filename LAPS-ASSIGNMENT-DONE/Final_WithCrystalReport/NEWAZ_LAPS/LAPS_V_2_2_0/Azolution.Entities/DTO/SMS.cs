using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
   public class Sms
    {
       public int SmsId { get; set; }
       public int SmsType { get; set; }
       public string Salutation { get; set; }
       public string Greetings { get; set; }
       public string CustomerInfo { get; set; }
       public string DueInfo { get; set; }
       public string PaidInfo { get; set; }
       public string Unit { get; set; }
       public string CodeInfo { get; set; }
       public string Warning { get; set; }
       public string Request { get; set; }
       public string Thanking { get; set; }
       public string GeneralSms { get; set; }
       public string SmsTypeName { get; set; }
       public int Status { get; set; }
    }
}
