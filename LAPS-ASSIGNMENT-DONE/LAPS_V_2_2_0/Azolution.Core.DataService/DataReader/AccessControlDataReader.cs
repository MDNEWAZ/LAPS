using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class AccessControlDataReader : EntityDataReader<AccessControl>
    {
        public int AccessIdColumn = -1;
        public int AccessNameColumn = -1;
        public int GroupNameColumn = -1;

        public AccessControlDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override AccessControl Read()
        {
            var objAccessControl = new AccessControl();
            objAccessControl.AccessId = GetInt(AccessIdColumn);
            objAccessControl.AccessName = GetString(AccessNameColumn);
            return objAccessControl;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "ACCESSID":
                        {
                            AccessIdColumn = i;
                            break;
                        }
                    case "ACCESSNAME":
                        {
                            AccessNameColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
