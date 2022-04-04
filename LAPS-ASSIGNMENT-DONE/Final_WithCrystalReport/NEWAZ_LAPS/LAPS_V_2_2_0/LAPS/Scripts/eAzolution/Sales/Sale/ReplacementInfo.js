

var saleReplacementInfoManager = {
    GenerateReplacementGrid: function () {

        $("#replacementGrid").kendoGrid({
            dataSource: [],//customerSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: saleReplacementInfoHelper.GenerateReplacementColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    ReplacementDataSource: function (invoiceNo) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Replacement/GetReplacementInfoByInvoiceNo/?invoiceNo=' + invoiceNo,
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
    },
    FillRplacementGrid: function (invoiceNo) {

        var replacementData = saleReplacementInfoManager.ReplacementDataSource(invoiceNo);
        var replacementGrid = $("#replacementGrid").data("kendoGrid");
        replacementGrid.setDataSource(replacementData);
    }
};

var saleReplacementInfoHelper = {
    GenerateReplacementColumns: function () {
        return columns = [
           { field: "AProduct.Code", title: "Product Code", width: 50,hidden:true },
           { field: "AProduct.Model", title: "Model No", width: 50 },
           { field: "AProduct.ProductName", title: "Product Name", width: 60 },
           { field: "ManufactureDate", title: "Manufacture Date", width: 80 },
           { field: "AProduct.WarrantyEndDate", title: "Warranty End Date", width: 80 },
           { field: "AProduct.Type", title: "Product Type", width: 60, },
           { field: "ReplacementDate", title: "Replacement Date", width: 70 },
           { field: "ALicense.Number", title: "License No", width: 60 },
           { field: "ALicense.LType", title: "License Type", width: 70 },
           //{ field: "Edit", title: "Receive", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Receive" id="btnEdit" onClick="SaleSummaryHelper.clickEventForEditButton()"/>', sortable: false }
        ];
    },
};