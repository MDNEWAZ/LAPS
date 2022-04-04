using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class PaymentReceivedInfo
    {
        public int CollectionId { get; set; }

        public string SaleInvoice { get; set; }
        public decimal ReceiveAmount { get; set; }
        public int CollectionType { get; set; }
        public int PaymentType { get; set; }
        public string TransectionId { get; set; }
        public int TransactionType { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string BranchSmsMobileNumber { get; set; }
        public DateTime PayDate { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerId { get; set; }
        public int SimplePaymentCollectionId { get; set; }
        public int SetteledId { get; set; }

        public int SaleId { get; set; }

        public string BranchCode { get; set; }
        public int IsCustomerUpgraded { get; set; }

        public int SaleTypeId { get; set; }

        public string CustomerName { get; set; }
        public string ProductId { get; set; }
        public int TypeId { get; set; }
        public DateTime SaleDate { get; set; }

    }
}
