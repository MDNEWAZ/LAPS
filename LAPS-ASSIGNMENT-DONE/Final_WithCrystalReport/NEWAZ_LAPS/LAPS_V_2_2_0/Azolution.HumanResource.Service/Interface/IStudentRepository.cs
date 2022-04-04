using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.HumanResource;
using Utilities;

namespace Azolution.HumanResource.Service.Interface
{
    public interface IStudentRepository
    {
        List<Department> GetAllDepartmentNameForCombo();
        string SaveStudent(Student student);
        GridEntity<Student> GetStudentSummary(GridOptions options);
        GridEntity<StudentEducationInfo> GetStudentEducationinfoSummary(GridOptions options, int id);
        List<Student> GetStudentInfoReport();
    }
}
