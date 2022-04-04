using Azolution.Entities.Sale;
using Laps.Army.DataServices;
using Laps.Army.Services.Interface;
using System.Collections.Generic;
using Utilities;

namespace Laps.Army.Services.Services
{
    public class ArmyServices : IArmyRepository
    {
        public ArmyDataServices armyDataServices = new ArmyDataServices();
        public List<ArmyGroups> PopulateArmyGroupCombo()
        {
            var data = armyDataServices.PopulateArmyGroupCombo();
            return data ;
        }
     

        public string SaveArmyInfo(ArmyInfo army)
        {
            return armyDataServices.SaveArmyInfo(army);
        }
        public GridEntity<ArmyInfo> ArmyInfoGrid(GridOptions options)
        {
            return armyDataServices.ArmyInfoGrid(options);
        }

        public string Delete(int ArmyId)
        {
            return armyDataServices.Delete(ArmyId);
        }

       
    }
}
