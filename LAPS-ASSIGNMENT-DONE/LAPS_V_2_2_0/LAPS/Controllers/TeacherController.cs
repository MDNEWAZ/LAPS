using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Entities.HumanResource;
using Utilities;

namespace LAPS.Controllers
{
    public class TeacherController : Controller
    {
        //
        // GET: /Teacher/
        TeacherService _teacherRepository = new TeacherService();

        public ActionResult Teacher()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("~/Views/Teacher/TeacherSetting.cshtml");
            }
            return RedirectToAction("Logoff", "Home");

        }

        public ActionResult GetAllDepartmentNameForCombo()
        {
            try
            {
                var data = _teacherRepository.GetAllDepartmentNameForCombo();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetAllDesignationNameForCombo()
        {
            try
            {
                var data = _teacherRepository.GetAllDesignationNameForCombo();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        
        }
	}
}