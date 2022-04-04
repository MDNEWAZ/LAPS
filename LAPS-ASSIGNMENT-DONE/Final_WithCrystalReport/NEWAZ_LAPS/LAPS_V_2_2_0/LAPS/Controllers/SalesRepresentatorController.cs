using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Azolution.Reports.Service.Interface;
using Azolution.Reports.Service.Service;
using Laps.SaleRepresentative.Service.Interface;
using Laps.SaleRepresentative.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class SalesRepresentatorController : Controller
    {
        //
        // GET: /SalesRepresentator/
        ISalesRepresentatorRepository _salesRepresentatorRepository= new SalesRepresentatorService();
        public ActionResult SalesRepresentatorSettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            return RedirectToAction("Logoff", "Home");
        }

        public JsonResult SaveSalesRepresentator(SalesRepresentator objSalesRepresentator)
        {
            return Json(_salesRepresentatorRepository.SaveSalesRepresentator(objSalesRepresentator), JsonRequestBehavior.AllowGet);
        }

        public string GetAllSalesRepresentator(GridOptions options)
        {
            return _salesRepresentatorRepository.GetAllSalesRepresentator(options);
        }

        public JsonResult GetSalesRepresentatorType()
        {
            return Json(_salesRepresentatorRepository.GetSalesRepresentatorType(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSalesRepresentatorCombo(int salesRepType=0)
        {
            var user = ((Users)(Session["CurrentUser"]));
            return Json(_salesRepresentatorRepository.GetAllSalesRepresentatorCombo(salesRepType, user), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSalesRepComboByCompanyBranchAndType(int salesRepType,int companyId,int branchId)
        {
            var user = ((Users)(Session["CurrentUser"]));
            return Json(_salesRepresentatorRepository.GetSalesRepComboByCompanyBranchAndType(salesRepType, companyId, branchId,user), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllSalesRepresentatorById(string salesRepId)
        {
            return Json(_salesRepresentatorRepository.GetAllSalesRepresentatorById(salesRepId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSalesRepresentatorByCompanyAndBranch(int companyId,int branchId)
        {
            var user = ((Users)(Session["CurrentUser"]));
            return Json(_salesRepresentatorRepository.GetSalesRepresentatorByCompanyAndBranch(companyId, branchId,user), JsonRequestBehavior.AllowGet);
        }

    }
}
