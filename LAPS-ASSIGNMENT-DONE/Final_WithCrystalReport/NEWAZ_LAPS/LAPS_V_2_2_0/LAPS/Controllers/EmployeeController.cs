using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;
using Azolution.Entities.Core;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult EmployeeSettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("../Employee/EmployeeSettings");
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }
        [HttpPost]
        public string SaveEmployee(string employee)
        {
            var res = "";
            Users objUsers = ((Users)(Session["CurrentUser"]));
            employee = employee.Replace("^", "&");
            IEmployeeRepository repository = new EmployeeService();
            var employees =
                    (Employee)Newtonsoft.Json.JsonConvert.DeserializeObject(employee, typeof(Employee));
            try
            {
                res = repository.SaveEmployee(employees);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
            new AuditTrailDataService().SendAudit(new IAuditHendler().GetAuditInfo(objUsers.UserId, "Save/Update Employee", employees.EmployeeID, res));
            return res;
        }

        public ActionResult GetEmployeeTwoSummary(GridOptions options)
        {
            IEmployeeRepository repository = new EmployeeService();
            var employeeList = repository.GetEmployeeSummary(options);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

       
    }
}
