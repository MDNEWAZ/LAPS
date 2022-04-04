
var pendingCollectionManager = {
    GeneratePendingTable: function () {
        $("#pendinlectionsTable").kendoGrid({
            //dataS;ource:[],
            dataSource: pendingCollectionManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: pendingCollecttonHellper.GenerateColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    gridDataSource: function () {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Dashboard/GetAllPendingCollections',
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    },
    getPenddingCollectionData: function (companyId, branchId, fromDate, toDate) {
        var pendingCollectionData = "";
        var jsonParam = '';
        var serviceUrl = "../Dashboard/GetAllPendingCollectionsForDashBoard/?companyId=" + companyId + "&branchId=" + branchId + "&fromDate=" + fromDate + "&toDate=" + toDate;
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);


        function onSuccess(jsonData) {
            pendingCollectionData = jsonData;
        }

        function onFailed(error) {

        }

        return pendingCollectionData;
    },
    getMonthWiseCollectionData: function (companyId, branchId, fromDate, toDate) {
        var pendingCollectionData = "";
        var jsonParam = '';
        var serviceUrl = "../Dashboard/GetMonthWiseCollectionData/?companyId=" + companyId + "&branchId=" + branchId + "&fromDate=" + fromDate + "&toDate=" + toDate;
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);


        function onSuccess(jsonData) {
            pendingCollectionData = jsonData;
        }

        function onFailed(error) {

        }

        return pendingCollectionData;
    },
    getMonthWiseSalesData: function (companyId, branchId, fromDate, toDate) {
        var pendingCollectionData = "";
        var jsonParam = '';
        var serviceUrl = "../Dashboard/GetMonthWiseSalesData/?companyId=" + companyId + "&branchId=" + branchId + "&fromDate=" + fromDate + "&toDate=" + toDate;
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);


        function onSuccess(jsonData) {
            pendingCollectionData = jsonData;
        }

        function onFailed(error) {

        }

        return pendingCollectionData;
    },
};
var pendingCollecttonHellper = {
    GenerateColumns: function() {
        return columns = [
            { field: "ACustomer.Name", title: "Name", width: 100 },
            { field: "ACustomer.Address", title: "Address", width: 100 },
            { field: "ACustomer.Phone", title: "Mobile No.", width: 100 },
            {
                field: "AInst" +
                    "allment.Number",
                title: "No of Install.",
                width: 100
            },
            //{ field: "Status", title: "Status", width: 100, template: "#=Status==1?'Paid':'Unpaid'#" },
            { field: "Edit", title: "Edit Interest", filterable: false, width: 60, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="interestSummaryHelper.clickEventForEditButton()"/>', sortable: false }
        ];
    },
    pendingCollectionChart: function (data) {
        //var data = pendingCollectionManager.getPenddingCollectionData();

        var chart = AmCharts.makeChart("table_div", {
            "type": "serial",
            //"pathToImages": "http://cdn.amcharts.com/lib/3/images/",
            "categoryField": "WarantyStartMonth",
            "rotate": false,
            "startDuration": 1,

            "categoryAxis": {
                "gridPosition": "start",
                "reversed": true,
                "position": "left",
                "labelRotation": 30
            },
            "legend": {
                "useGraphSettings": true,
                "markerSize": 12,
                "valueWidth": 0,
                "verticalGap": 0,
                "position": "top",
            },
            "trendLines": [],
            "graphs": [
                {
                    "balloonText": "Sales:[[value]]",
                    "fillAlphas": 0.8,
                    "fillColors": "#FF0F00",
                    "id": "AmGraph-1",
                    "lineAlpha": 0.2,
                    "title": "Total Sales",
                    "type": "column",
                    "valueField": "SalesPrice",
                    "color": "#165C04"
                },
                {
                    "balloonText": "Down Payment Receive Amount:[[value]]",
                    "fillAlphas": 0.8,
                    "fillColors": "#FCD202",
                    "id": "AmGraph-3",
                    "lineAlpha": 0.2,
                    "title": "Total DP Collection",
                    "type": "column",
                    "valueField": "DownPaymentReceiveAmount",

                    "color": "#dc143c"
                },
                 {
                     "balloonText": "Installment Receive Amount:[[value]]",
                     "fillAlphas": 0.8,
                     "fillColors": "#04D215",
                     "id": "AmGraph-4",
                     "lineAlpha": 0.2,
                     "title": "Total Installment Collection",
                     "type": "column",
                     "valueField": "InstallmentReceiveAmount",

                     "color": "#dc143c"
                 },
                {
                    "balloonText": "Outstanding:[[value]]",
                    "fillAlphas": 0.8,
                    "fillColors": "#0D52D1",
                    "id": "AmGraph-2",
                    "lineAlpha": 0.2,
                    "title": "Total Outstanding",
                    "type": "column",
                    "valueField": "OutStanding",
                    
                    "color": "#dc143c"
                },
                
            ],
            "depth3D": 20,
            "angle": 30,
            "guides": [],
            "valueAxes": [
                {
                    "id": "ValueAxis-1",
                    "position": "top",
                    "axisAlpha": 0
                }
            ],
            "allLabels": [],
            "amExport": {
                "right": 20,
                "top": 20
            },
            "balloon": {},
            "titles": [],
            "dataProvider": data
        });
    },
};