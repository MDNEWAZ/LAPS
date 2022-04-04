

$(document).ready(function () {

    ReportParamDetailsManager.GetParamDataSource();
    ReportParamDetailsHelper.PopulateZoneCombo();
    ReportParamDetailsHelper.PopulateRegionCombo();
    ReportParamDetailsHelper.PopulateBranchCombo("cmbBranch");
    ReportParamDetailsHelper.PopulatePackageCombo("cmbPackage");
    CustomerStatusReportHelper.ShowCustomerStatusReport("btnCusStatusShowReport");
    CustomerStatusReportHelper.populateStatusComboList("cmbStatus");
    //$("#txtFromDateTime").data("kendoDatePicker").value(new Date());
    //$("#txtToDateTime").data("kendoDatePicker").value(new Date());
    CustomerStatusReportHelper.PopulateCustomerCombo("cmbCustomer");

});

var CustomerStatusReportManager = {

};


var CustomerStatusReportHelper = {

    ShowCustomerStatusReport: function (btnshowreport) {
        $("#" + btnshowreport).click(function () {
            debugger;
            var reportParamObj = CustomerStatusReportHelper.GetCustomerStatusReportObject();
            var dueObj = $("#cmbStatus").data("kendoComboBox");
            var dueObjForCustomer = dueObj.dataItem(dueObj.select());
            if (dueObjForCustomer == undefined) {
                dueObjForCustomer = null;
            }

            var jsonParam = "param:" + JSON.stringify(reportParamObj) + ",dueObjForCustomer:" + JSON.stringify(dueObjForCustomer);

            var serviceUrl = "../Report/GetCustomerStatusReport/";
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

    populateStatusComboList: function (cmbStatus) {

        var objCusRatingStatus = CustomerStatusReportHelper.GetCustomerRatingStatusDataSource();
      
        $("#" + cmbStatus).kendoComboBox({
            placeholder:"Select Status" ,
            dataTextField: "Color",
            dataValueField: "DueId",
            dataSource: objCusRatingStatus,
            filter: "contains",
            suggest: true,
            //index: 0,
            change: function () {
       
                AjaxManager.isValidItem(cmbStatus, true);
                param.StatusId = this.value();
                param.Status = this.text();
            }
            
        });
        //$("#cmbStatus").parent().css('width', "7.4em");
    },

    GetCustomerRatingStatusDataSource: function () {
        var objCusStatus = "";
        var companyId = 0;
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../Customer/GetCustomerRatingByCompanyId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objCusStatus = jsonData;
            var obj = new Object();
            var obj1 = new Object();
            var obj3 = new Object();
            var obj4 = new Object();
            obj.Color = "All";
            obj.DueId = 0;
            obj1.Color = "";
            obj1.DueId = -1;
            obj3.Color = "Released";
            obj3.DueId = -2;
            obj4.Color = "Waiting For Release";
            obj4.DueId = -3;
            objCusStatus.unshift(obj);
            objCusStatus.unshift(obj1);
            objCusStatus.unshift(obj3);
            objCusStatus.unshift(obj4);
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCusStatus;
    },

    GetCustomerStatusReportObject: function () {

        var reportParam = new Object();
        reportParam.RegionId = $("#cmbRegion").val();
        reportParam.Region = $("#cmbRegion").data("kendoComboBox").text();
        reportParam.ZoneId = $("#cmbZone").val();
        reportParam.Zone = $("#cmbZone").data("kendoComboBox").text();
        reportParam.BranchId = $("#cmbBranch").val();
        reportParam.Branch = $("#cmbBranch").data("kendoComboBox").text();
        reportParam.StartDate = $("#txtFromDateTime").val();
        reportParam.EndDate = $("#txtToDateTime").val();
        reportParam.PackageId = $("#cmbPackage").val();
        reportParam.Package = $("#cmbPackage").data("kendoComboBox").text();
        reportParam.DueId = $("#cmbStatus").val();   //Status
        reportParam.Color = $("#cmbStatus").data("kendoComboBox").text();
        reportParam.CustomerId = $("#cmbCustomer").val();
        reportParam.CustomerCode = $("#cmbCustomer").data("kendoComboBox").text();
        return reportParam;
    },


    PopulateCustomerCombo: function (cmdCustomer) {
     
        var customerData = CustomerStatusReportHelper.GetAllCustomerInfo();
        $("#" + cmdCustomer).kendoComboBox({
            placeholder: "Select Customer Code",
            dataTextField: "CustomerCode",
            dataValueField: "CustomerId",
            dataSource: customerData,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem(cmdCustomer, true);
            }

        });
    },


    GetAllCustomerInfo: function () {
        var objCust = "";
        var jsonParam = "";
        var serviceUrl = "../Customer/GetAllActiveCustomer/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objCust = jsonData;
            //var obj = new Object();
            //var obj1 = new Object();
            //obj.CustomerCode = "All";
            //obj.CustomerId = 0;
            //obj1.CustomerCode = "";
            //obj1.CustomerId = -1;
            //objCust.unshift(obj);
            //objCust.unshift(obj1);
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCust;
    },

};

