using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Sale.SaleService.Interface;
using Laps.Sale.SaleService.Service;
using LapsUtility;
using Utilities;

namespace LAPS.Controllers
{
    public class WaitingForDiscountController : Controller
    {
        //
        // GET: /WaitingForDiscount/
        IWaitingForDiscountRepository _discountRepository = new WaitingForDiscountService();
        public ActionResult WaitingForDiscountSettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        public ActionResult GetWaitingForDiscountSummary(GridOptions options)
        {
           
            CommonController commonController = new CommonController();
            var user = (Users)(Session["CurrentUser"]);
            string companies = "";
            companies = commonController.GetCompaniesByHierecyFromSession(user);
            return Json(_discountRepository.GetWaitingForDiscountSummary(options, companies), JsonRequestBehavior.AllowGet);

        }
        public ActionResult ApproveWaitingForDiscount(WaitingForDiscount objWaitingForDiscount, int dpApplicabeStage)
        {
            IWaitingForDiscountRepository repository = new WaitingForDiscountService();
            var user = (Users)(Session["CurrentUser"]);
            if (user.IsViewer == 1) { return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet); }

            return Json(repository.ApproveWaitingForDiscount(objWaitingForDiscount, user, dpApplicabeStage), JsonRequestBehavior.AllowGet);
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

        public JsonResult GetDiscountTypeCombo()
        {
            return Json(_discountRepository.GetDiscountTypeCombo(), JsonRequestBehavior.AllowGet);
        }


    }
}
