using System.Data;
using Azolution.Core.DataService.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Entities.Core;

namespace Azolution.Core.Service.Service
{
    public class SystemSettingsService : ISystemSettingsRepository
    {
        readonly SystemSettingsDataService _systemSettingsDataService = new SystemSettingsDataService();

        public SystemSettings GetSystemSettingsDataByCompanyId(int companyId)
        {
            return _systemSettingsDataService.GetSystemSettingsDataByCompanyId(companyId);
        }

        public string SaveSystemSettings(SystemSettings objSystemSettings)
        {
            return _systemSettingsDataService.SaveSystemSettings(objSystemSettings);
        }

        public DataTable GetSystemSettingsData()
        {
            return _systemSettingsDataService.GetSystemSettingsData();
        }
    }
}
