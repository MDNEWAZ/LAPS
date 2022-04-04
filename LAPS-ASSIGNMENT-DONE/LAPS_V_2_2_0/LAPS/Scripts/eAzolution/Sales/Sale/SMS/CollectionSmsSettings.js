
$(document).ready(function () {
    CollectionSmsDetailsHelper.GenerateReceiveDate();
    $("#smsPopup").kendoWindow({
        title: "Edit Unrecognized Collection SMS",
        resizeable: false,
        width: "50%",
        actions: ["Pin", "Refresh", "Maximize", "Close"],
        modal: true,
        visible: false,
    });
  
    CollectionSmsSummaryHelper.GenerateCollectionSmsSummary();



    $("#smsSearch").click(function() {
        CollectionSmsSummaryHelper.GenerateCollectionSmsSummary();
    });

});