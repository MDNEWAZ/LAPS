using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
     internal class GroupDataReader : EntityDataReader<Group>
    {
        public int GroupIdColumn = -1;
        public int CompanyIdColumn = -1;
        public int GroupNameColumn = -1;
         public int IsDefaultColumn = -1;

          public GroupDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override Group Read()
        {
            var objGroup = new Group();
            objGroup.GroupId = GetInt(GroupIdColumn);
            objGroup.CompanyId = GetInt(CompanyIdColumn);
            objGroup.GroupName = GetString(GroupNameColumn);
            objGroup.IsDefault = GetInt(IsDefaultColumn);
            return objGroup;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "GROUPID":
                        {
                            GroupIdColumn = i;
                            break;
                        }
                    case "COMPANYID":
                        {
                            CompanyIdColumn = i;
                            break;
                        }
                    case "GROUPNAME":
                        {
                            GroupNameColumn = i;
                            break;
                        }
                    case "ISDEFAULT":
                        {
                            IsDefaultColumn = i;
                            break;
                        }
                }
            }
        }
    }
   
}
