using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using Azolution.Common.DataService;
using Azolution.Common.Helper;
using Azolution.Core.DataService.DataReader;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.HumanResource;
using Utilities;
using Utilities.Common;

namespace Azolution.Core.DataService.DataService
{
    public class UsersDataService 
    {
        #region Common
        private CommonDbHelper oracleDbHelper = null;
        private DBHelper SqlDbHelper = null;
        private OdbcDbHelper MySqlDbHelper = null;

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

        private string FilterCondition(AzFilter.GridFilters filter)
        {
            string whereClause = "";

            if (filter != null && (filter.Filters != null && filter.Filters.Count > 0))
            {

                var parameters = new List<object>();
                var filters = filter.Filters;

                for (var i = 0; i < filters.Count; i++)
                {
                    if (i == 0)
                    {
                        if (filters[i].Value == null)
                        {
                            i = i + 1;
                            if (filters.Count == i)
                            {
                                break;
                            }
                        }

                        whereClause += string.Format(" {0}",
                                                         UtilityCommon.BuildWhereClause<Users>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));

                    }
                    else
                    {
                        if (filters[i].Value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                                         UtilityCommon.ToLinqOperator(filter.Logic),
                                                         UtilityCommon.BuildWhereClause<Users>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));
                        }
                    }
                }
            }
            return whereClause;
        }

        #endregion

        #region SQL

       // private const string SAVE_USER =
          //  "Insert into Users (CompanyID,LoginId,UserName,Password,EmployeeId,CreatedDate,LastUpdateDate,LastLoginDate,FailedLoginNo,IsActive,IsExpired,IsNotify) values ({0},'{1}','{2}','{3}','{4}',{5},{6},{7},{8},{9},{10},{11})";

        private const string SAVE_USER = "SAVE_USER";

        private const string SAVE_USER_ORACLE =
           "Insert into Users (CompanyID,LoginId,UserName,Password,EmployeeId,CreatedDate,LastUpdateDate,LastLoginDate,FailedLoginNo,IsActive,IsExpired,IsNotify) values ({0},'{1}','{2}','{3}','{4}',{5},{6},{7},{8},{9},{10},{11}) returning UserId into :outId";

        private const string UPDATE_USER =
            "Update Users set CompanyID = {0}, LoginId='{1}', UserName='{2}', Password='{3}',CreatedDate={4},LastUpdateDate={5},LastLoginDate={6},FailedLoginNo={7},IsActive={8},IsExpired={9},BranchId={10},IsNotify = {11} where UserId={12}";
        private const string UPDATE_THEME =
          "Update Users set theme='{0}' where UserId={1}";

        private const string SAVE_GROUP_MEMBER = "Insert into GroupMember (GroupId,UserId) values ({0},{1})";

        private const string DELETE_GROUP_MEMBER_BY_USERID = "Delete GroupMember where UserId = {0}";

        private const string SELECT_USERS_BY_USERID = "Select * from Users where UserId = {0}";

        private const string SELECT_USERS_BY_LOGINID = "Select * from Users where Trim(Lower(LoginId)) = '{0}'";

        //private const string SELECT_USERS_BY_LOGINID_SQL = "Select Users.*,Branch.BranchCode from Users left outer join Branch on Branch.BRANCHID = Users.BranchId where rtrim(ltrim(Lower(LoginId))) = '{0}'";

        private const string SELECT_USERS_BY_LOGINID_SQL = "Select Users.*,Branch.BranchCode,Groups.IsViewer from  Users  " +
                            " left outer join Branch on Branch.BRANCHID = Users.BranchId "+
                            " left outer join GroupMember on GroupMember.UserId=Users.UserId"+
                            " left outer join Groups on Groups.GroupId=GroupMember.GroupId where rtrim(ltrim(Lower(LoginId))) = '{0}'";

        private const string SELECT_USERS_BY_LOGINID_AND_NOT_USERID =
            "Select * from Users where LoginId = '{0}' and UserId != {1}";

        private const string SELECT_GROUPMEMBER_BY_USERID = "Select * from GroupMember where UserId = {0}";

        private const string SAVE_PASSWORD_HISTORY =
            "insert into PasswordHistory (USERID, OLDPASSWORD, PASSWORDCHANGEDATE) values ({0}, '{1}', '{2}')";

        private const string SELECT_USERS_BY_EMAIL =
            "SELECT Users.UserId, Users.CompanyID, Users.LoginId, Users.UserName, Users.Password, Users.EmployeeId, Users.CreatedDate, Users.LastUpdateDate, Users.LastLoginDate, Users.FailedLoginNo, Users.IsActive, Users.IsExpired, Employment.OfficialEmail FROM Users INNER JOIN Employment ON Users.EmployeeId = Employment.HRRecordId WHERE (Employment.OfficialEmail = '{0}')";

      //  private const string SELECT_USERS_BY_HRRECORDID = "Select * from Users where EmployeeId = {0}";
        private const string SELECT_USERS_BY_UserId = "Select * from Users where UserId = {0}";

        #endregion



        public string SaveUser(Users users)
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
                var systemSettingsDataService = new SystemSettingsDataService();
                var objsystem = systemSettingsDataService.GetSystemSettingsDataByCompanyId(users.CompanyId);
                if (objsystem ==null)
                {
                    res = "Please First Save System Settings Data";
                    return res;
                }

                var validate = "";
                if (users.UserId == 0)
                {
                    validate = ValidateUser(users, objsystem);
                }
                else
                {
                    var userFromDb = GetUserById(users.UserId);
                    users.Password = users.Password == "" ? EncryptDecryptHelper.Decrypt(userFromDb.Password) : users.Password;
                    validate = ValidateUser(users, objsystem);
                }
                if (validate != "Valid")
                {
                    res = validate;
                    return res;
                }

                #region Sql Code
                if (SqlDbHelper != null)
                {
                    #region New Users
                    if (users.UserId == 0)
                    {

                        var objUserNewByLogInId = GetUserByLoginId(users.LoginId);
                        if (objUserNewByLogInId == null)
                        {
                            if (!IsExistsUserByEmployee(users.UserId))
                            {
                                
                           
                            users.CreatedDate = DateTime.Now;
                            users.LastUpdateDate = DateTime.Now;
                            users.IsExpired = false;

                            var encytpass = EncryptDecryptHelper.Encrypt(users.Password);
                            users.Password = encytpass;



                            GetDbHelper();
                            SqlDbHelper.BeginTransaction();
                             
                            int userId = SaveUserForSql(users, SqlDbHelper);
                            foreach (var groupMember in users.GroupMembers)
                            {
                                groupMember.UserId = Convert.ToInt32(userId);
                                var permisionQuary = string.Format(SAVE_GROUP_MEMBER,
                                                                   groupMember.GroupId,
                                                                   groupMember.UserId);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }

                            SqlDbHelper.CommitTransaction();
                            res = "Success";
                            }
                            else
                            {
                                res = "This Employee already exist";
                            }
                        }
                        else
                        {
                            res = "This Login ID already exist";
                            return res;
                        }
                    }
                    #endregion

                    #region Update User
                    else
                    {
                        var objUserNewByLogInId = GetUserByLoginIdAndNotUserId(users.LoginId, users.UserId);

                        if (objUserNewByLogInId == null)
                        {
                            var objUserforDb = GetUserById(users.UserId);
                            objUserforDb.CompanyId = users.CompanyId;
                            objUserforDb.LoginId = users.LoginId;
                            var encytpass = EncryptDecryptHelper.Encrypt(users.Password);
                            objUserforDb.Password = encytpass;
                            objUserforDb.UserName = users.UserName;
                            objUserforDb.BranchId = users.BranchId;
                            objUserforDb.IsActive = users.IsActive;
                            objUserforDb.LastUpdateDate = DateTime.Now;
                            objUserforDb.IsNotify = users.IsNotify;
                            if (objUserforDb.IsActive == true)
                            {
                                objUserforDb.FailedLoginNo = 0;
                            }
                            
                            var lastLoginDate =  objUserforDb.LastLoginDate == DateTime.MinValue? null: objUserforDb.LastLoginDate.ToShortDateString();
                            

                            GetDbHelper();
                            SqlDbHelper.BeginTransaction();
                           // "Update Users set CompanyID = {0}, LoginId='{1}', UserName='{2}', Password='{3}',CreatedDate={4},LastUpdateDate={5},LastLoginDate={6},FailedLoginNo={7},IsActive={8},IsExpired={9},BranchId={10} where UserId={11}";
                            var quary = string.Format(UPDATE_USER, objUserforDb.CompanyId, objUserforDb.LoginId, objUserforDb.UserName,
                                                      objUserforDb.Password,"'"+objUserforDb.CreatedDate + "'","'" + objUserforDb.LastUpdateDate + "'", "'" + lastLoginDate + "'", objUserforDb.FailedLoginNo,
                                                     "'" + objUserforDb.IsActive + "'", "'" + objUserforDb.IsExpired + "'", objUserforDb.BranchId,objUserforDb.IsNotify, objUserforDb.UserId);
                            SqlDbHelper.ExecuteNonQuery(quary);

                            var deleteGroupMemberQuary = string.Format(DELETE_GROUP_MEMBER_BY_USERID,
                                                                       users.UserId);
                            SqlDbHelper.ExecuteNonQuery(deleteGroupMemberQuary);

                            foreach (var groupMember in users.GroupMembers)
                            {
                                groupMember.UserId = users.UserId;
                                var permisionQuary = string.Format(SAVE_GROUP_MEMBER,
                                                                   groupMember.GroupId,
                                                                   groupMember.UserId);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }

                            SqlDbHelper.CommitTransaction();
                            res = "Success";
                        }
                        else
                        {
                            res = "This login ID already exist on another user";
                            return res;
                        }
                    }
                    #endregion
                }
                #endregion

                #region Oracle Code
                else if (oracleDbHelper != null)
                {
                    #region New User
                    if (users.UserId == 0)
                    {
                        var objUserNewByLogInId = GetUserByLoginId(users.LoginId);
                        if (objUserNewByLogInId == null)
                        {
                            string currentDate = DateTime.Today.ToString("dd/MMM/yyyy");
                            currentDate = currentDate.Replace('/', '-');
                            int isExpired = 0;

                            int isActive = users.IsActive == true ? 1 : 0;

                            var encytpass = EncryptDecryptHelper.Encrypt(users.Password);
                            users.Password = encytpass;
                            GetDbHelper();
                            //oracleDbHelper.BeginTransaction();
                            var quary = string.Format(SAVE_USER_ORACLE, users.CompanyId, users.LoginId, users.UserName,
                                                      users.Password, users.EmployeeId, "'" + currentDate + "'",
                                                      "'" + currentDate+ "'", "'"+ null + "'", users.FailedLoginNo,
                                                      isActive, isExpired,users.IsNotify);
                            //oracleDbHelper.ExecuteNonQuery(quary);
                            //int userId = GetMaxUserIdOracle();

                            int userId = 0;

                            var con = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;
                            using (OracleConnection connection = new OracleConnection(con))
                            {
                                connection.Open();

                                OracleCommand command = connection.CreateCommand();
                                OracleTransaction transaction;
                                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = connection;
                                command.Transaction = transaction;

                                try
                                {
                                    command.CommandText = quary.ToString();
                                    OracleParameter p2 = new OracleParameter("outId", OracleType.Int32);
                                    p2.Direction = ParameterDirection.Output;
                                    command.Parameters.Add(p2);
                                    command.ExecuteNonQuery();
                                    transaction.Commit();

                                    userId = Convert.ToInt32(command.Parameters[0].Value);
                                    res = "Success";
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        transaction.Rollback();
                                    }
                                    catch (OracleException ex)
                                    {
                                        string exp = ex.Message;
                                        res = exp;
                                    }
                                    finally
                                    {
                                        string exp = e.Message;
                                        res = exp;

                                    }

                                }
                            }

                            if (userId != 0)
                            {

                                foreach (var groupMember in users.GroupMembers)
                                {
                                    groupMember.UserId = userId;
                                    var permisionQuary = string.Format(SAVE_GROUP_MEMBER,
                                                                       groupMember.GroupId,
                                                                       groupMember.UserId);
                                    oracleDbHelper.ExecuteNonQuery(permisionQuary);
                                }
                            }

                            //oracleDbHelper.CommitTransaction();
                            res = "Success";
                        }
                        else
                        {
                            res = "This Login ID already exist";
                            return res;
                        }
                    }

                    #endregion

                    #region Update User
                    else
                    {
                        var objUserNewByLogInId = GetUserByLoginIdAndNotUserId(users.LoginId, users.UserId);
                        if (objUserNewByLogInId == null)
                        {

                            var objUserforDb = GetUserById(users.UserId);
                            objUserforDb.CompanyId = users.CompanyId;
                            objUserforDb.LoginId = users.LoginId;
                            var encytpass = EncryptDecryptHelper.Encrypt(users.Password);
                            objUserforDb.Password = encytpass;
                            objUserforDb.UserName = users.UserName;
                            objUserforDb.IsActive = users.IsActive;
                            objUserforDb.LastUpdateDate = DateTime.Now;
                            if (objUserforDb.IsActive == true)
                            {
                                objUserforDb.FailedLoginNo = 0;
                            }

                            var crdt = objUserforDb.CreatedDate.ToString("dd/MMM/yyyy");
                            crdt = crdt.Replace('/', '-');

                            string currentDate = DateTime.Today.ToString("dd/MMM/yyyy");
                            currentDate = currentDate.Replace('/', '-');

                            var lastlogInDate = "";
                            if (objUserforDb.LastLoginDate == DateTime.MinValue)
                            {
                                lastlogInDate = null;
                            }
                            else
                            {
                               lastlogInDate = objUserforDb.LastLoginDate.ToString("dd/MMM/yyyy");
                                lastlogInDate = lastlogInDate.Replace('/', '-');
                            }

                            int isActive = objUserforDb.IsActive == true ? 1 : 0;
                            int isExpired = objUserforDb.IsExpired == true ? 1 : 0;


                            GetDbHelper();
                            oracleDbHelper.BeginTransaction();

                            var quary = string.Format(UPDATE_USER, objUserforDb.CompanyId, objUserforDb.LoginId, objUserforDb.UserName,
                                                      objUserforDb.Password, objUserforDb.EmployeeId, "'" + crdt + "'",
                                                      "'" + currentDate +"'", "'" + lastlogInDate + "'", objUserforDb.FailedLoginNo,
                                                      isActive, isExpired, objUserforDb.UserId);
                            oracleDbHelper.ExecuteNonQuery(quary);

                            var deleteGroupMemberQuary = string.Format(DELETE_GROUP_MEMBER_BY_USERID,
                                                                       users.UserId);
                            oracleDbHelper.ExecuteNonQuery(deleteGroupMemberQuary);

                            foreach (var groupMember in users.GroupMembers)
                            {
                                groupMember.UserId = users.UserId;
                                var permisionQuary = string.Format(SAVE_GROUP_MEMBER,
                                                                   groupMember.GroupId,
                                                                   groupMember.UserId);
                                oracleDbHelper.ExecuteNonQuery(permisionQuary);
                            }

                            oracleDbHelper.CommitTransaction();
                            res = "Success";
                        }
                        else
                        {
                            res = "This login ID already exist on another user";
                            return res;
                        }
                    }
                    #endregion
                }
                   
                #endregion
            }
            return res;
        }

        private bool IsExistsUserByEmployee(int hrRecordId)
        {
            Users objUsers = null;
            GetDbHelper();

            //string query = "Select EmployeeId from Users Where EmployeeId={0}";
            string query = "Select UserId from Users Where UserId={0}";
            if (SqlDbHelper != null)
            {
                var quary = string.Format(query, hrRecordId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                var quary = string.Format(query, hrRecordId);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }

            if (objUsers == null)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        public int SaveUserForSql(Users objUser, DBHelper SqlDbHelper)
        {
            int userId = 0;
            try
            {
                var param = new[]
                                {
                                    new SQLParam("@NewuserId", 0, ParameterDirection.Output),
                                    new SQLParam("@userId", objUser.UserId),
                                    new SQLParam("@companyID", objUser.CompanyId),
                                    new SQLParam("@loginId", objUser.LoginId),
                                    new SQLParam("@userName", objUser.UserName),
                                    new SQLParam("@password", objUser.Password),
                                    new SQLParam("@isActive", objUser.IsActive),
                                    new SQLParam("@createdDate", DateTime.Now),
                                    new SQLParam("@lastUpdateDate", DBNull.Value),
                                    new SQLParam("@lastLoginDate", DBNull.Value),
                                    new SQLParam("@failedLoginNo", 0),
                                    new SQLParam("@isExpired", false),
                                    new SQLParam("@branchId",objUser.BranchId),
                                    new SQLParam("@isNotify", objUser.IsNotify)
                                };
               // SqlDbHelper.ExecuteNonQuery(SAVE_USER,param);

                var query = string.Format(@"Insert into Users (CompanyID,LoginId,UserName,Password,CreatedDate,LastUpdateDate,LastLoginDate,FailedLoginNo,IsActive,IsExpired,BranchId,IsNotify) values ({0},'{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}',{10},{11})"
                ,objUser.CompanyId, objUser.LoginId, objUser.UserName, objUser.Password, objUser.CreatedDate, objUser.LastUpdateDate,
                    DateTime.Now,objUser.FailedLoginNo,objUser.IsActive,objUser.IsExpired,objUser.BranchId,objUser.IsNotify);

                SqlDbHelper.ExecuteNonQuery(query);

                if (objUser.UserId == 0)
                {
                    userId = (int)param[0].Value;
                }

            }
            catch(Exception ex)
            {
                SqlDbHelper.RollBack();
            }
            return userId;

        }

        private Users GetUserByLoginIdAndNotUserId(string loginId, int userId)
        {
            Users objUsers = null;
            GetDbHelper();
            var quary = string.Format(SELECT_USERS_BY_LOGINID_AND_NOT_USERID, loginId, userId);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objUsers;
        }

        public Users GetUserByLoginId(string loginId)
        {
            Users objUsers = null;
            GetDbHelper();
            

            if (SqlDbHelper != null)
            {
                var quary = string.Format(SELECT_USERS_BY_LOGINID_SQL, loginId.ToLower().Trim());
                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                var quary = string.Format(SELECT_USERS_BY_LOGINID, loginId.ToLower().Trim());
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objUsers;
        }

        public Users GetUserById(int userId)
        {
            Users objUsers = null;
            GetDbHelper();
            var quary = string.Format(SELECT_USERS_BY_USERID, userId);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objUsers;
        }

        public Users GetUserByEmployeeId(int hrRecordId)
        {
            Users objUsers = null;
            GetDbHelper();
            var quary = string.Format(SELECT_USERS_BY_UserId, hrRecordId);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objUsers;
        }

        private string ValidateUser(Users users, SystemSettings objsystem)
        {
            string specialChs = @"! ~ @ # $ % ^ & * ( ) _ - + = { } [ ] : ; , . < > ? / | \";
            string[] specialCharacters = specialChs.Split(' ');
            string message = "Valid";
            if (users.LoginId != "")
            {
                if (objsystem.MinLoginLength > users.LoginId.Trim().Length)
                {
                    message = "Login ID must have to be minimum " + objsystem.MinLoginLength + " character length!";
                    return message;
                }
            }

            if (objsystem.MinPassLength > users.Password.Trim().Length)
            {
                message = "Password must have to be minimum " + objsystem.MinPassLength + " character length!";
                return message;
            }
            if (objsystem.MinLoginLength == 0 && objsystem.MinPassLength == 0 && objsystem.SpecialCharAllowed == false) return message;

            int numCount = 0; //Numaric Charcter in password text
            int charCount = 0; //Charecter count
            int specialcharCount = 0;
            char[] pasChars = users.Password.ToCharArray();
            for (int i = 0; i < pasChars.Length; i++)
            {
                if (pasChars[i] == '0' || pasChars[i] == '1' || pasChars[i] == '2' || pasChars[i] == '3' ||
                    pasChars[i] == '4' || pasChars[i] == '5' || pasChars[i] == '6' || pasChars[i] == '7' ||
                    pasChars[i] == '8' || pasChars[i] == '9')
                    numCount++;
                else
                {
                    IEnumerable<string> found = specialCharacters.Where(x => x == pasChars[i].ToString());
                    if (found.Count() == 0)
                        charCount++;
                    else
                        specialcharCount++;
                }
            }
            //passType 0 = Alpjabetic, 1=Numeric, 2=AlphaNumeric
            if (objsystem.PassType == 0)
            {
                //0 = Alpjabetic

                if (numCount > 0)
                {
                    message = "Password must not have any number!";
                    return message;
                }

                if (charCount == 0)
                {
                    message = "Password must have to be alphabetic characters!";
                    return message;
                }
            }
            else if (objsystem.PassType == 1)
            {
                //1=Numeric
                if (numCount == 0)
                {
                    message = "Password must have atleast one numeric character!";
                    return message;
                }

                if (charCount > 0)
                {
                    message = "Password must not have any alphabetic character!";
                    return message;
                }

            }
            else
            {
                //2=AlphaNumeric
                if (numCount == 0)
                {
                    message = "Password must have atleast one numeric character!";
                    return message;
                }

                if (charCount == 0)
                {
                    message = "Password must have atleast one alphabetic character!";
                    return message;
                }


            }
            if (objsystem.SpecialCharAllowed == true)
            {
                if (specialcharCount == 0)
                {

                    message = "Password must have atleast one special character!";
                    return message;
                }
            }

            return message;
        }

        

        private int GetMaxUserIdSql()
        {
            int groupId = 0;
            string totalSql = "Select Max(UserId) as Total from Users";

            IDataReader reader = SqlDbHelper.GetDataReader(totalSql);
            if (reader.Read())
            {
                groupId = reader.GetInt32(0);
            }
            reader.Close();
            return groupId;
        }

        public GridEntity<Users> GetUserSummary(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {

            var gridOption = new GridOptions();
            gridOption.skip = skip;
            gridOption.take = take;
            gridOption.page = page;
            gridOption.pageSize = pageSize;
            gridOption.filter = filter;
            gridOption.sort = sort;

            

            string quary = string.Format(@"Select Users.*,Company.CompanyName,Branch.BRANCHNAME from Users
left outer join Company on Company.CompanyId = Users.CompanyID
left outer join Branch on Branch.BRANCHID = Users.BranchId
 where Users.CompanyId={0}", companyId);
            return Kendo<Users>.Grid.DataSource(gridOption, quary, " UserName ");

           
        }

        private string GetOracleQuaryTotalCountForUserSummary(List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                condition1 += " and CompanyId=" + companyId;
            }
            else
            {
                condition1 = "where CompanyId=" + companyId;
            }

            var sql = string.Format(@"SELECT COUNT(*) as TotalCount  FROM Users {0}", condition1);

            return sql;
        }

        private string GetOracleQuaryForUserSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition + " and CompanyId=" + companyId;
                condition += " and CompanyId=" + companyId + " and ";
            }
            string orderby = "";
            if (sort != null)
            {
                foreach (var gridSort in sort)
                {
                    if (orderby == "")
                    {
                        orderby += "Order by " + gridSort.field + " " + gridSort.dir;
                    }
                    else
                    {
                        orderby += " , " + gridSort.field + " " + gridSort.dir;
                    }
                }
            }

            if (orderby == "")
            {
                orderby = "ORDER BY UserName";
            }
            
            var pageupperBound = skip + take;
            var sql = string.Format(@"Select * from ( Select ROW_NUMBER() OVER({8}) AS RowIndex, tbl.* from (Select Users.*,DepartmentId,BranchId from Users inner join Employment on Employment.HrREcordId = Users.EmployeeID) tbl  WHERE {4} CompanyId={7} {8}) where RowIndex >{0} AND RowIndex <= {6} ", skip, take, page, pageSize, condition, condition1, pageupperBound, companyId, orderby);

            return sql;
        }

        private string GetSqlQuaryForUserSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition + " and CompanyId=" +companyId;
                condition += " and CompanyId=" + companyId + " and ";
            }
            else
            {
                condition1 = "where CompanyId=" + companyId;
                condition += " CompanyId=" + companyId + " and ";
            }


            string orderby = "";
            if (sort != null)
            {
                foreach (var gridSort in sort)
                {
                    if (orderby == "")
                    {
                        orderby += "Order by " + gridSort.field + " " + gridSort.dir;
                    }
                    else
                    {
                        orderby += " , " + gridSort.field + " " + gridSort.dir;
                    }
                }
            }

            if (orderby == "")
            {
                orderby = "ORDER BY UserName";
            }


            var pageupperBound = skip + take;
            var sql = string.Format(@"
Select * from (
Select ROW_NUMBER() OVER({7}) AS RowIndex, Users.*,DepartmentId,BranchId
from Users inner join Employment on Employment.HrREcordId = Users.EmployeeID) as tbl
WHERE {4} RowIndex >{0} AND RowIndex <= {6} {7}
SELECT COUNT(*) as TotalCount  FROM Users {5}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
        }

        public List<GroupMember> GetGroupMemberByUserId(int userId)
        {
            var groupMemberList = new List<GroupMember>();
            GetDbHelper();
            if (SqlDbHelper != null)
            {
                string sql = string.Format(SELECT_GROUPMEMBER_BY_USERID, userId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var groupMemberDataReader = new GroupMemberDataReader(reader);
                while (reader.Read())
                    groupMemberList.Add(groupMemberDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_GROUPMEMBER_BY_USERID, userId);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var groupMemberDataReader = new GroupMemberDataReader(reader);
                while (reader.Read())
                    groupMemberList.Add(groupMemberDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_GROUPMEMBER_BY_USERID, userId);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var groupMemberDataReader = new GroupMemberDataReader(reader);
                while (reader.Read())
                    groupMemberList.Add(groupMemberDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return groupMemberList;
        }

        public string ResetPassword(int companyId, int userId)
        {
            var mes = "";
            try
            {

                var _usersDataService = new UsersDataService();
                var systemSettingsDataService = new SystemSettingsDataService();
                var objSystemSettings = systemSettingsDataService.GetSystemSettingsDataByCompanyId(companyId);
                var objUser = GetUserById(userId);

                var passHistory = _usersDataService.GetPassHistory(userId, objSystemSettings.OldPassUseRestriction);

                //objUser.Password = EncryptDecryptHelper.Encrypt(objSystemSettings.ResetPass);
                objUser.Password = objSystemSettings.ResetPass;
                GetDbHelper();
                if (SqlDbHelper != null)
                {
                    SqlDbHelper.BeginTransaction();
                    int isActive = objUser.IsActive == true ? 1 : 0;
                    //int isExpired = objUser.IsExpired == true ? 1 : 0;
                    int isExpired = 0;

                    //var quary = string.Format(UPDATE_USER, objUser.CompanyId, objUser.LoginId, objUser.UserName,
                    //                                   objUser.Password, objUser.EmployeeId, "'" + objUser.CreatedDate + "'",
                    //                                   "'" + objUser.LastUpdateDate + "'", "'" + objUser.LastLoginDate + "'", objUser.FailedLoginNo,
                    //                                   isActive, isExpired, objUser.UserId);


                    var quary = string.Format(UPDATE_USER, objUser.CompanyId, objUser.LoginId, objUser.UserName,
                                                                        objUser.Password, "'" + objUser.CreatedDate + "'", "'" + objUser.LastUpdateDate + "'", "'" + objUser.LastLoginDate + "'", objUser.FailedLoginNo,
                                                                       "'" + isActive + "'", "'" + isExpired + "'", objUser.BranchId,objUser.IsNotify, objUser.UserId);

                    SqlDbHelper.ExecuteNonQuery(quary);


                    if (passHistory != null)
                    {
                        var oldPass = passHistory.OldPassword == "" ? objUser.Password : passHistory.OldPassword;
                        var passHisQuary = string.Format(SAVE_PASSWORD_HISTORY, userId, oldPass, DateTime.Now);
                        SqlDbHelper.ExecuteNonQuery(passHisQuary);
                    }

                    SqlDbHelper.CommitTransaction();
                    SqlDbHelper.Close();

                    mes = "Success";
                }
                if (oracleDbHelper != null)
                {
                    if (objUser.IsActive == true)
                    {
                        objUser.FailedLoginNo = 0;
                    }

                    var crdt = objUser.CreatedDate.ToString("dd/MMM/yyyy");
                    crdt = crdt.Replace('/', '-');

                    string currentDate = DateTime.Today.ToString("dd/MMM/yyyy");
                    currentDate = currentDate.Replace('/', '-');

                    var lastlogInDate = "";
                    if (objUser.LastLoginDate == DateTime.MinValue)
                    {
                        lastlogInDate = null;
                    }
                    else
                    {
                        lastlogInDate = objUser.LastLoginDate.ToString("dd/MMM/yyyy");
                        lastlogInDate = lastlogInDate.Replace('/', '-');
                    }

                    int isActive = objUser.IsActive == true ? 1 : 0;
                    int isExpired = objUser.IsExpired == true ? 1 : 0;


                    GetDbHelper();
                    oracleDbHelper.BeginTransaction();
                    var quary = string.Format(UPDATE_USER, objUser.CompanyId, objUser.LoginId, objUser.UserName,
                                              objUser.Password, objUser.EmployeeId, "'" + crdt + "'",
                                              "'" + currentDate + "'", "'" + lastlogInDate + "'", objUser.FailedLoginNo,
                                              isActive, isExpired,objUser.IsNotify, objUser.UserId);
                    oracleDbHelper.ExecuteNonQuery(quary);
                    mes = "Success";
                }

            }
            catch (Exception ex)
            {
                mes = ex.Message;
            }
            return mes;
        }

        public PasswordHistory GetPassHistory(int userId, int passRestriction)
        {
            var passwordHistory = new PasswordHistory();
            GetDbHelper();



            try
            {
                if (SqlDbHelper != null)
                {
                    var quary = string.Format(@"SELECT [HistoryId],[UserId],[OldPassword],[PasswordChangeDate]
                                        FROM [dbo].[PasswordHistory]
                                        WHERE [UserId] = {0} and PasswordChangeDate=(Select MAX(PasswordChangeDate)
                                        From PasswordHistory where [UserId] = {0})", userId);
                    IDataReader reader =
                        SqlDbHelper.GetDataReader(quary);
                    var passwordHistoryDataReader = new PasswordHistoryDataReader(reader);
                    while (reader.Read())
                        passwordHistory = (passwordHistoryDataReader.Read());
                    reader.Close();
                    SqlDbHelper.Close();
                }

            }
            catch (Exception e)
            {
                string message = e.Message;
            }

            return passwordHistory;
        }

        public IQueryable<PasswordHistory> GetPasswordHistory(int userId, int passRestriction)
        {
            var passwordHistory = new List<PasswordHistory>();
            GetDbHelper();

            
            
            try
            {
                if (SqlDbHelper != null)
                {
                    var quary = string.Format(@"SELECT TOP {1} [HistoryId],[UserId],[OldPassword],[PasswordChangeDate]
                                        FROM [dbo].[PasswordHistory]
                                        WHERE [UserId] = {0}
                                        ORDER BY [PasswordChangeDate] DESC", userId, passRestriction);
                    IDataReader reader =
                        SqlDbHelper.GetDataReader(quary);
                    var passwordHistoryDataReader = new PasswordHistoryDataReader(reader);
                    while (reader.Read())
                        passwordHistory.Add(passwordHistoryDataReader.Read());
                    reader.Close();
                    SqlDbHelper.Close();
                }
                else if (oracleDbHelper != null)
                {
                    var quary = string.Format(@"Select * from (
SELECT HistoryId,UserId,OldPassword,PasswordChangeDate
FROM PasswordHistory
WHERE UserId = {0}
ORDER BY PasswordChangeDate DESC) where rownum = 1", userId, passRestriction);
                    IDataReader reader =
                        oracleDbHelper.GetDataReader(quary);
                    var passwordHistoryDataReader = new PasswordHistoryDataReader(reader);
                    while (reader.Read())
                        passwordHistory.Add(passwordHistoryDataReader.Read());
                    reader.Close();
                    oracleDbHelper.Close();
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
            }
            
            return passwordHistory.AsQueryable();
        }

        public string UpdateUser(Users users, PasswordHistory passHistory)
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
                if (SqlDbHelper != null)
                {
                    if (users.LastUpdateDate == DateTime.MinValue)
                    {
                        users.LastUpdateDate = DateTime.Now;
                    }
                    SqlDbHelper.BeginTransaction();
                    // "Update Users set CompanyID = {0}, LoginId='{1}', UserName='{2}', Password='{3}',CreatedDate={4},LastUpdateDate={5},LastLoginDate={6},FailedLoginNo={7},IsActive={8},IsExpired={9},BranchId={10} where UserId={11}";
                    var quary = string.Format(UPDATE_USER, users.CompanyId, users.LoginId, users.UserName,
                                              users.Password, "'" + users.CreatedDate + "'",
                                              "'" + users.LastUpdateDate + "'", "'" + DateTime.Now + "'", users.FailedLoginNo,
                                             "'" + users.IsActive + "'", "'" + users.IsExpired + "'",users.BranchId,users.IsNotify, users.UserId);
                    SqlDbHelper.ExecuteNonQuery(quary);
                    if (passHistory != null)
                    {
                        var passHisQuary = string.Format(SAVE_PASSWORD_HISTORY,passHistory.UserId, passHistory.OldPassword, DateTime.Now);
                        SqlDbHelper.ExecuteNonQuery(passHisQuary);
                    }
                    SqlDbHelper.CommitTransaction();
                    SqlDbHelper.Close();
                    res = "Success";
                }
                else if (oracleDbHelper != null)
                {
                    oracleDbHelper.BeginTransaction();

                    var crdt = users.CreatedDate.ToString("dd/MMM/yyyy");
                    crdt = crdt.Replace('/', '-');

                    string currentDate = DateTime.Today.ToString("dd/MMM/yyyy");
                    currentDate = currentDate.Replace('/', '-');

                    var lastlogInDate = "";
                    if (users.LastLoginDate == DateTime.MinValue)
                    {
                        lastlogInDate = null;
                    }
                    else
                    {
                        lastlogInDate = users.LastLoginDate.ToString("dd/MMM/yyyy");
                        lastlogInDate = lastlogInDate.Replace('/', '-');
                    }

                    int isActive = users.IsActive == true ? 1 : 0;
                    int isExpired = users.IsExpired == true ? 1 : 0;


                    var quary = string.Format(UPDATE_USER, users.CompanyId, users.LoginId, users.UserName,
                                                       users.Password, users.EmployeeId, "'" + crdt + "'",
                                                       "'" + currentDate + "'", "'" + lastlogInDate + "'", users.FailedLoginNo,
                                                       isActive, isExpired, users.UserId);
                    oracleDbHelper.ExecuteNonQuery(quary);
                    if (passHistory != null)
                    {

                        var passHisQuary = string.Format(SAVE_PASSWORD_HISTORY, passHistory.UserId, passHistory.OldPassword, currentDate);
                        oracleDbHelper.ExecuteNonQuery(passHisQuary);
                    }
                    oracleDbHelper.CommitTransaction();
                    oracleDbHelper.Close();
                    res = "Success";
                }
            }
            return res;
        }

        public Users GetUserByEmailAddress(string emailaddress)
        {
            Users objUsers = null;
            GetDbHelper();
            var quary = string.Format(SELECT_USERS_BY_EMAIL, emailaddress);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var usersDataReader = new UsersDataReader(reader);

                if (reader.Read())
                    objUsers = usersDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objUsers;
        }
        public string UpdateTheme(Users user)
        {
            try
            {
                CommonConnection connection = new CommonConnection();
                if (connection.DatabaseType == DatabaseType.Oracle)
                {
                    string sql = string.Format(UPDATE_THEME, user.Theme, user.UserId);
                    connection.ExecuteNonQuery(sql);
                    connection.Close();
                }
                else if (connection.DatabaseType == DatabaseType.SQL)
                {
                    string sql = string.Format(UPDATE_THEME, user.Theme, user.UserId);
                    connection.ExecuteNonQuery(sql);
                    connection.Close();
                }
                else if (connection.DatabaseType == DatabaseType.SQL)
                {
                    string sql = string.Format(UPDATE_THEME, user.Theme, user.UserId);
                    connection.ExecuteNonQuery(sql);
                    connection.Close();

                }
                return "Success";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            
            
        }



        public List<Group> GetUserTypeByUserId(int userId)
        {
            string query = string.Format(@"Select GroupName from Users
left outer join GroupMember on GroupMember.UserId = Users.UserId
left outer join Groups on Groups.GroupId = GroupMember.GroupId
where Users.UserId = {0}", userId);
            var data =  Data<Group>.DataSource(query);
            return data;

        }

        public CompanyBranchInfo GetBranchCodeByCompanyIdAndBranchId(int companyId, int branchId)
        {
            string query = string.Format(@"Select Branch.*,Company.CompanyStock,Company.CompanyType,Company.RootCompanyId,Company.MotherId From Company 
                    left join Branch on Branch.COMPANYID= Company.CompanyId
                  Where  Branch.COMPANYID = {0} and BRANCHID = {1}", companyId, branchId);

            var data = Data<CompanyBranchInfo>.DataSource(query);
            return data.SingleOrDefault();
        }
    }
}
