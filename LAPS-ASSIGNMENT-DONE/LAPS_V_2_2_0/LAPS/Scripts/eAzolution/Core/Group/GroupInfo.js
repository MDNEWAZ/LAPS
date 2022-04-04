
var allmoduleArray = [];

var groupInfoManager = {

    GetModule: function () {
        var objModule = "";
        var jsonParam = "";
        var serviceUrl = "../Module/SelectModule/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false,false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objModule = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objModule;
    },
    
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
    GetRootCompany: function () {
        var objCompany = "";
        var jsonParam = "";
        var serviceUrl = "../Company/GetRootCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCompany;
    }
    
};

var groupInfoHelper = {
    
    GenerateModuleForGroupInfo: function () {
        var link = "<ul>";
        allmoduleArray = [];
        var objModuleList = new Object();
        objModuleList = groupInfoManager.GetModule();
        
        

        for(var i=0; i<objModuleList.length; i++) {

            link += "<li><input type=\"checkbox\" class=\"chkBox\" id=\"chkModule" + objModuleList[i].ModuleId + "\" onclick=\"groupPermisionHelper.populateModuleCombo(" + objModuleList[i].ModuleId + ", '" + objModuleList[i].ModuleName + "', this.id)\"/> " + objModuleList[i].ModuleName + "</li>";
            allmoduleArray.push(objModuleList[i]);
        }
        link += "</ul>";
        $("#dynamicCheckBoxForModule").html(link);
    },
    
    GenerateMotherCompanyCombo: function () {
        var objCompany = new Object();
        objCompany = groupInfoManager.GetMotherCompany();

        $("#cmbCompanyName").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany
        });
    },
    
    clearGroupInfo: function () {
        $('.chkBox').attr('checked', false);
        $('#txtGroupName').val('');
        $('#cmbCompanyName').val('');
        $("#hdnGroupId").val('0');
        groupInfoHelper.GenerateMotherCompanyCombo();
        
        $("#divGroupId > form").kendoValidator();
        $("#divGroupId").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");
        
        $("#chkIsViewer").attr('checked', false);
        
        var tabStrip = $("#tabstrip").kendoTabStrip().data("kendoTabStrip");
        tabStrip.select(0);

    },
    
    validateForm: function () {
        var data = [];
        var validator = $("#divGroupId").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            var tabStrip = $("#tabstrip").kendoTabStrip().data("kendoTabStrip");
            tabStrip.select(0);
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }

    },
    
    CreateGroupInfo: function() {
        var objGroupInfo = new Object();
        objGroupInfo.GroupId = $("#hdnGroupId").val();
        objGroupInfo.CompanyId = $("#cmbCompanyName").val();
        objGroupInfo.GroupName = $("#txtGroupName").val();
        objGroupInfo.IsDefault = $("#chkIsDefault").is(':checked') == true ? 1 : 0;
        objGroupInfo.IsViewer = $("#chkIsViewer").is(':checked') == true ? 1 : 0;
        return objGroupInfo;
    },
    
    populateGroupInformationDetails: function (objGroup) {
        $("#hdnGroupId").val(objGroup.GroupId);
        var company = $("#cmbCompanyName").data("kendoComboBox");
        company.value(objGroup.CompanyId);
        $("#txtGroupName").val(objGroup.GroupName);
        
        $('#chkIsDefault').attr('checked', objGroup.IsDefault == 1 ? true : false);
        
        $('#chkIsViewer').attr('checked', objGroup.IsViewer == 1 ? true : false);
    }
    
    

};