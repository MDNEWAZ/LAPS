using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Collection.CollectionService.Interface
{
    public interface ISimplePaymentCollectionRepository
    {

        SimplePaymentCollection GetCustomerInfoWithInstallmentAmount(SimplePaymentCollection simplePaymentCollectionObj);

        string SaveAsDraftPayment(SimplePaymentCollection objPaymentInfo, Users user);
        object GetDraftedPaymentDataForGrid(GridOptions options, string collectionDate, Users users);

        void UpdateStatusForDrafted(int simplePaymentCollectionId);
    }
}
