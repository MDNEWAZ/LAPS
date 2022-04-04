using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.HumanResource
{
    class Patient
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientRegNo { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public int NationalId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DocctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        
    }
}
