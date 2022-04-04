using MobileService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileService.Service
{
    public class MobileService : IMobileRepository
    {
        private MobileDataService mobileds = new MobileDataService();
        
        public List<string> PopulateColorCombo()
        {
            var list = new List<string>();



            return list;
        }
    }
}
