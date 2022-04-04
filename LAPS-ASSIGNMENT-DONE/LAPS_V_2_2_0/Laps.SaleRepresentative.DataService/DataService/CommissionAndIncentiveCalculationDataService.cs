using System;
using System.Collections.Generic;
using Azolution.Entities.DTO;
using Utilities;

namespace Laps.SaleRepresentative.DataService.DataService
{
    public class CommissionAndIncentiveCalculationDataService
    {
        public List<SalesRepCommission> CommissionAndIncentiveCalculation(string saleMonthYear)
        {
            string sql = string.Format(@"Select SR.SalesRepId,SalesRepSMSMobNo,SalesRepBkashNo,SR.IsCommissionActive,SR.IsIncentiveActive,
             SRT.SalesRepTypeName,SRT.SalesRepTypeId As SalesRepType,SR.SalesRepCode,
             tblFinal.TotalSaleCash,tblFinal.TotalSaleInstallment,tblFinal.SaleMonthYear
             From SalesRepresentator SR 
             left join SalesRepresentatorType SRT on SRT.SalesRepTypeId = SR.SalesRepType
             inner join(Select tblTotalIns.SalesRepId,isnull(tblTotalIns.TotalSaleInstallment,0) As TotalSaleInstallment,
             isnull(tblTotalCash.TotalSaleCash,0) As TotalSaleCash,SaleMonthYear From (
             Select SalesRepId,TotalSale as TotalSaleInstallment,SaleMonthYear From (
             Select SalesRepId,COUNT(SaleId)TotalSale,SaleTypeId,RIGHT(CONVERT(VARCHAR(10), WarrantyStartDate, 103), 7) AS SaleMonthYear
             from Sale 
             Where SaleTypeId=1 and RIGHT(CONVERT(VARCHAR(10), WarrantyStartDate, 103), 7)='{0}'
             group by SaleTypeId,SalesRepId,RIGHT(CONVERT(VARCHAR(10), WarrantyStartDate, 103), 7)
             ) tbl1 ) tblTotalIns
             left outer join 
             (Select SalesRepId,TotalSale as TotalSaleCash From (
             Select SalesRepId,COUNT(SaleId)TotalSale,SaleTypeId,RIGHT(CONVERT(VARCHAR(10), WarrantyStartDate, 103), 7) AS SaleMonthYear
             from Sale 
             Where SaleTypeId=2 and RIGHT(CONVERT(VARCHAR(10), WarrantyStartDate, 103), 7)='{0}'
             group by SaleTypeId,SalesRepId,RIGHT(CONVERT(VARCHAR(10), WarrantyStartDate, 103), 7)
             ) tbl2 )tblTotalCash on tblTotalIns.SalesRepId=tblTotalCash.SalesRepId) tblFinal on tblFinal.SalesRepId=SR.SalesRepId
             --Where SR.SalesRepId='569000'", saleMonthYear);

            var data = Data<SalesRepCommission>.DataSource(sql);
            return data;
        }

        public void SaveSms(string smsText, SalesRepCommission rep, string phoneNo)
        {
            string sql = "";
            CommonConnection connection = new CommonConnection();
            string simNumber = phoneNo;
            try
            {
                sql = string.Format(@"INSERT INTO SMSSent(SMSText,MobileNumber,[RequestDateTime],[Status],ReplyFor,SimNumber) VALUES(N'{0}','{1}','{2}',{3},{4},'{5}')",
                    smsText, rep.SalesRepSmsMobNo, DateTime.Now, 0, 0, simNumber);
                connection.ExecuteNonQuery(sql);
             
            }
            catch (Exception)
            {
                throw new Exception("Error! While Save SMS");
            }
        }
    }
}