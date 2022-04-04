
var IncentiveSettingsDetailsManager = {    
    SaveIncentiveDetails: function () {
        var validator = $("#IncentiveDetailsDiv").kendoValidator().data("kendoValidator"),
         status = $(".status");
        if (validator.validate()) {
            var objIncentive = IncentiveSettingsDetailsHelper.CreateIncentiveDetailsObj();
            var jsonParam = "objIncentive:" + JSON.stringify(objIncentive);
            var serviceUrl = "../IncentiveSettings/SaveIncentiveSettings";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Incentive Settings Save Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            IncentiveSettingsDetailsHelper.ClearIncentiveDetailsForm();
                            $("#incentiveSummaryGrid").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Exists") {

                AjaxManager.MsgBox('warning', 'center', 'Already Exists:', 'Incentive Settings Already Exist',
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
    }
};


var IncentiveSettingsDetailsHelper = {

    InitIncentiveSettingsDetails:function() {
        $("#txtNumberOfSale").kendoNumericTextBox({format:"#"});
        $("#txtIncentiveAmount").kendoNumericTextBox({});
        
        $("#btnSave").click(function () {
            IncentiveSettingsDetailsManager.SaveIncentiveDetails();
        });
    },
    CreateIncentiveDetailsObj: function() {
        var obj = new Object();
        obj.IncentiveId = $("#hdnIncentiveId").val();
        obj.NumberOfSale = $("#txtNumberOfSale").data("kendoNumericTextBox").value();
        obj.IncentiveAmount = $("#txtIncentiveAmount").data("kendoNumericTextBox").value();
        obj.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;
        return obj;
    },
    
    ClearIncentiveDetailsForm:function() {
        $("#hdnIncentiveId").val(0);
        $("#txtNumberOfSale").data("kendoNumericTextBox").value("");
        $("#txtIncentiveAmount").data("kendoNumericTextBox").value("");
        
        $("#IncentiveDetailsDiv > form").kendoValidator();
        $("#IncentiveDetailsDiv").find("span.k-tooltip-validation").hide();
        $("#chkIsActive").removeProp('checked', 'checked');

        var status = $(".status");
        status.text("").removeClass("invalid");
    },
    FillIncentiveSettingsDetailsForm: function(obj) {
        $("#hdnIncentiveId").val(obj.IncentiveId);
        $("#txtNumberOfSale").data("kendoNumericTextBox").value(obj.NumberOfSale);
        $("#txtIncentiveAmount").data("kendoNumericTextBox").value(obj.IncentiveAmount);

        if (obj.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }

    }
};