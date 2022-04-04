using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
    public class BranchInfo
    {
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int CompanyStock { get; set; }
        public string CompanyType { get; set; }
        public int RootCompanyId { get; set; }
    }
}
