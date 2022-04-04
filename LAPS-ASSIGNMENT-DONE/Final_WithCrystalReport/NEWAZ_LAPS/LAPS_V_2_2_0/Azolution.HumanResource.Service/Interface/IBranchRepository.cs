using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Utilities;

namespace Azolution.HumanResource.Service.Interface
{
   public interface IBranchRepository
    {
        string SaveBranch(Branch branch);
        GridEntity<Branch> GetBranchSummary(GridOptions options, int companyID);
        GridEntity<Branch> GetBranchTwoSummary(GridOptions options, int companyID);
        List<Branch> GetBranchByCompanyId(int companyId, Users objUser);
        List<Branch> GetAllBranchByCompanyIdForCombo(int rootCompanyId);
        bool GetIsBranchCodeUsed(string branchCode);
        object GetBranchInfoByBranchId(int branchId);
        string UpgradeBranch(Branch objBranch);
        string SaveBranchTwo(Branch branch);
    }
}
