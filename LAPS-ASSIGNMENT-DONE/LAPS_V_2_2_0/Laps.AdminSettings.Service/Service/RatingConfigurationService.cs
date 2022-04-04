using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.AdminSettings.DataService.DataService;
using Laps.AdminSettings.Service.Interface;
using Utilities;
using Utilities.Common.Json;

namespace Laps.AdminSettings.Service.Service
{
    public class RatingConfigurationService:IRatingConfigurationRepository
    {
        RatingConfigurationDataService _configurationDataService =new RatingConfigurationDataService();
        public string DueSave(Due aDue, int userId)
        {
            return _configurationDataService.SaveDue(aDue, userId);
        }
        public string GetACompayDueKpi(GridOptions options, Users users)
        {
            string companyId = "";
            var companyList = users.CompanyList;
            foreach (var company in companyList)
            {

                companyId += company.CompanyId;
                companyId += ",";
            }
            companyId = companyId.Remove(companyId.Length - 1);

            var data = _configurationDataService.GetACompayDueKpi(options, companyId);
            var jsonHelper = new JsonHelper();
            return jsonHelper.GetJson(data);
        }

      

    }
}
