using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Collection.CollectionDataService.DataService;
using Laps.Collection.CollectionService.Interface;
using Utilities;

namespace Laps.Collection.CollectionService.Service
{
    public class SimplePaymentCollectionService : ISimplePaymentCollectionRepository
    {
        SimplePaymentCollectionDataService dataService = new SimplePaymentCollectionDataService();
        public SimplePaymentCollection GetCustomerInfoWithInstallmentAmount(SimplePaymentCollection simplePaymentCollectionObj)
        {

            if (!CheckExistDraftData(simplePaymentCollectionObj.CustomerCode))
            {
                return dataService.GetCustomerInfoWithInstallmentAmount(simplePaymentCollectionObj);
            }
            else
            {
                return null;
            }
        }

        private bool CheckExistDraftData(string customerCode)
        {
            return dataService.CheckExistDraftData(customerCode);
        }

        public string SaveAsDraftPayment(SimplePaymentCollection objPaymentInfo, Users user)
        {
            return dataService.SaveAsDraftPayment(objPaymentInfo, user);
        }

        public object GetDraftedPaymentDataForGrid(GridOptions options, string collectionDate, Users users)
        {
            return dataService.GetDraftedPaymentDataForGrid(options, collectionDate, users);
        }

        public void UpdateStatusForDrafted(int simplePaymentCollectionId)
        {
             dataService.UpdateStatusForDrafted(simplePaymentCollectionId);

        }
    }
}
