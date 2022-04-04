using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Azolution.Common.DataService;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.HumanResource;
using Azolution.Entities.Sale;
using Azolution.HumanResource.DataService.DataReader;
using Utilities;
using Utilities.Common;

namespace Azolution.HumanResource.DataService.DataService
{
    public class BranchDataService 
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
                                                         UtilityCommon.BuildWhereClause<Branch>(i, filter.Logic,
                                                                                                 filters[i],
                                                                                                 parameters));

                    }
                    else
                    {
                        if (filters[i].Value != null)
                        {
                            whereClause += string.Format(" {0} {1}",
                                                         UtilityCommon.ToLinqOperator(filter.Logic),
                                                         UtilityCommon.BuildWhereClause<Branch>(i, filter.Logic,
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

        
        //private const string SELECT_BRANCH_BY_COMPANYID = "Select * from Branch where CompanyId = {0}";

         private const string SELECT_BRANCH_BY_COMPANYID = @"Select BranchId, (convert(varchar(10),BranchCode)+' ' + '|'+' '+ BRANCHNAME) As BRANCHNAME, COMPANYID,SMSMobileNo,
            BRANCHDESCRIPTION,IsActive,BranchSmsMobileNumber,BranchCode From Branch {0}";

        private const string InsertBranchData =
            "Insert into Branch (COMPANYID,BRANCHNAME,BRANCHDESCRIPTION,IsActive,BranchSmsMobileNumber,BranchCode,IsUpgraded,IsSmsEligible) values ({0},'{1}','{2}',{3},'{4}','{5}',{6},{7})";

        private const string UpdateBranchData =
            "Update Branch set COMPANYID={0}, BRANCHNAME='{1}', BRANCHDESCRIPTION = '{2}',IsActive={3},BranchSmsMobileNumber='{4}',BranchCode = '{5}',IsSmsEligible={6} where BranchId = {7}";

        private const string CHECK_BRANCH_BY_CONDITION = "Select * from Branch {0}";

        private const string SELECT_BRANCHTWO_BY_COMPANYID = "Select BranchId, BRANCHNAME,BRANCHDESCRIPTION From BranchTwo {0}";

        private const string InsertBranchTwoData =
           "insert into BranchTwo(COMPANYID,BRANCHNAME,BRANCHDESCRIPTION,IsActive)values ({0},'{1}','{2}',{3})";

        private const string updateBranchTwoData =
            "Update BranchTwo set COMPANYID={0}, BRANCHNAME='{1}', BRANCHDESCRIPTION = '{2}',IsActive={3}  where BranchId = {4}";

        private const string CHECK_BRANCHTWO_BY_CONDITION = "Select * from BranchTwo {0}";

        #endregion

        public List<Branch> GetBranchByCompanyId(string condition)
        {
            var branchList = new List<Branch>();
            GetDbHelper();


            //WHERE     (CompanyID = @companyId)))) AND (dbo.Employee.StateId NOT IN (3, 4))";")

            if (SqlDbHelper != null)
            {


                string sql = string.Format(SELECT_BRANCH_BY_COMPANYID, condition);
                IDataReader reader =
                    SqlDbHelper.GetDataReader(sql);
                var branchDataReader = new BranchDataReader(reader);
                while (reader.Read())
                    branchList.Add(branchDataReader.Read());
                reader.Close();
                SqlDbHelper.Close();

            }
            else if (oracleDbHelper != null)
            {
                string sql = string.Format(SELECT_BRANCH_BY_COMPANYID, condition);
                IDataReader reader =
                    oracleDbHelper.GetDataReader(sql);
                var branchDataReader = new BranchDataReader(reader);
                while (reader.Read())
                    branchList.Add(branchDataReader.Read());
                reader.Close();
                oracleDbHelper.Close();
            }
            else
            {
                string sql = string.Format(SELECT_BRANCH_BY_COMPANYID, condition);
                IDataReader reader =
                    MySqlDbHelper.GetDataReader(sql);
                var branchDataReader = new BranchDataReader(reader);
                while (reader.Read())
                    branchList.Add(branchDataReader.Read());
                reader.Close();
                MySqlDbHelper.Close();
            }

            return branchList;
        }

        public string SaveBranchTwo(Branch branch)
        {
            var res = "";
            var connectionType = ConfigurationSettings.AppSettings["DataBaseType"];

            if (branch.BranchId == 0)
            {
                string condition = "";

                if (connectionType == "SQL")
                {
                     condition = "Where (CompanyId = " + branch.CompanyId +
                                       " and rtrim(ltrim(Lower(BranchName)))='" +
                                       branch.BranchName.ToLower().Trim() + "'" + ")";
                }
                var brnch = CheckBranchTwoExistByCondition(condition);
                if (brnch != null)
                {
                    return "Branch Already Exist";
                }
            }
            else
            {
                string condition = "";
                if (connectionType == "SQL")
                {
                    condition = "Where (BranchId != " + branch.BranchId + " and CompanyId = " + branch.CompanyId +
                                       " and rtrim(ltrim(Lower(BranchName)))='" + branch.BranchName.ToLower().Trim() +
                                       "'" + ")";
                }

                var brnch = CheckBranchTwoExistByCondition(condition);
                if (brnch != null)
                {
                    return "Branch Already Exist";
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



                var quary = BranchTwoSaveUpdateQuary(branch);
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
        public string SaveBranch(Branch branch)
        {
            var res = "";
            var connectionType = ConfigurationSettings.AppSettings["DataBaseType"];
            if (branch.BranchId == 0)
            {
                string condition = "";

                if (connectionType == "SQL")
                {
                    condition = "Where (CompanyId = " + branch.CompanyId + " and rtrim(ltrim(Lower(BranchName)))='" + branch.BranchName.ToLower().Trim() + "'" + ") or BranchCode = '" + branch.BranchCode + "'";
                }
                else if (connectionType == "Oracle")
                {
                    condition = "Where (CompanyId = " + branch.CompanyId + " and Trim(Lower(BranchName))='" + branch.BranchName.ToLower().Trim() + "'" + " ) or BranchCode = '" + branch.BranchCode + "'";
                }


                
                var brnch = CheckBranchExistByCondition(condition);
                if (brnch != null)
                {
                    return "Branch Already Exist";
                }
            }
            else
            {
                string condition = "";

                 if (connectionType == "SQL")
                 {
                     condition = "Where (BranchId != " + branch.BranchId + " and CompanyId = " + branch.CompanyId + " and rtrim(ltrim(Lower(BranchName)))='" + branch.BranchName.ToLower().Trim() + "'" + ")  or ( BranchId != " + branch.BranchId + " and BranchCode = '" + branch.BranchCode + "')"; 
                 }
                else if (connectionType == "Oracle")
                {
                    condition = "Where (BranchId != " + branch.BranchId + " and CompanyId = " + branch.CompanyId + " and Trim(Lower(BranchName))='" + branch.BranchName.ToLower().Trim() + "'" + ")  or  ( BranchId != " + branch.BranchId + " and BranchCode = '" + branch.BranchCode + "')";
                }

                
                var brnch = CheckBranchExistByCondition(condition);
                if (brnch != null)
                {
                    return "Branch Already Exist";
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
                


                var quary = BranchSaveUpdateQuary(branch);
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

        private Branch CheckBranchExistByCondition(string condition)
        {
            Branch objBranch = null;
            GetDbHelper();
            var quary = string.Format(CHECK_BRANCH_BY_CONDITION, condition);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var branchDataReader = new BranchDataReader(reader);

                if (reader.Read())
                    objBranch = branchDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var branchDataReader = new BranchDataReader(reader);

                if (reader.Read())
                    objBranch = branchDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objBranch;
        }

        private Branch CheckBranchTwoExistByCondition(string condition)
        {
            Branch objBranch = null;
            GetDbHelper();
            var quary = string.Format(CHECK_BRANCHTWO_BY_CONDITION, condition);

            if (SqlDbHelper != null)
            {

                IDataReader reader =
                    SqlDbHelper.GetDataReader(quary);
                var branchDataReader = new BranchDataReader(reader);

                if (reader.Read())
                    objBranch = branchDataReader.Read();
                reader.Close();
                SqlDbHelper.Close();
            }
            else if (oracleDbHelper != null)
            {
                IDataReader reader =
                    oracleDbHelper.GetDataReader(quary);
                var branchDataReader = new BranchDataReader(reader);

                if (reader.Read())
                    objBranch = branchDataReader.Read();
                reader.Close();
                oracleDbHelper.Close();
            }


            return objBranch;
        }
        private string BranchSaveUpdateQuary(Branch branch)
        {
            string quary = "";

            if (branch.BranchId == 0)
            {
                quary = string.Format(InsertBranchData, branch.CompanyId, branch.BranchName, branch.BranchDescription, branch.IsActive, branch.BranchSmsMobileNumber, branch.BranchCode, 1, branch.IsSmsEligible);
            }
            else
            {
                quary = string.Format(UpdateBranchData, branch.CompanyId, branch.BranchName, branch.BranchDescription, branch.IsActive, branch.BranchSmsMobileNumber, branch.BranchCode, branch.IsSmsEligible, branch.BranchId);
            }
            return quary;
        }

        private string BranchTwoSaveUpdateQuary(Branch branch)
        {
            string quary = "";

            if (branch.BranchId == 0)
            {
                quary = string.Format(InsertBranchTwoData, branch.CompanyId, branch.BranchName, branch.BranchDescription, branch.IsActive);
            }
            else
            {
                quary = string.Format(updateBranchTwoData, branch.CompanyId, branch.BranchName, branch.BranchDescription, branch.IsActive, branch.BranchId);
            }
            return quary;
        }

        public GridEntity<Branch> GetBranchSummary(GridOptions options, int companyID)
        {
            var branchList = new List<Branch>();
            string query = string.Format(@"Select * from Branch where COMPANYID = {0}", companyID);
            return Kendo<Branch>.Grid.DataSource(options, query, "BRANCHID");

        }
    
        public GridEntity<Branch> GetBranchTwoSummary(GridOptions options, int companyID)
        {
            var branchList = new List<Branch>();
            string query = string.Format(@"Select * from Branch where COMPANYID = {0}", companyID);
            return Kendo<Branch>.Grid.DataSource(options, query, "BRANCHID");

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

            var sql = string.Format(@"SELECT COUNT(*) as TotalCount  FROM Branch {0}", condition1);

            return sql;
        }

        private string GetOracleQuaryForBranchSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
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
                orderby = "ORDER BY BRANCHNAME";
            }

            var pageupperBound = skip + take;
            var sql = string.Format(@"Select * from ( Select ROW_NUMBER() OVER({8}) AS RowIndex, tbl.* from (Select BRANCHID, COMPANYID, BRANCHNAME, BRANCHDESCRIPTION FROM  Branch) tbl  WHERE {4} CompanyId={7} {8}) where RowIndex >{0} AND RowIndex <= {6}", skip, take, page, pageSize, condition, condition1, pageupperBound, companyId, orderby);

            return sql;
        }

        private string GetSqlQuaryForBranchSummary(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter, int companyId)
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
                orderby = "ORDER BY BRANCHNAME,BRANCHID asc";
            }


            var pageupperBound = skip + take;
            var sql = string.Format(@"
Select * from (
Select ROW_NUMBER() OVER({7}) AS RowIndex, BRANCHID, COMPANYID, BRANCHNAME, BRANCHDESCRIPTION,IsActive,BranchSmsMobileNumber
FROM  Branch) as tbl
WHERE {4} RowIndex >{0} AND RowIndex <= {6} {7}
SELECT COUNT(*) as TotalCount  FROM Branch {5}", skip, take, page, pageSize, condition, condition1, pageupperBound, orderby);

            return sql;
        }

        public List<Branch> GetAllBranchByCompanyIdForCombo(string condition)
        {
          
            string branchQuery = string.Format(@"Select * from Branch where IsActive=1 {0}", condition);

            return Kendo<Branch>.Combo.DataSource(branchQuery);
        }

        public bool GetIsBranchCodeUsed(string branchCode)
        {
            CommonConnection common = new CommonConnection();
            var isUsedBranchCode = false;
            string query = string.Format(@"Select * from Sale_Customer
            where CustomerCode like '{0}%'",branchCode);
            var data = Data<Customer>.DataSource(query);
            if (data.Count != 0)
            {
                isUsedBranchCode = true;
            }
            return isUsedBranchCode;
        }

        public Branch GetBranchInfoByBranchId(int branchId)
        {
            string sql = string.Format(@"Select * From Branch Where BranchId={0}", branchId);
            var data = Data<Branch>.DataSource(sql);
            return data.SingleOrDefault();
        }

        public string UpgradeBranch(Branch objBranch, List<Customer> customers)
        {
            string rv = "";
            string sql = "";
          
            CommonConnection connection = new CommonConnection();
            connection.BeginTransaction();
            try
            {
                sql = string.Format(@"Update Branch Set BranchCode='{0}',IsUpgraded={1} Where BranchId={2};", objBranch.NewBranchCode,1, objBranch.BranchId);
                connection.ExecuteNonQuery(sql);
                if (customers.Count != 0)
                {
                    UpgradeCustomerCode(customers, objBranch.NewBranchCode, connection);
                }
                rv = Operation.Success.ToString();
                connection.CommitTransaction();
            }
            catch (Exception exception)
            {
                connection.RollBack();
                rv = exception.Message;
            }
            finally
            {
               connection.Close();
            }

            return rv;

        }

        private void UpgradeCustomerCode(List<Customer> customers, string newbranchCode, CommonConnection connection)
        {
            string sql = "";
            StringBuilder qBuilder = new StringBuilder();
            foreach (var cust in customers)
            {
                var newCustomerCode = newbranchCode + cust.CustomerCode.Substring(cust.CustomerCode.Length-4);
                qBuilder.Append(string.Format(@"Update Sale_Customer Set CustomerCode='{0}',IsUpgraded={1} Where CustomerId={2};", newCustomerCode,1, cust.CustomerId));
            }

            if (qBuilder.ToString() != "")
            {
                sql = "Begin " + qBuilder + " End;";
                connection.ExecuteNonQuery(sql);
            }
            
        }

        public BranchInfo GetCompanyBranchInfoByBranchId(int branchId)
        {
            string sql = @"Select * From Branch left join Company on Company.CompanyId=Branch.COMPANYID Where BranchId= " + branchId;
            var data = Data<BranchInfo>.DataSource(sql);
            return data.SingleOrDefault();
        }

       
    }
}


