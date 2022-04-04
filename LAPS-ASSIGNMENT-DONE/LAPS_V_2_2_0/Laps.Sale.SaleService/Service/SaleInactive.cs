using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.AdminSettings.Service.Service;
using Laps.Sale.SaleDataService.DataService;
using Laps.Sale.SaleService.Interface;
using LapsUtility;
using Utilities;

namespace Laps.Sale.SaleService.Service
{
    public class SaleInactive : ISalesInactive
    {
        private readonly SaleInactiveDataService _dataService;
        public SaleInactive()
        {
            _dataService = new SaleInactiveDataService();
        }

        public List<Azolution.Entities.Sale.Sale> GetCustomerSalesInformation(string customerCode, int branchId, int companyId)
        {
            try
            {
                return _dataService.GetCustomerSalesInformation(customerCode, branchId, companyId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string MakeInactive(int saleId, string invoice)
        {
            var connection = new CommonConnection();
            string rv = "";
            try
            {
                if (CheckEligibilityForInactive(saleId, invoice))
                {
                    connection.BeginTransaction();

                    RollBackDpCollection(saleId, invoice, connection);

                    _dataService.SetSaleStateToBooked(saleId, connection);
                    _dataService.RolleBackInstallment(invoice, connection);
                    _dataService.DeleteLicenseOfSale(invoice, connection);

                    connection.CommitTransaction();
                    rv = Operation.Success.ToString();

                }
                else
                {
                    rv = "AccessDenied";
                }

            }
            catch (Exception ex)
            {
                connection.RollBack();
                rv = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return rv;
        }

        private void RollBackDpCollection(int saleId, string invoice, CommonConnection connection)
        {

            List<Azolution.Entities.Sale.Sale> salesList = _dataService.GetSaleInformationByInvoice(invoice);
            List<Azolution.Entities.Sale.Collection> collectionList = _dataService.GetAllCollectedDownPayByInvoice(invoice);

            
            var removeCollectionList = new List<Azolution.Entities.Sale.Collection>();
            var restCollectionList = new List<Azolution.Entities.Sale.Collection>();

            restCollectionList = MyClone.CloneList(collectionList);


            if (salesList != null && salesList.Count > 1)
            {
                salesList.RemoveAll(s => s.SaleId == saleId);


                var totalDownPay = salesList.Sum(s => s.DownPay);

                decimal totalCollectedDp = 0;

                if (collectionList != null)

                    foreach (var collection in collectionList)
                    {
                        totalCollectedDp += collection.ReceiveAmount;

                        if (totalDownPay >= totalCollectedDp)
                        {
                            removeCollectionList.Add(collection);
                        }
                    }


                foreach (var rmvcollection in removeCollectionList)
                {
                    foreach (var collection in collectionList)
                    {
                        restCollectionList.RemoveAll(r => r.CollectionId == rmvcollection.CollectionId);
                    }

                }

            }
            else
            {
                restCollectionList = collectionList;
            }
            if (restCollectionList != null && restCollectionList.Any())
            {
                GetBackupIntoTempAndDeleteCollection(restCollectionList, connection);
            }
            

        }

        private void GetBackupIntoTempAndDeleteCollection(List<Azolution.Entities.Sale.Collection> collectionList, CommonConnection connection)
        {
            var collectionObj = new Azolution.Entities.Sale.Collection();
            collectionObj.SaleInvoice = collectionList.FirstOrDefault().SaleInvoice;
            collectionObj.ReceiveAmount = collectionList.Sum(c => c.ReceiveAmount);
            collectionObj.PayDate = collectionList.FirstOrDefault().PayDate;
            _dataService.SaveCollectionIntoTemp(collectionObj, connection);
            _dataService.DeleteAllCollectionData(collectionList, connection);
        }

        public string SaleBranchSwitch(SaleRollback rollbackInfo, Users user)
        {
            string rv = string.Empty;
            //if (CheckIsPackageAvailable(rollbackInfo.ModelId, rollbackInfo.ChangedCompanyId))
            //{
            PhoneSettingsService _phoneSettingsService = new PhoneSettingsService();
            rollbackInfo.SimNumber = _phoneSettingsService.GetRandomPhoneNumber();

                rv = _dataService.SaleBranchSwitch(rollbackInfo, user);
            //}
            //else
            //{
            //    rv = "Package Not Available";
            //}
            return rv;
        }

        private bool CheckIsPackageAvailable(int modelId, int changedCompanyId)
        {
            return _dataService.CheckIsPackageAvailable(modelId, changedCompanyId);
        }

        private bool CheckEligibilityForInactive(int saleId, string invoice)
        {
            try
            {
                //if (_dataService.CheckDP(saleId))
                //{
                return _dataService.CheckInstallment(invoice);
                //}
                //return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
