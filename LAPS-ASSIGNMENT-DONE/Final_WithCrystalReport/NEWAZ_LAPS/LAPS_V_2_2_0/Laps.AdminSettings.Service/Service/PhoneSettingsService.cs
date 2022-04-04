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
   public class PhoneSettingsService:IPhoneSettingsRepository
    {
       PhoneSettingsDataService _phoneSettingsDataService = new PhoneSettingsDataService();
       public GridEntity<PhoneNoSettings> GetAllPhoneSettings(GridOptions options)
       {
           var data= _phoneSettingsDataService.GetAllPhoneSettings(options);
           GetRandomPhoneNumber();
           return data;
       }

       public string SavePhoneSettings(PhoneNoSettings phoneObj)
       {
           return _phoneSettingsDataService.SavePhoneSettings(phoneObj);
       }

       public string GetRandomPhoneNumber()
       {
           string phoneNo = "";
           var phoneList= _phoneSettingsDataService.GetAllActivePhoneNumber();
           if (phoneList.Count > 0)
           {
               Random rnd = new Random();
               int index = rnd.Next(phoneList.Count);
               phoneNo = phoneList[index].PhoneNumber;
           }

           return phoneNo;
       }
    }
}
