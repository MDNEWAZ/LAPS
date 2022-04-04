using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class WFStateDataReader : EntityDataReader<WFState>
    {
        public int WFStateIdColumn = -1;
        public int StateNameColumn = -1;
        public int MenuIDColumn = -1;
        public int IsDefaultStartColumn = -1;
        public int MenuNameColumn = -1;
        public int IsClosedColumn = -1;


        public WFStateDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override WFState Read()
        {
            var objWFState = new WFState();
            objWFState.MenuID = GetInt(MenuIDColumn);
            objWFState.StateName = GetString(StateNameColumn);
            objWFState.WFStateId = GetInt(WFStateIdColumn);
            try
            {
                objWFState.IsDefaultStart = GetBool(IsDefaultStartColumn);
            }
            catch
            {
                var isDefault = GetInt(IsDefaultStartColumn);
                objWFState.IsDefaultStart = isDefault == 1 ? true : false;
            }
            objWFState.MenuName = GetString(MenuNameColumn);
            objWFState.IsClosed = GetInt(IsClosedColumn);
            return objWFState;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "WFSTATEID":
                        {
                            WFStateIdColumn = i;
                            break;
                        }
                    case "STATENAME":
                        {
                            StateNameColumn = i;
                            break;
                        }
                    case "MENUID":
                        {
                            MenuIDColumn = i;
                            break;
                        }
                    case "ISDEFAULTSTART":
                        {
                            IsDefaultStartColumn = i;
                            break;
                        }
                    case "MENUNAME":
                        {
                            MenuNameColumn = i;
                            break;
                        }
                    case "ISCLOSED":
                        {
                            IsClosedColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
