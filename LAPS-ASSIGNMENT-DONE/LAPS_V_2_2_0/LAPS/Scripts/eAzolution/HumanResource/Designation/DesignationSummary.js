
var designationSummaryManager = {

    gridDataSource: function (companyId) {
        //var gridDataSource = new kendo.data.DataSource({
        //    type: "json",

        //    serverPaging: true,

        //    serverSorting: false,

        //    serverFiltering: true,

        //    allowUnsort: false,

        //    pageSize: 10,
        //    page: 1,

        //    transport: {
        //        read: {

        //            url: '../Designation/GetDesignationSummary/?CompanyId=' + companyId,

        //            type: "POST",

        //            dataType: "json",

        //            contentType: "application/json; charset=utf-8"
        //        },

        //        parameterMap: function (options) {

        //            return JSON.stringify(options);

        //        }
        //    },
        //    schema: {
        //        data: "Items", total: "TotalCount",
        //        model: {
        //            fields: {
        //                ConsumerRegDate: {
        //                    type: "date"
        //                }
        //            }
        //        }

        //    }


        //});
        //return gridDataSource;
        
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    url: '../Designation/GetDesignationSummary/?companyID=' + companyId,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../Designation/GetDesignationSummary/?companyID=' + companyId,
                    dataType: "json"
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    },
    
    GetMotherCompany: function () {
        var objCompany = "";
        var jsonParam = "";
        var serviceUrl = "../Company/GetMotherCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCompany;
    }
};

var designationSummaryHelper = {
    GenerateDesignationGrid: function (companyId) {
        //$("#gridDesignationSummary").kendoGrid({

        //    dataSource: designationSummaryManager.gridDataSource(companyId),
        //    pageable: {
        //        refresh: true,
        //        serverPaging: true,
        //        serverFiltering: true,
        //        serverSorting: true
        //        //pageSizes: true,
        //        //previousNext: true,


        //    },
        //    height: 450,
        //    filterable: true,
        //    sortable: true,
        //    columns: designationSummaryHelper.GeneratedDesignationColumns(),
        //    editable: false,
        //    navigatable: true,
        //    selectable: "row"
        
        $("#divgridDesignationSummary").kendoGrid({
            dataSource: designationSummaryManager.gridDataSource(companyId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: designationSummaryHelper.GeneratedDesignationColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });

    },

    GeneratedDesignationColumns: function () {
        return columns = [
        { filed: "DesignationId", title: "DesignationId", width: 50, hidden: true },
        { filed: "DepartmentId", title: "DepartmentId", width: 50, hidden: true },
        { field: "DesignationName", title: "Designation Name", width: 200, sortable: true },
        { field: "Status", title: "Status", width: 200, sortable: true, template: "#= (Status==1) ? 'Active' : 'Inactive' #" },
        { field: "Edit", title: "Edit", filterable: false, width: 70, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="designationSummaryHelper.clickEventForEditButton()"  />', sortable:false }
        ];
    },
    
    GenerateMotherCompanyCombo: function () {
        var objCompany = new Object();
        objCompany = designationSummaryManager.GetMotherCompany();

        $("#cmbCompanyNameForSummary").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany
        });
        
        if(CurrentUser != null) {
            var cmbComp = $("#cmbCompanyNameForSummary").data("kendoComboBox");
            cmbComp.value(CurrentUser.CompanyId);
        }
    },

    CompanyIndexChangeEvent: function () {
        var companyData = $("#cmbCompanyNameForSummary").data("kendoComboBox");
        var companyId = companyData.value();

        var companyName = companyData.text();
        if (companyId == companyName) {
            return false;
        }

        $("#divgridDesignationSummary").empty();
        $("#divgridDesignationSummary").kendoGrid();

        designationSummaryHelper.GenerateDesignationGrid(companyId);
    },
    
    //clickEventForEditDesignation: function () {
    //    $('#divgridDesignationSummary table tr').live('dblclick', function () {
    //        var entityGrid = $("#divgridDesignationSummary").data("kendoGrid");

    //        var selectedItem = entityGrid.dataItem(entityGrid.select());

    //        designationDetailsHelper.populateDesignationDetails(selectedItem);
    //    });
    //},
    
    clickEventForEditButton:function () {
        var entityGrid = $("#divgridDesignationSummary").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());

        designationDetailsHelper.populateDesignationDetails(selectedItem);
    }
};