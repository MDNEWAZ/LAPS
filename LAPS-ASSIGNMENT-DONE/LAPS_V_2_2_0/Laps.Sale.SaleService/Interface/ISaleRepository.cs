using System;
using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Sale.SaleService.Interface
{
   public interface ISaleRepository
    {
       string GetAllSale(GridOptions options, int userId, string invoiceNo, string month);
        string GetSaleType();
        //string SaveSale(SalesParam salesParam);
       dynamic GetSalesDetailsInfoByCustomerId(GridOptions options, int customerId);
       dynamic GetInstallmentInfoByModelId(GridOptions options, string invoice);
       dynamic GetAllInstalmentByInvoiceNo(GridOptions options, string invoiceNo);
       string GetAllSaleByMonth(GridOptions gridOptions, string invoiceNo, DateTime entryDateFrom, DateTime entryDateTo, Users user);
       string GetCompanyCode(int companyId);
       dynamic GetSalesItemInfoBySaleId(GridOptions options, int saleId);
       object GetSalesItemDataBySaleId(GridOptions options, int saleId);
       object GetItemDetailsInformationBySaleId(int saleId);
       int GetDefaultInstallmentNo(int companyId);
       Azolution.Entities.Sale.Collection GetDownPaymentCollectionInfoBySaleId(int saleId);
      // string SaveFinalSale(List<Azolution.Entities.Sale.Sale> objSalesObjList, List<License> licenseListObj, Users user);
       Azolution.Entities.Sale.Sale GetCustomerAndSaleInfoByCustomerCode(string customerCode, Users user);

       bool CheckExistInvoice(string invoiceNo);
       string GetAllBookedSale(GridOptions options, string invoiceNo, DateTime entryDateFrom, DateTime entryDateTo, Users user);
       string GetAllUnrecognizedSale(GridOptions options, string invoiceNo, Users user, string salesDateFrom, string salesDateTo);
       string SaveUnrecognizedSale(UnRecognizeSale unrecogSaleObject);
       int GetPackageWiseDefaultInstallmentNoByModelId(int modelId);
    }
}
