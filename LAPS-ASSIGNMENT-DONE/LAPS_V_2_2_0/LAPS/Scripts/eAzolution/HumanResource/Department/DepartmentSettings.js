$(document).ready(function () {
    departmentDetailsHelper.populateCompany();
    departmentSummaryHelper.GenerateMotherCompanyCombo();
    departmentSummaryHelper.CompanyIndexChangeEvent();
    departmentSummaryHelper.clickEventForEditDepartment();
    //departmentDetailsHelper.GetEmployeeByCompanyId(0);
});
