using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Sale.SaleService.Interface
{
    public interface IReSchedule
    {
        bool ReScheduleIm(string customerCode, int newImNo, DateTime firstPayDate,int companyId,int branchId);

        IEnumerable<Installment> GetExistingInstallmentScheduls(string customerCode, int companyId, int branchId);
    }
}
