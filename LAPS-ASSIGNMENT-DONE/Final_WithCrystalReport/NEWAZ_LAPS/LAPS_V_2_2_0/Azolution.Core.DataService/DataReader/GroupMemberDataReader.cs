using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class GroupMemberDataReader : EntityDataReader<GroupMember>
    {
        public int GroupIdColumn = -1;
        public int GroupOptionColumn = -1;
        public int UserIdColumn = -1;
        public int GroupNameColumn = -1;

        public GroupMemberDataReader(IDataReader reader)
            : base(reader)
        {
        }

        public override GroupMember Read()
        {
            var objGroupMember = new GroupMember();
            objGroupMember.GroupId = GetInt(GroupIdColumn);
            objGroupMember.UserId = GetInt(UserIdColumn);
            objGroupMember.GroupOption = GetString(GroupOptionColumn);
            return objGroupMember;
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
                    case "USERID":
                        {
                            UserIdColumn = i;
                            break;
                        }
                    case "GROUPOPTION":
                        {
                            GroupOptionColumn = i;
                            break;
                        }
                    case "GROUPNAME":
                        {
                            GroupNameColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
