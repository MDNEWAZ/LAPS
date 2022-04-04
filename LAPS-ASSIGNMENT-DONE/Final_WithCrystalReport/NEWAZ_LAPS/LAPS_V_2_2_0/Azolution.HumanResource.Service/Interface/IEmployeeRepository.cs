using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Utilities;

namespace Azolution.HumanResource.Service.Interface
{
    public interface IEmployeeRepository
    {
        string SaveEmployee(Employee employee);
        GridEntity<Employee> GetEmployeeSummary(GridOptions options);

        GridEntity<EducationDetail> GetEducation(GridOptions options);

        string NewSaveEmployee(Employee employee);
        GridEntity<Employee> GetAllEmployeeSummary(GridOptions options);
        string UpdateEmployee(Employee employee);
        GridEntity<EducationDetail> GetEducationSummary(GridOptions options, int id);
        GridEntity<Experience> GetExperienceSummary(GridOptions options, int id);

        //List<Experience> GetExperienceById(int id, Users objUser);
        List<Employee> GetEmployeeInfoForCheckBox(int id);



    }
}
