using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class Stock
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string Status { get; set; }
        public string EntryDate { get; set; }
        public string Updated { get; set; }
        public int Flag { get; set; }
        public int EntryUserId { get; set; }
        public int UpdateUserId { get; set; }

        public int ModelId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public Product AProduct { get; set; }
        public AllType AAllType { get; set; }


        public int CompanyId { get; set; }
        public int BranchId { get; set; }

        public string DeliveryChalanNo { get; set; }
        public string DeliveryOrderNo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string QBInvoiceNo { get; set; }
        public int StockCategoryId { get; set; }

    }
}
