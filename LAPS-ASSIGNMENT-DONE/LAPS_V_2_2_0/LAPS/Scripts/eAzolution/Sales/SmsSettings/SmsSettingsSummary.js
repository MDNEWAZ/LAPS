
var SmsSettingsSummaryManager = {

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
                    url: '../SmsSettings/GetAllSmsSettings/',

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
    }
};


var SmsSettingsSummaryHelper = {
    InitSmsSettingsSummary: function () {
        SmsSettingsSummaryHelper.GenerateSmsSummaryGrid();




    },

    GenerateSmsSummaryGrid: function () {
        $("#gridSmsSettingsSummary").kendoGrid({
            dataSource: SmsSettingsSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: SmsSettingsSummaryHelper.GenerateSmsSummaryColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },

    GenerateSmsSummaryColumns: function () {
        return columns = [
               { field: "SmsId", title: "SmsId", width: 80, hidden: true },
               { field: "SmsTypeName", title: "SMS Type", width: 80, },
               { field: "Salutation", title: "Salutation", width: 80, },
               { field: "Greetings", title: "Greetings", width: 60 },
               { field: "CustomerInfo", title: "CustomerInfo", width: 80 },
               { field: "DueInfo", title: "DueInfo", width: 80 },
               { field: "PaidInfo", title: "PaidInfo", width: 80 },
               { field: "Unit", title: "Unit", width: 60, hidden: true },
               { field: "CodeInfo", title: "CodeInfo", width: 60, hidden: true },
               { field: "Request", title: "Request", width: 60, hidden: true },
               { field: "Thanking", title: "Thanking", width: 60, hidden: true },
               { field: "Edit", title: "Edit", filterable: false, width: 60, template: '<button type="button" value="Edit" id="btnEdit" onClick="SmsSettingsSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#gridSmsSettingsSummary").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        SmsSettingsDetailsHelper.FillSmsDetailsForm(selectedItem);
    }
};

