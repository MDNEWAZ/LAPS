using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Dashboard.DataService;
using Azolution.Dashboard.Service.Interface;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;

namespace Azolution.Dashboard.Service.Service
{
    public class GraphAndChartService:IGraphAndChartRepository
    {
        GraphAndChartDataService _chartDataService = new GraphAndChartDataService();
        public List<StockGraphAndChart> GetMonthWiseStockData(Users objUser)
        {
            //not used
            var saleStockData= _chartDataService.GetMonthWiseStockData(objUser,1);
            var replacementStockData = _chartDataService.GetMonthWiseStockData(objUser, 2);

            return null;
        }
    }
}
