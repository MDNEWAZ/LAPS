var NewCustomerDetailsManager = {
    ProductDataSource: function () {

        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            batch: true,
            //autoSync: true,

            transport: {

                read: {
                    url: '../NewCustomer/GetProductSummary/',
                    type: "POST",
                    dataType: "json",
                    cache: false,
                    async: false,
                    contentType: "application/json; charset=utf-8"

                },


                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                    return JSON.stringify(options);
                }
            },

            schema: {
                model: {
                    id: "ProductID",
                    fields: {
                        ProductID: { type: 'number', editable: true },
                        ProductName: { editable: true },
                        CategoryId: { editable: true },
                        Quantity: { editable: true }
                        

                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    }

};

var NewCustomerDetailsHelper = {
    GenerateProductGrid: function () {

        $("#gridProductDetails").kendoGrid({
            dataSource: NewCustomerDetailsManager.ProductDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: NewCustomerDetailsHelper.GenerateProductGridColumns(),
            editable: true,
            toolbar: ["create"],
            selectable: "row,multiple",
            dataBound: function (e) {

            }

        });
    },
    GenerateProductGridColumns: function () {
        return columns = [

            { field: "ProductName", title: "Product Name", width: 100 },
             { field: "CategoryId", title: " Category Name", width: 100 },
            { field: "Quantity", title: "Quantity", width: 150 },
             { command: [{ name: "destroy", text: " " }],title: "&nbsp;", width: "50px" }
        ];

    }
};