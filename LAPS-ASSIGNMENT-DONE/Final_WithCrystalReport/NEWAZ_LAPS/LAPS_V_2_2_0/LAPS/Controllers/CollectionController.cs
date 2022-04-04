using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Collection.CollectionService.Interface;
using Laps.Collection.CollectionService.Service;
using LapsUtility;
using Utilities;

namespace LAPS.Controllers
{
    public class CollectionController : Controller
    {
        //
        // GET: /Collection/
        ICollectionRepository _collectionRepository = new CollectionService();
        public ActionResult Collection()
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

        public ActionResult GetPaymentType()
        {
            return Json(_collectionRepository.GetPaymentType(), JsonRequestBehavior.AllowGet);
        }

      public JsonResult GetNextInstallmentByInvoiceNo(int invoiceNo)
        {
            return Json(_collectionRepository.GetNextInstallmentByInvoiceNo(invoiceNo), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCollection(GridOptions options)
        {
            var users = ((Users)(Session["CurrentUser"]));
            CommonController commonController = new CommonController();
            var companies = commonController.GetCompaniesByHierecyFromSession(users);

            return Json(_collectionRepository.GetAllCollection(options, companies), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllInstallmentByInvoiceId(string invoiceNo)
        {
            return Json(_collectionRepository.GetAllInstalmentByInvoiceNo(invoiceNo), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDownpaymentByInvoiceId(int saleId)
        {
            return Json(_collectionRepository.GetDownpaymentByInvoiceId(saleId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDownpaymentByInvoiceNo(string invoiceNo)
        {
            return Json(_collectionRepository.GetDownpaymentByInvoiceNo(invoiceNo), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendSmsByCustomerRating(string saleInvoice)
        {
            Users user = (Users)(Session["CurrentUser"]);

            return Json(_collectionRepository.SendSmsByCustomerRatingWhenLogin(saleInvoice, user), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDuePercentByInvoiceNo(string invoiceNo)
        {
            return Json(_collectionRepository.GetDuePercentByInvoiceNo(invoiceNo), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GenerateReleaseLisenceFromRootUser(ReleaseLisenceGenerate strobjReleaseLicenseInfo, int varificationType)
        {

            return Json(_collectionRepository.GenerateReleaseLisenceFromRootUser(strobjReleaseLicenseInfo, varificationType), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ResendLicenseCodeSms(CustomerWithLicenseCode objResendSms)
        {
            return Json(_collectionRepository.ResendLicenseCodeSms(objResendSms), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPaymentAndCollect(PaymentReceivedInfo paymentInfoObj)
        {
            Users users = ((Users)(Session["CurrentUser"]));
            if (users.IsViewer == 1) { return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet); }

            return Json(_collectionRepository.GetPaymentAndCollect(paymentInfoObj, users.UserId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCollectionHistoryByInvoice(GridOptions options, string invoiceNo)
        {
            return Json(_collectionRepository.GetCollectionHistoryByInvoice(options, invoiceNo), JsonRequestBehavior.AllowGet);
        }

    }
}
