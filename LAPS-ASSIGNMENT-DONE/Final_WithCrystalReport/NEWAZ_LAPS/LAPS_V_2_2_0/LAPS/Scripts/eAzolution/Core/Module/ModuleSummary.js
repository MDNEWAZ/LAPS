var moduleSummaryManager = {
    GenerateModuleGrid: function () {
        $("#gridModule").kendoGrid({
            dataSource: moduleSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            sortable: true,
            filterable: true,
            
            columns: moduleSummaryHelper.GenerateModuleColumns(),
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
            sortable: true,
            pageSize: 10,

            transport: {
                read: {
                    url: '../Module/GetModuleSummary/',

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../Module/GetModuleSummary/',
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

var moduleSummaryHelper = {
    
    GenerateModuleColumns: function () {
        return columns = [
             { field: "ModuleName", title: "Module Name", width: 150, sortable:true },
            { field: "Edit", title: "Edit", filterable: false, width: 30, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="moduleSummaryHelper.clickEventForEditButton()"  />', sortable: false }
        ];

    },
GeRowDataOfModuleGrid: function () {
    $('#gridModule table tr').live('dblclick', function () {
        var entityGrid = $("#gridModule").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        moduleDetailsHelper.FillModuleDetailsInForm(selectedItem);
    });

},
    
    clickEventForEditButton: function () {
        var entityGrid = $("#gridModule").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        moduleDetailsHelper.FillModuleDetailsInForm(selectedItem);
    }
};