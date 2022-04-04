using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Report;
using Azolution.Entities.Sale;

namespace Azolution.Reports.Service.Interface
{
   public interface ISaleReportRepository
    {
        DataTable GetAllSaleReport();
      
    }
}
