$(document).ready(function () {
    systemSettingsHelper.createTab();
    //themeLanguageHelper.GenerateMotherCompanyCombo();
    loginRuleHelper.GenerateMotherCompanyCombo();
    themeLanguageHelper.GenerateThemeCombo();
    themeLanguageHelper.GenerateLanguageCombo();
    loginRuleHelper.InitiateloginRuleAndPolicy();

    loginRuleHelper.GeneratePasswordTypeCombo();
    loginRuleHelper.GenerateSpecialCharecterCombo();
    loginRuleHelper.GenerateChangePassAfterFirstLoginCombo();
    loginRuleHelper.GenerateODBCClientListCombo();

    $("#btnSave").click(function () { systemSettingsManager.SaveSystemSettings(); });
    $("#btnClearAll").click(function () { systemSettingsHelper.ClearSystemSettings(); });

});

var systemSettingsManager = {
    SaveSystemSettings: function () {
        if(systemSettingsHelper.formValidator()) {
            
            var objSystemSettings = new Object();
            objSystemSettings = themeLanguageHelper.CreateThemeObjectForSave(objSystemSettings);
            objSystemSettings = loginRuleHelper.CreateLoginRuleObjectForSave(objSystemSettings);
            
            var objSystemSettingsInfo = JSON.stringify(objSystemSettings).replace(/&/g, "^");
            var jsonParam = 'strobjSystemSettingsInfo=' + objSystemSettingsInfo;
            var serviceUrl = "../SystemSettings/SaveSystemSettings/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
        }
        function onSuccess(jsonData) {
            
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success', 'System Settings Saved Successfully.',
                   [{
                       addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                           $noty.close();
                           systemSettingsHelper.ClearSystemSettings();
                       }
                   }]);
            } else {
                AjaxManager.MsgBox('error', 'center', 'Login Failed', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Login Failed', error.statusText,
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();
                           }
                       }]);
        }
    }
};

var systemSettingsHelper = {
    
    createTab: function () {
        $("#tabstrip").kendoTabStrip({});
    },
    
    ClearSystemSettings: function () {
        //themeLanguageHelper.clearThemeForm();
        loginRuleHelper.clearLoginRulePolicy();

        var tabStrip = $("#tabstrip").kendoTabStrip().data("kendoTabStrip");
        tabStrip.select(0);
    },
    

    
    formValidator: function () {
        var res = false;

        //res = themeLanguageHelper.validateThemeForm();
        //if (res == false) {
        //    return res;
        //}
        //else {
        //    res = loginRuleHelper.validateLoginRuleForm();
        //    if (res == false) {
        //        return res;
        //    }
        //}
            res = loginRuleHelper.validateLoginRuleForm();
        if (res == false) {
            return res;
        }
        res = true;
        return res;
    }
};
