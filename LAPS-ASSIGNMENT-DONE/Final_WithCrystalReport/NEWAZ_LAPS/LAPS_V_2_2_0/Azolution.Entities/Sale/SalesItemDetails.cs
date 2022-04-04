using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class SalesItemDetails
    {
        public SalesItemDetails(){}
     

        public int SalesItemDetailsId { get; set; }
        public int ItemId { get; set; }
        public string ItemSLNo { get; set; }
        public string ItemManufactureDate { get; set; }
        public int ItemWarrantyPeriod { get; set; }

        public string ItemCode { get; set; }
        
    }
}
