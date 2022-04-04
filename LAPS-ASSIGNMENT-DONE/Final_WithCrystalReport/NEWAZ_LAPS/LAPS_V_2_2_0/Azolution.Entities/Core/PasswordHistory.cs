using System;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class PasswordHistory
    {
        public PasswordHistory()
        {
        }

        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public DateTime PasswordChangeDate { get; set; }
    }
}
