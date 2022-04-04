using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class UnRecognizeSale
    {
        public int SaleId { get; set; }
        public string Invoice { get; set; }
        public string CustomerCode { get; set; }
        public string NationalId { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public int ModelId { get; set; }
        public string BranchCode { get; set; }
        public decimal DownPay { get; set; }
        public decimal ReceiveAmount { get; set; }
        public decimal NewCollectedAmount { get; set; }
        public string TransectionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Price { get; set; }
        public int State { get; set; }
        public int TempState { get; set; }
        public int TypeOfUnRecognized { get; set; }
        public int IsSpecialDiscount { get; set; }
        public int IsApprovedSpecialDiscount { get; set; }
    }
}
