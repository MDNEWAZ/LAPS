using Azolution.Entities.HumanResource;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;


namespace LAPS.Controllers
{
    public class PersonalDetailsController : Controller
    {
        //
        // GET: /PersonalDetails/Azolution.

        IPersonalDetailsRepository _personalRepository = new PersonalDetailsService();
        public ActionResult PersonDetails()
        {
            if (Session["CurrentUser"]!= null)
            {
                return View("~/Views/PersonalDetails/PersonalSettings.cshtml");

            }
            return RedirectToAction("Logoff", "Home");

        }

        public ActionResult SavePersonDetails(PersonalDetails objPerson)
        {
            var data = _personalRepository.SavePersonDetails(objPerson);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPersonalSummary( GridOptions options)
        {
            var data = _personalRepository.GetPersonalSummary(options);
            return Json(data);
        }

        public ActionResult DeletePersonalInfo(PersonalDetails objPerson)
        {
            var data =_personalRepository.DeletePersonalInfo(objPerson);
            return Json(data, JsonRequestBehavior.AllowGet);

        }   
    }
}
