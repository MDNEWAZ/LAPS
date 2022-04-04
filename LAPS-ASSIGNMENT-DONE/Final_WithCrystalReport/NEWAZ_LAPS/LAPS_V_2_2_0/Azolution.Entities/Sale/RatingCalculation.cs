using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class RatingCalculation
    {
        public string Invoice { get; set; }
     
        public decimal Amount { get; set; }
        public decimal OutStandingAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal TotalDuePercent { get; set; }
        public decimal TotalReceivePercent { get; set; }
        public decimal DownPayment { get; set; }
        public decimal PaymentTillDate { get; set; }
        public decimal ReceiveAmountTillDate { get; set; }
        public decimal TotalReceivePercentTillDate { get; set; }
        public decimal TotalDuePercentTillDate { get; set; }

        public decimal RequiredReceiveAmountTillDate { get; set; }

        public decimal DueAmountTillDate { get; set; }

        public string CustomerCode { get; set; }
        public string ProductName { get; set; }
        public string Model { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public int IsRelease { get; set; }
        public int CompanyId { get; set; }
        public string BranchCode { get; set; }
        public int Type { get; set; }
        public string ProductTypeName { get; set; }
    }
}
