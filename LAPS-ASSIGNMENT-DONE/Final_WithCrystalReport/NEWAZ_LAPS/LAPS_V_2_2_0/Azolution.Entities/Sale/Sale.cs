using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class Sale
    {
        
        public int ParentSaleId { get; set; }

        public int SaleId { get; set; }
        public string Invoice { get; set; }
        public decimal Price { get; set; }
        public decimal DownPay { get; set; }
        public decimal NetPrice { get; set; } 

        public int Installment { get; set; }  
        public int WarrantyPeriod { get; set; }
        public int SaleTypeId { get; set; }
        public int SaleUserId { get; set; }

        public string ChallanNo { get; set; }
        public DateTime FirstPayDate { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime Updated { get; set; }
        public int PrantId { get; set; }
        public int Flag { get; set; }
        public int IsActive { get; set; }

        public int IsLisenceRequired { get; set; }

        public int IsRedCustomer { get; set; }

        public string SalesRepId { get; set; }

        public Product AProduct { get; set; }
        public Customer ACustomer { get; set; }
        public License ALicense { get; set; }
        public AllType AType { get; set; }
        public Discount ADiscount { get; set; }

        public int IsSmsSale { get; set; }

        public int State { get; set; }
        public int TempState { get; set; }
        public int TypeOfUnRecognized { get; set; }
        public decimal ReceiveAmount { get; set; }

        public int IsSpecialDiscount { get; set; }

        public int IIM { get; set; }

        public string ItemSlNo { get; set; }
    }
}
