using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataService;
using Azolution.HumanResource.Service.Interface;
using DataService.DataService;
using Utilities;

namespace Azolution.HumanResource.Service.Service
{
    public class BranchService : IBranchRepository
    {
        BranchDataService _branchDataService = new BranchDataService();

        public string SaveBranch(Branch branch)
        {
            return _branchDataService.SaveBranch(branch);
        }

        public GridEntity<Branch> GetBranchSummary(GridOptions options, int companyID)
        {
            return _branchDataService.GetBranchSummary(options, companyID);
        }
        public GridEntity<Branch> GetBranchTwoSummary(GridOptions options, int companyID)
        {
            return _branchDataService.GetBranchTwoSummary(options, companyID);
        }



        public List<Branch> GetBranchByCompanyId(int companyId, Users objUser)
        {
            string condition = "";
            if (companyId >0)
            {
                condition = " Where CompanyId=" + companyId;
            }
            return _branchDataService.GetBranchByCompanyId(condition);
        }

        public List<Branch> GetAllBranchByCompanyIdForCombo(int rootCompanyId)
        {
            string condition = "";

            if (rootCompanyId > 0)
            {
                List<Company> companies = new List<Company>();
                string sql = string.Format(@"Select CompanyId from Company where RootCompanyId = {0}", rootCompanyId);

                companies = Data<Company>.DataSource(sql);
                var companyList = "";
                foreach (var companie in companies)
                {
                    companyList += companie.CompanyId;
                    companyList += ',';
                }
                companyList = companyList.Remove(companyList.Length - 1);

                condition = string.Format(" And CompanyId in ({0})", companyList);
            }
            

            return _branchDataService.GetAllBranchByCompanyIdForCombo(condition);
        }

        public bool GetIsBranchCodeUsed(string branchCode)
        {
            return _branchDataService.GetIsBranchCodeUsed(branchCode);
        }

        public object GetBranchInfoByBranchId(int branchId)
        {
            return _branchDataService.GetBranchInfoByBranchId(branchId);
        }

        public string UpgradeBranch(Branch objBranch)
        {
           CustomerDataService _customerDataService = new CustomerDataService();
           var customerList= _customerDataService.GetCustomerByBranchId(objBranch.BranchId);
            return _branchDataService.UpgradeBranch(objBranch, customerList);
        }

        public string SaveBranchTwo(Branch branch)
        {
            return _branchDataService.SaveBranchTwo(branch);
        }

    }
}
