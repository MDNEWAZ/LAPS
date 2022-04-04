using Azolution.Entities.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Azolution.HumanResource.Service.Interface
{
    public interface IProductRepository
    {
        List<ProductType> GetAllProductType();
        string AddProduct(Products product);
        GridEntity<Products> GetProductSummery(GridOptions options);
        GridEntity<ProductModel> GetProductModelSummary(GridOptions options, int id);
        List<Products> GetProductForReport();
    }
}
