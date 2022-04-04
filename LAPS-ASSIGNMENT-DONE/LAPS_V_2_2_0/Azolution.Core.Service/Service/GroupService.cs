using System.Collections.Generic;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;
using Utilities;


namespace Azolution.Core.Service.Service
{
    public class GroupService : IGroupRepository
    {
        readonly  GroupDataService _groupDataService = new GroupDataService();

        public string SaveGroup(Group objGroup)
        {
            return _groupDataService.SaveGroup(objGroup);
        }

        public GridEntity<Group> GetGroupSummaryByCompanyIdWithPaging(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {
            return _groupDataService.GetGroupSummaryByCompanyIdWithPaging(companyId, skip, take, page, pageSize, sort, filter);
        }

        public List<GroupPermission> GetGroupPermisionbyGroupId(int groupId)
        {
            return _groupDataService.GetGroupPermisionbyGroupId(groupId);
        }

        public List<Group> GetGroupByCompanyId(int companyId)
        {
            return _groupDataService.GetGroupByCompanyId(companyId);
        }

        public List<AccessControl> GetAllAccess()
        {
            return _groupDataService.GetAllAccess();
        }

        public List<GroupPermission> GetAccessPermisionForCurrentUser(int moduleId, int userId)
        {
            return _groupDataService.GetAccessPermisionForCurrentUser(moduleId, userId);
        }


        public Group GetGroupByCondition(string condition)
        {
            return _groupDataService.GetGroupByCondition(condition);
        }
    }
}
