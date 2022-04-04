using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.Service.Interface
{
    public interface IPhoneSettingsRepository
    {
        GridEntity<PhoneNoSettings> GetAllPhoneSettings(GridOptions options);
        string SavePhoneSettings(PhoneNoSettings phoneObj);
        string GetRandomPhoneNumber();
    }
}
