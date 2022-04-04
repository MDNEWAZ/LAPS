using System;
using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Collection.CollectionService.Interface
{
    public interface ICollectionRepository
    {
        //string UpdatePaymentStatus(Collection objCollection);
        object GetPaymentType();
        object GetNextInstallmentByInvoiceNo(int invoiceNo);
        object GetAllCollection(GridOptions options, string companies);
        List<PendingCollectionChart> GetAllPendingCollections(Users userId, int companyId, int branchId, bool isAdministrator, DateTime? fromDate, DateTime? toDate, string companies);
        List<PendingCollectionChart> GetMonthWiseCollectionData(int userId, int companyId, int branchId, DateTime? fromDate, DateTime? toDate, string companies);
        List<PendingCollectionChart> GetMonthWiseSalesData(int userId, int companyId, int branchId, DateTime? fromDate, DateTime? toDate, string companies);
        object GetTenRedCustomer(GridOptions options, string invoice, bool isAdministrator, Users objUser, int companyId, int branchId, string companies);

        List<Installment> GetAllInstalmentByInvoiceNo(string invoiceNo);

        Azolution.Entities.Sale.Collection GetDownpaymentByInvoiceId(int saleId);

        //string CollectPaymentAndUpdatePaymentStatus(List<Collection> objCollection,Users user);
        string SendSmsByCustomerRatingWhenLogin(string saleInvoice, Users user);

        RatingCalculation GetDuePercentByInvoiceNo(string invoiceNo);

        GridEntity<RatingCalculation> GetDueCollectionCustomerGridData(GridOptions options, bool isAdministrator, Users objUser, int companyId, int branchId, string companies);

        GridEntity<ReleaseLisenceGenerate> GetCustomerForReleaseLisenceGridData(GridOptions options, string companies, int branchId);


        string GenerateReleaseLisenceFromRootUser(ReleaseLisenceGenerate strobjReleaseLicenseInfo, int varificationType);

        string GetPaymentAndCollect(PaymentReceivedInfo paymentInfoObj, int userId);

        GridEntity<CustomerWithLicenseCode> GetCustomerWithCodeSummary(GridOptions options, string customerCode, string smsMobileNumber);

        string ResendLicenseCodeSms(CustomerWithLicenseCode objResendSms);

        //void DownpaymentCollection(PaymentReceivedInfo objDownPayCollection, int userId);


        Azolution.Entities.Sale.Collection GetDownpaymentByInvoiceNo(string invoiceNo);
        GridEntity<CollectionDto> GetCollectionHistoryByInvoice(GridOptions options, string invoiceNo);
    }
}
