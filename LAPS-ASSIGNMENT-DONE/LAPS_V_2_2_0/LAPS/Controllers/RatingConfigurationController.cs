using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.AdminSettings.Service.Interface;
using Laps.AdminSettings.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class RatingConfigurationController : Controller
    {
        //
        // GET: /Due/
       IRatingConfigurationRepository _configuration = new RatingConfigurationService();
        public ActionResult RatingConfiguration() 
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            return RedirectToAction("Logoff", "Home");
        }

        public string GetACompayDueKpi(GridOptions options)
        {
            var user = ((Users)(Session["CurrentUser"]));
            var companyId = user.CompanyId;
            return _configuration.GetACompayDueKpi(options, user);
        }
        public JsonResult DueSave(Due objDueInfo)
        {
            var user = ((Users)(Session["CurrentUser"]));
            int userId = user.UserId;
            return Json(_configuration.DueSave(objDueInfo, userId), JsonRequestBehavior.AllowGet);
        }
    }
}
