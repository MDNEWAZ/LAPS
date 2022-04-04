using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;

using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Utilities;

namespace LAPS.Controllers
{
    public class GroupController : Controller
    {
        //
        // GET: /Group/

        public ActionResult GroupSettings()
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

        IGroupRepository _groupRepository = new GroupService();

        [HttpPost]
        public string SaveGroup(string strGroupInfo)
        {
            var strGroup = strGroupInfo.Replace("^", "&");
            var res = "";
            Users objUser = ((Users)(Session["CurrentUser"]));
            var groupId = 0;
            try
            {
                var objGroup = (Group)Newtonsoft.Json.JsonConvert.DeserializeObject(strGroup, typeof(Group));

                res = _groupRepository.SaveGroup(objGroup);
                groupId = objGroup.GroupId;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            new AuditTrailDataService().SendAudit(new IAuditHendler().GetAuditInfo(objUser.UserId, "Save/Update Group", groupId, res));
            return res;
        }

        public JsonResult GetGroupSummaryByCompanyIdWithPaging(int companyID, int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {

            var groupList = _groupRepository.GetGroupSummaryByCompanyIdWithPaging(companyID, skip, take, page, pageSize, sort, filter);


            return Json(groupList); 

        }

        public ActionResult GetGroupPermisionbyGroupId(int groupId)
        {

            var groupPermisionList = _groupRepository.GetGroupPermisionbyGroupId(groupId);
            return Json(groupPermisionList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGroupByCompanyId(int companyId)
        {


            var groupPermisionList = _groupRepository.GetGroupByCompanyId(companyId);
            return Json(groupPermisionList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllAccess()
        {


            var accessList = _groupRepository.GetAllAccess();
            return Json(accessList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccessPermisionForCurrentUser()
        {
            try
            {
                var salesModule = ConfigurationSettings.AppSettings["salesModuleId"];
                int moduleId = Convert.ToInt32(salesModule);
               
                Users objUser = ((Users)(Session["CurrentUser"]));

                var res = _groupRepository.GetAccessPermisionForCurrentUser(moduleId, objUser.UserId);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                throw exception;
                //return Json(exception.Message, JsonRequestBehavior.AllowGet);
            }
        }

       


    
    }
}
