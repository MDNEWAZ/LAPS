using Azolution.Entities.HumanResource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Azolution.HumanResource.DataService.DataService
{
   public class DoctorDataService
    {

        public Utilities.GridEntity<Entities.HumanResource.Doctor> GetDoctorSummary(Utilities.GridOptions options)
        {
            string query = string.Format(
                    @"Select Doctor.Exam,Doctor.Year,Doctor.DoctorId,Doctor.DoctorName,Doctor.RegNo,Doctor.Gender,Doctor.DepartmentId,Doctor.SectionId,Doctor.IsActive,Department.DepartmentName,CASE
    WHEN Doctor.Gender=1 THEN 'Male' WHEN Doctor.Gender=2 THEN 'Female'ELSE 'Other' END As DoctorGender, CASE
    WHEN Doctor.SectionId=1 THEN 'A'
    WHEN Doctor.SectionId=2 THEN 'B'
	WHEN Doctor.SectionId=3 THEN 'C'ELSE 'D' END As DoctorSection From Doctor 
    left   join Department on Doctor.DepartmentId = Department.DepartmentId 
	left join DoctorEduInfo on Doctor.DoctorId = DoctorEduInfo.DoctorId");

            return Kendo<Entities.HumanResource.Doctor>.Grid.DataSource(options, query, "DoctorId");
        }

        public Utilities.GridEntity<Entities.HumanResource.DoctorEducationInfo> GetDoctorEducationinfoSummary(Utilities.GridOptions options, int id)
        {
            string query = string.Format(@"Select DoctorEduInfiId,Exam,Year,Institute,Result,DoctorId
            from DoctorEduInfo  WHERE DoctorId={0}", id);
            return Kendo<DoctorEducationInfo>.Grid.DataSource(options, query, "DoctorEduInfiId");

        }

        public string SaveDoctor(Doctor doctor)
        {
            //var con = new CommonConnection(IsolationLevel.ReadCommitted);
            CommonConnection connection = new CommonConnection();
            var query = "";
            var res = "";
            //string sqlDotEduInfo = "";
            try
            {
                //con.BeginTransaction();

                if (doctor.DoctorId == 0)
                {

                    query =
                        string.Format(
                            @"INSERT INTO Doctor (DoctorName,RegNo,Gender,DepartmentId,SectionId,IsActive,Exam,Year)values ('{0}','{1}',{2},{3},{4},{5},'{6}','{7}')",
                            doctor.DoctorName, doctor.RegNo,
                            doctor.Gender, doctor.DepartmentId, doctor.SectionId, doctor.IsActive, doctor.Exam,doctor.Year);

                    connection.ExecuteNonQuery(query); 

                }
                else
                {
                    query =
                        string.Format(
                            @"Update Doctor Set DoctorName='{0}', Gender={1},DepartmentId={2},SectionId={3},IsActive={4}, RegNo='{6}',Exam='{7}',Year='{8}' Where DoctorId={5}",
                            doctor.DoctorName, doctor.Gender, doctor.DepartmentId, doctor.SectionId,
                            doctor.IsActive, doctor.DoctorId, doctor.RegNo,doctor.Exam,doctor.Year);


                    //con.ExecuteNonQuery(query);
                    connection.ExecuteNonQuery(query); 

                }
                //con.ExecuteNonQuery(query);

                //if (sqlDotEduInfo != "")
                //{
                //    con.ExecuteNonQuery(sqlDotEduInfo);
                //}

                //con.CommitTransaction();
                res = Operation.Success.ToString();



            }
            catch (Exception exception)
            {
                connection.RollBack();
                res = "Error! During Saving Doctor Information";
            }
            finally
            {
                connection.Close();
            }
            return res;
        }

        public List<Entities.HumanResource.Department> GetAllDepartmentNameForCombo()
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
            return Utilities.Kendo<Department>.Combo.DataSource(query);
        }

        public List<Doctor> GetDoctorInfoReport()
        {

            string sql = string.Format(@"Select Department.DepartmentName, Doctor.* from Doctor 
left join Department  on Department.DepartmentId=Doctor.DepartmentId");


                return Data<Doctor>.DataSource(sql);
          
        
        }

        public string DeleteDoctor(int DoctorId)
        {

          
            CommonConnection connection = new CommonConnection();
            var query = "";
            var res = "";
            //string sqlDotEduInfo = "";
            try
            {
                query = string.Format(@"Delete from Doctor where DoctorId={0}",DoctorId);
                connection.ExecuteNonQuery(query);
                res = Operation.Success.ToString();

            }
            
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                connection.Close();
            }
            return res;
        }
    }
}
