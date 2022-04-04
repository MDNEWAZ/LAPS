using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;
using Laps.AdminSettings.DataService.DataService;
using Laps.AdminSettings.Service.Interface;

namespace Laps.AdminSettings.Service.Service
{
    public class CommonService : ICommonRepository
    {
        CommonDataService _commonDataService = new CommonDataService();
        public CompanyHeirarchyPath GetCompanyHeirarchyPathData(int companyId, int branchId)
        {
            var companyData=_commonDataService.GetCompanyHeirarchy(companyId);
            var branchData = _commonDataService.GetBranchDataI(branchId);
            if (branchData != null)
            {
                companyData.BranchName = branchData.BranchName;                
            }
            return companyData;
        }
    }
}
