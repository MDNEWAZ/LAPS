using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Product.Service.Interface;
using Laps.Product.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        readonly IProductRepository _productRepository;

        public ProductController()
        {
            _productRepository = new ProductService();
        }

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ActionResult Product() 
        {
            if (Session["CurrentUser"]!=null)
            {
                return View();
            }
            return RedirectToAction("Logoff", "Home");
        }

        public ActionResult ProductPartial()
        {
            return PartialView("_ProductPopupPartial");
        }
        public string GetAllProduct(GridOptions options)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            return _productRepository.GetAllProduct(options,objUser);
        }
        public string GetProductType()
        {
            return _productRepository.GetAllProductType();
        }
        public JsonResult SaveProduct(Product strObjProductInfo, List<ProductItems> productItemList, List<ProductItems> removeItemList)
        {
            return Json(_productRepository.SaveProduct(strObjProductInfo,productItemList,removeItemList),JsonRequestBehavior.AllowGet);
        }
        public string GetAProductLicense(GridOptions options, int productId)
        {
            return _productRepository.GetAProductLicense(options,productId);
        }
        
        public JsonResult GetAProduct(string productCode)
        {
            return Json(_productRepository.GetAProduct(productCode),JsonRequestBehavior.AllowGet);
        }
        public string GetAllProdeuctModel()
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            return _productRepository.GetAllProductModel(objUser.RootCompanyId);
        }
        public string GetAProductModel(int modelId)
        {
            return _productRepository.GetAProductModel(modelId);
        }
        public JsonResult GetProductCustomerInfoByInvoiceNo(string invoiceNo,int saleId)
        {
            return Json(_productRepository.GetProductCustomerInfoByInvoiceNo(invoiceNo,saleId), JsonRequestBehavior.AllowGet);
        }

        public string GetAllProductItemByModelId(GridOptions options, int modelId)
        {
            return _productRepository.GetAllProductItemByModelId(options,modelId);
        }

        public string GetProductItemByModelId(int modelId)
        {
            return _productRepository.GetProductItemByModelId(modelId);
        }

        public string GetStockedProductItemByModelId(int modelId, int branchId)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));

            return _productRepository.GetStockedProductItemByModelId(modelId, branchId, objUser);
        }

        public string GetItemSlNoBySalesItemId(int salesItemId)
        {
            return _productRepository.GetItemSlNoBySalesItemId(salesItemId);
        }

        public JsonResult GetAllPackageByCompany(int packageType)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            return Json(_productRepository.GetAllPackageByCompany(packageType,objUser), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllPackageByTypeIdAndPackageId(int typeId, int packageType)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            return Json(_productRepository.GetAllPackageByTypeId(typeId,packageType, objUser), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemsOldSLNo(int salesItemId)
        {
            return Json(_productRepository.GetItemsOldSLNo(salesItemId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductItemsByPackage(string model)
        {
            return Json(_productRepository.GetProductItemsByPackage(model),JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductItemCodeData()
        {
            return Json(_productRepository.GetProductItemCodeData(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveItemCode(string itemCode)
        {
            return Json(_productRepository.SaveItemCode(itemCode), JsonRequestBehavior.AllowGet);
        }

    }
}
