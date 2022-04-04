using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Entities.Sale;
using Laps.AdminSettings.Service.Interface;
using Laps.AdminSettings.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class CreditPeriodController : Controller
    {
        //
        // GET: /CreditPeriod/

        public ActionResult CreditPeriodSettings()
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
        ICreditPeriodRepository _repository = new CreditPeriodService();
        public ActionResult SaveCreditPeriod(CreditPeriod objCreditPeriodInfo)
        {
            return Json(_repository.SaveCreditPeriod(objCreditPeriodInfo), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCreditPeriodSummary(GridOptions options)
        {
            return Json(_repository.GetCreditPeriodSummary(options), JsonRequestBehavior.AllowGet);
        }

    }
}
