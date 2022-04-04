$(document).ready(function () {
    creditPeriodSummaryHelper.GenerateCreditPeriodGrid();
    $("#btnSave").click(function() {
        creditPeriodManager.SaveCredidPeriodInformation();
    });
    $("#btnClearAll").click(function () {
        creditPeriodHelper.clearForm();
    });
});