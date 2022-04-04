using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Laps.Product.DataService.DataService;
using Laps.Replacement.DataService.DataService;
using Laps.Replacement.Service.Interface;
using Laps.Stock.DataService.DataService;
using LapsUtility;
using Utilities;

namespace Laps.Replacement.Service.Service
{
    public class ReplacementService : IReplacementRepository
    {
        ReplacementDataService _dataService = new ReplacementDataService();
        public string ReplaceProduct(Azolution.Entities.Sale.Replacement objReplacementInfo, Users user)
        {
            return _dataService.ReplaceProduct(objReplacementInfo,user);
        }

        public object GetReplacementInfoByInvoiceNo(GridOptions options, string invoiceNo)
        {
            return _dataService.GetReplacementInfoByInvoiceNo(options,invoiceNo);
        }

        public List<Installment> GetInstallmentID(string invoiceNo)
        {
            return _dataService.GetInstallmentId(invoiceNo);
        }

        public string SaveReplacementForCustomerService(ReplacementCs objReplacementCs, List<ReplaceItem> objItemList, Users user)
        {
            var companyBranchInfo = _dataService.GetBranchInfoByBranchId(objReplacementCs.BranchId);
            return _dataService.SaveReplacementForCustomerService(objReplacementCs, objItemList, companyBranchInfo, user);
        }

        public StockBalance checkExistStockBalanceByItemId(int itemId, int branchId, int stockCategoryId)
        {
            StockDataService _stockDataService = new StockDataService();
            var branchInfo = _dataService.GetBranchInfoByBranchId(branchId);
            var companyId = 0;
            var branch = 0;
             if (branchInfo.CompanyStock == 1)
            {

                if (branchInfo.CompanyType == "MotherCompany")
                {
                    companyId = branchInfo.CompanyId;
                }
                else
                {
                    companyId = branchInfo.RootCompanyId;
                }

                branchId = 0;
            }
            else
             {
                 companyId = branchInfo.CompanyId;
                 branchId = branchInfo.BranchId;
             }

            return _stockDataService.checkExistStockBalanceByItemId(itemId, companyId, branchId, stockCategoryId);

        }

        public CustomerPackage GetCustomerAndPackageInfoByCustomerCode(string customerCode, Users user)
        {
            return _dataService.GetCustomerAndPackageInfoByCustomerCode(customerCode, user);
        }

        public SalePackageInfo GetPackageSaleInfo(int modelId, string customerCode)
        {
            return _dataService.GetPackageSaleInfo(modelId, customerCode);
        }
    }
}
