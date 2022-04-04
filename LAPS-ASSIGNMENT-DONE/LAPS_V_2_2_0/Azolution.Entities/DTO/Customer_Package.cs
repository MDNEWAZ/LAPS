using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
   public class CustomerPackage
    {
       public int CustomerId { get; set; }
       public string Name { get; set; }
       public string CustomerCode { get; set; }
       public int CompanyId { get; set; }
       public int BranchId { get; set; }
       public List<PackageInfo> PackageInfos { get; set; }
    }

    public class PackageInfo
    {
        public int ModelId { get; set; }
        public string ProductName { get; set; }
    }
}
