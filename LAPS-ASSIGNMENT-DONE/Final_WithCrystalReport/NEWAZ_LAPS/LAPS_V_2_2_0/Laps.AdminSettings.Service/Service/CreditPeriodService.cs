using Azolution.Entities.Sale;
using Laps.AdminSettings.DataService.DataService;
using Laps.AdminSettings.Service.Interface;
using Utilities;

namespace Laps.AdminSettings.Service.Service
{
    public class CreditPeriodService : ICreditPeriodRepository
    {
        CreditPeriodDataService dataService = new CreditPeriodDataService();
        public string SaveCreditPeriod(CreditPeriod objCreditPeriodInfo)
        {
            return dataService.SaveCreditPeriod(objCreditPeriodInfo);
        }

        public GridEntity<CreditPeriod> GetCreditPeriodSummary(GridOptions options)
        {
            return dataService.GetCreditPeriodSummary(options);
        }
    }
}
