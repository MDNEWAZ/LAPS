using System;

namespace Azolution.Entities.Core
{
     [Serializable]
    public class GroupMember
    {
        public GroupMember()
        {
        }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string GroupOption { get; set; }
    }
}
