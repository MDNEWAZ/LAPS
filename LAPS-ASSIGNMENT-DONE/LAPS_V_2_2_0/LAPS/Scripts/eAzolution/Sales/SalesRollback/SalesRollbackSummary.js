
var SalesRollbackSummaryManager = {
    SalesInfoGridDataSource: function () {
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
                    url: '../SalesRollback/GetCustomerSalesInformation/?companyId=' + companyId + "&branchId=" + branchId + "&customerCode=" + customerCode,
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

var SalesRollbackSummaryHelper = {

    PopulateSalesInforamtionSummaryGrid: function () {
        var validator = $("#divSaleRollBack").kendoValidator().data("kendoValidator"), status = $(".status");
        if (validator.validate()) {
            var gridData = SalesRollbackSummaryManager.SalesInfoGridDataSource();

            var grid = $("#gridSaleInforamtion").data("kendoGrid");
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

    PopulateSaleRollbackSummary: function () {
        $("#gridSaleInforamtion").kendoGrid({
            dataSource: [],// ReScheduleSummaryManager.InstallmentGridDataSource(),
            filterable: true,
            sortable: true,
            columns: SalesRollbackSummaryHelper.GetSaleInfoColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",

        });
    },
    GetSaleInfoColumns: function () {
        return columns = [
          { field: "ACustomer.Name", title: "Customer Name", width: 70 },
          { field: "ACustomer.CustomerCode", title: "Customer Code", width: 70 },
          { field: "ACustomer.BranchCode", title: "Branch Code", width: 70 },
          { field: "AProduct.ProductName", title: "Package Name", width: 70 },
          { field: "Invoice", title: "Invoice", width: 70 },
          { field: "Price", title: "Price", width: 70 },
          { field: "Installment", title: "Installment No", width: 40 },
          { field: "WarrantyStartDate", title: "Sale Date", template: '#=kendo.toString(kendo.parseDate(WarrantyStartDate,"dd-MMM-yyyy"),"dd-MMM-yyyy")#', width: 60 },
          { field: "Edit", title: "Edit Sale", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Roll Back" id="btnInActive" onClick="SalesRollbackSummaryHelper.clickEventForInActiveButton()"/>', sortable: false },

        ];
    },
    
    clickEventForInActiveButton: function() {
        var entityGrid = $("#gridSaleInforamtion").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
          
            SalesRollbackInfoHelper.initSalesRollbackHelper();
            $("#salesRollbackPopupDiv").data("kendoWindow").open().center();
        }
    },
};