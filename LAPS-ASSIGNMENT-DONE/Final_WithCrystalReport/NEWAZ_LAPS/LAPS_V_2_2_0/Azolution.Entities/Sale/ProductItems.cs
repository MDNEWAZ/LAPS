using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class ProductItems
    {
        public ProductItems() { }

        public int ItemId { get; set; }
        public int ModelId { get; set; }
        public string ItemName { get; set; }
        public string ItemModel { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public int BundleQuantity { get; set; }
        public decimal Price { get; set; }

        public bool IsLisenceRequired { get; set; }
        public bool IsPriceApplicable { get; set; }
        public int WarrantyPeriod { get; set; }

        public decimal PackagePrice { get; set; }

        public string ItemCode { get; set; }

        public ItemCodeType ItemCodeType { get; set; }

    }

    public class ItemCodeType
    {
        public int  ItemCodeId { get; set; }
        public string ItemCode { get; set; }   
    }

}

