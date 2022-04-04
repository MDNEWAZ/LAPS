
var designationDetailsManager = {
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


    SaveDesignationInformation: function () {

        if (designationDetailsHelper.ValidateDesignationInfoForm()) {

            var objDesignation = designationDetailsHelper.CreateDesignationObject();

            var objDesignationInfo = JSON.stringify(objDesignation).replace(/&/g, "^");
            var jsonParam = 'strobjDesignation=' + objDesignationInfo;
            var serviceUrl = "../Designation/SaveDesignation/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);

        }
        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Designation Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            designationDetailsHelper.clearDesignationForm();
                            $("#divgridDesignationSummary").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Already Exist") {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Designation Already Exist For this Company.',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
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

var designationDetailsHelper = {

    populateCompany: function () {
        var objCompany = new Object();
        objCompany = designationDetailsManager.GetMotherCompany();

        $("#cmbCompanyName").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany
        });
    },
    changeCompanyName: function () {
        var companyId = $("#cmbCompanyName").val();
        var comboboxforCompany = $("#cmbCompanyName").data("kendoComboBox");
        var companyName = comboboxforCompany.text();
        if (companyId == companyName) {
            return false;
        }


        var comboboxforDepartment = $("#cmbDepartmentName").data("kendoComboBox");
        comboboxforDepartment.value("");
        comboboxforDepartment.destroy();

        empressCommonHelper.GetDepartmentByCompanyId(companyId, "cmbDepartmentName");


    },


    clearDesignationForm: function () {
        $("#hdnDesignationId").val("0");
        $("#cmbCompanyName").val("");
        $("#cmbDepartmentName").data("kendoComboBox").value("");

        $("#txtDesignationName").val("");

        var combostatus = $("#cmbStatus").data("kendoDropDownList");
        combostatus.value(1);
        designationDetailsHelper.populateCompany();

        $("#designationDetailsDiv > form").kendoValidator();
        $("#designationDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");



    },
    CreateDesignationObject: function () {
        var objDesignation = new Object();
        objDesignation.DesignationId = $("#hdnDesignationId").val();
        objDesignation.CompanyId = $("#cmbCompanyName").val();
        objDesignation.DesignationName = $("#txtDesignationName").val();
        objDesignation.DepartmentId = $("#cmbDepartmentName").data("kendoComboBox").value();
        // get a reference to the dropdown list
        var dropdownlist = $("#cmbStatus").data("kendoDropDownList");
        objDesignation.Status = dropdownlist.value();

        return objDesignation;
    },

    ValidateDesignationInfoForm: function () {
        var data = [];

        var validator = $("#designationDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            var companyId = $("#cmbCompanyName").val();
            var comboboxforCompany = $("#cmbCompanyName").data("kendoComboBox");
            var companyName = comboboxforCompany.text();
            if (companyId == companyName) {
                status.text("Oops! Company Name is invalid.").addClass("invalid");
                $("#cmbCompanyName").val("");
                designationDetailsHelper.populateCompany();
                return false;
            }

            if (!AjaxManager.isValidItem("cmbDepartmentName", true)) {
                status.text("Oops! Department name is invalid.").addClass("invalid");
                return false;
            }

            return true;



        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },

    populateDesignationDetails: function (objDesignation) {

        //designationDetailsHelper.clearDesignationForm();


        var company = $("#cmbCompanyName").data("kendoComboBox");
        company.value(objDesignation.CompanyId);

        //var combobox = $("#cmbDepartmentName").data("kendoComboBox");
        //combobox.destroy();
        empressCommonHelper.GetDepartmentByCompanyId(objDesignation.CompanyId, "cmbDepartmentName");
        var dept = $("#cmbDepartmentName").data("kendoComboBox");
      
        dept.value(objDesignation.DepartmentId);



        var combostatus = $("#cmbStatus").data("kendoDropDownList");
        combostatus.value(objDesignation.Status);

        $("#hdnDesignationId").val(objDesignation.DesignationId);
        $("#txtDesignationName").val(objDesignation.DesignationName);

    },

    LoadCmbStatus: function () {
        var data = [
            { text: "Active", value: "1" },
            { text: "Inactive", value: "0" }
        ];

        $("#cmbStatus").kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: data
        });
    },
};

