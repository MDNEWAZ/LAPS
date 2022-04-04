using System;
using System.Data;
using System.Linq;
using Azolution.BulkUploadService.Interface;

namespace Azolution.BulkUploadService.Service
{
    public class BulkUserUploadService : IUserUploadRepository
    {

        BulkUploadDataService.BulkUploadDataService _bulkUploadDataService = new BulkUploadDataService.BulkUploadDataService(); 
        

        public string ImportUserUplodedData(string importFilePath, int userId)
        {
            //ISystemSettingsRepository _systemSettingsRepository = new SystemSettingsDataService();
            //SystemSettingsService _st = new SystemSettingsService(_systemSettingsRepository);
            
            //string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + importFilePath + ";Extended Properties=Excel 12.0;";

            //OleDbConnection oConn = new OleDbConnection();
            //oConn.ConnectionString = strConn;
            //oConn.Open();

            //OleDbCommand oSelect = new OleDbCommand("SELECT * FROM [Sheet1$]", oConn);
            //oSelect.CommandType = CommandType.Text;
            //var dataTable = new DataTable();
            //dataTable.Load(oSelect.ExecuteReader());

            //DataTable dtEmployment = _employeeRepository.GetAllEmploymentInformation();

            //DataTable dtSystemSettings = _st.GetSystemSettingsData();

            //DataSet finalDataTable = CreateCustomDataTableForUserUpload(dataTable, dtEmployment, dtSystemSettings);
            //const bool mainTableInsert = true;
            //string condition =
            //    " Where EmployeeId not in (Select EmployeeId from USERS) and LOGINID not in (Select LOGINID from USERS)";
            //var res =
            //    _bulkUploadDataService.BulkInsertDataIntoDestinationTableFromDataTable(
            //        finalDataTable.Tables["UploadData"], "USERSTEMP", mainTableInsert, "USERS", condition,"");

            //if (finalDataTable.Tables["ErrorData"].Rows.Count > 0)
            //{
            //    res = "Data Partially upload";
            //}

            //return res;

            return "";

        }

