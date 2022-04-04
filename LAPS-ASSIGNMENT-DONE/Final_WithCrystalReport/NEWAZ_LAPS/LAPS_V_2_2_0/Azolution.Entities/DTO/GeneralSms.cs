using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;

namespace Azolution.Entities.DTO
{
   public class GeneralSms
    {
       public string CommissionMonthYear { get; set; }
       public decimal TotalAmount { get; set; }
       public int SmsType { get; set; }

       public LicenseInfo license { get; set; }
    }
}
