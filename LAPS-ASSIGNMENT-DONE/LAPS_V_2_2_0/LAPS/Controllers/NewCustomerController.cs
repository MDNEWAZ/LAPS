using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Entities.Core;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;

namespace LAPS.Controllers
{
    public class NewCustomerController : Controller
    {
        //
        // GET: /NewCustomer/

        public ActionResult NewCustomerSettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("../NewCustomer/NewCustomerSettings");
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        //public ActionResult GetAllCategoryForCombo()
        //{
        //    INewProductRepository repository = new NewProductService();
        //    Users objUser = ((Users)(Session["CurrentUser"]));

        //    var categoryList = repository.GetAllCategoryForCombo(0, objUser);


        //    return Json(categoryList, JsonRequestBehavior.AllowGet);
        //}

    }
}
