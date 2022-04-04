var StockDetailsHelper = {
    ShowStockReport: function (btnshowreport) {
        $("#" + btnshowreport).click(function () {
            var reportParam = new Object();
            reportParam.RegionId = $("#cmbRegion").val();
            reportParam.ZoneId = $("#cmbZone").val();
            reportParam.BranchId = $("#cmbBranch").val();
            reportParam.StartDate = $("#txtFromDateTime").val();
            reportParam.EndDate = $("#txtToDateTime").val();
            reportParam.StockType = $("#cmbStockType").val();
            var jsonParam = "param:" + JSON.stringify(reportParam);

            var serviceUrl = "../Report/GetStockDetails/";
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
    }
};