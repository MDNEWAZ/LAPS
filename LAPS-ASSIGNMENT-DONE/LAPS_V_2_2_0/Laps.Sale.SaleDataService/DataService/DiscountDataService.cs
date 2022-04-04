using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Sale.SaleDataService.DataService
{
    public class DiscountDataService
    {
        public List<DiscountType> GetDiscountTypeCombo()
        {
            string sql = string.Format(@"Select * From DiscountType Where IsActive=1");
            return Kendo<DiscountType>.Combo.DataSource(sql);
        }
        public Discount GetDiscountInfoByType(Users user)
        {
            var companyId = 0;
            if (user.ChangedCompanyId != 0)
            {
                companyId = user.ChangedCompanyId;
            }
            else
            {
                companyId = user.CompanyId;
            }

            string sql = string.Format(@"Select DefaultCashDiscount,DefaultAgentDiscount,CashDiscountPercentage,AgentDiscountPercentage From Sale_Interest Where Status=1 And CompanyId={0} ", companyId);
            return Data<Discount>.DataSource(sql).SingleOrDefault();
        }

        public Discount GetDiscountInfo(int saleId)
        {
            string sql = string.Format(@"Select * From Discount Where SaleId={0}", saleId);
            var data = Data<Discount>.GenericDataSource(sql).SingleOrDefault();
            return data;
        }
    }
}
