using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class Product
    {
        public int ModelId { get; set; }
        public string Model { get; set; }
        public string ProductName { get; set; }
        public string ProductNo { get; set; }
        public string Code { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string ProductType { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Capacity { get; set; }
        public string ManufactureDate { get; set; }
        public string EntryDate { get; set; }
        public string Updated { get; set; }
        public int Flag { get; set; }
        public int IsActive { get; set; }
        public License ALicense { get; set; }

        ///////// Here Extra Entity For Data Passing ///////////
        public int TotalStock { get; set; }
        public int TotalSale { get; set; } 
        public int CurrentStock { get; set; }
        public string WarrantyEndDate { get; set; }
        public string IssueDate { get; set; }

        public int MinNumber { get; set; }

        public int CompanyId { get; set; }
        public int TotalPrice { get; set; }

        public int DownPayPercent { get; set; }

        public int PackageType { get; set; }
        public int IsDPFixedAmount { get; set; }
        public int DefaultInstallmentNo { get; set; }
        public int ModelItemID { get; set; }
        public string ProductTypeName { get; set; }
    }
}
