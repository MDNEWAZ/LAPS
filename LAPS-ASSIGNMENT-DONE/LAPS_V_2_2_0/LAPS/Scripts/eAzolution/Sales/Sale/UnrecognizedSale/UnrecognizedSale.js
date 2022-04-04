var gbIsViewer;
$(document).ready(function () {
    if (CurrentUser != null) { gbIsViewer = CurrentUser.IsViewer; }
    if (gbIsViewer == 1) { UnrecognizedSaleHelper.HideAllOperationalButton(); }
    
    $("#popupUnrecognizedSaleDetails").kendoWindow({
        title: "Unrecognized Sale Details",
        resizeable: false,
        width: "70%",
        actions: ["Pin", "Refresh", "Maximize", "Close"],
        modal: true,
        visible: false,
    });
    UnrecognizedSaleSummaryManager.InitUnrecognizedSaleSummary();
    UnrecognizedSaleDetailsManager.InitUnrecognizedSaleDetails();

    $("#btnClose").click(function() {
        $("#popupUnrecognizedSaleDetails").data("kendoWindow").close();
    });

});

var UnrecognizedSaleHelper= {
    HideAllOperationalButton: function () {
        $("#btnSubmit").hide();
    }
}