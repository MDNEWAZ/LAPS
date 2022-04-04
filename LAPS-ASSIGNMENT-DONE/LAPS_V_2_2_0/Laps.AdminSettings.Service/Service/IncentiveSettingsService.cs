using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;
using Laps.AdminSettings.DataService.DataService;
using Laps.AdminSettings.Service.Interface;
using Utilities;

namespace Laps.AdminSettings.Service.Service
{
   public class IncentiveSettingsService:IIncentiveSettingsRepository
    {
       IncentiveSettingsDataService _incentiveSettingsDataService = new IncentiveSettingsDataService();
       public string SaveIncentiveSettings(Incentive objIncentive)
       {
           return _incentiveSettingsDataService.SaveIncentiveSettings(objIncentive);
       }

       public object GetIncentiveSettingsSummary(GridOptions options)
       {
           return _incentiveSettingsDataService.GetIncentiveSettingsSummary(options);
       }

       public List<Incentive> GetIncentiveSettingsData()
       {
           return _incentiveSettingsDataService.GetIncentiveSettingsData();
       }
    }
}
