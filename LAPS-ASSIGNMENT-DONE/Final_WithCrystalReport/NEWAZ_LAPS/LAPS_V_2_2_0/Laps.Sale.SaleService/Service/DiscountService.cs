using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Sale.SaleDataService.DataService;
using Laps.Sale.SaleService.Interface;

namespace Laps.Sale.SaleService.Service
{
    public class DiscountService:IDiscountRepository
    {
        DiscountDataService _discountDataService = new DiscountDataService();
        public object GetDiscountTypeCombo()
        {
            return _discountDataService.GetDiscountTypeCombo();
        }

        public object GetDiscountInfoByType(Users user)
        {
            return _discountDataService.GetDiscountInfoByType(user);
        }

        public Discount GetDiscountInfo(int saleId)
        {
            return _discountDataService.GetDiscountInfo(saleId);
        }
    }
}
