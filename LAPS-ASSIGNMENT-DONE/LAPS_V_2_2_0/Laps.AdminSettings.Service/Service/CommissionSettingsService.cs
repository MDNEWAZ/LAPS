using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Laps.AdminSettings.DataService.DataService;
using Laps.AdminSettings.Service.Interface;
using Utilities;

namespace Laps.AdminSettings.Service.Service
{
    public class CommissionSettingsService:ICommissionSettingsRepository
    {
        CommissionSettingsDataService _commissionSettingsDataService = new CommissionSettingsDataService();
        public string SaveCommissionSettings(Commission objCommission)
        {
            return _commissionSettingsDataService.SaveCommissionSettings(objCommission);
        }

        public object GetCommissionSettingsSummary(GridOptions options)
        {
            return _commissionSettingsDataService.GetCommissionSettingsSummary(options);
        }

        public CommissionAmount GetCommissionAmountBySaleRepType(int salesRepTypeId)
        {
            return _commissionSettingsDataService.GetCommissionAmountBySaleRepType(salesRepTypeId);
        }

        public List<Commission> GetCommissionInfoBySaleRepType(int salesRepType)
        {
            return _commissionSettingsDataService.GetCommissionInfoBySaleRepType(salesRepType);
        }
    }
}
