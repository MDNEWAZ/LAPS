using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.Service.Interface
{
    public interface ICommissionSettingsRepository
    {
        string SaveCommissionSettings(Commission objCommission);
        object GetCommissionSettingsSummary(GridOptions options);
        CommissionAmount GetCommissionAmountBySaleRepType(int salesRepTypeId);
    }
}
