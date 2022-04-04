var gbIsViewer;
$(document).ready(function () {
    if (CurrentUser != null) {
        var isViewer = CurrentUser.IsViewer;
        gbIsViewer = isViewer;
    }
    if (isViewer == 1) {
        SalesRollbackHelper.HideAllOperationalButton();
    }
    
    $("#salesRollbackPopupDiv").kendoWindow({
        title: "Sale Rollback Operation",
        resizable: false,
        modal: true,
        width: "50%",
        height:"60%",
        draggable: true,
        open: function (e) {
            this.wrapper.css({ top: 50 });
        }
    });
    
    empressCommonHelper.GenerareHierarchyCompanyCombo("cmbCompany");
    empressCommonHelper.GenerateBranchCombo(0, "cmbBranch");
    $("#cmbCompany").change(function () {
        SalesRollbackDetailHelper.ChangeEventForCompanyCombo();
    });

    SalesRollbackDetailManager.SaleRollbackDetailsInit();
    SalesRollbackSummaryHelper.PopulateSaleRollbackSummary();
    

    

});

var SalesRollbackHelper= {
    HideAllOperationalButton: function () {
        $("#btnRollback").hide();
       
    }

}