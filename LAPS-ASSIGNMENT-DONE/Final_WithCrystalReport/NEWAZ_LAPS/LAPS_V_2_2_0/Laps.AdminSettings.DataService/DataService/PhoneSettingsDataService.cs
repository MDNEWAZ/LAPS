using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.DataService.DataService
{
    public class PhoneSettingsDataService
    {
        public GridEntity<PhoneNoSettings> GetAllPhoneSettings(GridOptions options)
        {
            string sql = "Select * From PhoneNoSettings";
            var data = Kendo<PhoneNoSettings>.Grid.DataSource(options, sql, "PhoneNumber");
            return data;
        }

        public string SavePhoneSettings(PhoneNoSettings phoneObj)
        {
            string rv = "";
            string sql = "";
            CommonConnection connection = new CommonConnection();
            try
            {
                if (phoneObj.PhoneSettingsId == 0)
                {
                    sql = string.Format(@"Insert Into PhoneNoSettings(PhoneNumber,IsActive) Values('{0}',{1})", phoneObj.PhoneNumber, phoneObj.IsActive);

                }
                else
                {
                    sql = string.Format(@"Update PhoneNoSettings Set PhoneNumber='{0}',IsActive={1} Where PhoneSettingsId={2} ", phoneObj.PhoneNumber, phoneObj.IsActive, phoneObj.PhoneSettingsId);
                }

                connection.ExecuteNonQuery(sql);
                rv = Operation.Success.ToString();
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

        public List<PhoneNoSettings> GetAllActivePhoneNumber()
        {
            string sql = "Select * From PhoneNoSettings Where IsActive=1";
            var data = Data<PhoneNoSettings>.DataSource(sql);
            return data;
        }
    }
}
