using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;

namespace Laps.Sale.SaleService.Interface
{
   public interface IDiscountRepository
    {
      
        object GetDiscountTypeCombo();
        object GetDiscountInfoByType(Users user);
       Discount GetDiscountInfo(int saleId);
    }
}
