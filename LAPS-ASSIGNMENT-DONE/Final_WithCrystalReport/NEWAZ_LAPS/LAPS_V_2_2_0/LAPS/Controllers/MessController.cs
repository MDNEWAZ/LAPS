using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LAPS.Controllers
{
    public class MessController : Controller
    {
        // GET: Mess
        public ActionResult Index()
        {
            return View();
        }
    }
}