using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SalesRepresentatorType
    {
        public int SalesRepTypeId { get; set; }
        public string SalesRepTypeName { get; set; }
        public int IsActive { get; set; }
    }
}
