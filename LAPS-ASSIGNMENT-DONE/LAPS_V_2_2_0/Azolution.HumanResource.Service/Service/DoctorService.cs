using Azolution.Core.Service.Interface;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Azolution.HumanResource.Service.Service
{
  public  class DoctorService: IDoctorRepository
    {
      DoctorDataService _doctorDataService = new DoctorDataService();
        public List<Department> GetAllDepartmentNameForCombo()
        {
            return _doctorDataService.GetAllDepartmentNameForCombo();
        }
        public string SaveDoctor(Doctor doctor)
        {
            return _doctorDataService.SaveDoctor(doctor);
        }

        public GridEntity<Doctor> GetDoctorSummary(GridOptions options)
        {
            return _doctorDataService.GetDoctorSummary(options);
        }
        public GridEntity<DoctorEducationInfo> GetDoctorEducationinfoSummary(GridOptions options, int id)
        {
            return _doctorDataService.GetDoctorEducationinfoSummary(options, id);
        }
        public List<Doctor> GetDoctorInfoReport()
        {
            return _doctorDataService.GetDoctorInfoReport();
        }
        public string DeleteDoctor(int DoctorId)
        {
            return _doctorDataService.DeleteDoctor(DoctorId);
        }

    }
}
