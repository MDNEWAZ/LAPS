var currentUserlevel = [];
var EmployeeSummaryManager = {
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
                    url: '../Employee/GetEmployeeTwoSummary/',

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../Employee/GetEmployeeTwoSummary/',
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
var EmployeeSummaryHelper = {

    initForEmployeeGrid: function () {
        EmployeeSummaryHelper.GenerateEmployeeGrid();
    },
    GenerateEmployeeGrid: function () {

        $("#divgridEmployeeSummary").kendoGrid({
            dataSource: EmployeeSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: EmployeeSummaryHelper.GeneratedEmployeeColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });
    },
    GeneratedEmployeeColumns: function () {
        return columns = [
        { field: "EmployeeID", title: "EmployeeID", width: 50, hidden: true },
        { field: "EmployeeName", title: "Employee  Name", width: 50, sortable: false },
        { field: "Designation", title: "Designation", width: 50, sortable: true },
        { field: "Salary", title: "Salary", width: 50, sortable: false },
        { field: "Edit", title: "Edit", filterable: false, width: 30, template: '<button type="button" value="Edit" id="btnEdit" onClick="EmployeeSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#divgridEmployeeSummary").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            EmployeeDetailsHelper.populateEmployeeDetails(selectedItem);
        }


    }
};