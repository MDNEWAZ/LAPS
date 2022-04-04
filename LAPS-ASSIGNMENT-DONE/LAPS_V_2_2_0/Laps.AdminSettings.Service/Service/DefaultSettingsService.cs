using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.AdminSettings.DataService.DataService;
using Laps.AdminSettings.Service.Interface;
using Utilities;

namespace Laps.AdminSettings.Service.Service
{
    public class DefaultSettingsService : IDefaultSettingsRepository
    {
        readonly DefaultSettingsDataService _dataService = new DefaultSettingsDataService();
        public object GetAllInterestInfoByCompanyId(int companyId)
        {
            return _dataService.GetAllInterestInfoByCompanyId(companyId);
        }

        public object GetDefaultSettingsSummaryCompanies(GridOptions options, string companies)
        {
            return _dataService.GetDefaultSettingsSummaryCompanies(options, companies);
        }

        public string SaveDefaultSettings(Interest aInterest, int userId)
        {
            return _dataService.SaveDefaultSettings(aInterest, userId);
        }

        public string SaveOperationModeSettings(OperationMode operationModeObj, Users user)
        {
            return _dataService.SaveOperationModeSettings(operationModeObj, user);
        }

        public OperationMode GetOperationModeSettings()
        {
            return _dataService.GetOperationModeSettings();
        }
    }
}
