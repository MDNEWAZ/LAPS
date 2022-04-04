// <reference path="UserSummary.js" />
// <reference path="UserDetails.js" />
// <reference path="UserInfo.js" />


$(document).ready(function () {

    createTab();
    workFlowHelper.GenerateworkFlowSummaryGrid();
    stateDetailHelper.LoadMenuCombo();
    stateDetailHelper.LoadIsCloseCombo();
    workFlowHelper.clickEventForEditworkFlow();
    actionDetailHelper.clickEventForEditAction();
    actionDetailHelper.LoadNextStateCombo(0);
    //userSummaryHelper.GenerateMotherCompanyCombo();
    //userInfoHelper.GenerateMotherCompanyCombo();
    //userSummaryHelper.CompanyIndexChangeEvent();
    ////userInfoHelper.GetEmployeeByCompanyId(0);


});


function createTab() {
    $("#tabstrip").kendoTabStrip({});
}




