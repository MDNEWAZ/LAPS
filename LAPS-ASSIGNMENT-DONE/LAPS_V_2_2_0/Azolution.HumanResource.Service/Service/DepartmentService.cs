using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataService;
using Azolution.HumanResource.Service.Interface;
using Utilities;

namespace Azolution.HumanResource.Service.Service
{
    public class DepartmentService : IDepartmentRepository
    {
        DepartmentDataService _departmentDataService = new DepartmentDataService();

        public string SaveDepartment(Department department)
        {
            return _departmentDataService.SaveDepartment(department);
        }

        public object GetDepartmentSummary(int companyId, int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            return _departmentDataService.GetDepartmentSummary(companyId, skip, take, page, pageSize, sort, filter);
        }

        public IQueryable<Department> GetDepartmentByCompanyId(int companyId)
        {
            return _departmentDataService.GetDepartmentByCompanyId(companyId);
        }
    }
}
