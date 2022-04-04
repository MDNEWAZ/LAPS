using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Azolution.Common.DataService;
using Oracle.DataAccess.Client;

namespace Azolution.BulkUploadDataService
{
    public class BulkUploadDataService
    {

        #region Common

        private CommonDbHelper _oracleDbHelper;
        private DBHelper _sqlDbHelper;
        private OdbcDbHelper _mySqlDbHelper;

        public object GetDbHelper()
        {
            string connectionString;
            var connectionType = ConfigurationManager.ConnectionStrings["DataBaseType"].ConnectionString;
            //var connectionType = ConfigurationSettings.AppSettings["DataBaseType"];
            if (connectionType == "SQL")
            {
                connectionString = "SqlConnectionString";
                _sqlDbHelper = new DBHelper(connectionString);
                return _sqlDbHelper;
            }
            else if (connectionType == "MySql")
            {
                connectionString = "MySqlConnectionString";
                _mySqlDbHelper = new OdbcDbHelper(connectionString);
                return _mySqlDbHelper;

            }
            else
            {
                connectionString = "OracleConnectionString";
                _oracleDbHelper = new CommonDbHelper(connectionString);
                return _oracleDbHelper;
            }
        }

        

        #endregion

        public string BulkInsertDataIntoDestinationTableFromDataTable(DataTable dataTable, string destinationTable, bool mainTableInsert,string mainTableName,string condition, string conditionSql)
        {
            var res = "";
            var dbhelper = GetDbHelper();

            if (_sqlDbHelper != null)
            {
                try
                {
                   

                        var quary = string.Format("Delete " + destinationTable);
                        _sqlDbHelper.ExecuteNonQuery(quary);

                        var conString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                        using (SqlConnection dbConnection = new SqlConnection(conString))
                        {
                            dbConnection.Open();
                            using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                            {
                                s.DestinationTableName = destinationTable;
                                s.WriteToServer(dataTable);
                            }
                            dbConnection.Close();
                            dbConnection.Dispose();
                        }

                        if (mainTableInsert)
                        {

                            string column = "";

                            for (int i = 0; i < dataTable.Columns.Count; i++)
                            {
                                if (column == "")
                                {
                                    column += dataTable.Columns[i].ColumnName;
                                }
                                else
                                {
                                    column += "," + dataTable.Columns[i].ColumnName;
                                }
                            }


                            var newquary = string.Format("begin " +
                                                         "Insert into {0} ({1}) " +
                                                         "select {1} from {2} " +
                                                         "{3} ;" +
                                                         " end;",
                                                         mainTableName, column, destinationTable, conditionSql);
                            _sqlDbHelper.ExecuteNonQuery(newquary);
                        }

                    res = "Success";
                }
                catch (Exception ex)
                {
                    res = ex.Message;
                }
                finally
                {
                    _sqlDbHelper.Close();
                }

            }
            else if (_oracleDbHelper != null)
            {
                try
                {

                    var quary = string.Format("Delete "+destinationTable);
                    _oracleDbHelper.ExecuteNonQuery(quary);

                    var conString = ConfigurationManager.ConnectionStrings["OracleConnectionString"];
                    var oracleConnection = new OracleConnection(conString.ConnectionString);
                    oracleConnection.Open();

                    using (OracleBulkCopy bulkCopy = new OracleBulkCopy(oracleConnection))
                    {
                        bulkCopy.DestinationTableName = destinationTable;
                        bulkCopy.WriteToServer(dataTable);
                    }

                    oracleConnection.Close();
                    oracleConnection.Dispose();

                    if (mainTableInsert)
                    {

                        string column = "";

                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            if (column == "")
                            {
                                column += dataTable.Columns[i].ColumnName;
                            }
                            else
                            {
                                column += ","+ dataTable.Columns[i].ColumnName;
                            }
                        }

                        
                        var newquary = string.Format("begin " +
                                                     "Insert into {0} ({1}) " +
                                                     "select {1} from {2} " +
                                                     "{3} ;" +
                                                     " end;",
                                                     mainTableName, column, destinationTable,condition);
                        _oracleDbHelper.ExecuteNonQuery(newquary);
                    }


                    res = "Success";
                }
                catch (Exception ex)
                {
                    res = ex.Message;
                }
                finally
                {
                    _oracleDbHelper.Close();
                }
            }



            return res;
        }


    }
}
