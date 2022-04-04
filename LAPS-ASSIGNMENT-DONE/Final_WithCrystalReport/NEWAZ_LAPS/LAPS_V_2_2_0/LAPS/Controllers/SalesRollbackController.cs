using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Sale.SaleService.Interface;
using Laps.Sale.SaleService.Service;
using LapsUtility;

namespace LAPS.Controllers
{
    public class SalesRollbackController : Controller
    {
        //
        // GET: /SaleRollback/
        ISalesInactive _salesInactiveRepository = new SaleInactive();
        public ActionResult SalesRollback()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        public ActionResult GetCustomerSalesInformation(int companyId, int branchId, string customerCode)
        {
            var data = _salesInactiveRepository.GetCustomerSalesInformation(customerCode, branchId, companyId);
            var objList = new
            {
                Items = data,
                TotalCount = data.Count()
            };
            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MakeInactive(SaleRollback rollbackInfo, int operationType)
        {
            var user = ((Users)(Session["CurrentUser"]));
            if (user.IsViewer == 1) { return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet); }

            if (operationType == 1)//1 Means Rollback(back to book stage) 2= BranchSwithc
            {
                return Json(_salesInactiveRepository.MakeInactive(rollbackInfo.SaleId, rollbackInfo.InvoiceNo), JsonRequestBehavior.AllowGet);
               
            }
            else
            {
                return Json(_salesInactiveRepository.SaleBranchSwitch(rollbackInfo, user), JsonRequestBehavior.AllowGet);
               
            }
        }
    }
}
