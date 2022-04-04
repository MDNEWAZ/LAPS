using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.Service.Interface
{
    public interface IRatingConfigurationRepository
    {
        string GetACompayDueKpi(GridOptions options, Users user);
        string DueSave(Due aDue, int userId);
    }
}
