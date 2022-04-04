var gbIsViewer;

$(document).ready(function () {
    if (CurrentUser != null) {
        gbIsViewer = CurrentUser.IsViewer;
    }
    if (gbIsViewer == 1) {
        ReScheduleSettingsHelper.HideAllOperationalButton();
    }
    empressCommonHelper.GenerareHierarchyCompanyCombo("cmbCompany");
    empressCommonHelper.GenerateBranchCombo(0,"cmbBranch");
    $("#cmbCompany").change(function () {
        ReScheduleDetailHelper.ChangeEventForCompanyCombo();
    });
   
    ReScheduleDetailHelper.CreateFirstPayDateCalender("txtFirstPayDate");

    $("#btnReSchedule").click(function() {
        ReScheduleDetailManager.MakeReSchedule();
    });

    ReScheduleSummaryHelper.PopulateScheduleSummary();
    $("#btnSearchImSchedule").click(function() {
        ReScheduleSummaryHelper.PopulateRescheduleSummaryGrid();
    });
    $("#txtCustomerCode").keypress(function (event) {
        if (event.keyCode == 13) {
            ReScheduleSummaryHelper.PopulateRescheduleSummaryGrid();
        }
    });

});

var ReScheduleSettingsHelper= {
    HideAllOperationalButton: function () {
        $("#btnReSchedule").hide();
     
    }
}