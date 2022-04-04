using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Laps.Sale.SaleService.Interface
{
    public interface IPaymentSchedule
    {
        string MakePaymentSchedule(int installmentNo, int saleId, string invoiceNo, decimal netPrice, DateTime firstPayDate, int saleType, CommonConnection connection);
    }
}
