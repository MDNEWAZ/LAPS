using System;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class AccessControl
    {
        public AccessControl()
        {
        }
        public int AccessId { get; set; }
        public string AccessName { get; set; }
        public int TotalCount { get; set; }

    }
}
