using Azolution.Entities.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Azolution.Core.Service.Interface
{
 public  interface IDoctorRepository
    {
        List<Department> GetAllDepartmentNameForCombo();
        string SaveDoctor(Doctor doctor);
        GridEntity<Doctor> GetDoctorSummary(GridOptions options);
        GridEntity<DoctorEducationInfo> GetDoctorEducationinfoSummary(GridOptions options, int id);
        List<Doctor> GetDoctorInfoReport();
        string DeleteDoctor(int DoctorId );

    }
}
