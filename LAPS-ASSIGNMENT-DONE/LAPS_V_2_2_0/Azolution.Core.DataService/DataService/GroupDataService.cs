using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using Azolution.Common.DataService;
using Azolution.Core.DataService.DataReader;
using Azolution.Entities.Core;
using Utilities;
using Utilities.Common;

namespace Azolution.Core.DataService.DataService
{
    public class GroupDataService
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
                                                         UtilityCommon.BuildWhereClause<Group>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));

                    }
                    else
                    {
                        if (filters[i].Value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                                         UtilityCommon.ToLinqOperator(filter.Logic),
                                                         UtilityCommon.BuildWhereClause<Group>(i, filter.Logic,
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

        //private const string SAVE_GROUP = "INSERT INTO GROUPS(COMPANYID,GROUPNAME)" +
        //    " VALUES ({0}, '{1}') returning GROUPID into :outId";
        //private const string SAVE_GROUP = "INSERT INTO GROUPS(COMPANYID,GROUPNAME)" +
        //    " VALUES ({0}, '{1}')";
        private const string SAVE_GROUP = "SAVE_GROUP";
        private const string SAVE_GROUP_ORACLE = "INSERT INTO GROUPS(COMPANYID,GROUPNAME)" +
           " VALUES ({0}, '{1}') returning GROUPID into :outId";

        private const string SAVE_GROUP_PERMISION = "INSERT INTO GROUPPERMISSION(PERMISSIONTABLENAME,GROUPID,PARENTPERMISSION,REFERENCEID)" +
            " VALUES ('{0}', {1},{2},{3})";

        private const string UPDATE_GROUP = "Update GROUPS set COMPANYID = {0}, GROUPNAME = '{1}',IsDefault={2},IsViewer={3} where GROUPID={4}";

        private const string DELETE_GROUP_PERMISION_BY_GROUPID = "Delete GROUPPERMISSION where GROUPID = {0}";

        private const string SELECT_GROUPPERMISSION_BY_GROUPID = "Select * from GROUPPERMISSION where GROUPID = {0}";

        private const string SELECT_GROUP_BY_COMPANYID = "Select * from GROUPS where COMPANYID = {0}";

        private const string SELECT_ALL_ACCESS_CONTROL = "Select * from ACCESSCONTROL";

        private const string SELECT_ACCESSPERMISSION_BYMODULE_AND_USER =
            "Select distinct * from GroupPermission where GroupId in (Select GroupId from GroupMember where UserId = {1}) and PermissionTableName = 'Access' and ParentPermission = {0}";

        private const string CHECKGROUPBYCONDICTION = "Select * from Groups {0}";

        #endregion


        public string SaveGroup(Group objGroup)
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
                #region Sql Code
                if (SqlDbHelper != null)
                {
                    if (objGroup.GroupId == 0)
                    {

                        var condition = "where CompanyId = "+objGroup.CompanyId+" and ltrim(rtrim(lower(GroupName))) = '" + objGroup.GroupName.ToLower().Trim() + "'";
                        var objExGroup = CheckExistingGroupNameByConditionForSql(condition, SqlDbHelper);
                        if(objExGroup!= null)
                        {
                            return "Exist";
                        }
                        SqlDbHelper.BeginTransaction();
                        //var quary = string.Format(SAVE_GROUP, objGroup.CompanyId, objGroup.GroupName);
                        //SqlDbHelper.ExecuteNonQuery(quary);

                        if(objGroup.IsDefault==1)
                        {
                            var quary = string.Format("Update Groups set IsDefault=0 where CompanyId = {0}",
                                                      objGroup.CompanyId);
                            SqlDbHelper.ExecuteNonQuery(quary);
                        }

                        int groupId = SaveGroupSettings(objGroup, SqlDbHelper);


                        //int groupId = GetMaxGroupIdSql();
                        foreach (var groupPermission in objGroup.ModuleList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = groupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.MenuList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = groupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.AccessList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = groupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.StatusList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = groupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.ActionList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = groupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }

                        SqlDbHelper.CommitTransaction();
                        res = "Success";
                    }
                    else
                    {
                        var condition = "where CompanyId = "+objGroup.CompanyId+" and groupId != "+objGroup.GroupId+" and ltrim(rtrim(lower(GroupName))) = '" + objGroup.GroupName.ToLower().Trim() + "'";
                        var objExGroup = CheckExistingGroupNameByConditionForSql(condition, SqlDbHelper);
                        if (objExGroup != null)
                        {
                            return "Exist";
                        }

                        SqlDbHelper.BeginTransaction();

                        if (objGroup.IsDefault == 1)
                        {
                            var quaryupdate = string.Format("Update Groups set IsDefault=0 where CompanyId = {0}",
                                                      objGroup.CompanyId);
                            SqlDbHelper.ExecuteNonQuery(quaryupdate);
                        }

                        var quary = string.Format(UPDATE_GROUP, objGroup.CompanyId, objGroup.GroupName, objGroup.IsDefault,objGroup.IsViewer, objGroup.GroupId);
                        SqlDbHelper.ExecuteNonQuery(quary);

                        var deleteGroupPermisionQuary = string.Format(DELETE_GROUP_PERMISION_BY_GROUPID,
                                                                      objGroup.GroupId);
                        SqlDbHelper.ExecuteNonQuery(deleteGroupPermisionQuary);

                        foreach (var groupPermission in objGroup.ModuleList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.MenuList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.AccessList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.StatusList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.ActionList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                SqlDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }

                        SqlDbHelper.CommitTransaction();
                        res = "Success";
                    }
                }
                #endregion

                #region Oracle Code
                else if (oracleDbHelper != null)
                {
                    if (objGroup.GroupId == 0)
                    {
                        var condition = "where CompanyId = "+objGroup.CompanyId+" and trim(lower(GroupName)) = '" + objGroup.GroupName.ToLower().Trim() + "'";
                        var objExGroup = CheckExistingGroupNameByConditionForOracle(condition, oracleDbHelper);
                        if (objExGroup != null)
                        {
                            return "Exist";
                        }
                        oracleDbHelper.BeginTransaction();
                        var quary = string.Format(SAVE_GROUP_ORACLE, objGroup.CompanyId, objGroup.GroupName);

                        //oracleDbHelper.ExecuteNonQuery(quary);

                        var con = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;

                        int groupId = 0;//GetMaxGroupId();
                        using (OracleConnection connection = new OracleConnection(con))
                        {
                            connection.Open();

                            OracleCommand command = connection.CreateCommand();
                            OracleTransaction transaction;

                            // Start a local transaction.
                            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                            // Must assign both transaction object and connection
                            // to Command object for a pending local transaction
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

                                groupId = Convert.ToInt32(command.Parameters[0].Value);
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


                        if (groupId != 0)
                        {

                            foreach (var groupPermission in objGroup.ModuleList)
                            {
                                if (groupPermission.ReferenceID != 0)
                                {
                                    groupPermission.GroupId = groupId;
                                    var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                       groupPermission.PermissionTableName,
                                                                       groupPermission.GroupId,
                                                                       groupPermission.ParentPermission,
                                                                       groupPermission.ReferenceID);
                                    oracleDbHelper.ExecuteNonQuery(permisionQuary);
                                }
                            }
                            foreach (var groupPermission in objGroup.MenuList)
                            {
                                if (groupPermission.ReferenceID != 0)
                                {
                                    groupPermission.GroupId = groupId;
                                    var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                       groupPermission.PermissionTableName,
                                                                       groupPermission.GroupId,
                                                                       groupPermission.ParentPermission,
                                                                       groupPermission.ReferenceID);
                                    oracleDbHelper.ExecuteNonQuery(permisionQuary);
                                }
                            }
                            foreach (var groupPermission in objGroup.AccessList)
                            {
                                if (groupPermission.ReferenceID != 0)
                                {
                                    groupPermission.GroupId = groupId;
                                    var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                       groupPermission.PermissionTableName,
                                                                       groupPermission.GroupId,
                                                                       groupPermission.ParentPermission,
                                                                       groupPermission.ReferenceID);
                                    oracleDbHelper.ExecuteNonQuery(permisionQuary);
                                }
                            }
                            foreach (var groupPermission in objGroup.StatusList)
                            {
                                if (groupPermission.ReferenceID != 0)
                                {
                                    groupPermission.GroupId = groupId;
                                    var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                       groupPermission.PermissionTableName,
                                                                       groupPermission.GroupId,
                                                                       groupPermission.ParentPermission,
                                                                       groupPermission.ReferenceID);
                                    oracleDbHelper.ExecuteNonQuery(permisionQuary);
                                }
                            }
                            foreach (var groupPermission in objGroup.ActionList)
                            {
                                if (groupPermission.ReferenceID != 0)
                                {
                                    groupPermission.GroupId = groupId;
                                    var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                       groupPermission.PermissionTableName,
                                                                       groupPermission.GroupId,
                                                                       groupPermission.ParentPermission,
                                                                       groupPermission.ReferenceID);
                                    oracleDbHelper.ExecuteNonQuery(permisionQuary);
                                }
                            }
                        }

                        oracleDbHelper.CommitTransaction();
                        res = "Success";
                    }
                    else
                    {
                        var condition = "where CompanyId = "+objGroup.CompanyId+" and groupId != " + objGroup.GroupId + " and trim(lower(GroupName)) = '" + objGroup.GroupName.ToLower().Trim() + "'";
                        var objExGroup = CheckExistingGroupNameByConditionForOracle(condition, oracleDbHelper);
                        if (objExGroup != null)
                        {
                            return "Exist";
                        }
                        oracleDbHelper.BeginTransaction();
                        var quary = string.Format(UPDATE_GROUP, objGroup.CompanyId, objGroup.GroupName,objGroup.IsDefault, objGroup.GroupId);
                        oracleDbHelper.ExecuteNonQuery(quary);

                        var deleteGroupPermisionQuary = string.Format(DELETE_GROUP_PERMISION_BY_GROUPID,
                                                                      objGroup.GroupId);
                        oracleDbHelper.ExecuteNonQuery(deleteGroupPermisionQuary);

                        foreach (var groupPermission in objGroup.ModuleList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                oracleDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.MenuList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId; 
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                oracleDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.AccessList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                oracleDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.StatusList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                oracleDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }
                        foreach (var groupPermission in objGroup.ActionList)
                        {
                            if (groupPermission.ReferenceID != 0)
                            {
                                groupPermission.GroupId = objGroup.GroupId;
                                var permisionQuary = string.Format(SAVE_GROUP_PERMISION,
                                                                   groupPermission.PermissionTableName,
                                                                   groupPermission.GroupId,
                                                                   groupPermission.ParentPermission,
                                                                   groupPermission.ReferenceID);
                                oracleDbHelper.ExecuteNonQuery(permisionQuary);
                            }
                        }

                        oracleDbHelper.CommitTransaction();
                        res = "Success";
                    }
                }
                #endregion
            }
            return res;
        }

        private Group CheckExistingGroupNameByConditionForOracle(string condition, CommonDbHelper oracleDbHelper)
        {
            var objGroup = new Group();
            objGroup = null;
            try
            {
                var quary = string.Format(CHECKGROUPBYCONDICTION, condition);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var groupDataReader = new GroupDataReader(reader);

                if (reader.Read())
                    objGroup = groupDataReader.Read();
                reader.Close();
            }
            catch (Exception exception)
            {

                throw;
            }
            return objGroup;
        }

        private Group CheckExistingGroupNameByConditionForSql(string condition, DBHelper objSqlDbHelper)
        {
            var objGroup = new Group();
            objGroup = null;
            try
            {
                var quary = string.Format(CHECKGROUPBYCONDICTION, condition);
                IDataReader reader =
                    objSqlDbHelper.GetDataReader(quary);
                var groupDataReader = new GroupDataReader(reader);

                if (reader.Read())
                    objGroup = groupDataReader.Read();
                reader.Close();
            }
            catch (Exception exception)
            {
                
                throw;
            }
            return objGroup;
        }

        private int SaveGroupSettings(Group objGroup, DBHelper SqlDbHelper)
        {
            int groupId = 0;
            try
            {
                var param = new[]
                                {
                                    new SQLParam("@NewgroupId", 0, ParameterDirection.Output),
                                    new SQLParam("@groupId", objGroup.GroupId),
                                    new SQLParam("@companyId", objGroup.CompanyId),
                                    new SQLParam("@groupName", objGroup.GroupName),
                                    new SQLParam("@isDefault", objGroup.IsDefault),
                                    new SQLParam("@isViewer", objGroup.IsViewer)
                                };

                SqlDbHelper.ExecuteNonQuery(SAVE_GROUP, param);
                if (objGroup.GroupId == 0)
                {
                    groupId = (int)param[0].Value;
                }

            }
            catch (Exception ex)
            {
                SqlDbHelper.RollBack();
            }
            return groupId;
        }

        
        private int GetMaxGroupIdSql()
        {
            int groupId = 0;
            string totalSql = "Select Max(GroupId) as Total from Groups";

            IDataReader reader = SqlDbHelper.GetDataReader(totalSql);
            if (reader.Read())
            {
                groupId = reader.GetInt32(0);
            }
            reader.Close();
            return groupId;
        }

        public GridEntity<Group> GetGroupSummaryByCompanyIdWithPaging(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {

            var gridOption = new GridOptions();
            gridOption.skip = skip;
            gridOption.take = take;
            gridOption.page = page;
            gridOption.pageSize = pageSize;
            gridOption.filter = filter;
            gridOption.sort = sort;

            

            string quary = string.Format(@"Select * from Groups where CompanyId={0}", companyId);
            return Kendo<Group>.Grid.DataSource(gridOption, quary, " GroupName ");

            //var groupList = new List<Group>();
            //GetDbHelper();
            //if (SqlDbHelper != null)
            //{
            //    string sql = GetSqlQuaryForGroupSummary(skip, take, page, pageSize, sort, filter,companyId);
            //    IDataReader reader =
            //        SqlDbHelper.GetDataReader(sql);
            //    var groupDataReader = new GroupDataReader(reader);
            //    while (reader.Read())
            //        groupList.Add(groupDataReader.Read());
            //    reader.NextResult();
            //    if (reader.Read())
            //    {
            //        if (groupList.Count > 0)
            //        {
            //            groupList[0].TotalCount = reader.GetInt32(0);
            //        }
            //    }
            //    reader.Close();
            //    SqlDbHelper.Close();

            //}
            //else if (oracleDbHelper != null)
            //{
            //    string sql = GetOracleQuaryForGroupSummary(skip, take, page, pageSize, sort, filter,companyId);
            //    IDataReader reader =
            //       oracleDbHelper.GetDataReader(sql);
            //    var groupDataReader = new GroupDataReader(reader);
            //    while (reader.Read())
            //        groupList.Add(groupDataReader.Read());
            //    reader.Close();
            //    string totalSql = GetOracleQuaryTotalCountForGroupSummary(sort, filter, companyId);

            //    reader = oracleDbHelper.GetDataReader(totalSql);
            //    if (reader.Read())
            //    {
            //        if (groupList.Count > 0)
            //        {
            //            groupList[0].TotalCount = reader.GetInt32(0);
            //        }
            //    }
            //    reader.Close();

            //    oracleDbHelper.Close();
            //}
            //else
            //{
            //    string sql = "";
            //    IDataReader reader =
            //       MySqlDbHelper.GetDataReader(sql);
            //    var groupDataReader = new GroupDataReader(reader);
            //    while (reader.Read())
            //        groupList.Add(groupDataReader.Read());
            //    reader.Close();
            //    MySqlDbHelper.Close();
            //}

            //return groupList;
        }

        private string GetOracleQuaryTotalCountForGroupSummary(List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
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

            var sql = string.Format(@"SELECT COUNT(*) as TotalCount  FROM Groups {0}", condition1);

            return sql;
        }

        private string GetOracleQuaryForGroupSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            string condition = "";

            condition = FilterCondition(filter);

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
                orderby = "ORDER BY GroupName asc";
            }


            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                condition += " and ";
            }
            var pageupperBound = skip + take;
            var sql = string.Format(@"Select * from ( Select ROW_NUMBER() OVER({8}) AS RowIndex, tbl.* from (Select GroupId,companyId, GroupName from Groups) tbl  WHERE {4} CompanyId={7} {8}) where RowIndex >{0} AND RowIndex <= {6}", skip, take, page, pageSize, condition, condition1, pageupperBound,companyId, orderby);

            return sql;
        }

        private string GetSqlQuaryForGroupSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition + " and CompanyId=" + companyId;
                condition += " and CompanyId =" + companyId + " and ";
            }
            else
            {
                condition1 = "where CompanyId=" + companyId;
                condition = "CompanyId =" + companyId + " and ";
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
                orderby = "ORDER BY GroupName,GroupId";
            }

            var pageupperBound = skip + take;
            var sql = string.Format(@"
Select * from (
Select ROW_NUMBER() OVER({7}) AS RowIndex, GroupId,GroupName, CompanyId,IsDefault
from Groups) as tbl
WHERE {4} RowIndex >{0} AND RowIndex <= {6} {7}
SELECT COUNT(*) as TotalCount  FROM Groups {5}", skip, take, page, pageSize, condition, condition1, pageupperBound,orderby);

            return sql;
        }

        public List<GroupPermission> GetGroupPermisionbyGroupId(int groupId)
        {
            var groupPermissionList = new List<GroupPermission>();
            GetDbHelper();
            if (SqlDbHelper != null)
            {
                string sql = string.Format(SELECT_GROUPPERMISSION_BY_GROUPID, groupId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var groupPermissionDataReader = new GroupPermissionDataReader(reader);
                while (reader.Read())
                    groupPermissionList.Add(groupPermissionDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_GROUPPERMISSION_BY_GROUPID, groupId);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var groupPermissionDataReader = new GroupPermissionDataReader(reader);
                while (reader.Read())
                    groupPermissionList.Add(groupPermissionDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_GROUPPERMISSION_BY_GROUPID, groupId);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var groupPermissionDataReader = new GroupPermissionDataReader(reader);
                while (reader.Read())
                    groupPermissionList.Add(groupPermissionDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return groupPermissionList;
        }

        public List<Group> GetGroupByCompanyId(int companyId)
        {
            var groupList = new List<Group>();
            GetDbHelper();
            if (SqlDbHelper != null)
            {
                string sql = string.Format(SELECT_GROUP_BY_COMPANYID, companyId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var groupDataReader = new GroupDataReader(reader);
                while (reader.Read())
                    groupList.Add(groupDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_GROUP_BY_COMPANYID, companyId);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var groupDataReader = new GroupDataReader(reader);
                while (reader.Read())
                    groupList.Add(groupDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_GROUP_BY_COMPANYID, companyId);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var groupDataReader = new GroupDataReader(reader);
                while (reader.Read())
                    groupList.Add(groupDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return groupList;
        }

        public List<AccessControl> GetAllAccess()
        {
            var accessControlList = new List<AccessControl>();
            GetDbHelper();
            if (SqlDbHelper != null)
            {
                string sql = string.Format(SELECT_ALL_ACCESS_CONTROL);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var accessControlDataReader = new AccessControlDataReader(reader);
                while (reader.Read())
                    accessControlList.Add(accessControlDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_ALL_ACCESS_CONTROL);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var accessControlDataReader = new AccessControlDataReader(reader);
                while (reader.Read())
                    accessControlList.Add(accessControlDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_ALL_ACCESS_CONTROL);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var accessControlDataReader = new AccessControlDataReader(reader);
                while (reader.Read())
                    accessControlList.Add(accessControlDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return accessControlList;
        }

        public List<GroupPermission> GetAccessPermisionForCurrentUser(int moduleId, int userId)
        {
            try
            {
                var groupPermissionList = new List<GroupPermission>();
                GetDbHelper();
                if (SqlDbHelper != null)
                {
                    string sql = string.Format(SELECT_ACCESSPERMISSION_BYMODULE_AND_USER, moduleId, userId);
                    IDataReader reader =
                        SqlDbHelper.GetDataReader(sql);
                    var groupPermissionDataReader = new GroupPermissionDataReader(reader);
                    while (reader.Read())
                        groupPermissionList.Add(groupPermissionDataReader.Read());
                    reader.Close();
                    SqlDbHelper.Close();

                }
                else if (oracleDbHelper != null)
                {
                    string sql = string.Format(SELECT_ACCESSPERMISSION_BYMODULE_AND_USER, moduleId, userId);
                    IDataReader reader =
                        oracleDbHelper.GetDataReader(sql);
                    var groupPermissionDataReader = new GroupPermissionDataReader(reader);
                    while (reader.Read())
                        groupPermissionList.Add(groupPermissionDataReader.Read());
                    reader.Close();
                    oracleDbHelper.Close();
                }
                else
                {
                    string sql = string.Format(SELECT_ACCESSPERMISSION_BYMODULE_AND_USER, moduleId, userId);
                    IDataReader reader =
                        MySqlDbHelper.GetDataReader(sql);
                    var groupPermissionDataReader = new GroupPermissionDataReader(reader);
                    while (reader.Read())
                        groupPermissionList.Add(groupPermissionDataReader.Read());
                    reader.Close();
                    MySqlDbHelper.Close();
                }

                return groupPermissionList;
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public Group GetGroupByCondition(string condition)
        {
            var objGroup = new Group();

            var objSqlDbHelper = new DBHelper();
            objGroup = null;
            try
            {
                var quary = string.Format(CHECKGROUPBYCONDICTION, condition);
                IDataReader reader =
                    objSqlDbHelper.GetDataReader(quary);
                var groupDataReader = new GroupDataReader(reader);

                if (reader.Read())
                    objGroup = groupDataReader.Read();
                reader.Close();
            }
            catch (Exception exception)
            {

                throw;
            }
            return objGroup;
        }
    }
}
