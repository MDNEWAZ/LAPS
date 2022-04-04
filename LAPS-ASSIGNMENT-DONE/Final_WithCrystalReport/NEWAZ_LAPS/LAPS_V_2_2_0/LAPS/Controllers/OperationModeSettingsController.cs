using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.AdminSettings.Service.Interface;
using Laps.AdminSettings.Service.Service;

namespace LAPS.Controllers
{
    public class OperationModeSettingsController : Controller
    {
        //
        // GET: /OperationModeSettings/

        public ActionResult OperationModeSettings()
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

        public ActionResult SaveOperationModeSettings(OperationMode operationModeObj)
        {
            IDefaultSettingsRepository _defaultSettingsRepository =  new DefaultSettingsService();
            var user = ((Users)(Session["CurrentUser"]));
            return Json(_defaultSettingsRepository.SaveOperationModeSettings(operationModeObj,user), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOperationModeSettings()
        {
            IDefaultSettingsRepository _defaultSettingsRepository = new DefaultSettingsService();
            return Json(_defaultSettingsRepository.GetOperationModeSettings(), JsonRequestBehavior.AllowGet);
        }

    }
}
