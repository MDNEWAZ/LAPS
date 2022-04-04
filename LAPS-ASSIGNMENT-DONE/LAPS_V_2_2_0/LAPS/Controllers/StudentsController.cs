using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Utilities;
using Azolution.Entities.Report;

//using LAPS.Reports.Entity;

namespace LAPS.Controllers
{
    public class StudentsController : Controller
    {
        //
        // GET: /Students/
        readonly IStudentRepository _studentRepository = new StudentService();

        public ActionResult Student()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("~/Views/Students/StudentSetting.cshtml");
            }
            return RedirectToAction("Logoff", "Home");
            
        }

        public ActionResult GetAllDepartmentNameForCombo()
        {
            try
            {
                var data = _studentRepository.GetAllDepartmentNameForCombo();
                return Json(data,JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                
                throw;
            } 
        }

        public ActionResult SaveStudent(Student student)
        {
            var data = _studentRepository.SaveStudent(student);
            return Json(data,JsonRequestBehavior.AllowGet) ;
        }

        public ActionResult GetStudentSummary(GridOptions options)
        {
            var data = _studentRepository.GetStudentSummary(options);
            return Json(data);
        }

        public ActionResult  GetStudentInfoReport()
        {
            try
            {
                var data = new ReportData<Student>();
            

                data.DataSource = _studentRepository.GetStudentInfoReport();
                data.RptName = "StudentInfoReport.rpt";
                Session["report"] = data;
                return Json(Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ActionResult GetStudentEducationinfoSummary(GridOptions options, int id)
        {

            var studentList = _studentRepository.GetStudentEducationinfoSummary(options, id);
            return Json(studentList, JsonRequestBehavior.AllowGet);
        }
    }
}
