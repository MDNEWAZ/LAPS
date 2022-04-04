using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.HumanResource;
using Utilities;

namespace Azolution.Core.Service.Interface
{
    public interface IUsersRepository
    {
        string SaveUser(Users users);

        Users GetUserById(int userId);

        GridEntity<Users> GetUserSummary(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter);

        List<GroupMember> GetGroupMemberByUserId(int userId);

        string ResetPassword(int companyId, int userId);

        Users GetUserByLoginId(string loginId);

        IQueryable<PasswordHistory> GetPasswordHistory(int userId, int oldPassUseRestriction);

        string UpdateUser(Users users, PasswordHistory passHistory);

        Users GetUserByEmailAddress(string emailaddress);

        Users GetUserByEmployeeId(int hrRecordId);

        string UpdateTheme(Users user);



        string ChangePassword(string password, Users objUser, SystemSettings objSystemSettings);

        List<Group> GetUserTypeByUserId(int userId);

        CompanyBranchInfo GetBranchCodeByCompanyIdAndBranchId(int companyId, int branchId);
    }
}
