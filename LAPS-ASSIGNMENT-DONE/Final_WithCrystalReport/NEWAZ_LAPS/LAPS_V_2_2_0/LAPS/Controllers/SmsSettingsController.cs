using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Entities.DTO;
using SmsService;
using Utilities;

namespace LAPS.Controllers
{
    public class SmsSettingsController : Controller
    {
        private ISmsManager _smsManager = new SmsManager();

        public ActionResult SmsSettings()
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

        public JsonResult GetAllSmsSettings(GridOptions options)
        {
            return Json(_smsManager.GetAllSmsSettings(options),JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSmsTypeDataForCombo()
        {
            return Json(_smsManager.GetSmsTypeDataForCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSmsSettings(Sms smsObj)
        {
            return Json(_smsManager.SaveSmsSettings(smsObj), JsonRequestBehavior.AllowGet);
        }

    }
}
