using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public int Salary { get; set; }
        public string Exam { get; set; }
        public int Year { get; set; }
        public string Institute { get; set; }
        public string Result { get; set; }
        public List<EducationDetail> education { get; set; }

        public List<Experience> Experiences { get; set; }

        public List<Employee> Employees { get; set; }

        public int EmployeesInfo { get; set; }
    }
}
