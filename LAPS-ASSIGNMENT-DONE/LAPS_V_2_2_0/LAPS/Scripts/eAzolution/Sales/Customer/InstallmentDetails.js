

var InstallmentInfoSummaryManager = {
    InitInstallmentDetails:function() {
        $("#DivInstallmentGrid").kendoWindow({

            title: "Installment Details",
            resizeable: false,
            width: "60%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
        });

    },
    GenerateInstallmentInfoGrid: function (invoice) {

        $("#gridInstallmentInfoDetails").kendoGrid({
            dataSource: InstallmentInfoSummaryManager.gridDataSource(invoice),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: InstallmentInfoSummaryHelper.GenerateInstallmentInfoColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },

    gridDataSource: function (invoice) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 12,
            transport: {
                read: {
                    url: '../Sale/GetInstallmentInfoByInvoice/?invoice=' + invoice,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                //update: {
                //    url: '../Customer/GetAllCustomer/',
                //    dataType: "json"
                //},
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    }
};

var InstallmentInfoSummaryHelper = {

    GenerateInstallmentInfoColumns: function () {
        return columns = [
           { field: "Number", title: "Installment No", width: 100, },
            { field: "DueDate", title: "Due Date", width: 100 },
            { field: "Amount", title: "Installment Amount", width: 100 },
            //{ field: "PayDate", title: "Actual Payment Date", width: 100 },
            { field: "Status", title: "Payment Status", width: 100, hidden: false,template:"#= Status==0?'Unpaid':'Paid'#" }
           // { field: "LisenseIssue", title: "Last License", width: 100, hidden: true },
            //{ field: "Type", title: "Last Type", width: 120, hidden: true },

           
        ];
    },
    
    Close:function () {
        $("#DivInstallmentGrid").data("kendoWindow").close();
        $("#gridInstallmentInfoDetails").data('kendoGrid').dataSource.data([]);
    }
  
};