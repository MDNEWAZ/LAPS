using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SalesItem
    {
        public SalesItem(){}

        public int SalesItemId { get; set; }
        public int ItemId { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public int SaleId { get; set; }

        //public DateTime ItemManufactureDate { get; set; }
       // public int ItemWarrantyPeriod { get; set; }
        public string ItemModel { get; set; }
        public int SalesQty { get; set; }
        public decimal Price { get; set; }

        public bool IsLisenceRequired { get; set; }

        public string ItemSLNo { get; set; }

        public ProductItems ProductItems { get; set; }


    }
}
