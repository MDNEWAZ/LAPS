using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;
using Laps.Sale.SaleDataService.DataService;
using Laps.Sale.SaleService.Interface;
using Utilities;

namespace Laps.Sale.SaleService.Service
{
    public class PaymentSchedule : IPaymentSchedule
    {
        private readonly PaymentScheduleDataService _dataService;

        public PaymentSchedule()
        {
            _dataService=new PaymentScheduleDataService();
        }

        public string MakePaymentSchedule(int installmentNo,int saleId, string invoiceNo, decimal netPrice, DateTime firstPayDate,int saleType, CommonConnection connection)
        {
            try
            {
              IList<Installment> installments= GenerateInstallmentList(installmentNo, invoiceNo, netPrice,firstPayDate,saleType);
              return _dataService.SaveInstallment(installments.ToList(), saleId, invoiceNo, saleType,connection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IList<Installment> GenerateInstallmentList(int installmentNo, string invoiceNo, decimal netPrice, DateTime firstPayDate, int saleType)
        {
            try
            {
                firstPayDate = new DateTime(firstPayDate.Year, firstPayDate.Month, firstPayDate.Day).AddDays(-30);
                var installmentList = new List<Installment>();
                for (var i = 0; i < installmentNo; i++)
                {
                    var installmentObj = new Installment();
                    installmentObj.Number = i + 1;
                    installmentObj.SInvoice = invoiceNo;
                    installmentObj.Amount = netPrice / installmentNo;
                    installmentObj.DueAmount = 0;
                    installmentObj.ReceiveAmount = 0;
                   // installmentObj.DueDate = firstPayDate.AddMonths(i).ToString("dd-MMM-yyyy");
                    var currentMonth = firstPayDate.Month;
                    var imDate = firstPayDate.AddDays(30);
                    if (imDate.Month == currentMonth)
                    {
                        imDate = imDate.AddDays(1);
                    }
                    installmentObj.DueDate = imDate.ToString("dd-MMM-yyyy");
                    firstPayDate = new DateTime(imDate.Year,imDate.Month,imDate.Day);
                    installmentObj.Status = 0;

                    installmentList.Add(installmentObj);
                }

                return installmentList;
            }
            catch (Exception)
            {
                throw new Exception("Error While Generating IMs");
            }
        }


    }
}
