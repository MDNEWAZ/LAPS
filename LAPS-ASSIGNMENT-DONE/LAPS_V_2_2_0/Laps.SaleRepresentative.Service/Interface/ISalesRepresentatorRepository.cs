using System;
using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.SaleRepresentative.Service.Interface
{
    public interface ISalesRepresentatorRepository
    {
        String SaveSalesRepresentator(SalesRepresentator objSalesRepresentator);
        string GetAllSalesRepresentator(GridOptions options);
        object GetSalesRepresentatorType();
        List<SalesRepresentator> GetAllSalesRepresentatorCombo(int salesRepType, Users user);
        object GetAllSalesRepresentatorCombo();
        object GetAllSalesRepresentatorById(string salesRepId);
        List<SalesRepresentator> GetSalesRepresentatorByCompanyAndBranch(int companyId, int branchId, Users user);
        List<SalesRepresentator> GetSalesRepComboByCompanyBranchAndType(int salesRepType, int companyId, int branchId, Users user);
    }
}
