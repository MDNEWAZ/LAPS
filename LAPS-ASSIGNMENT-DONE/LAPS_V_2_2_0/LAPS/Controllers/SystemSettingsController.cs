using System.Web.Mvc;
using AuditTrail.Entity.DataService;
using Azolution.Common.Helper;
using Azolution.Common.Validation;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;

namespace LAPS.Controllers
{
    public class SystemSettingsController : Controller
    {
        //
        // GET: /SystemSettings/

        public ActionResult SystemSettings()
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
        ISystemSettingsRepository _systemSettingsRepository = new SystemSettingsService();
        public ActionResult GetSystemSettingsDataByCompanyId(int companyId)
        {


            var res = _systemSettingsRepository.GetSystemSettingsDataByCompanyId(companyId);
            if(res != null)
            {
                string decpass = EncryptDecryptHelper.Decrypt(res.ResetPass);
                res.ResetPass = decpass;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public string SaveSystemSettings(string strobjSystemSettingsInfo)
        {
            var validator = new ValidationHelper();
            strobjSystemSettingsInfo = strobjSystemSettingsInfo.Replace("^", "&");

            var systemSettings = (SystemSettings)Newtonsoft.Json.JsonConvert.DeserializeObject(strobjSystemSettingsInfo, typeof(SystemSettings));
          var systemId=  systemSettings.SettingsId;
            Users objUser = ((Users)(Session["CurrentUser"]));
            string validate = validator.ValidateUser("", systemSettings.MinLoginLength, systemSettings.ResetPass, systemSettings.MinPassLength, systemSettings.PassType, systemSettings.SpecialCharAllowed);
            if (validate == "Valid")
            {
                string decPass = EncryptDecryptHelper.Encrypt(systemSettings.ResetPass);
                systemSettings.ResetPass = decPass;
              
               
                systemSettings.UserId = objUser.UserId;

                var res = _systemSettingsRepository.SaveSystemSettings(systemSettings);

                new AuditTrailDataService().SendAudit(new IAuditHendler().GetAuditInfo(objUser.UserId, "Save/Update System Settings", systemId, res));

                return res;
            }
            else
            {
                return validate;
            }


        }

    }
}
