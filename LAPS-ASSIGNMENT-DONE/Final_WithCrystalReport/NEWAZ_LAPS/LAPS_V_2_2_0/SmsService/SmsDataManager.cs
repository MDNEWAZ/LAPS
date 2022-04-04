using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace SmsService
{
    public class SmsDataManager : ISmsDataManager
    {
        private readonly CommonConnection _connection;
        public SmsDataManager()
        {
            _connection = new CommonConnection();
        }

        public List<SalesSms> GetAllUnRecognizedSms(GridOptions options, string smsDateFrom, string smsDateTo)
        {
            var condition = "";
            if (smsDateFrom != "null" && smsDateTo != "null")
            {
                condition += string.Format(@" and cast(SmsDate as date) between '{0}' and '{1}'", smsDateFrom,
                    smsDateTo);
            }
            
            try
            {
                var query = string.Format(@"SELECT *  FROM SalesSms where IsUnrecognized=1 {0}",condition);
                return Data<SalesSms>.DataSource(query);
            }
            catch (Exception)
            {
                throw new Exception("Error! During Read Unrecognized SMS for Sales Request");
            }
        }

        public string UpdateSms(SalesSms sms)
        {
            try
            {
                var query =
                    string.Format(
                        @"  Update SalesSms set MobileNo1='{0}',Package='{1}',BranchCode='{2}',IsRead='False',IsUnrecognized=0 where SalesSmsId={3}",
                        sms.MobileNo1, sms.Package, sms.BranchCode, sms.SalesSmsId);
                _connection.ExecuteNonQuery(query);
                return Operation.Success.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Error! During Update Unrecognized SMS for Sales Request");
            }
            finally
            {
                _connection.Close();
            }
        }

        public List<SMSRecieved> GetAllUnrecognizedCollectionSms(string receiveDateFrom, string receiveDateTo)
        {
            var condition = "";
            if (receiveDateFrom != "null" && receiveDateTo != "null")
            {
                condition += string.Format(@" and cast(RecievedDate as date) between '{0}' and '{1}'", receiveDateFrom,
                    receiveDateTo);
            }
            

            try
            {
                var sql = String.Format(@"Select * from SMSRecieved where Status IN(2,3,4,5,9,11) {0}",condition);
                return Data<SMSRecieved>.DataSource(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public string EditCollectionSms(SMSRecieved sms)
        {
            try
            {
                var query =
                    string.Format(
                        @"  Update SMSRecieved set SMSText='{0}',Status=0 where ID={1}", sms.SMSText, sms.ID);
                _connection.ExecuteNonQuery(query);
                return Operation.Success.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Error! During Update Unrecognized Collection SMS");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Sms GetSmsTextByType(int smsTypeId)
        {
            string sql = "";
            try
            {
                sql = "Select * From SMSText Where Status=1 And SmsType=" + smsTypeId;
                var data = Data<Sms>.DataSource(sql).SingleOrDefault();
                return data;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Get SMS Text!");
            }
        }

        public GridEntity<Sms> GetAllSmsSettings(GridOptions options)
        {
            string sql = string.Format(@"Select SMSText.*,SmsTypeName From SMSText
                    Left outer join SmsType on SmsType.SmsType=SMSText.SmsType");
            var data = Kendo<Sms>.Grid.DataSource(options, sql, "SmsId");
            return data;
        }

        public List<SmsTypes> GetSmsTypeDataForCombo()
        {
            string sql = string.Format(@"Select * From SmsType");
            var data = Kendo<SmsTypes>.Combo.DataSource(sql);
            return data;
        }

        public string SaveSmsSettings(Sms smsObj)
        {
            string rv = "";
            string sql = "";
            CommonConnection connection = new CommonConnection();
            try
            {
                if (smsObj.SmsId == 0)
                {
                    sql = string.Format(@"Insert Into SMSText([SmsType],[Salutation],[Greetings],[CustomerInfo],[DueInfo]
                  ,[PaidInfo],[Unit],[CodeInfo],[Warning],[Request],[Thanking],[Status]) Values({0},N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}',N'{10}',{11})",
                  smsObj.SmsType, smsObj.Salutation, smsObj.Greetings, smsObj.CustomerInfo, smsObj.DueInfo, smsObj.PaidInfo, smsObj.Unit,
                  smsObj.CodeInfo, smsObj.Warning, smsObj.Request, smsObj.Thanking, smsObj.Status);


                }
                else
                {
                    sql = string.Format(@"Update SMSText Set [SmsType]={0},[Salutation]=N'{1}',[Greetings]=N'{2}',[CustomerInfo]=N'{3}',[DueInfo]=N'{4}'
                  ,[PaidInfo]=N'{5}',[Unit]=N'{6}',[CodeInfo]=N'{7}',[Warning]=N'{8}',[Request]=N'{9}',[Thanking]=N'{10}',[Status]={11} Where SmsId={12} ",
                 smsObj.SmsType, smsObj.Salutation, smsObj.Greetings, smsObj.CustomerInfo, smsObj.DueInfo, smsObj.PaidInfo, smsObj.Unit,
                 smsObj.CodeInfo, smsObj.Warning, smsObj.Request, smsObj.Thanking, smsObj.Status,smsObj.SmsId);

                }
                connection.ExecuteNonQuery(sql);
                rv = Operation.Success.ToString();
            }
            catch (Exception exception)
            {
                rv = exception.Message;
            }
            finally
            {
                connection.Close();
            }
            return rv;
        }

        public bool CheckExistActiveSmsSettings(Sms smsObj)
        {
            string condition = "";
            string sql = "";
            if (smsObj.SmsId == 0)
            {
                condition = string.Format(@" Where SmsType={0} And Status=1", smsObj.SmsType);
            }
            else
            {
                condition = string.Format(@" Where SmsId !={0} And SmsType={1} And Status=1", smsObj.SmsId, smsObj.SmsType);
            }
            sql = string.Format(@" Select * From SMSText {0}", condition);
            var data = Data<Sms>.DataSource(sql).SingleOrDefault();
            return data != null;
        }

      
    }
}
