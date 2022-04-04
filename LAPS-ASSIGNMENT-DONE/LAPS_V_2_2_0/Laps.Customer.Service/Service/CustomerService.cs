using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Azolution.Entities.Sale;
using DataService.DataService;
using Laps.Customer.Service.Interface;
using Utilities;
using Utilities.Common.Json;

namespace Laps.Customer.Service.Service
{
    public class CustomerService : ICustomerRepository
    {
        private readonly CustomerDataService _aCustomerDataService = new CustomerDataService();
        public string GetAllCustomer(GridOptions options, int companyId, int branchId, string customerCode)
        {
            var jsonHelper = new JsonHelper();
            string condition = "";
            if(companyId !=0)
            {
                condition = " Where Sale_Customer.CompanyId=" + companyId;
            }
            if(branchId !=0)
            {
                condition += " And Sale_Customer.BranchId=" + branchId;
            }
            if(customerCode !="")
            {
                condition += " And Sale_Customer.CustomerCode='"+ customerCode+"'";
            }
            var data = _aCustomerDataService.GetAllCustomer(options, condition);
            return jsonHelper.GetJson(data);
        }

        public string GetCustomerType()
        {
            var aHelper = new JsonHelper();
            var data = _aCustomerDataService.GetCustomerType();
            return aHelper.GetJson(data);
        }

        public string SaveCustomer(Azolution.Entities.Sale.Customer aCustomer)
        {
            return _aCustomerDataService.SaveCustomer(aCustomer);
        }

        public Azolution.Entities.Sale.Customer GetCustomerByCustomerCode(string customerCode, string phoneNo)
        {
            string condition = "";
            if (customerCode != "")
            {
                condition = " Where CustomerCode='"+ customerCode.Trim()+"'";
            }
            if (phoneNo != "")
            {
                condition = " Where Phone='" +phoneNo.Trim() + "'";
            }
            if (condition != "")
            {
                condition = condition + " And SC.IsActive=1";
            }

            return _aCustomerDataService.GetCustomerByCustomerCode(condition);
        }
       
        public string GetCustomerResult(string codeOrNationalIdOrPhone, string customerId, string ctrlId)
        {
            string condition = "";
            if (ctrlId == "NationalId")
            {
                condition = string.Format(@"where NID = '{0}' And IsActive=1", codeOrNationalIdOrPhone);
            }
            else if (ctrlId == "SMSMobileNumber")
            {
                condition = string.Format(@"where Phone = '{0}' And IsActive=1", codeOrNationalIdOrPhone);
            }
            else if (ctrlId == "CustomerCode")
            {

                condition = string.Format(@"where CustomerCode = '{0}' And IsActive=1", codeOrNationalIdOrPhone);
               
            }

            if (customerId != "0" && condition != "")
            {
                condition += string.Format(@" And CustomerId != {0}", customerId);
            }
            return _aCustomerDataService.GetCustomerResult(condition);
        }

        public Branch GetSmsMobileNumberByBranchId(int branchId)
        {
            return _aCustomerDataService.GetSmsMobileNumberByBranchId(branchId);
        }

        public Sale GetProductInfoByCustomerId(int customerId)
        {
            return _aCustomerDataService.GetProductInfoByCustomerId(customerId);
        }

        public List<Due> GetCustomerRatingByCompanyId(int companyId)
        {
            return _aCustomerDataService.GetCustomerRatingByCompanyId(companyId);
        }

        public string CheckExistCustomerByCode(string customerCode, Users user)
        {
            return _aCustomerDataService.CheckExistCustomerByCode(customerCode,user);
        }

        public bool GetExistMobileNo(string mobileNo, Users user, int noOfMobile)
        {
            return _aCustomerDataService.GetExistMobileNo(mobileNo, user,noOfMobile);
        }

        public List<Azolution.Entities.Sale.CustomerInfo> GetAllActiveCustomer()
        {
            return _aCustomerDataService.GetAllActiveCustomer();
        }

        public Azolution.Entities.Sale.Sale GetCustomerAndSaleInfoByCustomerCode(string condition)  //Only for Customer
        {

            return _aCustomerDataService.GetCustomerAndSaleInfoByCustomerCode(condition);
        }
    }
}
