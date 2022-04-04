using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Common.DataService;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.HumanResource;
using Utilities;

namespace Azolution.HumanResource.DataService.DataService
{
    public class EmployeeDataService
    {
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

        private const string InsertEmployeeData =
          "insert into Employee(EmployeeName,Mobile,Email)values ('{0}','{1}','{2}')";
        private const string InsertEmployeeDetailsData =
         "insert into EmployeeDetails(CompanyName,Designation,salary,EmployeeID)values ('{0}','{1}','{2}',{3})";

       
          
      
         

        public string SaveEmployee(Employee employee)
        {
            var res = "";
            var connectionType = ConfigurationSettings.AppSettings["DataBaseType"];

            if (employee.EmployeeID== 0)
            {
                string condition = "";

                if (connectionType == "SQL")
                {
                    condition = "Where (EmployeeID = " + employee.EmployeeID +
                                      " and rtrim(ltrim(Lower(EmployeeName)))='" +
                                      employee.EmployeeName.ToLower().Trim() + "'" + ")";
                }
            }
            else
            {
                string condition = "";
                if (connectionType == "SQL")
                {
                    condition = "Where (EmployeeID != " + employee.EmployeeID + " and EmployeeName = " + employee.EmployeeName +
                                       " and rtrim(ltrim(Lower(Mobile)))='" + employee.Mobile.ToLower().Trim() +
                                       "'" + "and EmployeeName = " + employee.Email +
                                       ")";
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



                var quary = EmployeeInsertQuery(employee);
               
                if (SqlDbHelper != null)
                {
                    try
                    {
                        SqlDbHelper.ExecuteNonQuery(quary);
                        employee.EmployeeID = GetMasterId().EmployeeID;
                        var queryTwo = EmployeeDetailsInsertQuery(employee);
                        SqlDbHelper.ExecuteNonQuery(queryTwo);

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

        public string NewSaveEmployee(Employee employee)
        {
            var res = "";
            var conn = new CommonConnection(IsolationLevel.ReadCommitted);
            try
            {
                conn.BeginTransaction();

                //Employee Save
                if (employee.EmployeeID == 0)
                {

                    var empSql =
                        string.Format(
                            "insert into NewEmployee(EmployeeName,Mobile,Email,Designation)values ('{0}','{1}','{2}','{3}')",
                            employee.EmployeeName, employee.Mobile, employee.Email, employee.Designation);

                    var id = conn.ExecuteAfterReturnId(empSql, "");

                    string sqlEdu = "";
                    

                    foreach (var item in employee.education)
                    {
                        sqlEdu +=
                            string.Format(
                                "insert into Education(Exam,Year,Institute,Result,EmployeeId)values ('{0}',{1},'{2}','{3}',{4});",
                                item.Exam, item.Year, item.Institute, item.Result, id);
                    }
                    string sql = "";

                    foreach (var item in employee.Experiences)
                    {
                        sql += 
                            string.Format(
                            "insert into Experience(Company,FromDate,ToDate,Remarks,EmployeeId)values ('{0}','{1}','{2}','{3}',{4});",
                                item.Company, item.FromDate.ToString("MM/dd/yyyy"), item.ToDate.ToString("MM/dd/yyyy"), item.Remarks, id);
                    }

                    var empInfo = "";
                    foreach (var item in employee.Employees)
                    {
                        empInfo += string.Format(
                            "insert into EmployeeInfo(EmployeeID,EmployeesInfo)values ({0},{1})",
                            id, item.EmployeeID);
                    }
                    if (sqlEdu != "")
                    {
                        conn.ExecuteNonQuery(sqlEdu);
                    }
                    if (sql != "")
                    {
                        conn.ExecuteNonQuery(sql);
                    }
                    if (empInfo != "")
                    {
                        conn.ExecuteNonQuery(empInfo);
                    }
                }
                else
                {
                    var sql = "";
                    sql = string.Format(@"Update NewEmployee Set EmployeeName='{0}',Mobile='{1}', Designation='{2}' Where EmployeeID={3};", employee.EmployeeName, employee.Mobile, employee.Designation, employee.EmployeeID);
                    conn.ExecuteNonQuery(sql);
                    var id = employee.EmployeeID;
                   
                    string sqlEdu = "";
                  
                    foreach (var item in employee.education)
                    {
                        if (item.EducationID == 0)
                        {
                            sqlEdu +=
                             string.Format(
                                 "insert into Education(Exam,Year,Institute,Result,EmployeeId)values ('{0}',{1},'{2}','{3}',{4});",
                                 item.Exam, item.Year, item.Institute, item.Result, id);
                        }
                        else
                        {
                            sqlEdu +=
                            string.Format(
                                "update Education set Exam='{0}',Year={1},Institute='{2}',Result='{3}' where EducationId={4};",
                                item.Exam, item.Year, item.Institute, item.Result, item.EducationID);
                        }
                        
                    }

                    string sqlExp = "";

                    foreach (var item in employee.Experiences)
                    {
                        if (item.ExperienceID == 0)
                        {
                            sqlExp +=
                             string.Format(
                                 "insert into Experience(Company,FromDate,ToDate,Remarks,EmployeeId)values ('{0}','{1}','{2}','{3}',{4});",
                                 item.Company, item.FromDate.ToString("MM/dd/yyyy"), item.ToDate.ToString("MM/dd/yyyy"), item.Remarks, id);
                        }
                        else
                        {
                            sqlExp +=
                            string.Format(
                                "update Experience set Company='{0}',FromDate='{1}',ToDate='{2}',Remarks='{3}' where ExperienceID={4};",
                                item.Company, item.FromDate.ToString("MM/dd/yyyy"), item.ToDate.ToString("MM/dd/yyyy"), item.Remarks, item.ExperienceID);
                        }

                    }
                    if (sqlEdu != "")
                    {
                        conn.ExecuteNonQuery(sqlEdu);
                    }
                    if (sqlExp != "")
                    {
                        conn.ExecuteNonQuery(sqlExp);
                    }
                }
            
                conn.CommitTransaction();
                res = Operation.Success.ToString();

            }
            catch (Exception)
            {

                conn.RollBack();
            }
            finally
            {
                conn.Close();
            }
             
            

            return res;
        }
        private string EmployeeInsertQuery(Employee employee)
        {
            string quary = "";
            quary = string.Format(InsertEmployeeData, employee.EmployeeName, employee.Mobile, employee.Email);
            return quary;
        }

       

        private string EmployeeDetailsInsertQuery(Employee employee)
        {
            string quary = "";
            quary = string.Format(InsertEmployeeDetailsData, employee.CompanyName, employee.Designation, employee.Salary, employee.EmployeeID);
            return quary;
        }
        
        Employee GetMasterId()
        {
            string query = "select tOP 1 * from Employee order by EmployeeID dESC";
            var data = Data<Employee>.DataSource(query);
            return data.SingleOrDefault();
        }

        public GridEntity<Employee> GetEmployeeSummary(GridOptions options)
        {
            var employeeList = new List<Employee>();
            string query = string.Format(@"Select E.EmployeeID,EmployeeName,Designation,Salary from Employee as E, EmployeeDetails as ED where E.EmployeeID=ED.EmployeeID");
            return Kendo<Employee>.Grid.DataSource(options, query, "EmployeeID");

        }
        public GridEntity<Employee> GetAllEmployeeSummary(GridOptions options)
        {
            var employeeList = new List<Employee>();
            string query = string.Format(@"Select EmployeeID,EmployeeName,Designation,Mobile,Email from NewEmployee");
            return Kendo<Employee>.Grid.DataSource(options, query, "EmployeeID");

        }

        public GridEntity<EducationDetail> GetEducationSummary(GridOptions options,int id)
        {
            string query = string.Format(@"Select EducationID,Exam,Year,Institute,Result 
            from Education WHERE EmployeeID={0}", id);
            return Kendo<EducationDetail>.Grid.DataSource(options, query, "EducationID");

        }

        public List<Employee> GetEmployeeInfoForCheckBox(int id)
        {
            string query = string.Format(@"Select * from EmployeeInfo WHERE EmployeeID={0}", id);
            return Data<Employee>.DataSource(query);

        }

        public GridEntity<Experience> GetExperienceSummary(GridOptions options, int id)
        {
            string query = string.Format(@"Select ExperienceID,Company,FromDate,ToDate,Remarks 
            from Experience WHERE EmployeeID={0}", id);
            return Kendo<Experience>.Grid.DataSource(options, query, "ExperienceID");

        }

        public Experience GetExperienceById(int id, Users objUser)
        {
            string sql = string.Format(@"Select * From Experience Where ExperienceID={0}", id);
            var data = Data<Experience>.DataSource(sql);
            return data.SingleOrDefault();
        }

        //public List<Experience> GetExperienceById(string condition)
        //{
       
        //    var experienceList = new List<Experience>();
           

        //        string sql = string.Format(SELECT_BRANCH_BY_COMPANYID, condition);
        //        IDataReader reader =
        //            SqlDbHelper.GetDataReader(sql);
        //        var branchDataReader = new BranchDataReader(reader);
        //        while (reader.Read())
        //            branchList.Add(branchDataReader.Read());
        //        reader.Close();
        //        SqlDbHelper.Close();

           
           
        //    return branchList;
        //}

        
    }
}
