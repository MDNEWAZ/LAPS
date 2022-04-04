using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Azolution.Common.DataService;
using Azolution.Core.DataService.DataReader;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;
using Utilities.Common;

namespace Azolution.Core.DataService.DataService
{

    public class CompanyDataService
    {

        private CommonDbHelper oracleDbHelper;
        private DBHelper SqlDbHelper;
        private OdbcDbHelper MySqlDbHelper;
        private SqlCommand _aCommand;

        #region Sql

        private const string InsertCompanyData = "INSERT INTO Company(CompanyName,Address,Phone,Fax,Email,FullLogoPath,PrimaryContact,Flag,FiscalYearStart,MotherId,IsActive,CompanyCode,CompanyType,CompanyStock,RootCompanyId)" +
            " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}',{7},{8},{9},{10},'{11}','{12}',{13},{14})";

        private const string UpdateCompanyData =
            "Update Company set CompanyName = '{0}', Address='{1}', Phone = '{2}', Fax = '{3}', Email='{4}', FullLogoPath='{5}', " +
            "PrimaryContact='{6}', Flag={7}, FiscalYearStart = {8}, MotherId = {9},IsActive={10},CompanyType = '{12}',CompanyStock ={13},CompanyCode = '{14}',RootCompanyId={15} where CompanyId = {11}";

        private const string SELECT_COMPANY_BY_COMPANYID = "Select * from Company where CompanyId = {0}";

        private const string CHECK_COMPANY_BY_CONDITION = "Select * from Company {0}";

        private const string GETMOTHERCOMPANYQUARYFOREDITCOMPANY = @"Select * from (
with hierarchy (CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart)
as
( select CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart
  from   Company
  where  CompanyId={1} 
  union all
  select x.CompanyId
, x.CompanyName
, x.PrimaryContact
, x.Email
, x.Fax
, x.Phone
, x.Address
, x.FullLogoPath
, x.MotherId
,x.Flag
,x.FiscalYearStart

  from   Company x join hierarchy y
         on ( y.CompanyId = x.MotherId and x.CompanyId<>{0})
)
select *
from   hierarchy
ORDER BY CompanyName ASC) tbl";

