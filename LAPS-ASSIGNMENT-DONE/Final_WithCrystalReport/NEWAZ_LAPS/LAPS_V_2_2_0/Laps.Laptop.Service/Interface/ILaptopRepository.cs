using Azolution.Entities.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Laps.Laptop.Service.Interface
{
 public   interface ILaptopRepository
    {
        List<LaptopColors> PopulateColorCombo();
        List<LaptopBrands> PopulateBrandCombo();
        string SaveLaptopInfo(LaptopInfo laptop);
        GridEntity<LaptopInfo> LaptopInfoGrid(GridOptions options);
        string Delete(int LaptopId);
    }
}
