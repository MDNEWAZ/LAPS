using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace SmsService
{
    public class SmsManager : ISmsManager
    {
        ISmsDataManager _dbManager;
        public SmsManager()
        {
            _dbManager = new SmsDataManager();
        }

        public List<SalesSms> GetAllUnRecognizedSms(GridOptions options, string smsDateFrom, string smsDateTo)
        {
            try
            {
                return _dbManager.GetAllUnRecognizedSms(options,smsDateFrom,smsDateTo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string UpdateSms(SalesSms sms)
        {
            try
            {
                return _dbManager.UpdateSms(sms);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<SMSRecieved> GetAllUnrecognizedCollectionSms(string receiveDateFrom, string receiveDateTo)
        {
            return _dbManager.GetAllUnrecognizedCollectionSms(receiveDateFrom,receiveDateTo);
        }

        public string EditCollectionSms(SMSRecieved sms)
        {
            return _dbManager.EditCollectionSms(sms);
        }

        public string GetSmsText(LicenseInfo licenseInfo)
        {
            string smsText = string.Empty;
            var smsObj = GetSmsTextByType(licenseInfo.LType);
            if (smsObj != null)
            {
                if (licenseInfo.LType == 1)
                {
                    smsText = GenerateInitialLicenseSmsText(licenseInfo, smsObj);
                }
                else if (licenseInfo.LType == 2)
                {
                    smsText = GenerateRenewalLicenseSmsText(licenseInfo, smsObj);
                  
                }
                else if (licenseInfo.LType == 3)
                {
                    smsText = GenerateReleaseLicenseSmsText(licenseInfo, smsObj);
                }
                else if(licenseInfo.LType==4)
                {
                    smsText = GenerateInstallmentAlertSmsText(licenseInfo, smsObj);
                    
                }
            }

            return smsText;
        }

        public string GetGeneralSmsText(GeneralSms generalSms)
        {
            string smsText = string.Empty;
            var smsObj = GetSmsTextByType(generalSms.SmsType);
            if (generalSms.SmsType == 5)//Representator SMS
            {
                smsText = GenerateRepresentatorSmsText(generalSms, smsObj); 
            }
           
            else if (generalSms.SmsType == 6)//Representator SMS
            {
                smsText = GenerateBMSmsText(generalSms, smsObj);
            }

            else if (generalSms.SmsType == 7)//Dealer SMS
            {
                smsText = GenerateInitialLicenseSmsTextForDealer(generalSms, smsObj);
            }

            else if (generalSms.SmsType == 8)//Inventory Info SMS while cheking
            {
                smsText = GenerateInventoryInfoSmsText(generalSms, smsObj);
            }
            return smsText;
        }

        private string GenerateRepresentatorSmsText(GeneralSms generalSms, Sms smsObj)
        {
            string smsText = string.Empty;

            smsText = smsObj.Salutation+"," +smsObj.DueInfo+":" +generalSms.TotalAmount +""+smsObj.Unit+" " +smsObj.Thanking;

            return smsText;
        }

        private string GenerateInventoryInfoSmsText(GeneralSms generalSms, Sms smsObj)
        {
            string smsText = string.Empty;

            smsText = smsObj.Salutation + "," + smsObj.DueInfo + ":" + generalSms.TotalAmount + "" + smsObj.Unit + " " + smsObj.Thanking;

            return smsText;
        }

        private string GenerateBMSmsText(GeneralSms generalSms, Sms smsObj)
        {
            string smsText = string.Empty;
            string licenseType = "";
            decimal paidAmount = 0;
            if (generalSms.license.LType == 1)
            {
                licenseType = " Initial Code:";
                paidAmount = generalSms.license.DownPay;
            }
            else if (generalSms.license.LType == 2)
            {
                licenseType = " Renewal Code:";
                paidAmount = generalSms.license.ReceivedAmount;
            }
            else if (generalSms.license.LType == 3)
            {
                licenseType = " Release Code:";
            }

            smsText = smsObj.Salutation + "," + smsObj.CustomerInfo + ":" +generalSms.license.CustomerCode +" " + smsObj.PaidInfo + ""
                + paidAmount + smsObj.Unit + "" + licenseType + "" + generalSms.license.Code + " " + smsObj.Thanking;
            return smsText;
        }

        private string GenerateInitialLicenseSmsText(LicenseInfo licenseInfo,Sms smsObj)
        {
            string smsText = string.Empty;

            smsText = smsObj.Salutation +"," + smsObj.Greetings +" "+ smsObj.CustomerInfo +" "+
                      licenseInfo.CustomerCode +","+ smsObj.PaidInfo + licenseInfo.DownPay + smsObj.Unit +","+ smsObj.CodeInfo +":"+
                      licenseInfo.Code + " " + smsObj.Thanking;

            return smsText;
        }

        private string GenerateInitialLicenseSmsTextForDealer(GeneralSms generalSms, Sms smsObj)
        {
            string smsText = string.Empty;

            smsText = smsObj.Salutation + "," + smsObj.Greetings + " " + smsObj.CustomerInfo + " " +
                      generalSms.license.CustomerCode + "," + smsObj.CodeInfo + ":" +
                      generalSms.license.Code + " " + smsObj.Thanking;

            return smsText;
        }
        private string GenerateInstallmentAlertSmsText(LicenseInfo licenseInfo,Sms smsObj)
        {
            string smsText = string.Empty;

            smsText = licenseInfo.CustomerName + "," + smsObj.Greetings + " " + smsObj.CustomerInfo + " " +
                      licenseInfo.CustomerCode + ", " + smsObj.PaidInfo + licenseInfo.DownPay + smsObj.Unit + ", " + smsObj.CodeInfo + " " +
                      licenseInfo.Code + " " + smsObj.Thanking;

            return smsText;
        }
        private string GenerateRenewalLicenseSmsText(LicenseInfo licenseInfo, Sms smsObj)
        {
            string smsText = string.Empty;

            smsText = smsObj.CodeInfo + ":" + licenseInfo.Code + " " + smsObj.PaidInfo + ":" + licenseInfo.ReceivedAmount + smsObj.Unit +
                      smsObj.DueInfo + ":" + licenseInfo.DueAmount + smsObj.Unit + " " + smsObj.Thanking;
                    

            return smsText;
        }
        private string GenerateReleaseLicenseSmsText(LicenseInfo licenseInfo, Sms smsObj)
        {
            string smsText = string.Empty;

            smsText = smsObj.Salutation +","+ smsObj.CodeInfo + ":" + licenseInfo.Code +" "+ smsObj.Thanking;   
            return smsText;
        }

      
        public Sms GetSmsTextByType(int smsTypeId)
        {
            var sms = _dbManager.GetSmsTextByType(smsTypeId);
            return sms;
        }

        public GridEntity<Sms> GetAllSmsSettings(GridOptions options)
        {
            return _dbManager.GetAllSmsSettings(options);
        }

        public List<SmsTypes> GetSmsTypeDataForCombo()
        {
            return _dbManager.GetSmsTypeDataForCombo();
        }

        public string SaveSmsSettings(Sms smsObj)
        {
            string rv = "";
            if (!CheckExistActiveSmsSettings(smsObj))
            {
                rv = _dbManager.SaveSmsSettings(smsObj);
            }
            else
            {
                rv = Operation.Exists.ToString();
            }
            return rv;
        }

        private bool CheckExistActiveSmsSettings(Sms smsObj)
        {
            return _dbManager.CheckExistActiveSmsSettings(smsObj);
        }
    }
}
