using System.Web.UI.WebControls;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Laps.Sale.SaleDataService.DataService;
using Laps.Sale.SaleService.Interface;
using Utilities;
using Utilities.Common.Json;

namespace Laps.Sale.SaleService.Service
{
    public class WaitingForDiscountService : IWaitingForDiscountRepository
    {
        WaitingForDiscountDataService _discountDataService = new WaitingForDiscountDataService();
        JsonHelper jsonHelper = new JsonHelper();
        public GridEntity<WaitingForDiscountSummaryDto> GetWaitingForDiscountSummary(GridOptions options, string companies)
        {

            return _discountDataService.GetWaitingForDiscountSummary(options, companies);
        }

        public object GetDiscountInfoByType(Users user)
        {
            return _discountDataService.GetDiscountInfoByType(user);
        }

        /// <summary>
        /// dpApplicabeStage 1=Before DP, 2=After DP
        /// </summary>
        /// <param name="objWaitingForDiscount"></param>
        /// <param name="user"></param>
        /// <param name="dpApplicabeStage"></param>
        /// <returns></returns>
        public string ApproveWaitingForDiscount(WaitingForDiscount objWaitingForDiscount, Users user, int dpApplicabeStage)
        {
            WaitingForDiscountDataService dataService = new WaitingForDiscountDataService();

            decimal downpayment = 0;
            decimal priceWithoutDownPay = 0;
            float outStandingTotalPrice = 0;

            if (objWaitingForDiscount.DiscountedAmount == 0)// if discount amount is 0 on null
            {
                objWaitingForDiscount.DiscountedAmount = objWaitingForDiscount.Price;
            }

            if (objWaitingForDiscount.SaleTypeId == 2)
            {
                objWaitingForDiscount.Price = objWaitingForDiscount.DiscountedAmount;

            }
            else
            {
                double installmentYear = (double)objWaitingForDiscount.InstallmentNo / 12;
                double interestbyYear = objWaitingForDiscount.Interests * installmentYear;
                var discountedAmt = objWaitingForDiscount.DiscountedAmount;
                var price = objWaitingForDiscount.Price;

                if (dpApplicabeStage == 1)//Before DP
                {
                    if (objWaitingForDiscount.IsDPFixedAmount == 1)
                    {
                        downpayment = objWaitingForDiscount.DownPayPercent;
                    }
                    else
                    {
                        downpayment = (discountedAmt * objWaitingForDiscount.DownPayPercent) / 100;
                    }

                     priceWithoutDownPay = (discountedAmt - downpayment);
                     outStandingTotalPrice = ((float)(priceWithoutDownPay)) + (float)((priceWithoutDownPay) * (((decimal)interestbyYear / 100)));
                }
                else if (dpApplicabeStage == 2)//After DP
                {
                    if (objWaitingForDiscount.IsDPFixedAmount == 1)
                    {
                        downpayment = objWaitingForDiscount.DownPayPercent;
                    }
                    else
                    {
                        downpayment = (discountedAmt * objWaitingForDiscount.DownPayPercent) / 100;
                    }
                    priceWithoutDownPay = (price - downpayment);
                    var discount = price - discountedAmt;
                    var outstanding = priceWithoutDownPay - discount;
                    outStandingTotalPrice = ((float)(outstanding)) + (float)((outstanding) * (((decimal)interestbyYear / 100)));
                }
                else
                {
                    return "Select When SD Applicable";
                }
              
               

                objWaitingForDiscount.DiscountAmount = (objWaitingForDiscount.Price-objWaitingForDiscount.DiscountedAmount);
               // objWaitingForDiscount.Price = objWaitingForDiscount.DiscountedAmount;
                objWaitingForDiscount.DownPay = downpayment;
                objWaitingForDiscount.NetPrice = (decimal)outStandingTotalPrice;
            }

            return dataService.ApproveWaitingForDiscount(objWaitingForDiscount, user);
        }

        public object GetDiscountInfo(int saleId)
        {
            return _discountDataService.GetDiscountInfo(saleId);
        }

        public object GetDiscountTypeCombo()
        {
            return _discountDataService.GetDiscountTypeCombo();
        }

       
    }
}
