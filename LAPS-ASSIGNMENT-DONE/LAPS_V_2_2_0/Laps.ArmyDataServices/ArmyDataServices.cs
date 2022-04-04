using Azolution.Entities.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Laps.Army.DataServices
{
    public class ArmyDataServices
    {
        public List<ArmyGroups> PopulateArmyGroupCombo()
        {
            CommonConnection conn = new CommonConnection();

            try
            {
                conn.BeginTransaction();
                string query = "SELECT * FROM ArmyGroups";

                return Kendo<ArmyGroups>.Combo.DataSource(query);
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
       


        public string SaveArmyInfo(ArmyInfo army)
        {
            string resultMSG = "";
            CommonConnection conn = new CommonConnection();
            try
            {
                conn.BeginTransaction();

                if (army.ArmyId != 0)
                {
                    string updateQuery = string.Format(@"UPDATE [dbo].[ArmyInfo]
                                   SET [DefenceName] = '{0}'
                                      ,[ArmyGroupId] = {1}                                  
                                      ,[Salary] = {2}
                                      ,[IsOfficer] = {3}
                                      ,[Married] = {4}
                                 WHERE ArmyId = {5}", 
                             army.DefenceName, army.ArmyGroupId, army.Salary, army.IsOfficer, army.Married, army.ArmyId);
                    conn.ExecuteNonQuery(updateQuery);
                }
                else
                {
                    string saveQuery = string.Format(@"INSERT INTO [dbo].[ArmyInfo]
                                                   ([DefenceName]
                                                   ,[ArmyGroupId]
                                                   ,[Salary]
                                                   ,[IsOfficer]
                                                   ,[Married]
                                                   
                                             VALUES
                                                   ('{0}'
                                                   ,{1}
                                                   ,{2}
                                                   ,{3}
                                                   ,{4})", army.DefenceName, army.ArmyGroupId,army.Salary, army.IsOfficer, army.Married);

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

        public GridEntity<ArmyInfo> ArmyInfoGrid(GridOptions options)
        {
            CommonConnection conn = new CommonConnection();

            try
            {
                conn.BeginTransaction();
                            string query = string.Format(@"SELECT ArmyInfo.ArmyId,ArmyInfo.DefenceName,ArmyGroups.ArmyGroup,
                                                                ArmyInfo.Salary,ArmyInfo.IsOfficer,ArmyInfo.Married
                                                                FROM ArmyInfo
                                                                LEFT JOIN ArmyGroups ON ArmyGroups.ArmyGroupId=ArmyInfo.ArmyGroupId 
");

                return Kendo<ArmyInfo>.Grid.DataSource(options, query, "ArmyId");
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
        public string Delete(int ArmyId)
        {
            CommonConnection conn = new CommonConnection();
            var query = "";
            var res = "";
            try
            {
                conn.BeginTransaction();
                query = string.Format(@"DELETE  FROM ArmyInfo    WHERE ArmyId = {0}", ArmyId);

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

