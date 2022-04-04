using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.HumanResource;

namespace Azolution.HumanResource.DataService.DataReader
{

    internal class BranchDataReader : EntityDataReader<Branch>
    {
        public int BranchIdColumn = -1;
        public int CompanyIdColumn = -1;
        public int BranchNameColumn = -1;
        public int BranchDescriptionColumn = -1;
        public int IsActiveColumn = -1;
        public int BranchSmsMobileNumberColumn = -1;
        public int BranchCodeColumn = -1;

        public BranchDataReader(IDataReader reader)
            : base(reader)
        {
        }


        public override Branch Read()
        {
            var objBranch = new Branch();
            objBranch.BranchId = GetInt(BranchIdColumn);
            objBranch.CompanyId = GetInt(CompanyIdColumn);
            objBranch.BranchName = GetString(BranchNameColumn);
            objBranch.BranchDescription = GetString(BranchDescriptionColumn);
            objBranch.IsActive = GetInt(IsActiveColumn);
            objBranch.BranchSmsMobileNumber = GetString(BranchSmsMobileNumberColumn);
            objBranch.BranchCode = GetString(BranchCodeColumn);
            return objBranch;
        }


        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "BRANCHID":
                        {
                            BranchIdColumn = i;
                            break;
                        }
                    case "COMPANYID":
                        {
                            CompanyIdColumn = i;
                            break;
                        }
                    case "BRANCHNAME":
                        {
                            BranchNameColumn = i;
                            break;
                        }
                    case "BRANCHDESCRIPTION":
                        {
                            BranchDescriptionColumn = i;
                            break;
                        }
                    case "ISACTIVE":
                        {
                            IsActiveColumn = i;
                            break;
                        }
                    case "BRANCHSMSMOBILENUMBER":
                        {
                            BranchSmsMobileNumberColumn = i;
                            break;
                        }
                    case "BRANCHCODE":
                        {
                            BranchCodeColumn = i;
                            break;
                        }

                }
            }
        }
    }
}
