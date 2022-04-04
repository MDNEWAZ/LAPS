using System.Collections.Generic;
using System.Linq;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;
using Utilities;

namespace Azolution.Core.Service.Service
{
    public class StatusService : IStatusRepository
    {
       readonly StatusDataService _statusDataService = new StatusDataService();

        public List<WFState> GetStatusByMenuId(int menuId)
        {
            return _statusDataService.GetStatusByMenuId(menuId);
        }

        public List<WFAction> GetActionByStatusId(int statusId)
        {
            return _statusDataService.GetActionByStatusId(statusId);
        }
        public IQueryable<AccessControl> SelectAllAccessControl()
        {
            return _statusDataService.SelectAllAccessControl();
        }
        public string SaveAccessControl(AccessControl accessControl)
        {
            return _statusDataService.SaveAccessControl(accessControl);
        }

        public GridEntity<AccessControl> GetAccessControlSummary(int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {
            return _statusDataService.GetAccessControlSummary(skip, take, page, pageSize, sort, filter);
        }

        public GridEntity<AccessControl> GetAccessControlSummary2(int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {
            return _statusDataService.GetAccessControlSummary2(skip, take, page, pageSize, sort, filter);
           
        }



        public WFState GetDefaultStatusByMenuId(int menuId)
        {
            return _statusDataService.GetDefaultStatusByMenuId(menuId);
        }

        public List<WFAction> GetActionByStateIdAndUserId(int stateId, int userId)
        {
            return _statusDataService.GetActionByStateIdAndUserId(stateId, userId);
        }

        public GridEntity<WFState> GetWorkFlowSummary(int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {
            return _statusDataService.GetWorkFlowSummary(skip, take, page, pageSize, sort, filter);
        }

        public List<Menu> GetMenu()
        {
            return _statusDataService.GetMenu();
        }

        public WFState GetApproveStatusByMenuId(int menuId)
        {
            return _statusDataService.GetApproveStatusByMenuId(menuId);
        }

        public string SaveState(WFState state)
        {
            return _statusDataService.SaveState(state);
        }

        public string SaveAction(WFAction action)
        {
            return _statusDataService.SaveAction(action);
        }

        public string DeleteStatusByActionId(int actionId)
        {
            return _statusDataService.DeleteStatusByActionId(actionId);
        }
    }
}
