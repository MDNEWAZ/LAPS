using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class GroupPermissionDataReader : EntityDataReader<GroupPermission>
    {
        public int PermissionIdColumn = -1;
        public int PermissionTableNameColumn = -1;
        public int GroupIDColumn = -1;
        public int ParentPermissionColumn = -1;
        public int ReferenceIdColumn = -1;

        public GroupPermissionDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override GroupPermission Read()
        {
            var objGroupPermission = new GroupPermission();
            objGroupPermission.PermissionId = GetInt(PermissionIdColumn);
            objGroupPermission.PermissionTableName = GetString(PermissionTableNameColumn);
            objGroupPermission.GroupId = GetInt(GroupIDColumn);
            objGroupPermission.ParentPermission = GetInt(ParentPermissionColumn);
            objGroupPermission.ReferenceID = GetInt(ReferenceIdColumn);
            return objGroupPermission;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "PERMISSIONID":
                        {
                            PermissionIdColumn = i;
                            break;
                        }
                    case "PERMISSIONTABLENAME":
                        {
                            PermissionTableNameColumn = i;
                            break;
                        }
                    case "GROUPID":
                        {
                            GroupIDColumn = i;
                            break;
                        }
                    case "PARENTPERMISSION":
                        {
                            ParentPermissionColumn = i;
                            break;
                        }
                    case "REFERENCEID":
                        {
                            ReferenceIdColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
