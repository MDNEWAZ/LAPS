$(document).ready(function () {
    designationSummaryHelper.GenerateDesignationGrid(0);
    designationDetailsHelper.LoadCmbStatus();
    designationDetailsHelper.populateCompany();
    designationSummaryHelper.GenerateMotherCompanyCombo();

    empressCommonHelper.GetDepartmentByCompanyId(0, "cmbDepartmentName");
    $("#cmbCompanyName").change(function () { designationDetailsHelper.changeCompanyName(); });
    
});