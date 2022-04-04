using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Entities.Core;
using Laps.Sale.SaleService.Service;
using Laps.Sale.SaleService.Interface;
namespace LAPS.Controllers
{
    public class DiscountController : Controller
    {
        //
        // GET: /Discount/
        private IDiscountRepository _discountRepository = new DiscountService();
        
        //public ActionResult Index()
        //{
        //    return View();
        //}


        public JsonResult GetDiscountTypeCombo()
        {
            return Json(_discountRepository.GetDiscountTypeCombo(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiscountInfoByType()
        {
            var user = (Users)(Session["CurrentUser"]);
            return Json(_discountRepository.GetDiscountInfoByType(user), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiscountInfo(int saleId)
        {
            return Json(_discountRepository.GetDiscountInfo(saleId), JsonRequestBehavior.AllowGet);
        }
    }
}
