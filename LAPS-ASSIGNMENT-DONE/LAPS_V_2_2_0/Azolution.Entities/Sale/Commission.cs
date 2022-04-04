using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
   public class Commission
    {
       public int CommissionId { get; set; }
       public string SalesRepTypeName { get; set; }
       public int SaleRepTypeId { get; set; }
       public int SaleTypeId { get; set; }
       public string SaleTypeName { get; set; }
       public decimal ComissionAmount { get; set; }
      
       public int DayOfMonth { get; set; }

       public DateTime ConfirmationSmsDate { get; set; }

       public int IsActive { get; set; }
    }
}
