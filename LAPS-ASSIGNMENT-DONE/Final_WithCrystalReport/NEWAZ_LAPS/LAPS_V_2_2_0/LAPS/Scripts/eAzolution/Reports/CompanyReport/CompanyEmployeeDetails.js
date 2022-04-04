var CompanyEmployeeReportDetailsHelper = {
    PopulateCompanyCombo: function (cmbCompany) {
        var objBranch = empressCommonManager.GetHierarchyCompany(0);
        var obj = new Object();
        var obj1 = new Object();
        obj.CompanyName = "All";
        obj.CompanyId = 0;
        obj1.CompanyName = "";
        obj1.CompanyId = -1;
        objBranch.unshift(obj);
        objBranch.unshift(obj1);

        $("#" + cmbCompany).kendoComboBox({
            placeholder: "Select Company",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objBranch,
            filter: "contains",
            suggest: true
        });
    },
    ShowReport: function(btnshowreport) {
        $("#" + btnshowreport).click(function () {
            var reportParam = new Object();
            reportParam.Company = $("#cmbCompany").val();
            var jsonParam = "param:" + JSON.stringify(reportParam);
            var serviceUrl = "../Report/CompanyWiseEmployeeDetails/";
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