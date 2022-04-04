using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LAPS.Controllers
{
    public class TeachersController : Controller
    {
        //
        // GET: /Teachers/

        public ActionResult Teacher()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("~/Views/Teachers/TeacherSetting.cshtml");
            }
            return RedirectToAction("Logoff", "Home");

        }

	}
}