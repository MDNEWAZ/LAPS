using System;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class WFState
    {
        public WFState()
        {
        }

        public int WFStateId { get; set; }
        public string StateName { get; set; }
        public int MenuID { get; set; }
        public bool IsDefaultStart { get; set; }
        public string MenuName { get; set; }
        public int IsClosed { get; set; }

        //public Menu menu { get; set; }


        public int TotalCount { get; set; }
    }
}
