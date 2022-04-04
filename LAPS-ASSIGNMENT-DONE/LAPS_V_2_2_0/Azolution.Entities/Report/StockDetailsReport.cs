using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Report
{
    public class StockDetailsReport
    {
        public DateTime ChalanDate { get; set; }
        public string ChalanNo { get; set; }
        public string Model { get; set; }
        public string DOnumbeer { get; set; }
        public string ItemName { get; set; }
        public string ItemModel { get; set; }
        public int Quantity { get; set; }
        public string QBInvoiceNo { get; set; }
        public string DeliveryType { get; set; }
        public string IssuedFrom { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }

    }
}
