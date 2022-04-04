using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class UsersDataReader : EntityDataReader<Users>
    {
        public int CompanyIDColumn = -1;
        public int CreatedDateColumn = -1;
        public int EmployeeIdColumn = -1;
        public int FailedLoginNoColumn = -1;
        public int IsActiveColumn = -1;
        public int IsExpiredColumn = -1;
        public int LastLoginDateColumn = -1;
        public int LastUpdateDateColumn = -1;
        public int LoginIdColumn = -1;
        public int PasswordColumn = -1;
        public int UserIdColumn = -1;
        public int UserNameColumn = -1;
        public int CompanyNameColumn = -1;
        public int FullLogoPathColumn = -1;
        public int LogHourEnableColumn = -1;
        public int FiscalYearStartColumn = -1;
        public int ThemeColumn = -1;
        public int BranchIdColumn = -1;
        public int DepartmentIdColumn = -1;
        public int BranchCodeColumn = -1;
        public int IsViewerColumn = -1;
        public int IsNotifyColumn = -1;
        public UsersDataReader(IDataReader reader)
            : base(reader)
        {
        }

        public override Users Read()
        {
            var objUser = new Users();
            objUser.UserId = GetInt(UserIdColumn);
            objUser.CompanyId = GetInt(CompanyIDColumn);
            objUser.LoginId = GetString(LoginIdColumn);
            objUser.UserName = GetString(UserNameColumn);
            objUser.Password = GetString(PasswordColumn);
            objUser.EmployeeId = GetInt(EmployeeIdColumn);
            objUser.CreatedDate = GetDate(CreatedDateColumn);
            objUser.LastUpdateDate = GetDate(LastUpdateDateColumn);
            objUser.LastLoginDate = GetDate(LastLoginDateColumn);
            objUser.FailedLoginNo = GetInt(FailedLoginNoColumn);
            objUser.Theme = GetString(ThemeColumn);
            objUser.BranchCode = GetString(BranchCodeColumn);

            objUser.IsViewer = GetInt(IsViewerColumn);
            objUser.IsNotify = GetInt(IsNotifyColumn);
            try
            {
                objUser.IsActive = GetBool(IsActiveColumn);
            }
            catch
            {
                var isActive = GetInt(IsActiveColumn);
                objUser.IsActive = isActive == 1 ? true : false;
            }

            try
            {
                objUser.IsExpired = GetBool(IsExpiredColumn);
            }
            catch
            {
                var isExpired = GetInt(IsExpiredColumn);
                objUser.IsExpired = isExpired == 1 ? true : false;
            }
            objUser.CompanyName = GetString(CompanyNameColumn);
            objUser.FullLogoPath = GetString(FullLogoPathColumn);
            try
            {
                objUser.LogHourEnable = GetBool(LogHourEnableColumn);
            }
            catch
            {
                var lgEnavle = GetInt(LogHourEnableColumn);
                objUser.LogHourEnable = lgEnavle == 1 ? true : false;
            }
            objUser.FiscalYearStart = GetInt(FiscalYearStartColumn);
            objUser.BranchId = GetInt(BranchIdColumn) == int.MinValue ? 0 : GetInt(BranchIdColumn);
            objUser.DepartmentId = GetInt(DepartmentIdColumn) == int.MinValue ? 0 : GetInt(DepartmentIdColumn);
            return objUser;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "USERID":
                        {
                            UserIdColumn = i;
                            break;
                        }
                    case "COMPANYID":
                        {
                            CompanyIDColumn = i;
                            break;
                        }
                    case "LOGINID":
                        {
                            LoginIdColumn = i;
                            break;
                        }
                    case "USERNAME":
                        {
                            UserNameColumn = i;
                            break;
                        }
                    case "PASSWORD":
                        {
                            PasswordColumn = i;
                            break;
                        }
                    case "EMPLOYEEID":
                        {
                            EmployeeIdColumn = i;
                            break;
                        }
                    case "CREATEDDATE":
                        {
                            CreatedDateColumn = i;
                            break;
                        }
                    case "LASTUPDATEDATE":
                        {
                            LastUpdateDateColumn = i;
                            break;
                        }
                    case "LASTLOGINDATE":
                        {
                            LastLoginDateColumn = i;
                            break;
                        }
                    case "FAILEDLOGINNO":
                        {
                            FailedLoginNoColumn = i;
                            break;
                        }
                    case "ISACTIVE":
                        {
                            IsActiveColumn = i;
                            break;
                        }
                    case "ISEXPIRED":
                        {
                            IsExpiredColumn = i;
                            break;
                        }
                    case "COMPANYNAME":
                        {
                            CompanyNameColumn = i;
                            break;
                        }
                    case "FULLLOGOPATH":
                        {
                            FullLogoPathColumn = i;
                            break;
                        }
                    case "LOGHOURENABLE":
                        {
                            LogHourEnableColumn = i;
                            break;
                        }
                    case "FISCALYEARSTART":
                        {
                            FiscalYearStartColumn = i;
                            break;
                        }
                    case "THEME":
                        {
                            ThemeColumn = i;
                            break;
                        }
                    case "BRANCHID":
                        {
                            BranchIdColumn = i;
                            break;
                        }
                    case "DEPARTMENTID":
                        {
                            DepartmentIdColumn = i;
                            break;
                        }
                    case "BRANCHCODE":
                        {
                            BranchCodeColumn = i;
                            break;
                        }

                    case "ISVIEWER":
                        {
                            IsViewerColumn = i;
                            break;
                        }

                    case "ISNOTIFY":
                        {
                            IsNotifyColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
