/// <reference path="UserSummary.js" />
/// <reference path="UserDetails.js" />
/// <reference path="UserInfo.js" />
/// <reference path="../../Common/EmpressCommon.js" />
/// <reference path="../../Common/OrganogramTree.js" />


$(document).ready(function () {
    empressCommonHelper.initePanelBer("ulIdentityPanelOrganogram");
    organogramTreeHelper.populateOrganogramTree();
    userInfoHelper.initiateConveyanceReport();

    //$("#cmbCompanyNameDetails").change(function () {
    //    userInfoHelper.changeCompanyName();
    //});

    //$("#cmbDepartmentNameDetails").change(function () {
    //    userInfoHelper.changeDepartmentName();
    //});
    //userInfoHelper.populateCompany();
    

    userDetailsHelper.createTab();
    userSummaryHelper.clickEventForResetPassword();
    userSummaryHelper.clickEventForEditUser();
    userSummaryHelper.GenerateMotherCompanyCombo();
   // userInfoHelper.GenerateMotherCompanyCombo();
    userSummaryHelper.CompanyIndexChangeEvent();
    // userInfoHelper.GetEmployeeByCompanyId(0);
    userUploadManager.userUpload();
    
    organogramTreeHelper.initiatTreeSerch();
    
    
});

var userSettingsManager = {};

var userSettingsHelper = {};

