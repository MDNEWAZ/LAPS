
var SmsSettingsDetailsManager = {
    
    SaveSmsSettings:function() {
        var validator = $("#smsDetailsDiv").kendoValidator().data("kendoValidator"),
         status = $(".status");
        if (validator.validate()) {
            var objSmsObj = SmsSettingsDetailsHelper.CreateSmsDetailsObject();
            var smsObj = JSON.stringify(objSmsObj);
            var jsonParam = "smsObj:" + smsObj;
            var serviceUrl = "../SmsSettings/SaveSmsSettings";
                AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
          
        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'SMS Save Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            SmsSettingsDetailsHelper.ClearSmsDetailsForm();
                            $("#gridSmsSettingsSummary").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Exists") {

                AjaxManager.MsgBox('warning', 'center', 'Already Exists:', 'Sms Settings Already Exist',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }
        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
        }
    },


    GetSmsTypeData: function () {
        var objSmsType = "";
        var jsonParam = "";
        var serviceUrl = "../SmsSettings/GetSmsTypeDataForCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objSmsType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objSmsType;
    },

};

var SmsSettingsDetailsHelper = {
    InitSmsSettingsDetails: function () {
        SmsSettingsDetailsHelper.PopulateSmsTypeCombo();
        $("#btnSaveSmsText").click(function() {
            SmsSettingsDetailsManager.SaveSmsSettings();
        });
        $("#btnClearAll").click(function () {
            SmsSettingsDetailsHelper.ClearSmsDetailsForm();
        });

        SmsSettingsDetailsHelper.ChangeEventForSMSTextInputFields();
    },
    
    PopulateSmsTypeCombo:function() {
        $("#cmbsmsType").kendoComboBox({
            placeholder: "Select Type",
            dataTextField: 'SmsTypeName',
            dataValueField: 'SmsType',
            dataSource: SmsSettingsDetailsManager.GetSmsTypeData(),
            filter: "contains",
            suggest: true
    });
    },
    
    CreateSmsDetailsObject:function() {
        var obj = new Object();
        obj.SmsId = $("#hdnsmsId").val();
        obj.SmsType = $("#cmbsmsType").data("kendoComboBox").value();
        obj.Salutation = $("#txtSalutation").val();
        obj.Greetings = $("#txtGreetings").val();
        obj.CustomerInfo = $("#txtCustomerInfo").val();
        obj.DueInfo = $("#txtDueInfo").val();
        obj.PaidInfo = $("#txtPaidInfo").val();
        obj.Unit = $("#txtUnit").val();
        obj.CodeInfo = $("#txtCodeInfo").val();
        obj.Request = $("#txtRequest").val();
        obj.Thanking = $("#txtThanking").val();
        obj.GeneralSms = $("#txtGeneralSms").val();
        obj.Status = $("#chkIsActive").is(':checked') == true ? 1 : 0;
        return obj;
    },
    FillSmsDetailsForm: function (sms) {
        
        SmsSettingsDetailsHelper.GenerateFullSms(sms);
        $("#hdnsmsId").val(sms.SmsId);
        $("#cmbsmsType").data("kendoComboBox").value(sms.SmsType);
        $("#txtSalutation").val(sms.Salutation);
        $("#txtGreetings").val(sms.Greetings);
        $("#txtCustomerInfo").val(sms.CustomerInfo);
        $("#txtDueInfo").val(sms.DueInfo);
        $("#txtPaidInfo").val(sms.PaidInfo);
        $("#txtUnit").val(sms.Unit);
        $("#txtCodeInfo").val(sms.CodeInfo);
        $("#txtRequest").val(sms.Request);
        $("#txtThanking").val(sms.Thanking);
        $("#txtGeneralSms").val(sms.GeneralSms);
        if (sms.Status == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }
       
    },
    
    GenerateFullSms: function (sms) {

        var fullSms = (sms.Salutation == null ? "" : sms.Salutation) + " " + (sms.Greetings == null ? "" : sms.Greetings) + " " + (sms.CustomerInfo == null ? "" : sms.CustomerInfo)
            + " " + (sms.DueInfo == null ? "" : sms.DueInfo) + (sms.PaidInfo == null ? "" : sms.PaidInfo) + " " + (sms.Unit == null ? "" : sms.Unit) + " " +
            (sms.CodeInfo == null ? "" : sms.CodeInfo) + " " + (sms.Request == null ? "" : sms.Request) + " " + (sms.Thanking == null ? "" : sms.Thanking) + " " + (sms.GeneralSms == null ? "" : sms.GeneralSms);

        $("#lblMessageText").html(fullSms);
      
        $("#lblMessageCount").html(fullSms.length);
    },
    
    ClearSmsDetailsForm: function () {
        $("#hdnsmsId").val(0);
        $("#cmbsmsType").data("kendoComboBox").value("");
        $("#txtSalutation").val("");
        $("#txtGreetings").val("");
        $("#txtCustomerInfo").val("");
        $("#txtDueInfo").val("");
        $("#txtPaidInfo").val("");
        $("#txtUnit").val("");
        $("#txtCodeInfo").val("");
        $("#txtRequest").val("");
        $("#txtThanking").val("");
        $("#txtGeneralSms").val("");
        $("#chkIsActive").removeProp('checked', 'checked');
        $("#lblMessageText").html("");
    },

    ChangeEventForSMSTextInputFields:function() {
        


    },
};