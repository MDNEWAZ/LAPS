using System;
using System.Collections.Generic;
using System.Linq;
using Azolution.Common.Helper;
using Azolution.Common.Validation;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.HumanResource;
using Utilities;

namespace Azolution.Core.Service.Service
{
    public class UsersService : IUsersRepository
    {
        readonly UsersDataService _usersDataService = new UsersDataService();

        
        public string SaveUser(Users users)
        {
            return _usersDataService.SaveUser(users);
        }

        public Users GetUserById(int userId)
        {
            return _usersDataService.GetUserById(userId);
        }

        public GridEntity<Users> GetUserSummary(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {
            return _usersDataService.GetUserSummary(companyId, skip, take, page, pageSize, sort, filter);
        }

        public List<GroupMember> GetGroupMemberByUserId(int userId)
        {
            return _usersDataService.GetGroupMemberByUserId(userId);
        }

        public string ResetPassword(int companyId, int userId)
        {
            return _usersDataService.ResetPassword(companyId, userId);
        }



        public IQueryable<PasswordHistory> GetPasswordHistory(int userId, int oldPassUseRestriction)
        {
            return _usersDataService.GetPasswordHistory(userId, oldPassUseRestriction);
        }

        public string UpdateUser(Users users, PasswordHistory passHistory)
        {
            return _usersDataService.UpdateUser(users, passHistory);
        }

        public Users GetUserByEmailAddress(string emailaddress)
        {

            var objUser = _usersDataService.GetUserByEmailAddress(emailaddress);
            return objUser;
        
        }

        public string ChangePassword(string password, Users objUser, SystemSettings objSystemSettings)
        {
            var validator = new ValidationHelper();
            
            var message = "";

            string validate = validator.ValidateUser("", objSystemSettings.MinLoginLength, password, objSystemSettings.MinPassLength, objSystemSettings.PassType, objSystemSettings.SpecialCharAllowed);
            if (validate == "Valid")
            {

                var user = _usersDataService.GetUserByLoginId(objUser.LoginId);


                //CHECKING IF NEW PASSWORD EXIST IN PASSWORD HISTORY
                IQueryable<PasswordHistory> passwordHistory = _usersDataService.GetPasswordHistory(user.UserId, objSystemSettings.OldPassUseRestriction);
                if (passwordHistory.Count() != 0)
                {
                    for (int i = 0; i < passwordHistory.Count(); i++)
                    {
                        if (passwordHistory.ElementAt(i).OldPassword == password)
                        {
                            return "You have already used your new password! \nPlease try with another one.";
                        }
                    }
                }

                var passHistory = new PasswordHistory
                {
                    HistoryId = 0,
                    UserId = user.UserId,
                    OldPassword = EncryptDecryptHelper.Decrypt(user.Password),
                    PasswordChangeDate = DateTime.Now
                };

                string encPass = EncryptDecryptHelper.Encrypt(password);
                //if (objUser.IsFirstLogin == "Yes")
                //{
                    user.LastLoginDate = DateTime.Now;
                //}
                user.LastUpdateDate = DateTime.Now;
                user.Password = encPass;
                message = _usersDataService.UpdateUser(user, passHistory);
                return message;
            }
            else
            {
                return validate;
            }
        }

        public List<Group> GetUserTypeByUserId(int userId)
        {
            return _usersDataService.GetUserTypeByUserId(userId);
        }

        public CompanyBranchInfo GetBranchCodeByCompanyIdAndBranchId(int companyId, int branchId)
        {
            return _usersDataService.GetBranchCodeByCompanyIdAndBranchId(companyId, branchId);
          
        }


        public Users GetUserByLoginId(string loginId)
        {
            return _usersDataService.GetUserByLoginId(loginId);
        }

        public Users GetUserByEmployeeId(int hrRecordId)
        {
            return _usersDataService.GetUserByEmployeeId(hrRecordId);
        }

        public string UpdateTheme(Users user)
        {
            return _usersDataService.UpdateTheme(user);
        }
    }
}
