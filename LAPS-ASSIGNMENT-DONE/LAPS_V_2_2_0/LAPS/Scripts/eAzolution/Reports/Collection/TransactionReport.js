

$(document).ready(function() {

    ReportParamDetailsManager.GetParamDataSource();
    ReportParamDetailsHelper.PopulateZoneCombo();
    ReportParamDetailsHelper.PopulateRegionCombo();
    ReportParamDetailsHelper.PopulateBranchCombo("cmbBranch");
    TransactionReportHelper.ShowTransactionReport("btnTransactionShowReport");
    //$("#txtFromDateTime").data("kendoDatePicker").value(new Date());
    //$("#txtToDateTime").data("kendoDatePicker").value(new Date());
});

var TransactionReportManager = {

};


var TransactionReportHelper = {
    ShowTransactionReport: function (btnshowreport) {
        $("#" + btnshowreport).click(function () {

            var reportParam = TransactionReportHelper.GetTransactionReportObject();
            var jsonParam = "param:" + JSON.stringify(reportParam);

            var serviceUrl = "../Report/GetTransactionReport/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
            function onSuccess(jsonData) {
                if (jsonData == "Success") {
                    window.open('../Reports/LapsRepot.aspx', '_blank');
                }
            }
            function onFailed(error) {
                AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
               [{
                   addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                       $noty.close();
                   }
               }]);
            }
        });
    },

    GetTransactionReportObject: function () {
        var reportParam = new Object();
        reportParam.RegionId = $("#cmbRegion").val();
        reportParam.Region = $("#cmbRegion").data("kendoComboBox").text();
        reportParam.ZoneId = $("#cmbZone").val();
        reportParam.Region = $("#cmbZone").data("kendoComboBox").text();
        reportParam.BranchId = $("#cmbBranch").val();
        reportParam.Branch = $("#cmbBranch").data("kendoComboBox").text();
        reportParam.StartDate = $("#txtFromDateTime").val();
        reportParam.EndDate = $("#txtToDateTime").val();
        return reportParam;
    }
};

