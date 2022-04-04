$(document).ready(function () {
    branchDetailsHelper.populateCompany();
    branchSummaryHelper.GenerateMotherCompanyCombo();
    branchSummaryHelper.CompanyIndexChangeEvent();
    branchSummaryHelper.clickEventForEditBranch();
    //branchSummaryHelper.initForSummaryGrid();

});