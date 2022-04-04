using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.HumanResource;


namespace Azolution.HumanResource.Service.Interface
{
    public interface ITeacherRepository
    {
        List<Department> GetAllDepartmentNameForCombo();
        List<Designation> GetAllDesignationForCombo();
    }

    

   
}





