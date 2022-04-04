var themeLanguageManager = {
    
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

var themeLanguageHelper = {
    
    GenerateMotherCompanyCombo: function () {
        var objCompany = new Object();
        objCompany = themeLanguageManager.GetMotherCompany();

        $("#cmbCompanyName").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany
        });
    },
    
    GenerateThemeCombo: function () {
        $("#cmbTheme").kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Default", value: "default" },
                { text: "Green", value: "metro" },
                { text: "Gray", value: "gray" },
                { text: "Blue", value: "blueopal" },
                    { text: "Contrast", value: "highcontrast" },
                    { text: "Metro Black", value: "metroblack" },
                    { text: "Silver", value: "silver" },
                    { text: "Yellow", value: "yellow" },
                   { text: "Orange", value: "orange" },
                    { text: "Black", value: "black" }
                   

            ],
            filter: "contains",
            suggest: true,
            index: 0
        });
    },
    
    GenerateLanguageCombo: function () {
        $("#cmbLanguage").kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "English", value: "0" },
                { text: "Bangla", value: "1" }
            ],
            filter: "contains",
            suggest: true,
            index: 0
        });
    },
    
    clearThemeForm: function() {
        $("#hdnSystemSettingsId").val("0");
        $("#cmbCompanyName").val("");
        themeLanguageHelper.GenerateMotherCompanyCombo();
        
        $("#cmbTheme").val("");
        var comboboxforTheme = $("#cmbTheme").data("kendoComboBox");
        comboboxforTheme.refresh();

        $("#cmbLanguage").val("");
        var comboboxforLanguage = $("#cmbLanguage").data("kendoComboBox");
        comboboxforLanguage.refresh();
        
    },
    
    validateThemeForm: function () {
        
        var data = [];
        var validator = $("#divTheme").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");


            //Need to Optimize here...
            var companyId = $("#cmbCompanyName").val();
            var comboboxforCompany = $("#cmbCompanyName").data("kendoComboBox");
            var companyName = comboboxforCompany.text();
            if(companyId == companyName) {
                status.text("Oops! Company Name is invalid.").addClass("invalid");
                $("#cmbCompanyName").val("");
                themeLanguageHelper.GenerateMotherCompanyCombo();
                return false;
            }
            var themeId = $("#cmbTheme").val();
            var comboboxforTheme = $("#cmbTheme").data("kendoComboBox");
            var themeName = comboboxforTheme.text();
            if (themeId == themeName) {
                status.text("Oops! Theme is invalid.").addClass("invalid");
                $("#cmbTheme").val("");
                comboboxforTheme.refresh();
                return false;
            }
            var languageId = $("#cmbLanguage").val();
            var comboboxforLanguage = $("#cmbLanguage").data("kendoComboBox");
            var languageName = comboboxforLanguage.text();
            if (languageId == languageName) {
                status.text("Oops! Language is invalid.").addClass("invalid");
                $("#cmbLanguage").val("");
                comboboxforLanguage.refresh();
                return false;
            }
            


            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
    
    CreateThemeObjectForSave: function (objSystemSettings) {
        objSystemSettings.SettingsId = $("#hdnSystemSettingsId").val();
        objSystemSettings.CompanyId = $("#cmbCompanyName").val();
        objSystemSettings.Theme = $("#cmbTheme").val();
        objSystemSettings.Language = $("#cmbLanguage").val();

        return objSystemSettings;
    },
    
    GenerateThemeObject: function(objSystemSettings) {
       
        var theme = objSystemSettings.Theme;
        var dropdownlist = $("#cmbTheme").data("kendoComboBox");
        dropdownlist.value(theme);
        
        //$("#cmbTheme").kendoComboBox({
        //    dataTextField: "text",
        //    dataValueField: "value",
        //    dataSource: [
        //        { text: "Default", value: "0" },
        //        { text: "Green", value: "1" },
        //        { text: "Gray", value: "2" },
        //        { text: "Blue", value: "3" }
        //    ],
        //    filter: "contains",
        //    suggest: true,
        //    index: theme
        //});
        
        var language = parseInt(objSystemSettings.Language);
        $("#cmbLanguage").val("");
        $("#cmbLanguage").kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "English", value: "0" },
                { text: "Bangla", value: "1" }
            ],
            filter: "contains",
            suggest: true,
            index: language
        });
        
        
    },
    
    changeCompanyName: function () {
        
        var companyId = $("#cmbCompanyName").val();
        var comboboxforCompany = $("#cmbCompanyName").data("kendoComboBox");
        var companyName = comboboxforCompany.text();
        if (companyId == companyName) {
            return false;
        }
        var objSystemSettings = themeLanguageManager.GetSystemSettingsDataByCompanyId(companyId);
        
        if (objSystemSettings.length > 0) {
            $("#hdnSystemSettingsId").val(objSystemSettings[0].SettingsId);
            var thm = $("#cmbTheme").data("kendoComboBox");
            thm.value(objSystemSettings[0].Theme);
            
            var lng = $("#cmbLanguage").data("kendoComboBox");
            lng.value(objSystemSettings[0].Language);

            loginRuleHelper.GenerateLoginRuleData(objSystemSettings);
        }
        
    }

};