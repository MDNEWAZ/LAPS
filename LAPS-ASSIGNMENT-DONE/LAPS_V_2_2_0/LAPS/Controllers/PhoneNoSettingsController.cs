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
    public class PhoneNoSettingsController : Controller
    {
        //
        // GET: /PhoneNoSettings/
        IPhoneSettingsRepository _phoneSettingsRepository = new PhoneSettingsService();
        public ActionResult PhoneNoSettings()
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

        public JsonResult GetAllPhoneSettings(GridOptions options)
        {
            return Json(_phoneSettingsRepository.GetAllPhoneSettings(options), JsonRequestBehavior.AllowGet);
        }



        public ActionResult SavePhoneSettings(PhoneNoSettings phoneObj)
        {
            return Json(_phoneSettingsRepository.SavePhoneSettings(phoneObj), JsonRequestBehavior.AllowGet);
        }

    }
}
