using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Azolution.Common.DataService;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataReader;
using Utilities;
using Utilities.Common;

namespace Azolution.HumanResource.DataService.DataService
{
    public class DesignationDataService 
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
                                                         UtilityCommon.BuildWhereClause<Designation>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));

                    }
                    else
                    {
                        if (filters[i].Value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                                         UtilityCommon.ToLinqOperator(filter.Logic),
                                                         UtilityCommon.BuildWhereClause<Designation>(i, filter.Logic,
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




        private const string INSERT_DESIGNATION_DATA =
            "Insert into DESIGNATION (DESIGNATIONNAME,COMPANYID,STATUS,DEPARTMENTID) values ('{0}',{1},{2},{3})";

        private const string UPDATE_DESIGNATION_DATA =
            "Update DESIGNATION set DESIGNATIONNAME='{1}', COMPANYID = '{2}',STATUS={3},DEPARTMENTID={4} where DESIGNATIONID={0}";

        private const string SELECT_DESIGNATION =
            "Select DESIGNATIONID,DESIGNATIONNAME,COMPANYID,STATUS FROM DESIGNATION {0}";

        private const string SELECT_DESIGNATION_BY_COMPANYID = "Select DESIGNATIONID,DESIGNATIONNAME,COMPANYID,STATUS FROM DESIGNATION Where COMPANYID={0} ";
        private const string SELECT_DESIGNATION_BY_COMPANYID_AND_STATUS = "Select DESIGNATIONID,DESIGNATIONNAME,COMPANYID,STATUS FROM DESIGNATION Where COMPANYID={0} AND STATUS={1} ";
        

        #endregion
        public string SaveDesignation(Designation objDesignation)
       {
           var res = "";
           //if (objDesignation != null && objDesignation.DesignationId == 0)
           //{
           var desig = IsDesignationExists(objDesignation);
           if (desig != null)
           {
               return "Already Exist";
           }

               GetDbHelper();
               objDesignation.DesignationName = objDesignation.DesignationName.Replace("'", "''");

               if (SqlDbHelper == null && oracleDbHelper == null && MySqlDbHelper == null)
               {
                   res = "Please Configure Database type";
                   return res;
               }
               else
               {

                   var quary = GetQueryString(objDesignation);

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
           //}
           //else
           //    return "Nothing to Save";

            return res;
       }

        private string GetQueryString(Designation objDesignation)
        {
            string quary = "";

            if (objDesignation.DesignationId == 0)
            {
                quary = string.Format(INSERT_DESIGNATION_DATA, objDesignation.DesignationName, objDesignation.CompanyId,objDesignation.Status,objDesignation.DepartmentId);
            }
            else
            {
                quary = string.Format(UPDATE_DESIGNATION_DATA, objDesignation.DesignationId, objDesignation.DesignationName, objDesignation.CompanyId,objDesignation.Status,objDesignation.DepartmentId);

            }
            return quary;
        }

        private Designation IsDesignationExists(Designation aDesignation)
        {
            Designation objDesignation = null;
            GetDbHelper();

            var quary = "";
            var condition = "";
            if (SqlDbHelper != null)
            {
                if (aDesignation.DesignationId == 0)
                {
                    condition = string.Format("WHERE rtrim(ltrim(Lower(DESIGNATIONNAME))) ='{0}' AND COMPANYID={1}",
                                                  aDesignation.DesignationName.ToLower().Trim(), aDesignation.CompanyId);
                }
                else
                {
                    condition = string.Format("WHERE rtrim(ltrim(Lower(DESIGNATIONNAME))) ='{0}' AND COMPANYID={1} and DESIGNATIONID != {2}",
                                                  aDesignation.DesignationName.ToLower().Trim(), aDesignation.CompanyId, aDesignation.DesignationId);
                }
                quary = string.Format(SELECT_DESIGNATION, condition);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var departmentDataReader = new DesignationDataReader(reader);

                if (reader.Read())
                    objDesignation = departmentDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                if (aDesignation.DesignationId == 0)
                {
                    condition = string.Format("WHERE trim(LOWER(DESIGNATIONNAME)) ='{0}' AND COMPANYID={1}",
                                              aDesignation.DesignationName.ToLower().Trim(), aDesignation.CompanyId);
                }
                else
                {
                    condition = string.Format("WHERE trim(LOWER(DESIGNATIONNAME)) ='{0}' AND COMPANYID={1} and DESIGNATIONID != {2}",
                                              aDesignation.DesignationName.ToLower().Trim(), aDesignation.CompanyId, aDesignation.DesignationId);
                }
                quary = string.Format(SELECT_DESIGNATION, condition);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var departmentDataReader = new DesignationDataReader(reader);

                if (reader.Read())
                    objDesignation = departmentDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objDesignation;
        }

        public List<Designation> GetDesignationByCompanyId(int companyId)
        {
            var designationList = new List<Designation>();
            GetDbHelper();


            //WHERE     (CompanyID = @companyId)))) AND (dbo.Employee.StateId NOT IN (3, 4))";")

            if (SqlDbHelper != null)
            {


                string sql = string.Format(SELECT_DESIGNATION_BY_COMPANYID, companyId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var designationDataReader = new DesignationDataReader(reader);
                while (reader.Read())
                    designationList.Add(designationDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_DESIGNATION_BY_COMPANYID, companyId);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var designationDataReader = new DesignationDataReader(reader);
                while (reader.Read())
                    designationList.Add(designationDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_DESIGNATION_BY_COMPANYID, companyId);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var designationDataReader = new DesignationDataReader(reader);
                while (reader.Read())
                    designationList.Add(designationDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return designationList;
        }
        
        public List<Designation> GetAllDesignationByCompanyIdAndStatus(int companyId, int status)
        {
            var designationList = new List<Designation>();
            GetDbHelper();


            
            if (SqlDbHelper != null)
            {


                string sql = string.Format(SELECT_DESIGNATION_BY_COMPANYID_AND_STATUS, companyId,status);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var designationDataReader = new DesignationDataReader(reader);
                while (reader.Read())
                    designationList.Add(designationDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_DESIGNATION_BY_COMPANYID_AND_STATUS, companyId,status);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var designationDataReader = new DesignationDataReader(reader);
                while (reader.Read())
                    designationList.Add(designationDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_DESIGNATION_BY_COMPANYID, companyId);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var designationDataReader = new DesignationDataReader(reader);
                while (reader.Read())
                    designationList.Add(designationDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return designationList;
        }

        public GridEntity<Designation> GetDesignationSummary(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {
            var gridOption = new GridOptions();
            gridOption.skip = skip;
            gridOption.take = take;
            gridOption.page = page;
            gridOption.pageSize = pageSize;
            gridOption.filter = filter;
            gridOption.sort = sort;



            string quary = string.Format(@"Select * from Designation where CompanyId={0}", companyId);
            return Kendo<Designation>.Grid.DataSource(gridOption, quary, " DESIGNATIONNAME ");

            //var designationList = new List<Designation>();
            //GetDbHelper();
            //if (SqlDbHelper != null)
            //{
            //    string sql = GetSqlQuaryForDesignationSummary(skip, take, page, pageSize, sort, filter, companyId);
            //    IDataReader reader =
            //        SqlDbHelper.GetDataReader(sql);
            //    var designationDataReader = new DesignationDataReader(reader);
            //    while (reader.Read())
            //        designationList.Add(designationDataReader.Read());
            //    reader.NextResult();
            //    if (reader.Read())
            //    {
            //        if (designationList.Count > 0)
            //        {
            //            designationList[0].TotalCount = reader.GetInt32(0);
            //        }
            //    }
            //    reader.Close();
            //    SqlDbHelper.Close();

            //}
            //else if (oracleDbHelper != null)
            //{
            //    string sql = GetOracleQuaryForDesignationSummary(skip, take, page, pageSize, sort, filter, companyId);
            //    IDataReader reader =
            //       oracleDbHelper.GetDataReader(sql);
            //    var designationDataReader = new DesignationDataReader(reader);
            //    while (reader.Read())
            //        designationList.Add(designationDataReader.Read());
            //    reader.Close();
            //    string totalSql = GetOracleQuaryTotalCountForUserSummary(sort, filter, companyId);

            //    reader = oracleDbHelper.GetDataReader(totalSql);
            //    if (reader.Read())
            //    {
            //        if (designationList.Count > 0)
            //        {
            //            designationList[0].TotalCount = reader.GetInt32(0);
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
            //    var designationDataReader = new DesignationDataReader(reader);
            //    while (reader.Read())
            //        designationList.Add(designationDataReader.Read());
            //    reader.Close();
            //    MySqlDbHelper.Close();
            //}

            //return designationList;
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

            var sql = string.Format(@"SELECT COUNT(*) as TotalCount  FROM Designation {0}", condition1);

            return sql;
        }

        private string GetOracleQuaryForDesignationSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
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
                orderby = "ORDER BY DESIGNATIONNAME";
            }

            var pageupperBound = skip + take;
            var sql = string.Format(@"Select * from ( Select ROW_NUMBER() OVER({8}) AS RowIndex, tbl.* from (Select DESIGNATIONID, COMPANYID, DESIGNATIONNAME, STATUS,DEPARTMENTID FROM  Designation) tbl  WHERE {4} CompanyId={7} {8}) where RowIndex >{0} AND RowIndex <= {6} ", skip, take, page, pageSize, condition, condition1, pageupperBound, companyId, orderby);

            return sql;
        }

        private string GetSqlQuaryForDesignationSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            string condition = "";

            condition = FilterCondition(filter);
            var condition1 = "";
            if (condition != "")
            {
                condition1 = "where " + condition + " and CompanyId=" + companyId;
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
                orderby = "ORDER BY DESIGNATIONNAME";
            }

            var pageupperBound = skip + take;
            var sql = string.Format(@"
Select * from (
Select ROW_NUMBER() OVER(ORDER BY DESIGNATIONID asc) AS RowIndex, DESIGNATIONID, COMPANYID, DESIGNATIONNAME, STATUS,DEPARTMENTID
FROM  Designation) as tbl
WHERE {4} RowIndex >{0} AND RowIndex <= {6} {7}
SELECT COUNT(*) as TotalCount  FROM Designation {5}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
        }

        public List<Designation> GenerateDesignationByDepartmentIdCombo(int departmentId, int status)
        {
            string quary = string.Format(@"Select * from DESIGNATION where DEPARTMENTID={0} and STATUS={1}",
                                         departmentId, status);

            return Kendo<Designation>.Combo.DataSource(quary);
        }
    }
}


