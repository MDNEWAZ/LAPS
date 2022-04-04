var accessControlSummaryManager = {
    GenerateAccessControlGrid: function () {
        $("#gridAccessControl").kendoGrid({
            dataSource: accessControlSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: accessControlSummaryHelper.GenerateAccessControlColumns(),
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
                    url: '../AccessControl/GetAccessControlSummary/',

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../AccessControl/GetAccessControlSummary/',
                    dataType: "json"
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
var accessControlSummaryHelper = {
    
    GenerateAccessControlColumns: function () {
        return columns = [
             { field: "AccessName", title: "Access Name", width: 150 },
             { field: "Edit", title: "Edit", filterable: false, width: 30, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="accessControlSummaryHelper.clickEventForEditButton()"  />', sortable:false }
        ];

    },
    GeRowDataOfAccessControlGrid: function () {
        $('#gridAccessControl table tr').live('dblclick', function () {
            var entityGrid = $("#gridAccessControl").data("kendoGrid");
            var selectedItem = entityGrid.dataItem(entityGrid.select());
            accessControlDetailsHelper.FillAccessControlDetailsInForm(selectedItem);
        });

    },
    
    clickEventForEditButton: function () {
        
        var entityGrid = $("#gridAccessControl").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        accessControlDetailsHelper.FillAccessControlDetailsInForm(selectedItem);

    },
};