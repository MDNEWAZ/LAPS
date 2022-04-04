using System;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class Module
    {
        public Module()
        {
        }

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }



        public int TotalCount { get; set; }
    }
}
