using System.Collections.Generic;
using System.Linq;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;
using Utilities;

namespace Azolution.Core.Service.Service
{
    public class MenuService : IMenuRepository
    {
       readonly MenuDataService _menuDataService = new MenuDataService();

        public GridEntity<Menu> GetMenuSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            return _menuDataService.GetMenuSummary(skip, take, page, pageSize, sort, filter);
        }

        public IQueryable<Menu> SelectAllMenu()
        {
            return _menuDataService.SelectAllMenu();
        }

        public string SaveMenu(Menu menu)
        {
            return _menuDataService.SaveMenu(menu);
        }

        public IQueryable<Menu> SelectAllMenuByModuleId(int moduleId)
        {
            return _menuDataService.SelectAllMenuByModuleId(moduleId);
        }

        public IQueryable<Menu> SelectMenuByUserPermission(int userId)
        {
            return _menuDataService.SelectMenuByUserPermission(userId);
        }

        public List<Menu> GetToDoList()
        {
            return _menuDataService.GetToDoList();
        }

        public string UpdateMenuSorting(List<Menu> menuList)
        {
            return _menuDataService.UpdateMenuSorting(menuList);
        }

        public List<Menu> GetParentMenuByMenu(int parentMenuId)
        {
            return _menuDataService.GetParentMenuByMenu(parentMenuId);
        }
    }
}