        private DataSet CreateCustomDataTableForUserUpload(DataTable dataTable, DataTable dtEmployment, DataTable dtSystemSettings)
        {
            DataSet ds = new DataSet();

            DataTable uploadData = new DataTable();
            DataTable errorData = new DataTable();

            #region Header For Upload Data
            DataColumn dc = new DataColumn();
            dc.DataType = Type.GetType("System.Int64");
            dc.ColumnName = "COMPANYID";
            uploadData.Columns.Add(dc);

            DataColumn dc1 = new DataColumn();
            dc1.DataType = Type.GetType("System.String");
            dc1.ColumnName = "LOGINID";
            uploadData.Columns.Add(dc1);

            DataColumn dc2 = new DataColumn();
            dc2.DataType = Type.GetType("System.String");
            dc2.ColumnName = "USERNAME";
            uploadData.Columns.Add(dc2);

            DataColumn dc3 = new DataColumn();
            dc3.DataType = Type.GetType("System.String");
            dc3.ColumnName = "PASSWORD";
            uploadData.Columns.Add(dc3);

            DataColumn dc4 = new DataColumn();
            dc4.DataType = Type.GetType("System.Int64");
            dc4.ColumnName = "EMPLOYEEID";
            uploadData.Columns.Add(dc4);

            DataColumn dc5 = new DataColumn();
            dc5.DataType = Type.GetType("System.DateTime");
            dc5.ColumnName = "CREATEDDATE";
            uploadData.Columns.Add(dc5);

            DataColumn dc6 = new DataColumn();
            dc6.DataType = Type.GetType("System.DateTime");
            dc6.ColumnName = "LASTUPDATEDATE";
            uploadData.Columns.Add(dc6);

            DataColumn dc7 = new DataColumn();
            dc7.DataType = Type.GetType("System.DateTime");
            dc7.ColumnName = "LASTLOGINDATE";
            uploadData.Columns.Add(dc7);

            DataColumn dc8 = new DataColumn();
            dc8.DataType = Type.GetType("System.Int64");
            dc8.ColumnName = "FAILEDLOGINNO";
            uploadData.Columns.Add(dc8);

            DataColumn dc9 = new DataColumn();
            dc9.DataType = Type.GetType("System.Int64");
            dc9.ColumnName = "ISACTIVE";
            uploadData.Columns.Add(dc9);

            DataColumn dc10 = new DataColumn();
            dc10.DataType = Type.GetType("System.Int64");
            dc10.ColumnName = "ISEXPIRED";
            uploadData.Columns.Add(dc10);

            DataColumn dc11 = new DataColumn();
            dc11.DataType = Type.GetType("System.String");
            dc11.ColumnName = "THEME";
            uploadData.Columns.Add(dc11);
            #endregion

            #region Header For Error Data

            DataColumn dc12 = new DataColumn();
            dc12.DataType = Type.GetType("System.String");
            dc12.ColumnName = "EmployeeCode";
            errorData.Columns.Add(dc12);

            DataColumn dc13 = new DataColumn();
            dc13.DataType = Type.GetType("System.String");
            dc13.ColumnName = "LoginId";
            errorData.Columns.Add(dc13);

            DataColumn dc14 = new DataColumn();
            dc14.DataType = Type.GetType("System.String");
            dc14.ColumnName = "UserName";
            errorData.Columns.Add(dc14);

            DataColumn dc15 = new DataColumn();
            dc15.DataType = Type.GetType("System.String");
            dc15.ColumnName = "ErrorMessage";
            errorData.Columns.Add(dc15);

            #endregion

            foreach (DataRow _dataR in dataTable.Rows)
            {
                try
                {
                    var obj = (from d in dtEmployment.AsEnumerable()
                               where d.Field<string>("EMPLOYEEID") == Convert.ToString(_dataR["EmployeeCode"].ToString())
                               select d).FirstOrDefault();
                    if (obj != null)
                    {
                        #region Uploaded Row Data

                        DataRow objSystem;
                        try
                        {
                            objSystem = (from d in dtSystemSettings.AsEnumerable()
                                             where
                                                 d.Field<decimal>("CompanyId") ==
                                                 Convert.ToDecimal(obj["CompanyId"].ToString())
                                             select d).FirstOrDefault();
                        }
                        catch
                        {
                            objSystem = (from d in dtSystemSettings.AsEnumerable()
                                         where
                                             d.Field<int>("CompanyId") ==
                                             Convert.ToDecimal(obj["CompanyId"].ToString())
                                         select d).FirstOrDefault();
                        }

                        if (objSystem != null)
                        {
                            DataRow dr = uploadData.NewRow();
                            dr["COMPANYID"] = Convert.ToInt64(obj["CompanyId"].ToString());
                            dr["LOGINID"] = Convert.ToString(_dataR["LoginId"].ToString());
                            dr["USERNAME"] = Convert.ToString(_dataR["UserName"].ToString());
                            dr["PASSWORD"] = objSystem["RESETPASS"].ToString();
                            dr["EMPLOYEEID"] = Convert.ToInt64(obj["HRRECORDID"].ToString());
                            dr["CREATEDDATE"] = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                            dr["LASTUPDATEDATE"] = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                            dr["LASTLOGINDATE"] = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                            dr["FAILEDLOGINNO"] = 0;
                            dr["ISACTIVE"] = 1;
                            dr["ISEXPIRED"] = 0;
                            dr["THEME"] = "";
                            uploadData.Rows.Add(dr);


                        }
                        else
                        {
                            #region Error Row Data

                            DataRow dr = errorData.NewRow();
                            dr["EmployeeCode"] = Convert.ToString(_dataR["EmployeeCode"].ToString());
                            dr["LoginId"] = Convert.ToString(_dataR["LoginId"].ToString());
                            dr["UserName"] = Convert.ToString(_dataR["UserName"].ToString());
                            dr["ErrorMessage"] = "Sytem Settings Not Found For This Company";

                            errorData.Rows.Add(dr);

                            #endregion
                        }

                        #endregion

                    }
                    else
                    {
                        #region Error Row Data

                        DataRow dr = errorData.NewRow();
                        dr["EmployeeCode"] = Convert.ToString(_dataR["EmployeeCode"].ToString());
                        dr["LoginId"] = Convert.ToString(_dataR["LoginId"].ToString());
                        dr["UserName"] = Convert.ToString(_dataR["UserName"].ToString());
                        dr["ErrorMessage"] = "Employee Information Not Found on Personal Management";

                        errorData.Rows.Add(dr);

                        #endregion
                    }
                }
                catch(Exception ex)
                {
                    
                }
            }
            uploadData.TableName = "UploadData";
            errorData.TableName = "ErrorData";
            ds.Tables.Add(uploadData);
            ds.Tables.Add(errorData);

            return ds;
        }
    }
}
