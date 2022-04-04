using System;
using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Azolution.Entities.Sale;
using Utilities;

namespace DataService.DataService
{
    public class CustomerDataService
    {

        private CommonConnection connection;
        public CustomerDataService()
        {

            connection = new CommonConnection();
        }

        public GridEntity<Customer> GetAllCustomer(GridOptions options, string condition)
        {
            try
            {
                string query = string.Format("SELECT Sale_Customer.*,Branch.BranchSmsMobileNumber FROM Sale_Customer left outer join Branch on Branch.BRANCHID = Sale_Customer.BranchId {0}", condition);
                var data = Kendo<Azolution.Entities.Sale.Customer>.Grid.GenericDataSource(options, query, "Name");
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object GetCustomerType()
        {
            try
            {
                string query = string.Format("SELECT TypeId,Type FROM Sale_AllType Where Flag='{0}'", "Customer");
                var data = Kendo<AllType>.Combo.DataSource(query);
                return data;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string SaveCustomer(Customer aCustomer)
        {
            try
            {
                string query;
                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");
                var updatedate = DateTime.Now.ToString("dd-MMM-yyyy");
                string condition;
                if (aCustomer.CustomerId == 0)
                {
                    condition = " Where CustomerCode=" + aCustomer.CustomerCode + " And BranchId= " + aCustomer.BranchId;
                }
                else
                {
                    condition = string.Format(@" Where CustomerCode={0} And CustomerId!={1} And BranchId !={2}", aCustomer.CustomerCode, aCustomer.CustomerId, aCustomer.BranchId);
                }

                var res = GetExistCustomerCode(condition);
                if (res > 0)
                {
                    return Operation.Exists.ToString();
                }

                if (aCustomer.CustomerId == 0)
                {
                    query = string.Format(@"INSERT INTO Sale_Customer(Name,FatherName,CustomerCode,Address,District,Thana,DOB,NID,Gender,Phone
                                ,EntryDate,UpdateDate,CompanyId,BranchId,IsActive,Phone2,ReferenceId,ProductId)
                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},{13},{14},'{15}','{16}','{17}')",
                            aCustomer.Name, aCustomer.FatherName, aCustomer.CustomerCode, aCustomer.Address, aCustomer.District, aCustomer.Thana, aCustomer.Dob,
                            aCustomer.NId, aCustomer.Gender, aCustomer.Phone, entrydate, "", aCustomer.CompanyId, aCustomer.BranchId, aCustomer.IsActive, aCustomer.Phone2, aCustomer.Phone2, aCustomer.ProductId);

                }
                else
                {
                    query = string.Format(@"Update Sale_Customer Set Name='{0}',FatherName='{1}',CustomerCode='{2}',Address='{3}',District='{4}',Thana='{5}',DOB='{6}',NID='{7}',Gender='{8}',Phone='{9}'
                                ,EntryDate='{10}',UpdateDate='{11}',CompanyId={12},BranchId={13},IsActive={14},Phone2='{15}',ReferenceId='{16}',ProductId = '{17}' Where CustomerId={18}",
                          aCustomer.Name, aCustomer.FatherName, aCustomer.CustomerCode, aCustomer.Address, aCustomer.District, aCustomer.Thana, aCustomer.Dob,
                          aCustomer.NId, aCustomer.Gender, aCustomer.Phone, entrydate, updatedate, aCustomer.CompanyId, aCustomer.BranchId, aCustomer.IsActive, aCustomer.Phone2, aCustomer.Phone2, aCustomer.ProductId, aCustomer.CustomerId);

                }
                connection.ExecuteNonQuery(query);
                var rv = Operation.Success.ToString();

                return rv;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }



        private int GetExistCustomerCode(string condition)
        {
            try
            {
                string sql = string.Format(@" Select * From Sale_Customer {0}", condition);
                var data = Data<Customer>.DataSource(sql);
                return data.Count;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public Customer GetCustomerByCustomerCode(string condition)
        {
            try
            {
                string sql = string.Format(@"Select CustomerId,Name,FatherName,CustomerCode,TypeId ,SC.[Address],District,Thana,DOB,NID ,Gender,
                SC.Phone,EntryDate,Updated,SC.CompanyId,SC.BranchId,SC.Flag,SC.IsActive,B.BranchSmsMobileNumber,Company.CompanyCode
                From Sale_Customer SC
                left outer join Company on Company.CompanyId= SC.CompanyId
                left outer join Branch B on B.BRANCHID=SC.BranchId {0}", condition);
                var data = Data<Customer>.GenericDataSource(sql);
                return data.SingleOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetCustomerResult(string condition)
        {
            try
            {

                string sql = string.Format(@"Select * From Sale_Customer {0}", condition);
                var data = Data<Customer>.DataSource(sql);
                if (data.Count != 0) return Operation.Exists.ToString();
                return Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                return Operation.Failed.ToString();
            }
        }

        public Branch GetSmsMobileNumberByBranchId(int branchId)
        {
            try
            {
                string query = string.Format(@"Select BranchSmsMobileNumber from Branch where BranchId = {0}", branchId);
                return Data<Branch>.DataSource(query).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public Sale GetProductInfoByCustomerId(int customerId)
        {
            string query = string.Format(@"Select Sale.*,Sale_License.LicenseId,Sale_License.Number,Sale_License.LType,Sale_Product.ProductName from Sale
left outer join Sale_License on Sale_License.ModelId = Sale.ModelId
left outer join Sale_Product on Sale_Product.ModelId = Sale.ModelId
 where CustomerId = {0}", customerId);
            return Data<Sale>.GenericDataSource(query).FirstOrDefault();

        }

        public List<Due> GetCustomerRatingByCompanyId(int companyId)
        {
            string query = string.Format(@"Select Sale_Due.*,Sale_AllType.Type as Color from Sale_Due
left outer join Sale_AllType on Sale_AllType.TypeId = Sale_Due.TypeId and Sale_AllType.Flag='Color' and Sale_AllType.IsActive = 1
where CompanyId = {0} and Status = 1", companyId);
            return Data<Due>.GenericDataSource(query);
        }

        public string CheckExistCustomerByCode(string customerCode, Users user)
        {
            string code = "";
            string sql = "";

            var companyId = user.ChangedCompanyId == 0 ? user.CompanyId : user.ChangedCompanyId;
            var branchId = user.ChangedBranchId == 0 ? user.BranchId : user.ChangedBranchId;


            sql = string.Format(@"Select CustomerCode From Sale_Customer
            where CustomerCode ='{0}' and CompanyId={1} and BranchId={2}", customerCode, companyId, branchId);

            var data = Data<Azolution.Entities.Sale.Customer>.DataSource(sql).SingleOrDefault();
            if (data != null) code = data.CustomerCode;
            return code;
        }

        public bool GetExistMobileNo(string mobileNo, Users user, int mobileType)
        {
            var companyId = user.ChangedCompanyId == 0 ? user.CompanyId : user.ChangedCompanyId;
            var branchId = user.ChangedBranchId == 0 ? user.BranchId : user.ChangedBranchId;
            var sql = string.Format(@"Select * From Sale_Customer
            where {3} ='{0}' and CompanyId={1} and BranchId={2}", mobileNo, companyId, branchId, GetPhoneType(mobileType));
            var data = Data<Azolution.Entities.Sale.Customer>.DataSource(sql);
            return data != null && data.Any();
        }

        private static string GetPhoneType(int mobileType)
        {
            return mobileType == 1 ? "Phone" : "Phone2";
        }

        public List<Customer> GetCustomerByBranchId(int branchId)
        {
            string sql = "";
            sql = string.Format(@"Select * From Sale_Customer Where BranchId={0}", branchId);
            var data = Data<Customer>.DataSource(sql);
            return data;
        }

        //GetCustomerInfoByInvoiceNo  Code added by Rubel

        public Customer GetCustomerInfobyInvoiceNo(string invoiceNo)
        {
            string sql = string.Format(@"  Select Sale_Customer.* from Sale_Customer inner join Sale 
          on Sale_Customer.CustomerId = Sale.CustomerId where sale.Invoice = '{0}'", invoiceNo);
            var data = Data<Customer>.DataSource(sql);
            if (data.Count > 0)
            {
                return data.SingleOrDefault();
            }
            return null;
        }

        public List<CustomerInfo> GetAllActiveCustomer()
        {

            string sql = string.Format(@"Select CustomerId,CustomerCode
                From Sale_Customer SC
                left outer join Company on Company.CompanyId= SC.CompanyId
                left outer join Branch B on B.BRANCHID=SC.BranchId where Sc.IsActive = 1");
            var data = Data<CustomerInfo>.DataSource(sql);
            if (data.Count > 0)
            {
                return data;
            }
            return null;

        }

        public Azolution.Entities.Sale.Sale GetCustomerAndSaleInfoByCustomerCode(string condition)
        {
            //            string sql = string.Format(@"Select SC.CustomerCode,SC.CustomerId,SC.Name,SC.Phone,SC.CompanyId,SC.BranchId,
            //                    Sale.SaleId,Sale.Invoice,Sale.WarrantyStartDate,Sale.ModelId
            //                    From Sale_Customer SC
            //                    Inner join Sale on Sale.CustomerId = SC.CustomerId 
            //                    {0}", condition);

            string sql = string.Format(@"Select SC.CustomerCode,SC.CustomerId,SC.Name,SC.Phone,SC.Phone2,SC.CompanyId,SC.BranchId,Branch.IsSmsEligible,BranchCode,
                    Sale.SaleId,Sale.Invoice,Sale.WarrantyStartDate,Sale.ModelId,Sale.SaleTypeId,Sale.State,
                    Sale.TempState,Sale.SalesRepId,Sale.Installment,Sale.Price,Sale.NetPrice,Sale.DownPay,
                    Sale.FirstPayDate,
                     Sale_Product.DefaultInstallmentNo,DownPayPercent,PackageType,TypeId,ModelItemID, Convert(Datetime,Sale.WarrantyStartDate,111) EntryDate2,
              Sale.ProductNo,
                 isnull(DPT.ReceiveAmount,0) TempReceiveAmount,
                isnull(tblColl.ReceiveAmount,0)ReceiveAmount,
                Sale.IIM, Sale.IsDownPayCollected, 
                SC.CustomerCode, SC.Name,SC.Phone,SC.Phone2,Sale_Product.ProductName,Sale_Product.Model,Sale.CompanyId,Branch.BRANCHID,BranchCode,
                sale.IsActive,BranchSmsMobileNumber,IsSmsEligible,(Select distinct IsLisenceRequired From Sale_Product_Items Where ModelId=Sale.ModelId and IsLisenceRequired=1)IsLisenceRequired,
                Discount.DiscountTypeCode,Discount.IsApprovedSpecialDiscount,Sale_Product.TypeId, Sale_Product.ModelId,SC.ProductId
                    From Sale_Customer SC
                    Inner join Sale on Sale.CustomerId = SC.CustomerId 
                    inner join Sale_Product on Sale_Product.ModelId = Sale.ModelId
                    Left join Branch on Branch.BRANCHID = Sale.BranchId
                 Left Outer join Discount on Discount.SaleId=Sale.SaleId
                Left outer join DownpamentCollection_tmp DPT on DPT.SaleId=Sale.SaleId
                left outer join (Select SaleInvoice,SUM(ReceiveAmount)ReceiveAmount From Sale_Collection
                group by SaleInvoice)tblColl on tblColl.SaleInvoice=Sale.Invoice
                    {0}", condition);

            var data = Data<Azolution.Entities.Sale.Sale>.GenericDataSource(sql).SingleOrDefault();
            return data;
        }
    }
}
