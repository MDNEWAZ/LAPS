using System;

namespace Azolution.Entities.Sale
{
    public class SalesSms
    {
        public int SalesSmsId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNid { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string Package { get; set; }
        public int ExtraLight { get; set; }
        public int ExtraSwitch { get; set; }
        public decimal DownPayment { get; set; }
        public int InstallmentNo { get; set; }
        public string BranchCode { get; set; }
        public string SalesRepId { get; set; }
        public bool IsSd { get; set; }
        public DateTime SmsDate { get; set; }
        public bool IsRead { get; set; }
        public int IsUnrecognized { get; set; }
        public void CreateSmsObject(int position, string smsPart,DateTime recieveDate)
        {
            switch (position)
            {
                case 0: CustomerName = smsPart; break;
                case 1: CustomerNid = smsPart; break;
                case 2: MobileNo1 = smsPart; break;
                case 3: MobileNo2 = smsPart; break;
                case 4: Package = smsPart; break;
                case 5: ExtraLight = smsPart != "" ? Convert.ToInt32(smsPart) : 0; break;
                case 6: ExtraSwitch = smsPart != "" ? Convert.ToInt32(smsPart) : 0; break;
                case 7: DownPayment = smsPart != "" ? Convert.ToDecimal(smsPart) : 0; break;
                case 8: InstallmentNo = smsPart != "" ? Convert.ToInt32(smsPart) : 0; break;
                case 9: BranchCode = smsPart; break;
                case 10: SalesRepId = smsPart; break;
                case 11: IsSd = smsPart == "1"; break;
            }
            SmsDate = recieveDate;
            IsRead = false;
        }
    }
}
