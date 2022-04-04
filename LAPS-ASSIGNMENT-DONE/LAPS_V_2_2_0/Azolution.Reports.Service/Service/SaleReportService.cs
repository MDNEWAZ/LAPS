using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Report;
using Azolution.Entities.Sale;
using Azolution.Reports.DataService.DataService;
using Azolution.Reports.Service.Interface;

namespace Azolution.Reports.Service.Service
{
    public class SaleReportService :ISaleReportRepository
    {
        
        DataTable ISaleReportRepository.GetAllSaleReport()
        {
            var aService = new SaleReportDataService();
            return aService.GetAllSaleReport();
        }

       
    }
}
