using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataService;
using Azolution.HumanResource.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Azolution.HumanResource.Service.Service
{
    public class ProductService : IProductRepository
    {
        ProductDataService _productdataservice = new ProductDataService();
        public string AddProduct(Products product)
        {
            return _productdataservice.AddProduct(product);
        }

        public GridEntity<ProductModel> GetProductModelSummary(GridOptions options, int id)
        {
            return _productdataservice.GetProductModelSummary(options, id);
        }

        public List<ProductType> GetAllProductType()
        {
            return _productdataservice.GetAllProductType();
        }

        public List<Products> GetProductForReport()
        {
            throw new NotImplementedException();
        }

        public GridEntity<Products> GetProductSummery(GridOptions options)
        {
            return _productdataservice.GetProductSummery(options);
        }
    }
}
