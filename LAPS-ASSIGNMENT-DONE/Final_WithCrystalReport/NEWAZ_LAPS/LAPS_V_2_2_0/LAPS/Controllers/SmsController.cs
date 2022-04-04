using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Common.Helper;
using Azolution.Entities.Sale;
using SmsService;
using Utilities;

namespace LAPS.Controllers
{
    public class SmsController : Controller
    {
        private readonly ISmsManager _smsManager;
        public SmsController()
        {
            _smsManager=new SmsManager();
        }

        public ActionResult SaleRequestBySms()
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
        public ActionResult CollectionSms()
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


        public ActionResult GetAllUnrecognizedSms(GridOptions options, string smsDateFrom, string smsDateTo)
        {
            
            try
            {
                var data = _smsManager.GetAllUnRecognizedSms(options, smsDateFrom,smsDateTo);
               
                var gridData = new 
                {
                    Items = data,
                    TotalCount = data.Count
                };

                return Json(gridData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult EditSms(SalesSms sms)
        {
            try
            {
                var data = _smsManager.GetSmsTextByType(1);
                return Json(_smsManager.UpdateSms(sms),JsonRequestBehavior.AllowGet);
               

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult GetAllUnrecognizedCollectionSms(string receiveDateFrom, string receiveDateTo)
        {
            try
            {
                var data = _smsManager.GetAllUnrecognizedCollectionSms(receiveDateFrom, receiveDateTo);

                var gridData = new
                {
                    Items = data,
                    TotalCount = data.Count
                };

                return Json(gridData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult EditCollectionSms(SMSRecieved sms)
        {
            try
            {
                return Json(_smsManager.EditCollectionSms(sms), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
