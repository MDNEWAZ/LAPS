using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class Discount
    {

        public decimal DefaultCashDiscount { get; set; }
        public decimal DefaultAgentDiscount { get; set; }

        public decimal CashDiscountPercentage { get; set; }
        public decimal AgentDiscountPercentage { get; set; }
        //------------------------------------------------------------
        public int DiscountId { get; set; }
        public int DiscountTypeId { get; set; }
        public int DiscountOptionId { get; set; }
        public decimal DiscountAmount { get; set; }
        public int SaleId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime EntryDate { get; set; }
        public int IsApprovedSpecialDiscount { get; set; }
        public string DiscountTypeCode { get; set; }
    }
}
