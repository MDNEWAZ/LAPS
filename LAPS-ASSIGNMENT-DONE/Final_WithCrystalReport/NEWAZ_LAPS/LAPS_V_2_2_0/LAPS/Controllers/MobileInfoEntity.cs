using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LAPS.Controllers
{
   public class MobileInfoEntity<T>
    {
        public List<T> DataSource { get; set; }
        public string RptName { get; set; }
        public string ReportName { get; set; }
        public string PageTile { get; set; }
        public string FilterKeys { get; set; }
    
       

    }
}
