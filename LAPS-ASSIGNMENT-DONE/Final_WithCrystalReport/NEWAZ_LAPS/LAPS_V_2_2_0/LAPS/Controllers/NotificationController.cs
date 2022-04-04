using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Common.Validation;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;

namespace LAPS.Controllers
{
    public class NotificationController : Controller
    {
        //
        // GET: /Notification/

     INotificationRepository _notificationRepository = new NotificationService();
       
        //data
        public JsonResult NoticeForNotification()
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.NoticeForNotification(usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SmsSentNoticeForNotification()
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.SmsSentNoticeForNotification(usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SmsFailNoticeForNotification()
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.SmsFailNoticeForNotification(usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        //count//
        public ActionResult NoticeCountForNotification()
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.NoticeCountForNotification(usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SmsSentNoticeCountForNotification()
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.SmsSentNoticeCountForNotification(usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SmsFailNoticeCountForNotification()
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.SmsFailNoticeCountForNotification(usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnRecognizeCollSmsNoticeCountForNotification()
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.UnRecognizeCollSmsNoticeCountForNotification(usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnRecognizeSaleNoticeCountForNotification()
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.UnRecognizeSaleNoticeCountForNotification(usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnRecognizeSmsNoticeCountForNotification()
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.UnRecognizeSmsNoticeCountForNotification(usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MarkAsReadBySmsNotificationId(int smsNotificationId)
        {
            Users usr = (Users)Session["CurrentUser"];
            var data = _notificationRepository.MarkAsReadBySmsNotificationId(smsNotificationId, usr.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
