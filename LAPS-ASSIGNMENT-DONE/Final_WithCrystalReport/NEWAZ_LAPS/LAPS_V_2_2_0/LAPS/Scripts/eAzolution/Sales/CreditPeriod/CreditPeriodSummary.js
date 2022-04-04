var creditPeriodSummaryManager = {


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
                    url: '../CreditPeriod/GetCreditPeriodSummary/',

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
var creditPeriodSummaryHelper = {
    GenerateCreditPeriodGrid: function () {
        $("#gridCreditPeriod").kendoGrid({
            dataSource: creditPeriodSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: creditPeriodSummaryHelper.GeneratecreditPeriodColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },

    GeneratecreditPeriodColumns: function () {
        return columns = [
            { field: "ItemId", width: "100px", hidden: true },
                { field: "ProductItems", title: "Item Name", width: "60px", editor: StockProductItemsHelper.ProductItemDropDownEditor, template: "#=ProductItems.ItemName#" },
                { field: "ReceiveDate", title: "Receive Date", width: "60px", format: "{0:MM/dd/yyyy}" },
                { field: "Quantity", title: "Quantity", width: "50px", }

        ];

    },


    clickEventForEditButton: function () {

        var entityGrid = $("#gridAccessControl").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        creditPeriodHelper.FillCreditPeriodDetailsInForm(selectedItem);

    },
};