using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Laps.AdminSettings.Service.Service;
using Laps.SaleRepresentative.DataService.DataService;
using SmsService;

namespace Laps.SaleRepresentative.Service.Service
{
    public class CommissionAndIncentiveCalculation
    {
        private CommissionAndIncentiveCalculationDataService _commissionAndIncentiveCalculationDataService;

        public CommissionAndIncentiveCalculation()
        {
            _commissionAndIncentiveCalculationDataService = new CommissionAndIncentiveCalculationDataService();
        }

        public void CommissionIncentiveCalculation()
        {
            var _commissionSettingsService = new CommissionSettingsService();
            var _incentiveSettingsService = new IncentiveSettingsService();

            var dateToday = DateTime.Now;
            var previousMonth = new DateTime(dateToday.Year, dateToday.Month, dateToday.Day).AddMonths(-1);
            string previousMonthYear = previousMonth.ToString("MM/yyyy");
            string saleMonthYear = previousMonthYear;

            var salesRepInfo = _commissionAndIncentiveCalculationDataService.CommissionAndIncentiveCalculation(saleMonthYear);
            var incentiveInfo = _incentiveSettingsService.GetIncentiveSettingsData();

            if (salesRepInfo != null)
                foreach (var rep in salesRepInfo)
                {
                    var totalInstallmentSale = rep.TotalSaleInstallment;
                    var totalCashSale = rep.TotalSaleCash;
                    var totalSale = totalInstallmentSale + totalCashSale;
                    if (totalSale > 0)
                    {
                        decimal netTotalIncentiveAmount = 0;
                        decimal netTotalCommision = 0;

                        //Calculate Commission Amount.....................................
                        if (incentiveInfo != null && incentiveInfo.Count > 0)
                        {
                            if (rep.IsCommissionActive == 1)// Check is Commission allowed for this Representative
                            {
                                var commissionInfo = _commissionSettingsService.GetCommissionInfoBySaleRepType(rep.SalesRepType);
                                netTotalCommision = CommissionCalculation(commissionInfo, rep);
                            }
                        }

                        //Calculate Incentive Amount.....................................
                        if (rep.IsIncentiveActive == 1)// Check is Incentive allowed for this Representative
                        {
                            if (incentiveInfo != null && incentiveInfo.Count > 0)
                            {
                                foreach (var incentive in incentiveInfo)
                                {
                                    if (incentive.NumberOfSale == totalSale)
                                    {
                                        netTotalIncentiveAmount = incentive.IncentiveAmount;
                                    }
                                }
                            }
                        }
                       
                        var netTotalAmount = netTotalCommision + netTotalIncentiveAmount;

                        if (netTotalAmount > 0)
                        {
                            SendSMSToSaleRepresentator(rep, netTotalAmount, saleMonthYear);
                        }
                        
                    }

                }
        }

        private void SendSMSToSaleRepresentator(SalesRepCommission rep, decimal netTotalAmount, string saleMonthYear)
        {
            PhoneSettingsService _phoneSettingsService = new PhoneSettingsService();
            SmsManager _smsManager = new SmsManager();
            string smsText = "";

            var commissionMonthYear = saleMonthYear;

            string phoneNo = _phoneSettingsService.GetRandomPhoneNumber();

            GeneralSms generalSms = new GeneralSms();
            generalSms.CommissionMonthYear = commissionMonthYear;
            generalSms.TotalAmount = netTotalAmount;
            generalSms.SmsType = 5;//General SMS
            smsText = _smsManager.GetGeneralSmsText(generalSms);

            _commissionAndIncentiveCalculationDataService.SaveSms(smsText, rep, phoneNo);
        }

        private static decimal CommissionCalculation(List<Commission> commissionInfo, SalesRepCommission rep)
        {
            decimal totalInstallmentCommission = 0;
            decimal totalCashSaleCommission = 0;

            //if (commissionInfo.First(c => c.SaleTypeId == 1) != null)
            //{
            //    decimal perSalecommissionAmtForInstallment = commissionInfo.Where(c => c.SaleTypeId == 1).First().ComissionAmount;
            //    var totalInstallmentSale = rep.TotalSaleInstallment;
            //     totalInstallmentCommission = totalInstallmentSale * perSalecommissionAmtForInstallment;
            //}


            //if (commissionInfo.First(c => c.SaleTypeId == 2) != null)
            //{
            //    decimal perSalecommissionAmtForCash = commissionInfo.Where(c => c.SaleTypeId == 2).First().ComissionAmount;
            //    var totalCashSale = rep.TotalSaleCash;
            //    totalCashSaleCommission = totalCashSale * perSalecommissionAmtForCash;
            //}


            foreach (var commission in commissionInfo)
            {
                if (commission.SaleTypeId == 1)
                {
                    decimal perSalecommissionAmtForInstallment = commission.ComissionAmount;
                    var totalInstallmentSale = rep.TotalSaleInstallment;
                    totalInstallmentCommission = totalInstallmentSale * perSalecommissionAmtForInstallment;
                }
                else if (commission.SaleTypeId == 2)
                {
                    decimal perSalecommissionAmtForCash = commission.ComissionAmount;
                    var totalCashSale = rep.TotalSaleCash;
                    totalCashSaleCommission = totalCashSale * perSalecommissionAmtForCash;
                }
            }

            decimal netTotalCommision = totalInstallmentCommission + totalCashSaleCommission;
            return netTotalCommision;
        }
    }
}