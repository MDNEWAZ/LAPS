using System;
using System.Collections.Generic;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class Group
    {
        public Group()
        {
            
        }
        public int GroupId { get; set; }
        public int CompanyId { get; set; }
        public string GroupName { get; set; }
        public int IsDefault { get; set; }
        public int IsViewer { get; set; }

        public List<GroupPermission> ModuleList { get; set; }

        public List<GroupPermission> MenuList { get; set; }

        public List<GroupPermission> AccessList { get; set; }

        public List<GroupPermission> StatusList { get; set; }

        public List<GroupPermission> ActionList { get; set; }

        public int TotalCount { get; set; }
    }
}
