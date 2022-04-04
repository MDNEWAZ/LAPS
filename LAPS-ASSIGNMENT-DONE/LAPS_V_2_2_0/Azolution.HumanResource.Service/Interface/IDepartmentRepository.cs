using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.HumanResource;

namespace Azolution.HumanResource.Service.Interface
{
    public interface IDepartmentRepository
    {

        string SaveDepartment(Department department);

        object GetDepartmentSummary(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter);

        IQueryable<Department> GetDepartmentByCompanyId(int companyId);
    }
}
