using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;

using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class DepartmentController : Controller
    {
        //
        // GET: /Department/

        public ActionResult DepartmentSettings()
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

        [HttpPost]
        public string SaveDepartment(string strobjDepartment)
        {
            var user = ((Users)(Session["CurrentUser"]));
            var res = "";
            var departmentId = 0;
            try
            {
                strobjDepartment = strobjDepartment.Replace("^", "&");
                IDepartmentRepository departmentService = new DepartmentService();
                
                var department =(Department) Newtonsoft.Json.JsonConvert.DeserializeObject(strobjDepartment, typeof (Department));
                res = departmentService.SaveDepartment(department);
                departmentId = department.DepartmentId;
            }
            catch(Exception ex)
            {
                res = ex.Message;
            }
            new AuditTrailDataService().SendAudit(new IAuditHendler().GetAuditInfo(user.UserId, "Save/Update Department", departmentId, res));
            return res;
        }

        public ActionResult GetDepartmentSummary(int companyID, int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            IDepartmentRepository departmentService = new DepartmentService();

            var departmentList = departmentService.GetDepartmentSummary(companyID, skip, take, page, pageSize, sort, filter);



            return Json(departmentList);
        }

        public ActionResult GetDepartmentByCompanyId(int companyId)
        {
            try
            {
                IDepartmentRepository departmentService = new DepartmentService();
                IQueryable<Department> departmentList = departmentService.GetDepartmentByCompanyId(companyId);
                return Json(departmentList, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
