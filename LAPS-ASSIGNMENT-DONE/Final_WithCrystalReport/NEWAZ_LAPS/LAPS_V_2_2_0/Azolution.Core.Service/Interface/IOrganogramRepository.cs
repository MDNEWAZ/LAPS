using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;

namespace Azolution.Core.Service.Interface
{
    public interface IOrganogramRepository
    {
        List<Company> GetOrganogramTreeData(int companyId);
    }
}
