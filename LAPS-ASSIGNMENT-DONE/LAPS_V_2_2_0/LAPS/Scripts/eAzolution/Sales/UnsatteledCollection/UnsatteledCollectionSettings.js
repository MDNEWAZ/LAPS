$(document).ready(function () {
    UnsatteledCollectionSummaryHelper.GenerateUnsatteledCollectionGrid();
    
    $("#sattelPopupWindow").kendoWindow({
        title: "Sattel Payment",
        resizeable: false,
        width: "40%",
        actions: ["Pin", "Refresh", "Maximize", "Close"],
        modal: true,
        visible: false,
    });
    
    $("#txtPayDate").kendoDatePicker({
        format: "dd-MMM-yyyy"

    });
    CollectionDetailsHelper.GeneratePaymentTypeCombo();
    
    
    $("#txtMobileNumber").keypress(function (event) {
        if (event.keyCode == 13) {
           
        }
    });

    $("#txtReceiveAmount").kendoNumericTextBox({format:"#"});

    $("#btnSaveManualCollection").click(function () {
       
        SattelDetailsManager.SaveSetteledCollection();
    });

    $("#btnClearAll").click(function() {
        SattelDetailsHelper.ClearAllSattelDetails();
    });

});