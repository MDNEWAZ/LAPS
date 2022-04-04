using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace MobileDataService.DataService
{
    public class MobileDataService
    {
        public List<string> PopulateColorCombo()
        {
            CommonConnection connection = new CommonConnection();            
            try
            {
                connection.BeginTransaction();

                string query = "SELECT Color FROM MobileColors";

                return Kendo<string>.Combo.DataSource(query);
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
