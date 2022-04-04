using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;

namespace Azolution.Entities.Report
{
    public class CustomerStatusReport : RatingCalculation
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CompanyName { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public string FilterOrganogram { get; set; }
        public string BranchName { get; set; }
        public string Status { get; set; }
        public string LicenseCode { get; set; }
        public DateTime IssueDate { get; set; }
        public string License { get; set; }

    }
}
