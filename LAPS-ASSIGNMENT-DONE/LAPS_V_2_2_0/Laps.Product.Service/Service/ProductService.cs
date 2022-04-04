using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Product.DataService.DataService;
using Laps.Product.Service.Interface;
using Laps.Stock.Service.Service;
using Utilities;
using Utilities.Common.Json;

namespace Laps.Product.Service.Service
{
    public class ProductService : IProductRepository
    {
        private readonly ProductDataService _aProductDataService = new ProductDataService();
        public string SaveProduct(Azolution.Entities.Sale.Product aProduct, List<ProductItems> productItemList, List<ProductItems> removeItemList)
        {
            string rv = "";

            //if (!CheckExistModel(aProduct.ModelId, aProduct.Model,aProduct.TypeId,aProduct))   //Previous Code
            //{
            //    rv = _aProductDataService.SaveProduct(aProduct, productItemList, removeItemList);
            //}
            //else
            //{
            //    rv = Operation.Exists.ToString();
            //}
            rv = _aProductDataService.SaveProduct(aProduct, productItemList, removeItemList);
           
            return rv;
        }

        private bool CheckExistModel(int modelId, string model, int typeId,Azolution.Entities.Sale.Product aProduct)
        {
            string condition = "";
            if (modelId == 0)
            {
                condition = string.Format(" Where Model='{0}' And TypeId = {1}", model,typeId);
            }
            else
            {
                condition = string.Format(" Where Model='{0}' And ModelId={1} And TypeId = {2} and ProductName = '{3}'and Description = '{4}' and IsActive = {5} and TotalPrice = {6}" +
                                          " and DownPayPercent = {7} and PackageType = {8} and DefaultInstallmentNo = {9} and IsDPFixedAmount = {10}",
                                          model, modelId, typeId, aProduct.ProductName.Trim(), aProduct.Description.Trim(), aProduct.IsActive, aProduct.TotalPrice,
                                          aProduct.DownPayPercent, aProduct.PackageType, aProduct.DefaultInstallmentNo, aProduct.IsDPFixedAmount);
            }

            return _aProductDataService.CheckExistModel(condition);
        }

        public string GetAllProduct(GridOptions options, Users objUser)
        {
            string condition = "";
            //var companyId = 0;
            //if (objUser.CompanyType == "MotherCompany")
            //{
            //    companyId = objUser.CompanyId;
            //}
            //else
            //{
            //    companyId = objUser.RootCompanyId;
            //}

            //if (objUser.LoginId != "admin")
            //{
            //    condition = " And CompanyId="+companyId;
            //}

            var jsonHelper = new JsonHelper();
            var data = _aProductDataService.GetAllProduct(options, condition);
            return jsonHelper.GetJson(data);
        }

        public List<Azolution.Entities.Sale.Product> GetAllProductWithPaging(object skip, object take, object page, object pageSize, object sort, object filter, int companyId)
        {
            return null;
        }

        public string GetAllProductType()
        {
            var jsonHelper = new JsonHelper();
            var data = _aProductDataService.GetAllProductType();
            return jsonHelper.GetJson(data);
        }

        public string GetAProductLicense(GridOptions options, int productCode)
        {
            var jsonHelper = new JsonHelper();
            var data = _aProductDataService.GetAProductLicense(options, productCode);
            return jsonHelper.GetJson(data);
        }

        public Azolution.Entities.Sale.Product GetAProduct(string productCode)
        {
            return _aProductDataService.GetAProduct(productCode);
        }

        public object GetProductCustomerInfoByInvoiceNo(string invoiceNo, int saleId)
        {
            return _aProductDataService.GetProductCustomerInfoByInvoiceNo(invoiceNo,saleId);
        }

        public string GetAProductStock(GridOptions options, int modelId)
        {
            var jsonHelper = new JsonHelper();
            var data = _aProductDataService.GetAProductStock(options, modelId);
            return jsonHelper.GetJson(data);
        }

