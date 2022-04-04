using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.Core;
using Utilities;

namespace Azolution.Core.Service.Interface
{
    public interface IMenuRepository
    {
        GridEntity<Menu> GetMenuSummary(int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter);

        IQueryable<Menu> SelectAllMenu();

        string SaveMenu(Menu menu);

        IQueryable<Menu> SelectAllMenuByModuleId(int moduleId);

        IQueryable<Menu> SelectMenuByUserPermission(int userId);

        List<Menu> GetToDoList();

        string UpdateMenuSorting(List<Menu> menuList);

        List<Menu> GetParentMenuByMenu(int parentMenuId);
    }
}
