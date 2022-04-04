using System.Collections.Generic;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataService;
using Azolution.HumanResource.Service.Interface;
using Utilities;

namespace Azolution.HumanResource.Service.Service
{
    public class DesignationService : IDesignationRepository
    {
        DesignationDataService _designationDataService = new DesignationDataService();

        public string SaveDesignation(Designation designation)
        {
            return _designationDataService.SaveDesignation(designation);
        }

        public object GetDesignationSummary(int companyId, int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            return _designationDataService.GetDesignationSummary(companyId, skip, take, page, pageSize, sort, filter);
        }



        public List<Designation> GetDesignationByCompanyId(int companyId)
        {
            return _designationDataService.GetDesignationByCompanyId(companyId);
        }

        public List<Designation> GetAllDesignationByCompanyIdAndStatus(int companyId, int status)
        {
            return _designationDataService.GetAllDesignationByCompanyIdAndStatus(companyId, status);
        }

        public List<Designation> GenerateDesignationByDepartmentIdCombo(int departmentId, int status)
        {
            return _designationDataService.GenerateDesignationByDepartmentIdCombo(departmentId, status);
        }
    }
}
