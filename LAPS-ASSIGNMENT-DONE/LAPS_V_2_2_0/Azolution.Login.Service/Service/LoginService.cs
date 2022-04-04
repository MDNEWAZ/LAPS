using System;
using System.Linq;
using System.Text;
using Azolution.Common.Validation;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Security;

namespace Azolution.Login.Service.Service
{
    public class LoginService
    {
        readonly IUsersRepository _usersRepository = new UsersService();
        readonly ISystemSettingsRepository _sysRepository = new SystemSettingsService();
        readonly ICompanyRepository _companyRepository = new CompanyService();

        public string ValidateUserLogin(string loginId, string password)
        {
            var minDate = DateTime.MinValue;
            const string output = "";
            const int shiftId = 0;


            var user = _usersRepository.GetUserByLoginId(loginId);


            var currentDate = DateTime.Now;
            string message = "";
            var sb = new StringBuilder();
            if (user == null)
            {
                message = "FAILED";
                sb.AppendFormat("{0}", message);
                return sb.ToString();
            }
            if (!user.IsActive)
            {
                message = "INACTIVE";
                sb.AppendFormat("{0}", message);
                return sb.ToString();
            }
            if (user.IsExpired)
            {
                message = "EXPIRED";
                sb.AppendFormat("{0}", message);
                return sb.ToString();
            }
            //var userService = new UsersService(UsersRepository);


            var systemSettings = _sysRepository.GetSystemSettingsDataByCompanyId(user.CompanyId);

            var objCompany = _companyRepository.SelectCompanyByCompanyId(user.CompanyId);
            user.CompanyName = objCompany.CompanyName;
            user.RootCompanyId = objCompany.RootCompanyId;
            user.CompanyType = objCompany.CompanyType;
            user.CompanyStock = objCompany.CompanyStock;
            user.FiscalYearStart = objCompany.FiscalYearStart;
            user.FullLogoPath = objCompany.FullLogoPath;
                        
            //user.BranchId = objCompany.BranchId;
                        
            var theme = String.IsNullOrEmpty(user.Theme) ? systemSettings.Theme : user.Theme;
                      

            if (ValidationHelper.ValidateLoginPassword(password, user.Password, true))
            {
                var passwordHistory = _usersRepository.GetPasswordHistory(user.UserId, systemSettings.OldPassUseRestriction);
                var dayCount = 0;
                if (!passwordHistory.Any())
                {
                    var span = currentDate.Subtract(user.CreatedDate);//00:00:00
                    dayCount = span.Days;
                    if (dayCount > systemSettings.PassExpiryDays)
                    {
                        //update user as expired
                        user.IsExpired = true;

                        message = _usersRepository.UpdateUser(user, null);
                        if (message == "Success")
                        {
                            message = "EXPIRED";
                        }
                        sb.AppendFormat("{0}", message);
                        return sb.ToString();
                    }
                }
                else
                {
                    var span = currentDate.Subtract(passwordHistory.ElementAt(0).PasswordChangeDate);
                    dayCount = span.Days;
                    if (dayCount > systemSettings.PassExpiryDays)
                    {
                        //update user as expired
                        if (user.LoginId.ToLower() != "admin")
                        {
                            user.IsExpired = true;

                            message = _usersRepository.UpdateUser(user, null);
                            if (message == "Success")
                            {
                                message = "EXPIRED";
                            }
                            sb.AppendFormat("{0}", message);
                            return sb.ToString();
                        }
                       
                    }

                }
                if (systemSettings.ChangePassFirstLogin && user.LastLoginDate == DateTime.MinValue)
                {
                    message = "CHANGE";
                    sb.AppendFormat("{0}", message);
                }
                else
                {
                    user.LastLoginDate = currentDate;
                }

                if (user.LastUpdateDate != DateTime.MinValue)
                {
                    var tSpan = currentDate.Subtract(user.LastUpdateDate);
                    dayCount = tSpan.Days;
                    if (dayCount > systemSettings.ChangePassDays)
                    {
                        message = "CHANGE";
                    }
                }
                //Update Failed Loging No = 0 of User table
                user.FailedLoginNo = 0;
                message = _usersRepository.UpdateUser(user, null);

            }
            else
            {
                user.FailedLoginNo += 1;

                //if Failed Login reached to "Wrong attempt to Lock" then deactivate user
                if (user.FailedLoginNo >= systemSettings.WrongAttemptNo)
                {
                    //Deactivate User
                    user.IsActive = false;
                }

                message = _usersRepository.UpdateUser(user, null);
                if (message == "Success")
                {
                    message = "FAILED";
                }
                sb.AppendFormat("{0}", message);
                return sb.ToString();
            }

            var license = new AzolutionLicense();
            var expiryDate = DateTime.Now;//license.GetExpiryDate();
            const int licUserNo = 0; //license.GetNumberOfUser();

            var isviewer = user.IsViewer < 0  ? 0 : user.IsViewer;

            sb.AppendFormat("{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}^{8}^{9}^{10}^{11}^{12}^{13}^{14}^{15}^{16}^{17}^{18}^{19}^{20}", message, user.UserId, user.LoginId, user.UserName, user.CompanyId, user.EmployeeId, user.CompanyName, user.FullLogoPath, user.LogHourEnable, expiryDate, licUserNo, user.FiscalYearStart, output, shiftId, theme, user.BranchId, user.RootCompanyId, user.CompanyType, user.CompanyStock, user.BranchCode, isviewer);
            return sb.ToString();
        }


        public Users GetCurrentUser(string userData)
        {
            var data = userData.Split('^');
            var currentUser = new Users
            {
                UserId = Convert.ToInt32(data[1]),
                LoginId = data[2],
                UserName = data[3],
                CompanyId = Convert.ToInt32(data[4]),
                EmployeeId = Convert.ToInt32(data[5]),
                CompanyName = data[6],
                FullLogoPath = data[7],
                LogHourEnable = Convert.ToBoolean(data[8]),
                IsFirstLogin = data[0] == "CHANGESuccess" ? "Yes" : "No",
                LicenseExpiryDate = Convert.ToDateTime(data[9]),
                LicenseUserNo = Convert.ToInt32(data[10]),
                FiscalYearStart = Convert.ToInt32(data[11]),
                ShiftId = Convert.ToInt32(data[13]),
                Theme = data[14],
                BranchId = Convert.ToInt32(data[15]),
                RootCompanyId = Convert.ToInt32(data[16]),
                CompanyType = Convert.ToString(data[17]),
                CompanyStock = Convert.ToInt32(data[18]),
                BranchCode = Convert.ToString(data[19]),

                IsViewer = Convert.ToInt32(data[20]),
               
            };
            return currentUser;
        }
    }
}
