using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;

namespace Azolution.Entities.DTO
{
    public class CollectionDto
    {
        public int CollectionId { get; set; }

        public int InstallmentId { get; set; }
        public int InstallmentNo { get; set; }
        public string SaleInvoice { get; set; }
        public string Contant { get; set; }
        public int PaymentType { get; set; }
        public string TransectionId { get; set; }
        public decimal ReceiveAmount { get; set; }
        public decimal DueAmount { get; set; }
        public int Flag { get; set; }
        public int IsRead { get; set; }
        public int CollectionType { get; set; }

        public DateTime PayDate { get; set; }
        public DateTime DueDate { get; set; }

        public int CollectedBy { get; set; }
        public DateTime EntryDate { get; set; }

        public Installment AInstallment { get; set; }
        public Product AProduct { get; set; }
        public Customer ACustomer { get; set; }

        //To devide payment status
        public int PaymentStatus { get; set; }

        public decimal DownPay { get; set; }

        public int FinalInstallment { get; set; }

        public int SaleId { get; set; }
        public int SaleTypeId { get; set; }

        public string CollectionTypeName { get; set; }
    }
}
