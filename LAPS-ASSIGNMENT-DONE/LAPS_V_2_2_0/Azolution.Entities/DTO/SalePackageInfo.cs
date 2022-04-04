using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
   public class SalePackageInfo
    {
       public int CustomerId { get; set; }
        public string Name { get; set; }
        public string CustomerCode { get; set; }
        public int SaleId { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public PackageInfo PackageInfos { get; set; }
    }
}
