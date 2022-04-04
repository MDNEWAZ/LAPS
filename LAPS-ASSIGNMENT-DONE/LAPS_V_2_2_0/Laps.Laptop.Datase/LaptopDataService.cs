using Azolution.Entities.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Laps.Laptop.Datase
{
   public class LaptopDataService
    {
        public List<LaptopColors> PopulateColorCombo()
        {
            CommonConnection conn = new CommonConnection();

            try
            {
                conn.BeginTransaction();
                string query = "SELECT * FROM LaptopColors";

                return Kendo<LaptopColors>.Combo.DataSource(query);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        public List<LaptopBrands> PopulateBrandCombo()
        {
            CommonConnection conn = new CommonConnection();
            try
            {
                conn.BeginTransaction();
                string query = "SELECT * FROM  LaptopBrands";
                return Kendo<LaptopBrands>.Combo.DataSource(query);
            }
            catch (Exception)
            {
                return null;
            }

            finally
            {
                conn.Close();
            }
        }



        public string SaveMobileInfo(LaptopInfo laptop)
        {
            string resultMSG = "";
            CommonConnection conn = new CommonConnection();
            try
            {
                conn.BeginTransaction();

                if (laptop.LaptopId != 0)
                {
                    string updateQuery = string.Format(@"UPDATE [dbo].[LaptopInfo]
                                   SET [ModelName] = '{0}'
                                      ,[BrandId] = {1}
                                      ,[ColorId] = {2}
                                      ,[price] = {3}
                                      ,[IsSsd] = {4}
                                      ,[Used] = {5}
                                 WHERE LaptopId = {6}",
                             laptop.ModelName, laptop.BrandId, laptop.ColorId, laptop.price, laptop.IsSsd, laptop.Used, laptop.LaptopId);
                    conn.ExecuteNonQuery(updateQuery);
                }
                else
                {
                    string saveQuery = string.Format(@"INSERT INTO [dbo].[LaptopInfo]
                                                   ([ModelName]
                                                   ,[BrandId]
                                                   ,[ColorId]
                                                   ,[Price]
                                                   ,[IsSsd]
                                                   ,[Used])
                                             VALUES
                                                   ('{0}'
                                                   ,{1}
                                                   ,{2}
                                                   ,{3}
                                                   ,{4}
                                                   ,{5})", laptop.ModelName, laptop.BrandId, laptop.ColorId, laptop.price, laptop.IsSsd, laptop.Used);

                    conn.ExecuteNonQuery(saveQuery);
                }



                conn.CommitTransaction();
                resultMSG = "Success";
            }
            catch (Exception ex)
            {
                resultMSG = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return resultMSG;
        }

        public GridEntity<LaptopInfo> LaptopInfoGrid(GridOptions options)
        {
            CommonConnection conn = new CommonConnection();

            try
            {
                conn.BeginTransaction();
                string query = string.Format(@"SELECT LaptopInfo.LaptopId,LaptopInfo.ModelName,LaptopBrands.Brand,LaptopColors.Color,LaptopInfo.Price,LaptopInfo.IsSsd,LaptopInfo.Used
	                                                   FROM LaptopInfo
	                                                   LEFT JOIN LaptopBrands ON LaptopBrands.BrandId=LaptopInfo.BrandId
	                                                   LEFT JOIN LaptopColors ON LaptopColors.ColorId = LaptopInfo.ColorId ");

                return Kendo<LaptopInfo>.Grid.DataSource(options, query, "LaptopId");
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public string Delete(int LaptopId)
        {
            CommonConnection conn = new CommonConnection();
            var query = "";
            var res = "";
            try
            {
                conn.BeginTransaction();
                query = string.Format(@"DELETE  FROM LaptopInfo    WHERE LaptopId = {0}", LaptopId);

                conn.ExecuteNonQuery(query);
                res = Operation.Success.ToString();
                conn.CommitTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return res;
        }

    }
}
