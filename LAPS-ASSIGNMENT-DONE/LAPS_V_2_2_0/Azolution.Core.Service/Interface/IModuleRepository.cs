using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.Core;
using Utilities;

namespace Azolution.Core.Service.Interface
{
    public interface IModuleRepository
    {
        GridEntity<Module> GetModuleSummary(int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter);
        IQueryable<Module> SelectAllModule();

        string SaveModule(Module module);
    }
}
