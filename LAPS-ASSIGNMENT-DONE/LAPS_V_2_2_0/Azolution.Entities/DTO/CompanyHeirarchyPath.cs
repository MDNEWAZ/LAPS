using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
    public class CompanyHeirarchyPath
    {
        public string RootCompanyName { get; set; } 
        public string MotherCompanyName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
    }
}
