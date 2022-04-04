using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class MenuDataReader : EntityDataReader<Menu>
    {
        public int MenuIdColumn = -1;
        public int ModuleIdColumn = -1;
        public int MenuNameColumn = -1;
        public int MenuPathColumn = -1;
        public int ParentMenuColumn = -1;
        public int ModuleNameColumn = -1;
        public int ToDoColumn = -1;
        public int SortOrderColumn = -1;
        public int ParentMenuNameColumn = -1;

        public MenuDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override Menu Read()
        {
            var objMenu = new Menu();
            objMenu.MenuId = GetInt(MenuIdColumn);
            objMenu.MenuName = GetString(MenuNameColumn);
            objMenu.MenuPath = GetString(MenuPathColumn);
            objMenu.ModuleId = GetInt(ModuleIdColumn);
            if (objMenu.ModuleId == int.MinValue)
            {
                objMenu.ModuleId = 0;
            }
            objMenu.ParentMenu = GetInt(ParentMenuColumn);
            if (objMenu.ParentMenu == int.MinValue)
            {
                objMenu.ParentMenu = 0;
            }
            objMenu.ModuleName = GetString(ModuleNameColumn);
            objMenu.ToDo = GetInt(ToDoColumn);
            if(objMenu.ToDo == int.MinValue)
            {
                objMenu.ToDo = 0;
            }

            objMenu.SortOrder = GetInt(SortOrderColumn);
            if (objMenu.SortOrder == int.MinValue)
            {
                objMenu.SortOrder = 0;
            }
            objMenu.ParentMenuName = GetString(ParentMenuNameColumn);

            return objMenu;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "MENUID":
                        {
                            MenuIdColumn = i;
                            break;
                        }
                    case "MODULEID":
                        {
                            ModuleIdColumn = i;
                            break;
                        }
                    case "MENUNAME":
                        {
                            MenuNameColumn = i;
                            break;
                        }
                    case "MENUPATH":
                        {
                            MenuPathColumn = i;
                            break;
                        }
                    case "PARENTMENU":
                        {
                            ParentMenuColumn = i;
                            break;
                        }
                    case "MODULENAME":
                        {
                            ModuleNameColumn = i;
                            break;
                        }
                    case "TODO":
                        {
                            ToDoColumn = i;
                            break;
                        }
                    case "SORORDER":
                        {
                            SortOrderColumn = i;
                            break;
                        }
                    case "PARENTMENUNAME":
                        {
                            ParentMenuNameColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
