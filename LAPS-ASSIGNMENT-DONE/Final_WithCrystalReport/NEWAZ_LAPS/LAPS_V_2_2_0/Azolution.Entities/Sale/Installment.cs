using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class Installment
    {
        public int InstallmentId { get; set; }
        public string SInvoice { get; set; }
        public int Number { get; set; }
        //public int Amount { get; set; }
        public decimal Amount { get; set; }
        //public int ReceiveAmount { get; set; }
        //public int DueAmount { get; set; }

        public decimal ReceiveAmount { get; set; }
        public decimal DueAmount { get; set; }

        public int Status { get; set; }
        public string DueDate { get; set; }
        public string PayDate { get; set; }
        public string EntryDate { get; set; }
        public string Updated { get; set; }
        public int Flag { get; set; }
        public int PaymentType { get; set; }

        public Product AProduct { get; set; }
        public string ProductNo { get; set; }
        public Collection ACollection { get; set; }

    }
}