using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;

using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Utilities;

namespace LAPS.Controllers
{
    public class ModuleController : Controller
    {
        //
        // GET: /Module/

        public ActionResult ModuleSettings()
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

        IModuleRepository _moduleRepository = new ModuleService();

        public JsonResult GetModuleSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {


            var moduleList = _moduleRepository.GetModuleSummary(skip, take, page, pageSize, sort, filter);
          
            return Json(moduleList);
        }

        [HttpGet]
        public JsonResult SelectModule()
        {
            IQueryable<Module> moduleList = _moduleRepository.SelectAllModule();
            return Json(moduleList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string SaveModule(string strobjModuleInfo)
        { 
        
            var res = "";
            strobjModuleInfo = strobjModuleInfo.Replace("^", "&");
            Users user = ((Users)(Session["CurrentUser"]));
            var moduleId = 0;
            try

                {
                var module = (Module)Newtonsoft.Json.JsonConvert.DeserializeObject(strobjModuleInfo, typeof(Module));


                res = _moduleRepository.SaveModule(module);
                    moduleId = module.ModuleId;
                }
            catch (Exception exception)
            {
                res = exception.Message;
            }
            new AuditTrailDataService().SendAudit(new IAuditHendler().GetAuditInfo(user.UserId, "Save/Update Module", moduleId, res));
            return res;
        }

    }
}
