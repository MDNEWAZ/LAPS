using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class ModuleDataReader : EntityDataReader<Module>
    {
        public int ModuleIdColumn = -1;
        public int ModuleNameColumn = -1;

        public ModuleDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override Module Read()
        {
            var objModule = new Module();
            objModule.ModuleId = GetInt(ModuleIdColumn);
            objModule.ModuleName = GetString(ModuleNameColumn);
            return objModule;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "MODULEID":
                        {
                            ModuleIdColumn = i;
                            break;
                        }
                    case "MODULENAME":
                        {
                            ModuleNameColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
