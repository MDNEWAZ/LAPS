using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.DTO
{
    public class WaitingForDiscountSummaryDto
    {
        public int DiscountId { get; set; }
        public int SaleId { get; set; }
        public string Invoice { get; set; }
        public decimal Price { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int IsActive { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public int IsSpecialDiscount { get; set; }
        public string SalesRepId { get; set; }
        public int StaffId { get; set; }
        public int SaleTypeId { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public int DiscountOptionId { get; set; }
        public decimal DownPay { get; set; }
        public string DiscountTypeCode { get; set; }

        public int DownPayPercent { get; set; }
        public int Interests { get; set; }
        public int InstallmentNo { get; set; }
        public int State { get; set; }
        public int TempState { get; set; }
        public string BranchCode { get; set; }
        public string DiscountTypeName { get; set; }
        public int IsDPFixedAmount { get; set; }
    }  
}
