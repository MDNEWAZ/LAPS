using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Azolution.Common.DataService;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataReader;
using Utilities;
using Utilities.Common;

namespace Azolution.HumanResource.DataService.DataService
{
    public class DepartmentDataService 
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
                                                         UtilityCommon.BuildWhereClause<Department>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));

                    }
                    else
                    {
                        if (filters[i].Value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                                         UtilityCommon.ToLinqOperator(filter.Logic),
                                                         UtilityCommon.BuildWhereClause<Department>(i, filter.Logic,
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

        private const string InsertDepartmentData = "Insert into Department (DepartmentName,CompanyId,DepartmentHeadId,IsActive) values ('{0}', {1}, {2},{3})";

        private const string UpdateDepartmentData =
            "Update Department set DepartmentName='{0}', CompanyId={1}, DepartmentHeadId={2},IsActive={3} where DepartmentId = {4}";

        private const string SELECT_DEPARTMENT_BY_COMPANYID = "Select * from Department where CompanyId = {0}";

        private const string CHECK_DEPARTMENT_BY_CONDITION = "Select * from Department {0}";

        #endregion

        #region Method

        public string SaveDepartment(Department department)
        {
            var res = "";
            var connectionType = ConfigurationSettings.AppSettings["DataBaseType"];
            if(department.DepartmentId == 0)
            {
                string condition = "";

                if (connectionType == "SQL")
                {
                    condition = "Where CompanyId = " + department.CompanyId + " and rtrim(ltrim(Lower(DepartmentName)))='" + department.DepartmentName.ToLower().Trim() + "'";
                }
                else if (connectionType == "Oracle")
                {
                    condition = "Where CompanyId = " + department.CompanyId + " and Trim(Lower(DepartmentName))='" + department.DepartmentName.ToLower().Trim() + "'";
                }

                
                var dep = CheckDepartmentExistByCondition(condition);
                if (dep != null)
                {
                    return "Department Already Exist";
                }
            }
            else
            {
                string condition = "";
                if (connectionType == "SQL")
                {
                    condition = "Where DepartmentId != " + department.DepartmentId + " and CompanyId = " + department.CompanyId + " and rtrim(ltrim(Lower(DepartmentName)))='" + department.DepartmentName.ToLower().Trim() + "'";
                }
                else if (connectionType == "Oracle")
                {
                    condition = "Where DepartmentId != " + department.DepartmentId + " and CompanyId = " + department.CompanyId + " and Trim(Lower(DepartmentName))='" + department.DepartmentName.ToLower().Trim() + "'";
                }

                
                var dep = CheckDepartmentExistByCondition(condition);
                if (dep != null)
                {
                    return "Department Already Exist";
                }
            }


            GetDbHelper();
            if (SqlDbHelper == null && oracleDbHelper == null && MySqlDbHelper == null)
            {
                res = "Please Configure Database type";
                return res;
            }
            else
            {

                var quary = DepartmentSaveUpdateQuary(department);
                var updateEmploymentQuary = UpdateEmploymentQuary(department);
                if (SqlDbHelper != null)
                {
                    try
                    {
                        SqlDbHelper.BeginTransaction();
                        SqlDbHelper.ExecuteNonQuery(quary);
                        SqlDbHelper.ExecuteNonQuery(updateEmploymentQuary);
                        SqlDbHelper.CommitTransaction();
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
                        oracleDbHelper.BeginTransaction();
                        oracleDbHelper.ExecuteNonQuery(quary);
                        oracleDbHelper.ExecuteNonQuery(updateEmploymentQuary);
                        oracleDbHelper.CommitTransaction();
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

        private Department CheckDepartmentExistByCondition(string condition)
        {
            Department objDepartment = null;
            GetDbHelper();
            var quary = string.Format(CHECK_DEPARTMENT_BY_CONDITION, condition);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var departmentDataReader = new DepartmentDataReader(reader);

                if (reader.Read())
                    objDepartment = departmentDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var departmentDataReader = new DepartmentDataReader(reader);

                if (reader.Read())
                    objDepartment = departmentDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objDepartment;
        }

        private string UpdateEmploymentQuary(Department department)
        {
            string quary = "";

            quary = string.Format(
                "update Employment Set ReportTo = {1} where DepartmentId = {0} and HrRecordId <> {1}",
                department.DepartmentId, department.DepartmentHeadId);
            
            return quary;
        }

        private string DepartmentSaveUpdateQuary(Department department)
        {
            string quary = "";

            if (department.DepartmentId == 0)
            {
                quary = string.Format(InsertDepartmentData, department.DepartmentName, department.CompanyId, department.DepartmentHeadId,department.IsActive);
            }
            else
            {
                quary = string.Format(UpdateDepartmentData, department.DepartmentName, department.CompanyId, department.DepartmentHeadId,department.IsActive, department.DepartmentId);
            }
            return quary;
        }

        public GridEntity<Department> GetDepartmentSummary(int companyId, int skip, int take, int page, int pageSize, List<Utilities.AzFilter.GridSort> sort, Utilities.AzFilter.GridFilters filter)
        {

            var gridOption = new GridOptions();
            gridOption.skip = skip;
            gridOption.take = take;
            gridOption.page = page;
            gridOption.pageSize = pageSize;
            gridOption.filter = filter;
            gridOption.sort = sort;



            string quary = string.Format(@"Select DepartmentId, DepartmentName, Department.CompanyId, DepartmentHeadId, CompanyName, 'Add Department Head Name' as DepartmentHeadName,Department.IsActive
                                            FROM  Department INNER JOIN Company ON Department.CompanyId = Company.CompanyId                                             
                                            where Department.CompanyId={0}", companyId);
            return Kendo<Department>.Grid.DataSource(gridOption, quary, " DepartmentName ");

            //var departmentList = new List<Department>();
            //GetDbHelper();
            //if (SqlDbHelper != null)
            //{
            //    string sql = GetSqlQuaryForDepartmentSummary(skip, take, page, pageSize, sort, filter, companyId);
            //    IDataReader reader =
            //        SqlDbHelper.GetDataReader(sql);
            //    var departmentDataReader = new DepartmentDataReader(reader);
            //    while (reader.Read())
            //        departmentList.Add(departmentDataReader.Read());
            //    reader.NextResult();
            //    if (reader.Read())
            //    {
            //        if (departmentList.Count > 0)
            //        {
            //            departmentList[0].TotalCount = reader.GetInt32(0);
            //        }
            //    }
            //    reader.Close();
            //    SqlDbHelper.Close();

            //}
            //else if (oracleDbHelper != null)
            //{
            //    string sql = GetOracleQuaryForDepartmentSummary(skip, take, page, pageSize, sort, filter, companyId);
            //    IDataReader reader =
            //       oracleDbHelper.GetDataReader(sql);
            //    var departmentDataReader = new DepartmentDataReader(reader);
            //    while (reader.Read())
            //        departmentList.Add(departmentDataReader.Read());
            //    reader.Close();
            //    string totalSql = GetOracleQuaryTotalCountForUserSummary(sort, filter, companyId);

            //    reader = oracleDbHelper.GetDataReader(totalSql);
            //    if (reader.Read())
            //    {
            //        if (departmentList.Count > 0)
            //        {
            //            departmentList[0].TotalCount = reader.GetInt32(0);
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
            //    var departmentDataReader = new DepartmentDataReader(reader);
            //    while (reader.Read())
            //        departmentList.Add(departmentDataReader.Read());
            //    reader.Close();
            //    MySqlDbHelper.Close();
            //}

            //return departmentList;
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

            var sql = string.Format(@"SELECT COUNT(*) as TotalCount  FROM (Select DepartmentId, DepartmentName, Department.CompanyId, DepartmentHeadId, CompanyName, FullName as DepartmentHeadName FROM  Department INNER JOIN Company ON Department.CompanyId = Company.CompanyId LEFT OUTER JOIN Employee ON Department.DepartmentHeadId = Employee.HRRecordId ) tbl {0}", condition1);

            return sql;
        }

        private string GetOracleQuaryForDepartmentSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
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
                orderby = "ORDER BY DepartmentName asc";
            }
            
            var pageupperBound = skip + take;
            var sql = string.Format(@"Select * from ( Select ROW_NUMBER() OVER({8}) AS RowIndex, tbl.* from (Select DepartmentId, DepartmentName, Department.CompanyId, DepartmentHeadId, CompanyName, FullName as DepartmentHeadName FROM  Department INNER JOIN Company ON Department.CompanyId = Company.CompanyId LEFT OUTER JOIN Employee ON Department.DepartmentHeadId = Employee.HRRecordId ) tbl  WHERE {4} CompanyId={7} {8}) where RowIndex >{0} AND RowIndex <= {6} ", skip, take, page, pageSize, condition, condition1, pageupperBound, companyId, orderby);

            return sql;
        }

        private string GetSqlQuaryForDepartmentSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
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
                orderby = "ORDER BY DepartmentName,DepartmentId asc";
            }


            var pageupperBound = skip + take;
            var sql = string.Format(@"
Select * from (
Select ROW_NUMBER() OVER({7}) AS RowIndex, DepartmentId, DepartmentName, Department.CompanyId, DepartmentHeadId, CompanyName, FullName as DepartmentHeadName,Department.IsActive
FROM  Department INNER JOIN Company ON Department.CompanyId = Company.CompanyId LEFT OUTER JOIN
                      Employee ON Department.DepartmentHeadId = Employee.HRRecordId) as tbl
WHERE {4} RowIndex >{0} AND RowIndex <= {6} {7}
SELECT COUNT(*) as TotalCount  FROM (Select DepartmentId, DepartmentName, Department.CompanyId, DepartmentHeadId, CompanyName, FullName as DepartmentHeadName,Department.IsActive
FROM  Department INNER JOIN Company ON Department.CompanyId = Company.CompanyId LEFT OUTER JOIN
                      Employee ON Department.DepartmentHeadId = Employee.HRRecordId) as tbl1 {5}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
        }

        public IQueryable<Department> GetDepartmentByCompanyId(int companyId)
        {
            var departmentList = new List<Department>();
            GetDbHelper();
            if (SqlDbHelper != null)
            {


                string sql = string.Format(SELECT_DEPARTMENT_BY_COMPANYID, companyId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var departmentDataReader = new DepartmentDataReader(reader);
                while (reader.Read())
                    departmentList.Add(departmentDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_DEPARTMENT_BY_COMPANYID, companyId);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var departmentDataReader = new DepartmentDataReader(reader);
                while (reader.Read())
                    departmentList.Add(departmentDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_DEPARTMENT_BY_COMPANYID, companyId);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var departmentDataReader = new DepartmentDataReader(reader);
                while (reader.Read())
                    departmentList.Add(departmentDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return departmentList.AsQueryable();
        }

        #endregion
    }
}
