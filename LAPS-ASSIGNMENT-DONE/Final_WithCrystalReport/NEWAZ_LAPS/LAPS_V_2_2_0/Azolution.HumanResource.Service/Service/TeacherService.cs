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
    public class TeacherService
    {
       
        
            TeacherDataService _teacherDataService = new TeacherDataService();
            public List<Department> GetAllDepartmentNameForCombo()
            {
                return _teacherDataService.GetAllDepartmentNameForCombo();
            }
        }
 }




  
