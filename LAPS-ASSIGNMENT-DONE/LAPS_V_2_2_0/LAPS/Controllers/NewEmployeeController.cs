using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class NewEmployeeController : Controller
    {
        //
        // GET: /NewEmployee/

        public ActionResult GetEducation(GridOptions options)
        {
            IEmployeeRepository repository = new EmployeeService();
            var employeeList = repository.GetEducation(options);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployeesSetting()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("../Employees/EmployeesSettings");
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

    

        public string NewSaveEmployee(string employee)
        {
            var res = "";
            Users objUsers = ((Users)(Session["CurrentUser"]));
            employee = employee.Replace("^", "&");
            IEmployeeRepository repository = new EmployeeService();
            var employees =
                    (Employee)Newtonsoft.Json.JsonConvert.DeserializeObject(employee, typeof(Employee));
            try
            {
                res = repository.NewSaveEmployee(employees);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
          
            return res;
        }

        public ActionResult GetEducationSummary(GridOptions options, int id)
        {
            IEmployeeRepository repository = new EmployeeService();
            var employeeList = repository.GetEducationSummary(options,id);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExperienceSummary(GridOptions options, int id)
        {
            IEmployeeRepository repository = new EmployeeService();
            var experienceList = repository.GetExperienceSummary(options, id);
            return Json(experienceList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeInfoForCheckBox(int id)
        {
            IEmployeeRepository repository = new EmployeeService();
            var employeeInfoList = repository.GetEmployeeInfoForCheckBox(id);
            return Json(employeeInfoList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeTwoSummary(GridOptions options)
        {
            IEmployeeRepository repository = new EmployeeService();
            var employeeList = repository.GetAllEmployeeSummary(options);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

        //public string UpdateEmployee(Employee employee)
        //{
            
        //}

        //public ActionResult GetExperienceById(int id)
        //{
        //    IEmployeeRepository repository = new EmployeeService();
        //    Users objUser = ((Users)(Session["CurrentUser"]));
        //    var experienceList = repository.GetExperienceById(id, objUser);
        //    var results = new
        //    {
        //        Items = experienceList,
        //        TotalCount = experienceList.Count > 0 ? experienceList[0].TotalCount : 0
        //    };

        //    return Json(results, JsonRequestBehavior.AllowGet);
        //}

    }

}
