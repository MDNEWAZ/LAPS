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
    public class MenuDataService
    {
        private CommonDbHelper oracleDbHelper = null;
        private DBHelper SqlDbHelper = null;
        private OdbcDbHelper MySqlDbHelper = null;

        #region SQL

        private const string SELECT_ALL_MENU = @"Select MenuId,Menu.ModuleId, menuName, menuPath, parentMenu,ModuleName,ToDo,SORORDER,(Select MenuName from Menu mn where mn.MenuId = menu.parentMenu) as ParentMenuName 
from Menu left outer join Module on module.ModuleId = menu.ModuleId
Order by ModuleName asc,ParentMenu asc, SorOrder asc,MenuName asc";
        private const string SELECT_ALL_MENU_BY_MODULEID = "Select * from Menu where ModuleId = {0} order by SorOrder,menuName asc";

        private const string InsertMenuData =
            "INSERT INTO Menu(ModuleID,MenuName,MenuPath,ParentMenu,TODO,SORORDER)" +
            " VALUES ({0}, '{1}', '{2}', {3},{4},{5})";

        private const string UpdateMenuData =
            "Update Menu set ModuleID = {0}, MenuName='{1}', MenuPath = '{2}', ParentMenu = {3}, TODO = {4},SORORDER={5} where MenuID = {6}";

        private const string SELECT_MENU_BY_USERS_PERMISION =
            "SELECT DISTINCT Menu.MenuId,Menu.ModuleId, GroupMember.UserId, GroupPermission.PermissionTableName, Menu.MenuName, Menu.MenuPath, Menu.ParentMenu,SORORDER,ToDo FROM GroupMember INNER JOIN Groups ON GroupMember.GroupId = Groups.GroupId INNER JOIN GroupPermission ON Groups.GroupId = GroupPermission.GroupId INNER JOIN Menu ON GroupPermission.ReferenceID = Menu.MenuId WHERE (GroupMember.UserId ={0}) AND (GroupPermission.PermissionTableName = 'Menu') order by Sororder, Menu.MenuName";

        private const string SELECT_TOLIST = "Select * from Menu where TODO = 1 order by SorOrder,menuName";

        #endregion


        public GridEntity<Menu> GetMenuSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {

            var gridOption = new GridOptions();
            gridOption.skip = skip;
            gridOption.take = take;
            gridOption.page = page;
            gridOption.pageSize = pageSize;
            gridOption.filter = filter;
            gridOption.sort = sort;



            string quary = string.Format(@"Select MenuId,Menu.ModuleId, menuName, menuPath, parentMenu,ModuleName,ToDo,SORORDER,(Select MenuName from Menu mn where mn.MenuId = menu.parentMenu) as ParentMenuName 
from Menu left outer join Module on module.ModuleId = menu.ModuleId");
            return Kendo<Menu>.Grid.DataSource(gridOption, quary, " ModuleName asc,ParentMenu asc, MenuName ");

           
        }

        private string GetOracleQuaryTotalCountForMenuSummary(List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                condition += " and ";
            }

            var sql = string.Format(@"Select Count(*) from ( Select ROW_NUMBER() OVER(ORDER BY MenuId asc) AS RowIndex, tbl.* from (Select MenuId,Menu.ModuleId, menuName, menuPath, parentMenu,ModuleName,ToDo,SORORDER,(Select MenuName from Menu mn where mn.MenuId = menu.parentMenu) as ParentMenuName from Menu left outer join Module on module.ModuleId = menu.ModuleId) tbl {0} )", condition1);

            return sql;
        }

        private string GetSqlQuaryForMenuSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
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
                orderby = "Order by ModuleName asc,ParentMenu asc, MenuName asc";
            }

            var pageupperBound = skip + take;
            var sql = string.Format(@"
Select * from (
Select ROW_NUMBER() OVER({7}) AS RowIndex, MenuId,Menu.ModuleId, menuName, menuPath, parentMenu,ModuleName,ToDo,SORORDER,(Select MenuName from Menu mn where mn.MenuId = menu.parentMenu) as ParentMenuName 
from Menu left outer join Module on module.ModuleId = menu.ModuleId) as tbl
WHERE {4} RowIndex >{0} AND RowIndex <= {6} {7}
SELECT COUNT(*) as TotalCount  FROM Menu {5}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
        }

        private string GetOracleQuaryForMenuSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
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
                orderby = "Order by ModuleName asc,ParentMenu desc, MenuName asc";
            }


            var pageupperBound = skip + take;
            var sql = string.Format(@"Select * from ( Select ROW_NUMBER() OVER({7}) AS RowIndex, tbl.* from (Select MenuId,Menu.ModuleId, menuName, menuPath, parentMenu,ModuleName,ToDo,SORORDER,(Select MenuName from Menu mn where mn.MenuId = menu.parentMenu) as ParentMenuName from Menu left outer join Module on module.ModuleId = menu.ModuleId) tbl {5} {7}) where RowIndex >{0} AND RowIndex <= {6}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
        }

        

        public IQueryable<Menu> SelectAllMenu()
        {
            var menu = new List<Menu>();
            GetDbHelper();

            if (SqlDbHelper != null)
            {
                IDataReader reader = SqlDbHelper.GetDataReader(SELECT_ALL_MENU);
                var menuDataReader = new MenuDataReader(reader);
                while (reader.Read())
                    menu.Add(menuDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader = oracleDbHelper.GetDataReader(SELECT_ALL_MENU);
                var menuDataReader = new MenuDataReader(reader);
                while (reader.Read())
                    menu.Add(menuDataReader.Read());
                reader.Close();

                oracleDbHelper.Close();
            }


            return menu.AsQueryable();
        }

        public IQueryable<Menu> SelectAllMenuByModuleId(int moduleId)
        {
            var menu = new List<Menu>();
            GetDbHelper();
            string quary = string.Format(SELECT_ALL_MENU_BY_MODULEID, moduleId);
            if (SqlDbHelper != null)
            {
                IDataReader reader = SqlDbHelper.GetDataReader(quary);
                var menuDataReader = new MenuDataReader(reader);
                while (reader.Read())
                    menu.Add(menuDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader = oracleDbHelper.GetDataReader(quary);
                var menuDataReader = new MenuDataReader(reader);
                while (reader.Read())
                    menu.Add(menuDataReader.Read());
                reader.Close();

                oracleDbHelper.Close();
            }


            return menu.AsQueryable();
        }

        public string SaveMenu(Menu menu)
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

                var quary = MenuSaveUpdateQuary(menu);
                if (SqlDbHelper != null)
                {
                    try
                    {
                        SqlDbHelper.ExecuteNonQuery(quary);
                        res = "Success";
                    }
                    catch(Exception ex)
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

        public string MenuSaveUpdateQuary(Menu menu)
        {
            string quary = "";
            string parentId = menu.ParentMenu == 0 ? "null" : menu.ParentMenu.ToString();
            if (menu.MenuId == 0)
            {

                var maxOrder = GetMaxOrder();
                menu.SortOrder = maxOrder;
                quary = string.Format(InsertMenuData, menu.ModuleId, menu.MenuName,
                                      menu.MenuPath, parentId,menu.ToDo, menu.SortOrder);
            }
            else
            {
                quary = string.Format(UpdateMenuData, menu.ModuleId, menu.MenuName, menu.MenuPath, parentId, menu.ToDo, menu.SortOrder, menu.MenuId);
            }
            return quary;
        }

        private int GetMaxOrder()
        {
            var maxOrder = 0;
            string totalSql = "Select Max(SorOrder) as SorOrder from Menu";
            if(SqlDbHelper != null)
            {
                

                IDataReader reader = SqlDbHelper.GetDataReader(totalSql);
                if (reader.Read())
                {
                    maxOrder = reader.GetInt32(0) + 1;
                }
                reader.Close();
            }
            if(oracleDbHelper != null)
            {
                IDataReader reader = oracleDbHelper.GetDataReader(totalSql);
                if (reader.Read())
                {
                    maxOrder = reader.GetInt32(0) + 1;
                }
                reader.Close();
            }
            return maxOrder;
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
                                                         UtilityCommon.BuildWhereClause<Company>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));

                    }
                    else
                    {
                        if (filters[i].Value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                                         UtilityCommon.ToLinqOperator(filter.Logic),
                                                         UtilityCommon.BuildWhereClause<Company>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));
                        }
                    }
                }
            }
            return whereClause;
        }

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

        public IQueryable<Menu> SelectMenuByUserPermission(int userId)
        {
            var menu = new List<Menu>();
            GetDbHelper();
            string quary = string.Format(SELECT_MENU_BY_USERS_PERMISION, userId);
            if (SqlDbHelper != null)
            {
                IDataReader reader = SqlDbHelper.GetDataReader(quary);
                var menuDataReader = new MenuDataReader(reader);
                while (reader.Read())
                    menu.Add(menuDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader = oracleDbHelper.GetDataReader(quary);
                var menuDataReader = new MenuDataReader(reader);
                while (reader.Read())
                    menu.Add(menuDataReader.Read());
                reader.Close();

                oracleDbHelper.Close();
            }


            return menu.AsQueryable();
        }

        public List<Menu> GetToDoList()
        {
            var menu = new List<Menu>();
            GetDbHelper();

            if (SqlDbHelper != null)
            {
                IDataReader reader = SqlDbHelper.GetDataReader(SELECT_TOLIST);
                var menuDataReader = new MenuDataReader(reader);
                while (reader.Read())
                    menu.Add(menuDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader = oracleDbHelper.GetDataReader(SELECT_TOLIST);
                var menuDataReader = new MenuDataReader(reader);
                while (reader.Read())
                    menu.Add(menuDataReader.Read());
                reader.Close();

                oracleDbHelper.Close();
            }


            return menu;
        }

        public string UpdateMenuSorting(List<Menu> menuList)
        {
            var res = "";
            GetDbHelper();
            try
            {
                //var quary = " begin";

                var quary = "";
                foreach (var menu in menuList)
                {
                    quary += string.Format(" Update Menu Set SorOrder = {0} where MenuId = {1};", menu.SortOrder,menu.MenuId);
                }

                if(SqlDbHelper!= null)
                {
                    SqlDbHelper.ExecuteNonQuery(quary);
                    res = "Success";
                }

                //quary += " end ;";

                if(oracleDbHelper != null)
                {
                    quary = " begin " + quary + " end ;";


                    oracleDbHelper.ExecuteNonQuery(quary);
                    res = "Success";
                }
            }
            catch (Exception exception)
            {
                res = exception.Message;
            }
            return res;
        }

        public List<Menu> GetParentMenuByMenu(int parentMenuId)
        {


            string query = string.Format(@"Select * from Menu where MenuID = {0}", parentMenuId);

            return Data<Menu>.DataSource(query);

        }
    }
}
