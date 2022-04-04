using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace Solaric.GenerateCode
{
    public class Sms
    {
        public int LicenseId { get; set; }
        public string LiNumber { get; set; }
        public int SaleId { get; set; }
        public string MobileNo { get; set; }

        SqlCommand _aCommand;
        SqlDataAdapter _adapter;
        readonly string _connection = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        private readonly SqlConnection _aConnection;
        private static Timer aTimer;
        public Sms()
        {
            _aConnection = new SqlConnection(_connection);
        }
        private void Send()
        {
            if (aTimer == null)
            {
                aTimer = new Timer(5000);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.Interval = 3000;
                aTimer.Enabled = true;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {

            var aTable = new DataTable();
            var query = string.Format("select Top 10 SL.LicenseId, SL.Number, S.SaleId, SC.Phone from Sale_License SL " +
                              "Left Join Sale S On S.ProductNo=SL.ProductNo Left Join Sale_Customer SC On SC.CustomerId=S.CustomerId Where SL.SaleId=0 ");
            _aCommand = new SqlCommand(query, _aConnection);
            _adapter = new SqlDataAdapter(_aCommand);
            _adapter.Fill(aTable);
            DataRow aRow = aTable.Select().FirstOrDefault();
            if (aRow != null)
            {
                var aSms = new Sms
                {
                    LicenseId = (int)aRow["LicenseId"],
                    LiNumber = (string)aRow["Number"],
                    SaleId = (int)aRow["SaleId"],
                    MobileNo = (string)aRow["MobileNo"]
                };
                if (SendSMS(aSms))
                {
                    SaveLicecse(aSms);
                    aTimer.Enabled = false;
                }
            }
        }

        private bool SendSMS(Sms aSms)
        {
            try
            {
                // Here SendSmsOkThen()
                //var query = string.Format("UPDATE [Sale_License] SET [SaleId] = {0} WHERE [LicenseId]= {1}", aSms.SaleId, aSms.LicenseId);
                //_aCommand = new SqlCommand(query, _aConnection);
                //_adapter = new SqlDataAdapter { UpdateCommand = _aCommand };
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void SaveLicecse(Sms aSms)
        {
            try
            {
                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");
                var updatedate = DateTime.Now.ToString("dd-MMM-yyyy");

                var query = string.Format("INSERT INTO [Sale_SmsSend] VALUES({0},{1},{2},'{3}','{4}','{5}',{6},{7},'{8}')", aSms.LicenseId, 0, aSms.LiNumber, "", aSms.MobileNo, updatedate, 0, 0, 1);
                _aCommand = new SqlCommand(query, _aConnection);
                _adapter = new SqlDataAdapter { InsertCommand = _aCommand };
                _adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
        }

        //aList.Add(aSms);
        //var results = from dataRow in aTable.AsEnumerable()
        //              where dataRow.Field<int>("LicenseId") > 0
        //              select new
        //              {
        //                  LicenseId = dataRow.Field<int>("LicenseId"),
        //                  Number = dataRow.Field<string>("Number"),
        //                  SaleId = dataRow.Field<int>("SaleId")
        //              };
        //var aSms = results.FirstOrDefault();
    }
}
