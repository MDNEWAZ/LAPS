

var moduleDetailsManager = {
    SaveModuleInformation: function () {
        if (moduleDetailsHelper.validator()) {

            var objModule = moduleDetailsHelper.CreateModuleForSaveData();

            objModule = JSON.stringify(objModule).replace(/&/g, "^");
            var jsonParam = 'strobjModuleInfo=' + objModule;
            var serviceUrl = "../Module/SaveModule/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);

        }
        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Module Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            moduleDetailsHelper.clearModuleForm();
                            $("#gridModule").data("kendoGrid").dataSource.read();
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

var moduleDetailsHelper = {
    clearModuleForm: function () {
        $("#hdModuleId").val("0");
        $("#txtModuleName").val("");
        
        $("#moduleDetailsDiv > form").kendoValidator();
        $("#moduleDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");
    },

    validator: function () {
        var data = [];
        var validator = $("#moduleDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {

            var res = AjaxManager.checkSpecialCharacters('txtModuleName');
            if (res) {
                status.text("").addClass("valid");
                return true;
            }
            else {
                status.text("Oops! There is invalid data in the form.").addClass("invalid");
                return false;
            }
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
    CreateModuleForSaveData: function () {
        var objModule = new Object();
        objModule.ModuleId = $("#hdModuleId").val();
        objModule.ModuleName = $("#txtModuleName").val();
       
        return objModule;
    },
    
    FillModuleDetailsInForm: function (module) {
        $("#hdModuleId").val(module.ModuleId);
        $("#txtModuleName").val(module.ModuleName);
    }
};