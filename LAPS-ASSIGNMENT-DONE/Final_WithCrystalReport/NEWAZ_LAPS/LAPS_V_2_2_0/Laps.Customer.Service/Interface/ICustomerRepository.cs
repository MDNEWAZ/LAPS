using System.Collections.Generic;
using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Customer.Service.Interface
{
   public interface ICustomerRepository
   {
       string GetAllCustomer(GridOptions options,int companyId,int branchId,string customerCode);
       string GetCustomerType();
       string SaveCustomer(Azolution.Entities.Sale.Customer aCustomer);
       Azolution.Entities.Sale.Customer GetCustomerByCustomerCode(string customerCode, string phoneNo);
       string GetCustomerResult(string codeOrNationalIdOrPhone, string customerId, string ctrlId);

       Branch GetSmsMobileNumberByBranchId(int branchId);

       Sale GetProductInfoByCustomerId(int customerId);

       List<Due> GetCustomerRatingByCompanyId(int companyId);
       string CheckExistCustomerByCode(string customerCode, Users user);
       bool GetExistMobileNo(string mobileNo, Users user, int noOfMobile);

       List<Azolution.Entities.Sale.CustomerInfo> GetAllActiveCustomer();
   }
}
