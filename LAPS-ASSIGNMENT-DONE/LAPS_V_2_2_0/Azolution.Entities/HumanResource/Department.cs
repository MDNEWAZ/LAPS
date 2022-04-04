using System;
using System.Collections.Generic;

namespace Azolution.Entities.HumanResource
{
    //[Serializable]
    public class Department
    {
        public Department()
        {
        }
        public int DepartmentId { get; set; }
        public int CompanyId { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyName { get; set; }
        public int DepartmentHeadId { get; set; }
        public string DepartmentHeadName { get; set; }
        public List<Designation> Designations { get; set; } 
        public int TotalCount { get; set; }
        public int IsActive { get; set; }
    }
}
