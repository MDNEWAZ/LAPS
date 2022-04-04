using System.Collections.Generic;
using Azolution.Entities.Core;
using Utilities;

namespace Azolution.Core.Service.Interface
{
    public interface IGroupRepository
    {

        string SaveGroup(Group objGroup);

        GridEntity<Group> GetGroupSummaryByCompanyIdWithPaging(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter);

        List<GroupPermission> GetGroupPermisionbyGroupId(int groupId);

        List<Group> GetGroupByCompanyId(int companyId);

        List<AccessControl> GetAllAccess();

        List<GroupPermission> GetAccessPermisionForCurrentUser(int moduleId, int userId);

        Group GetGroupByCondition(string condition);
    }
}