        private const string GETMOTHERCOMPANYQUARYFOREDITCOMPANYSQL = @"WITH hierarchy AS (
                            SELECT ROW_NUMBER() OVER (ORDER BY t.CompanyName) as RowIndex
                        , t.CompanyId
                        , t.CompanyName
                        , t.PrimaryContact
                        , t.Email
                        , t.Fax
                        , t.Phone
                        , t.Address
                        , t.FullLogoPath
                        , t.MotherId
                            FROM dbo.Company t
                            WHERE t.CompanyId={0}
                            UNION ALL
                            SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName) as RowIndex
                        ,x.CompanyId
                        , x.CompanyName
                        , x.PrimaryContact
                        , x.Email
                        , x.Fax
                        , x.Phone
                        , x.Address
                        , x.FullLogoPath
                        , x.MotherId
                            FROM dbo.Company x
                            JOIN hierarchy y ON x.CompanyId = y.MotherId)
                        SELECT *  FROM hierarchy s
						where CompanyId <> {0}
                        ORDER BY CompanyName";

        #endregion

        public string SaveCompany(Company objCompany)
        {
            string res;
            objCompany.CompanyName = objCompany.CompanyName.Replace("'", "''");
            GetDbHelper();
            if (SqlDbHelper == null && oracleDbHelper == null && MySqlDbHelper == null)
            {
                return "Please Configure Database type";

            }
            var quary = CompanySaveUpdateQuary(objCompany);
            if (SqlDbHelper != null)
            {
                if (objCompany.CompanyId == 0)
                {
                    //string condition = "where rtrim(ltrim(lower(CompanyName))) = '" + objCompany.CompanyName.ToLower().Trim() +
                    //                   "'";

                    string condition = string.Format(@"where (rtrim(ltrim(lower(CompanyName))) = '{0}' and (RootCompanyId = {1} or CompanyId = {1} )) or CompanyCode = '{2}'", objCompany.CompanyName.ToLower().Trim(), objCompany.RootCompanyId, objCompany.CompanyCode);
                    var comp = CheckCompanyExistByCondition(condition);
                    if (comp != null)
                    {
                        return "Already Exist";
                    }
                }
                else
                {
                    //string condition = "where rtrim(ltrim(lower(CompanyName))) = '" + objCompany.CompanyName.ToLower().Trim() + "' and CompanyId != " + objCompany.CompanyId;

                    string condition = string.Format(@"where (rtrim(ltrim(lower(CompanyName))) = '{0}' and (RootCompanyId = {1} or CompanyId = {1} ) and CompanyId != {2}) or (CompanyCode = '{3}' and CompanyId != {2})", objCompany.CompanyName.ToLower().Trim(), objCompany.RootCompanyId, objCompany.CompanyId, objCompany.CompanyCode);

                    var comp = CheckCompanyExistByCondition(condition);
                    if (comp != null)
                    {
                        return "Already Exist";
                    }
                }

                GetDbHelper();
                res = ExecututeSqlQuary(quary);
            }
            else if (oracleDbHelper != null)
            {
                if (objCompany.CompanyId == 0)
                {
                    string condition = "where trim(lower(CompanyName)) = '" + objCompany.CompanyName.ToLower().Trim() + "'";
                    var comp = CheckCompanyExistByCondition(condition);
                    if (comp != null)
                    {
                        return "Already Exist";
                    }
                }
                else
                {
                    string condition = "where trim(lower(CompanyName)) = '" + objCompany.CompanyName.ToLower().Trim() + "' and CompanyId != " + objCompany.CompanyId;
                    var comp = CheckCompanyExistByCondition(condition);
                    if (comp != null)
                    {
                        return "Already Exist";
                    }
                }

                GetDbHelper();
                res = ExecututeOracleQuary(quary);
            }
            else
            {
                res = ExecututeMySqlQuary(quary);
            }

            return res;
        }

        private Company CheckCompanyExistByCondition(string condition)
        {
            Company objCompany = null;
            GetDbHelper();
            var quary = string.Format(CHECK_COMPANY_BY_CONDITION, condition);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var companyDataReader = new CompanyDataReader(reader);

                if (reader.Read())
                    objCompany = companyDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var companyDataReader = new CompanyDataReader(reader);

                if (reader.Read())
                    objCompany = companyDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objCompany;
        }

        public IQueryable<Company> GetMotherCompany(int companyId)
        {
            var company = new List<Company>();
            GetDbHelper();
            var sql = MotherCompanyQuary(companyId);

            if (SqlDbHelper != null)
            {
                IDataReader reader = SqlDbHelper.GetDataReader(sql);
                var companyDataReader = new CompanyDataReader(reader);
                while (reader.Read())
                    company.Add(companyDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                   oracleDbHelper.GetDataReader(sql);
                var companyDataReader = new CompanyDataReader(reader);
                while (reader.Read())
                    company.Add(companyDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
                //DataTable companyTable = oracleDbHelper.GetTable(sql);
                //if (companyTable != null)
                //{
                //    DbDataReader reader = companyTable.CreateDataReader();
                //    var companyDataReader = new CompanyDataReader(reader);
                //    while (reader.Read())
                //    {
                //        company.Add(companyDataReader.Read());


                //    }
                //    reader.Close();
                //} 

            }
            else
            {
                IDataReader reader =
                   MySqlDbHelper.GetDataReader(sql);
                var companyDataReader = new CompanyDataReader(reader);
                while (reader.Read())
                    company.Add(companyDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }
            return company.AsQueryable();

        }

        public IQueryable<Company> GetMotherCompanyForEditCompanyCombo(int companyId, int seastionCompnayId)
        {
            var company = new List<Company>();
            GetDbHelper();

            if (SqlDbHelper != null)
            {
                var sql = string.Format(GETMOTHERCOMPANYQUARYFOREDITCOMPANYSQL, companyId, seastionCompnayId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var companyDataReader = new CompanyDataReader(reader);
                while (reader.Read())
                    company.Add(companyDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                var sql = string.Format(GETMOTHERCOMPANYQUARYFOREDITCOMPANY, companyId, seastionCompnayId);
                IDataReader reader =
                   oracleDbHelper.GetDataReader(sql);
                var companyDataReader = new CompanyDataReader(reader);
                while (reader.Read())
                    company.Add(companyDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
                //DataTable companyTable = oracleDbHelper.GetTable(sql);
                //if (companyTable != null)
                //{
                //    DbDataReader reader = companyTable.CreateDataReader();
                //    var companyDataReader = new CompanyDataReader(reader);
                //    while (reader.Read())
                //    {
                //        company.Add(companyDataReader.Read());


                //    }
                //    reader.Close();
                //} 

            }
            else
            {
                var sql = "";
                IDataReader reader =
                   MySqlDbHelper.GetDataReader(sql);
                var companyDataReader = new CompanyDataReader(reader);
                while (reader.Read())
                    company.Add(companyDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }
            return company.AsQueryable();
        }

        public List<Company> GetAllCompaniesWithPaging(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            var companyList = new List<Company>();
            GetDbHelper();

            if (SqlDbHelper != null)
            {
                string sql = GetSqlQuaryForCompanyPageSummary(skip, take, page, pageSize, sort, filter, companyId);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var companyDataReader = new CompanyDataReader(reader);
                while (reader.Read())
                    companyList.Add(companyDataReader.Read());
                reader.NextResult();
                if (reader.Read())
                {
                    if (companyList.Count > 0)
                    {
                        companyList[0].TotalCount = reader.GetInt32(0);
                    }
                }
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = GetOracleQuaryForCompanyPageSummary(skip, take, page, pageSize, sort, filter, companyId);
                IDataReader reader = oracleDbHelper.GetDataReader(sql);
                var companyDataReader = new CompanyDataReader(reader);
                while (reader.Read())
                    companyList.Add(companyDataReader.Read());
                reader.Close();
                string totalSql = GetOracleQuaryTotalCountForCompanyPaging(sort, filter, companyId);
                //DataTable dt = oracleDbHelper.GetTable(totalSql);
                //if (dt != null)
                //{
                //    if (companyList.Count > 0)
                //    {
                //        companyList[0].TotalCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                //    }
                //}
                reader = oracleDbHelper.GetDataReader(totalSql);
                if (reader.Read())
                {
                    if (companyList.Count > 0)
                    {
                        companyList[0].TotalCount = reader.GetInt32(0);
                    }
                }
                reader.Close();

                oracleDbHelper.Close();
            }
            else
            {
                string sql = "";
                IDataReader reader =
                   MySqlDbHelper.GetDataReader(sql);
                var companyDataReader = new CompanyDataReader(reader);
                while (reader.Read())
                    companyList.Add(companyDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }
            return companyList;
        }

        public int GetTotalCountForCompanyPaging(List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            int totalCount = 0;
            GetDbHelper();


            if (oracleDbHelper != null)
            {
                string sql = GetOracleQuaryTotalCountForCompanyPaging(sort, filter, companyId);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                if (reader.Read())
                    totalCount = reader.GetInt32(0);
                reader.Close();
                oracleDbHelper.Close();
            }

            return totalCount;
        }

        private string GetOracleQuaryTotalCountForCompanyPaging(List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            string condition = "";

            condition = FilterCondition(filter);
            if (condition != "")
            {
                condition = "where " + condition;
            }

            var sql =
                string.Format(
                    @"with hierarchy (CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart) as (select CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart from Company where  CompanyId={1} union all select x.CompanyId, x.CompanyName, x.PrimaryContact, x.Email, x.Fax, x.Phone, x.Address, x.FullLogoPath, x.MotherId,x.Flag,x.FiscalYearStart  from   Company x join hierarchy y         on (y.CompanyId = x.MotherId)) select Count(CompanyId) as TotalCount from  (Select ROWNUM rnum, a.* from (Select * from hierarchy) a) {0}",
                    condition, companyId);

            return sql;
        }

        private string GetSqlQuaryForCompanyPageSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
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
                orderby = "ORDER BY CompanyName ASC";
            }

            var sql = string.Format(@"DECLARE @PageLowerBound int;
                        DECLARE @PageUpperBound int;
                        DECLARE @PageIndex int;
                        DECLARE @PageSize int;
                        set @PageIndex = {2};
                        set @PageSize = {3};
                        SET @PageLowerBound = @PageIndex-1 * @PageIndex;
                        SET @PageUpperBound = @PageLowerBound + @PageSize;
                        WITH hierarchy AS (
                            SELECT ROW_NUMBER() OVER ({7}) as RowIndex
                        , t.CompanyId
                        , t.CompanyCode
                        , t.CompanyName
                        , t.RootCompanyId
                        , t.CompanyType
                        , t.CompanyStock
                        , t.PrimaryContact
                        , t.Email
                        , t.Fax
                        , t.Phone
                        , t.Address
                        , t.FullLogoPath
                        , t.MotherId
                        , t.FiscalYearStart
                        , t.IsActive
                            FROM dbo.Company t
                            WHERE t.CompanyId={5}
                            UNION ALL
                            SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName ASC) as RowIndex
                        ,x.CompanyId
                        , x.CompanyCode
                        , x.CompanyName
                        , x.RootCompanyId
                        , x.CompanyType
                        , x.CompanyStock
                        , x.PrimaryContact
                        , x.Email
                        , x.Fax
                        , x.Phone
                        , x.Address
                        , x.FullLogoPath
                        , x.MotherId
                        , x.FiscalYearStart
                        , x.IsActive
                            FROM dbo.Company x
                            JOIN hierarchy y ON y.CompanyId = x.MotherId)
                        SELECT *  FROM hierarchy s
                        WHERE {4}  RowIndex > @PageLowerBound
                        AND RowIndex <= @PageLowerBound + @PageSize
                       {7};

                        WITH hierarchy AS (SELECT t.CompanyId
                        , t.MotherId
                            FROM dbo.Company t
                            WHERE {4} t.CompanyId={5}
                            UNION ALL
                        select
                            x.CompanyId
                        , x.MotherId
                            FROM dbo.Company x
                            JOIN hierarchy y ON y.CompanyId = x.MotherId {6})
                        SELECT COUNT(*)  FROM hierarchy s;", skip, take, page, pageSize, condition, companyId, condition1, orderby);

            return sql;
        }

        private string GetOracleQuaryForCompanyPageSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
        {
            string condition = "";

            condition = FilterCondition(filter);
            if (condition != "")
            {
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
                orderby = "ORDER BY CompanyName ASC";
            }

            var pageLowerBound = (page - 1) * page;
            var pageuperBound = pageLowerBound + pageSize;
            var sql =
                string.Format(
                    @"with hierarchy (CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart) as 
(select CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart from Company where  CompanyId={3} 
union all select x.CompanyId, x.CompanyName, x.PrimaryContact, x.Email, x.Fax, x.Phone, x.Address, x.FullLogoPath, x.MotherId,x.Flag,x.FiscalYearStart  
from   Company x join hierarchy y         on (y.CompanyId = x.MotherId)) select * from  (Select ROWNUM rnum, a.* from (Select * from hierarchy) a) 
where {2} rnum BETWEEN {1} and {0} {4}",
                    pageuperBound, pageLowerBound, condition, companyId, orderby);

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

        public string MotherCompanyQuary(int companyId)
        {
            string sql = "";
            if (companyId == 0)
            {
                sql = "Select * from Company";
            }
            else
            {
                sql =
                    string.Format(
                        @"with hierarchy (CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart,IsActive,CompanyType)
                        as
                        ( select CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag,FiscalYearStart,IsActive,CompanyType
                          from   Company
                          where  CompanyId={0}
                          union all
                          select x.CompanyId
                        , x.CompanyName
                        , x.PrimaryContact
                        , x.Email
                        , x.Fax
                        , x.Phone
                        , x.Address
                        , x.FullLogoPath
                        , x.MotherId
                        ,x.Flag
                        ,x.FiscalYearStart
                        , x.IsActive
                        ,x.CompanyType
                          from   Company x join hierarchy y
                                 on (y.CompanyId = x.MotherId)
                        )
                        select *
                        from   hierarchy where IsActive=1
                        ORDER BY CompanyName ASC


                           ", companyId);
            }
            return sql;
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

        public string CompanySaveUpdateQuary(Company objCompany)
        {
            string quary = "";
            string motherId = objCompany.MotherId == 0 ? "null" : objCompany.MotherId.ToString();



            if (objCompany.CompanyId == 0)
            {


                quary = string.Format(InsertCompanyData, objCompany.CompanyName, objCompany.Address,
                                      objCompany.Phone, objCompany.Fax, objCompany.Email, objCompany.FullLogoPath,
                                      objCompany.PrimaryContact, objCompany.Flag, objCompany.FiscalYearStart,
                                      motherId, objCompany.IsActive, objCompany.CompanyCode, objCompany.CompanyType, objCompany.CompanyStock, objCompany.RootCompanyId);
            }
            else
            {
                quary = string.Format(UpdateCompanyData, objCompany.CompanyName, objCompany.Address,
                                      objCompany.Phone, objCompany.Fax, objCompany.Email, objCompany.FullLogoPath,
                                      objCompany.PrimaryContact, objCompany.Flag, objCompany.FiscalYearStart,
                                      motherId, objCompany.IsActive, objCompany.CompanyId, objCompany.CompanyType, objCompany.CompanyStock, objCompany.CompanyCode, objCompany.RootCompanyId);
            }
            return quary;
        }

        public string ExecututeSqlQuary(string quary)
        {
            string mes;
            try
            {
                SqlDbHelper.ExecuteNonQuery(quary);
                mes = "Success";
            }
            catch (Exception exception)
            {
                mes = exception.Message;
                //SqlDbHelper.RollBack();
            }
            finally
            {
                SqlDbHelper.Close();
            }
            return mes;
        }

        public string ExecututeOracleQuary(string quary)
        {
            var mes = "";
            try
            {
                GetDbHelper();
                oracleDbHelper.ExecuteNonQuery(quary);
                mes = "Success";
            }
            catch (Exception exception)
            {
                mes = exception.Message;
                //oracleDbHelper.RollBack();
            }
            finally
            {
                oracleDbHelper.Close();
            }
            return mes;
        }

        public string ExecututeMySqlQuary(string quary)
        {
            var mes = "";
            try
            {
                MySqlDbHelper.ExecuteNonQuery(quary);
                mes = "Success";
            }
            catch (Exception exception)
            {
                mes = exception.Message;
                MySqlDbHelper.RollBack();
            }
            finally
            {
                MySqlDbHelper.Close();
            }
            return mes;
        }

        public Company SelectCompanyByCompanyId(int companyId)
        {
            Company objCompany = null;
            GetDbHelper();
            var quary = string.Format(SELECT_COMPANY_BY_COMPANYID, companyId);
            if (SqlDbHelper != null)
            {
                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var companyDataReader = new CompanyDataReader(reader);
                if (reader.Read())
                    objCompany = companyDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader = oracleDbHelper.GetDataReader(quary);
                var companyDataReader = new CompanyDataReader(reader);
                if (reader.Read())
                    objCompany = companyDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }
            return objCompany;
        }

        public GridEntity<Interest> GetAllCompanies(GridOptions aOptions, int companyId)
        {
            var query = string.Format("Select SI.InterestId, SI.Interests, SI.DownPay, SI.EntryDate, SI.Status,SI.DefaultInstallmentNo, C.CompanyId, C.CompanyName from Sale_Interest SI Right JOIN Company C ON C.CompanyId=SI.CompanyId ");
            var data = Kendo<Interest>.Grid.GenericDataSource(aOptions, query, "CompanyId");
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDue"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
     

        private static bool HasThisInterest(Interest aDue)
        {
            var aConnection = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            var query = string.Format("SELECT * FROM [dbo].[Sale_Interest]");
            var adapter = new SqlDataAdapter(query, aConnection);
            var aTable = new DataTable();
            adapter.Fill(aTable);
            return aTable.Rows.Count != 0 && aTable.Rows.Cast<DataRow>().Any(aRow => (int)aRow[1] == aDue.ACompany.CompanyId);
        }

        public Interest GetAInterest(int companyId)
        {
            var query = string.Format("Select * from Sale_Interest SI Where SI.CompanyId ={0}", companyId);
            return Kendo<Interest>.Combo.DataSource(query).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
       

        private bool HasThisDue(Due aDue)
        {
            return true;
        }

        public List<Company> GetRootMotherCompany()
        {
            string sql = string.Format(@"Select CompanyId,CompanyName from Company Where CompanyType = 'MotherCompany'");
            return Kendo<Company>.Combo.DataSource(sql);
        }

        public object GetRootCompany()
        {
            string sql = string.Format(@"Select CompanyId,CompanyName from Company where CompanyType = 'MotherCompany'");
            return Kendo<Company>.Combo.DataSource(sql);
        }

        public Company GetCompanyInfoByBranchCode(string branchCode)
        {
            string sql = string.Format(@"Select Company.*,Branch.BRANCHID,Branch.BranchCode From Company 
                    inner join Branch on Branch.COMPANYID= Company.CompanyId
                    Where BranchCode ='{0}'",branchCode);
            return Data<Company>.GenericDataSource(sql).SingleOrDefault();
        }

        public GridEntity<Company> GetCompanySummary(GridOptions options, int companyId)
        {
           string sql = "Select * from Company";

            var data = Kendo<Company>.Grid.DataSource(options, sql, "CompanyName");
            return data;
        }

        public List<Company> GetAllCompaniesForCombo()
        {
            string sql = "Select * from Company";
            var data = Data<Company>.DataSource(sql);
            return data;
        }
    }
}
