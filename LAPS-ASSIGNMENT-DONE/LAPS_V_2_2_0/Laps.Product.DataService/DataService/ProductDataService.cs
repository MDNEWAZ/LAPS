using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Product.DataService.DataService
{
    public class ProductDataService
    {
        SqlCommand _aCommand;
        SqlDataAdapter _adapter;
        readonly string _connection = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        private readonly SqlConnection _aConnection;
        public ProductDataService()
        {
            _aConnection = new SqlConnection(_connection);
        }
    

        public string SaveProduct(Azolution.Entities.Sale.Product aProduct, List<ProductItems> productItemList, List<ProductItems> removeItemList)
        {
            CommonConnection connection = new CommonConnection();
            CommonConnection connectionForReturnId = new CommonConnection(IsolationLevel.ReadCommitted);
            try
            {

                string query;
                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");
                var updatedate = DateTime.Now.ToString("dd-MMM-yyyy");


                connection.BeginTransaction();
                connectionForReturnId.BeginTransaction();

                StringBuilder qBuilder = new StringBuilder();

                if (aProduct.ModelId == 0)
                {
                    query =
                        string.Format(@"INSERT INTO Sale_Product([Model],[ProductName],[TypeId],[Description],[Color],[Capacity],[ManufactureDate],[EntryDate],[Updated],[Code],[Flag],[IsActive],CompanyId,TotalPrice,DownPayPercent,PackageType,DefaultInstallmentNo,IsDPFixedAmount,ModelItemID)
                                            VALUES('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}',{9},{10},{11},{12},{13},{14},{15},{16},{17},{18})",
                            aProduct.Model, aProduct.ProductName, aProduct.TypeId, aProduct.Description, aProduct.Color,
                            aProduct.Capacity, "", entrydate, "", 0, aProduct.Flag, aProduct.IsActive, aProduct.CompanyId, aProduct.TotalPrice, aProduct.DownPayPercent, aProduct.PackageType, aProduct.DefaultInstallmentNo, aProduct.IsDPFixedAmount,aProduct.ModelItemID);

                    var modelId = connectionForReturnId.ExecuteAfterReturnId(query, "ModelId");
                    if (modelId > 0)
                    {
                        if (productItemList != null)
                        {
                            foreach (var itemes in productItemList)
                            {
                                var manufacturingDate = DateFormatter.DateForQuery(itemes.ManufacturingDate, connection.DatabaseType);

                                qBuilder.Append(
                                    string.Format(@" Insert Into Sale_Product_Items([ModelId],[ItemName],[ItemModel],[ManufacturingDate],[BundleQuantity],[Price],IsLisenceRequired,IsPriceApplicable,WarrantyPeriod,ItemCode) 
                                                   Values({0},'{1}','{2}',{3},{4},{5},'{6}','{7}',{8},'{9}');", modelId, itemes.ItemName,
                                        itemes.ItemModel, manufacturingDate, itemes.BundleQuantity, itemes.Price, itemes.IsLisenceRequired, itemes.IsPriceApplicable, itemes.WarrantyPeriod, itemes.ItemCodeType.ItemCode));
                            }
                        }
                    }


                    if (qBuilder.ToString() != "")
                    {
                        query = "Begin " + qBuilder.ToString() + " End;";
                        connection.ExecuteNonQuery(query);
                    }

                }
                else
                {
                    query =
                        string.Format(
                            @"UPDATE Sale_Product SET [Model] = '{0}',[ProductName] = '{1}',[TypeId] ={2},[Description] = '{3}'" +
                            ",[Color] = '{4}',[Capacity] = '{5}',[ManufactureDate] = '{6}',[Updated] = '{7}',[Code]={8},[Flag] = {9},[IsActive] = {10},CompanyId={12},TotalPrice={13},DownPayPercent={14},PackageType={15},DefaultInstallmentNo = {16},IsDPFixedAmount = {17},ModelItemID = {18} WHERE ModelId={11} AND Flag=0",
                            aProduct.Model, aProduct.ProductName, aProduct.TypeId, aProduct.Description, aProduct.Color,
                            aProduct.Capacity, "", updatedate, 0, aProduct.Flag, aProduct.IsActive, aProduct.ModelId, aProduct.CompanyId, aProduct.TotalPrice, aProduct.DownPayPercent, aProduct.PackageType, aProduct.DefaultInstallmentNo, aProduct.IsDPFixedAmount,aProduct.ModelItemID);

                    connection.ExecuteNonQuery(query);

                    if (productItemList != null)
                    {

                        foreach (var itemes in productItemList)
                        {
                            var manufacturingDate = DateFormatter.DateForQuery(itemes.ManufacturingDate, connection.DatabaseType);
                            if (itemes.ItemId == 0)
                            {
                                qBuilder.Append(
                                   string.Format(@" Insert Into Sale_Product_Items([ModelId],[ItemName],[ItemModel],[ManufacturingDate],[BundleQuantity],[Price],IsLisenceRequired,IsPriceApplicable,WarrantyPeriod,ItemCode) 
                                                   Values({0},'{1}','{2}',{3},{4},{5},'{6}','{7}',{8},'{9}');", aProduct.ModelId, itemes.ItemName,
                                       itemes.ItemModel, manufacturingDate, itemes.BundleQuantity, itemes.Price, itemes.IsLisenceRequired, itemes.IsPriceApplicable, itemes.WarrantyPeriod, itemes.ItemCodeType.ItemCode));
                            }
                            else
                            {
                                qBuilder.Append(string.Format(@" Update Sale_Product_Items Set ItemName='{0}',ItemModel='{1}',ManufacturingDate={2},BundleQuantity={3},[Price]={4},IsLisenceRequired='{5}',IsPriceApplicable='{7}',WarrantyPeriod={8},ItemCode='{9}' Where ItemId={6};",
                               itemes.ItemName, itemes.ItemModel, manufacturingDate, itemes.BundleQuantity, itemes.Price, itemes.IsLisenceRequired, itemes.ItemId, itemes.IsPriceApplicable, itemes.WarrantyPeriod,itemes.ItemCodeType.ItemCode));
                            }


                        }
                    }
                    if (removeItemList != null)
                    {
                        foreach (var item in removeItemList)
                        {
                            qBuilder.Append(string.Format(@" Delete From Sale_Product_Items Where ItemId={0};", item.ItemId));
                        }

                    }

                    if (qBuilder.ToString() != "")
                    {
                        query = "Begin " + qBuilder.ToString() + " End;";
                        connection.ExecuteNonQuery(query);
                    }


                }
                connection.CommitTransaction();
                connectionForReturnId.CommitTransaction();
                return Operation.Success.ToString();
            }
            catch (Exception ex)
            {

                connection.RollBack();
                connectionForReturnId.RollBack();
                return Operation.Failed.ToString();
            }
            finally
            {
                connection.Close();
                connectionForReturnId.Close();
            }
        }
        public GridEntity<Azolution.Entities.Sale.Product> GetAllProduct(GridOptions options, string condition)
        {
            try
            {
                var query = string.Format(@"Select SP.*, AT.[Type], ISNULL(SS.TotalStock,0) TotalStock, ISNULL(SS.TotalSale,0) TotalSale,ISNULL(SS.CurrentStock,0) CurrentStock from Sale_Product SP
                              LEFT JOIN Sale_AllType AT ON AT.TypeId=SP.TypeId 
                             LEFT JOIN (SELECT SS.ModelId, SUM(SS.Quantity) TotalStock, Max(ISNULL(S.Quantity,0)) TotalSale,(SUM(SS.Quantity)- Max(ISNULL(S.Quantity,0))) CurrentStock FROM Sale_Stock SS 
                              LEFT JOIN (SELECT ModelId, COUNT(*) Quantity FROM Sale S Group BY ModelId) S ON S.ModelId=SS.ModelId 
                              Group BY SS.ModelId) SS ON SS.ModelId=SP.ModelId 
                              WHERE AT.Flag='Product'AND AT.IsActive=1 {0}", condition);
                return Kendo<Azolution.Entities.Sale.Product>.Grid.DataSource(options, query, "Code");
            }
            catch (Exception)
            {
                _aConnection.Close();
                return null;
            }
        }
        public List<AllType> GetAllProductType()
        {
            try
            {
                var query = string.Format(@"SELECT TypeId, Type FROM [Sale_AllType] Where [Flag]='Product' And [IsActive]=1");
                return Kendo<AllType>.Combo.DataSource(query);
            }
            catch (Exception)
            {
                _aConnection.Close();
                return null;
            }
        }
        public GridEntity<License> GetAProductLicense(GridOptions options, int productId)
        {
            try
            {
                var query = string.Format(@"SELECT ROW_NUMBER() OVER (ORDER BY ProductId) AS [Sl],* FROM [Sale_License] WHERE ProductId={0}", productId);
                return Kendo<License>.Grid.DataSource(options, query, "Number");
            }
            catch (Exception)
            {
                _aConnection.Close();
                return null;
            }
        }


        public Azolution.Entities.Sale.Product GetAProduct(string productCode)
        {
            var aProduct = new Azolution.Entities.Sale.Product();
            try
            {
                var sql = string.Format(@"SELECT * FROM [Sale_Product] SP LEFT JOIN Sale_License SL ON Sl.ProductId=SP.ModelId " +
                                   "Where SP.[IsActive]=1 AND Code={0}", productCode);
                return Data<Azolution.Entities.Sale.Product>.GenericDataSource(sql).SingleOrDefault();
            }
            catch (Exception ex)
            {
                aProduct.Code = productCode;
            }
            return aProduct;
        }

        public Sale GetProductCustomerInfoByInvoiceNo(string invoiceNo, int saleId)
        {
            try
            {
                var sql = string.Format(@"Select Sale.*,CONVERT(VARCHAR(11),(DateAdd(mm,WarrantyPeriod,WarrantyStartDate)),106) AS [WarrantyEndDate],SC.ProductId,Sale_AllType.[Type] as ProductTypeName,
                             SC.Name,SC.FatherName,SC.Gender,SC.NID,SC.Address,SC.District,SC.DOB,SC.Phone,SC.CustomerCode,SC.IsStaff,SC.StaffId,SC.Phone2,SC.ReferenceId,
                             Company.CompanyCode,SP.Model,SP.ProductName,SP.ManufactureDate,SP.Code,SP.TypeId,SP.TotalPrice,SP.PackageType,
                             Branch.BranchSmsMobileNumber,Sale_License.LType,Sale_License.Number,Sale_License.IsActive As Status,AT.Type,SP.ModelItemID   
                             From Sale 
                             left join Sale_Customer SC on SC.CustomerId=Sale.CustomerId 
                             left outer join Company on Company.CompanyId=SC.CompanyId
                             left outer join Branch on Branch.BRANCHID=SC.BranchId 
                             left join (Select SP.* From Sale_Product SP 
                             ) SP ON SP.ModelId =Sale.ModelId 
                             left join Sale_License on Sale_License.SaleInvoice = Sale.Invoice And Sale_License.IsActive=1 And Sale_License.IsSentSMS=1
                            left outer join Sale_AllType AT on AT.TypeId = Sale_License.LType And AT.Flag='Lisence'
                             left outer join Sale_AllType on Sale_AllType.TypeId = SP.TypeId and Sale_AllType.Flag = 'Product' and Sale_AllType.IsActive = 1
                            Where Invoice='{0}' AND SaleId={1}", invoiceNo,saleId);
                var data = Data<Sale>.GenericDataSource(sql).SingleOrDefault();

                return data;
            }
            catch (Exception)
            {
                _aConnection.Close();
                return null;
            }

        }

        public Sale GetCustomerInfoByInvoiceNo(string invoiceNo)
        {
            try
            {

                var sql = string.Format(@"Select Sale_Customer.CustomerId,CustomerCode,Phone,Phone2,Sale_Customer.IsUpgraded,Name,Branch.BranchSmsMobileNumber,Branch.IsSmsEligible,Sale_Customer.CompanyId,Sale.Invoice from Sale_Customer 
left outer join Sale on Sale.CustomerId = Sale_Customer.CustomerId
left outer join Branch on Branch.BRANCHID=Sale_Customer.BranchId 

where Sale.Invoice = '{0}'", invoiceNo);
                var data = Data<Sale>.GenericDataSource(sql).SingleOrDefault();

                return data;
            }
            catch (Exception)
            {
                _aConnection.Close();
                return null;
            }

        }

        public GridEntity<Stock> GetAProductStock(GridOptions options, int modelId)
        {
            try
            {
                var query = string.Format("SELECT Top 10 SP.*, PRO.ProductName, PRO.Model, (select SUM(Quantity) Quantity  FROM [Sale_Stock] Where StockId <= SP.StockId AND ModelId={0} AND Status=1 ) 'Total' " +
                                      "FROM [Sale_Stock] SP LEFT JOIN Sale_Product PRO ON PRO.ModelId=SP.ModelId WHERE SP.ModelId={0}", modelId);
                return Kendo<Stock>.Grid.GenericDataSource(options, query, "StockId DESC");

            }
            catch (Exception)
            {
                _aConnection.Close();
                return null;
            }

        }

        public string SaveStock(List<Stock> productItemsStock)
        {
            CommonConnection connection = new CommonConnection();
            try
            {
                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");

                string query = "";
                StringBuilder qBuilder = new StringBuilder();
                if (productItemsStock != null)
                {
                    foreach (var aStock in productItemsStock)
                    {
                        var receiveDate = DateFormatter.DateForQuery(aStock.ReceiveDate, connection.DatabaseType);

                        qBuilder.Append(
                            string.Format(@"INSERT INTO Sale_Stock([ModelId],[Quantity],[ItemId],[ReceiveDate],[EntryDate],[Updated],[Flag],[EntryUserId],[UpdateUserId]) 
                                              VALUES ({0},{1},{2},{3},'{4}','{5}','{6}',{7},'{8}')",
                                aStock.ModelId, aStock.Quantity, aStock.ItemId, receiveDate, entrydate, "", aStock.Flag,
                                aStock.EntryUserId, aStock.UpdateUserId));

                    }

                    if (qBuilder.ToString() != "")
                    {
                        query = "Begin " + qBuilder.ToString() + " End;";
                        connection.ExecuteNonQuery(query);
                    }

                }

                return Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                return Operation.Failed.ToString();
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Azolution.Entities.Sale.Product> GetAllProductModel(int rootCompanyId)
        {
            try
            {
                var query = string.Format("SELECT * FROM [Sale_Product] Where [IsActive]=1 And CompanyId={0}",rootCompanyId);
                var data = Kendo<Azolution.Entities.Sale.Product>.Combo.DataSource(query);
                return data;
            }
            catch (Exception)
            {
                _aConnection.Close();
                return null;
            }

        }

        public Azolution.Entities.Sale.Product GetAProductModel(int modelId)
        {
            try
            {
                var query = string.Format("SELECT * FROM [Sale_Product] Where [IsActive]=1 AND [ModelId]={0}", modelId);
                return Kendo<Azolution.Entities.Sale.Product>.Combo.DataSource(query).SingleOrDefault();
            }
            catch (Exception)
            {
                _aConnection.Close();
                return null;
            }
        }

        public GridEntity<ProductItems> GetAllProductItemByModelId(GridOptions options, int modelId)
        {
            var sql = string.Format(@" Select Sale_Product_Items.*,TotalPrice PackagePrice,ProductItemCode.ItemCodeId  From Sale_Product_Items 
            left outer join Sale_Product on Sale_Product.ModelId=Sale_Product_Items.ModelId
            left outer join ProductItemCode on ProductItemCode.ItemCode =Sale_Product_Items.ItemCode 
            Where Sale_Product_Items.ModelId={0}", modelId);

            var data = Kendo<ProductItems>.Grid.GenericDataSource(null, sql, "ItemId");
            return data;
        }

        public List<ProductItems> GetProductItemByModelId(int modelId)
        {
            string sql = string.Format(@"Select ItemId,ItemName From Sale_Product_Items Where ModelId={0}", modelId);
            var data = Kendo<ProductItems>.Combo.DataSource(sql);
            return data;
        }

        public List<ProductItems> GetStockedProductItemByModelId(int modelId, int branchId, int companyId)
        {
            string condition = "";
            if (branchId > 0)
            {
                condition = " And CompanyId=" + companyId + " And BranchId=" + branchId;
            }
            else
            {
                condition = " And CompanyId=" + companyId;
            }

            string sql = string.Format(@"Select distinct Sale_Stock.ItemId,ItemName From Sale_Product_Items
                    inner join Sale_Stock on Sale_Stock.ItemId= Sale_Product_Items.ItemId
                    Where Sale_Stock.ModelId={0} {1}", modelId,condition);
            var data = Kendo<ProductItems>.Combo.DataSource(sql);
            return data;
        }

        public List<SalesItemDetails> GetItemSlNoBySalesItemId(int salesItemId)
        {
            var query = string.Format(@"Select SalesItemDetails.SalesItemDetailsId,ItemSLNo From SalesItem 
left outer join SalesItemDetails on SalesItemDetails.SalesItemId=SalesItem.SalesItemId
Where SalesItem.SalesItemId={0}", salesItemId);
            return Kendo<SalesItemDetails>.Combo.DataSource(query);
        }

        public List<Azolution.Entities.Sale.Product> GetAllPackageByCompany(string packageCondition, string condition)
        {
            var sql = string.Format(@"Select ModelId,Model,TypeId from Sale_Product
            where IsActive=1 {1} {0}", condition, packageCondition);
            return Kendo<Azolution.Entities.Sale.Product>.Combo.DataSource(sql);
        }

        public List<SalesItemDetails> GetItemsOldSLNo(int salesItemId)
        {
            string sql = string.Format(@"Select ItemSLNo From SalesItem
        left outer join SalesItemDetails on SalesItemDetails.SalesItemId=SalesItem.SalesItemId
        Where SalesItem.SalesItemId={0}
        Union
        Select ReplacedItemSLNo As ItemSLNo From Sale_Replacement Where SalesItemId={0}",salesItemId);

            var data = Data<SalesItemDetails>.GenericDataSource(sql);
            return data;
        }

        public List<ProductItems> GetProductItemsByPackage(string model)
        {
            string sql =
                string.Format(@"Select Sale_Product_Items.*,TotalPrice As PackagePrice, Model From Sale_Product_Items 
                left outer join Sale_Product on Sale_Product.ModelId=Sale_Product_Items.ModelId
                                Where Model= '{0}'",model);
            var data = Data<ProductItems>.GenericDataSource(sql);
            return data;

        }

        public List<ProductItems> GetAllItemsByModelId(int modelId)
        {
            string sql =
                string.Format(@"Select * From Sale_Product_Items
                Where ModelId={0}", modelId);
            var data = Data<ProductItems>.GenericDataSource(sql);
            return data;

        }

        public List<ItemCodeType> GetProductItemCodeData()
        {
            string sql =
                  string.Format(@"Select * From ProductItemCode");
            var data = Data<ItemCodeType>.GenericDataSource(sql);
            return data;
        }

        public string SaveItemCode(string itemCode)
        {
            CommonConnection connection = new CommonConnection();
            string rv = "";
            try
            {
                string code = itemCode.ToUpper();
                string sql = string.Format(@"Insert Into ProductItemCode (ItemCode) Values('{0}')", code);
                connection.ExecuteNonQuery(sql);
                rv = Operation.Success.ToString();
            }
            catch (Exception exception)
            {
                rv = Operation.Failed.ToString();
            }
            finally
            {
                connection.Close();
            }
            return rv;
        }

        public bool CheckExistModel(string condition)
        {
            string sql = string.Format(@"Select * From Sale_Product {0}", condition);
            var data = Data<Azolution.Entities.Sale.Product>.DataSource(sql);
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckExistItemCode(string itemCode)
        {
            string sql = string.Format(@"Select * From ProductItemCode Where ItemCode= '{0}'", itemCode);
            var data = Data<Azolution.Entities.Sale.Product>.DataSource(sql).SingleOrDefault();
            return data != null;
        }

        public List<Azolution.Entities.Sale.Product> GetAllPackageByTypeId(string condition)
        {
            var sql = string.Format(@"Select ModelId,Model,TypeId from Sale_Product
            where IsActive=1 {0}", condition);
            return Kendo<Azolution.Entities.Sale.Product>.Combo.DataSource(sql);
        }

        public List<SalesItem> GetProductItemInfoBySaleId(int saleId)
        {
            var sql = string.Format(@"Select IsLisenceRequired,SalesItem.ItemId,ItemModel,ItemPrice,ItemPrice,ItemQuantity as SalesQty,ItemSLNo,Price,SaleId,
            SalesItem.SalesItemId from SalesItem
            left join SalesItemDetails on SalesItem.SalesItemId = SalesItemDetails.SalesItemId
            inner join Sale_Product_Items on Sale_Product_Items.ItemId = SalesItem.ItemId
            where SaleId = {0}", saleId);
            return Data<SalesItem>.GenericDataSource(sql);
        }
    }
}
