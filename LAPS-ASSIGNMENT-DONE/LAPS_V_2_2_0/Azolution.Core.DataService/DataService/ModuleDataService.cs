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
    public class ModuleDataService
    {
        private CommonDbHelper oracleDbHelper = null;
        private DBHelper SqlDbHelper = null;
        private OdbcDbHelper MySqlDbHelper = null;

        #region 
        private const string MODULE_SELECT_ALL = "Select * from Module";

        private const string InsertModuleData =
            "INSERT INTO Module(ModuleName)" +
            " VALUES ('{0}')";

        private const string UpdateModuleData = "Update Module set ModuleName = '{0}' where ModuleId={1}";

        #endregion

        public GridEntity<Module> GetModuleSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {

            var gridOption = new GridOptions();
            gridOption.skip = skip;
            gridOption.take = take;
            gridOption.page = page;
            gridOption.pageSize = pageSize;
            gridOption.filter = filter;
            gridOption.sort = sort;



            string quary = string.Format(@"Select * from Module ");
            return Kendo<Module>.Grid.DataSource(gridOption, quary, " ModuleName ");
        }
        private string GetOracleQuaryTotalCountForModuleSummary(List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                condition += " and ";
            }

            var sql = string.Format(@"SELECT COUNT(*) as TotalCount  FROM Module {0}", condition1);

            return sql;
        }
        private string GetSqlQuaryForModuleSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
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
                orderby = "ORDER BY ModuleName";
            }


            var pageupperBound = skip + take;
            var sql = string.Format(@"
Select * from (
Select ROW_NUMBER() OVER({7}) AS RowIndex, ModuleId,ModuleName 
from Module) as tbl
WHERE {4} RowIndex >{0} AND RowIndex <= {6} {7}
SELECT COUNT(*) as TotalCount  FROM Module {5}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
        }
        private string GetOracleQuaryForModuleSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            string condition = "";
            //string orderby = "ORDER BY ModuleName";
            string orderby = "";

            condition = FilterCondition(filter);
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

            if(orderby=="")
            {
                orderby = "ORDER BY ModuleName";
            }

            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition;
                //condition += " and ";
            }
            var pageupperBound = skip + take;
            var sql = string.Format(@"Select * from ( Select ROW_NUMBER() OVER({7}) AS RowIndex, tbl.* from (Select ModuleId, ModuleName from Module) tbl {5} {7}) where RowIndex >{0} AND RowIndex <= {6} ", skip, take, page, pageSize, condition, condition1, pageupperBound,orderby);

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
                                                         UtilityCommon.BuildWhereClause<Module>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));

                    }
                    else
                    {
                        if (filters[i].Value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                                         UtilityCommon.ToLinqOperator(filter.Logic),
                                                         UtilityCommon.BuildWhereClause<Module>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));
                        }
                    }
                }
            }
            return whereClause;
        }



        public IQueryable<Module> SelectAllModule()
        {
            var module = new List<Module>();
            GetDbHelper();

            if (SqlDbHelper != null)
            {
                IDataReader reader = SqlDbHelper.GetDataReader(MODULE_SELECT_ALL);
                var moduleDataReader = new ModuleDataReader(reader);
                while (reader.Read())
                    module.Add(moduleDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader = oracleDbHelper.GetDataReader(MODULE_SELECT_ALL);
                var moduleDataReader = new ModuleDataReader(reader);
                while (reader.Read())
                    module.Add(moduleDataReader.Read());
                reader.Close();

                oracleDbHelper.Close();
            }


            return module.AsQueryable();
        }

        public string SaveModule(Module module) {
            var res = "";

            GetDbHelper();
            if (SqlDbHelper == null && oracleDbHelper == null && MySqlDbHelper == null)
            {
                res = "Please Configure Database type";
                return res;
            }
             else
            {

                var quary = ModuleSaveUpdateQuary(module);
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

        private string ModuleSaveUpdateQuary(Module module)
        {
        
            string quary = "";
            
            if (module.ModuleId == 0)
            {


                quary = string.Format(InsertModuleData,module.ModuleName);
            }
            else
            {
                quary = string.Format(UpdateModuleData, module.ModuleName, module.ModuleId);
            }
            return quary;
        
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
    }
}
