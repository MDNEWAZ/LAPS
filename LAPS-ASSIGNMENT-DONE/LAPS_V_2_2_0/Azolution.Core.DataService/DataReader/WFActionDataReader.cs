using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class WFActionDataReader : EntityDataReader<WFAction>
    {
        public int WFActionIdColumn = -1;
        public int ActionNameColumn = -1;
        public int WFStateIdColumn = -1;
        public int StateNameColumn = -1;
        public int NextStateIdColumn = -1;
        public int NextStateNameColumn = -1;
        public int EmailNotificationColumn = -1;
        public int SMSNotificationColumn = -1;

        public WFActionDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override WFAction Read()
        {
            var objWFAction = new WFAction();
            objWFAction.ActionName = GetString(ActionNameColumn);
            objWFAction.NextStateId = GetInt(NextStateIdColumn);
            objWFAction.WFActionId = GetInt(WFActionIdColumn);
            objWFAction.WFStateId = GetInt(WFStateIdColumn);
            objWFAction.StateName = GetString(StateNameColumn);
            objWFAction.NextStateName = GetString(NextStateNameColumn);
            objWFAction.EmailNotification = GetInt(EmailNotificationColumn);
            objWFAction.SMSNotification = GetInt(SMSNotificationColumn);
            return objWFAction;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "WFACTIONID":
                        {
                            WFActionIdColumn = i;
                            break;
                        }
                    case "WFSTATEID":
                        {
                            WFStateIdColumn = i;
                            break;
                        }
                    case "ACTIONNAME":
                        {
                            ActionNameColumn = i;
                            break;
                        }
                    case "NEXTSTATEID":
                        {
                            NextStateIdColumn = i;
                            break;
                        }
                    case "STATENAME":
                        {
                            StateNameColumn = i;
                            break;
                        }
                    case "NEXTSTATENAME":
                        {
                            NextStateNameColumn = i;
                            break;
                        }
                    case "EMAIL_ALERT":
                        {
                            EmailNotificationColumn = i;
                            break;
                        }
                    case "SMS_ALERT":
                        {
                            SMSNotificationColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
