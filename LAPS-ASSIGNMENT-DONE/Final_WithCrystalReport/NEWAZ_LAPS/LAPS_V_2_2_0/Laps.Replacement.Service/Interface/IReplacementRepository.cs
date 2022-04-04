using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Replacement.Service.Interface
{
    public interface IReplacementRepository
    {
        string ReplaceProduct(Azolution.Entities.Sale.Replacement objReplacementInfo, Users user);
        object GetReplacementInfoByInvoiceNo(GridOptions options, string invoiceNo);
        List<Installment> GetInstallmentID(string invoiceNo);
        string SaveReplacementForCustomerService(ReplacementCs objReplacementCs, List<ReplaceItem> objItemList, Users user);
        StockBalance checkExistStockBalanceByItemId(int itemId, int branchId, int stockCategoryId);
        CustomerPackage GetCustomerAndPackageInfoByCustomerCode(string customerCode, Users user);
        SalePackageInfo GetPackageSaleInfo(int modelId, string customerCode);
    }
}
