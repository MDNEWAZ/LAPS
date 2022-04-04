using System;
using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.HumanResource;

namespace Azolution.HumanResource.DataService.DataReader
{

    internal class DesignationDataReader : EntityDataReader<Designation>
    {
        public int DesignationIdColumn = -1;
        public int DesignationNameColumn = -1;
        public int CompanyIdColumn = -1;
        public int StatusColumn = -1;
        public int DEPARTMENTIDColumn = -1;

        public DesignationDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override Designation Read()
        {
            var objDesignation = new Designation();
            objDesignation.DesignationId = GetInt(DesignationIdColumn);
            objDesignation.DesignationName = GetString(DesignationNameColumn);
            objDesignation.CompanyId = GetInt(CompanyIdColumn);
            objDesignation.Status = Convert.ToInt16(GetInt(StatusColumn));
            objDesignation.DepartmentId = GetInt(DEPARTMENTIDColumn);
            if (objDesignation.DepartmentId == int.MinValue)
            {
                objDesignation.DepartmentId = 0;
            }
            return objDesignation;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "DESIGNATIONID":
                        {
                            DesignationIdColumn = i;
                            break;
                        }
                    case "DESIGNATIONNAME":
                        {
                            DesignationNameColumn = i;
                            break;
                        }
                    case "COMPANYID":
                        {
                            CompanyIdColumn = i;
                            break;
                        }
                    case "STATUS":
                        {
                            StatusColumn = i;
                            break;
                        }
                    case "DEPARTMENTID":
                        {
                            DEPARTMENTIDColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
