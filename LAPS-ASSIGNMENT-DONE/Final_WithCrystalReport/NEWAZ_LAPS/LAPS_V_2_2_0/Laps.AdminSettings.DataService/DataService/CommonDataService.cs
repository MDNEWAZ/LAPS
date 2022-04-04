using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;
using Utilities;

namespace Laps.AdminSettings.DataService.DataService
{
    public class CommonDataService
    {
        public CompanyHeirarchyPath GetCompanyHeirarchy(int companyId)
        {
            string sql =
                string.Format(@"Select CRoot.CompanyName As RootCompanyName,CMother.CompanyName As MotherCompanyName ,C.CompanyName
                From Company C
                Left outer join Company CRoot on CRoot.CompanyId=C.RootCompanyId 
                Left Outer join Company CMother on CMother.CompanyId=C.MotherId
                Where C.CompanyId={0}", companyId);
            var data = Data<CompanyHeirarchyPath>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public CompanyHeirarchyPath GetBranchDataI(int branchId)
        {
            string sql =
                string.Format(@"Select BranchName From Branch Where BranchId={0}", branchId);
            var data = Data<CompanyHeirarchyPath>.DataSource(sql).SingleOrDefault();
            return data;
        }
    }
}
