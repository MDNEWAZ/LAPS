using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;

namespace Azolution.Dashboard.Service.Interface
{
    public interface IGraphAndChartRepository
    {
        List<StockGraphAndChart> GetMonthWiseStockData(Users objUser);
    }
}
