using Azolution.Entities.Sale;
using Laps.Laptop.Datase;
using Laps.Laptop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Laps.Laptop.Service.Service
{
    public class LaptopService : ILaptopRepository
    {
        private LaptopDataService laptopDataService = new LaptopDataService();

        public List<LaptopColors> PopulateColorCombo()
        {
            var data = laptopDataService.PopulateColorCombo();
            return data;
        }
        public List<LaptopBrands> PopulateBrandCombo()
        {
            var datas = laptopDataService.PopulateBrandCombo();
            return datas;
        }

        public string SaveLaptopInfo(LaptopInfo laptop)
        {
            return laptopDataService.SaveLaptopInfo(laptop);
        }
        public GridEntity<LaptopInfo> LaptopnfoGrid(GridOptions options)
        {
            return laptopDataService.LaptopInfoGrid(options);
        }

        public string Delete(int LaptopId)
        {
            return laptopDataService.Delete(LaptopId);
        }
    }
}
