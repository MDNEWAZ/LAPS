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

using System.Web.Services.Description;
using Azolution.Core.Service.Interface;

//using LAPS.Reports.Entity;

namespace LAPS.Controllers
{
    public class DoctorsController : Controller
    {
        //
        // GET: /Doctors/

        readonly IDoctorRepository _doctorRepository =new DoctorService();

        public ActionResult Doctor()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("~/Views/Doctors/DoctorSetting.cshtml");
            }
            return RedirectToAction("Logoff", "Home");

        }

        public ActionResult GetAllDepartmentNameForCombo()
        {
            try
            {
                var data = _doctorRepository.GetAllDepartmentNameForCombo();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult SaveDoctor(Doctor doctor)
        {
            var data = _doctorRepository.SaveDoctor(doctor);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDoctorSummary(GridOptions options)
        {
            var data = _doctorRepository.GetDoctorSummary(options);
            return Json(data);
        }

        public ActionResult GetDoctorInfoReport()
        {
            try
            {
                var data = new ReportData<Doctor>();


                data.DataSource = _doctorRepository.GetDoctorInfoReport();
                data.RptName = "DoctorReportInfo.rpt";
                Session["report"] = data;
                return Json(Utilities.Operation.Success.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ActionResult GetDoctorEducationinfoSummary(GridOptions options, int id)
        {

            var doctorList = _doctorRepository.GetDoctorEducationinfoSummary(options, id);
            return Json(doctorList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteDoctor(int DoctorId)
        {
            var data = _doctorRepository.DeleteDoctor(DoctorId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
