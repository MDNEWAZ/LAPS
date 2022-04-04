$(document).ready(function () {

    branchDetailsHelper.populateCompany();
    //branchDetailsHelper.branchCodeFieldEnableOrDisable();
    branchSummaryHelper.GenerateMotherCompanyCombo();
    branchSummaryHelper.CompanyIndexChangeEvent();
    branchSummaryHelper.clickEventForEditBranch();
    
   // $('#txtBranchSMSMobileNumber').mask('01999999999');

});