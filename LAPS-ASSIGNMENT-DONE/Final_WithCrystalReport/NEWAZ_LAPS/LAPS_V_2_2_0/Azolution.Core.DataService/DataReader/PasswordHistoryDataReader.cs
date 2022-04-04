using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class PasswordHistoryDataReader : EntityDataReader<PasswordHistory>
    {
        public int HistoryIdColumn = -1;
        public int UserIdColumn = -1;
        public int OldPasswordColumn = -1;
        public int PasswordChangeDateColumn = -1;

        public PasswordHistoryDataReader(IDataReader reader)
            : base(reader)
        {
        }

        public override PasswordHistory Read()
        {
            var objPasswordHistory = new PasswordHistory();
            objPasswordHistory.HistoryId = GetInt(HistoryIdColumn);
            objPasswordHistory.UserId = GetInt(UserIdColumn);
            objPasswordHistory.OldPassword = GetString(OldPasswordColumn);
            objPasswordHistory.PasswordChangeDate = GetDate(PasswordChangeDateColumn);
            return objPasswordHistory;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "HISTORYID":
                        {
                            HistoryIdColumn = i;
                            break;
                        }
                    case "USERID":
                        {
                            UserIdColumn = i;
                            break;
                        }
                    case "OLDPASSWORD":
                        {
                            OldPasswordColumn = i;
                            break;
                        }
                    case "PASSWORDCHANGEDATE":
                        {
                            PasswordChangeDateColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
