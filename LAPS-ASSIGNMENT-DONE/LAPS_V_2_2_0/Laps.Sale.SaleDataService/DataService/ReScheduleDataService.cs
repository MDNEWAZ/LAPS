using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Sale.SaleDataService.DataService
{
    public class ReScheduleDataService
    {
        public bool Save(IEnumerable<Installment> newIms, CommonConnection dbConnection)
        {
            try
            {
                var qBuilder = new StringBuilder();
                foreach (var installment in newIms)
                {
                    DateTime adate = Convert.ToDateTime(installment.DueDate);
                    var dueDate = adate.ToString("dd-MMM-yyyy");
                    var query = string.Format(@"Insert Into Sale_Installment([SInvoice],[ProductNo],[Number],[Amount],[Status],[DueDate],[EntryDate],[Updated],[Flag])
                    Values('{0}','{1}',{2},{3},{4},'{5}','{6}','{7}',{8});", installment.SInvoice, "", installment.Number, installment.Amount, installment.Status, dueDate, installment.EntryDate, "", installment.Flag);
                    qBuilder.Append(query);
                }
                if (qBuilder.ToString() != "")
                {
                    var sql = "Begin " + qBuilder + " End;";
                    dbConnection.ExecuteNonQuery(sql);
                }
                return true;

            }
            catch (Exception)
            {
                throw new Exception("Error! During Saving new IMs");
            }
        }

        public bool DeleteIm(IEnumerable<int> ids, CommonConnection dbConnection)
        {
            try
            {
                string query = string.Format(@"Delete from Sale_Installment where InstallmentId in({0})", GetInIds(ids));
                dbConnection.ExecuteNonQuery(query);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error! During Deleting Existing IMs");
            }
        }

        private string GetInIds(IEnumerable<int> ids)
        {
            string inquery = "";
            var i = 0;
            for (int j = 0; j < ids.ToList().Count(); j++)
            {
                inquery = inquery + ids.ToList()[j];
                if (i < ids.Count() - 1)
                {
                    inquery = inquery + ",";
                    i++;
                }
            }
            return inquery;

        }

        public IEnumerable<Installment> GetExistingInstallemntSchedules(string customerCode, int companyId, int branchId, CommonConnection dbConnection)
        {
            try
            {
                var query = string.Format(@"SELECT * FROM Sale_Installment where Sale_Installment.SInvoice=(
                select top 1 Sale.Invoice from Sale
                inner join Sale_Customer on Sale_Customer.CustomerId=Sale.CustomerId And Sale_Customer.IsActive=1
                where Sale_Customer.CustomerCode='{0}'and Sale_Customer.BranchId={1} and Sale_Customer.CompanyId={2}) order by InstallmentId", customerCode, branchId, companyId);
                var ims= Data<Installment>.DataSource(query);
                ims.Where(i=>i.Status==1).ToList().ForEach(i=>i.ReceiveAmount=i.Amount);
                ims.Where(i => i.Status == 0).ToList().ForEach(i => i.DueAmount = i.Amount);
                var partialPaidIms = ims.Where(i => i.Status == 2);

                if (partialPaidIms.Any())
                {
                    var collection = GetExistingDueOfPartialPayment(partialPaidIms.FirstOrDefault().InstallmentId);
                    ims.Where(i => i.Status == 2).FirstOrDefault().DueAmount = collection.DueAmount;
                    ims.Where(i => i.Status == 2).FirstOrDefault().ReceiveAmount =ims.Where(i => i.Status == 2).FirstOrDefault().Amount- collection.DueAmount;
                  
                }
                return ims;
            }
            catch (Exception)
            {
                throw new Exception("Error! During retrieving Existing IMs");
            }
            
        }

        public CollectionInfo GetExistingDueOfPartialPayment(int installmentId)
        {
            try
            {
                var query = string.Format(@"Select DueAmount From Sale_Collection Where InstallmentId= {0}
                and CollectionId = (Select MAX(CollectionId) From Sale_Collection Where InstallmentId={0})", installmentId);
                var dueAmount = Data<CollectionInfo>.DataSource(query).FirstOrDefault();
                return dueAmount;

            }
            catch (Exception)
            {
                throw new Exception("Error! During retrieving Existing Due");
            }
        }

        public int GetImStatusFromTemp(string sInvoice)
        {
            try
            {
                var query = string.Format(@"Select * From Sale_Installment_Temp Where Status=1 And SInvoice='{0}'", sInvoice);
                var installmentData = Data<Installment>.DataSource(query);
                if (installmentData != null) return installmentData.Count;
                return 0;
            }
            catch (Exception)
            {
                throw new Exception("Error! During retrieving Installment Temp data");
            }
        }

        public void SaveInstallmentDataToTemp(string sInvoice)
        {
            var connection = new CommonConnection();
            try
            {
                
                var deleteQuery = string.Format(@"Delete from Sale_Installment_temp where SInvoice='{0}'", sInvoice);
                var query =
                    string.Format(
                        @"INSERT INTO Sale_Installment_Temp SELECT * FROM Sale_Installment where SInvoice='{0}'", sInvoice);
                connection.ExecuteNonQuery("Begin "+deleteQuery+";"+query+" End;");
            }
            catch (Exception)
            {
                throw new Exception("Failed to Save Installment to Temp table");

            }
            finally
            {
                connection.Close();
            }
        }
    }
}
