using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;

namespace Azolution.Core.Service.Service
{
    public class OrganogramService : IOrganogramRepository
    {
        readonly OrganogramDataservice _organogramDataservice = new OrganogramDataservice();


        public List<Company> GetOrganogramTreeData(int companyId)
        {
            //string condition = "";
            //if (companyId != 0)
            //{
            //    condition = " Where Company.CompanyId=" + companyId;
            //}

            var obj = _organogramDataservice.GetOrganogramTreeData(companyId);

            //var companies = new List<Company>();

            //foreach (var organogramTree in obj)
            //{
            //    var company = new Company();
            //    company.CompanyId = organogramTree.CompanyId;
            //    company.CompanyName = organogramTree.CompanyName;
            //    company.MotherId = organogramTree.MotherId;
            //    companies.Add(company);
            //}

            //var cp = companies.AsQueryable().GroupBy(c => c.CompanyId).Select(g => g.FirstOrDefault()).ToList();

            var queryForCompany = from item in obj
                                  select
                                      new Company()
                                      {
                                          CompanyName = item.CompanyName,
                                          CompanyId = item.CompanyId,
                                          MotherId = item.MotherId
                                      };

            var cp = queryForCompany.GroupBy(c => c.CompanyId).Select(g => g.FirstOrDefault()).ToList();



            // Branch Sectiom
            foreach (var company in cp)
            {

                var query = from item in obj
                            where item.CompanyId == company.CompanyId
                            select new Branch() { BranchName = item.BranchName, BranchId = item.BranchId };

                var objBranch = query.GroupBy(c => c.BranchId).Select(g => g.FirstOrDefault()).ToList();
                if (objBranch[0].BranchId > 0)
                {
                    company.Branches = objBranch;

                    foreach (var branch in company.Branches)
                    {

                        var queryForDepartment = from item in obj
                                                 where item.CompanyId == company.CompanyId
                                                 select
                                                     new Department()
                                                     {
                                                         DepartmentName = item.DepartmentName,
                                                         DepartmentId = item.DepartmentId
                                                     };

                        var objDepartment = queryForDepartment.GroupBy(c => c.DepartmentId).Select(g => g.FirstOrDefault()).ToList();

                        if (objDepartment[0].DepartmentId > 0)
                        {

                            branch.Departments = objDepartment;

                            foreach (var department in branch.Departments)
                            {
                                var queryForDesignation = from item in obj
                                                          where item.DepartmentId == department.DepartmentId
                                                          select
                                                              new Designation()
                                                              {
                                                                  DesignationName = item.DesignationName,
                                                                  DesignationId = item.DesignationId
                                                              };

                                var objDesignation = queryForDesignation.GroupBy(c => c.DesignationId).Select(g => g.FirstOrDefault()).ToList();
                                if (objDesignation[0].DesignationId > 0)
                                {
                                    department.Designations = objDesignation;
                                }
                            }


                        }
                    }


                }

            }

            return cp;
        }
    }
}
