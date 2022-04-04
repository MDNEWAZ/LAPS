using Azolution.Entities.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Laps.Army.Services.Interface
{
    public interface IArmyRepository
    {
        List<ArmyGroups> PopulateArmyGroupCombo();
       
        string SaveArmyInfo(ArmyInfo army);
        GridEntity<ArmyInfo> ArmyInfoGrid(GridOptions options);
       string Delete(int ArmyId);

       
    }
}
