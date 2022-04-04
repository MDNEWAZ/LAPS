var creditPeriodManager = {
    SaveCredidPeriodInformation: function () {
        if (creditPeriodHelper.validator()) {

            var objCreditPeriod = creditPeriodHelper.CreateCreditPeriodForSaveData();

            objCreditPeriod = JSON.stringify(objCreditPeriod).replace(/&/g, "^");
            var jsonParam = 'objCreditPeriodInfo:' + objCreditPeriod;
            var serviceUrl = "../CreditPeriod/SaveCreditPeriod/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        }
        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Credid Period Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            creditPeriodHelper.clearForm();
                            $("#gridCreditPeriod").data("kendoGrid").dataSource.read();
                        }
                    }]);

            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Failed', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Failed', error.statusText,
                         [{
                             addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                 $noty.close();
                             }
                         }]);
        }
    }
};


var creditPeriodHelper = {
    clearForm: function () {
        $("input[type='text']").val("");
    },
    validator: function () {
        var data = [];
        var validator = $("#creditPeriodDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {

            var chkspAcesName = AjaxManager.checkSpecialCharacters("txtDefaultInstallmentNo");
            if (!chkspAcesName) {
                status.text("Oops! There is invalid data in the form.").addClass("invalid");
                return false;
            }
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
    CreateCreditPeriodForSaveData: function () {
        var obj = new Object();
        
        obj.DefaultInstallmentNo = $("#txtDefaultInstallmentNo").val();

        return obj;
    },
    FillCreditPeriodDetailsInForm: function (item) {
        
        $("#txtDefaultInstallmentNo").val(item.DefaultInstallmentNo);
    }
};




