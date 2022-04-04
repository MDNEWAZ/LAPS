using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Report;
using Azolution.Entities.Sale;
using Utilities;

namespace Azolution.Reports.DataService.DataService
{
    public class SaleReportDataService
    {
        SqlCommand _aCommand;
        SqlDataAdapter _adapter;
        readonly string _connection = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;

        public DataTable GetAllSaleReport()
        {
            try
            {
                var aTable = new DataTable();
                using (var conn = new SqlConnection(_connection))
                {
                    conn.Open();
                    var adapter = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand("[GetCollecteList_ByCustomer]", conn)
                            {
                                CommandType = CommandType.StoredProcedure
                            }
                    };
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@userId", 24));
                    var ds = new DataSet();
                    adapter.Fill(aTable);
                }
                return aTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
