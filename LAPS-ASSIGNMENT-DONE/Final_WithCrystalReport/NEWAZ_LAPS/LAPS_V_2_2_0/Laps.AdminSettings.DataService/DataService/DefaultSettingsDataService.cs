using System;
using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.DataService.DataService
{
    public class DefaultSettingsDataService
    {
        public Interest GetAllInterestInfoByCompanyId(int companyId)
        {
            string sql = string.Format(@" Select * From Sale_Interest Where Status=1 And CompanyId={0}", companyId);
            var data = Data<Interest>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public object GetDefaultSettingsSummaryCompanies(GridOptions options, string companies)
        {
            string query = string.Format(@"Select SI.InterestId, SI.Interests, SI.DownPay, SI.EntryDate, SI.Status,SI.DefaultInstallmentNo, C.CompanyId, C.CompanyName,SI.DefaultCashDiscount,SI.CashDiscountPercentage from Sale_Interest SI
 Right JOIN Company C ON C.CompanyId=SI.CompanyId
Where SI.CompanyId in ({0})", companies);

            return Kendo<Interest>.Grid.GenericDataSource(options, query, "CompanyId");

        }

        public string SaveDefaultSettings(Interest aDue, int userId)
        {
            var aConnection = new CommonConnection();
            string seccess;
            try
            {
                string query = string.Empty;
                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");
                var updatedate = DateTime.Now.ToString("dd-MMM-yyyy");



                if (aDue.InterestId == 0)
                {
                    if (!CheckExistConfiguration(aDue))
                    {
                        //query = string.Format(@"UPDATE Sale_Interest SET [Status]={0} Where CompanyId={1}", 0,
                        //    aDue.ACompany.CompanyId);

                        query =
                            string.Format(
                                @"INSERT INTO Sale_Interest([CompanyId],[DownPay],[Interests],[Status],[EntryDate] ,[Updated]
           ,[Flag],[UserId],[UpdateBy],[DefaultInstallmentNo],[DefaultCashDiscount],CashDiscountPercentage) VALUES({0},{1},{2},{3},'{4}','{5}',{6},{7},{8},{9},{10},{11})",
                                aDue.ACompany.CompanyId, aDue.DownPay, aDue.Interests, aDue.Status, entrydate, "", 0,
                                userId, 0, aDue.DefaultInstallmentNo, aDue.DefaultCashDiscount,aDue.CashDiscountPercentage);
                    }
                    else
                    {
                        return Operation.Exists.ToString();
                    }

                }
                else
                {
                    //query = string.Format(@"UPDATE Sale_Interest SET [Status]={0} Where CompanyId={1}", 0, aDue.ACompany.CompanyId);

                    query = string.Format(" UPDATE Sale_Interest SET [DownPay]={0},[Interests] = {1},[Status]={2},[Updated] = '{3}',[Flag] = '{4}',[UpdateBy]={5},[DefaultInstallmentNo]={6},[DefaultCashDiscount]={7},CashDiscountPercentage={8} Where [InterestId]={9}",
                             aDue.DownPay, aDue.Interests, aDue.Status, updatedate, 0, userId, aDue.DefaultInstallmentNo, aDue.DefaultCashDiscount, aDue.CashDiscountPercentage, aDue.InterestId);
                }
                aConnection.ExecuteNonQuery(query);
                aConnection.Close();
                seccess = Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                aConnection.Close();
                seccess = Operation.Failed.ToString();
            }
            return seccess;
        }

        private bool CheckExistConfiguration(Interest aDue)
        {
            string sql = string.Format(@"Select * From Sale_Interest Where CompanyId={0}", aDue.ACompany.CompanyId);
            var data = Data<Interest>.DataSource(sql);
            return data != null && data.Any();
        }

        public string SaveOperationModeSettings(OperationMode operationModeObj, Users user)
        {
            string rv = "";
            string sql = "";
            CommonConnection connection = new CommonConnection();
            try
            {
                if (operationModeObj.OperationModeId == 0)
                {
                    sql = string.Format(@"Insert Into OperationMode (AutoOperation,ManualOperation,AutoSale,AutoCollection,ManualSale,ManualCollection,EntryDate,EntryUserId,AutoInventoryChecking,ManualInventoryChecking)
                    Values({0},{1},{2},{3},{4},{5},'{6}',{7},{8},{9})",operationModeObj.AutoOperation,operationModeObj.ManualOperation,operationModeObj.AutoSale,operationModeObj.AutoCollection,
                                                    operationModeObj.ManualSale,operationModeObj.ManualCollection,DateTime.Now,user.UserId,operationModeObj.AutoInventoryChecking,operationModeObj.ManualInventoryChecking);
                }
                else
                {
                    sql = string.Format(@"Update OperationMode Set AutoOperation={0},ManualOperation={1},AutoSale={2},AutoCollection={3},ManualSale={4},ManualCollection={5},UpdateDate='{6}',AutoInventoryChecking = {7},ManualInventoryChecking = {8} Where OperationModeId={9}", 
                        operationModeObj.AutoOperation, operationModeObj.ManualOperation, operationModeObj.AutoSale, operationModeObj.AutoCollection,
                         operationModeObj.ManualSale, operationModeObj.ManualCollection, DateTime.Now,operationModeObj.AutoInventoryChecking, operationModeObj.ManualInventoryChecking, operationModeObj.OperationModeId);
                }
                connection.ExecuteNonQuery(sql);
                rv = Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                rv = Operation.Failed.ToString();
            }
            finally
            {
                connection.Close();
            }

            return rv;
        }

        public OperationMode GetOperationModeSettings()
        {
            string sql = "Select * From OperationMode";
            var data = Data<OperationMode>.DataSource(sql).SingleOrDefault();
            return data;
        }
    }
}
