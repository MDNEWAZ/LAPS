using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Collection.CollectionService.Interface;
using Laps.Collection.CollectionService.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class SimplePaymentCollectionController : Controller
    {
        public ActionResult SimplePaymentCollectionSettings()
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
        ISimplePaymentCollectionRepository _repository = new SimplePaymentCollectionService();
        public JsonResult GetCustomerInfoWithInstallmentAmount(string customerCode)
        {

            var objUser = ((Users)(Session["CurrentUser"]));
            var simplePaymentCollectionObj = new SimplePaymentCollection();
            simplePaymentCollectionObj.CustomerCode = customerCode;
            //simplePaymentCollectionObj.CompanyId = user.ChangedCompanyId;
            //simplePaymentCollectionObj.BranchId = user.ChangedBranchId;

            simplePaymentCollectionObj.CompanyId = objUser.ChangedCompanyId == 0 ? objUser.CompanyId : objUser.ChangedCompanyId;
            simplePaymentCollectionObj.BranchId = objUser.ChangedBranchId == 0 ? objUser.BranchId : objUser.ChangedBranchId;


            return Json(_repository.GetCustomerInfoWithInstallmentAmount(simplePaymentCollectionObj), JsonRequestBehavior.AllowGet);
        }

    public JsonResult SaveAsDraftPayment(SimplePaymentCollection objPaymentInfo)
        {
            var user = ((Users)(Session["CurrentUser"]));
            objPaymentInfo.CompanyId = user.ChangedCompanyId;
            objPaymentInfo.BranchId = user.ChangedBranchId;
            return Json(_repository.SaveAsDraftPayment(objPaymentInfo, user), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDraftedPaymentDataForGrid(GridOptions options, string collectionDate)
        {
            Users users = ((Users)(Session["CurrentUser"]));
            return Json(_repository.GetDraftedPaymentDataForGrid(null, collectionDate, users), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FinalSavePaymentCollection(List<PaymentReceivedInfo> objPaymentInfo)
        {
            string res = "";
            try
            {
                Users users = ((Users)(Session["CurrentUser"]));
                ICollectionRepository _collectionRepository = new CollectionService();
                foreach (var simplePaymentCollection in objPaymentInfo)
                {
                   
                    res = _collectionRepository.GetPaymentAndCollect(simplePaymentCollection, users.UserId);
                    if (res == "Success")
                    {
                        _repository.UpdateStatusForDrafted(simplePaymentCollection.SimplePaymentCollectionId);
                    }
                }
            }
            catch (Exception ex)
            {

                res = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}
