using System.Collections.Generic;
using Azolution.Entities.HumanResource;

namespace Azolution.HumanResource.Service.Interface
{
    public interface IDesignationRepository
    {

        string SaveDesignation(Designation designation);

        object GetDesignationSummary(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter);

        List<Designation> GetDesignationByCompanyId(int companyId);

        List<Designation> GetAllDesignationByCompanyIdAndStatus(int companyId, int status);

        List<Designation> GenerateDesignationByDepartmentIdCombo(int departmentId, int status);
    }

}
