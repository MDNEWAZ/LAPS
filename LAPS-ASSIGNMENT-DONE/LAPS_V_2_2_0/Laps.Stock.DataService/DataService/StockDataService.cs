using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using MySql.Data.MySqlClient;
using Utilities;

namespace Laps.Stock.DataService.DataService
{
    public class StockDataService
    {
        // Type Id 
        // 1= Purchase or Stock
        // 2 = Addition / update
        // 3 = Deduction / damage
        // 4 = Deduction / sale
        // 5 = Deduction / Replacement
        // 6=  add/Sales update

        public GridEntity<Azolution.Entities.Sale.Stock> GetAProductStock(GridOptions options, int modelId, int branchId, Users users, int stockCategoryId, int companyId)
        {
            try
            {
                string condition = "";
                var query = "";

                if (branchId > 0)
                {
                    condition = " And CompanyId=" + companyId + " And BranchId=" + branchId;
                }
                else
                {
                    condition = " And CompanyId=" + companyId;
                }

                if (stockCategoryId == 1)
                {
                    query = string.Format(@"  Select sb.EntryDate,si.ItemName,sb.StockBalanceQty Quantity, sb.ItemId 
                         From StockBalance Sb 
                        inner join Sale_Product_Items si on si.ItemId=Sb.ItemId
                        inner join 
                        ( Select Max(EntryDate) as EntryDate,sb.ItemId 
                         From StockBalance Sb
                        where Sb.ModelId={0} {1} 
                        group by sb.ItemId) tblTemp on tblTemp.EntryDate = sb.EntryDate and tblTemp.ItemId = sb.ItemId
                        where Sb.ModelId={0} {1} ", modelId, condition);
                }
                else if (stockCategoryId == 2)
                {
                    query = string.Format(@"  Select sb.EntryDate,si.ItemName,sb.StockBalanceQty Quantity, sb.ItemId 
                         From StockBalance_Replacement Sb 
                        inner join Sale_Product_Items si on si.ItemId=Sb.ItemId
                        inner join 
                        ( Select Max(EntryDate) as EntryDate,sb.ItemId 
                         From StockBalance_Replacement Sb
                        where Sb.ModelId={0}  {1} 
                        group by sb.ItemId) tblTemp on tblTemp.EntryDate = sb.EntryDate and tblTemp.ItemId = sb.ItemId
                        where Sb.ModelId={0} {1}", modelId, condition);
                }

                return Kendo<Azolution.Entities.Sale.Stock>.Grid.GenericDataSource(options, query, "ItemId");

            }
            catch (Exception)
            {
                return null;
            }
        }

        public GridEntity<Azolution.Entities.Sale.Stock> GetAllStockItemsByItemId(GridOptions options, int itemId)
        {
            var query = string.Format(@"Select Sale_Stock.*,ItemName From Sale_Stock
            Left outer join Sale_Product_Items si on si.ItemId = Sale_Stock.ItemId where Sale_Stock.ItemId={0}", itemId);
            return Kendo<Azolution.Entities.Sale.Stock>.Grid.GenericDataSource(options, query, "ReceiveDate Desc");
        }

        public string SaveStock(List<Azolution.Entities.Sale.Stock> objStockItemList, Users users, int companyId, int branchId)
        {

            CommonConnection connection = new CommonConnection();
            connection.BeginTransaction();
            try
            {
                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");

                string query = "";
                StringBuilder qBuilder = new StringBuilder();
                if (objStockItemList != null)
                {
                    foreach (var aStock in objStockItemList)
                    {
                        // branchId = aStock.BranchId;
                        var receiveDate = DateFormatter.DateForQuery(aStock.ReceiveDate, connection.DatabaseType);
                        if (aStock.StockId == 0)
                        {
                            qBuilder.Append(
                          string.Format(@"INSERT INTO Sale_Stock([ModelId],[Quantity],[ItemId],[ReceiveDate],[EntryDate],[Updated],[Flag],[EntryUserId],[UpdateUserId],CompanyId,BranchId,DeliveryChalanNo,DeliveryOrderNo,DeliveryDate,QBInvoiceNo,StockCategoryId) 
                                              VALUES ({0},{1},{2},{3},'{4}','{5}','{6}',{7},'{8}',{9},{10},'{11}','{12}','{13}','{14}',{15});",
                              aStock.ModelId, aStock.Quantity, aStock.ItemId, receiveDate, entrydate, "", aStock.Flag,
                              users.UserId, aStock.UpdateUserId, companyId, branchId, aStock.DeliveryChalanNo, aStock.DeliveryOrderNo, aStock.DeliveryDate, aStock.QBInvoiceNo, aStock.StockCategoryId));

                            var stockbalance = new StockBalance();
                            stockbalance = CheckExistStock(aStock.ModelId, aStock.ItemId, companyId, branchId, aStock.StockCategoryId);

                            if (stockbalance != null)
                            {
                                var newStockbalance = new StockBalance();
                                newStockbalance.ModelId = aStock.ModelId;
                                newStockbalance.ItemId = aStock.ItemId;
                                newStockbalance.StockQuantity = stockbalance.StockBalanceQty;
                                newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty + aStock.Quantity);
                                newStockbalance.EntryUserId = users.UserId;
                                newStockbalance.Type = 1; //1 for purchage/stock

                                SaveStockBalance(qBuilder, newStockbalance.ModelId, newStockbalance.ItemId, newStockbalance.StockQuantity, newStockbalance.StockBalanceQty, newStockbalance.EntryUserId, companyId, branchId, stockbalance.Type, aStock.StockCategoryId);
                            }
                            else
                            {
                                SaveStockBalance(qBuilder, aStock.ModelId, aStock.ItemId, aStock.Quantity, aStock.Quantity, users.UserId, companyId, branchId, 1, aStock.StockCategoryId);
                            }

                        }


                    }

                    if (qBuilder.ToString() != "")
                    {
                        query = "Begin " + qBuilder.ToString() + " End;";
                        connection.ExecuteNonQuery(query);
                        connection.CommitTransaction();
                    }

                }

                return Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                connection.RollBack();
                return Operation.Failed.ToString();
            }
            finally
            {
                connection.Close();
            }
        }

        public string SaveStockAdjustment(List<StockAdjustment> objStockAdjList, Users users, int stockCategoryId, int companyId, int branchId)
        {

            CommonConnection connection = new CommonConnection();
            connection.BeginTransaction();
            try
            {
                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");

                string query = "";
                StringBuilder qBuilder = new StringBuilder();
                if (objStockAdjList != null)
                {
                    foreach (var stockAdj in objStockAdjList)
                    {
                        if (stockAdj.StockAdjustmentId == 0)
                        {
                            qBuilder.Append(
                          string.Format(@"INSERT INTO StockAdjustment([ModelId],[ItemId],[AdjustmentQuantity],[AdjustmentDate],[Description],[AdjustmentBy],CompanyId,BranchId)
                                         Values({0},{1},{2},'{3}','{4}',{5},{6},{7});",
                              stockAdj.ModelId, stockAdj.ItemId, stockAdj.AdjustmentQuantity, entrydate, stockAdj.Description, users.UserId, companyId, branchId));

                            var stockbalance = new StockBalance();

                            stockbalance = CheckExistStock(stockAdj.ModelId, stockAdj.ItemId, companyId, branchId, stockCategoryId);

                            if (stockbalance != null)
                            {
                                var newStockbalance = new StockBalance();
                                newStockbalance.ModelId = stockAdj.ModelId;
                                newStockbalance.ItemId = stockAdj.ItemId;
                                newStockbalance.StockQuantity = stockbalance.StockBalanceQty;
                                if (stockAdj.AdjustmentTypeId == 1) // AdjustmentTypeId 1 for addition , 2 for deduction
                                {
                                    newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty + stockAdj.AdjustmentQuantity);
                                    newStockbalance.Type = 2; // TypeId 2 for addition/update
                                }
                                else
                                {
                                    newStockbalance.StockBalanceQty = (stockbalance.StockBalanceQty - stockAdj.AdjustmentQuantity);
                                    newStockbalance.Type = 3; // TypeId 3 for deduction/damage
                                }
                                newStockbalance.EntryDate = DateTime.Now;
                                newStockbalance.EntryUserId = users.UserId;

                                SaveStockBalance(qBuilder, newStockbalance.ModelId, newStockbalance.ItemId, newStockbalance.StockQuantity, newStockbalance.StockBalanceQty, newStockbalance.EntryUserId, companyId, branchId, stockbalance.Type, stockCategoryId);
                            }
                            else
                            {
                                return "You have no any Stock !";
                            }
                        }
                    }

                    if (qBuilder.ToString() != "")
                    {
                        query = "Begin " + qBuilder.ToString() + " End;";
                        connection.ExecuteNonQuery(query);
                        connection.CommitTransaction();
                    }

                }

                return Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                connection.RollBack();
                return Operation.Failed.ToString();
            }
            finally
            {
                connection.Close();
            }
        }

        public void SaveStockBalance(StringBuilder qBuilder, int modelId, int itemId, int stockQuantity, int stockBalanceQty, int entryUserId, int companyId, int branchId, int type, int stockCategoryId)
        {
            if (stockCategoryId == 1)//For Sale
            {
                qBuilder.Append(
                                   string.Format(@" Insert Into StockBalance(ModelId,ItemId,StockQuantity,StockBalanceQty,EntryDate,[Type],EntryUserId,CompanyId,BranchId) 
                                                    Values({0},{1},{2},{3},'{4}',{5},{6},{7},{8});",
                                                modelId, itemId, stockQuantity, stockBalanceQty, DateTime.Now, type, entryUserId, companyId, branchId));
            }
            else if (stockCategoryId == 2)//For Replacement
            {
                qBuilder.Append(
                                   string.Format(@" Insert Into StockBalance_Replacement(ModelId,ItemId,StockQuantity,StockBalanceQty,EntryDate,[Type],EntryUserId,CompanyId,BranchId) 
                                                    Values({0},{1},{2},{3},'{4}',{5},{6},{7},{8});",
                                                modelId, itemId, stockQuantity, stockBalanceQty, DateTime.Now, type, entryUserId, companyId, branchId));
            }

        }

        public StockBalance CheckExistStock(int modelId, int itemId, int companyId, int branchId, int stockCategoryId)
        {
            string condition = "";
            string sql = "";
            if (branchId != 0)
            {
                condition = " And BranchId = " + branchId;
            }
            if (stockCategoryId == 1)
            {
                sql = string.Format(@"Select * From StockBalance 
                Where ModelId={0} And ItemId={1} and EntryDate=(Select MAX(EntryDate) From StockBalance Where ModelId={0} And ItemId={1} and CompanyId = {2} {3})  and CompanyId = {2} {3}", modelId, itemId, companyId, condition);

            }
            else if (stockCategoryId == 2)
            {
                sql = string.Format(@"Select * From StockBalance_Replacement 
                Where ModelId={0} And ItemId={1} and EntryDate=(Select MAX(EntryDate) From StockBalance_Replacement Where ModelId={0} And ItemId={1} and CompanyId = {2} {3})  and CompanyId = {2} {3}", modelId, itemId, companyId, condition);
            }

            return Data<StockBalance>.DataSource(sql).SingleOrDefault();

        }

        public bool CheckStock(int modelId)
        {
            string sql = string.Format(@"Select * From Sale_Stock Where ModelId={0}", modelId);
            var data = Data<Azolution.Entities.Sale.Stock>.DataSource(sql);
            if (data != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public StockBalance checkExistStockBalanceByItemId(int itemId, int companyId, int branchId, int stockCategoryId)
        {
            string condition = "";
            var sql = "";

            if (branchId > 0)
            {
                condition = " And CompanyId=" + companyId + " And BranchId=" + branchId;
            }
            else
            {
                condition = " And CompanyId=" + companyId;
            }


            if (stockCategoryId == 1)
            {
                sql = string.Format(@"Select * From StockBalance where ItemId={0} and
                    EntryDate=(Select MAX(EntryDate)From StockBalance Where ItemId={0} {1}) {1}",
                    itemId, condition);
            }
            else if (stockCategoryId == 2)
            {
                sql = string.Format(@"Select * From StockBalance_Replacement where ItemId={0} and
                    EntryDate=(Select MAX(EntryDate)From StockBalance_Replacement Where ItemId={0} {1}){1}",
                    itemId, condition);
            }

            var data = Data<StockBalance>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public bool CheckExistStockByModelId(int modelId, Users objUser)
        {
            var companyId = 0;
            var branchId = 0;
            if (objUser.CompanyStock == 1)
            {
                //companyId = objUser.ChangedCompanyId == 0 ? objUser.CompanyId : objUser.ChangedCompanyId;
                //branchId = 0;
                if (objUser.CompanyType == "MotherCompany")
                {
                    companyId = objUser.CompanyId;
                }
                else
                {
                    companyId = objUser.RootCompanyId;
                }
                //companyId = objUser.RootCompanyId;
            }
            else
            {
                if (objUser.CompanyType == "MotherCompany")
                {
                    companyId = objUser.CompanyId;
                }
                else
                {
                    companyId = objUser.RootCompanyId;
                }
                //companyId = objUser.ChangedCompanyId == 0 ? objUser.CompanyId : objUser.ChangedCompanyId;
                branchId = objUser.ChangedBranchId == 0 ? objUser.BranchId : objUser.ChangedBranchId;


                //companyId = objUser.RootCompanyId == 0 ? objUser.CompanyId : objUser.RootCompanyId;//Worked
            }
            string sql = string.Format(@"Select tbl.*,StockBalance.StockBalanceQty From StockBalance
                inner join (Select Max(StockBalanceId)StockBalanceId,ItemId,MAX(EntryDate) EntryDate from StockBalance Where ModelId={0} And CompanyId={1} And BranchId={2} group by ItemId)tbl
                on tbl.StockBalanceId = StockBalance.StockBalanceId", modelId, companyId, branchId);
            var stockdata = Data<StockBalance>.DataSource(sql);
            var bundleItem = GetProductItemBundleQtyByModelId(modelId);

            bool has = false;
            foreach (var bndlItems in bundleItem)
            {
                var exst = stockdata.Where(s => s.ItemId == bndlItems.ItemId && bndlItems.BundleQuantity <= s.StockBalanceQty);
                has = exst.Any() ? true : false;
                if (has == false)
                {
                    return false;
                }
            }
            return true;

            //bool any = true;
            //if (stockdata.Count != 0)
            //{
            //    foreach (StockBalance stockitem in stockdata)
            //        foreach (ProductItems bundleitem in bundleItem)
            //        {
            //            if (stockitem.ItemId == bundleitem.ItemId)
            //            {
            //                if (stockitem.StockBalanceQty < bundleitem.BundleQuantity)
            //                {
            //                    any = false;
            //                    break;
            //                }
            //            }
            //        }

            //}
            //else
            //{
            //    any = false;
            //}
            //return any;
        }

        public List<ProductItems> GetProductItemBundleQtyByModelId(int modelId)
        {
            string sql = string.Format(@"Select * From Sale_Product_Items Where ModelId={0} ", modelId);
            var prodItemBundle = Data<ProductItems>.DataSource(sql);
            return prodItemBundle;
        }

        public GridEntity<ProductItems> GetAllStockModelId(GridOptions options, int modelId)
        {
            string sql = string.Format(@"Select * from Sale_Product_Items where ModelId  ={0}", modelId);
            return Kendo<ProductItems>.Grid.DataSource(options, sql, "ItemId");
        }

        public List<MonthWiseStockGraph> GetSaleStockData(Users objUser, int companyId)
        {
            string query = "";
            if (companyId > 0)
            {
                query = GetCompanyWiseSaleStockQuery(companyId);
            }
            else
            {
                query = GetItemWiseCurrentSaleStockQuery();
            }


            return Data<MonthWiseStockGraph>.DataSource(query);
        }

        private static string GetItemWiseCurrentSaleStockQuery()
        {
            string query = "";
            query = string.Format(@"
                Select Sale_Product.Model +' ('+ Sale_Product_Items.ItemName+')' ModelItem, Sale_Product.ModelId,Sale_Product_Items.ItemId,isnull(tblStock.StockBalanceQty,0) StockBalanceQty
              From Sale_Product
              inner join Sale_Product_Items on Sale_Product_Items.ModelId=Sale_Product.ModelId and Sale_Product.IsActive=1
              left join (
              Select  tblF.ModelId,tblF.ItemId,tblF.StockBalanceQty 
              From (
               Select ModelId,ItemId, SUM(StockBalanceQty)StockBalanceQty
               --,CompanyId --conditional Company
               From (
 
               Select SB.ModelId,SB.ItemId,SB.StockBalanceQty,SB.EntryDate,SB.CompanyId
               From StockBalance SB 
               Inner join 
               (Select MAX(EntryDate) EntryDate,ItemId From StockBalance
               group by ItemId
               ,CompanyId )tbl 
               on tbl.EntryDate=SB.EntryDate And tbl.ItemId=SB.ItemId
 
               )tblStockSale
               group by ModelId,ItemId
               --,CompanyId --conditional company
 
               )tblF )tblStock on tblStock.ModelId=Sale_Product.ModelId And tblStock.ItemId=Sale_Product_Items.ItemId ");
            return query;
        }

        private string GetCompanyWiseSaleStockQuery(int companyId)
        {

            string sql = string.Format(@"
            Select Sale_Product.Model +' ('+ Sale_Product_Items.ItemName+')' ModelItem, 
            Sale_Product.ModelId,Sale_Product_Items.ItemId,isnull(tblStock.StockBalanceQty,0) StockBalanceQty,
            tblStock.CompanyId
            From Sale_Product 
            inner join Sale_Product_Items on Sale_Product_Items.ModelId=Sale_Product.ModelId and Sale_Product.IsActive=1
            left join (
             Select  tblF.ModelId,tblF.ItemId,tblF.StockBalanceQty,tblF.CompanyId 
             From (
             Select ModelId,ItemId, SUM(StockBalanceQty)StockBalanceQty
             ,CompanyId --conditional Company
             From (
             Select SB.ModelId,SB.ItemId,SB.StockBalanceQty,SB.EntryDate,SB.CompanyId
             From StockBalance SB 
             Inner join 
             (Select MAX(EntryDate) EntryDate,ItemId From StockBalance
              group by ItemId
              ,CompanyId,BranchId )tbl 
             on tbl.EntryDate=SB.EntryDate And tbl.ItemId=SB.ItemId
             )tblStockSale
             group by ModelId,ItemId ,CompanyId --conditional company
             )tblF 
             )tblStock 
             on tblStock.ModelId=Sale_Product.ModelId And tblStock.ItemId=Sale_Product_Items.ItemId
    
                    Where tblStock.CompanyId={0}", companyId);
            return sql;
        }

        private string GetCompanyWiseReplacementStockQuery(int companyId)
        {

            string sql = string.Format(@"
            Select Sale_Product.Model +' ('+ Sale_Product_Items.ItemName+')' ModelItem, 
            Sale_Product.ModelId,Sale_Product_Items.ItemId,isnull(tblStock.StockBalanceQty,0) StockBalanceQty,
            tblStock.CompanyId
            From Sale_Product 
            inner join Sale_Product_Items on Sale_Product_Items.ModelId=Sale_Product.ModelId and Sale_Product.IsActive=1
            left join (
             Select  tblF.ModelId,tblF.ItemId,tblF.StockBalanceQty,tblF.CompanyId 
             From (
             Select ModelId,ItemId, SUM(StockBalanceQty)StockBalanceQty
             ,CompanyId --conditional Company
             From (
             Select SB.ModelId,SB.ItemId,SB.StockBalanceQty,SB.EntryDate,SB.CompanyId
             From StockBalance_Replacement SB 
             Inner join 
             (Select MAX(EntryDate) EntryDate,ItemId From StockBalance_Replacement
              group by ItemId
              ,CompanyId,BranchId )tbl 
             on tbl.EntryDate=SB.EntryDate And tbl.ItemId=SB.ItemId
             )tblStockSale
             group by ModelId,ItemId ,CompanyId --conditional company
             )tblF 
             )tblStock 
             on tblStock.ModelId=Sale_Product.ModelId And tblStock.ItemId=Sale_Product_Items.ItemId
    
                    Where tblStock.CompanyId={0}", companyId);
            return sql;
        }

        public List<MonthWiseStockGraph> GetReplacementStockData(Users objUser, int companyId)
        {
            string query = "";
            if (companyId > 0)
            {
                query = GetCompanyWiseReplacementStockQuery(companyId);
            }
            else
            {
                query = GetItemWiseCurrentReplacementStockQuery();
            }
            return Data<MonthWiseStockGraph>.DataSource(query);
        }

        private static string GetItemWiseCurrentReplacementStockQuery()
        {
            string query = "";
            query = string.Format(@"
           Select Sale_Product.Model +' ('+ Sale_Product_Items.ItemName+')' ModelItem, Sale_Product.ModelId,Sale_Product_Items.ItemId,isnull(tblStock.StockBalanceQty,0) StockBalanceQty
                From Sale_Product
                inner join Sale_Product_Items on Sale_Product_Items.ModelId=Sale_Product.ModelId and Sale_Product.IsActive=1
                left join (
                 Select  tblF.ModelId,tblF.ItemId,tblF.StockBalanceQty 
                 From (
                 Select ModelId,ItemId, SUM(StockBalanceQty)StockBalanceQty
                 --,CompanyId --conditional Company
                 From (
 
                 Select SB.ModelId,SB.ItemId,SB.StockBalanceQty,SB.EntryDate,SB.CompanyId
                 From StockBalance_Replacement SB 
                 Inner join 
                 (Select MAX(EntryDate) EntryDate,ItemId From StockBalance_Replacement
                  group by ItemId
                  ,CompanyId )tbl 
                 on tbl.EntryDate=SB.EntryDate And tbl.ItemId=SB.ItemId
 
                 )tblStockSale
                 group by ModelId,ItemId
                 --,CompanyId --conditional company
 
                 )tblF 
                 )tblStock on tblStock.ModelId=Sale_Product.ModelId And tblStock.ItemId=Sale_Product_Items.ItemId ");
            return query;
        }


        //New Stock Inventory Reduce for Dealer--------- Dealer Sale
        public string ReduceStockInventoryForDealerbyDealerIdAndModelId(List<Azolution.Entities.Sale.Sale> objSalesObjList,Users saleUser, CommonConnection connection)
        {
            var res = "";
            try
            {
                var query = "";

                var queryBulder = new StringBuilder();
                foreach (var sale in objSalesObjList)
                {
                    queryBulder.Append(
                 string.Format(
                     @"Update stock_dealer set StockDealerQty = StockDealerQty - 1,StockDealerUpdate = '{2}',StockDealerStatus = 0,StockDealerLastQty = 1 where DealerID = {0} and ItemID = {1};",
                     Convert.ToInt32(sale.SalesRepId.Substring(2, sale.SalesRepId.Length - 2)), sale.AProduct.ModelItemID, DateTime.Now));
                }

                if (queryBulder.ToString() != "")
                {
                    query = "Begin\r\n " + queryBulder.ToString() + " \r\nEnd;";

                }
                connection.ExecuteNonQuery(query);
                res = Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                return Operation.Failed.ToString();
            }
            return res;

        }

        public string ReduceStockInventoryForDealerbyDealerIdAndModelIdFromRemoteMySql(List<Azolution.Entities.Sale.Sale> objSalesObjList, Users saleUser)
        {
            var res = "";
            var mySQlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString);
            try
            {
                var query = "";
                mySQlConn.Open();

                foreach (var sale in objSalesObjList)
                {
                    query =
                        string.Format(
                            @"Update stock_dealer set StockDealerQty = StockDealerQty - 1,StockDealerUpdate = '{2}',StockDealerStatus = 0,StockDealerLastQty = 1 where DealerID = {0} and ItemID = {1};",
                            Convert.ToInt32(sale.SalesRepId.Substring(2, sale.SalesRepId.Length - 2)),
                            sale.AProduct.ModelItemID, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));


                    if (query != "")
                    {
                        var cmd = new MySqlCommand(query, mySQlConn);
                        cmd.ExecuteNonQuery();
                    }
                }

                res = Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                mySQlConn.Close();
            }

            return res;
        }


        //New Stock Inventory Reduce for Branch---------  Retail Sale
        public string ReduceStockInventoryForBranchbyBranchIdAndModelId(List<Azolution.Entities.Sale.Sale> objSalesObjList,Users saleUser, CommonConnection connection)
        {
            var res = "";
            try
            {
                var query = "";
                var branchCode = saleUser.ChangedBranchCode == "" ? saleUser.BranchCode : saleUser.ChangedBranchCode;

                var queryBulder = new StringBuilder();
                foreach (var sale in objSalesObjList)
                {
                    queryBulder.Append(
                string.Format(
                    @"Update stock_branch set StockBranchQty = StockBranchQty - 1, StockBranchUpdate = '{2}',StockBranchStatus = 0, StockBranchLastQty = 1 where BranchID = {0} and ItemID = {1};",
                   Convert.ToInt32(branchCode.Substring(2, branchCode.Length - 2)), sale.AProduct.ModelItemID,DateTime.Now));
                }


                if (queryBulder.ToString() != "")
                {
                    query = "Begin\r\n " + queryBulder.ToString() + " \r\nEnd;";

                }

                connection.ExecuteNonQuery(query);
                return Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }

        public string ReduceStockInventoryForBranchbyBranchIdAndModelIdFromRemoteMySql(List<Azolution.Entities.Sale.Sale> objSalesObjList, Users saleUser)
        {
            var res = "";

            var mySQlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString);
            try
            {
                var query = "";
                var branchCode = saleUser.ChangedBranchCode == "" ? saleUser.BranchCode : saleUser.ChangedBranchCode;
                mySQlConn.Open();

                foreach (var sale in objSalesObjList)
                {
                    query =
                        string.Format(
                            @"Update stock_branch set StockBranchQty = StockBranchQty - 1,StockBranchUpdate = '{2}',StockBranchStatus = 0, StockBranchLastQty = 1 where BranchID = {0} and ItemID = {1};",
                            Convert.ToInt32(branchCode.Substring(2, branchCode.Length - 2)),
                            sale.AProduct.ModelItemID,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                    if (query != "")
                    {
                        var cmd = new MySqlCommand(query, mySQlConn);
                        cmd.ExecuteNonQuery();

                    }
                }
                res = Operation.Success.ToString();

            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                mySQlConn.Close();
            }

            return res;
        }

        public bool CheckStockInventoryDealerByDealerIdAndModelId(string salesRepId, object modelItemId)  //D-type Inventory
        {
            string sql = string.Format(@"Select * From stock_dealer where StockDealerQty>0 and DealerID = {0} and ItemID = {1}", Convert.ToInt32(salesRepId), modelItemId);
            var data = Data<StockInventoryDealer>.DataSource(sql);
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckStockInventoryBranchByBranchIdAndModelId(int branchId, object modelItemId)   //R-Type Inventory
        {
            string sql = string.Format(@"Select * From stock_branch where StockBranchQty>0 and BranchID = {0} and ItemID = {1}", branchId, modelItemId);
            var data = Data<StockInventoryBranch>.DataSource(sql);
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
