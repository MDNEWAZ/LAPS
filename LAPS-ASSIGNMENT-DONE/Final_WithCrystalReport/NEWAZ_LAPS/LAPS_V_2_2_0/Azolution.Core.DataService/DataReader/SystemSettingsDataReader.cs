using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class SystemSettingsDataReader : EntityDataReader<SystemSettings>
    {
        public int SettingsIdColumn = -1;
        public int CompanyIdColumn = -1;
        public int ThemeColumn = -1;
        public int LanguageColumn = -1;
        public int MinLoginLengthColumn = -1;
        public int MinPassLengthColumn = -1;
        public int PassTypeColumn = -1;
        public int SpecialCharAllowedColumn = -1;
        public int WrongAttemptNoColumn = -1;
        public int ChangePassDaysColumn = -1;
        public int ChangePassFirstLoginColumn = -1;
        public int PassExpiryDaysColumn = -1;
        public int ResetPassColumn = -1;
        public int PassResetByColumn = -1;
        public int OldPassUseRestrictionColumn = -1;
        public int OdbcClientListColumn = -1;
        public int UserIdColumn = -1;
        public int LastUpdateDateColumn = -1;

        public SystemSettingsDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override SystemSettings Read()
        {
            var objSystemSettings = new SystemSettings();
            objSystemSettings.SettingsId = GetInt(SettingsIdColumn);
            objSystemSettings.CompanyId = GetInt(CompanyIdColumn);
            objSystemSettings.Theme = GetString(ThemeColumn);
            objSystemSettings.Language = GetString(LanguageColumn);
            objSystemSettings.MinLoginLength = GetInt(MinLoginLengthColumn);
            objSystemSettings.MinPassLength = GetInt(MinPassLengthColumn);
            objSystemSettings.PassType = GetInt(PassTypeColumn);
            try
            {
                objSystemSettings.SpecialCharAllowed = GetBool(SpecialCharAllowedColumn);
            }
            catch
            {
                var spal = GetInt(SpecialCharAllowedColumn);
                objSystemSettings.SpecialCharAllowed = spal == 1 ? true : false;
            }
            objSystemSettings.WrongAttemptNo = GetInt(WrongAttemptNoColumn);
            objSystemSettings.ChangePassDays = GetInt(ChangePassDaysColumn);
            try
            {
                objSystemSettings.ChangePassFirstLogin = GetBool(ChangePassFirstLoginColumn);
            }
            catch
            {
                var changpass = GetInt(ChangePassFirstLoginColumn);
                objSystemSettings.ChangePassFirstLogin = changpass == 1 ? true : false;
            }
            objSystemSettings.PassExpiryDays = GetInt(PassExpiryDaysColumn);
            objSystemSettings.ResetPass = GetString(ResetPassColumn);
            objSystemSettings.PassResetBy = GetInt(PassResetByColumn);
            objSystemSettings.OldPassUseRestriction = GetInt(OldPassUseRestrictionColumn);
            try
            {
                objSystemSettings.OdbcClientList = GetBool(OdbcClientListColumn);
            }
            catch
            {
                var odbcClientList = GetInt(OdbcClientListColumn);
                objSystemSettings.OdbcClientList = odbcClientList == 1 ? true : false;
            }

            objSystemSettings.UserId = GetInt(UserIdColumn);
            objSystemSettings.LastUpdateDate = GetDate(LastUpdateDateColumn);
            return objSystemSettings;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "SETTINGSID":
                        {
                            SettingsIdColumn = i;
                            break;
                        }
                    case "COMPANYID":
                        {
                            CompanyIdColumn = i;
                            break;
                        }
                    case "THEME":
                        {
                            ThemeColumn = i;
                            break;
                        }
                    case "LANGUAGE":
                        {
                            LanguageColumn = i;
                            break;
                        }
                    case "MINLOGINLENGTH":
                        {
                            MinLoginLengthColumn = i;
                            break;
                        }
                    case "MINPASSLENGTH":
                        {
                            MinPassLengthColumn = i;
                            break;
                        }
                    case "PASSTYPE":
                        {
                            PassTypeColumn = i;
                            break;
                        }
                    case "SPECIALCHARALLOWED":
                        {
                            SpecialCharAllowedColumn = i;
                            break;
                        }
                    case "WRONGATTEMPTNO":
                        {
                            WrongAttemptNoColumn = i;
                            break;
                        }
                    case "CHANGEPASSDAYS":
                        {
                            ChangePassDaysColumn = i;
                            break;
                        }
                    case "CHANGEPASSFIRSTLOGIN":
                        {
                            ChangePassFirstLoginColumn = i;
                            break;
                        }
                    case "PASSEXPIRYDAYS":
                        {
                            PassExpiryDaysColumn = i;
                            break;
                        }
                    case "RESETPASS":
                        {
                            ResetPassColumn = i;
                            break;
                        }
                    case "PASSRESETBY":
                        {
                            PassResetByColumn = i;
                            break;
                        }
                    case "OLDPASSUSERESTRICTION":
                        {
                            OldPassUseRestrictionColumn = i;
                            break;
                        }
                    case "ODBCCLIENTLIST":
                        {
                            OdbcClientListColumn = i;
                            break;
                        }
                    case "USERID":
                        {
                            UserIdColumn = i;
                            break;
                        }
                    case "LASTUPDATEDATE":
                        {
                            LastUpdateDateColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
