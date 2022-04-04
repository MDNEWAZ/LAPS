using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.HumanResource;
using Utilities;

namespace Azolution.HumanResource.DataService.DataService
{
    public class StudentDataService

    {
        public List<Department> GetAllDepartmentNameForCombo()
        {
            var query = "";
            try
            {
                query = string.Format(@"select Department.DepartmentId, Department.DepartmentName from department");
            }
            catch (Exception)
            {

                throw;
            }
            return Kendo<Department>.Combo.DataSource(query);
        }

        public string SaveStudent(Student student)
        {
            var con = new CommonConnection(IsolationLevel.ReadCommitted);
            var query = "";
            var res = "";
            string sqlStuEduInfo = "";
            try
            {
                con.BeginTransaction();

                if (student.StudentId == 0)
                {

                    query =
                        string.Format(
                            @"INSERT INTO Student (StudentName,RegNo,Gender,DepartmentId,SectionId,IsActive)values ('{0}','{1}',{2},{3},{4},{5})",
                            student.StudentName, student.RegNo,
                            student.Gender, student.DepartmentId, student.SectionId, student.IsActive);

                    var studentId = con.ExecuteAfterReturnId(query, "");

                    foreach (var item in student.StudentEducationInfo)
                    {
                        sqlStuEduInfo +=
                            string.Format(
                                "insert into StudentEduInfo(Exam,Year,Institute,Result,StudentId)values ('{0}','{1}','{2}',{3},{4});",
                                item.Exam, item.Year, item.Institute, item.Result, studentId);
                    }



                }
                else
                {
                    query =
                        string.Format(
                            @"Update Student Set StudentName='{0}', Gender={1},DepartmentId={2},SectionId={3},IsActive={4} Where StudentId={5}",
                            student.StudentName, student.Gender, student.DepartmentId, student.SectionId,
                            student.IsActive, student.StudentId);


                    con.ExecuteNonQuery(query);

                    foreach (var item in student.StudentEducationInfo)
                    {
                        if (item.StudentEduInfiId == 0)
                        {
                            sqlStuEduInfo +=
                             string.Format(
                                 "insert into StudentEduInfo(Exam,Year,Institute,Result,StudentId" +
                                 ")values ('{0}','{1}','{2}',{3},{4});",
                                 item.Exam, item.Year, item.Institute, item.Result, student.StudentId);
                        }
                        else
                        {
                            sqlStuEduInfo +=
                            string.Format(
                                "update StudentEduInfo set Exam='{0}',Year='{1}',Institute='{2}',Result={3} where StudentId={4};",
                                item.Exam, item.Year, item.Institute, item.Result, item.StudentEduInfiId);
                        }

                    }

                }
                //con.ExecuteNonQuery(query);

                if (sqlStuEduInfo != "")
                {
                    con.ExecuteNonQuery(sqlStuEduInfo);
                }

                con.CommitTransaction();
                res = Operation.Success.ToString();



            }
            catch (Exception exception)
            {
                con.RollBack();
                res = "Error! During Saving Student Information";
            }
            finally
            {
                con.Close();
            }
            return res;
        }

        private int ExitRegNo(string regNo)
        {
            var connection = new CommonConnection();

            string query = string.Format(" SELECT RegNo From Student where RegNo='{0}'", regNo);
            DataTable dt = new DataTable();
            dt = connection.GetDataTable(query);
            var total = dt == null ? 0 : dt.Rows.Count;
            return total;
        }

        public GridEntity<Student> GetStudentSummary(GridOptions options)
        {

            string query =  string.Format(
                    @"Select StudentEduInfo.Exam,Student.StudentId,Student.StudentName,Student.RegNo,Student.Gender,Student.DepartmentId,Student.SectionId,Student.IsActive,DepartmentName,CASE
    WHEN Student.Gender=1 THEN 'Male' WHEN Student.Gender=2 THEN 'Female'ELSE 'Other' END As StudentGender, CASE
    WHEN Student.SectionId=1 THEN 'A'
    WHEN Student.SectionId=2 THEN 'B'
	WHEN Student.SectionId=3 THEN 'C'ELSE 'D' END As StudentSection From Student 
    left   join Department on Student.DepartmentId = Department.DepartmentId 
	left join StudentEduInfo on Student.StudentId = StudentEduInfo.StudentId");

            return Kendo<Student>.Grid.DataSource(options, query, "StudentId");

        }

        public GridEntity<StudentEducationInfo> GetStudentEducationinfoSummary(GridOptions options, int id)
        {
            string query = string.Format(@"Select StudentEduInfiId,Exam,Year,Institute,Result,StudentId
            from StudentEduInfo  WHERE StudentId={0}", id);
            return Kendo<StudentEducationInfo>.Grid.DataSource(options, query, "StudentEduInfiId");


        }

        public List<Student> GetStudentInfoReport()
        {
            try
            {

                string sql = string.Format(@"select Student.StudentId, Student.StudentName,Department.DepartmentName ,StudentEduInfo.Exam,StudentEduInfo.Institute,StudentEduInfo.Year,StudentEduInfo.Result  from StudentEduInfo 
       left join Student on StudentEduInfo.StudentId=Student.StudentId
	   left join Department on Department.DepartmentId= Student.DepartmentId
              order by StudentId desc");


                return Data<Student>.DataSource(sql);
            }
            catch (Exception)
            {
                throw new Exception(" Error! While Exceuting SQL Query");
            }
        }
    }
}
