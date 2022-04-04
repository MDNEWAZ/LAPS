
var CollectionHistoryManager = {    
    GenerateCollectionHistoryGrid: function (invoiceNo) {
      
        $("#gridCollectionHistorySummary").kendoGrid({
            dataSource: CollectionHistoryManager.gridDataSource(invoiceNo),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: CollectionHistoryHelper.GenerateCollectionHistoryColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },
    gridDataSource: function (invoiceNo) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Collection/GetCollectionHistoryByInvoice/?invoiceNo=' + invoiceNo,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: {
                data: "Items", total: "TotalCount",
                model: {
                    fields: {
                        DueDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        },
                        PayDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        },
                        EntryDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        },
                    }
                },
            }
        });
        return gridDataSource;
    }
};


var CollectionHistoryHelper = {
    InitCollectionHistory: function () {
        CollectionHistoryHelper.CollectionHistoryPopUpWindow();
        $("#btnCollectionHistory").click(function() {
            $("#divCollectionHistory").data("kendoWindow").open().center();
            var invoiceNo = $("#txtInvoiceProInfo").val();
            CollectionHistoryManager.GenerateCollectionHistoryGrid(invoiceNo);
        });
        $("#btnCloseHistory").click(function () {
            $("#divCollectionHistory").data("kendoWindow").close();

        });
    },
    CollectionHistoryPopUpWindow: function () {
        $("#divCollectionHistory").kendoWindow({
            title: "Collection History",
            resizeable: false,
            width: "90%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
        });
    },
    
    GenerateCollectionHistoryColumns: function () {
        return columns = [
          
            { field: "InstallmentNo", title: "Installment No", width: 50 },
            { field: "TransectionId", title: "TransectionId", width: 50 },
            { field: "ReceiveAmount", title: "Receive Amount", width: 50 },
            { field: "DueAmount", title: "Due Amount", width: 60 },
            { field: "DueDate", title: "Due Date", width: 50, template: '#=kendo.toString(DueDate,"dd/MM/yyyy")#' },
            { field: "PayDate", title: "Pay Date", width: 60, template: '#=kendo.toString(PayDate,"dd/MM/yyyy")#' },
            { field: "EntryDate", title: "Entry Date", width: 60, template: '#=kendo.toString(EntryDate,"dd/MM/yyyy")#' },
            { field: "CollectionTypeName", title: "Collection Type", width: 60 }
          
        ];
    },
};

