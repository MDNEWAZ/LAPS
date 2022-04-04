var loginRuleManager = {
    GetMotherCompany: function () {
        var objCompany = "";
        var jsonParam = "";
        var serviceUrl = "../Company/GetMotherCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCompany;
    },
    
    GetSystemSettingsDataByCompanyId: function (companyId) {
        var objSystemSettings = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../SystemSettings/GetSystemSettingsDataByCompanyId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objSystemSettings = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objSystemSettings;
    }
};

var loginRuleHelper = {
    
    GenerateMotherCompanyCombo: function () {
        var objCompany = new Object();
        objCompany = loginRuleManager.GetMotherCompany();

        $("#cmb_CompanyName").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany
        });
    },
    
    GeneratePasswordTypeCombo: function () {
        $("#cmbPasswordType").kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Alphabetic", value: "0" },
                { text: "Numeric", value: "1" },
                { text: "Alphanumeric", value: "2" }
            ],
            filter: "contains",
            suggest: true,
            index: 2
        });
    },
    
    GenerateSpecialCharecterCombo: function () {
        $("#cmbSpecialCharacterinPassword").kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Yes", value: "1" },
                { text: "No", value: "0" }
            ],
            filter: "contains",
            suggest: true,
            index: 1
        });
    },
    
    GenerateChangePassAfterFirstLoginCombo: function () {
        $("#cmbChangePasswordat1stLogin").kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Yes", value: "1" },
                { text: "No", value: "0" }
            ],
            filter: "contains",
            suggest: true,
            index: 1
        });
    },
    
    clearLoginRulePolicy: function () {
        
        $("#cmb_CompanyName").val("");
        loginRuleHelper.GenerateMotherCompanyCombo();
        
        //$("#txtMinimumLoginLength").val('5');
        //$("#txtMinimumPasswordLength").val('6');
        
        $("#cmbPasswordType").val('');
        var comboboxforPasswordType = $("#cmbPasswordType").data("kendoComboBox");
        comboboxforPasswordType.refresh();
        
        $("#cmbSpecialCharacterinPassword").val('');
        var comboboxforSpecialCharInPass = $("#cmbSpecialCharacterinPassword").data("kendoComboBox");
        comboboxforSpecialCharInPass.refresh();

        //$("#txtWrongattempttoLock").val('3');
        //$("#txtChangePasswordafter").val('45');
        
        $("#cmbChangePasswordat1stLogin").val('');
        var comboboxforchangePassAfterFirstLogIn = $("#cmbChangePasswordat1stLogin").data("kendoComboBox");
        comboboxforchangePassAfterFirstLogIn.refresh();
        
        //$("#txtPasswordExpiredafter").val('30');
        $("#txtResetPassword").val('');
        //$("#txtOldPasswordRestriction").val('3');
        loginRuleHelper.InitiateloginRuleAndPolicy();
    },
    
    GenerateODBCClientListCombo: function () {
        $("#cmbOdbcClientList").kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "No", value: "0" },
                { text: "Yes", value: "1" }
            ],
            filter: "contains",
            suggest: true,
            index: 0
        });
    },
    
    validateLoginRuleForm: function () {
        var data = [];
        var validator = $("#divLoginRule").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");


            //Need to Optimize here...
            

            var companyId = $("#cmb_CompanyName").val();
            var comboboxforCompany = $("#cmb_CompanyName").data("kendoComboBox");
            var companyName = comboboxforCompany.text();
            if (companyId == companyName) {
                status.text("Oops! Company Name is invalid.").addClass("invalid");
                $("#cmb_CompanyName").val("");
                loginRuleHelper.GenerateMotherCompanyCombo();
                return false;
            }
            
            var passTypeId = $("#cmbPasswordType").val();
            var comboboxforPasswordType = $("#cmbPasswordType").data("kendoComboBox");
            
            var passTypeName = comboboxforPasswordType.text();
            if (passTypeId == passTypeName) {
                status.text("Oops! Password Type is invalid.").addClass("invalid");
                $("#cmbPasswordType").val("");
                comboboxforPasswordType.refresh();
                return false;
            }
            var spcharId = $("#cmbSpecialCharacterinPassword").val();
            var comboboxforSpecialCharInPass = $("#cmbSpecialCharacterinPassword").data("kendoComboBox");
            var spcharName = comboboxforSpecialCharInPass.text();
            if (spcharId == spcharName) {
                status.text("Oops! Special Charecter in password is invalid.").addClass("invalid");
                $("#cmbSpecialCharacterinPassword").val("");
                comboboxforSpecialCharInPass.refresh();
                return false;
            }
            var firstLoginId = $("#cmbChangePasswordat1stLogin").val();
            var comboboxforchangePassAfterFirstLogIn = $("#cmbChangePasswordat1stLogin").data("kendoComboBox");
            var firstLoginName = comboboxforchangePassAfterFirstLogIn.text();
            if (firstLoginId == firstLoginName) {
                status.text("Oops! Change Password at first login ID is invalid.").addClass("invalid");
                $("#cmbChangePasswordat1stLogin").val("");
                comboboxforchangePassAfterFirstLogIn.refresh();
                return false;
            }



            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
    
    CreateLoginRuleObjectForSave: function (objSystemSettings) {
        objSystemSettings.CompanyId = $("#cmb_CompanyName").val();
        objSystemSettings.MinLoginLength = $("#txtMinimumLoginLength").val();
        objSystemSettings.MinPassLength = $("#txtMinimumPasswordLength").val();
        objSystemSettings.PassType = $("#cmbPasswordType").val();
        objSystemSettings.SpecialCharAllowed = $("#cmbSpecialCharacterinPassword").val();
        if (objSystemSettings.SpecialCharAllowed == "1") {
            objSystemSettings.SpecialCharAllowed = true;
        }
        else {
            objSystemSettings.SpecialCharAllowed = false;
        }
        objSystemSettings.WrongAttemptNo = $("#txtWrongattempttoLock").val();
        objSystemSettings.ChangePassDays = $("#txtChangePasswordafter").val();
        objSystemSettings.ChangePassFirstLogin = $("#cmbChangePasswordat1stLogin").val();
        if (objSystemSettings.ChangePassFirstLogin == "1") {
            objSystemSettings.ChangePassFirstLogin = true;
        }
        else {
            objSystemSettings.ChangePassFirstLogin = false;
        }
        objSystemSettings.PassExpiryDays = $("#txtPasswordExpiredafter").val();
        objSystemSettings.ResetPass = $("#txtResetPassword").val();
        objSystemSettings.OldPassUseRestriction = $("#txtOldPasswordRestriction").val();
        objSystemSettings.OdbcClientList = $("#cmbOdbcClientList").val();
        if (objSystemSettings.OdbcClientList == "1") {
            objSystemSettings.OdbcClientList = true;
        }
        else {
            objSystemSettings.OdbcClientList = false;
        }
        return objSystemSettings;
    },
    
    GenerateLoginRuleData: function (objSystemSettings) {
          
        var cmbComp = $("#cmb_CompanyName").data("kendoComboBox");
        cmbComp.value(objSystemSettings.CompanyId);



        $("#txtMinimumLoginLength").val(objSystemSettings.MinLoginLength);
        var minlogInLengthtextbox = $("#txtMinimumLoginLength").data("kendoNumericTextBox");
        minlogInLengthtextbox.value(objSystemSettings.MinLoginLength);
        
        $("#txtMinimumPasswordLength").val(objSystemSettings.MinPassLength);
        var minPassLengthtextbox = $("#txtMinimumPasswordLength").data("kendoNumericTextBox");
        minPassLengthtextbox.value(objSystemSettings.MinPassLength);
        
        var pasType = $("#cmbPasswordType").data("kendoComboBox");
        pasType.select(objSystemSettings.PassType);
        
        var spcaral = $("#cmbSpecialCharacterinPassword").data("kendoComboBox");
        if (objSystemSettings.SpecialCharAllowed == true) {
            $("#cmbSpecialCharacterinPassword").val("");
            $("#cmbSpecialCharacterinPassword").kendoComboBox({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: [
                    { text: "Yes", value: "1" },
                    { text: "No", value: "0" }
                ],
                filter: "contains",
                suggest: true,
                index: 0
            });
        }
        else {
            $("#cmbSpecialCharacterinPassword").val("");
            $("#cmbSpecialCharacterinPassword").kendoComboBox({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: [
                    { text: "Yes", value: "1" },
                    { text: "No", value: "0" }
                ],
                filter: "contains",
                suggest: true,
                index: 1
            });
        }
        $("#txtWrongattempttoLock").val(objSystemSettings.WrongAttemptNo);
        var wrongAttemptLocktextbox = $("#txtWrongattempttoLock").data("kendoNumericTextBox");
        wrongAttemptLocktextbox.value(objSystemSettings.WrongAttemptNo);

        $("#txtChangePasswordafter").val(objSystemSettings.ChangePassDays);
        var changePassAftertextbox = $("#txtChangePasswordafter").data("kendoNumericTextBox");
        changePassAftertextbox.value(objSystemSettings.ChangePassDays);
        
        var chgpassfrlgin = $("#cmbChangePasswordat1stLogin").data("kendoComboBox");
        if (objSystemSettings.ChangePassFirstLogin == true) {
            $("#cmbChangePasswordat1stLogin").val("");
            $("#cmbChangePasswordat1stLogin").kendoComboBox({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: [
                    { text: "Yes", value: "1" },
                    { text: "No", value: "0" }
                ],
                filter: "contains",
                suggest: true,
                index: 0
            });
        }
        else {
            $("#cmbChangePasswordat1stLogin").val("");
            $("#cmbChangePasswordat1stLogin").kendoComboBox({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: [
                    { text: "Yes", value: "1" },
                    { text: "No", value: "0" }
                ],
                filter: "contains",
                suggest: true,
                index: 1
            });
        }
        $("#txtPasswordExpiredafter").val(objSystemSettings.PassExpiryDays);
        var changePassExpiretextbox = $("#txtPasswordExpiredafter").data("kendoNumericTextBox");
        changePassExpiretextbox.value(objSystemSettings.PassExpiryDays);

        $("#txtResetPassword").val(objSystemSettings.ResetPass);
        $("#txtOldPasswordRestriction").val(objSystemSettings.OldPassUseRestriction);
        var oldPassrestructiontextbox = $("#txtOldPasswordRestriction").data("kendoNumericTextBox");
        oldPassrestructiontextbox.value(objSystemSettings.OldPassUseRestriction);
        
        var odbcClientList = $("#cmbOdbcClientList").data("kendoComboBox");
        if (objSystemSettings.OdbcClientList == true) {
            $("#cmbOdbcClientList").val("");
            $("#cmbOdbcClientList").kendoComboBox({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: [
                    { text: "Yes", value: "1" },
                    { text: "No", value: "0" }
                ],
                filter: "contains",
                suggest: true,
                index: 0
            });
        }
        else {
            $("#cmbOdbcClientList").val("");
            $("#cmbOdbcClientList").kendoComboBox({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: [
                    { text: "Yes", value: "1" },
                    { text: "No", value: "0" }
                ],
                filter: "contains",
                suggest: true,
                index: 1
            });
        }

    },
    
    changeCompanyName: function () {

        var companyId = $("#cmb_CompanyName").val();
        var comboboxforCompany = $("#cmb_CompanyName").data("kendoComboBox");
        var companyName = comboboxforCompany.text();
        if (companyId == companyName) {
            return false;
        }
        var objSystemSettings = loginRuleManager.GetSystemSettingsDataByCompanyId(companyId);
        $("#hdnSystemSettingsId").val("0");
        //if (objSystemSettings.length > 0) {
         //   $("#hdnSystemSettingsId").val(objSystemSettings[0].SettingsId);
            ////var thm = $("#cmbTheme").data("kendoComboBox");
           // //thm.value(objSystemSettings[0].Theme);

            ////var lng = $("#cmbLanguage").data("kendoComboBox");
            ////lng.value(objSystemSettings[0].Language);

           // loginRuleHelper.GenerateLoginRuleData(objSystemSettings);
        //}
        if(objSystemSettings != null) {
            $("#hdnSystemSettingsId").val(objSystemSettings.SettingsId);
            loginRuleHelper.GenerateLoginRuleData(objSystemSettings);
            themeLanguageHelper.GenerateThemeObject(objSystemSettings);
        }
        else {
            loginRuleHelper.InitiateloginRuleAndPolicy();
        }

    },
    
    InitiateloginRuleAndPolicy: function () {
        $("#txtMinimumLoginLength").kendoNumericTextBox();
        $("#txtMinimumPasswordLength").kendoNumericTextBox();
        $("#txtWrongattempttoLock").kendoNumericTextBox();
        $("#txtChangePasswordafter").kendoNumericTextBox();
        $("#txtPasswordExpiredafter").kendoNumericTextBox();
        $("#txtOldPasswordRestriction").kendoNumericTextBox();
        

        var minlogInLengthtextbox = $("#txtMinimumLoginLength").data("kendoNumericTextBox");
        minlogInLengthtextbox.value("5");

        var minPassLengthtextbox = $("#txtMinimumPasswordLength").data("kendoNumericTextBox");
        minPassLengthtextbox.value("6");

        var wrongAttemptLocktextbox = $("#txtWrongattempttoLock").data("kendoNumericTextBox");
        wrongAttemptLocktextbox.value("3");
        
        var changePassAftertextbox = $("#txtChangePasswordafter").data("kendoNumericTextBox");
        changePassAftertextbox.value("30");
        

        var changePassExpiretextbox = $("#txtPasswordExpiredafter").data("kendoNumericTextBox");
        changePassExpiretextbox.value("30");

        var oldPassrestructiontextbox = $("#txtOldPasswordRestriction").data("kendoNumericTextBox");
        oldPassrestructiontextbox.value("3");
        
        $("#txtMinimumLoginLength").parent().css('width', "15.4em");
        $("#txtMinimumPasswordLength").parent().css('width', "15.4em");
        $("#txtWrongattempttoLock").parent().css('width', "15.4em");
        $("#txtChangePasswordafter").parent().css('width', "15.4em");
        $("#txtPasswordExpiredafter").parent().css('width', "15.4em");
        $("#txtOldPasswordRestriction").parent().css('width', "15.4em");

    }

};