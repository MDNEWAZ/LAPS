using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;

namespace Laps.Sale.SaleService.Interface
{
    public interface ISalesInactive
    {
        List<Azolution.Entities.Sale.Sale> GetCustomerSalesInformation(string customerCode,int branchId,int companyId);
        string MakeInactive(int saleId, string invoice);
      
        string SaleBranchSwitch(SaleRollback rollbackInfo, Users user);
    }
}
