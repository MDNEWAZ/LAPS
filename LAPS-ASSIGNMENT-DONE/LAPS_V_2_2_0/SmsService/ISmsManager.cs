using System;
using System.Collections.Generic;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace SmsService
{
    public interface ISmsManager
    {
        List<SalesSms> GetAllUnRecognizedSms(GridOptions options, string smsDateFrom, string smsDateTo);
        string UpdateSms(SalesSms sms);
        List<SMSRecieved> GetAllUnrecognizedCollectionSms(string receiveDateFrom, string receiveDateTo);
        string EditCollectionSms(SMSRecieved sms);

        Sms GetSmsTextByType(int smsTypeId);
        GridEntity<Sms> GetAllSmsSettings(GridOptions options);
        List<SmsTypes> GetSmsTypeDataForCombo();
        string SaveSmsSettings(Sms smsObj);
    }
}