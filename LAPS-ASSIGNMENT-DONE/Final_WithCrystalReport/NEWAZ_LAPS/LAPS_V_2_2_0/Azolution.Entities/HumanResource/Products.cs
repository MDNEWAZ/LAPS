using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.HumanResource
{
    public class Products
    {
        public Products()
        {

        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductModelId { get; set; }   
        public int ProductTypeId { get; set; }
        public string ProductModelName { get; set; }
        public double ProductModelPrice { get; set; }
        public int IsActive { get; set; }
        public string ProductTypeName { get; set; }
        public List<ProductModel> ProductModel { get; set; }

        public List<ProductModel> RemoveProductModel { get; set; }
    }
}
