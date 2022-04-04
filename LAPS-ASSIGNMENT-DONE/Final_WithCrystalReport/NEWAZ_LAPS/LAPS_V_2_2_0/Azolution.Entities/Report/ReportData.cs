using System.Collections.Generic;

namespace Azolution.Entities.Report
{
    public class ReportData<T>{

        public List<T> DataSource { get; set; }
        public string RptName { get; set; }
        public string ReportName { get; set; }
        public string PageTile { get; set; }
        public string FilterKeys { get; set; }
    }
}