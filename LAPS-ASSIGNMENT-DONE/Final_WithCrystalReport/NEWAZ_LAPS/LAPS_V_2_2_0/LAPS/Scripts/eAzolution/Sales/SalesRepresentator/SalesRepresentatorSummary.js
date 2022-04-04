var SalesRepresentatorSummaryManager = {
    GenerateSalesRepresentatorGrid: function () {
        $("#gridSalesRepSummary").kendoGrid({
            dataSource:SalesRepresentatorSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: SalesRepresentatorSummaryHelper.GenerateSalesRepresentatorColumns(),
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
                    url: '../SalesRepresentator/GetAllSalesRepresentator/',

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

var SalesRepresentatorSummaryHelper = {
    GenerateSalesRepresentatorColumns: function () {
        return columns = [
               { field: "SalesRepCode", title: "Rep Code", width: 80, hidden: true },
               { field: "SalesRepId", title: "Rep ID", width: 80 },
               { field: "SalesRepTypeName", title: "Rep Type", width: 60 },
               { field: "Address", title: "Address", width: 80 },
               { field: "SalesRepSmsMobNo", title: "SMS Mob No", width: 80 },
               { field: "SalesRepBkashNo", title: "BKash Mob No", width: 80 },
               { field: "FixedAmount", title: "Amount", width: 60,hidden:true },
               { field: "Edit", title: "Edit", filterable: false, width: 60, template: '<button type="button" value="Edit" id="btnEdit" onClick="SalesRepresentatorSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#gridSalesRepSummary").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        SalesRepresentatorDetailsHelper.FillSalesRepresentatorForm(selectedItem);
    }
};