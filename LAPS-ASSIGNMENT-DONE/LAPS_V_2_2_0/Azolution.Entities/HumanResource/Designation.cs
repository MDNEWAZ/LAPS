using System;

namespace Azolution.Entities.HumanResource
{
     [Serializable]
  public  class Designation
    {
        public Designation(){}
        public Int32 DesignationId { get; set; }
        public string DesignationName { get; set; }
        public Int32 CompanyId { get; set; }
        public Int16 Status { get; set; }

        public int DepartmentId { get; set; }
         public int TotalCount { get; set; }
    }
}
