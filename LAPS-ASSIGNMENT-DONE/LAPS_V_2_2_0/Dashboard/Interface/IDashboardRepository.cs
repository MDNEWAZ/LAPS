using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Azolution.Dashboard.Service.Interface
{
   public interface IDashboardRepository
    {
       List<PendingCollectionChart> GetAllPendingCollectionsForDashBoard(Users objUser, int companyId, int branchId, bool isAdministrator, DateTime? fromDate, DateTime? toDate, string companies);
       List<PendingCollectionChart> GetMonthWiseCollectionData(int userId, int companyId, int branchId, DateTime? fromDate, DateTime? toDate, string companies);
       List<PendingCollectionChart> GetMonthWiseSalesData(int userId, int companyId, int branchId, DateTime? fromDate, DateTime? toDate, string companies);
       GridEntity<RatingCalculation> GetDueCollectionCustomerGridData(GridOptions options, bool isAdministrator, Users objUser, int companyId, int branchId, string companies);
       object GetCusomerRatingInfoData(GridOptions options, string invoice, bool isAdministrator, Users objUser, int companyId, int branchId, string companies);
    }
}
