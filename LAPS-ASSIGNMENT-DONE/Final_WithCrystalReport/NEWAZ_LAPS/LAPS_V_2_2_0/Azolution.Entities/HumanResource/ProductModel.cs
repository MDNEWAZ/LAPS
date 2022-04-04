using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.HumanResource
{
    public class ProductModel
    {
        public ProductModel()
        {

        }
        public int ProductModelId { get; set; }
        public string ProductModelName { get; set; }
        public double ProductModelPrice { get; set; }
        public int ProductId { get; set; }

    }
}
