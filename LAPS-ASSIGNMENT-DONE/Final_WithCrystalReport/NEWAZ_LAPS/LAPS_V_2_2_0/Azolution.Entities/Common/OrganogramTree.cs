using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.HumanResource;

namespace Azolution.Entities.Common
{
    public class OrganogramTree
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int MotherId { get; set; }

        public int BranchId { get; set; }

        public string BranchName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int DesignationId { get; set; }

        public string DesignationName { get; set; }

        public List<Branch> Branches { get; set; } 
    }
}
