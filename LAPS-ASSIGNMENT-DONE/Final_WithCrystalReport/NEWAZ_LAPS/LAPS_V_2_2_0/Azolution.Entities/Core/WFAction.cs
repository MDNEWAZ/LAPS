using System;

namespace Azolution.Entities.Core
{
    [Serializable]
    public class WFAction
    {
        public WFAction()
        {
        }

        public int WFActionId { get; set; }
        public string ActionName { get; set; }
        public int WFStateId { get; set; }
        public string StateName { get; set; }
        public int NextStateId { get; set; }
        public string NextStateName { get; set; }
        public int EmailNotification { get; set; }
        public int SMSNotification { get; set; }
        public int IsDefaultStart { get; set; }
        public int IsClosed { get; set; }
    }
}
