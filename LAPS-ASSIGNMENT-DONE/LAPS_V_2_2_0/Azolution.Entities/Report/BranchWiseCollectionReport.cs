using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Report
{
    public class BranchWiseCollectionReport
    {
        public string BranchName { get; set; }
        public string Invoice { get; set; }
        public decimal InsAmount { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public int TotalInstallment { get; set; }
        public int NoOfPaidInstallment { get; set; }
        public decimal LastCollection { get; set; }
        public DateTime CollectionDate { get; set; }
        public string TranactionId { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalDueAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCollection { get; set; }

    }
}
