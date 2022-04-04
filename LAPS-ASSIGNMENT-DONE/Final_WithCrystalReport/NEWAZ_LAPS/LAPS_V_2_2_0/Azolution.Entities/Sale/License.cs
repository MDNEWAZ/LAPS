using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class License
    {
        public int Id { get; set; }
        public int Sl { get; set; }
        public string Number { get; set; }
        public int LType { get; set; }
        public DateTime IssueDate { get; set; }
        public int Status { get; set; }// IsActive
        public DateTime EntryDate { get; set; }
        public DateTime Updated { get; set; }
        public int Flag { get; set; }
        public Product AProduct { get; set; }
        public int IsSMSSent { get; set; }
        public string SaleInvoice { get; set; }

      
        public string MobileNumber { get; set; }

        public int VarificationType { get; set; }
        
    }
}
