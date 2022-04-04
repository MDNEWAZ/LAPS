
var StockItemsDetailsManager = {
    gridDataSource: function (itemId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    url: '../Stock/GetAllStockItemsByItemId/?itemId=' + itemId,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: {
                model: {
                    fields: {

                        ProductItems: { defaultValue: { ItemId: 0, ItemName: "--Select--" } },

                        Quantity: {
                            type: "number", validation: { required: true, min: 0 },

                        },
                        StockId: {
                            type: "number",
                        },
                        ReceiveDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        },


                    }
                },
                data: "Items", total: "TotalCount"
            }
        });


        return gridDataSource;


    },
};

var StockItemsDetailsHelper = {
    GenerateStockItemsDetailsGrid: function (itemId) {

        $("#stockDetailsGrid").kendoGrid({
            dataSource: StockItemsDetailsManager.gridDataSource(itemId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: StockItemsDetailsHelper.ProductStockItemsColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },

    ProductStockItemsColumns: function () {
        return columns = [
              { field: "ItemId", title: "Item Id", width: 70, hidden: true },
              { field: "ItemName", title: "Item Name", width: 70 },
              { field: "Quantity", title: "Quantity", width: 70, editable: true, },
              { field: "ReceiveDate", title: "Receive Date", width: "60px", format: "{0:MM/dd/yyyy}" },
            //{ field: "Total", hidden: true},
            //{ field: "Status", title: "Status", width: 70, template: "#= Status==0?'InActive':'Active'#" }
            //{ field: "Edit", title: "Action", filterable: false, width: 50, template: '<input type="button" class="k-button" value="View" id="btnEdit" onClick="stockHelper.clickEventForViewButton()"/>', sortable: false }
        ];
    },
};