
$(document).ready(function () {

    AuditTrailsReportHelper.initiatePersonalInfoReport();

    $("#cmbDepartmentNameDetails").change(function () {
        AuditTrailsReportHelper.changeDepartmentName();
    });

    $("#btnGenerate").click(function () {
        AuditTrailsReportManager.AuditTrailsReportPrint();
    });

});

var AuditTrailsReportManager = {
    
    AuditTrailsReportPrint: function () {
        var objReportParam = AuditTrailsReportHelper.CreateAuditTrailsReportParamObject();
        var objReportParamInfo = JSON.stringify(objReportParam);

        var jsonParam = "objReportParams:" + objReportParamInfo;
        var serviceUrl = "../Reports/GenerateAuditTrailsReport/";
        AjaxManager.GetReport(serviceUrl, jsonParam, onFailed);
        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
        }
    },
};

var AuditTrailsReportHelper = {

    initiatePersonalInfoReport: function () {

        empressCommonHelper.GenerareHierarchyCompanyCombo("cmbCompanyNameDetails");

        if (CurrentUser.CompanyId != null) {
            var companyData = $("#cmbCompanyNameDetails").data("kendoComboBox");
            companyData.value(CurrentUser.CompanyId);
            AuditTrailsReportHelper.changeCompanyName();
        }
        AuditTrailsReportHelper.datepickerForAuditTrails();
    },

    ViewDetails: function (data) {

        //$('<a href="' + data.AttachedDocument + '" target="_blank" title="View">View</a>').appendTo(container);
        if (data.UploadFile != "") {
            var res = '<a href="' + data.UploadFile + '" target="_blank" title="View">View</a>';
        }
        else {
            res = "";
        }
        return res;

    },

    datepickerForAuditTrails: function () {
        var fromDate = $("#txtFromDate").kendoDatePicker({
            value: new Date(),
            max: new Date(),
            format: "MM/dd/yyyy",
            //change: DeploymentPlanReportHelper.changeFromDate
        }).data("kendoDatePicker");

        var toDate = $("#txtToDate").kendoDatePicker({
            value: new Date(),
            max: new Date(),
            format: "MM/dd/yyyy",
            //change: DeploymentPlanReportHelper.changeToDate
        }).data("kendoDatePicker");

        $("#txtFromDate").parent().parent().css('width', "17.4em");
        $("#txtToDate").parent().parent().css('width', "17.4em");


    },
    changeFromDate: function () {
        var dt = $("#txtFromDate").val();

        if (!AjaxManager.isDate(dt)) {
            alert("Please Insert a valid Date");
            $("#txtFromDate").val('');
            $("#txtFromDate").focus();

        }

    },
    changeToDate: function () {
        var dt = $("#txtToDate").val();
        if (!AjaxManager.isDate(dt)) {
            alert("Please Insert a valid Date");
            $("#txtToDate").val('');
            $("#txtToDate").focus();
        }
    },

    CreateReportParamObject: function () {
        var array = [];
        var objRep = new Object();

        objRep.FromDate = $("#txtFromDate").val();
        objRep.ToDate = $("#txtToDate").val();

        var objDataSource = new Object();
        objDataSource.CompanyId = $("#cmbCompanyNameDetails").val();
        objDataSource.BranchId = $("#cmbBranchDetails").val();
        objDataSource.DepartmentId = $("#cmbDepartmentNameDetails").val();
        objDataSource.HrRecordId = $("#cmbEmployee").val();
        objDataSource.LeaveTypeId = $("#cmbLeaveType").val();
        array.push(objDataSource);
        objRep.DataSource = array;
        return objRep;

    },

    changeCompanyName: function () {

        var comboboxbranch = $("#cmbBranchDetails").data("kendoComboBox");
        var comboboxDep = $("#cmbDepartmentNameDetails").data("kendoComboBox");
        var comboboxEmp = $("#cmbEmployee").data("kendoComboBox");


        var companyData = $("#cmbCompanyNameDetails").data("kendoComboBox");
        var companyId = companyData.value();
        var companyName = companyData.text();

        if (companyId == companyName) {
            companyData.value('');
            comboboxbranch.value('');
            comboboxbranch.destroy();

            comboboxDep.value('');
            comboboxDep.destroy();
            comboboxEmp.value('');
            comboboxEmp.destroy();


            empressCommonHelper.GenerateBranchCombo(0, "cmbBranchDetails");
            empressCommonHelper.GetDepartmentByCompanyId(0, "cmbDepartmentNameDetails");
            empressCommonHelper.GenerateEmployeeByCompanyId(0, 0, 0, "cmbEmployee");
            return false;
        }
        if (comboboxbranch != undefined) {
            comboboxbranch.value('');
            comboboxbranch.destroy();
        }
        if (comboboxDep != undefined) {
            comboboxDep.value('');
            comboboxDep.destroy();
        }

        if (comboboxEmp != undefined) {
            comboboxEmp.value('');
            comboboxEmp.destroy();
        }

        empressCommonHelper.GenerateBranchCombo(companyId, "cmbBranchDetails");

        if (CurrentUser != null) {
            comboboxbranch = $("#cmbBranchDetails").data("kendoComboBox");
            comboboxbranch.value(CurrentUser.BranchId);
            AuditTrailsReportHelper.changeBranchName();
        }

        empressCommonHelper.GetDepartmentByCompanyId(companyId, "cmbDepartmentNameDetails");
        empressCommonHelper.GenerateEmployeeByCompanyId(companyId, 0, 0, "cmbEmployee");


    },

    changeBranchName: function () {

        var comboboxbranch = $("#cmbBranchDetails").data("kendoComboBox");
        var comboboxDep = $("#cmbDepartmentNameDetails").data("kendoComboBox");
        var comboboxEmp = $("#cmbEmployee").data("kendoComboBox");


        var companyData = $("#cmbCompanyNameDetails").data("kendoComboBox");
        var companyId = companyData.value();
        var companyName = companyData.text();

        var branchId = comboboxbranch.value();
        var branchName = comboboxbranch.text();
        if (branchId == branchName) {
            comboboxbranch.value('');
            comboboxDep.value('');
            comboboxDep.destroy();
            comboboxEmp.value('');
            comboboxEmp.destroy();

            empressCommonHelper.GetDepartmentByCompanyId(companyId, "cmbDepartmentNameDetails");
            empressCommonHelper.GenerateEmployeeByCompanyId(companyId, 0, 0, "cmbEmployee");
            return false;
        }

        if (comboboxDep != undefined) {
            comboboxDep.value('');
            comboboxDep.destroy();
        }

        if (comboboxEmp != undefined) {
            comboboxEmp.value('');
            comboboxEmp.destroy();
        }
        empressCommonHelper.GetDepartmentByCompanyId(companyId, "cmbDepartmentNameDetails");
        empressCommonHelper.GenerateEmployeeByCompanyId(companyId, branchId, 0, "cmbEmployee");



    },

    changeDepartmentName: function () {

        var companyData = $("#cmbCompanyNameDetails").data("kendoComboBox");
        var companyId = companyData.value();
        var companyName = companyData.text();

        var comboboxbranch = $("#cmbBranchDetails").data("kendoComboBox");
        var branchId = comboboxbranch.value();
        var branchName = comboboxbranch.text();

        var comboboxDep = $("#cmbDepartmentNameDetails").data("kendoComboBox");
        var departmentId = comboboxDep.value();
        var departmentName = comboboxDep.text();

        var comboboxEmp = $("#cmbEmployee").data("kendoComboBox");

        if (departmentId == departmentName) {
            if (comboboxEmp != undefined) {
                comboboxEmp.value('');
                comboboxEmp.destroy();
            }
            empressCommonHelper.GenerateEmployeeByCompanyId(companyId, branchId, 0, "cmbEmployee");
            return false;
        }


        if (comboboxEmp != undefined) {
            comboboxEmp.value('');
            comboboxEmp.destroy();
        }
        empressCommonHelper.GenerateEmployeeByCompanyId(companyId, branchId, departmentId, "cmbEmployee");



    },

    clearAuditTrailsReport: function () {

        var cmbMotherCompany = $("#cmbCompanyNameDetails").data("kendoComboBox");
        cmbMotherCompany.value('');

        var cmbBranch = $("#cmbBranchDetails").data("kendoComboBox");
        cmbBranch.value('');

        var cmbDepartment = $("#cmbDepartmentNameDetails").data("kendoComboBox");
        cmbDepartment.value('');

        var cmbEmployee = $("#cmbEmployee").data("kendoComboBox");
        cmbEmployee.value('');

        var cmbEmployeeType = $("#cmbEmployeeType").data("kendoComboBox");
        cmbEmployeeType.value('');


    },

    validateEmploymentInformation: function () {
        var data = [];
        var validator = $("#divLeaveReport").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {

            var emtypeId = $("#cmbEmployeeType").val();
            var cmbEmployeeType = $("#cmbEmployeeType").data("kendoComboBox");
            var empName = cmbEmployeeType.text();
            if (emtypeId == empName) {
                status.text("Oops! Employee Type is invalid.").addClass("invalid");
                cmbEmployeeType.value('');
                return false;
            }

            var companyId = $("#cmbCompanyNameDetails").val();
            var comboboxforCompany = $("#cmbCompanyNameDetails").data("kendoComboBox");
            var companyName = comboboxforCompany.text();
            if (companyId == companyName) {
                status.text("Oops! Company Name is invalid.").addClass("invalid");
                comboboxforCompany.value('');
                return false;
            }

            var branchId = $("#cmbBranchDetails").val();
            var comboboxforBranch = $("#cmbBranchDetails").data("kendoComboBox");
            var branchName = comboboxforBranch.text();
            if (branchId == branchName) {
                status.text("Oops! Branch Name is invalid.").addClass("invalid");
                comboboxforBranch.value('');
                return false;
            }

            var departmentId = $("#cmbDepartmentNameDetails").val();
            var comboboxforDepartment = $("#cmbDepartmentNameDetails").data("kendoComboBox");
            var departmentName = comboboxforDepartment.text();
            if (departmentId == departmentName) {
                status.text("Oops! Department Name is invalid.").addClass("invalid");
                comboboxforDepartment.value('');
                return false;
            }

            var reportId = $("#cmbReportTo").val();
            var comboboxforReport = $("#cmbReportTo").data("kendoComboBox");
            var reportName = comboboxforReport.text();
            if (reportId == reportName) {
                status.text("Oops! Report Name is invalid.").addClass("invalid");
                comboboxforReport.value('');
                return false;
            }
            var shiftId = $("#cmbShift").val();
            var comboboxforShift = $("#cmbShift").data("kendoComboBox");
            var shiftName = comboboxforShift.text();
            if (shiftId == shiftName) {
                status.text("Oops! Shift Name is invalid.").addClass("invalid");
                comboboxforShift.value('');
                return false;
            }

            var designationId = $("#cmbCurrentPossition").val();
            var designationCombo = $("#cmbCurrentPossition").data("kendoComboBox");
            var designationName = designationCombo.text();
            if (designationId == designationName && designationId != "") {
                status.text("Oops! Designation Name is invalid.").addClass("invalid");
                designationCombo.value('');
                return false;
            }

            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },

    CreateAuditTrailsReportParamObject: function () {
        var objDataSource = new Object();
        objDataSource.CompanyId = $("#cmbCompanyNameDetails").val();
        objDataSource.BranchId = $("#cmbBranchDetails").val();
        objDataSource.DepartmentId = $("#cmbDepartmentNameDetails").val();
        objDataSource.EmployeeId = $("#cmbEmployee").val();

        objDataSource.fromDate = $("#txtFromDate").val();
        objDataSource.todate = $("#txtToDate").val();

        var companyData = $("#cmbCompanyNameDetails").data("kendoComboBox");
        var companyName = companyData.text();
        objDataSource.CompanyName = companyName;

        return objDataSource;


    },
};