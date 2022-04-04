var PackageSaleReportDetailsHelper = {

    CreteDateTime: function () {
        $("#txtFromDateTime").kendoDatePicker({
            animation: false
        });
        $("#txtToDateTime").kendoDatePicker({
            animation: false
        });
    },
    ShowReport: function (btnshowreport) {
        $("#" + btnshowreport).click(function () {
            var reportParam = new Object();
            reportParam.StartDate = $("#txtFromDateTime").val();
            reportParam.EndDate = $("#txtToDateTime").val();
            reportParam.PackageId = $("#cmbPackage").val();
            var jsonParam = "param:" + JSON.stringify(reportParam);
            var serviceUrl = "../Report/PackageWiseSalesData/";
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
    PopulatePackageCombo: function (cmbpackage) {
        var objModel = empressCommonManager.GetProductModel();
        var obj = new Object();
        var obj1 = new Object();
        obj.Model = "All";
        obj.ModelId = 0;
        obj1.Model = "";
        obj1.ModelId = -1;
        objModel.unshift(obj);
        objModel.unshift(obj1);

        $("#" + cmbpackage).kendoComboBox({
            placeholder: "Select Model",
            dataTextField: "Model",
            dataValueField: "ModelId",
            dataSource: objModel,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem(cmbpackage, true);
                param.PackageId = this.value();
                param.Package = this.text();
            }
        });
    }
};

var PackageSaleReportDetailsManager = {
    GetAllPackages: function () {
        var serviceUrl = "../../Report/GetAllPackageCombo/";
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
