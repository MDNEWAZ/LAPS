using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Stock.Service.Interface
{
    public interface IStockRepository
    {
        string GetAProductStock(GridOptions options, int modelId, int branchId, Users users, int stockCategoryId);
        string GetAllStockItemsByItemId(GridOptions options, int itemId);
        string SaveStock(List<Azolution.Entities.Sale.Stock> objStockItemList, Users users, int branchId);
        string SaveStockAdjustment(List<StockAdjustment> objStockAdjList, Users users, int stockCategoryId, int branchId);
        bool CheckStock(int modelId);
        StockBalance checkExistStockBalanceByItemId(int itemId, int branchId, int stockCategoryId, Users users);
        bool CheckExistStockByModelId(int modelId, Users objUser);

        GridEntity<ProductItems> GetAllStockModelId(GridOptions options, int modelId);

        List<MonthWiseStockGraph> GetSaleStockData(Users objUser, int companyId);
        List<MonthWiseStockGraph> GetReplacementStockData(Users objUser, int companyId);
    }
}
