using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
    public class CompanyBranchInfo
    {
        public string BranchCode { get; set; }
        public int CompanyStock { get; set; }
        public string CompanyType { get; set; }
        public int MotherId { get; set; }
        public int RootCompanyId { get; set; }
    }
}
