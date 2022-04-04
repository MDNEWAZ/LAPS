using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.DataService.DataService;
using Utilities;

namespace Azolution.HumanResource.Service.Service
{
    public class EmployeeService : IEmployeeRepository
    {
        EmployeeDataService _employeeDataService = new EmployeeDataService();
        public string SaveEmployee(Employee employee)
        {
            return _employeeDataService.SaveEmployee(employee);
        }

        public GridEntity<Employee> GetEmployeeSummary(GridOptions options)
        {
            return _employeeDataService.GetEmployeeSummary(options);
        }

        public GridEntity<Employee> GetAllEmployeeSummary(GridOptions options)
        {
            return _employeeDataService.GetAllEmployeeSummary(options);
        }

        public string UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }


        public string NewSaveEmployee(Employee employee)
        {
           return _employeeDataService.NewSaveEmployee(employee);
        }

        public GridEntity<EducationDetail> GetEducation(GridOptions options)
        {
            return new GridEntity<EducationDetail>();
        }

        public GridEntity<EducationDetail> GetEducationSummary(GridOptions options,int id)
        {
            return _employeeDataService.GetEducationSummary(options,id);
        }

        public GridEntity<Experience> GetExperienceSummary(GridOptions options, int id)
        {
            return _employeeDataService.GetExperienceSummary(options, id);
        }

        public List<Employee> GetEmployeeInfoForCheckBox(int id)
        {
            return _employeeDataService.GetEmployeeInfoForCheckBox(id);
        }

        //public List<Experience> GetExperienceById(int id, Users objUser)
        //{

        //    string condition = "";
        //    if (id > 0)
        //    {
        //        condition = " Where ExperienceId=" + id;
        //    }
        //    return _employeeDataService.GetExperienceById(condition);
        //}
    }
}
