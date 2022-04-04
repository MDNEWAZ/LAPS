using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataService;
using Azolution.HumanResource.Service.Interface;
using Utilities;

namespace Azolution.HumanResource.Service.Service
{
    public class StudentService: IStudentRepository
    {
        StudentDataService _studentDataService = new StudentDataService();
        public List<Department> GetAllDepartmentNameForCombo()
        {
            return _studentDataService.GetAllDepartmentNameForCombo();
        }
        public string SaveStudent(Student student)
        {
            return _studentDataService.SaveStudent(student);
        }

        public GridEntity<Student> GetStudentSummary(GridOptions options)
        {
            return _studentDataService.GetStudentSummary(options);
        }
        public GridEntity<StudentEducationInfo> GetStudentEducationinfoSummary(GridOptions options, int id)
        {
            return _studentDataService.GetStudentEducationinfoSummary(options, id);
        }
        public List<Student> GetStudentInfoReport()
        {
          return  _studentDataService.GetStudentInfoReport();
        }
    }
}


public class TeacherService
{
    TeacherDataService _teacherDataService = new TeacherDataService();
    public List<Department> GetAllDepartmentNameForCombo()
    {
        return _teacherDataService.GetAllDepartmentNameForCombo();
    }

    public List<Designation> GetAllDesignationNameForCombo()
    {
        return _teacherDataService.GetAllDesignationNameForCombo();
    }


}
