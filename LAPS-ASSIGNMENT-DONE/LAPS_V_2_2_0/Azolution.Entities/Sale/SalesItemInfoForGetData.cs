using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SalesItemInfoForGetData
    {
        public int SalesItemId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public int SaleId { get; set; }
        public string ItemModel { get; set; }
        public int SalesQty { get; set; }
        public decimal Price { get; set; }
        public int BundleQuantity { get; set; }
        public bool IsLisenceRequired { get; set; }
        public bool IsPriceApplicable { get; set; }

        public DateTime WarrantyEndDate { get; set; }
        public int RemainingWarrantyPeriod { get; set; }
        public int ItemWarrantyPeriod { get; set; }

        public decimal PackagePrice { get; set; }
        public int ModelId { get; set; }
    }
}
