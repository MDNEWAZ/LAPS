using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;

using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class DesignationController : Controller
    {
        private IDesignationRepository designationService = new DesignationService();
        //
        // GET: /Designation/

        public ActionResult DesignationSettings()
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


        public ActionResult GetDesignationByCompanyId(int companyId)
        {
            // IDesignationRepository designationService = new DesignationService();

            var designationList = designationService.GetDesignationByCompanyId(companyId);
            var results = new
            {
                Items = designationList,
                TotalCount = designationList.Count > 0 ? designationList[0].TotalCount : 0
            };

            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllDesignationByCompanyIdAndStatus(int companyId, int status)
        {
            //            IDesignationRepository designationService = new DesignationService();
            var designationList = designationService.GetAllDesignationByCompanyIdAndStatus(companyId, status);
            //var results = new
            //{
            //    Items = designationList,
            //    TotalCount = designationList.Count > 0 ? designationList[0].TotalCount : 0
            //};

            return Json(designationList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDesignationByCompanyIdForCombo(int companyId)
        {
            //    IDesignationRepository designationService = new DesignationService();
            var designationList = designationService.GetDesignationByCompanyId(companyId);


            return Json(designationList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string SaveDesignation(string strobjDesignation)
        {
            var res = "";
            Users user = ((Users)(Session["CurrentUser"]));
            var designationId = 0;
            try
            {
                strobjDesignation = strobjDesignation.Replace("^", "&");
                //    IDesignationRepository designationService = new DesignationService();
                var designation =
                    (Designation)Newtonsoft.Json.JsonConvert.DeserializeObject(strobjDesignation, typeof(Designation));
                res = designationService.SaveDesignation(designation);
                designationId = designation.DesignationId;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            new AuditTrailDataService().SendAudit(new IAuditHendler().GetAuditInfo(user.UserId, "Save/Update Designation", designationId, res));
            return res;
        }

        public ActionResult GetDesignationSummary(int companyID, int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));

            if (companyID == 0)
            {
                companyID = objUser.CompanyId;
            }

            //    IDesignationRepository designationService = new DesignationService();

            var designationList = designationService.GetDesignationSummary(companyID, skip, take, page, pageSize, sort, filter);



            return Json(designationList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GenerateDesignationByDepartmentIdCombo(int departmentId, int status)
        {
            //    IDesignationRepository designationService = new DesignationService();
            var designationList = designationService.GenerateDesignationByDepartmentIdCombo(departmentId,status);


            return Json(designationList, JsonRequestBehavior.AllowGet);
        }

    }

}
