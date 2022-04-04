var ProductModel = "";
var removeItemArray = [];
var ProductModelDetailsManager = {

    ProductModelDataSource: function (productId) {
        debugger;
        if (productId === 0) {
            return [];
        }
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
                    url: '../ProductType/GetProductModelSummary/?id=' + productId,
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
                    id: "ProductModelId",
                  fields: {
                      ProductModelId: { type: 'number', editable: true },
                      Edit: { editable:false  },
                       ProductModelName: { editable: true },
                        ProductModelPrice: {  editable: true },
                         
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    },

};

var ProductModelDetailsHelper = {
    GenerateProductModelGrid: function (productId) {
        
        $("#ProductModelDetailsDiv").kendoGrid({
            dataSource: ProductModelDetailsManager.ProductModelDataSource(productId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: ProductModelDetailsHelper.GenerateGridColumns(),
            editable: true,
            toolbar: ["create"],
            //navigatable: true,
            selectable: "row,multiple",
            //dataBound: function (e) {

            //}

        });
    },


    GenerateGridColumns: function () {
        return columns = [
            { field: "ProductModelName", title: "Model", width: 100 },
            { field: "ProductModelPrice", title: "Price", width: 150 },
            { field: "Edit", title: "Delete", filterable: false, width: 30, template: '<button type="button" value="Delete" id="btnEdit" onClick="ProductModelDetailsHelper.deleteRow()" ><span class="k-icon k-i-close"></span></button>', sortable: false }
        ];

    },
    deleteRow: function () {

        var gridProductItems = $("#ProductModelDetailsDiv").data("kendoGrid");
        var selectedItem = gridProductItems.dataItem(gridProductItems.select());
        removeItemArray.push(selectedItem);
        
        gridProductItems.dataSource.remove(selectedItem);

        return removeItemArray;
    }

 




};