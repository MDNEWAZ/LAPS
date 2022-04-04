using System.Collections.Generic;
using System.Linq;
using Azolution.Common.Helper;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;
using JsonHelper = Utilities.Common.Json.JsonHelper;

namespace Azolution.Core.Service.Service
{
    public class CompanyService : ICompanyRepository
    {
        private readonly CompanyDataService _companyDataService = new CompanyDataService();


        public string SaveCompany(Company company)
        {
            return _companyDataService.SaveCompany(company);
        }

        public IQueryable<Company> GetMotherCompany(int companyId)
        {
            return _companyDataService.GetMotherCompany(companyId);
        }

        public List<Company> GetAllCompaniesWithPaging(int skip, int take, int page, int pageSize,
            List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            return _companyDataService.GetAllCompaniesWithPaging(skip, take, page, pageSize, sort, filter, companyId);
        }

        public Company SelectCompanyByCompanyId(int companyId)
        {
            return _companyDataService.SelectCompanyByCompanyId(companyId);
        }

        public IQueryable<Company> GetMotherCompanyForEditCompanyCombo(int companyId, int seastionCompanyId)
        {
            return _companyDataService.GetMotherCompanyForEditCompanyCombo(companyId, seastionCompanyId);
        }

        public string GetAllCompanies(GridOptions options, int companyId)
        {
            var data = _companyDataService.GetAllCompanies(options, companyId);
            var jsonHelper = new JsonHelper();
            return jsonHelper.GetJson(data);
        }

        /// <summary>
        /// Interest
        /// </summary>
        /// <param name="aInterest"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

    

        public string GetAInterest(int companyId)
        {
            var data = _companyDataService.GetAInterest(companyId);
            var jsonHelper = new JsonHelper();
            return jsonHelper.GetJson(data);
        }

      
        public List<Company> GetRootMotherCompany()
        {
            return _companyDataService.GetRootMotherCompany();
        }

        public object GetRootCompany()
        {
            return _companyDataService.GetRootCompany();
        }

        public Company GetCompanyInfoByBranchCode(string branchCode)
        {
            return _companyDataService.GetCompanyInfoByBranchCode(branchCode);
        }

        public object GetCompanySummary(GridOptions options, int companyId)
        {
            return _companyDataService.GetCompanySummary(options,companyId);
        }

        public List<Company> GetAllCompaniesForCombo()
        {
            return _companyDataService.GetAllCompaniesForCombo();
        }
    }
}
            