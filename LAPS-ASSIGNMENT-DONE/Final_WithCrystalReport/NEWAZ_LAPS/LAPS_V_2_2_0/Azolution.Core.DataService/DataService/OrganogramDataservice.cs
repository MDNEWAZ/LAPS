using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Common;
using Utilities;

namespace Azolution.Core.DataService.DataService
{
    public class OrganogramDataservice
    {
        public List<OrganogramTree> GetOrganogramTreeData(int companyId)
        {
            var objOrganoGram = new List<OrganogramTree>();
            try
            {
//                string quary = string.Format(@"Select Company.CompanyId,CompanyName,MotherId ,BranchID,BranchName,Department.DepartmentId,DepartmentName,DesignationId,DesignationName
//                from Company
//                left outer join Branch on Branch.CompanyId = Company.CompanyId
//                left outer join Department on Department.CompanyId = Company.CompanyId
//                left outer join Designation on Designation.DepartmentId = Department.departmentId");

                string quary = string.Format(@"  with hierarchy (CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart,IsActive)
                        as
                        ( select CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart,IsActive
                          from   Company
                          where  CompanyId={0}
                          union all
                          select x.CompanyId
                        , x.CompanyName
                        , x.PrimaryContact
                        , x.Email
                        , x.Fax
                        , x.Phone
                        , x.Address
                        , x.FullLogoPath
                        , x.MotherId
                        ,x.Flag
                        ,x.FiscalYearStart
                        , x.IsActive
                          from   Company x join hierarchy y
                                 on (y.CompanyId = x.MotherId)
                        )
                        select *
                        from   hierarchy
                          left outer join Branch on Branch.CompanyId = hierarchy.CompanyId
                         where hierarchy.IsActive=1
                        ORDER BY hierarchy.MotherId ASC", companyId);
                
                
                
               

                objOrganoGram = Data<OrganogramTree>.DataSource(quary);

            }
            catch (Exception)
            {

                throw;
            }
            return objOrganoGram;
        }
    }
}
