using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Azolution.Core.Service.Interface
{
    public interface ICompanyRepository
    {
        /// <summary>
        /// Company
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        string SaveCompany(Company company);
        IQueryable<Company> GetMotherCompany(int companyId);
        List<Company> GetAllCompaniesWithPaging(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId);
        Company SelectCompanyByCompanyId(int companyId);
        IQueryable<Company> GetMotherCompanyForEditCompanyCombo(int companyId, int seastionCompanyId);
        string GetAllCompanies(GridOptions options,int companyId);
        
        /// <summary>
        /// Interest
        /// </summary>
        /// <param name="aInterest"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
    
        string GetAInterest(int companyId);

        List<Company> GetRootMotherCompany();

        object GetRootCompany();
        Company GetCompanyInfoByBranchCode(string branchCode);
        object GetCompanySummary(GridOptions options, int companyId);
    }
}
