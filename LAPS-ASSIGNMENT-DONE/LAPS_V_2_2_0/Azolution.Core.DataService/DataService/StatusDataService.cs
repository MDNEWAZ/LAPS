using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Azolution.Common.DataService;
using Azolution.Core.DataService.DataReader;
using Azolution.Entities.Core;
using Utilities;
using Utilities.Common;

namespace Azolution.Core.DataService.DataService
{
    public class StatusDataService 
    {

        //public StatusDataService()
        //{
        //    GetDbHelper();
        //}

        #region Global Variable

        private CommonDbHelper oracleDbHelper = null;
        private DBHelper SqlDbHelper = null;
        private OdbcDbHelper MySqlDbHelper = null;

        private const string ACCESSCONTROL_SELECT_ALL = "Select * from AccessControl";

        private const string InsertAccessControlData =
            "INSERT INTO AccessControl(AccessName)" +
            " VALUES ('{0}')";

        private const string UpdateAccessControlData = "Update AccessControl set AccessName = '{0}' where AccessId={1}";

        #endregion

        #region SQL

        private const string SELECT_STATUS_BY_MENUID = "Select * from WFState where MenuId = {0}";

        private const string SELECT_ACTION_BY_STATUSID = "Select *, (Select StateName from WFState where WFStateId = NextStateId) as NEXTSTATENAME from WFAction where WFStateId = {0}";


        private const string SELECT_DEFAULT_STATUS_BY_MENUID =
            "Select * from WfState where menuId = {0} and IsDefaultStart = 1";

        private const string GETACTION_BYSTATE_AND_USERID =
            "Select distinct * from WFAction where WfActionId in (Select ReferenceId from GroupPermission where PermissionTableName = 'Action' and ParentPermission = {0} and GroupId in (Select GroupId from GroupMember where UserId = {1}))";

        private const string SELECT_APPROVED_STATUS_BY_MENUID =
            "Select * from WfState where menuId = {0} and ISCLOSED = 2";


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


