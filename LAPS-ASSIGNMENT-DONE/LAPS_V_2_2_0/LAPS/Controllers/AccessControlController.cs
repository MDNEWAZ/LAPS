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
    public class AccessControlController : Controller
    {
        //
        // GET: /AccessControl/
        IStatusRepository _statusRepository = new StatusService();
        public ActionResult AccessControlSettings()
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

        public JsonResult GetAccessControlSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {

            var accessControlList = _statusRepository.GetAccessControlSummary(skip, take, page, pageSize, sort, filter);

            return Json(accessControlList);
        }

     
        public JsonResult GetAccessControlSummary2(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {

            var accessControlList = _statusRepository.GetAccessControlSummary2(skip, take, page, pageSize, sort, filter);

            //var obj = new 
            //{
            //    Items = accessControlList,
            //    TotalCount = accessControlList.Count
            //};

            return Json(accessControlList);
        }

        [HttpGet]
        public JsonResult SelectAccessControl()
        {
            IQueryable<AccessControl> accessControlList = _statusRepository.SelectAllAccessControl();
            return Json(accessControlList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string SaveAccessControl(string strobjAccessControlInfo)
        {

            var res = "";
            Users usr = (Users)Session["CurrentUser"];
            strobjAccessControlInfo = strobjAccessControlInfo.Replace("^", "&");
            try
            {
                var accessControl = (AccessControl)Newtonsoft.Json.JsonConvert.DeserializeObject(strobjAccessControlInfo, typeof(AccessControl));

                res = _statusRepository.SaveAccessControl(accessControl);
                new AuditTrailDataService().SendAudit(new IAuditHendler().GetAuditInfo(usr.UserId, "Save/Update Access Control", accessControl.AccessId, res));
            }
            catch (Exception exception)
            {
                res = exception.Message;
            }
            return res;
        }


    }
}
