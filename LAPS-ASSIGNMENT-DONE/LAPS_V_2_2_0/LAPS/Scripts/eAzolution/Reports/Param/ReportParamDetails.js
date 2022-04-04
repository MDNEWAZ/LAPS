var data = "";
//var zoneId = -1;
//var regionId = -1;
//var branchId = -1;
//var packageId = -1;
//var repId = -1;
var param = new Object();
var ReportParamDetailsHelper = {
    PopulateZoneCombo: function () {
        var obj = new Object();
        var obj1 = new Object();
        obj.CompanyName = "All";
        obj.CompanyId = 0;
        obj1.CompanyName = "";
        obj1.CompanyId = -1;
        data.Zone.unshift(obj);
        data.Zone.unshift(obj1);
        $("#cmbZone").kendoComboBox({
            placeholder: "Select Zone",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: data.Zone,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem("cmbZone", true);
                param.zoneId = this.value();
                param.Zone =this.text();
            }
        });
    },
    PopulateRegionCombo: function () {
        var obj = new Object();
        var obj1 = new Object();
        obj.CompanyName = "All";
        obj.CompanyId = 0;
        obj1.CompanyName = "";
        obj1.CompanyId = -1;
        data.Region.unshift(obj);
        data.Region.unshift(obj1);
        $("#cmbRegion").kendoComboBox({
            placeholder: "Select Region",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: data.Region,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem("cmbRegion", true);
                param.RegionId = this.value();
                param.Region = this.text();
            }
        });
    },
    PopulateSaleRepType: function (cmbsalesrep) {
        var salesReps = ReportParamDetailsManager.GetAllRepresentatorCombo();
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
                param.SaleRep =this.text();
            }

        });
    },
    PopulateBranchCombo: function (cmbbranch) {
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
            suggest: true,
            change: function () {
                AjaxManager.isValidItem(cmbbranch, true);
                param.BranchId = this.value();
                param.Branch =this.text();
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
                param.Package =this.text();
            }
        });
    },
    ShowReport: function (btnshowreport) {
        $("#" + btnshowreport).click(function () {
            param.StartDate = $("#txtFromDateTime").val();
            param.EndDate = $("#txtToDateTime").val();
           // param.ReportType = $('input[name=reportType]:checked').val();
            //if (param.ReportType == undefined) {
            //    AjaxManager.MsgBox('error', 'center', 'Error', 'Please Select Report Type',
            //      [{
            //          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
            //              $noty.close();
            //          }
            //      }]);
            //}else {}
            var reportParam = JSON.stringify(param);//"reportType:" + reportType + ",zoneId:" + zoneId + ",regionId:" + regionId + ",branchId:" + branchId + ",packageId:" + packageId + ",repId:" + repId + ",fromDate:" + from + ",toDate:" + to;
            var jsonParam = "param:" + reportParam;
            var serviceUrl = "../../Report/ShowSaleOrCollectionReport/";
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
    CreteDateTime: function () {
        $("#txtFromDateTime").kendoDatePicker({
            animation: false
        });
        $("#txtToDateTime").kendoDatePicker({
            animation: false
        });
    },
    PopulateStockType: function (cmbstocktype) {
        var types = [{ Name: "", Id: 0 }, { Name: "Company", Id: 1 }, { Name: "Branch", Id: 2 }];
        $("#" + cmbstocktype).kendoComboBox({
            placeholder: "Select Stock Type",
            dataTextField: "Name",
            dataValueField: "Id",
            dataSource: types,
            filter: "contains",
            suggest: true
        });
    }
};
var ReportParamDetailsManager = {

    GetParamDataSource: function () {
        var serviceUrl = "../../Report/GetParamDataSource/";
        AjaxManager.GetJsonResult(serviceUrl, "", false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            data = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return data;
    },
    GetAllRepresentatorCombo: function () {
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
}