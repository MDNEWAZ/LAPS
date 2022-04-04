using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;

namespace Laps.AdminSettings.Service.Interface
{
   public interface ICommonRepository
    {
       CompanyHeirarchyPath GetCompanyHeirarchyPathData(int companyId, int branchId);
    }
}
