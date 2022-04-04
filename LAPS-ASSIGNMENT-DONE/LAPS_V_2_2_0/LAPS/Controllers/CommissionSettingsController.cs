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
    public class CommissionSettingsController : Controller
    {
        //
        // GET: /CommissionSettings/
        ICommissionSettingsRepository _commissionSettingsRepository = new CommissionSettingsService();
        public ActionResult CommissionSettings()
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

        public ActionResult SaveCommissionSettings(Commission objCommission)
        {
            return Json(_commissionSettingsRepository.SaveCommissionSettings(objCommission), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCommissionSettingsSummary(GridOptions options)
        {
            return Json(_commissionSettingsRepository.GetCommissionSettingsSummary(options), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCommissionAmountBySaleRepType(int salesRepTypeId)
        {
            return Json(_commissionSettingsRepository.GetCommissionAmountBySaleRepType(salesRepTypeId), JsonRequestBehavior.AllowGet);
        }

    }
}
