/// <reference path="GroupSummary.js" />

$(document).ready(function () {
    groupDetailsHelper.createTab();
   // groupSummaryManager.GenerateGroupGrid();
    groupInfoHelper.GenerateModuleForGroupInfo();
    groupInfoHelper.GenerateMotherCompanyCombo();
    groupSummaryHelper.GenerateMotherCompanyCombo();
    groupSummaryHelper.clickEventForEditGroup();
    //menuPermisionHelper.clickFormenuCheckbox();
    groupSummaryHelper.CompanyIndexChangeEvent();
    
   
});

var groupSettingsManager = {};

var groupSettingsHelper = { };