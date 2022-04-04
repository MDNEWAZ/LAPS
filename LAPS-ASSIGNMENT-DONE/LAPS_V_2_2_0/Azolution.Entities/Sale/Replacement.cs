using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class Replacement
    {
        public int ReplacementId { get; set; }
        public string ReplaceInvoiceNo { get; set; }
        public decimal AdditionPrice { get; set; }
        public string SaleMode { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime Updated { get; set; }
        public int SaleId { get; set; }
        public int Flag { get; set; }
        public int IsActive { get; set; }

        public string ReplacedItemSLNo { get; set; }
        public int TypeId { get; set; }
        public string ReplacedChalanNo { get; set; }
        public string ReplacedItemModel { get; set; }

        public DateTime ReplacementDate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime InstallmentDate { get; set; }
       // public DateTime WarrantyEndDate { get; set; }
        public string Type { get; set; }

        public Product AProduct { get; set; }
        public Customer ACustomer { get; set; }
        public License ALicense { get; set; }

        public int RefItemId { get; set; }
        public string RefItemSLNo { get; set; }
        public string SaleInvoice { get; set; }

        public int SalesItemId { get; set; }

    }
}
