using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileService.Interface
{
    public interface IMobileRepository
    {
        public List<string> PopulateColorCombo();
    }
}
