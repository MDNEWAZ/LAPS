using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;

namespace Azolution.Entities.Report
{
   public class TransactionReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string FilterOrganogram { get; set; }    
        public string Status { get; set; }
        public DateTime PayDate { get; set; }
        public string ReferenceId { get; set; }
        public decimal ReceiveAmount { get; set; }
        public string TransectionId { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public int IsRelease { get; set; }
        public int InstallmentNo { get; set; }

    }
}
