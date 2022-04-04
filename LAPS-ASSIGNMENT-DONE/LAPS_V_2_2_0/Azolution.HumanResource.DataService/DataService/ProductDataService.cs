using Azolution.Entities.HumanResource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Azolution.HumanResource.DataService.DataService
{
    public class ProductDataService
    {
        public string AddProduct(Products product)
        {
            var query = "";
            var con = new CommonConnection(IsolationLevel.ReadCommitted);
            var con2 = new CommonConnection();
            var res = "";
            string sqlProductModel = "";
            StringBuilder qBuilder = new StringBuilder();
            try
            {
                con.BeginTransaction();
                string delQuary =
                            string.Format(
                                @"Delete ProductModel where  ProductId = {0}",product.ProductId);
                con2.ExecuteNonQuery(delQuary);


                if (product.ProductId == 0)
                {
                    query = string.Format(@"INSERT INTO Product(ProductName,ProductCode,ProductTypeId,IsActive) Values('{0}','{1}',{2},{3})", product.ProductName, product.ProductCode, product.ProductTypeId, product.IsActive);

                    var productId = con.ExecuteAfterReturnId(query, "");

                    foreach (var proModel in product.ProductModel)
                    {
                        sqlProductModel +=
                            string.Format(@"insert into ProductModel(ProductModelName,ProductModelPrice,ProductId) Values('{0}',{1},{2});", proModel.ProductModelName, proModel.ProductModelPrice, productId);
                    }
                   
                }
                else
                {
                    query = string.Format(@"Update Product Set ProductName='{0}',ProductCode='{1}',ProductTypeId={2},IsActive={3} where ProductId={4}", product.ProductName, product.ProductCode, product.ProductTypeId, product.IsActive,
                        product.ProductId);

                    con.ExecuteNonQuery(query);

                    foreach (var proModel in product.ProductModel)
                    {
                        sqlProductModel +=
                         string.Format(@"insert into ProductModel(ProductModelName,ProductModelPrice,ProductId) Values('{0}',{1},{2});", proModel.ProductModelName, proModel.ProductModelPrice, product.ProductId);


                       
                    }

                }

                if (sqlProductModel != "")
                {
                    con.ExecuteNonQuery(sqlProductModel);
                }

                con.CommitTransaction();

                res = Operation.Success.ToString();
            }
            catch(Exception exception)
            {
                con.RollBack();
                res = "Error! During Saving Student Information";
            }
            finally
            {
                con.Close();
                con2.Close();
            }
            return res;
        }

        public List<ProductType> GetAllProductType()
        {
            var data = "";
            try
            {
                data = string.Format(@"select ProductType.ProductTypeId,ProductType.ProductTypeName from ProductType");
            }
            catch (Exception)
            {
                throw;
            }
            return Kendo<ProductType>.Combo.DataSource(data);
        }

        public List<Products> GetProductForReport()
        {
            throw new NotImplementedException();
        }

        public GridEntity<Products> GetProductSummery(GridOptions options)
        {
            string data = string
                .Format(@"select ProductType.ProductTypeId,Product.ProductName,Product.ProductCode,Product.IsActive,ProductTypeName,Product.ProductId from Product
			left join ProductType on Product.producttypeid=ProductType.ProductTypeId");
            return Kendo<Products>.Grid.DataSource(options, data, "ProductId");
        }
        public GridEntity<ProductModel> GetProductModelSummary(GridOptions options, int id)
        {
            string query =
                string.Format(@"select ProductModelId,ProductModelName,ProductModelPrice,ProductId from ProductModel where ProductId={0}", id);
            return Kendo<ProductModel>.Grid.DataSource(options, query, "ProductModelId");
        }
    }
}
