var EmployeesDetailsHelper = {
    ShowReport: function (btnshowreport) {
        $("#" + btnshowreport).click(function () {

            var serviceUrl = "../Report/GetEmployeeInfoSummary/";
            AjaxManager.SendJson2(serviceUrl, "", onSuccess, onFailed);
            function onSuccess(jsonData) {
                debugger;
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