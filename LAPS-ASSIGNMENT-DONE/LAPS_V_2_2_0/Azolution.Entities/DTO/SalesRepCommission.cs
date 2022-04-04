using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
    public class SalesRepCommission
    {
        public string SalesRepId { get; set; }
        public int SalesRepType { get; set; }
        public string SalesRepCode { get; set; }
        public string Address { get; set; }
        public string SalesRepSmsMobNo { get; set; }
        public string SalesRepBkashNo { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string SalesRepTypeName { get; set; }

        public int TotalSaleCash { get; set; }
        public int TotalSaleInstallment { get; set; }
        public string SaleMonthYear { get; set; }
        public int IsIncentiveActive { get; set; }
        public int IsCommissionActive { get; set; }

    }
}
