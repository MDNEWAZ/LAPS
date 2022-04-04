using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;

namespace Azolution.Core.Service.Service
{
    public class NotificationService : INotificationRepository
    {
        NotificationDataService _notificationData = new NotificationDataService();
        public List<NotificationInfo> NoticeForNotification(int userId)
        {
            return _notificationData.NoticeForNotification(userId);
        }
        public int NoticeCountForNotification(int userId)
        {
            return _notificationData.NoticeCountForNotification(userId);
        }

        public string MarkAsReadBySmsNotificationId(int smsNotificationId, int userId)
        {
            return _notificationData.MarkAsReadBySmsNotificationId(smsNotificationId, userId);
        }

        public List<SmsNotificationInfo> SmsSentNoticeForNotification(int userId)
        {
            return _notificationData.SmsSentNoticeForNotification(userId);
        }

        public List<SmsNotificationInfo> SmsFailNoticeForNotification(int userId)
        {
            return _notificationData.SmsFailNoticeForNotification(userId);
        }

        public int SmsSentNoticeCountForNotification(int userId)
        {
            return _notificationData.SmsSentNoticeCountForNotification(userId);
        }

        public int SmsFailNoticeCountForNotification(int userId)
        {
            return _notificationData.SmsFailNoticeCountForNotification(userId);
        }

        public int UnRecognizeCollSmsNoticeCountForNotification(int userId)
        {
            return _notificationData.UnRecognizeCollSmsNoticeCountForNotification(userId);
        }

        public int UnRecognizeSaleNoticeCountForNotification(int userId)
        {
            return _notificationData.UnRecognizeSaleNoticeCountForNotification(userId);
        }

        public int UnRecognizeSmsNoticeCountForNotification(int userId)
        {
            return _notificationData.UnRecognizeSmsNoticeCountForNotification(userId);
        }
    }
}
