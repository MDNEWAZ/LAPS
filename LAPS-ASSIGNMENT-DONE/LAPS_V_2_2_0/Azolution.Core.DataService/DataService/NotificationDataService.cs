using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Azolution.Core.DataService.DataService
{
   public class NotificationDataService
    {
       public List<NotificationInfo> NoticeForNotification(int userId)
       {
           string query = string.Format(@"Select * from Notification
left outer join NotificationUser on NotificationUser.NotificationId = Notification.NotificationId 
inner join Users on Users.UserId = NotificationUser.UserId and Users.IsNotify = 1
where (Notification.IsAnnonymous = 1  or (NotificationUser.UserId = {0})) and Notification.ExpiryDate > GETDATE() and (ViewStatus is null or ViewStatus = 0) ", userId);
           var data = Data<NotificationInfo>.DataSource(query);

           return data;
       }

       public int NoticeCountForNotification(int userId)
       {
           CommonConnection connection = new CommonConnection();

           string query = string.Format(@"Select Count(*) from Notification
left outer join NotificationUser on NotificationUser.NotificationId = Notification.NotificationId 
inner join Users on Users.UserId = NotificationUser.UserId and Users.IsNotify = 1
where (Notification.IsAnnonymous = 1  or (NotificationUser.UserId = {0})) and Notification.ExpiryDate > GETDATE() and (ViewStatus is null or ViewStatus = 0) ", userId);
           return connection.GetScaler(query);
       }

       public string MarkAsReadBySmsNotificationId(int smsNotificationId, int userId)
       {
           string res = "";
           string insertOrUpdateQuery = "";

           try
           {
               CommonConnection connection = new CommonConnection();

              // var ifAnnonymous = IfAnnonymous(smsNotificationId, userId, connection);
               //if (ifAnnonymous > 0)
               //{
                   insertOrUpdateQuery = string.Format(@"Update SmsNotificationUser set ViewStatus = 1 where UserId = {0} and SmsNotificationId = {1}", userId, smsNotificationId);
               //}
               //else if (ifAnnonymous == 0)
               //{
               //    insertOrUpdateQuery = string.Format(@"Insert Into SmsNotificationUser (SmsNotificationId,UserId,ViewStatus) values({0},{1},{2})", smsNotificationId, userId, 1);
               //}
               connection.ExecuteNonQuery(insertOrUpdateQuery);
               res = Operation.Success.ToString();
           }
           catch (Exception ex)
           {
               res = ex.Message;
           }
           return res;
       }

       public int IfAnnonymous(int smsNotificationId, int userId, CommonConnection connection)
       {
           string query = string.Format(@"Select * from SmsNotificationUser where UserId = {0} and SmsNotificationId = {1} ", userId, smsNotificationId);
           DataTable dt = connection.GetDataTable(query);

           var total = dt == null ? 0 : dt.Rows.Count;
           return total;
       }

       public Users IfNotifyUser(int userId)
       {
           var query = string.Format(@"Select * from Users where UserId = {0} and IsNotify = 1", userId);
           var data = Data<Users>.DataSource(query);
           if (data.Count != 0)
           {
               return data.SingleOrDefault();
           }
           return null;
       }
       public List<SmsNotificationInfo> SmsSentNoticeForNotification(int userId)
       {
           var notify = IfNotifyUser(userId);
           if (notify.IsNotify == 1)
           {
               var currDate = DateTime.Now.ToShortDateString();
               var last5Days = DateTime.Now.AddDays(-5).ToShortDateString();
               var query =
                   string.Format(@"Select * from SmsNotification 
                left outer join SmsNotificationUser on SmsNotification.SmsNotificationId = SmsNotificationUser.SmsNotificationId
                inner join Users on Users.UserId = SmsNotificationUser.UserId and Users.IsNotify = 1
                where SmsNotificationUser.UserId = {0} and (ViewStatus is null or ViewStatus = 0) and SmsNotification.[Status] in (1,2)
                order by RequestDateTime DESC",
                       userId);
               var data = Data<SmsNotificationInfo>.DataSource(query);
               if (data.Count > 0)
               {
                   return data;
               }
           }

           return null;
       }

       public List<SmsNotificationInfo> SmsFailNoticeForNotification(int userId)
       {
           var notify = IfNotifyUser(userId);
           if (notify != null && notify.IsNotify == 1)
           {
               var currDate = DateTime.Now.ToShortDateString();
               var last5Days = DateTime.Now.AddDays(-5).ToShortDateString();
               var query =
                   string.Format(@"Select * from SmsNotification 
                left outer join SmsNotificationUser on SmsNotification.SmsNotificationId = SmsNotificationUser.SmsNotificationId
                inner join Users on Users.UserId = SmsNotificationUser.UserId and Users.IsNotify = 1
                where SmsNotificationUser.UserId ={0}  and (ViewStatus is null or ViewStatus = 0) and SmsNotification.[Status] > 2
                order by RequestDateTime DESC",
                       userId);
               var data = Data<SmsNotificationInfo>.DataSource(query);
               if (data.Count > 0)
               {
                   return data;
               }
           }

           return null;
       }

       public int SmsSentNoticeCountForNotification(int userId)
       {
           
           var notify = IfNotifyUser(userId);
           if (notify != null && notify.IsNotify == 1)
           {
               CommonConnection connection = new CommonConnection();

               var currDate = DateTime.Now.ToShortDateString();
               var last5Days = DateTime.Now.AddDays(-5).ToShortDateString();
               var query =
                  string.Format(@"Select Count(*) from SmsNotification 
                left outer join SmsNotificationUser on SmsNotification.SmsNotificationId = SmsNotificationUser.SmsNotificationId
                inner join Users on Users.UserId = SmsNotificationUser.UserId and Users.IsNotify = 1
                where SmsNotificationUser.UserId = {0} and (ViewStatus is null or ViewStatus = 0) and SmsNotification.[Status] in (1,2)",
                      userId);

               return connection.GetScaler(query);
           }
           return 0;
       }

       public int SmsFailNoticeCountForNotification(int userId)
       {           
           var notify = IfNotifyUser(userId);
           if (notify != null && notify.IsNotify == 1)
           {
               CommonConnection connection = new CommonConnection();

               var currDate = DateTime.Now.ToShortDateString();
               var last5Days = DateTime.Now.AddDays(-5).ToShortDateString();
               var query =
                    string.Format(@"Select Count(*) from SmsNotification 
                left outer join SmsNotificationUser on SmsNotification.SmsNotificationId = SmsNotificationUser.SmsNotificationId
                inner join Users on Users.UserId = SmsNotificationUser.UserId and Users.IsNotify = 1
                where SmsNotificationUser.UserId ={0}  and (ViewStatus is null or ViewStatus = 0) and SmsNotification.[Status] > 2",
                        userId);

               return connection.GetScaler(query);
           }
           return 0;
       }

       public int UnRecognizeCollSmsNoticeCountForNotification(int userId)
       {
           var notify = IfNotifyUser(userId);
           if (notify != null && notify.IsNotify == 1)
           {
               CommonConnection connection = new CommonConnection();

               var currDate = DateTime.Now.ToShortDateString();
               var last5Days = DateTime.Now.AddDays(-5).ToShortDateString();
               var query =
                   string.Format(
                       @"Select Count(*) from SMSRecieved where Status IN(2,3,4,5,9,11) 
                        --and Cast(RecievedDate as date) between '{0}' and '{1}'",
                       last5Days, currDate);

               return connection.GetScaler(query);
           }
           return 0;
       }

       public int UnRecognizeSaleNoticeCountForNotification(int userId)
       {
           var notify = IfNotifyUser(userId);
           if (notify != null && notify.IsNotify == 1)
           {
               CommonConnection connection = new CommonConnection();

               var currDate = DateTime.Now.ToShortDateString();
               var last5Days = DateTime.Now.AddDays(-5).ToShortDateString();
               var query =
                   string.Format(
                       @"SELECT Count(*)
                FROM Sale S 
                LEFT JOIN Sale_Product SP ON SP.ModelId=S.ModelId 
                INNER JOIN Sale_Customer SC ON SC.CustomerId=S.CustomerId and SC.IsActive=1
                Left Outer join Branch on Branch.BRANCHID = SC.BranchId
                Left Outer join Discount on Discount.SaleId=S.SaleId
                Left outer join DownpamentCollection_tmp DPT on DPT.SaleId=S.SaleId
                Where S.IsActive=0 And S.State=3  and UnRecognizeType <>1 
                --and cast(PayDate as DATE) between '{0}' and '{1}'",
                       last5Days, currDate);

               return connection.GetScaler(query);
           }
           return 0;
       }

       public int UnRecognizeSmsNoticeCountForNotification(int userId) //SaleSms Unrecognize
       {
           var notify = IfNotifyUser(userId);
           if (notify != null && notify.IsNotify == 1)
           {
               CommonConnection connection = new CommonConnection();

               var currDate = DateTime.Now.ToShortDateString();
               var last5Days = DateTime.Now.AddDays(-5).ToShortDateString();
               var query =
                   string.Format(
                       @"SELECT Count(*)  FROM SalesSms where IsUnrecognized=1
                        --and cast(SmsDate as date) between '{0}' and '{1}'",
                       last5Days, currDate);

               return connection.GetScaler(query);
           }
           return 0;
       }
    }
}
