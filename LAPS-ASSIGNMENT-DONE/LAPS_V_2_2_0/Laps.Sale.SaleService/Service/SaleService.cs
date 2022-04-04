using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Collection.CollectionDataService.DataService;
using Laps.Collection.CollectionService.Service;
using DataService.DataService;
using Laps.Sale.SaleService.Interface;
using Laps.Stock.DataService.DataService;
using Utilities;
using Utilities.Common.Json;

namespace Laps.Sale.SaleService.Service
{
    public class SaleService : ISaleRepository
    {
        private readonly SaleDataService.DataService.SaleDataService _aService = new SaleDataService.DataService.SaleDataService();
        private JsonHelper jsonHelper;
        public string GetAllSale(GridOptions options, int userId, string invoiceNo, string month)
        {
            jsonHelper = new JsonHelper();
            string condition = "";
            if (invoiceNo != "")
            {
                condition = string.Format(@" And S.Invoice='{0}'", invoiceNo);
            }

            var data = _aService.GetAllSale(options, userId, condition);
            return jsonHelper.GetJson(data);
        }

        public string GetSaleType()
        {
            jsonHelper = new JsonHelper();
            var data = _aService.GetSaleType();
            return jsonHelper.GetJson(data);
        }


        private bool CheckDuplicateInvoiceNo(Azolution.Entities.Sale.Sale aSale)
        {
            string condition = "";
            if (aSale.SaleId == 0)
            {
                condition = " Where Invoice = '" + aSale.Invoice + "'";
            }
            if (aSale.SaleId > 0)
            {
                condition = " Where Invoice = '" + aSale.Invoice + "' And SaleId != " + aSale.SaleId;
            }

            var res = _aService.CheckExistInvoice(condition);
            return res;
        }

        public dynamic GetSalesDetailsInfoByCustomerId(GridOptions options, int customerId)
        {
            var data = _aService.GetSalesDetailsInfoByCustomerId(options, customerId);
            return data;
        }

        public dynamic GetInstallmentInfoByModelId(GridOptions options, string invoice)
        {
            return _aService.GetInstallmentInfoByModelId(options, invoice);
        }

        public dynamic GetAllInstalmentByInvoiceNo(GridOptions options, string invoiceNo)
        {
            return _aService.GetAllInstallmentByInvoiceNo(options, invoiceNo);
        }

        public string GetAllSaleByMonth(GridOptions gridOptions, string invoiceNo, DateTime entryDateFrom, DateTime entryDateTo, Users user)
        {
            jsonHelper = new JsonHelper();
            var condition = string.Empty;
            if (entryDateFrom != DateTime.MinValue && entryDateTo != DateTime.MinValue)
            {
                condition = string.Format(@" And ( S.EntryDate Between '" + entryDateFrom.ToString("dd-MMM-yyyy") + "' And '" + entryDateTo.ToString("dd-MMM-yyyy") + "')");
            }
            if (!string.IsNullOrEmpty(invoiceNo.Trim()))
            {
                condition += string.Format("AND (S.Invoice='{0}' or SC.Phone2 like '%{0}%')", invoiceNo.Trim());
            }
            return jsonHelper.GetJson(_aService.GetAllSaleByMonth(null, condition, user));
        }

        public string GetCompanyCode(int companyId)
        {
            return _aService.GetCompanyCode(companyId);
        }

        public dynamic GetSalesItemInfoBySaleId(GridOptions options, int saleId)
        {
            return _aService.GetSalesItemInfoBySaleId(options, saleId);
        }

        public object GetSalesItemDataBySaleId(GridOptions options, int saleId)
        {
            return _aService.GetSalesItemDataBySaleId(options, saleId);
        }

        public object GetItemDetailsInformationBySaleId(int saleId)
        {
            return _aService.GetItemDetailsInformationBySaleId(saleId);
        }

        public int GetDefaultInstallmentNo(int companyId)
        {
            return _aService.GetDefaultInstallmentNo(companyId);
        }

        public Azolution.Entities.Sale.Collection GetDownPaymentCollectionInfoBySaleId(int saleId)
        {
            return _aService.GetDownPaymentCollectionInfoBySaleId(saleId);
        }

     
        public Azolution.Entities.Sale.Sale GetCustomerAndSaleInfoByCustomerCode(string customerCode, Users user)
        {
            string condition = "";
            int companyId = 0;
            //if (user != null)
            //{
            //    var companyList = user.CompanyList;
            //    foreach (var company in companyList)
            //    {

            //        companyId += company.CompanyId;
            //        companyId += ",";
            //    }
            //    companyId = companyId.Remove(companyId.Length - 1);
            //}

            if (companyId !=0)
            {
                condition = string.Format(@" Where CustomerCode='{0}' And Sale.CompanyId in ({1})", customerCode, companyId);
            }
            else
            {
                condition = string.Format(@" Where CustomerCode='{0}'",customerCode);
            }

            return _aService.GetCustomerAndSaleInfoByCustomerCode(condition);
        }

        public bool CheckExistInvoice(string invoiceNo)
        {
            return _aService.IsDuplicateInvoiceNo(invoiceNo);
        }

        public string GetAllBookedSale(GridOptions options, string invoiceNo, DateTime entryDateFrom, DateTime entryDateTo, Users user)
        {
            jsonHelper = new JsonHelper();
            var condition = string.Empty;
            if (entryDateFrom != DateTime.MinValue && entryDateTo != DateTime.MinValue)
            {
                condition = string.Format(@" And ( S.EntryDate Between '" + entryDateFrom.ToString("dd-MMM-yyyy") + "' And '"+entryDateTo.ToString("dd-MMM-yyyy")+"')");
               
            }
            if (!string.IsNullOrEmpty(invoiceNo.Trim()))
            {
                condition += string.Format("AND (S.Invoice='{0}' or SC.Phone2 like '%{0}%')", invoiceNo.Trim());
            }
            return jsonHelper.GetJson(_aService.GetAllBookedSale(null, condition, user));
        }

        public string GetAllUnrecognizedSale(GridOptions options, string invoiceNo, Users user, string salesDateFrom, string salesDateTo)
        {
            jsonHelper = new JsonHelper();
            var condition = string.Empty;
            if (!string.IsNullOrEmpty(invoiceNo.Trim()))
            {
                condition += string.Format("AND S.Invoice='{0}' or SC.Phone2 like '%{0}%')", invoiceNo.Trim());
            }
            if (salesDateFrom != "null" && salesDateTo != "null")
            {
                condition += string.Format(@" AND cast(PayDate as date) between '{0}' and '{1}'", salesDateFrom,
                    salesDateTo);
            }
            return jsonHelper.GetJson(_aService.GetAllUnrecognizedSale(null, condition, user));
        }

        public string SaveUnrecognizedSale(UnRecognizeSale unrecogSaleObj)
        {
            return _aService.SaveUnrecognizedSale(unrecogSaleObj);
        }

        public int GetPackageWiseDefaultInstallmentNoByModelId(int modelId)
        {
            return _aService.GetPackageWiseDefaultInstallmentNoByModelId(modelId);
        }

    }
}
