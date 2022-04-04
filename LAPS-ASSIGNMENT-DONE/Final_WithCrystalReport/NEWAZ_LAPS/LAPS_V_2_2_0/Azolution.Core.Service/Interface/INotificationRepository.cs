using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;

namespace Azolution.Core.Service.Interface
{
    public interface INotificationRepository
    {
        List<NotificationInfo> NoticeForNotification(int userId);
        int NoticeCountForNotification(int userId);
        string MarkAsReadBySmsNotificationId(int smsNotificationId, int userId);
        List<SmsNotificationInfo> SmsSentNoticeForNotification(int userId);
        List<SmsNotificationInfo> SmsFailNoticeForNotification(int userId);
        int SmsSentNoticeCountForNotification(int userId);
        int SmsFailNoticeCountForNotification(int userId);
        int UnRecognizeCollSmsNoticeCountForNotification(int userId);
        int UnRecognizeSaleNoticeCountForNotification(int userId);
        int UnRecognizeSmsNoticeCountForNotification(int userId);
    }
}
