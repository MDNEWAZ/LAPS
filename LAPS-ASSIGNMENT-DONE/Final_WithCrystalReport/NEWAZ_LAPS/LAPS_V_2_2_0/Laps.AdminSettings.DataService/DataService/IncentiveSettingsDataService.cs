using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.DataService.DataService
{
    public class IncentiveSettingsDataService
    {
        public string SaveIncentiveSettings(Incentive objIncentive)
        {
            string sql = "";
            string rv = "";
            CommonConnection connection = new CommonConnection();
            try
            {
                if (!CheckExistIncentiveSettings(ExistCheckCondition(objIncentive)))
                {
                    if (objIncentive.IncentiveId == 0)
                    {
                        sql = string.Format(
                            @"Insert Into IncentiveSettings (NumberOfSale,IncentiveAmount,IsActive) Values({0},{1},{2})",
                            objIncentive.NumberOfSale, objIncentive.IncentiveAmount,objIncentive.IsActive);
                    }
                    else
                    {
                        sql = string.Format(
                            @"Update IncentiveSettings Set NumberOfSale={0},IncentiveAmount={1},IsActive={2} Where IncentiveId={3}",
                            objIncentive.NumberOfSale, objIncentive.IncentiveAmount,objIncentive.IsActive, objIncentive.IncentiveId);
                    }

                    connection.ExecuteNonQuery(sql);
                    rv = Operation.Success.ToString();
                }
                else
                {
                    rv = Operation.Exists.ToString();
                }

               
            }
            catch (Exception exception)
            {
                rv = exception.Message;
            }
            finally
            {
                connection.Close();
            }

            return rv;
        }

        private static string ExistCheckCondition(Incentive objIncentive)
        {
            string condition = "";
            if (objIncentive.IncentiveId == 0)
            {
                condition = " Where NumberOfSale=" + objIncentive.NumberOfSale;
            }
            else
            {
                condition = " Where NumberOfSale=" + objIncentive.NumberOfSale + " And IncentiveId !=" +
                            objIncentive.IncentiveId;
            }
            return condition;
        }

        private bool CheckExistIncentiveSettings(string condition)
        {
            string sql = string.Format(@"Select * From IncentiveSettings {0}", condition);
            var data = Data<Incentive>.DataSource(sql);
            return data.Count != 0 || data.Any();
        }

        public GridEntity<Incentive> GetIncentiveSettingsSummary(GridOptions options)
        {
            string sql = "Select * From IncentiveSettings";
            var data = Kendo<Incentive>.Grid.DataSource(options, sql, "IncentiveId");
            return data;
        }

        public List<Incentive> GetIncentiveSettingsData()
        {
            string sql = "Select * From IncentiveSettings Where IsActive=1";
            var data = Data<Incentive>.DataSource(sql);
            return data;
        }
    }
}
