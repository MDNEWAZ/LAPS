using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Azolution.Entities.DTO;
namespace Laps.Sale.SaleService.Interface
{
    public interface ISalesProcess
    {
        string SaveAsBooked(SalesParam salesParam, bool iSAtumated);
        string Draft(List<Azolution.Entities.Sale.Sale> objSalesObjList, Users user);
        string Sold(List<Azolution.Entities.Sale.Sale> salesList, Users user);
        string MakeUnRecognized(SaleSummary sale);
        string SaveSaleForSpecialPackage(SalesParam saleParam, bool iSAtumated);
    }
}
