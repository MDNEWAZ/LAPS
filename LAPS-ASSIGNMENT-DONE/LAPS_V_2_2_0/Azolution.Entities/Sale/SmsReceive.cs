using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
   public class SmsReceive
    {
       public int SmsId { get; set; }
       public string Contant { get; set; }
       public string PaymentType { get; set; } 
       public string TransectionId { get; set; }
       public string ReceiveDate { get; set; }
       public string Mobile { get; set; }
       public int Flag { get; set; }
       public int IsRead { get; set; }
       public Product AProduct { get; set; } 
    }
}
