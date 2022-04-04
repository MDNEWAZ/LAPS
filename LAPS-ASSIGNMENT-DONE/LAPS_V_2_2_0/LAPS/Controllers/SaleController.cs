using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Azolution.HumanResource.Service.Service;
using Laps.AdminSettings.Service.Service;
using Laps.Sale.SaleService.Interface;
using Laps.Sale.SaleService.Service;
using Laps.Stock.Service.Service;
using LapsUtility;
using Utilities;


namespace LAPS.Controllers
{
    public class SaleController : Controller
    {
        //
        // GET: /Sale/
        private ISaleRepository _aSaleRepository = new SaleService();
        ISalesProcess _saleProcess = new SalesProcess();
        ICompanyRepository _companyRepository = new CompanyService();
        public ActionResult Sale()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        public ActionResult UnrecognizedSale()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("UnrecognizedSale/UnrecognizedSale");
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        public ActionResult ReScheduleInstallment()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        public ActionResult ArmyIndex()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("~/Views/Sale/Army/ArmySettings.cshtml");

            }
            return RedirectToAction("Logoff", "Home");
        }

        public string GetSaleType()
        {
            return _aSaleRepository.GetSaleType();
        }

        public string GetAInterest()
        {
            string seccess;
            try
            {
                var user = ((Users)(Session["CurrentUser"]));
                //int companyId = user.CompanyId;
                int companyId = user.ChangedCompanyId == 0 ? user.CompanyId : user.ChangedCompanyId;
                return GetCompanySettingsInfo(companyId);
            }
            catch (Exception ex)
            {
                seccess = Operation.Failed.ToString();
            }
            return seccess;
        }

        private string GetCompanySettingsInfo(int companyId)
        {
            return _companyRepository.GetAInterest(companyId);
        }

        public JsonResult GetDefaultInstallmentNo()
        {
            var user = ((Users)(Session["CurrentUser"]));
            return Json(_aSaleRepository.GetDefaultInstallmentNo(user.CompanyId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPackageWiseDefaultInstallmentNoByModelId(int modelId)
        {
            var data = _aSaleRepository.GetPackageWiseDefaultInstallmentNoByModelId(modelId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveSale(Sale strObjSaleInfo, List<Installment> objInstallmentList, List<SalesItem> objItemInfoList, List<SalesItemInformation> objItemDetailsInfoList, PaymentReceivedInfo objDownPayCollection, Customer objCustomerInfo, Discount objDiscountInfo)
        {
            Users user = ((Users)(Session["CurrentUser"]));
            if (user.IsViewer == 1)
            {
                return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet);
            }
            string res = SaveSaleAsBooked(strObjSaleInfo, objInstallmentList, objItemInfoList,
                     objItemDetailsInfoList, objDownPayCollection, objCustomerInfo, objDiscountInfo, user);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private string SaveSaleAsBooked(Sale strObjSaleInfo, List<Installment> objInstallmentList, List<SalesItem> objItemInfoList, List<SalesItemInformation> objItemDetailsInfoList, PaymentReceivedInfo objDownPayCollection, Customer objCustomerInfo, Discount objDiscountInfo, Users user)
        {
            string res = "";
            var saleParam = new SalesParam();
            DefaultSettingsService _defaultSettingsService = new DefaultSettingsService();
            StockService _stockService = new StockService();
       
            var stockInventory = true;   //Stock inventory Branch
            var modeOperation = _defaultSettingsService.GetOperationModeSettings();
            if (modeOperation.ManualInventoryChecking == 1)  // Retail-Sale
            {   
                //Inventory Cheking from LAPS DB
                var branchCode = user.ChangedBranchCode == "" ? user.BranchCode : user.ChangedBranchCode;
                stockInventory = _stockService.CheckStockInventoryBranchByBranchIdAndModelId(Convert.ToInt32(branchCode.Substring(2, branchCode.Length - 2)), strObjSaleInfo.AProduct.ModelItemID);
            }

            if (stockInventory == true)
            {
                saleParam.SaleInfo = strObjSaleInfo;
                saleParam.ItemInfoLis = objItemInfoList;
                saleParam.ItemDetailsInfoList = objItemDetailsInfoList;
                saleParam.DownPayCollection = objDownPayCollection;
                saleParam.CustomerInfo = objCustomerInfo;
                saleParam.DiscountInfo = objDiscountInfo;
                saleParam.User = user;
                res = _saleProcess.SaveAsBooked(saleParam, false);

                if (res != "Success")
                {
                    res = "Failed";
                }

            }
            else
            {
                res = "EmptyInventoryInfo";
            }
            return res;
        }
        public ActionResult SaveSaleAsDraft(List<Sale> objSalesObjList)
        {
            var user = ((Users)(Session["CurrentUser"]));
            if (user.IsViewer == 1)
            {
                return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet);
            }
            return Json(_saleProcess.Draft(objSalesObjList, user), JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveFinalSale(List<Sale> objSalesObjList)
        {

            try
            {
                var user = ((Users)(Session["CurrentUser"]));

                if (user.IsViewer == 1) { return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet); }

                return Json(_saleProcess.Sold(objSalesObjList, user), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult MakeUnRecognized(SaleSummary sale)
        {
            var user = ((Users)(Session["CurrentUser"]));
            if (user.IsViewer == 1)
            {
                return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet);
            }
            return Json(_saleProcess.MakeUnRecognized(sale), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInvoice(string invoice)
        {
            var inv = "";
            inv = GenerateInvoiceNo(invoice);
            return Json(inv, JsonRequestBehavior.AllowGet);
        }

        private static string GenerateInvoiceNo(string invoice)
        {

            var inv = "";
            if (invoice == "")
            {
                var timespanvalue = "rr";
                var cDate = DateTime.Now;
                TimeSpan timeSpan = new TimeSpan(cDate.Day, cDate.Hour, cDate.Minute, cDate.Second);
                timespanvalue = timeSpan.TotalMilliseconds.ToString();
                timespanvalue = timespanvalue.Substring(2, 4);
                var sdate = DateTime.Now;
                var smonth = sdate.ToString("MM");
                var syear = sdate.ToString("yy");
                inv = smonth + "-" + syear + "-" + timespanvalue;
            }
            else
            {
                var lastinvoice = Regex.Split(invoice, @"\D+").Last().Trim();
                var date = DateTime.Now;
                var month = date.ToString("MM");
                var year = date.ToString("yy");
                inv = month + "-" + year + "-" + lastinvoice;
            }

            return inv;

        }

        public JsonResult GetCustomerAndSaleInfoByCustomerCode(string customerCode)
        {
            var user = ((Users)(Session["CurrentUser"]));
            return Json(_aSaleRepository.GetCustomerAndSaleInfoByCustomerCode(customerCode, user),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesItemDataBySaleId(GridOptions options, int saleId)
        {
            return Json(_aSaleRepository.GetSalesItemDataBySaleId(options, saleId), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetItemDetailsInformationBySaleId(int saleId)
        {
            return Json(_aSaleRepository.GetItemDetailsInformationBySaleId(saleId), "application/json; charset=utf-8",
                JsonRequestBehavior.AllowGet);
        }

        public string GetAllSaleByMonth(GridOptions options, string invoiceNo, DateTime entryDateFrom, DateTime entryDateTo)
        {
            try
            {
                var user = ((Users)(Session["CurrentUser"]));
                if (user == null) return null;
                return _aSaleRepository.GetAllSaleByMonth(options, invoiceNo, entryDateFrom,entryDateTo, user);
            }
            catch (Exception)
            {
                return Operation.Failed.ToString();
            }
        }

        public JsonResult GetAllInstalmentByInvoiceNo(GridOptions options, string invoiceNo)
        {
            return Json(_aSaleRepository.GetAllInstalmentByInvoiceNo(null, invoiceNo), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDownPaymentCollectionInfoBySaleId(int saleId)
        {
            return Json(_aSaleRepository.GetDownPaymentCollectionInfoBySaleId(saleId), JsonRequestBehavior.AllowGet);
        }

        public string GetAllBookedSale(GridOptions options, string invoiceNo, DateTime entryDateFrom, DateTime entryDateTo)
        {
            try
            {
                var user = ((Users)(Session["CurrentUser"]));
                if (user == null) return null;
                return _aSaleRepository.GetAllBookedSale(options, invoiceNo, entryDateFrom,entryDateTo, user);
            }
            catch (Exception)
            {
                return Operation.Failed.ToString();
            }
        }

        public string GetAllUnrecognizedSale(GridOptions options, string invoiceNo, string salesDateFrom, string salesDateTo)
        {
            try
            {
                var user = ((Users)(Session["CurrentUser"]));
                if (user == null) return null;
                return _aSaleRepository.GetAllUnrecognizedSale(options, invoiceNo, user,salesDateFrom,salesDateTo);
            }
            catch (Exception)
            {
                return Operation.Failed.ToString();
            }
        }

        public ActionResult SaveUnrecognizedSale(UnRecognizeSale unrecogSaleObject)
        {
            var user = ((Users)(Session["CurrentUser"]));
            if (user.IsViewer == 1) { return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet); }

            var res = _aSaleRepository.SaveUnrecognizedSale(unrecogSaleObject);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCompany()
        {
            var companyService = new CompanyService();
            return Json(companyService.GetAllCompaniesForCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranch(int companyId)
        {
            var branchService = new BranchService();
            return Json(branchService.GetAllBranchByCompanyIdForCombo(companyId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllInstallmentOfCustomer(string customerCode, int companyId, int branchId)
        {
            IReSchedule reSchedule = new ReSchedule();
            var data = reSchedule.GetExistingInstallmentScheduls(customerCode, companyId, branchId);
            var objList = new
            {
                Items = data,
                TotalCount = data.Count()
            };
            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MakeReSchedule(int companyId, int branchId, string customerCode, int ims, DateTime firsyPayDate)
        {
            var user = ((Users)(Session["CurrentUser"]));
            if (user.IsViewer == 1) { return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet); }

            IReSchedule reSchedule = new ReSchedule();
            var res = reSchedule.ReScheduleIm(customerCode, ims, firsyPayDate, companyId, branchId);
            var rv = res == true ? Operation.Success.ToString() : Operation.Error.ToString();
            return Json(rv, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesDetailsInfoByCustomerId(GridOptions options, int customerId)
        {
            return Json(_aSaleRepository.GetSalesDetailsInfoByCustomerId(options, customerId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesItemInfoBySaleId(GridOptions options, int saleId)
        {
            return Json(_aSaleRepository.GetSalesItemInfoBySaleId(options, saleId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInstallmentInfoByInvoice(GridOptions options, string invoice)
        {
            return Json(_aSaleRepository.GetInstallmentInfoByModelId(options, invoice), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSaleForSpecialPackage(Sale strObjSaleInfo, List<Installment> objInstallmentList, List<SalesItem> objItemInfoList, List<SalesItemInformation> objItemDetailsInfoList, PaymentReceivedInfo objDownPayCollection, Customer objCustomerInfo, Discount objDiscountInfo)  //D-Type
        {
            string res = "";

            Users user = ((Users)(Session["CurrentUser"]));
            DefaultSettingsService _defaultSettingsService = new DefaultSettingsService();
            StockService _stockService = new StockService();

            if (user.IsViewer == 1)
            {
                return Json(CommonMessage.ErroMessage, JsonRequestBehavior.AllowGet);
            }

            //Inventory Cheking from LAPS DB
            var stockInventory = true;
            var modeOperation = _defaultSettingsService.GetOperationModeSettings();
            if (modeOperation.ManualInventoryChecking == 1)  //Special D-Type Package
            {
                stockInventory = _stockService.CheckStockInventoryDealerByDealerIdAndModelId(strObjSaleInfo.SalesRepId.Substring(2, strObjSaleInfo.SalesRepId.Length - 2), strObjSaleInfo.AProduct.ModelItemID);
            }

            if (stockInventory == true)
            {
                var saleParam = new SalesParam();
                saleParam.SaleInfo = strObjSaleInfo;
                saleParam.ItemInfoLis = objItemInfoList;
                saleParam.ItemDetailsInfoList = objItemDetailsInfoList;
                saleParam.DownPayCollection = objDownPayCollection;
                saleParam.CustomerInfo = objCustomerInfo;
                saleParam.DiscountInfo = objDiscountInfo;
                saleParam.User = user;
                res = _saleProcess.SaveSaleForSpecialPackage(saleParam, false);

                if (res != Operation.Success.ToString())
                {
                    res = Operation.Failed.ToString();
                }
            }
            else
            {
                res = "EmptyInventoryInfo";
            }

            return Json(res,JsonRequestBehavior.AllowGet);
        }
    }
}
