using Azolution.Entities.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Laps.Mobile.Service.Interface
{
    public interface IMobileRepository
    {
        List<MobileColor> PopulateColorCombo();
        List<MobileBrand> PopulateBrandCombo();
        List<MobileColor> PopulateColorCombo2();
        List<MobileBrand> PopulateBrandCombo2();
        string SaveMobileInfo(MobileInfo mobile);
        GridEntity<MobileInfo> MobileInfoGrid(GridOptions options);

        //ChkPopup
        string SaveFeature(MobileFeature feature);
        GridEntity<MobileFeature> MobileFeatureGrid(GridOptions options);


       string Delete(int MobileId);

       
    }
}
