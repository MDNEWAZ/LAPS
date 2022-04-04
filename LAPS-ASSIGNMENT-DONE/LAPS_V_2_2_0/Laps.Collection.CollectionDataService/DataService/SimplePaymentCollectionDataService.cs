using System;
using System.Linq;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;


namespace Laps.Collection.CollectionDataService.DataService
{
    public class SimplePaymentCollectionDataService
    {
        public SimplePaymentCollection GetCustomerInfoWithInstallmentAmount(SimplePaymentCollection simplePaymentCollectionObj)
        {
            string query = string.Format(@"Select Distinct Sale_Customer.CustomerCode,Sale_Customer.Name,Sale_Customer.Phone,Sale_Customer.CustomerId,
                    Sale_Customer.CompanyId,Sale_Customer.BranchId,Sale.Invoice As SInvoice,Branch.BranchSmsMobileNumber,Sale.NetPrice As Amount
                    from Sale_Customer
                    left outer join Sale on Sale.CustomerId = Sale_Customer.CustomerId
                    left outer join Sale_Installment on Sale_Installment.SInvoice = Sale.Invoice
                    left outer join Branch on Branch.BRANCHID = Sale_Customer.BranchId
                    Where Sale_Customer.CompanyId = {0} and Sale_Customer.BranchId = {1} and Sale_Customer.CustomerCode = '{2}' and (Sale.State=4 or Sale.State = 5)  and Sale_Customer.IsActive = 1", simplePaymentCollectionObj.CompanyId, simplePaymentCollectionObj.BranchId, simplePaymentCollectionObj.CustomerCode);

            var data= Data<SimplePaymentCollection>.DataSource(query).SingleOrDefault();

            var insData = new Installment();
            if (data != null)
            {
                string sql = string.Format(@"Select top 1 * From Sale_Installment Where SInvoice='{0}' And Status=0",
                    data.SInvoice);

                 insData = Data<Installment>.DataSource(sql).SingleOrDefault();
                if (insData != null)
                {
                    data.Amount = insData.Amount;
                }
              
            }

           

            return data;

        }

        public string SaveAsDraftPayment(SimplePaymentCollection objPaymentInfo, Users user)
        {
            CommonConnection connection = new CommonConnection();
            string res = "";
            string sql = "";
            try
            {
                if (objPaymentInfo.SimplePaymentCollectionId == 0)
                {
                    sql = string.Format(@"Insert Into Temp_SimplePaymentCollection(CustomerCode,SInvoice,Name,Phone,CompanyId,BranchId,BranchSmsMobileNumber,ReceiveAmount,CollectionType,TransectionId,PayDate,Amount,UserId,EntryDate) values('{0}','{1}','{2}','{3}',{4},{5},'{6}',{7},{8},'{9}','{10}',{11},{12},'{13}')",
                        objPaymentInfo.CustomerCode, objPaymentInfo.SInvoice, objPaymentInfo.Name, objPaymentInfo.Phone, objPaymentInfo.CompanyId, objPaymentInfo.BranchId, objPaymentInfo.BranchSmsMobileNumber, objPaymentInfo.ReceiveAmount, objPaymentInfo.CollectionType, objPaymentInfo.TransectionId, objPaymentInfo.PayDate,objPaymentInfo.Amount,user.UserId,DateTime.Now);
                }
                else
                {
                    sql = string.Format(@"Update Temp_SimplePaymentCollection set CustomerCode='{0}',SInvoice='{1}',Name='{2}',Phone='{3}',CompanyId={4},BranchId={5},BranchSmsMobileNumber='{6}',ReceiveAmount={7},CollectionType={8},TransectionId='{9}',PayDate='{10}',Amount={11},UserId = {12},EntryDate='{13}' where SimplePaymentCollectionId={14}",
                        objPaymentInfo.CustomerCode, objPaymentInfo.SInvoice, objPaymentInfo.Name, objPaymentInfo.Phone, objPaymentInfo.CompanyId, objPaymentInfo.BranchId, objPaymentInfo.BranchSmsMobileNumber, objPaymentInfo.ReceiveAmount, objPaymentInfo.CollectionType, objPaymentInfo.TransectionId, objPaymentInfo.PayDate,objPaymentInfo.Amount,user.UserId,DateTime.Now, objPaymentInfo.SimplePaymentCollectionId);
                }
                connection.ExecuteNonQuery(sql);
                res = Operation.Success.ToString();

            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public object GetDraftedPaymentDataForGrid(GridOptions options, string collectionDate, Users users)
        {
            string query = string.Format(@"Select TC.*,Branch.BranchCode,Sale_Customer.IsUpgraded As IsCustomerUpgraded,Sale_Customer.Phone2,Sale_Customer.CustomerId,Sale_Customer.Name As CustomerName  from Temp_SimplePaymentCollection TC
            left outer join Branch on Branch.BRANCHID = TC.BranchId
            left outer join Sale_Customer on Sale_Customer.CustomerCode = TC.CustomerCode And Sale_Customer.BranchId=TC.BranchId
            where CONVERT(VARCHAR(10),TC.EntryDate,110) = '{0}' and Status = 0 and TC.CompanyId = {1} and TC.BranchId = {2} and UserId = {3}", 
              collectionDate, users.ChangedCompanyId, users.ChangedBranchId,users.UserId);
            return Kendo<SimplePaymentCollection>.Grid.DataSource(options, query, "SimplePaymentCollectionId");
        }

        public void UpdateStatusForDrafted(int simplePaymentCollectionId)
        {
            CommonConnection co = new CommonConnection();
            string res = "";
            try
            {
                string sql = string.Format(@"Update Temp_SimplePaymentCollection set Status = 1 where SimplePaymentCollectionId = {0}", simplePaymentCollectionId);
                co.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        public bool CheckExistDraftData(string customerCode)
        {
            string sql = string.Format(@"  Select * From Temp_SimplePaymentCollection Where Status=0 And CustomerCode='{0}'", customerCode);
            var data = Data<SimplePaymentCollection>.DataSource(sql);
            return data!=null &&  data.Any();
        }
    }
}
