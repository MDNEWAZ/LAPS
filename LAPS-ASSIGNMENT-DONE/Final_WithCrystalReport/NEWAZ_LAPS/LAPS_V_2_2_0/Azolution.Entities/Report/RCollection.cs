using System;
using System.Collections.Generic;
using Azolution.Entities.Sale;

namespace Azolution.Entities.Report
{
    public class RCollection
    {

        public string Invoice { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string NID { get; set; }
        public decimal Amount { get; set; }
        public decimal NetPrice { get; set; }
        public decimal DueAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string DuePercent { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public string CustomerCode { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public int IsUpgraded { get; set; }
        public string ReferenceId { get; set; }
        public string ReceivePercent { get; set; }
        public int IsStaff { get; set; }
        public int BranchId { get; set; }
        public string BranchSmsMobNo { get; set; }

        public Product AProduct { get; set; }
        public int CustomerId { get; set; }
       
        public int IsCustomerUpgraded { get; set; }
        public int SaleTypeId { get; set; }
        public string ProductId { get; set; }
        
    }
}