using Azolution.Entities.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Laps.Mobile.DataService
{
    public class MobileDataService
    {
        public List<MobileColor> PopulateColorCombo()
        {
            CommonConnection conn = new CommonConnection();

            try
            {
                conn.BeginTransaction();
                string query = "SELECT * FROM MobileColors";

                return Kendo<MobileColor>.Combo.DataSource(query);
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
        public List<MobileBrand> PopulateBrandCombo()
        {
            CommonConnection conn = new CommonConnection();
            try
            {
                conn.BeginTransaction();
                string query = "SELECT * FROM  MobileBrands";
                return Kendo<MobileBrand>.Combo.DataSource(query);
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


        public List<MobileColor> PopulateColorCombo2()
        {
            CommonConnection conn = new CommonConnection();

            try
            {
                conn.BeginTransaction();
                string query = "SELECT * FROM MobileColors";

                return Kendo<MobileColor>.Combo.DataSource(query);
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
        public List<MobileBrand> PopulateBrandCombo2()
        {
            CommonConnection conn = new CommonConnection();
            try
            {
                conn.BeginTransaction();
                string query = "SELECT * FROM  MobileBrands";
                return Kendo<MobileBrand>.Combo.DataSource(query);
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
        public string SaveMobileInfo(MobileInfo mobile)
        {
            string resultMSG = "";
            CommonConnection conn = new CommonConnection();
            try
            {
                conn.BeginTransaction();

                if (mobile.MobileId != 0)
                {
                    string updateQuery = string.Format(@"UPDATE [dbo].[MobileInfo]
                                   SET [ModelName] = '{0}'
                                      ,[BrandId] = {1}
                                      ,[ColorId] = {2}
                                      ,[price] = {3}
                                      ,[Is5G] = {4}
                                      ,[IsSmart] = {5}
                                 WHERE MobileId = {6}", 
                             mobile.ModelName, mobile.BrandId, mobile.ColorId, mobile.Price, mobile.Is5G, mobile.IsSmart, mobile.MobileId);
                    conn.ExecuteNonQuery(updateQuery);
                }
                else
                {
                    string saveQuery = string.Format(@"INSERT INTO [dbo].[MobileInfo]
                                                   ([ModelName]
                                                   ,[BrandId]
                                                   ,[ColorId]
                                                   ,[Price]
                                                   ,[Is5G]
                                                   ,[IsSmart])
                                             VALUES
                                                  ('{0}'
                                                   ,{1}
                                                   ,{2}
                                                   ,{3}
                                                   ,{4}
                                                   ,{5})", mobile.ModelName, mobile.BrandId, mobile.ColorId, mobile.Price, mobile.Is5G, mobile.IsSmart);

                    conn.ExecuteNonQuery(saveQuery);
                }
                

                
                conn.CommitTransaction();
                resultMSG = "Success";
            }
            catch(Exception ex)
            {
                resultMSG = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return resultMSG;
        }

        public GridEntity<MobileInfo> MobileInfoGrid(GridOptions options)
        {
            CommonConnection conn = new CommonConnection();

            try
            {
                conn.BeginTransaction();
                string query = string.Format(@"SELECT MobileInfo.MobileId,MobileInfo.BrandId,MobileInfo.ColorId,MobileInfo.ModelName,MobileBrands.Brand,
                                                        MobileColors.Color, MobileInfo.price,MobileInfo.Is5G,MobileInfo.IsSmart  
                                                        FROM MobileInfo
                                                        LEFT JOIN MobileBrands ON MobileBrands.BrandId = MobileInfo.BrandId
                                                        LEFT JOIN MobileColors ON MobileColors.ColorId = MobileInfo.ColorId");

                return Kendo<MobileInfo>.Grid.DataSource(options, query, "MobileId");
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public string Delete(int MobileId)
        {
            CommonConnection conn = new CommonConnection();
            var query = "";
            var res = "";
            try
            {
                conn.BeginTransaction();
                query = string.Format(@"DELETE  FROM MobileInfo    WHERE MobileId = {0}", MobileId);

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




        //ChkPopup
        public string SaveFeature(MobileFeature feature)
        {
            string resultMSG = "";
            CommonConnection conn = new CommonConnection();
            try
            {
                conn.BeginTransaction();

              
               
                    string saveQuery = string.Format(@"INSERT INTO [dbo].[MobileFeature]
                                                   ([Smart]                                                 
                                                   ,[iOS]
                                                   ,[Android]
                                                   ,[Support5G]
                                                   ,[VRSupport])


                                             VALUES
                                                  ('{0}'
                                                   ,{1}
                                                   ,{2}
                                                   ,{3}
                                                   ,{4})", feature.Smart, feature.iOS, feature.Android, feature.Support5G, feature.VRSupport);
                                                   

                

                conn.ExecuteNonQuery(saveQuery);
                



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

        public GridEntity<MobileFeature> MobileFeatureGrid(GridOptions options)
        {
            CommonConnection conn = new CommonConnection();

            try
            {
                conn.BeginTransaction();
                string query = string.Format(@"SELECT * FROM MobileFeature;");

                return Kendo<MobileFeature>.Grid.DataSource(options, query, "FeatureId");
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
    }
}

