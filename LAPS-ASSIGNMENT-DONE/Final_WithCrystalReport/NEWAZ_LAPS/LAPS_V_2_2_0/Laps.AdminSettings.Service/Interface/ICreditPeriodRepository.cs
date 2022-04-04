using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.Service.Interface
{
    public interface ICreditPeriodRepository
    {
        string SaveCreditPeriod(CreditPeriod objCreditPeriodInfo);

        GridEntity<CreditPeriod> GetCreditPeriodSummary(GridOptions options);
    }
}
