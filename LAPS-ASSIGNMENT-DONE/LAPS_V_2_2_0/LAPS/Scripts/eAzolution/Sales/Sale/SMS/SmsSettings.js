
$(document).ready(function () {

    $("#smsPopup").kendoWindow({
        title: "Edit Unrecognized SMS",
        resizeable: false,
        width: "50%",
        actions: ["Pin", "Refresh", "Maximize", "Close"],
        modal: true,
        visible: false,
    });

    SmsSummaryHelper.initSmsDate();
    SmsSummaryHelper.GenerateSmsSummary();

    $("#btnSearchSalesRequestSmsInfo").click(function() {
        SmsSummaryHelper.GenerateSmsSummary();
    });


});