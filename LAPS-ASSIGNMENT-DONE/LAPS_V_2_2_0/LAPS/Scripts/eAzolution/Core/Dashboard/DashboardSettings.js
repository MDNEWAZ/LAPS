var currentUserlevel = [];

var DashboardSettingsManager = {
    
    gridDataSource: function (invoice) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Dashboard/GetTenRedCustomer/?invoice=' + invoice,
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
    changeSessionOnBranchChange: function (companyId,branchId) {
          //var objAccessControl = accessControlDetailsHelper.CreateAccessControlForSaveData();
          //objAccessControl = JSON.stringify(objAccessControl).replace(/&/g, "^");
          var jsonParam = 'companyId=' + companyId + '&branchId=' + branchId;
          var serviceUrl = "../Dashboard/ChangeSessionOnBranchChange/";
          AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
      
      function onSuccess(jsonData) {

          if (jsonData == "Success") {
          }
          else {
              AjaxManager.MsgBox('error', 'center', 'Failed', jsonData,
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
          }
      }

      function onFailed(error) {
          AjaxManager.MsgBox('error', 'center', 'Failed', error.statusText,
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();
                           }
                       }]);
      }
    },
    
    userLevel: function () {
        var isRootLevelUser = "";
        var jsonParam = "";
        var serviceUrl = "../Dashboard/CheckIsRootLevelAdmin/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            isRootLevelUser = jsonData;
        }
        function onFailed(error) {
        }
        return isRootLevelUser;
    },
    getStockGraphData: function (companyId) {
      
        var stockData = "";
        var jsonParam = 'companyId='+companyId;
        var serviceUrl = "../Dashboard/GetSaleStockData";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            stockData = jsonData;
        }
        function onFailed(error) {
        }
        return stockData;
    },
    
    getReplacementStockGraphData: function (companyId) {
        var stockData = "";
        var jsonParam = 'companyId=' + companyId;
        var serviceUrl = "../Dashboard/GetReplacementStockData";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            stockData = jsonData;
        }
        function onFailed(error) {
        }
        return stockData;
    },
};

