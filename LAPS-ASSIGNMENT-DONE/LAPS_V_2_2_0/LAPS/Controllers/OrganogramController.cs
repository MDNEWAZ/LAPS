using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Utilities;

namespace LAPS.Controllers
{
    public class OrganogramController : Controller
    {
        //
        // GET: /CompanyTree/
        private readonly IOrganogramRepository _organogramRepository = new OrganogramService();
        

        public ActionResult GetOrganogramTreeData()
        {

            var companyId = 0;
            Users user = ((Users)(Session["CurrentUser"]));
            companyId = user.CompanyId;
            var organogramList = _organogramRepository.GetOrganogramTreeData(companyId);

            //modefied by Ashraful 29/12/2014
            // get all hierarchay company id and load session
            //List<Company> companies = organogramList;
            //user.CompanyList = companies;
            //end of Ashraful

            //return Json(companyList, JsonRequestBehavior.AllowGet);
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var result = new ContentResult
            {
                Content = serializer.Serialize(organogramList),
                ContentType = "application/json"
            };

            //return Json(result, JsonRequestBehavior.AllowGet);
           // return result;

            return Json(organogramList, JsonRequestBehavior.AllowGet); 
        }

        
    }
}
