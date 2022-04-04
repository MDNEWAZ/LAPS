using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Product.Service.Interface
{
   public interface IProductRepository
   {
       string SaveProduct(Azolution.Entities.Sale.Product aProduct, List<ProductItems> productItemList, List<ProductItems> removeItemList);
       string GetAllProduct(GridOptions options, Users objUser);
       List<Azolution.Entities.Sale.Product> GetAllProductWithPaging(object skip, object take, object page, object pageSize, object sort, object filter, int companyId);
       string GetAllProductType();
       string GetAProductLicense(GridOptions options, int productCode);
       Azolution.Entities.Sale.Product GetAProduct(string productCode);
       object GetProductCustomerInfoByInvoiceNo(string invoiceNo, int saleId);
       string GetAProductStock(GridOptions options, int modelId);
       string SaveStock(List<Azolution.Entities.Sale.Stock> strObjStockInfo);
       string GetAllProductModel(int rootCompanyId);
       string GetAProductModel(int modelId);
       string GetAllProductItemByModelId(GridOptions options, int modelId);
       string GetProductItemByModelId(int modelId);
       string GetStockedProductItemByModelId(int modelId,int branchId,Users users);
       string GetItemSlNoBySalesItemId(int salesItemId);

       List<Azolution.Entities.Sale.Product> GetAllPackageByCompany(int packageType, Users objUser);

       List<SalesItemDetails> GetItemsOldSLNo(int salesItemId);
       List<ProductItems> GetProductItemsByPackage(string model);
       List<ItemCodeType> GetProductItemCodeData();
       string SaveItemCode(string itemCode);
       List<Azolution.Entities.Sale.Product> GetAllPackageByTypeId(int typeId,int packageType, Users objUser);
   }
}
