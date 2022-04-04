using System;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class GroupPermission
    {
        public GroupPermission()
        {
            
        }

        public int PermissionId { get; set; }
        public string PermissionTableName { get; set; }
        public int GroupId { get; set; }
        public int ReferenceID { get; set; }
        public int ParentPermission { get; set; }

        
    }
}
