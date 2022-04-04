using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Utilities;
using Azolution.Entities.HumanResource;

namespace Azolution.HumanResource.DataService.DataService
{
    public class TeacherDataService
    {
        public List<Department> GetAllDepartmentNameForCombo()
        {
            var query = "";
            try
            {
                query = string.Format(@"select Department.DepartmentId, Department.DepartmentName from department");
            }
            catch (Exception)
            {

                throw;
            }
            return Kendo<Department>.Combo.DataSource(query);
        }




        public List<Designation> GetAllDesignationNameForCombo()
        {
            var query = "";
            try
            {
                query = string.Format(@"select  DESIGNATION.DESIGNATIONID, DESIGNATION.DESIGNATIONNAME from DESIGNATION");
            }
            catch (Exception)
            {

                throw;
            }
            return Kendo<Designation>.Combo.DataSource(query);
        }
    }
}
