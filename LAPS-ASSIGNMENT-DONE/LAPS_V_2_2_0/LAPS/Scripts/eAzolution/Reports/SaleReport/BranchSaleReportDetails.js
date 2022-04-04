var BranchSaleReportDetailsHelper = {
    PopulateBranchCombo: function(cmbbranch) {
        var objBranch = empressCommonManager.GenerateBranchCombo(0);
        var obj = new Object();
        var obj1 = new Object();
        obj.BranchName = "All";
        obj.BranchId = 0;
        obj1.BranchName = "";
        obj1.BranchId = -1;
        objBranch.unshift(obj);
        objBranch.unshift(obj1);

        $("#" + cmbbranch).kendoComboBox({
            placeholder: "Select Branch",
            dataTextField: "BranchName",
            dataValueField: "BranchId",
            dataSource: objBranch,
            filter: "contains",
            suggest: true
        });
    },
    CreteDateTime: function() {
        $("#txtFromDateTime").kendoDatePicker({
            animation: false
        });
        $("#txtToDateTime").kendoDatePicker({
            animation: false
        });
    },
    ShowReport: function(btnshowreport) {
        $("#" + btnshowreport).click(function () {
            var reportParam = new Object();
            reportParam.StartDate = $("#txtFromDateTime").val();
            reportParam.EndDate = $("#txtToDateTime").val();
            reportParam.BranchId = $("#cmbBranch").val();
            var jsonParam = "param:" + JSON.stringify(reportParam);
            var serviceUrl = "../Report/ShowBranchSaleReport/";
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
var BranchSaleReportDetailsManager = {    
    
};