using Azolution.Entities.Core;

namespace Azolution.Core.Service.Interface
{
    public interface ISystemSettingsRepository
    {
        SystemSettings GetSystemSettingsDataByCompanyId(int companyId);

        string SaveSystemSettings(SystemSettings objSystemSettings);

        System.Data.DataTable GetSystemSettingsData();
    }
}
