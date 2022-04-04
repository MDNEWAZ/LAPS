

var ReplacementSummaryManager = {
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
            columns: ReplacementSummaryHelper.GenerateReplacementColumns(),
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
            schema: {
                model: {
                    fields: {

                        ManufactureDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        },
                        AProduct: {
                            WarrantyEndDate: {
                                type: "date",
                                template: '#= kendo.toString("MM/dd/yyyy") #'
                            },
                        },
                        ReplacementDate: {
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
    FillRplacementGrid: function (invoiceNo) {

        var replacementData = ReplacementSummaryManager.ReplacementDataSource(invoiceNo);
        var replacementGrid = $("#replacementGrid").data("kendoGrid");
        replacementGrid.setDataSource(replacementData);
    }
};

var ReplacementSummaryHelper = {
    GenerateReplacementColumns: function () {
        return columns = [

           { field: "AProduct.Model", title: "Model No", width: 50 },
           { field: "AProduct.ProductName", title: "Product Name", width: 60 },
           { field: "Type", title: "Product Type", width: 60, },
           { field: "ReplacedItemSLNo", title: "Replace SL No", width: 60, },
           { field: "ReplaceInvoiceNo", title: "Replace Invoice No", width: 60, },
           { field: "ManufactureDate", title: "Manufacture Date", width: 80, template: '#=kendo.toString(ManufactureDate,"dd/MM/yyyy")#' },
           { field: "AProduct.WarrantyEndDate", title: "Warranty End Date", width: 80, template: '#=kendo.toString(AProduct.WarrantyEndDate,"dd/MM/yyyy")#' },
           { field: "ReplacementDate", title: "Replacement Date", width: 70, template: '#=kendo.toString(ReplacementDate,"dd-MMM-yyyy")#' }
           //{ field: "ALicense.Number", title: "License No", width: 60 },
           //{ field: "ALicense.LType", title: "License Type", width: 70 },
           //{ field: "Edit", title: "Receive", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Receive" id="btnEdit" onClick="SaleSummaryHelper.clickEventForEditButton()"/>', sortable: false }
        ];
    },
};