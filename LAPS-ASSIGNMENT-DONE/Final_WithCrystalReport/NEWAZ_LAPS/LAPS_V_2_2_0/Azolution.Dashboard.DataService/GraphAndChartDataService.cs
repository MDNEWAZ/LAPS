using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Utilities;

namespace Azolution.Dashboard.DataService
{
    public class GraphAndChartDataService
    {
        public List<StockGraphAndChart> GetMonthWiseStockData(Users objUser,int stockCategory)
        {
            string sql = string.Format(@"Select SUM(Quantity)SaleStockQty,CONVERT(VARCHAR(2),MONTH(ReceiveDate)) + '/' + CONVERT(VARCHAR(4),YEAR(ReceiveDate)) As ReceiveDate From Sale_Stock
                    Where StockCategoryId={0}
                    group by CONVERT(VARCHAR(2),MONTH(ReceiveDate)) + '/' + CONVERT(VARCHAR(4),YEAR(ReceiveDate)),StockCategoryId
                    order by CONVERT(VARCHAR(2),MONTH(ReceiveDate)) + '/' + CONVERT(VARCHAR(4),YEAR(ReceiveDate)) ",stockCategory);

            var data = Data<StockGraphAndChart>.DataSource(sql);
            return data;
        }
    }
}
