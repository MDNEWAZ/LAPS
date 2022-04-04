

var accessControlDetailsManager = {
    SaveAccessControlInformation: function () {
        if (accessControlDetailsHelper.validator()) {

            var objAccessControl = accessControlDetailsHelper.CreateAccessControlForSaveData();

            objAccessControl = JSON.stringify(objAccessControl).replace(/&/g, "^");
            var jsonParam = 'strobjAccessControlInfo=' + objAccessControl;
            var serviceUrl = "../AccessControl/SaveAccessControl/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);

        }
        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Access Name Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            accessControlDetailsHelper.clearAccessControlForm();
                            $("#gridAccessControl").data("kendoGrid").dataSource.read();
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
var accessControlDetailsHelper = {
    clearAccessControlForm: function () {
        $("#hdAccessControlId").val("0");
        $("#txtAccessControlName").val("");
    },
    validator: function () {
        var data = [];
        var validator = $("#accessControlDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            
            var chkspAcesName = AjaxManager.checkSpecialCharacters("txtAccessControlName");
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
    CreateAccessControlForSaveData: function () {
        var objaccessControl = new Object();
        objaccessControl.AccessId = $("#hdAccessControlId").val();
        objaccessControl.AccessName = $("#txtAccessControlName").val();
       
        return objaccessControl;
    },
    FillAccessControlDetailsInForm: function (accessControl) {
        $("#hdAccessControlId").val(accessControl.AccessId);
        $("#txtAccessControlName").val(accessControl.AccessName);
    }
};