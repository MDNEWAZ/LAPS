using System;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.DataService.DataService
{
    public class CreditPeriodDataService
    {
        public string SaveCreditPeriod(CreditPeriod objCreditPeriodInfo)
        {
            string res = "";
            CommonConnection connection = new CommonConnection();
            try
            {
                string deletequery = string.Format(@"Delete from CreditPeriod");
                connection.ExecuteNonQuery(deletequery);
                string query = string.Format(@" Insert into CreditPeriod(DefaultInstallmentNo,Status) values({0},{1})", objCreditPeriodInfo.DefaultInstallmentNo,1);
                connection.ExecuteNonQuery(query);
                res = "Success";
            }
            catch (Exception ex)
            {

                res = ex.Message;
            }
            return res;
        }

        public GridEntity<CreditPeriod> GetCreditPeriodSummary(GridOptions options)
        {
            string query = string.Format(@"Select * from CreditPeriod");
            return Kendo<CreditPeriod>.Grid.DataSource(options, query, "DefaultInstallmentNo");
        }
    }
}
