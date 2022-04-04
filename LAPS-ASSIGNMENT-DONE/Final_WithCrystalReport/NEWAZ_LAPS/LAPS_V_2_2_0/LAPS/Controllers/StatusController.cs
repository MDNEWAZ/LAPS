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
    public class StatusController : Controller
    {
        //
        // GET: /Status/

        //public ActionResult AccessControlSettings()
        //{
        //    if (Session["CurrentUser"] != null)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //}
        IStatusRepository _statusRepository = new StatusService();
        IGroupRepository _groupRepository = new GroupService();
        public ActionResult GetStatusByMenuId(int menuId)
        {

            var res = _statusRepository.GetStatusByMenuId(menuId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActionByStatusIdForGroup(int statusId)
        {

            var res = _statusRepository.GetActionByStatusId(statusId);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActionByStatusId(int statusId)
        {

            var res = _statusRepository.GetActionByStatusId(statusId);

            var results = new
            {
                Items = res,
                TotalCount = res.Count
            };

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActionButtonForSalaryDefaultState()
        {
            
            
          

            var salryMenuId = Convert.ToInt32(ConfigurationSettings.AppSettings["salaryMenuId"]);
            var objStatus = _statusRepository.GetDefaultStatusByMenuId(salryMenuId);
            Users objUser = ((Users)(Session["CurrentUser"]));
            var res = _statusRepository.GetActionByStateIdAndUserId(objStatus.WFStateId, objUser.UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

       


        public ActionResult GetLeaveStatus()
        {
            var menuIdFromAppConfig = ConfigurationSettings.AppSettings["leaveMenuId"];
            int menuId = Convert.ToInt32(menuIdFromAppConfig);

            var res = _statusRepository.GetStatusByMenuId(menuId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

       

        public ActionResult GetActionByStateIdAndUserId(int stateId)
        {
            try
            {
                Users objUser = ((Users)(Session["CurrentUser"]));

                var res = _statusRepository.GetActionByStateIdAndUserId(stateId, objUser.UserId);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

     

        public ActionResult GetAccessPermissionForCurrentUserForHrAccountsModule()
        {
            

            Users objUser = ((Users)(Session["CurrentUser"]));

            var hrAccountsModule = ConfigurationSettings.AppSettings["hrAccountsModuleId"];
            int moduleId = Convert.ToInt32(hrAccountsModule);



            var res = _groupRepository.GetAccessPermisionForCurrentUser(moduleId, objUser.UserId);

            return Json(res,JsonRequestBehavior.AllowGet);
        }

       

        #region Wrok Flow

        public ActionResult WorkFlowStatus()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        //These two line is called for work with audit trail
        IAuditHendler hendler = new IAuditHendler();
        AuditTrailDataService aService = new AuditTrailDataService();

        public ActionResult GetWorkFlowSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            
            var wrokFlowList = _statusRepository.GetWorkFlowSummary(skip, take, page, pageSize, sort, filter);

            //var results = new
            //{
            //    Items = wrokFlowList,
            //    TotalCount = wrokFlowList.Count > 0 ? wrokFlowList[0].TotalCount : 0
            //};

            return Json(wrokFlowList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMenu()
        {
           
            var MenuList = _statusRepository.GetMenu();

            return Json(MenuList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public string SaveState(string stateObj)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            stateObj = stateObj.Replace("^", "&");
            var state = (WFState) Newtonsoft.Json.JsonConvert.DeserializeObject(stateObj, typeof (WFState));

          
            var res = _statusRepository.SaveState(state);
            var stateId = state.WFStateId;
            //Audittail
            var audit = hendler.GetAuditInfo(objUser.UserId, "Save/Update WFState", stateId, res);
            aService.SendAudit(audit);
            return res;
        }

        [HttpPost]
        public string SaveAction(string ActionObj)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            ActionObj = ActionObj.Replace("^", "&");
            var action = (WFAction)Newtonsoft.Json.JsonConvert.DeserializeObject(ActionObj, typeof(WFAction));

            var res = _statusRepository.SaveAction(action);
            var actionId = action.WFActionId;
            //Audittail
            var audit = hendler.GetAuditInfo(objUser.UserId, "Save/Update WFAction", actionId, res);
            aService.SendAudit(audit);

            return res;
        }

        public string DeleteStatusByActionId(int actionId)
        {
            var res = "";
            Users objUser = ((Users)(Session["CurrentUser"]));
            try
            {
              
                res = _statusRepository.DeleteStatusByActionId(actionId);
            }
            catch (Exception exception)
            {
                res = exception.Message;
            }
            //Audittail
            var audit = hendler.GetAuditInfo(objUser.UserId, "Delete Status By Action Id", "Delete", res);
            aService.SendAudit(audit);

            return res;

        }

        #endregion 



    }
}
