using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
   public class CollectionInfo
    {
       public string Invoice { get; set; }
       public decimal DownPay { get; set; }
       public decimal ReceiveAmount { get; set; }
       public decimal DownPayDue { get; set; }

       public decimal DueAmount { get; set; }
    }
}