var DashboardSettingsHelper = {
    checkUserLevel: function () {
        currentUserlevel = [];
        
        var userLevel = DashboardSettingsManager.userLevel();
        currentUserlevel = userLevel;
        if (userLevel == 4) {//Super User
            $("#LeftDivDashboard").addClass("LeftDivDashboard");
            $("#MonthWiseCharts").show();
            $("#divDateRange").show();
            $("#divCompanyAndBranchFilter").show();
            $("#divPastDueCollection").show();
            $("#divMonthlyBusinessTrend").show();
            $("#divCustomerRating").show();
            $("#RightDivDashboard").show();
            DashboardSettingsHelper.showMonthWiseCharts(0, 0, '', '');//if ths will company user then company id will set as perameter in controller

            //$("#divReleaseLicenseGenerate").show();
            //releaseLicenseHelper.GenerateReleaseLicenseGrid();
            //releaseLicenseHelper.releaseLicenseGridDataSet(0, 0);//if ths will company user then company id will set as perameter in controller
        }
        else if (userLevel == 22) {//Company MD
            $("#LeftDivDashboard").addClass("LeftDivDashboard");
            $("#MonthWiseCharts").show();
            DashboardSettingsHelper.showMonthWiseCharts(0, 0, '', '');//if ths will company user then company id will set as perameter in controller
            $("#divDateRange").show();
            $("#divCompanyAndBranchFilter").show();
           // $("#divPastDueCollection").show();
            $("#divMonthlyBusinessTrend").show();
            //$("#divCustomerRating").show();
            $("#RightDivDashboard").show();
            
        }
        else if (userLevel == 23) {//HO
            //$("#LeftDivDashboard").addClass("LeftDivDashboard");
            //$("#MonthWiseCharts").show();
            //DashboardSettingsHelper.showMonthWiseCharts(0, 0, '', '');//if ths will company user then company id will set as perameter in controller
            //$("#divDateRange").show();
            //$("#divCompanyAndBranchFilter").show();
            //$("#divPastDueCollection").show();
            //$("#divMonthlyBusinessTrend").show();
           //$("#divCustomerRating").show();
            //$("#RightDivDashboard").show();
            
        }
        else if (userLevel == 24) {//Region, Division, Zone
            $("#LeftDivDashboard").removeClass("LeftDivDashboard").addClass("fullDivDashboard");
            //$("#divStockGraph").show();
            //$("#LeftDivDashboard").addClass("LeftDivDashboard");
            //$("#MonthWiseCharts").show();
            //DashboardSettingsHelper.showMonthWiseCharts(0, 0, '', '');//if ths will company user then company id will set as perameter in controller
            //$("#divDateRange").show();
            $("#divCompanyAndBranchFilter").show();
            $("#divPastDueCollection").show();
            $("#divMonthlyBusinessTrend").show();
            $("#divCustomerRating").show();
            //$("#RightDivDashboard").show();
            
        }
        else if (userLevel == 25) {//Delivary InCharge
            $("#LeftDivDashboard").removeClass("LeftDivDashboard").addClass("fullDivDashboard");
            
            $("#divStockGraph").show();
            $("#divStockGraphSales").show();
            $("#divStockGraphReplacement").show();

            empressCommonHelper.GenerareHierarchyCompanyCombo("cmbCompany");
            
         
            DashboardSettingsHelper.CurrentStockGraphSale(0);

            DashboardSettingsHelper.CurrentStockGraphReplacement(0);
       
            $("#cmbCompany").change(function () {
                var id = $("#cmbCompany").data("kendoComboBox").value();
                var companyId = id == "" ? 0 : id;
                DashboardSettingsHelper.CurrentStockGraphSale(companyId);
                DashboardSettingsHelper.CurrentStockGraphReplacement(companyId);
            });


            //$("#LeftDivDashboard").addClass("LeftDivDashboard");
            //$("#MonthWiseCharts").show();
            //DashboardSettingsHelper.showMonthWiseCharts(0, 0, '', '');//if ths will company user then company id will set as perameter in controller
            //$("#divDateRange").show();
            //$("#divCompanyAndBranchFilter").show();
            //$("#divPastDueCollection").show();
            //$("#divMonthlyBusinessTrend").show();
            //$("#divCustomerRating").show();
            //$("#RightDivDashboard").show();

        }
       
        else if (userLevel == 26) {//Data Entry Operator
            
            $("#LeftDivDashboard").addClass("LeftDivDashboard");
            $("#LeftDivDashboard").removeClass("LeftDivDashboard").addClass("fullDivDashboard");
            
            //$("#menu").hide();
            //$("#MonthWiseCharts").show();
            //DashboardSettingsHelper.showMonthWiseCharts(0, 0, '', '');//if ths will company user then company id will set as perameter in controller
            //$("#divDateRange").show();
            $("#divCompanyAndBranchFilter").show();
            //$("#divPastDueCollection").show();
            //$("#divMonthlyBusinessTrend").show();
            //$("#divCustomerRating").show();
            //$("#RightDivDashboard").show();
            //$("#divStockStatus").show();
            //$("#staticMenuForDataEntryOperator").show();

        }
        else if (userLevel == 27) {//HO Accounts
            $("#LeftDivDashboard").addClass("LeftDivDashboard");
            $("#MonthWiseCharts").show();
            DashboardSettingsHelper.showMonthWiseCharts(0, 0, '', '');//if ths will company user then company id will set as perameter in controller
            $("#divDateRange").show();
            $("#divCompanyAndBranchFilter").show();
            //$("#divTabularFormateOfRating").show();
            //$("#divPastDueCollection").show();
            $("#divMonthlyBusinessTrend").show();
            
            $("#divMonthlyBusinessTrend").show();
            $("#divCustomerRating").show();
            //$("#RightDivDashboard").show();
            //$("#divStockStatus").show();
            

        }
        else {
            
          // =========== previous code ============
            //$("#divReleaseLicenseGenerate").hide();
            $("#divCompanyAndBranchFilter").hide();
            $("#RightDivDashboard").hide();
            $("#LeftDivDashboard").removeClass("LeftDivDashboard").addClass("fullDivDashboard");
            

            //============ new ============
            //$("#LeftDivDashboard").addClass("LeftDivDashboard");
            //$("#MonthWiseCharts").show();
            //DashboardSettingsHelper.showMonthWiseCharts(0, 0, '', '');//if ths will company user then company id will set as perameter in controller
            //$("#divDateRange").show();
            //$("#divCompanyAndBranchFilter").show();
            //$("#divPastDueCollection").show();
            //$("#divMonthlyBusinessTrend").show();
            //$("#divCustomerRating").show();
            //$("#RightDivDashboard").show();
            //$("#divStockStatus").show();

        }
    },



    GenerateRedCustomer: function () {

        $("#topTenRedCustomerGrid").kendoGrid({
            dataSource: [],
            //dataSource: pendingRedCustomerManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: DashboardSettingsHelper.GenerateColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    GenerateColumns: function () {
        return columns = [
            //{ field: "Name", title: "Name", width: 100 },
            //{ field: "Address", title: "Address", width: 100 },
            //{ field: "Phone", title: "Mobile No", width: 100 },
            //{ field: "OutStandingAmount", title: "Total Outstanding", width: 110 },
            //{ field: "DueAmountTillDate", title: "Previous Dues", width: 80 },
            //{ field: "TotalDuePercentTillDate", title: "Due (%)", width: 70 },
            ////{ field: "Status", title: "Status", width: 100, template: "#=Status==1?'Paid':'Unpaid'#" },
            //{ field: "Edit", title: "Rating", width: 80, template: "#=DashboardSettingsHelper.RatingSetforCustomer(data)#" }
            
            { field: "CustomerCode", title: "Customer ID", width: 100 },
            { field: "Name", title: "Name", width: 100 },
            { field: "Address", title: "Address", width: 100, hidden: true },
            { field: "Phone2", title: "Mobile No", width: 100 },
            { field: "BranchCode", title: "Branch Code", width: 100 },
            { field: "ProductName", title: "Package Name", width: 100 },
            { field: "Model", title: "Model", width: 100, hidden: true },
            { field: "ProductTypeName", title: "Type", width: 80 },

            { field: "DueAmountTillDate", title: "Past Dues", width: 80 },
            { field: "OutStandingAmount", title: "Balance", width: 110, hidden: false },
            { field: "TotalDuePercentTillDate", title: "Due (%)", width: 70, hidden: true },
            //{ field: "Status", title: "Status", width: 100, template: "#=Status==1?'Paid':'Unpaid'#" },
            { field: "Edit", title: "Status", width: 80, template: "#=DueCollectionHelper.RatingSetforCustomer(data)#" }
        ];
    },
    RatingSetforCustomer: function (data) {
 
        return CustomerRatingHellper.RatingSetforCustomer(data);
    },
    CustomerDueGridDataSet: function (invoice,companyId,branchId) {
      
        var data = pendingRedCustomerManager.gridDataSource(invoice, companyId, branchId);
        $("#topTenRedCustomerGrid").data().kendoGrid.setDataSource(data);
    },
    generateCharts: function (companyId, branchId,fromDate,toDate) {
        
        var data = pendingCollectionManager.getPenddingCollectionData(companyId, branchId, fromDate, toDate);
        pendingCollecttonHellper.pendingCollectionChart(data);
        
        //var monthWiseCollectionData = pendingCollectionManager.getMonthWiseCollectionData(companyId, branchId);
        //var monthWiseSalesData = pendingCollectionManager.getMonthWiseSalesData(companyId, branchId);
        
        
        //DashboardSettingsHelper.monthWiseCollectionAndSalesChart(monthWiseCollectionData, "monthWiseCollection", "OutStanding", "Collection");
        
        //DashboardSettingsHelper.monthWiseCollectionAndSalesChart(monthWiseSalesData, "monthWiseSales", "OutStanding", "Sales");
    },
    showMonthWiseCharts: function (companyId, branchId, fromDate, toDate) {
        var monthWiseCollectionData = pendingCollectionManager.getMonthWiseCollectionData(companyId, branchId, fromDate, toDate);
        var monthWiseSalesData = pendingCollectionManager.getMonthWiseSalesData(companyId, branchId, fromDate, toDate);


        DashboardSettingsHelper.monthWiseCollectionChart(monthWiseCollectionData, "monthWiseCollection", "OutStanding", "Collection");

        DashboardSettingsHelper.monthWiseSalesChart(monthWiseSalesData, "monthWiseSales", "OutStanding", "Sales");
    },
    monthWiseSalesChart: function (data) {
        var chart = AmCharts.makeChart("monthWiseSales", {
            "theme": "none",
            "type": "serial",
            "startDuration": 2,
            "dataProvider": data,
            "graphs": [{
                "balloonText": "Sales: <b>[[value]]</b>",
                "colors": ["#FF6600", "#04D215", "#FF9E01", "#FCD202", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000", "#57032A", "#CA9726", "#990000", "#4B0C25"],
                "fillAlphas": 1,
                "fillColors": "#FF0F00",
                "lineAlpha": 0.1,
                "type": "column",
                "valueField": "OutStanding"
            }],
            "depth3D": 20,
            "angle": 30,
            "chartCursor": {
                "categoryBalloonEnabled": false,
                "cursorAlpha": 0,
                "zoomable": false
            },
            "categoryField": "WarantyStartMonth",
            "categoryAxis": {
                "gridPosition": "start",
                "labelRotation": 30
            },
            "exportConfig": {
                "menuTop": "20px",
                "menuRight": "20px",
                "menuItems": [{
                    "icon": '/lib/3/images/export.png',
                    "format": 'png'
                }]
            }
        });
        jQuery('.chart-input').off().on('input change', function () {
            var property = jQuery(this).data('property');
            var target = chart;
            chart.startDuration = 0;

            if (property == 'topRadius') {
                target = chart.graphs[0];
                if (this.value == 0) {
                    this.value = undefined;
                }
            }

            target[property] = this.value;
            chart.validateNow();
        });
    },
    monthWiseCollectionChart: function (data) {
            var chart = AmCharts.makeChart("monthWiseCollection", {
                "theme": "none",
                "type": "serial",
                "startDuration": 2,
                "dataProvider": data,
                "valueAxes": [{
                    "position": "left",
                    "axisAlpha": 0,
                    "gridAlpha": 0
                }],
                "graphs": [{
                    "balloonText": "Collection: <b>[[value]]</b>",
                    //"colorField": "color",
                    "colors": ["#04D215", "#FF6600", "#FF9E01", "#FCD202", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000", "#57032A", "#CA9726", "#990000", "#4B0C25"],
                    "fillAlphas": 0.85,
                    "fillColors": "#04D215",
                    "lineAlpha": 0.1,
                    "type": "column",
                    "topRadius": 1,
                    "valueField": "OutStanding"
                }],
                "depth3D": 40,
                "angle": 30,
                "chartCursor": {
                    "categoryBalloonEnabled": false,
                    "cursorAlpha": 0,
                    "zoomable": false
                },
                "categoryField": "WarantyStartMonth",
                "categoryAxis": {
                    "gridPosition": "start",
                     "labelRotation": 30,
                    "axisAlpha": 0,
                    "gridAlpha": 0      
                },
                "exportConfig": {
                    "menuTop": "20px",
                    "menuRight": "20px",
                    "menuItems": [{
                        "icon": '/lib/3/images/export.png',
                        "format": 'png'
                    }]
                }
            }, 0);

            jQuery('.chart-input').off().on('input change', function() {
                var property = jQuery(this).data('property');
                var target = chart;
                chart.startDuration = 0;

                if (property == 'topRadius') {
                    target = chart.graphs[0];
                }

                target[property] = this.value;
                chart.validateNow();
            });
    },
    

    CurrentStockGraphSale: function (companyId) {

        var fillColor =  "#0D52D1";
        if (companyId > 0) {
            fillColor = "#754DEB";
        }
        
        var data = DashboardSettingsManager.getStockGraphData(companyId);
        var chart = AmCharts.makeChart("stockGraphSales", {
            "theme": "none",
            "type": "serial",
            "startDuration": 2,
            "dataProvider": data,
            "valueAxes": [{
                "position": "left",
                "title": "Stock"
            }],
            "graphs": [{
                "balloonText": "[[category]]: <b>[[value]]</b>",
                "colorField": "color",
                "fillAlphas": 1,
                "lineAlpha": 0.1,
                "type": "column",
                "fillColors": fillColor,
                "valueField": "StockBalanceQty"
            }],
            "depth3D": 20,
            "angle": 30,
            "chartCursor": {
                "categoryBalloonEnabled": false,
                "cursorAlpha": 0,
                "zoomable": false
            },
            "categoryField": "ModelItem",
            "categoryAxis": {
                "gridPosition": "start",
                "labelRotation": 45
            },
            "exportConfig": {
                "menuTop": "20px",
                "menuRight": "20px",
                "menuItems": [{
                    "icon": '/lib/3/images/export.png',
                    "format": 'png'
                }]
            }
        });
        jQuery('.chart-input').off().on('input change', function () {
            var property = jQuery(this).data('property');
            var target = chart;
            chart.startDuration = 0;

            if (property == 'topRadius') {
                target = chart.graphs[0];
                if (this.value == 0) {
                    this.value = undefined;
                }
            }

            target[property] = this.value;
            chart.validateNow();
        });
    },

    CurrentStockGraphReplacement: function (companyId) {
        var fillColor = "#04D215";
        if (companyId > 0) {
            fillColor = "#0D8ECF";
        }
        var data = DashboardSettingsManager.getReplacementStockGraphData(companyId);
        var chart = AmCharts.makeChart("stockGraphReplacement", {
            "theme": "none",
            "type": "serial",
            "startDuration": 2,
            "dataProvider": data,
            
            "valueAxes": [{
                "position": "left",
                "title": "Stock",
               
            }],
            "graphs": [{
                "balloonText": "[[category]]: <b>[[value]]</b>",
                "colorField": "color",
                "fillAlphas": 1,
                "lineAlpha": 0.1,
                "type": "column",
                "fillColors": fillColor,
                "valueField": "StockBalanceQty"
            }],
            "depth3D": 20,
            "angle": 30,
           
            "chartCursor": {
                "categoryBalloonEnabled": false,
                "cursorAlpha": 0,
                "zoomable": false
            },
            "categoryField": "ModelItem",
            "categoryAxis": {
                "gridPosition": "start",
                "labelRotation": 45
               
            },
            "exportConfig": {
                "menuTop": "20px",
                "menuRight": "20px",
                "menuItems": [{
                    "icon": '/lib/3/images/export.png',
                    "format": 'png'
                }]
            }
        });
        jQuery('.chart-input').off().on('input change', function () {
            var property = jQuery(this).data('property');
            var target = chart;
            chart.startDuration = 0;

            if (property == 'topRadius') {
                target = chart.graphs[0];
                if (this.value == 0) {
                    this.value = undefined;
                }
            }

            target[property] = this.value;
            chart.validateNow();
        });
    },


        //monthWiseCollectionAndSalesChart: function (data, ctlId, valueField,title) {


    //    var chart = AmCharts.makeChart(ctlId, {
    //        "type": "serial",
    //        "categoryField": "WarantyStartMonth",
    //        "rotate": false,
    //        "startDuration": 1,

    //        "categoryAxis": {
    //            "gridPosition": "start",
    //            "reversed": true,
    //            "position": "left",
    //            "labelRotation": 20
    //        },
    //        //"legend": {
    //        //    "useGraphSettings": true,
    //        //    "markerSize": 12,
    //        //    "valueWidth": 0,
    //        //    "verticalGap": 0
    //        //},
    //        "trendLines": [],
    //        "graphs": [
                
    //            {
    //                "balloonText": title+":[[value]]",
    //                "fillAlphas": 0.8,
    //                "id": "AmGraph-3",
    //                "lineAlpha": 0.2,
    //                "title": "" + title,
    //                "type": "column",
    //                "color":"red",
    //                "valueField": ""+valueField
    //            }
                
    //        ],
    //        "guides": [],
    //        "valueAxes": [
    //            {
    //                "id": "ValueAxis-1",
    //                "position": "top",
    //                "axisAlpha": 0
    //            }
    //        ],
    //        "allLabels": [],
    //        "amExport": {
    //            "right": 20,
    //            "top": 20
    //        },
    //        "balloon": {},
    //        //"titles": [],
    //        "dataProvider": data
    //    });
    //},
};