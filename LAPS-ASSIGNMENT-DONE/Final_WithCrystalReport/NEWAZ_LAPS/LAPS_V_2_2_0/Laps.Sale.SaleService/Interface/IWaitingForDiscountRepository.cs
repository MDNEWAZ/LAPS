using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Sale.SaleService.Interface
{
    public interface IWaitingForDiscountRepository
    {
        GridEntity<WaitingForDiscountSummaryDto> GetWaitingForDiscountSummary(GridOptions options, string companies);

        string ApproveWaitingForDiscount(WaitingForDiscount objWaitingForDiscount, Users user, int dpApplicabeStage);
        object GetDiscountInfoByType(Users user);
        object GetDiscountInfo(int saleId);
        object GetDiscountTypeCombo();
    }
}
