using System.Collections.Generic;
using Azolution.Entities.Core;

namespace Azolution.Entities.Sale
{
    public class SalesParam
    {
        public Azolution.Entities.Sale.Sale SaleInfo;
        public List<Installment> InstallmentList;
        public List<SalesItem> ItemInfoLis;
        public List<SalesItemInformation> ItemDetailsInfoList;
        public PaymentReceivedInfo DownPayCollection;
        public Customer CustomerInfo;
        public Discount DiscountInfo;
        public Users User;
    }
}