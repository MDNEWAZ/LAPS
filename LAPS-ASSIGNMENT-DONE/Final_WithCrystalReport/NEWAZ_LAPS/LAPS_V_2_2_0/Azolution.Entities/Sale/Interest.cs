using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;

namespace Azolution.Entities.Sale
{
    public class Interest
    {
        public int InterestId  { get; set; }
        public int DownPay { get; set; } 
        public int Interests { get; set; }
        public int Status { get; set; }
        public string EntryDate { get; set; }
        public string Updated { get; set; }
        public int UserId { get; set; }
        public int UpdateBy { get; set; }
        public int DefaultInstallmentNo { get; set; }
        public Company ACompany { get; set; }
        public decimal DefaultCashDiscount { get; set; }
        public decimal CashDiscountPercentage { get; set; }
      
    }
}
