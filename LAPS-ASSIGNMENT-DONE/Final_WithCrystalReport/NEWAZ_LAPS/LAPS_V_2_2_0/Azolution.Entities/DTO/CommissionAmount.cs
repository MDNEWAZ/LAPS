using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
   public class CommissionAmount
    {
       public int SaleRepTypeId { get; set; }
       public decimal InstallmentSaleCommission { get; set; }
       public decimal CashSaleCommission { get; set; }

    }
}
