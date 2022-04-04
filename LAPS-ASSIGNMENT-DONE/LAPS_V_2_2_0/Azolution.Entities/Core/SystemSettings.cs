using System;

namespace Azolution.Entities.Core
{
    public class SystemSettings
    {
        public SystemSettings()
        {
        }
        public int SettingsId { get; set; }
        public int CompanyId { get; set; }
        public string Theme { get; set; }
        public string Language { get; set; }
        public int MinLoginLength { get; set; }
        public int MinPassLength { get; set; }
        public int PassType { get; set; }
        public bool SpecialCharAllowed { get; set; }
        public int WrongAttemptNo { get; set; }
        public int ChangePassDays { get; set; }
        public bool ChangePassFirstLogin { get; set; }
        public int PassExpiryDays { get; set; }
        public string ResetPass { get; set; }
        public int PassResetBy { get; set; }
        public int OldPassUseRestriction { get; set; }
        public bool OdbcClientList { get; set; }
        public int UserId { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
