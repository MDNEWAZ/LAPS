using System.Collections.Generic;
using System.Linq;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;
using Utilities;

namespace Azolution.Core.Service.Service
{
    public class ModuleService : IModuleRepository
    {
       readonly ModuleDataService _moduleDataService = new ModuleDataService();

        public IQueryable<Module> SelectAllModule()
        {
            return _moduleDataService.SelectAllModule();
        }

        public string SaveModule(Module module)
        {
            return _moduleDataService.SaveModule(module);
        }

        public GridEntity<Module> GetModuleSummary(int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {
            return _moduleDataService.GetModuleSummary(skip, take, page, pageSize, sort, filter);
        }
    }
}
