
var departmentDetailsManager = {
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
    GetEmployeeByCompanyId: function (companyId) {
        debugger;
        var objEmployee = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../Department/GetEmployeeByCompanyId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objEmployee = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployee;
    },
    SaveDepartmentInformation: function () {
        if (departmentDetailsHelper.ValidateDepartmentInfoForm()) {
            var objDepartment = departmentDetailsHelper.CreateDepartmentObject();
            var objDepartmentInfo = JSON.stringify(objDepartment).replace(/&/g, "^");
            var jsonParam = 'strobjDepartment=' + objDepartmentInfo;
            var serviceUrl = "../Department/SaveDepartment/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
        }
        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Department Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            departmentDetailsHelper.clearDepartmentForm();
                            $("#gridDepartment").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Department Already Exist") {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Department Already Exist For this Company.',
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
            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();
                           }
                       }]);
        }
    }
};

var departmentDetailsHelper = {
    
    populateCompany: function () {
        var objCompany = new Object();
        objCompany = departmentDetailsManager.GetMotherCompany();

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

        var combobox = $("#cmbEmployee").data("kendoComboBox");
        combobox.destroy();

        departmentDetailsHelper.GetEmployeeByCompanyId(companyId);
    },
    GetEmployeeByCompanyId: function (companyId) {
        debugger;
        var objEmployee;
        $('#cmbEmployee').val("");
        objEmployee = departmentDetailsManager.GetEmployeeByCompanyId(companyId);

        $("#cmbEmployee").kendoComboBox({
            placeholder: "Select Employee...",
            dataTextField: "FullName",
            dataValueField: "HRRecordId",
            dataSource: objEmployee
        });
    },
    clearDepartmentForm: function () {
        $("#hdnDepartmentId").val("0");
        $("#cmbCompanyName").val("");
        departmentDetailsHelper.populateCompany();
        $("#txtDepartmentName").val("");
        $("#cmbEmployee").val("");
        departmentDetailsHelper.GetEmployeeByCompanyId(0);

        $("#departmentDetailsDiv > form").kendoValidator();
        $("#departmentDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");

    },
    CreateDepartmentObject: function () {
        var objDepartment = new Object();
        objDepartment.DepartmentId = $("#hdnDepartmentId").val();
        objDepartment.CompanyId = $("#cmbCompanyName").val();
        objDepartment.DepartmentName = $("#txtDepartmentName").val();
        objDepartment.DepartmentHeadId = $("#cmbEmployee").val();
        if (objDepartment.DepartmentHeadId == "") {
            objDepartment.DepartmentHeadId = 0;
        }
        objDepartment.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;
        return objDepartment;
    },
    ValidateDepartmentInfoForm: function () {
        var data = [];

        var validator = $("#departmentDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            var companyId = $("#cmbCompanyName").val();
            var comboboxforCompany = $("#cmbCompanyName").data("kendoComboBox");
            var companyName = comboboxforCompany.text();
            if (companyId == companyName) {
                status.text("Oops! Company Name is invalid.").addClass("invalid");
                $("#cmbCompanyName").val("");
                return false;
            }
            var employeeId = $("#cmbEmployee").val();
            var comboboxforEmployee = $("#cmbEmployee").data("kendoComboBox");
            var employeeName = comboboxforEmployee.text();
            if (employeeId == employeeName && employeeId != "") {
                status.text("Oops! Employee Name is invalid.").addClass("invalid");
                $("#cmbEmployee").val("");
                return false;
            }

            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
    populateDepartmentDetails: function (objDepartment) {
        departmentDetailsHelper.clearDepartmentForm();
        $("#hdnDepartmentId").val(objDepartment.DepartmentId);
        var company = $("#cmbCompanyName").data("kendoComboBox");
        company.value(objDepartment.CompanyId);
        var combobox = $("#cmbEmployee").data("kendoComboBox");
        combobox.destroy();
        departmentDetailsHelper.GetEmployeeByCompanyId(objDepartment.CompanyId);
        $("#txtDepartmentName").val(objDepartment.DepartmentName);
        if (objDepartment.DepartmentHeadId != 0) {
            var employee = $("#cmbEmployee").data("kendoComboBox");
            employee.value(objDepartment.DepartmentHeadId);
        }
        if (objDepartment.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }

    }
};

