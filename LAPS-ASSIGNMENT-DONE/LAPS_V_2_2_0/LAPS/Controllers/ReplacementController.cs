using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Laps.Product.Service.Service;
using Laps.Replacement.Service.Interface;
using Laps.Replacement.Service.Service;
using Laps.Stock.Service.Interface;
using Laps.Stock.Service.Service;
using Solaric.GenerateCode;
using Utilities;

namespace LAPS.Controllers
{
    public class ReplacementController : Controller
    {
        //
        // GET: /Replacement/
        IReplacementRepository _replacementRepository = new ReplacementService();
        public ActionResult Replacement()
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

        public ActionResult ReplacementForCustomerService()
        {

            if (Session["CurrentUser"] != null)
            {
                return View("ReplacementForCustomerService/ReplacementForCustomerService");
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }
        public ActionResult ReplaceProduct(Replacement objReplacementInfo)
        {
            var user = ((Users)(Session["CurrentUser"]));

            return Json(_replacementRepository.ReplaceProduct(objReplacementInfo, user), JsonRequestBehavior.AllowGet);
        }
        public void GetLicence(Replacement objReplacement, List<Installment> installments)
        {
            int lType = 0;
            const decimal totalInstt = 36;
            var prantId = objReplacement.SaleId.ToString(CultureInfo.InvariantCulture);
            string code = prantId.Substring(prantId.Length - 2);
            foreach (var instt in installments)
            {
                if (instt.AProduct.MinNumber == 1)
                {
                    lType = 0;
                    string installmentId = instt.InstallmentId.ToString();
                    code += installmentId.Substring(installmentId.Length - 2); 
                }
                else if (instt.Number == totalInstt)
                {
                    lType = 2;
                    string installmentId = instt.InstallmentId.ToString();
                    code += installmentId.Substring(installmentId.Length - 2);
                }
                else
                {
                    lType = 1;
                    string installmentId = instt.InstallmentId.ToString();
                    code += installmentId.Substring(installmentId.Length - 2);
                }
            }
            ////              /////
            var mobileNo = objReplacement.ACustomer.Phone;
            var aGenerator = new LicenceService();
            objReplacement.ALicense.Number = aGenerator.GetLicence(mobileNo, code, lType);

            objReplacement.ALicense.LType = lType;
            objReplacement.ALicense.IssueDate = DateTime.Now;
        }

        private bool ValidateRequestforLicense(List<Installment> installments)
        {
            if (installments.Count!=0)
            {
                foreach (var installment in installments)
                {
                    DateTime date = Convert.ToDateTime(installment.AProduct.IssueDate).AddMonths(1);
                    DateTime toDay = DateTime.Now;
                    if (date <= toDay)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public ActionResult GetReplacementInfoByInvoiceNo(GridOptions options,string invoiceNo)
        {
            
            return Json(_replacementRepository.GetReplacementInfoByInvoiceNo(options,invoiceNo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReplacementInvoice(string invoice)
        {
            var inv = "";
            if (string.IsNullOrEmpty(invoice))
                return Json(inv, JsonRequestBehavior.AllowGet);
            var lastinvoice = Regex.Split(invoice, @"\D+").Last().Trim();
            var date = DateTime.Now;
            var month = date.ToString("MM");
            var year = date.ToString("yy");
            inv = "RE-"+month + "-" + year + "-" + lastinvoice;
            return Json(inv, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveReplacementForCustomerService(ReplacementCs objReplacementCs, List<ReplaceItem> objItemList)
        {
            var user = ((Users)(Session["CurrentUser"]));
            return Json(_replacementRepository.SaveReplacementForCustomerService(objReplacementCs,objItemList, user),JsonRequestBehavior.AllowGet);
        }

        public string GetProductDetailsInfor(GridOptions options,int modelId)
        {
            var productService = new ProductService();
            var data = productService.GetAllProductItemByModelId(options, modelId);
            return data; //Json(data,JsonRequestBehavior.AllowGet);
        }
        public JsonResult checkExistStockBalanceByItemId(int itemId, int stockCategoryId,int branchId)
        {
            return Json(_replacementRepository.checkExistStockBalanceByItemId(itemId, branchId, stockCategoryId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerAndPackageInfoByCustomerCode(string customerCode)
        {
            var user = ((Users)(Session["CurrentUser"]));
            return Json(_replacementRepository.GetCustomerAndPackageInfoByCustomerCode(customerCode, user),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPackageSaleInfo(int modelId,string customerCode)
        {
            return Json(_replacementRepository.GetPackageSaleInfo(modelId,customerCode), JsonRequestBehavior.AllowGet);
        }
        
    }
}