        public string SaveStock(List<Azolution.Entities.Sale.Stock> aStock)
        {
            return _aProductDataService.SaveStock(aStock);
        }

        public string GetAllProductModel(int rootCompanyId)
        {
            var jsonHelper = new JsonHelper();
            var data = _aProductDataService.GetAllProductModel(rootCompanyId);
            return jsonHelper.GetJson(data);
        }

        public string GetAProductModel(int modelId)
        {
            var jsonHelper = new JsonHelper();
            var data = _aProductDataService.GetAProductModel(modelId);
            return jsonHelper.GetJson(data);
        }

        public string GetAllProductItemByModelId(GridOptions options, int modelId)
        {
            var jsonHelper = new JsonHelper();
            var data = _aProductDataService.GetAllProductItemByModelId(options, modelId);
            return jsonHelper.GetJson(data);
        }

        public string GetProductItemByModelId(int modelId)
        {
            var jsonHelper = new JsonHelper();
            var data = _aProductDataService.GetProductItemByModelId(modelId);
            return jsonHelper.GetJson(data);
        }

        public string GetStockedProductItemByModelId(int modelId, int branch, Users users)
        {
            var jsonHelper = new JsonHelper();
            StockService _stockService= new StockService();
            var companyId = 0;
            var branchId = 0;
            _stockService.GetStockCompanyBranchParam(users, branch, ref companyId, ref branchId);

            var data = _aProductDataService.GetStockedProductItemByModelId(modelId, branchId,companyId);
            return jsonHelper.GetJson(data);
        }

        public string GetItemSlNoBySalesItemId(int salesItemId)
        {
            var jsonHelper = new JsonHelper();
            var data = _aProductDataService.GetItemSlNoBySalesItemId(salesItemId);
            return jsonHelper.GetJson(data);
        }

        public List<Azolution.Entities.Sale.Product> GetAllPackageByCompany(int packageType, Users objUser)
        {
            string condition = "";
            //var companyId = 0;
            //if (objUser.CompanyType == "MotherCompany")
            //{
            //    companyId = objUser.CompanyId;
            //}
            //else
            //{
            //    companyId = objUser.RootCompanyId;
            //}

            //if (objUser.LoginId == "admin")
            //{
            //    condition = "";
            //}
            //else
            //{
            //    condition = " And CompanyId=" + companyId;
            //}

            var pakConditions = "";
            if (packageType > 0)
            {
                pakConditions = "And PackageType=" + packageType;
            }


            return _aProductDataService.GetAllPackageByCompany(pakConditions, condition);
        }

        public List<SalesItemDetails> GetItemsOldSLNo(int salesItemId)
        {
            return _aProductDataService.GetItemsOldSLNo(salesItemId);
        }

        public List<ProductItems> GetProductItemsByPackage(string model)
        {
            return _aProductDataService.GetProductItemsByPackage(model);
        }

        public List<ItemCodeType> GetProductItemCodeData()
        {
            return _aProductDataService.GetProductItemCodeData();
        }

        public string SaveItemCode(string itemCode)
        {
            string rv = "";
            if (!CheckExistItemCode(itemCode))
            {
                rv = _aProductDataService.SaveItemCode(itemCode);
            }
            else
            {
                rv = Operation.Exists.ToString();
            }
            return rv;
        }

        public List<Azolution.Entities.Sale.Product> GetAllPackageByTypeId(int typeId, int packageType,  Users objUser)
        {
            var condition = "";
            if (typeId > 0 && packageType > 0)
            {
                condition += string.Format(@" And TypeId = {0} and PackageType = {1}", typeId,packageType);
            }
            return _aProductDataService.GetAllPackageByTypeId(condition);
        }

        private bool CheckExistItemCode(string itemCode)
        {
            return _aProductDataService.CheckExistItemCode(itemCode);
        }

        public List<SalesItem> GetProductItemInfoBySaleId(int saleId)
        {
            return _aProductDataService.GetProductItemInfoBySaleId(saleId);
        }
    }
}
