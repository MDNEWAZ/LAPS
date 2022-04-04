
var PhoneNoDetailslManager = {    
    SavePhoneNoSettings: function () {
        var validator = $("#phoneDetailsDiv").kendoValidator().data("kendoValidator"),
         status = $(".status");
        if (validator.validate()) {
            var objphone = PhoneNoDetailsHelper.CreatePhoneDetailsObject();
            var phoneObj = JSON.stringify(objphone);
            var jsonParam = "phoneObj:" + phoneObj;
            var serviceUrl = "../PhoneNoSettings/SavePhoneSettings";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'SMS Save Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            PhoneNoDetailsHelper.ClearPhoneNoDetailsForm();
                            $("#gridPhoneSettingsSummary").data("kendoGrid").dataSource.read();
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
};

var PhoneNoDetailsHelper = {
    InitPhoneNoDetails: function() {
        $("#btnSavePhoneNo").click(function () {
            PhoneNoDetailslManager.SavePhoneNoSettings();
        });
        $("#btnClear").click(function () {
            PhoneNoDetailsHelper.ClearPhoneNoDetailsForm();
        });
    },
    
    CreatePhoneDetailsObject: function () {
        var obj = new Object();
        obj.PhoneSettingsId = $("#hdnPhoneSettingsId").val();
        obj.PhoneNumber = $("#txtPhoneNumber").val();
        obj.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;
        return obj;
    },
    FillPhoneNoDetailsForm: function (phoneObj) {

        $("#hdnPhoneSettingsId").val(phoneObj.PhoneSettingsId);
        $("#txtPhoneNumber").val(phoneObj.PhoneNumber);
        if (phoneObj.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }

    },
    
    ClearPhoneNoDetailsForm: function () {
        $("#hdnPhoneSettingsId").val(0);
        $("#txtPhoneNumber").val("");
        $("#chkIsActive").removeProp('checked', 'checked');
        
        $("#phoneDetailsDiv > form").kendoValidator();
        $("#phoneDetailsDiv").find("span.k-tooltip-validation").hide();

        var status = $(".status");
        status.text("").removeClass("invalid");
      
    },
};