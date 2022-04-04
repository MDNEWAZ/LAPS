using System;
using System.Configuration;
using System.Data;
using Azolution.Common.DataService;
using Azolution.Core.DataService.DataReader;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataService
{
    public class SystemSettingsDataService 
    {

        #region Global Variable

        private CommonDbHelper oracleDbHelper = null;
        private DBHelper SqlDbHelper = null;
        private OdbcDbHelper MySqlDbHelper = null;

        #endregion

        #region SQL

        private const string SELECT_SYSTEM_SETTINGS_BY_COMPANYID =
            "SELECT SettingsId, CompanyId, Theme, Language, MinLoginLength," +
            "MinPassLength, PassType, SpecialCharAllowed, WrongAttemptNo, ChangePassDays, ChangePassFirstLogin, PassExpiryDays, " +
            "ResetPass,PassResetBy,OldPassUseRestriction, OdbcClientList FROM SystemSettings where CompanyId = {0}";

        private const string InsertSystemSettingsData =
            "INSERT INTO SystemSettings(CompanyId,Theme,Language,MinLoginLength,MinPassLength,PassType,SpecialCharAllowed,WrongAttemptNo,ChangePassDays,ChangePassFirstLogin,PassExpiryDays,ResetPass,PassResetBy,OldPassUseRestriction,LastUpdateDate,OdbcClientList,UserId)" +
            " VALUES ({0}, '{1}', '{2}', '{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',{14},'{15}','{16}')";

        private const string UpdateSystemSettingsData =
            "Update SystemSettings set CompanyId = {0}, Theme='{1}', Language = '{2}', MinLoginLength = '{3}', MinPassLength='{4}',PassType='{5}',SpecialCharAllowed = '{6}',WrongAttemptNo='{7}',ChangePassDays='{8}',ChangePassFirstLogin='{9}',PassExpiryDays='{10}',ResetPass='{11}',PassResetBy='{12}',OldPassUseRestriction='{13}',LastUpdateDate={14},OdbcClientList='{15}',UserId='{16}' where SettingsId = {17}";


        #endregion

        #region Global Method

        public void GetDbHelper()
        {
            var connectionString = "";
            var connectionType = ConfigurationSettings.AppSettings["DataBaseType"];
            if (connectionType == "SQL")
            {
                connectionString = "SqlConnectionString";
                SqlDbHelper = new DBHelper(connectionString);
            }
            else if (connectionType == "MySql")
            {
                connectionString = "MySqlConnectionString";
                MySqlDbHelper = new OdbcDbHelper(connectionString);

            }
            else if (connectionType == "Oracle")
            {
                connectionString = "OracleConnectionString";
                oracleDbHelper = new CommonDbHelper(connectionString);
            }
        }

        #endregion


        public SystemSettings GetSystemSettingsDataByCompanyId(int companyId)
        {
            SystemSettings objSystemSettings = null;
            GetDbHelper();
            var quary = string.Format(SELECT_SYSTEM_SETTINGS_BY_COMPANYID, companyId);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var systemsettingsDataReader = new SystemSettingsDataReader(reader);

                if (reader.Read())
                    objSystemSettings = systemsettingsDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var systemsettingsDataReader = new SystemSettingsDataReader(reader);

                if (reader.Read())
                    objSystemSettings = systemsettingsDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objSystemSettings;
        }

        public string SaveSystemSettings(SystemSettings objSystemSettings)
        {
            var res = "";
            GetDbHelper();
            if (SqlDbHelper == null && oracleDbHelper == null && MySqlDbHelper == null)
            {
                res = "Please Configure Database type";
                return res;
            }
            else
            {

                var quary = "";
                if (SqlDbHelper != null)
                {
                    try
                    {
                        
                        if(objSystemSettings.SettingsId == 0)
                        {
                            quary = string.Format(InsertSystemSettingsData, objSystemSettings.CompanyId,
                                                  objSystemSettings.Theme, objSystemSettings.Language,
                                                  objSystemSettings.MinLoginLength, objSystemSettings.MinPassLength,
                                                  objSystemSettings.PassType, objSystemSettings.SpecialCharAllowed,
                                                  objSystemSettings.WrongAttemptNo, objSystemSettings.ChangePassDays,
                                                  objSystemSettings.ChangePassFirstLogin,
                                                  objSystemSettings.PassExpiryDays, objSystemSettings.ResetPass,
                                                  objSystemSettings.PassResetBy, objSystemSettings.OldPassUseRestriction,
                                                  "'" + DateTime.Now + "'", objSystemSettings.OdbcClientList,
                                                  objSystemSettings.UserId);
                        }
                        else
                        {
                            quary = string.Format(UpdateSystemSettingsData, objSystemSettings.CompanyId,
                                                  objSystemSettings.Theme, objSystemSettings.Language,
                                                  objSystemSettings.MinLoginLength, objSystemSettings.MinPassLength,
                                                  objSystemSettings.PassType, objSystemSettings.SpecialCharAllowed,
                                                  objSystemSettings.WrongAttemptNo, objSystemSettings.ChangePassDays,
                                                  objSystemSettings.ChangePassFirstLogin,
                                                  objSystemSettings.PassExpiryDays, objSystemSettings.ResetPass,
                                                  objSystemSettings.PassResetBy, objSystemSettings.OldPassUseRestriction,
                                                  "'" + DateTime.Now + "'", objSystemSettings.OdbcClientList,
                                                  objSystemSettings.UserId,objSystemSettings.SettingsId);
                        }
                        
                        SqlDbHelper.ExecuteNonQuery(quary);
                        res = "Success";
                    }
                    catch (Exception ex)
                    {
                        res = ex.Message;
                        SqlDbHelper.RollBack();
                    }
                    finally
                    {
                        SqlDbHelper.Close();
                    }
                }
                else if (oracleDbHelper != null)
                {
                    try
                    {
                        var specialCharAllowed = objSystemSettings.SpecialCharAllowed == false ? 0 : 1;
                        var changePassAfterFirstLogin = objSystemSettings.ChangePassFirstLogin == false ? 0 : 1;
                        var odbcClientList = objSystemSettings.OdbcClientList == false ? 0 : 1;
                        string lastUpdateDate = DateTime.Today.ToString("dd/MMM/yyyy");
                        lastUpdateDate = lastUpdateDate.Replace('/', '-');
                        if (objSystemSettings.SettingsId == 0)
                        {
                            quary = string.Format(InsertSystemSettingsData, objSystemSettings.CompanyId,
                                                  objSystemSettings.Theme, objSystemSettings.Language,
                                                  objSystemSettings.MinLoginLength, objSystemSettings.MinPassLength,
                                                  objSystemSettings.PassType, specialCharAllowed,
                                                  objSystemSettings.WrongAttemptNo, objSystemSettings.ChangePassDays,
                                                  changePassAfterFirstLogin,
                                                  objSystemSettings.PassExpiryDays, objSystemSettings.ResetPass,
                                                  objSystemSettings.PassResetBy, objSystemSettings.OldPassUseRestriction,
                                                  "to_date('" + lastUpdateDate + "')", odbcClientList,
                                                  objSystemSettings.UserId);
                        }
                        else
                        {
                            quary = string.Format(UpdateSystemSettingsData, objSystemSettings.CompanyId,
                                                  objSystemSettings.Theme, objSystemSettings.Language,
                                                  objSystemSettings.MinLoginLength, objSystemSettings.MinPassLength,
                                                  objSystemSettings.PassType, specialCharAllowed,
                                                  objSystemSettings.WrongAttemptNo, objSystemSettings.ChangePassDays,
                                                  changePassAfterFirstLogin,
                                                  objSystemSettings.PassExpiryDays, objSystemSettings.ResetPass,
                                                  objSystemSettings.PassResetBy, objSystemSettings.OldPassUseRestriction,
                                                  "to_date('" + lastUpdateDate + "')", odbcClientList,
                                                  objSystemSettings.UserId, objSystemSettings.SettingsId);
                        }
                        oracleDbHelper.ExecuteNonQuery(quary);
                        res = "Success";
                    }
                    catch (Exception ex)
                    {
                        res = ex.Message;
                        oracleDbHelper.RollBack();
                    }
                    finally
                    {
                        oracleDbHelper.Close();
                    }
                }
            }

            return res;
        }

        public DataTable GetSystemSettingsData()
        {
            var dt = new DataTable();
            GetDbHelper();
            var quary = string.Format(@"Select * from SYSTEMSETTINGS");
            if (SqlDbHelper != null)
            {

                //to_date('" + attendanceDate.ToString("MM/dd/yyyy");
                try
                {


                    dt = SqlDbHelper.GetDataTable(quary);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    SqlDbHelper.Close();
                }
            }
            if (oracleDbHelper != null)
            {
                try
                {

                    dt = oracleDbHelper.GetTable(quary);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    oracleDbHelper.Close();
                }
            }
            return dt;
        }
    }
}