        public List<WFState> GetStatusByMenuId(int menuId)
        {
            var wFStateList = new List<WFState>();
            GetDbHelper();
            if (SqlDbHelper != null)
            {
                string sql = string.Format(SELECT_STATUS_BY_MENUID, menuId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var wFStateDataReader = new WFStateDataReader(reader);
                while (reader.Read())
                    wFStateList.Add(wFStateDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_STATUS_BY_MENUID, menuId);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var wFStateDataReader = new WFStateDataReader(reader);
                while (reader.Read())
                    wFStateList.Add(wFStateDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_STATUS_BY_MENUID, menuId);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var wFStateDataReader = new WFStateDataReader(reader);
                while (reader.Read())
                    wFStateList.Add(wFStateDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return wFStateList;
        }

        public List<WFAction> GetActionByStatusId(int statusId)
        {
            var wFActionList = new List<WFAction>();
            GetDbHelper();
            if (SqlDbHelper != null)
            {
                string sql = string.Format(SELECT_ACTION_BY_STATUSID, statusId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var wFActionDataReader = new WFActionDataReader(reader);
                while (reader.Read())
                    wFActionList.Add(wFActionDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_ACTION_BY_STATUSID, statusId);
                sql =
                    string.Format(
                        @"select wa.WFACTIONID,wa.WFSTATEID,wa.ACTIONNAME,wa.NEXTSTATEID,wf1.STATENAME,wf.STATENAME NEXTSTATENAME,
                            wa.EMAIL_ALERT,wa.SMS_ALERT from WFACTION wa,WFSTATE wf,WFSTATE wf1 
                            where wa.NEXTSTATEID = wf.WFSTATEID 
                            and wf1.WFStateId = wa.WFStateId
                            and wa.WFStateId = {0}", statusId);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var wFActionDataReader = new WFActionDataReader(reader);
                while (reader.Read())
                    wFActionList.Add(wFActionDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_ACTION_BY_STATUSID, statusId);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var wFActionDataReader = new WFActionDataReader(reader);
                while (reader.Read())
                    wFActionList.Add(wFActionDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return wFActionList;
        }

        public GridEntity<AccessControl> GetAccessControlSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            var gridOption = new GridOptions();
            gridOption.skip = skip;
            gridOption.take = take;
            gridOption.page = page;
            gridOption.pageSize = pageSize;
            gridOption.filter = filter;
            gridOption.sort = sort;
            string quary = string.Format(@"Select AccessId,AccessName from AccessControl");
            return Kendo<AccessControl>.Grid.DataSource(gridOption, quary, " AccessName ");
        }

        private string GetOracleQuaryTotalCountForAccessControlSummary(List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                condition += " and ";
            }

            var sql = string.Format(@"SELECT COUNT(*) as TotalCount  FROM AccessControl {0}", condition1);

            return sql;
        }


        public string GetSqlQuaryForAccessControlSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                //condition += " and ";
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
                orderby = "ORDER BY AccessName";
            }

            var pageupperBound = skip + take;
            var sql = string.Format(@"
Select * from (
Select ROW_NUMBER() OVER({7}) AS RowIndex, tbl.* from (Select AccessId,AccessName 
from AccessControl) as tbl {4}) as tbl where RowIndex >{0} AND RowIndex <= {6} {7}
SELECT COUNT(*) as TotalCount  FROM AccessControl {4}", skip, take, page, pageSize, condition1, condition, pageupperBound, orderby);

            return sql;
        }

        private string GetOracleQuaryForAccessControlSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
               // condition += " and ";
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
                orderby = "ORDER BY AccessName";
            }


            var pageupperBound = skip + take;
            var sql = string.Format(@"Select * from ( Select ROW_NUMBER() OVER({7}) AS RowIndex, tbl.* from (Select AccessId, AccessName from AccessControl) tbl {5} {7}) where RowIndex >{0} AND RowIndex <= {6}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
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
                                                         UtilityCommon.BuildWhereClause<AccessControl>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));

                    }
                    else
                    {
                        if (filters[i].Value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                                         UtilityCommon.ToLinqOperator(filter.Logic),
                                                         UtilityCommon.BuildWhereClause<AccessControl>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));
                        }
                    }
                }
            }
            return whereClause;
        }

        public IQueryable<AccessControl> SelectAllAccessControl()
        {
            var accessControl = new List<AccessControl>();
            GetDbHelper();

            if (SqlDbHelper != null)
            {
                IDataReader reader = SqlDbHelper.GetDataReader(ACCESSCONTROL_SELECT_ALL);
                var accessControlDataReader = new AccessControlDataReader(reader);
                while (reader.Read())
                    accessControl.Add(accessControlDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader = oracleDbHelper.GetDataReader(ACCESSCONTROL_SELECT_ALL);
                var accessControlDataReader = new AccessControlDataReader(reader);
                while (reader.Read())
                    accessControl.Add(accessControlDataReader.Read());
                reader.Close();

                oracleDbHelper.Close();
            }


            return accessControl.AsQueryable();
        }



        public string SaveAccessControl(AccessControl accessControl)
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

                var quary = AccessControlSaveUpdateQuary(accessControl);
                if (SqlDbHelper != null)
                {
                    try
                    {
                        SqlDbHelper.ExecuteNonQuery(quary);
                        res = "Success";
                    }
                    catch (Exception ex)
                    {
                        res = ex.Message;
                        //SqlDbHelper.RollBack();
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
                        oracleDbHelper.ExecuteNonQuery(quary);
                        res = "Success";
                    }
                    catch (Exception ex)
                    {
                        res = ex.Message;
                        //oracleDbHelper.RollBack();
                    }
                    finally
                    {
                        oracleDbHelper.Close();
                    }
                }
            }

            return res;
        }


        private string AccessControlSaveUpdateQuary(AccessControl accessControl)
        {

            string quary = "";

            if (accessControl.AccessId == 0)
            {


                quary = string.Format(InsertAccessControlData, accessControl.AccessName);
            }
            else
            {
                quary = string.Format(UpdateAccessControlData, accessControl.AccessName, accessControl.AccessId);
            }
            return quary;

        }

        public WFState GetDefaultStatusByMenuId(int menuId)
        {
            WFState objWFState = null;
            GetDbHelper();
            var quary = string.Format(SELECT_DEFAULT_STATUS_BY_MENUID, menuId);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var wFStateDataReader = new WFStateDataReader(reader);

                if (reader.Read())
                    objWFState = wFStateDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var wFStateDataReader = new WFStateDataReader(reader);

                if (reader.Read())
                    objWFState = wFStateDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objWFState;
        }

        public WFState GetApproveStatusByMenuId(int menuId)
        {
            WFState objWFState = null;
            GetDbHelper();
            var quary = string.Format(SELECT_APPROVED_STATUS_BY_MENUID, menuId);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var wFStateDataReader = new WFStateDataReader(reader);

                if (reader.Read())
                    objWFState = wFStateDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var wFStateDataReader = new WFStateDataReader(reader);

                if (reader.Read())
                    objWFState = wFStateDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objWFState;
        }

        public List<WFAction> GetActionByStateIdAndUserId(int stateId, int userId)
        {
            var wFActionList = new List<WFAction>();
            GetDbHelper();

            if (SqlDbHelper != null)
            {
                string quary = string.Format(GETACTION_BYSTATE_AND_USERID, stateId, userId);
                IDataReader reader = SqlDbHelper.GetDataReader(quary);
                var wFActionDataReader = new WFActionDataReader(reader);
                while (reader.Read())
                    wFActionList.Add(wFActionDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                string quary = string.Format(GETACTION_BYSTATE_AND_USERID, stateId, userId);
                IDataReader reader = oracleDbHelper.GetDataReader(quary);
                var wFActionDataReader = new WFActionDataReader(reader);
                while (reader.Read())
                    wFActionList.Add(wFActionDataReader.Read());
                reader.Close();

                oracleDbHelper.Close();
            }


            return wFActionList;
        }




        #region Work Flow


        public GridEntity<WFState> GetWorkFlowSummary(int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {
            var gridOption = new GridOptions();
            gridOption.skip = skip;
            gridOption.take = take;
            gridOption.page = page;
            gridOption.pageSize = pageSize;
            gridOption.filter = filter;
            gridOption.sort = sort;



            string quary = string.Format(@"Select a.WFSTATEID,a.STATENAME,a.MENUID,a.ISDEFAULTSTART,a.ISCLOSED,m.MODULEID,m.MENUNAME,md.MODULENAME 
from WFSTATE a,MENU m,MODULE md where a.MENUID = m.MENUID and m.MODULEID = md.MODULEID");
            return Kendo<WFState>.Grid.DataSource(gridOption, quary, " MENUNAME,ISDEFAULTSTART,STATENAME asc ");
            
        }

        private string GetSqlQuaryForWorkFlowSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                condition += " and ";
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
                orderby = "ORDER BY MENUNAME,ISDEFAULTSTART,STATENAME asc";
            }


            var pageupperBound = skip + take;
            var sql = string.Format(@"
Select * from (
Select ROW_NUMBER() OVER({7}) AS RowIndex, a.WFSTATEID,a.STATENAME,a.MENUID,a.ISDEFAULTSTART,a.ISCLOSED,m.MODULEID,m.MENUNAME,md.MODULENAME 
from WFSTATE a,MENU m,MODULE md where a.MENUID = m.MENUID and m.MODULEID = md.MODULEID) as tbl
WHERE {4} RowIndex >{0} AND RowIndex <= {6} {7}
SELECT COUNT(*) as TotalCount  FROM (Select a.WFSTATEID,a.STATENAME,a.MENUID,a.ISDEFAULTSTART,a.ISCLOSED,
	                   m.MODULEID,m.MENUNAME,md.MODULENAME 
	   
	                    from WFSTATE a,MENU m,MODULE md where a.MENUID = m.MENUID and m.MODULEID = md.MODULEID) as tbl {5}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
        }

        private string GetOracleQuaryTotalCountForWorkFlowSummary(List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                //condition1 += " and CompanyId=" + companyId;
            }
            else
            {
                //condition1 = "where CompanyId=" + companyId;
            }

            var sql = string.Format(@"SELECT COUNT(*) as TotalCount  FROM (Select * from (Select a.WFSTATEID,a.STATENAME,a.MENUID,a.ISDEFAULTSTART,a.ISCLOSED,
	                   m.MODULEID,m.MENUNAME,md.MODULENAME 
	   
	                    from WFSTATE a,MENU m,MODULE md where a.MENUID = m.MENUID and m.MODULEID = md.MODULEID) tbl {0})", condition1);

            return sql;
        }

        private string GetOracleQuaryForWorkFlowSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                //condition += " and ";
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
                orderby = "ORDER BY MENUNAME,ISDEFAULTSTART,STATENAME asc";
            }

            var pageupperBound = skip + take;
            var sql = string.Format(@"Select * from ( Select ROW_NUMBER() OVER({7}) AS RowIndex, tbl.* from (

	                   Select a.WFSTATEID,a.STATENAME,a.MENUID,a.ISDEFAULTSTART,a.ISCLOSED,
	                   m.MODULEID,m.MENUNAME,md.MODULENAME 
	   
	                    from WFSTATE a,MENU m,MODULE md where a.MENUID = m.MENUID and m.MODULEID = md.MODULEID
	   
                ) tbl  {5} {7}) where RowIndex >{0} AND RowIndex <= {6}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
        }

        public List<Menu> GetMenu()
        {
            string sql = "Select MENUID,MENUNAME from MENU";
            List<Menu> menuList = new List<Menu>();
            GetDbHelper();


            if (oracleDbHelper != null)
            {
                DataTable dt = oracleDbHelper.GetTable(sql);

                if(dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Menu m = new Menu();

                        m.MenuId = Convert.ToInt32("0" + dr["MENUID"]);
                        m.MenuName = Convert.ToString(dr["MENUNAME"]);
                        menuList.Add(m);
                    }
                }
            }

            return menuList;
        }

        public string SaveState(WFState state)
        {
            string sql = "";
            string rv = "";
            int isDefault = state.IsDefaultStart ? 1 : 0;
            GetDbHelper();

            if(state.WFStateId > 0)
            {
                //update
                sql =
                    string.Format(
                        @"Update WFSTATE set STATENAME='{0}',MENUID={1}, ISDEFAULTSTART={2}, ISCLOSED={3} where WFSTATEID = {4}",
                        state.StateName, state.MenuID, isDefault, state.IsClosed, state.WFStateId);
            }
            else
            {
                //insert new
                sql =
                    string.Format(
                        @"insert into WFSTATE(STATENAME,MENUID,ISDEFAULTSTART,ISCLOSED) values('{0}',{1},{2},{3})",
                        state.StateName, state.MenuID, isDefault, state.IsClosed);
            }

            if (oracleDbHelper != null)
            {
                oracleDbHelper.BeginTransaction();
                if (state.IsDefaultStart)
                {
                    string isDefaultUpdate = string.Format(@"update WFSTATE set ISDEFAULTSTART = 0 where MENUID = {0}",
                                                           state.MenuID);
                    oracleDbHelper.ExecuteNonQuery(isDefaultUpdate);
                }
                oracleDbHelper.ExecuteNonQuery(sql);
                oracleDbHelper.CommitTransaction();
                rv = "Success";
            }
            if (SqlDbHelper != null)
            {
                SqlDbHelper.BeginTransaction();
                if (state.IsDefaultStart)
                {
                    string isDefaultUpdate = string.Format(@"update WFSTATE set ISDEFAULTSTART = 0 where MENUID = {0}",
                                                           state.MenuID);
                    SqlDbHelper.ExecuteNonQuery(isDefaultUpdate);
                }
                SqlDbHelper.ExecuteNonQuery(sql);
                SqlDbHelper.CommitTransaction();
                rv = "Success";
            }


            return rv;
        }

        public string SaveAction(WFAction action)
        {
            string sql = "";
            string rv = "";
            //int isDefault = state.IsDefaultStart ? 1 : 0;
            GetDbHelper();

            if (action.WFStateId <= 0)
                return "State ID should be grater than zero";
            if (action.WFActionId > 0)
            {
                //update
                sql =
                    string.Format(
                        @"Update WFACTION set WFSTATEID={0},ACTIONNAME='{1}', NEXTSTATEID={2}, EMAIL_ALERT={3}, SMS_ALERT={4} where WFACTIONID = {5}",
                        action.WFStateId, action.ActionName, action.NextStateId, action.EmailNotification, action.SMSNotification, action.WFActionId);
            }
            else
            {
                //insert new
                sql =
                    string.Format(
                        @"insert into WFACTION(WFSTATEID,ACTIONNAME,NEXTSTATEID,EMAIL_ALERT,SMS_ALERT) values({0},'{1}',{2},{3},{4})",
                        action.WFStateId, action.ActionName, action.NextStateId, action.EmailNotification,
                        action.SMSNotification);
            }

            if (oracleDbHelper != null)
            {
                oracleDbHelper.ExecuteNonQuery(sql);
                rv = "Success";
            }
            if (SqlDbHelper != null)
            {
                SqlDbHelper.ExecuteNonQuery(sql);
                rv = "Success";
            }


            return rv;
        }

        public string DeleteStatusByActionId(int actionId)
        {
            var res = "";
            try
            {
                GetDbHelper();

                var quary = "Delete WfAction where WfactionId=" + actionId;

                if(SqlDbHelper!= null)
                {
                    SqlDbHelper.ExecuteNonQuery(quary);
                    SqlDbHelper.Close();
                    res = "Success";
                }
                else if(oracleDbHelper != null)
                {
                    oracleDbHelper.ExecuteNonQuery(quary);
                    oracleDbHelper.Close();
                    res = "Success";
                }
            }
            catch (Exception exception)
            {
                res = exception.Message;
            }
            return res;
        }

        
        #endregion

        public GridEntity<AccessControl> GetAccessControlSummary2(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            // string quary = string.Format(@"Select AccessId,AccessName from AccessControl");
            //var data = Data<AccessControl>.DataSource(quary);
            //return data;
           var obj= GetAccessSummary(skip, take, page, pageSize, sort, filter);
            var obj1 = new GridEntity<AccessControl>();
            obj1.Items = obj;
            obj1.TotalCount = obj.Count;
            
           return obj1;
        }

        public List<AccessControl> GetAccessSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            var accessControlList = new List<AccessControl>();
            GetDbHelper();
            if (SqlDbHelper != null)
            {
                string sql = GetSqlQuaryForAccessControlSummary(skip, take, page, pageSize, sort, filter);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var accessDataReader = new AccessControlDataReader(reader);
                while (reader.Read())
                    accessControlList.Add(accessDataReader.Read());
                reader.NextResult();
                if (reader.Read())
                {
                    if (accessControlList.Count > 0)
                    {
                        accessControlList[0].TotalCount = reader.GetInt32(0);
                    }
                }
                reader.Close();
                SqlDbHelper.Close();


            }
            return accessControlList;
        } 
    }
}
