
var CreateInstallmentManager = {

    InitiateCreateInstallment: function () {
       // InstallmentDetailsHelper.GenerateStatusCombo();
        CreateInstallmentManager.GenerateInstalmentGrid();
    },

    GenerateInstalmentGrid: function () {
        $("#gridCreateInstalment").kendoGrid({
            dataSource: [],
            //dataSource: saleDetailsHelper.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            height: 250,
            filterable: true,
            sortable: true,
            columns: CreateInstallmentHelper.GenerateInstalmentColumns(),
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
            pageSize: 5,
            transport: {
                read: {
                    url: '../Sale/GetAllInstalmentByInvoiceNo/?invoiceNo=' + invoiceNo,
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
        //gridDataSource.filter({ field: "Status", operator: "eq", value: parseInt(0) });
        return gridDataSource;
    },

    FillInstallmentGrid: function (invoiceNo) {
        var data = CreateInstallmentManager.gridDataSource(invoiceNo);
        var installmentGrid = $("#gridCreateInstalment").data("kendoGrid");
        installmentGrid.setDataSource(data);
    }

};

var CreateInstallmentHelper = {

    GenerateInstalmentColumns: function () {
        return columns = [
           { field: "Number", title: "Installment Number", width: 50 },
           { field: "DueDate", title: "Due Date", width: 50, template: '#=kendo.toString(DueDate,"dd-MMM-yyyy")#' },
           { field: "Amount", title: "Installment Amount", width: 60 },
           { field: "Status", title: "Status", width: 50, template: "#= Status==0?'Unpaid':'Paid'#" }
          // { field: "Edit", title: "Action", filterable: false, width: 40, template: "#=Status==0?InstallmentDetailsHelper.buttonTemplate():''#", sortable: false }//#=Status==1?"InActive":"Active"#


        ];
    },

    buttonTemplate: function () {
        return '<input type="button" class="k-button" value="Receive" id="btnEdit" onClick="InstallmentDetailsHelper.clickEventForEditButton()"/>';
    },


    clickEventForEditButton: function () {
        var entityGrid = $("#gridInstalment").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        CollectionDetailsHelper.FillPaymentCollectionForm(selectedItem);
    },


    //GenerateStatusCombo: function () {
    //    var dropdown = $("#cmbStatus").kendoDropDownList({

    //        dataTextField: "text",
    //        dataValueField: "value",
    //        autoBind: false,
    //        //optionLabel: "All",
    //        index: 2,
    //        dataSource: [
    //            { text: "All" },
    //            { text: "Paid", value: "1" },
    //            { text: "Unpaid", value: "0" }

    //        ],
    //        change: function () {

    //            var value = this.value();
    //            if (value == 1 || value == 0) {
    //                $("#gridInstalment").data("kendoGrid").dataSource.filter({ field: "Status", operator: "eq", value: parseInt(value) });

    //            } else {
    //                $("#gridInstalment").data("kendoGrid").dataSource.filter({});

    //            }
    //        },

    //    }).data('kendoDropDownList');
    //    dropdown.value(0);

    //},
};


