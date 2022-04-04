using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
//using CrystalDecisions.CrystalReports.ViewerObjectModel;
using Laps.Customer.Service.Interface;
using Laps.Customer.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        readonly ICustomerRepository _aCustomerRepository =new CustomerService();
        public ActionResult Customer() 
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

     

        public string GetAllCustomer(GridOptions options,int companyId, int branchId, string customerCode)
        {
            return _aCustomerRepository.GetAllCustomer(options,companyId,branchId,customerCode);
        }

        public string GetCustomerType()
        {
            return _aCustomerRepository.GetCustomerType(); 
        }

        public ActionResult SaveCustomer(Customer strObjCustomerInfo)
        {
            return Json(_aCustomerRepository.SaveCustomer(strObjCustomerInfo),JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerByCustomerCode(string customerCode, string phoneNo)
        {
            return Json(_aCustomerRepository.GetCustomerByCustomerCode(customerCode,phoneNo),JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerResult(string codeOrNationalIdOrPhone, string customerId, string ctrlId)
        {
            return Json(_aCustomerRepository.GetCustomerResult(codeOrNationalIdOrPhone, customerId, ctrlId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSmsMobileNumberByBranchId (int branchId)
        {
            return Json(_aCustomerRepository.GetSmsMobileNumberByBranchId(branchId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductInfoByCustomerId(int customerId)
        {
            return Json(_aCustomerRepository.GetProductInfoByCustomerId(customerId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerRatingByCompanyId(int companyId)
        {
            Users user = (Users) (Session["CurrentUser"]);
            if(companyId ==0)
            {
                companyId = user.CompanyId;
            }

            return Json(_aCustomerRepository.GetCustomerRatingByCompanyId(companyId), JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckExistCustomerByCode(string customerCode)
        {
            Users user = (Users)(Session["CurrentUser"]);
            return Json(_aCustomerRepository.CheckExistCustomerByCode(customerCode, user), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetExistMobileNo(string mobileNo, int noOfMobile)
        {
            Users user = (Users)(Session["CurrentUser"]);
            return Json(_aCustomerRepository.GetExistMobileNo(mobileNo, user, noOfMobile), JsonRequestBehavior.AllowGet);
         
        }

        public JsonResult GetAllActiveCustomer()
        {
            var data = _aCustomerRepository.GetAllActiveCustomer();
            return Json(data,JsonRequestBehavior.AllowGet);
        }
      
    }
}

