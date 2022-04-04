using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Azolution.Entities.Sale
{
    public class MobileInfo
    {
        public int MobileId { get; set; }
     //   public int MobileId2 { get; set; }


       // [Required]
        public string ModelName { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public int Price { get; set; }
        public int Is5G { get; set; }
        public int IsSmart { get; set; }


    }
}
