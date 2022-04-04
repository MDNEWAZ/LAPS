/// <reference path="DueSummary.js" />
/// <reference path="DueDetails.js" />
/// <reference path="../../Common/common.js" />

$(document).ready(function () {
    dueSummaryManager.InitDueDetails();
});

var dueManager = {
    GeRowDataOfDueGrid: function () {
        //var entityGrid = $("#gridDue").data("kendoGrid");
        //var selectedItem = entityGrid.dataItem(entityGrid.select());
        //DueHelper.FillDueDetailsInForm(selectedItem);
    },
};

var dueHelper = {
    FillDueDetailsInForm: function (objDue) {
        //DueDetailsHelper.clearDueForm()
        //$('#hdnDueCompanyId').val(objDue.ACompany.CompanyId);
        //$("#txtDueCompanyName").val(objDue.ACompany.CompanyName);
        //$("#txtDues").val(objDue.Dues);
        //if (objDue.Status==1) {
        //    $("#chkIsActiveDue").prop('checked', 'checked');
        //} else {
        //    $("#chkIsActiveDue").removeProp('checked', 'checked');
        //};
       
    }
};


