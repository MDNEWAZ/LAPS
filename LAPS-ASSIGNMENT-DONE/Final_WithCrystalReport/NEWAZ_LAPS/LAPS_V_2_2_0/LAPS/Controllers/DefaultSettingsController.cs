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
    public class DefaultSettingsController : Controller
    {
        ICompanyRepository _companyRepository = new CompanyService();
        IDefaultSettingsRepository _defaultSettingsRepository= new DefaultSettingsService();
        //
        // GET: /Interest/

        public ActionResult DefaultSettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            return RedirectToAction("Logoff", "Home");
        }

        public string LoadAllCompanies(GridOptions options)
        {
            var user = ((Users)(Session["CurrentUser"]));
            int companyId = user.CompanyId;
            return _companyRepository.GetAllCompanies(options, companyId);
        }
        public JsonResult GetDefaultSettingsSummary(GridOptions options)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            CommonController commonController = new CommonController();
            var companies = commonController.GetCompaniesByHierecyFromSession(objUser);
            return Json(_defaultSettingsRepository.GetDefaultSettingsSummaryCompanies(options, companies), JsonRequestBehavior.AllowGet);

        }
        public JsonResult SaveDefaultSettings(Interest objInterestInfo)
        {
            var user = ((Users)(Session["CurrentUser"]));
            int userId = user.UserId;
            return Json(_defaultSettingsRepository.SaveDefaultSettings(objInterestInfo, userId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllInterestInfoByCompanyId(int companyId)
        {
            var user = ((Users)(Session["CurrentUser"]));
            if (companyId == 0)
            {
                companyId = user.CompanyId;
            }

            return Json(_defaultSettingsRepository.GetAllInterestInfoByCompanyId(companyId), JsonRequestBehavior.AllowGet);
        }
    } 
}
