using Azolution.Entities.Sale;
using Laps.Mobile.DataService;
using Laps.Mobile.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Laps.Mobile.Service.Service
{
    public class MobileService : IMobileRepository
    {
        private MobileDataService mobileDataService = new MobileDataService();
        public List<MobileColor> PopulateColorCombo()
        {
            var data = mobileDataService.PopulateColorCombo();
            return data;
        }
        public List<MobileBrand> PopulateBrandCombo()
        {
            var datas = mobileDataService.PopulateBrandCombo();
            return datas;
        }

        public List<MobileColor> PopulateColorCombo2()
        {
            var data = mobileDataService.PopulateColorCombo2();
            return data;
        }
        public List<MobileBrand> PopulateBrandCombo2()
        {
            var datas = mobileDataService.PopulateBrandCombo2();
            return datas;
        }

        public string SaveMobileInfo(MobileInfo mobile)
        {
            return mobileDataService.SaveMobileInfo(mobile);
        }
        public GridEntity<MobileInfo> MobileInfoGrid(GridOptions options)
        {
            return mobileDataService.MobileInfoGrid(options);
        }
       
        public string Delete(int MobileId)
        {
            return mobileDataService.Delete(MobileId);
        }


        //ChkPoPup
        public string SaveFeature(MobileFeature feature)
        {
            return mobileDataService.SaveFeature(feature);
        }
        public GridEntity<MobileFeature> MobileFeatureGrid(GridOptions options)
        {
            return mobileDataService.MobileFeatureGrid(options);
        }
        public List<MobileInfo> GetMobileInfoReport()
        {
            return mobileDataService.GetMobileInfoReport();

        }

    }
}
