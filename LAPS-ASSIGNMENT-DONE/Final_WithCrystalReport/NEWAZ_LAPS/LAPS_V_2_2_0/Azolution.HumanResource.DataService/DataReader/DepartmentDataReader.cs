    using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.HumanResource;

namespace Azolution.HumanResource.DataService.DataReader
{
    internal class DepartmentDataReader : EntityDataReader<Department>
    {
        public int DepartmentIdColumn = -1;
        public int CompanyIdColumn = -1;
        public int DepartmentNameColumn = -1;
        public int CompanyNameColumn = -1;
        public int DepartmentHeadIdColumn = -1;
        public int DepartmentHeadNameColumn = -1;
        public int IsActiveColumn = -1;

        public DepartmentDataReader(IDataReader reader)
            : base(reader)
        { }

        public override Department Read()
        {
            var objDepartment = new Department();
            objDepartment.DepartmentId = GetInt(DepartmentIdColumn);
            objDepartment.CompanyId = GetInt(CompanyIdColumn);
            objDepartment.DepartmentName = GetString(DepartmentNameColumn);
            objDepartment.CompanyName = GetString(CompanyNameColumn);
            objDepartment.DepartmentHeadId = GetInt(DepartmentHeadIdColumn);
            objDepartment.DepartmentHeadName = GetString(DepartmentHeadNameColumn);
            objDepartment.IsActive = GetInt(IsActiveColumn);
            return objDepartment;
        }
        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "DEPARTMENTID":
                        {
                            DepartmentIdColumn = i;
                            break;
                        }
                    case "COMPANYID":
                        {
                            CompanyIdColumn = i;
                            break;
                        }
                    case "DEPARTMENTNAME":
                        {
                            DepartmentNameColumn = i;
                            break;
                        }
                    case "COMPANYNAME":
                        {
                            CompanyNameColumn = i;
                            break;
                        }
                    case "DEPARTMENTHEADID":
                        {
                            DepartmentHeadIdColumn = i;
                            break;
                        }
                    case "DEPARTMENTHEADNAME":
                        {
                            DepartmentHeadNameColumn = i;
                            break;
                        }
                    case "ISACTIVE":
                        {
                            IsActiveColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
