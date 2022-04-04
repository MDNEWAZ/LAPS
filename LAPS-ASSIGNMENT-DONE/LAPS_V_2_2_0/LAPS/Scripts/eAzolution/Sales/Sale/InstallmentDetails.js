

var InstallmentDetailsManager = {
    
    InitiateInstallmentDetails: function () {
       
        InstallmentDetailsHelper.GenerateStatusCombo();
        $("#cmbStatus").parent().css('width', "10em");

        InstallmentDetailsManager.GenerateInstalmentGrid();
       
    },
     
    GenerateInstalmentGrid: function () {
        $("#gridInstalment").kendoGrid({
            dataSource: [],
            filterable: true,
            sortable: true,
            columns: InstallmentDetailsHelper.GenerateInstalmentColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
            dataBound: function () {

                var dataView = this.dataSource.view();
                for (var i = 0; i < dataView.length; i++) {
                    var uid = dataView[i].uid;

                    if (dataView[i].Status === 1) {
                        $("#gridInstalment tbody").find("tr[data-uid=" + uid + "]").css("color", "#006633");
                    }
                    if (dataView[i].Status === 2) {
                        $("#gridInstalment tbody").find("tr[data-uid=" + uid + "]").css("color", "#FFAE00");
                    }

                }
            },
        });

    },
    gridDataSource: function (invoiceNo) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: false,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
           //pageSize: 5,
            transport: {
                read: {
                    url: '../Sale/GetAllInstalmentByInvoiceNo/?invoiceNo='+invoiceNo,
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
        gridDataSource.filter({ field: "Status", operator: "eq", value: parseInt(0) });
        return gridDataSource;
    },
    
    FillInstallmentGrid: function (invoiceNo) {
     

        var data = InstallmentDetailsManager.gridDataSource(invoiceNo);
        
       $("#gridInstalment").data("kendoGrid").setDataSource(data);
    }

};

var InstallmentDetailsHelper= {

    GenerateInstalmentColumns: function () {
        return columns = [
           { field: "ProductNo", title: "ProductNo",hidden:true },
           { field: "Number", title: "Inst. No", width: 70 },
           { field: "SInvoice", title: "Invoice No",  },
           { field: "DueDate", title: "Due Date", template: '#=kendo.toString(DueDate,"dd-MMM-yyyy")#' },
           { field: "Amount", title: "Amount" },//format: "{0:d}" 
           { field: "ReceiveAmount", title: "Recvd. Amount.",},
           { field: "DueAmount", title: "Due Amount", },//format: "{0:d}"
           //{ field: "Status", title: "Status", template: "#= Status==0?'Unpaid':'Paid'#" },
            { field: "Status", title: "Status", template: "#= InstallmentDetailsHelper.dynamicStatus(data) #" },
           //{ field: "Edit", title: "Action", filterable: false, template: "#=Status==0?InstallmentDetailsHelper.buttonTemplate():''#", sortable: false }//#=Status==1?"InActive":"Active"#
          
            
        ];
    },
    dynamicStatus:function (data) {
        if (data.Status == 0) {
            return "Unpaid";
        }
        else if (data.Status == 1) {
            return "Paid";
        }
        else if (data.Status == 2) {
            return "Partial Paid";
        }
    },
    buttonTemplate: function () {
      return '<input type="button" class="k-button" value="Receive" id="btnEdit" onClick="InstallmentDetailsHelper.clickEventForEditButton()"/>';
    },
    

    clickEventForEditButton: function () {
        var entityGrid = $("#gridInstalment").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        CollectionDetailsHelper.FillPaymentCollectionForm(selectedItem);
    },
    

    //Status Combo for Installment Grid
    GenerateStatusCombo: function () {
        var dropdown = $("#cmbStatus").kendoDropDownList({

            dataTextField: "text",
            dataValueField: "value",
            autoBind: false,
            //optionLabel: "All",
            index: 2,
            dataSource: [
                { text: "All" },
                { text: "Paid", value: "1" },
                { text: "Unpaid", value: "0" }

            ],
            change: function () {

                var value = this.value();
                if (value == 1 || value == 0) {
                    $("#gridInstalment").data("kendoGrid").dataSource.filter({ field: "Status", operator: "eq", value: parseInt(value) });
                   
                } else {
                    $("#gridInstalment").data("kendoGrid").dataSource.filter({});
                  
                }
            },

        }).data('kendoDropDownList');
        dropdown.value(0);

    },
};


