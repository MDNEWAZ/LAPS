using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Laps.Sale.SaleDataService.DataService;
using Laps.Sale.SaleService.Interface;
using Utilities;

namespace Laps.Sale.SaleService.Service
{
    public class ReSchedule : IReSchedule
    {
        private readonly ReScheduleDataService _dataService;

        public ReSchedule()
        {
            _dataService = new ReScheduleDataService();
        }

        public bool ReScheduleIm(string customerCode, int newImNo, DateTime firstPayDate,int companyId, int branchId )
        {
            CommonConnection dbConnection = new CommonConnection();
            try
            {
                decimal existingDue =0;
                var existingIms = GetExistingInstallmentScheduls(customerCode,companyId, branchId );
                var tempIms = GetImStatusFromTemp(existingIms.First().SInvoice);

                if (tempIms < existingIms.Count(i => i.Status == 1))
                {
                    SaveInstallmentDataToTemp(existingIms.First().SInvoice);
                }
                var installmentData = existingIms.Where(s => s.Status == 2).FirstOrDefault();
                if (installmentData != null)
                {
                   var installmentId = installmentData.InstallmentId;
                   var existingDueInfo = GetExistingDueOfPartialPayment(installmentId);
                    if (existingDueInfo != null)
                    {
                        existingDue = existingDueInfo.DueAmount;
                    }
                }
               

                dbConnection.BeginTransaction();
                decimal unPaidAmount = existingIms.Where(i => i.Status == 0).Sum(i=>i.Amount);
                unPaidAmount = unPaidAmount + existingDue;
                var stratInsNo = existingIms.Count(i => i.Status == 1);

                var newIms = CreateNewIm(unPaidAmount, firstPayDate, newImNo, existingIms.FirstOrDefault().SInvoice, stratInsNo + 1);
                var res =DeleteExistingUnPaidIm(existingIms.Where(i => i.Status != 1).Select(i => i.InstallmentId), dbConnection);
                if (res)
                {
                    Save(newIms, dbConnection);
                    dbConnection.CommitTransaction();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                dbConnection.RollBack();
                throw new Exception("Error during Re-Scheduling IM");
            }
            finally
            {
                dbConnection.Close();
            }
        }

        private void SaveInstallmentDataToTemp(string sInvoice)
        {
            try
            {
                 _dataService.SaveInstallmentDataToTemp(sInvoice);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private int GetImStatusFromTemp(string sInvoice)
        {
            try
            {
                return _dataService.GetImStatusFromTemp(sInvoice);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private CollectionInfo GetExistingDueOfPartialPayment(int installmentId)
        {
            try
            {
                return _dataService.GetExistingDueOfPartialPayment(installmentId);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool Save(IEnumerable<Installment> newIms, CommonConnection dbConnection)
        {
            try
            {
                return _dataService.Save(newIms, dbConnection);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool DeleteExistingUnPaidIm(IEnumerable<int> ids, CommonConnection dbConnection)
        {
            try
            {
                return _dataService.DeleteIm(ids, dbConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IEnumerable<Installment> CreateNewIm(decimal unPaidAmount, DateTime firstPayDate, int newImNo, string inVoice, int startImNo)
        {
            var newIms = new List<Installment>();

            var firstDate = new DateTime(firstPayDate.Year, firstPayDate.Month, firstPayDate.Day).AddDays(-30);
            
            for (int i = 0; i < newImNo; i++)
            {
                var payDate = new DateTime(firstDate.Year, firstDate.Month, firstDate.Day).AddDays(30);
                var m1 = firstDate.Month;
                var m2 = payDate.Month;
                if (m1 == m2)
                {
                    payDate = new DateTime(payDate.Year, payDate.Month, payDate.Day).AddDays(1);
                }
                var im = new Installment();
                im.SInvoice = inVoice;
                im.Number = startImNo++;
                im.Amount = unPaidAmount/newImNo;
                im.Status = 0;
                im.DueDate = payDate.ToShortDateString();
                im.EntryDate = DateTime.Now.ToShortDateString();
                im.Flag = 0;
                newIms.Add(im);
                firstDate = payDate;
            }
            return newIms;
        }

        public IEnumerable<Installment> GetExistingInstallmentScheduls(string customerCode, int companyId, int branchId)
        {
            var dbConnection = new CommonConnection();
            try
            {
                return _dataService.GetExistingInstallemntSchedules(customerCode, companyId, branchId, dbConnection);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
