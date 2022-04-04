using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.Core;
using Utilities;

namespace Azolution.Core.Service.Interface
{
    public interface IStatusRepository
    {
        List<WFState> GetStatusByMenuId(int menuId);

        List<WFAction> GetActionByStatusId(int statusId);

        GridEntity<AccessControl> GetAccessControlSummary(int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter);
        GridEntity<AccessControl> GetAccessControlSummary2(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter);
        IQueryable<AccessControl> SelectAllAccessControl();

        string SaveAccessControl(AccessControl accessControl);



        WFState GetDefaultStatusByMenuId(int menuId);

        List<WFAction> GetActionByStateIdAndUserId(int stateId, int userId);

        //object GetWorkFlowSummary(int skip, int take, int page, int pageSize, Utilities.AzFilter.GridSort[] sort, Utilities.AzFilter.GridFilters filter);
        GridEntity<WFState> GetWorkFlowSummary(int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter);


        List<Menu> GetMenu();

        WFState GetApproveStatusByMenuId(int menuId);

        string SaveState(WFState state);

        string SaveAction(WFAction action);

        string DeleteStatusByActionId(int actionId);
    }
}
