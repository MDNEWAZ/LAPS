using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SimplePaymentCollection
    {
        public int SimplePaymentCollectionId { get; set; }
        public string CustomerCode { get; set; }
        public string SInvoice { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string BranchSmsMobileNumber { get; set; }

        public decimal ReceiveAmount { get; set; }
        public int CollectionType { get; set; }
        public string TransectionId { get; set; }
        public DateTime PayDate { get; set; }
        public int Status { get; set; }

        public string BranchCode { get; set; }
        public string Phone2 { get; set; }
        public int IsCustomerUpgraded { get; set; }
        public string CustomerName { get; set; }

    }
}
