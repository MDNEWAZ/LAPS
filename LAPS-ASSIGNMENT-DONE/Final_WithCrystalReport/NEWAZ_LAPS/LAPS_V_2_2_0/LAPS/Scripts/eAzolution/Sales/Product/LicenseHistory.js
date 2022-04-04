
var productLicenseManager = {
    GenerateLicenseGrid: function () {
        $("#gridLicense").kendoGrid({
            dataSource:[],
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: productLicenseHelper.GenerateProductColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    gridDataSource: function (productId) {
        var gridData = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Product/GetAProductLicense/?productId=' + productId,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                //update: {
                //    url: '../Product/LoadAllCompanies/',
                //    dataType: "json"
                //},

                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: {
                model: {
                    fields: {
                        Sl:{type:'number',defaultValue:0}
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridData;
    }
};

var productLicenseHelper = {
    GenerateProductColumns: function () {
        return columns = [
            { field: "Sl", title: "SL", width: 30},
            { field: "Number", title: "License Number", width: 70 },
            { field: "LType", title: "License Type", width: 70 },
            { field: "IssueDate", title: "License Date", width: 70 }
        ];
    }
};

//var productSummaryManager = {
//    GenerateProductGrid: function () {
//        $("#gridProduct").kendoGrid({
//            dataSource: productSummaryManager.gridDataSource(),
//            pageable: {
//                refresh: true,
//                serverPaging: true,
//                serverFiltering: true,
//                serverSorting: true
//            },
//            filterable: true,
//            sortable: true,
//            columns: productSummaryHelper.GenerateProductColumns(),
//            editable: false,
//            navigatable: true,
//            selectable: "row",
//        });
//    },
//    gridDataSource: function () {
//        var gridData = new kendo.data.DataSource({
//            type: "json",
//            serverPaging: true,
//            serverSorting: true,
//            serverFiltering: true,
//            allowUnsort: true,
//            pageSize: 10,
//            transport: {
//                read: {
//                    url: '../Product/GetAllProduct/',
//                    type: "POST",
//                    dataType: "json",
//                    contentType: "application/json; charset=utf-8"
//                },
//                //update: {
//                //    url: '../Product/LoadAllCompanies/',
//                //    dataType: "json"
//                //},

//                parameterMap: function (options) {
//                    return JSON.stringify(options);
//                }
//            },
//            schema: { data: "Items", total: "TotalCount" }
//        });
//        return gridData;
//    }
//};

//var productSummaryHelper = {
//    GenerateProductColumns: function () {
//        return columns = [
//            { field: "Model", title: "Model", width: 70 },
//            { field: "ProductName", title: "Name", width: 90 },
//            { field: "Description", title: "Description", width: 100 },
//            { field: "Capacity", title: "Capacity", width: 70 },
//            { field: "Code", hidden: true },
//            { field: "Type", hidden: true },
//            { field: "Color", hidden: true },
//            { field: "FullLogoPath", hidden: true },
//            { field: "PrimaryContact", hidden: true },
//            { field: "FiscalYearStart", hidden: true },
//            { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="productSummaryHelper.clickEventForEditButton()"  />', sortable: false }
//        ];
//    },
//    clickEventForEditButton: function () {
//        var entityGrid = $("#gridProduct").data("kendoGrid");
//        var selectedItem = entityGrid.dataItem(entityGrid.select());
//        productHelper.FillProductDetailsInForm(selectedItem);
//    }
//};