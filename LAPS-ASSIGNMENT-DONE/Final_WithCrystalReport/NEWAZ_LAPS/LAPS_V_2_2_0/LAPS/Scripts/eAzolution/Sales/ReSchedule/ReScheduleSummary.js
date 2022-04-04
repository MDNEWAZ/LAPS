var ReScheduleSummaryHelper = {

    PopulateRescheduleSummaryGrid: function () {
        var validator = $("#divReschedule").kendoValidator().data("kendoValidator"), status = $(".status");
        if (validator.validate()) {
            var gridData = ReScheduleSummaryManager.InstallmentGridDataSource();
      
                var grid = $("#gridInstallment").data("kendoGrid");
                grid.setDataSource(gridData);
                
                if (gridData._data.length == 0) {
                    AjaxManager.MsgBox('warning', 'center', 'Warning', 'Data Not Found !',
                       [{
                           addClass: 'btn btn-primary',
                           text: 'Ok',
                           onClick: function ($noty) {
                               $noty.close();

                           }
                       }]);
                }
              
            

        }

    },

    PopulateScheduleSummary: function () {
        $("#gridInstallment").kendoGrid({
            dataSource: [],// ReScheduleSummaryManager.InstallmentGridDataSource(),
            filterable: true,
            sortable: true,
            columns: ReScheduleSummaryHelper.GetImGridColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
            dataBound: function () {

                var dataView = this.dataSource.view();
                for (var i = 0; i < dataView.length; i++) {
                    var uid = dataView[i].uid;

                    if (dataView[i].Status === 1) {
                        $("#gridInstallment tbody").find("tr[data-uid=" + uid + "]").css("background-color", "#5ef1a8").css("border-bottom", "1px solid #000");
                    }
                    if (dataView[i].Status === 2) {
                        $("#gridInstallment tbody").find("tr[data-uid=" + uid + "]").css("background-color", "#FFAE00").css("border-bottom", "1px solid #000");
                    }

                }
            },
        });
    },
    GetImGridColumns: function () {
        return columns = [
          { field: "ProductNo", title: "ProductNo", hidden: true },
          { field: "Number", title: "Inst. No", width: 70 },
          { field: "SInvoice", title: "Invoice No", },
          { field: "DueDate", title: "Due Date", template: '#=kendo.toString(DueDate,"dd-MMM-yyyy")#' },
          { field: "Amount", title: "Ins. Amount" },//format: "{0:d}" 
          { field: "ReceiveAmount", title: "Recvd. Amount.", },
          { field: "DueAmount", title: "Due Amount", },//format: "{0:d}" 
          { field: "Status", title: "Status", template: "#=ReScheduleSummaryHelper.ShowStatus(data) #" } //Status==0?'Unpaid':'Paid'
        ];
    },
    ShowStatus: function (data) {
        if (data.Status == 1) {
            return "Paid";
            //  return ' <span style="background-color: red">R</span>';
        }
        if (data.Status == 2) {
            return "Partial Paid";
        }
        if (data.Status == 0) {
            return "Un Paid";
        }
    }
};
var ReScheduleSummaryManager = {
    InstallmentGridDataSource: function () {
        var companyId = $("#cmbCompany").val();
        var branchId = $("#cmbBranch").val();
        var customerCode = $("#txtCustomerCode").val();
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: false,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            //pageSize: 5,
            transport: {
                read: {
                    url: '../Sale/GetAllInstallmentOfCustomer/?companyId=' + companyId + "&branchId=" + branchId + "&customerCode=" + customerCode,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    async: false,
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