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
using Laps.Stock.Service.Interface;
using Laps.Stock.Service.Service;
using LapsUtility;
using Utilities;

namespace LAPS.Controllers
{
    public class StockController : Controller
    {
        readonly IProductRepository _productRepository = new ProductService();
        IStockRepository _stockRepository = new StockService();
        //
        // GET: /Stock/
        public ActionResult StockManager() 
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            return RedirectToAction("Logoff", "Home");
        }
        public ActionResult StockPartial()
        {
            return PartialView("StockDetails");
        }
        public string GetAProductStock(GridOptions options, int modelId, int branchId, int stockCategoryId)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
           //return _productRepository.GetAProductStock(options, modelId);
            return _stockRepository.GetAProductStock(options, modelId, branchId, objUser, stockCategoryId);
        }
        public JsonResult SaveStock( List<Stock> objStockItemList,int branchId)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            if (objUser.IsViewer == 1) { return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet); }

            return Json(_stockRepository.SaveStock(objStockItemList, objUser, branchId), JsonRequestBehavior.AllowGet);
        }
        public string GetAllStockItemsByItemId(GridOptions options, int itemId)
        {
          
            return _stockRepository.GetAllStockItemsByItemId(options, itemId);
        }

        public JsonResult SaveStockAdjustment(List<StockAdjustment> objStockAdjList, int stockCategoryId, int branchId)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            if (objUser.IsViewer == 1) { return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet); }

            return Json(_stockRepository.SaveStockAdjustment(objStockAdjList, objUser, stockCategoryId,branchId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckStock(int modelId)
        {
             return Json(_stockRepository.CheckStock(modelId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult checkExistStockBalanceByItemId(int itemId, int stockCategoryId,int branchId)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            //var companyId = 0;
            //var branchId = 0;
         
            //if (objUser.CompanyStock == 1)
            //{
               
            //    if (objUser.CompanyType == "MotherCompany")
            //    {
            //        companyId = objUser.CompanyId;
            //    }
            //    else
            //    {
            //        companyId = objUser.RootCompanyId;
            //    }
                
            //    branchId = 0;
            //}
            //else
            //{
                
            //    if (objUser.CompanyType == "MotherCompany")
            //    {
            //        companyId = objUser.CompanyId;
            //    }
            //    else
            //    {
            //        companyId = objUser.RootCompanyId;
            //    }
            //    branchId = objUser.ChangedBranchId == 0 ? objUser.BranchId : objUser.ChangedBranchId;
            //}

            return Json(_stockRepository.checkExistStockBalanceByItemId(itemId, branchId, stockCategoryId, objUser), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckExistStockByModelId(int modelId)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            
            return Json(_stockRepository.CheckExistStockByModelId(modelId,objUser), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllStockModelId(GridOptions options,int modelId)
        {
            var data = _stockRepository.GetAllStockModelId(options, modelId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
