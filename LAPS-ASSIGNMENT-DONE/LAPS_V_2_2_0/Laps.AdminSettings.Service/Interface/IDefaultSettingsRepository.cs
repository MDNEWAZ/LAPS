
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.Service.Interface
{
   public interface IDefaultSettingsRepository
    {
       object GetAllInterestInfoByCompanyId(int companyId);

       object GetDefaultSettingsSummaryCompanies(GridOptions options, string companies);
       string SaveDefaultSettings(Interest aInterest, int userId);
       string SaveOperationModeSettings(OperationMode operationModeObj, Users user);
       OperationMode GetOperationModeSettings();
    }
}
