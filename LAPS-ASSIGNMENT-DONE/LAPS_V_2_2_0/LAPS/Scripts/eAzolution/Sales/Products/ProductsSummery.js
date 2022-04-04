var ProductsSummary = "";
var ProductsSummaryManager = {
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
                    url: '../ProductType/GetProductSummary/',

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
var ProductsSummaryHelper = {

    GenerateProductsGrid: function () {
        debugger;
        $("#ProductsSummaryDiv").kendoGrid({
            dataSource: ProductsSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns:ProductsSummaryHelper.GeneratedProductsColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });
    },
    GeneratedProductsColumns: function () {
        return columns = [
            { field: "ProductsId", hidden: true },
            { field: "ProductName", title: "ProductName", width: 50 },
            //{ field: "ProductModelName", title: " Model", width: 50, sortable: false },
            { field: "ProductCode", title: "ProductCode", width: 50, sortable: true },
            //{ field: "ProductModelPrice", title: "Price", width: 50, sortable: true },
            { field: "ProductTypeName", title: "ProductTypeName", width: 50, sortable: false },
            
            { field: "IsActive", title: "Status", width: 50, sortable: false, template: "#=IsActive==1?'Active':'In Active'#" },
            { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Edit" id="btnEdit" onClick="ProductsSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-pencil"></span></button>', sortable: false }

        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#ProductsSummaryDiv").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            ProductsDetailsHelper.populateProductsDetails(selectedItem);
        }


    }
};