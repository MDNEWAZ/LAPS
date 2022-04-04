using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Report
{
    public class ReportParam
    {
        public ReportParam()
        {
            
        }

        public int ReportType { get; set; }
        public string Zone { get; set; }
        public int ZoneId { get; set; }
        public string Region { get; set; }
        public int RegionId { get; set; }
        public string Branch { get; set; }
        public int BranchId { get; set; }
        public string Package { get; set; }
        public int PackageId { get; set; }
        public string SaleRep { get; set; }
        public int SalesRepId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StockType { get; set; }
        public string Color { get; set; }
        public int DueId { get; set; }
        public string SearchParam
        {
            get { return SetFilterParam(); }
        }

        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }

        public int Company { get; set; }
        private string SetFilterParam()
        {
            var filter = "";
           // var filter ="Sale & Collection:";// ReportType == 1 ? "Sale Report-" : "Collection Report-";
            if (Region != null)
            {
                filter = filter + "Region:" + Region+"/";
            }
            if (Zone != null)
            {
                filter = filter + "Zone:" + Zone+"/";
            }
            if (Branch != null)
            {
                filter = filter + "Branch:" + Branch+"/";
            }
            if (Package != null)
            {
                filter = filter + "Package:" + Package+"/";
            }
            if (SaleRep != null)
            {
                filter = filter + "SaleRep:" + SaleRep;
            }
            if (Color != null)
            {
                filter = filter + "Status:" + Color;
            }
            if (CustomerCode != null)
            {
                filter = filter + "Customer ID:" + CustomerCode;
            }
            return filter;
        }
    }
}
