var CommisionReportDetails = {

    CreteDateTime: function () {
        $("#txtFromDateTime").kendoDatePicker({
            animation: false
        });
        $("#txtToDateTime").kendoDatePicker({
            animation: false
        });
    },
    ShowCommReport: function (btnshowreport) {
        $("#" + btnshowreport).click(function () {
            var reportParam = new Object();
            reportParam.StartDate = $("#txtFromDateTime").val();
            reportParam.EndDate = $("#txtToDateTime").val();
            var jsonParam = "param:" + JSON.stringify(reportParam);
            var serviceUrl = "../Report/GetCommisionReport/";
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
    PopulateRepresentatorCombo: function (cmbsalesrep) {
        var salesReps = RepresentatorSaleReportDetailsManager.GetAllRepresentators();
        $("#" + cmbsalesrep).kendoComboBox({
            placeholder: "Select Sales Rep",
            dataTextField: "SalesRepCode",
            dataValueField: "SalesRepId",
            dataSource: salesReps,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem(cmbsalesrep, true);
                param.SalesRepId = this.value();
                param.SaleRep = this.text();
            }

        });
    }
};

var RepresentatorSaleReportDetailsManager = {
    GetAllRepresentators: function () {
        var serviceUrl = "../../Report/GetAllRepresentatorCombo/";
        AjaxManager.GetJsonResult(serviceUrl, "", false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            data = jsonData;
            var obj = new Object();
            var obj1 = new Object();
            obj.SalesRepCode = "All";
            obj.SalesRepId = 0;
            obj1.SalesRepCode = "";
            obj1.SalesRepId = -1;
            data.unshift(obj);
            data.unshift(obj1);
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return data;
    }
};
