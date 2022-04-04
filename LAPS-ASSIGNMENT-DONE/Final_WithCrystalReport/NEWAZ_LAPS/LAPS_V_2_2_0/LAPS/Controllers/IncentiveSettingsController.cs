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
    public class IncentiveSettingsController : Controller
    {
        //
        // GET: /Incentive/
        IIncentiveSettingsRepository _incentiveSettingsRepository = new IncentiveSettingsService();
        public ActionResult IncentiveSettings()
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

        public ActionResult SaveIncentiveSettings(Incentive objIncentive)
        {
           return Json(_incentiveSettingsRepository.SaveIncentiveSettings(objIncentive), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIncentiveSettingsSummary(GridOptions options)
        {
            return Json(_incentiveSettingsRepository.GetIncentiveSettingsSummary(options), JsonRequestBehavior.AllowGet);
        }

    }
}
