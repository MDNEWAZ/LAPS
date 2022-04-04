using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Azolution.HumanResource.DataService.DataService;
using Laps.Stock.DataService.DataService;
using Laps.Stock.Service.Interface;
using MySql.Data.MySqlClient;
using Utilities;
using Utilities.Common.Json;

namespace Laps.Stock.Service.Service
{
    public class StockService : IStockRepository
    {
        StockDataService _dataService = new StockDataService();
        public string GetAProductStock(GridOptions options, int modelId, int branch, Users users, int stockCategoryId)
        {
            var jsonHelper = new JsonHelper();
            var companyId = 0;
            var branchId = 0;
            GetStockCompanyBranchParam(users, branch, ref companyId, ref branchId);


            var data = _dataService.GetAProductStock(options, modelId, branchId, users, stockCategoryId,companyId);
            return jsonHelper.GetJson(data);
        }

        public string GetAllStockItemsByItemId(GridOptions options, int itemId)
        {
            var jsonHelper = new JsonHelper();
            var data = _dataService.GetAllStockItemsByItemId(options, itemId);
            return jsonHelper.GetJson(data); ;
        }

        public string SaveStock(List<Azolution.Entities.Sale.Stock> objStockItemList, Users users, int branch)
        {
           
            var companyId = 0;
            var branchId = 0;
            GetStockCompanyBranchParam(users, branch, ref companyId, ref branchId);

            return _dataService.SaveStock(objStockItemList, users, companyId, branchId);
        }

        public  void GetStockCompanyBranchParam(Users users, int branch, ref int companyId, ref int branchId)
        {
            if (branch > 0)
            {
                BranchDataService _branchDataService = new BranchDataService();
                var companyBranchInfo = _branchDataService.GetCompanyBranchInfoByBranchId(branch);
                if (companyBranchInfo.CompanyStock != 1)
                {
                    companyId = companyBranchInfo.CompanyId;
                    branchId = companyBranchInfo.BranchId;
                }
                else
                {
                    companyId = companyBranchInfo.CompanyType == "MotherCompany"
                        ? companyBranchInfo.CompanyId
                        : companyBranchInfo.RootCompanyId;
                }
            }
            else
            {
                companyId = users.CompanyType == "MotherCompany" ? users.CompanyId : users.RootCompanyId;
            }
        }

        public string SaveStockAdjustment(List<StockAdjustment> objStockAdjList, Users users, int stockCategoryId, int branch)
        {
            var companyId = 0;
            var branchId = 0;
            GetStockCompanyBranchParam(users, branch, ref companyId, ref branchId);
            return _dataService.SaveStockAdjustment(objStockAdjList, users, stockCategoryId,companyId,branchId);
        }

        public bool CheckStock(int modelId)
        {
            return _dataService.CheckStock(modelId);
        }

        public StockBalance checkExistStockBalanceByItemId(int itemId, int branch, int stockCategoryId, Users users)
        {
            var companyId = 0;
            var branchId = 0;
            GetStockCompanyBranchParam(users, branch, ref companyId, ref branchId);
            return _dataService.checkExistStockBalanceByItemId(itemId, companyId, branchId, stockCategoryId);
        }

        public bool CheckExistStockByModelId(int modelId, Users objUser)
        {
            return _dataService.CheckExistStockByModelId(modelId, objUser);
        }

        public GridEntity<ProductItems> GetAllStockModelId(GridOptions options, int modelId)
        {
            return _dataService.GetAllStockModelId(options, modelId);
        }

        public List<MonthWiseStockGraph> GetSaleStockData(Users objUser,int companyId)
        {
            StockDataService stockDataService = new StockDataService();
            return stockDataService.GetSaleStockData(objUser, companyId);
        }

        public List<MonthWiseStockGraph> GetReplacementStockData(Users objUser, int companyId)
        {
            StockDataService stockDataService = new StockDataService();
            return stockDataService.GetReplacementStockData(objUser,companyId);
        }

        //New Stock Inventory Code by Rubel 
        public string ReduceStockInventoryForDealerbyDealerIdAndModelId(List<Azolution.Entities.Sale.Sale> objSalesObjList,Users saleUser, CommonConnection connection)
        {
            StockDataService stockDataService = new StockDataService();
            return stockDataService.ReduceStockInventoryForDealerbyDealerIdAndModelId(objSalesObjList,saleUser, connection);

        }

        public string ReduceStockInventoryForDealerbyDealerIdAndModelIdFromRemoteMySql(List<Azolution.Entities.Sale.Sale> objSalesObjList, Users saleUser)
        {
            StockDataService stockDataService = new StockDataService();
            return stockDataService.ReduceStockInventoryForDealerbyDealerIdAndModelIdFromRemoteMySql(objSalesObjList, saleUser);

        }

        public string ReduceStockInventoryForBranchbyBranchIdAndModelId(List<Azolution.Entities.Sale.Sale> objSalesObjList,Users saleUser, CommonConnection connection)
        {
            StockDataService stockDataService = new StockDataService();
            return stockDataService.ReduceStockInventoryForBranchbyBranchIdAndModelId(objSalesObjList,saleUser, connection);
        }

        public string ReduceStockInventoryForBranchbyBranchIdAndModelIdFromRemoteMySql(List<Azolution.Entities.Sale.Sale> objSalesObjList, Users saleUser)
        {
            StockDataService stockDataService = new StockDataService();
            return stockDataService.ReduceStockInventoryForBranchbyBranchIdAndModelIdFromRemoteMySql(objSalesObjList, saleUser);
        }


        public bool CheckStockInventoryDealerByDealerIdAndModelId(string salesRepId, int modelId)
        {
            StockDataService stockDataService = new StockDataService();
            return stockDataService.CheckStockInventoryDealerByDealerIdAndModelId(salesRepId, modelId);
        }

        public bool CheckStockInventoryBranchByBranchIdAndModelId(int branchId, int modelId)
        {
            StockDataService stockDataService = new StockDataService();
            return stockDataService.CheckStockInventoryBranchByBranchIdAndModelId(branchId, modelId);
        }
    }
}
