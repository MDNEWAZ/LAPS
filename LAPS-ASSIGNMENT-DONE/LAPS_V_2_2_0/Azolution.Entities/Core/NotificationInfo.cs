using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Core
{
    public class NotificationInfo
    {
        public int NotificationId { get; set; }
        public int NotificationCategoryId { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationDetails { get; set; }
        public DateTime Publishdate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int IsAnnonymous { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string NotificationCategoryDescription { get; set; }

        public int NotificationUserId { get; set; }
        public int UserId { get; set; }
        public int ViewStatus { get; set; }
    }
}
