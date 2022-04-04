using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.HumanResource
{
    [Serializable]
    class NewCustomer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }   
    }
}
