
var gbUserLevel = 0;
var gbUserLevel2 = 0;
var UnrecognizedSaleSummaryManager = {
    InitUnrecognizedSaleSummary: function () {
        UnrecognizedSaleSummaryHelper.SalesDateGenerate();
        UnrecognizedSaleSummaryManager.GenerateUnrecognizedSaleGrid();

        $("#btnSearchUnrecognizedSaleInfo").click(function () {
            UnrecognizedSaleSummaryManager.SearchUnrecognizedSalesSummaryByParam();
        });
       
        $("#txtInvoiceNo").change(function () {
            UnrecognizedSaleSummaryManager.SearchUnrecognizedSalesSummaryByParam();
        });

        //check user level
        var userLevel = empressCommonManager.GetUserLevel("unrecognizedMuduleId");
        gbUserLevel = userLevel;

    },

    GenerateUnrecognizedSaleGrid: function () {
        $("#gridUnrecognizedSale").kendoGrid({
            dataSource: UnrecognizedSaleSummaryManager.gridDataSource(),
          
            filterable: true,
            sortable: true,
            columns: UnrecognizedSaleSummaryHelper.GenerateUnrecognizedSaleColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },

    gridDataSource: function () {
        var invoiceNo = $("#txtInvoiceNo").val();
        var salesDateFrom = $("#dtSalesDateFrom").data("kendoDatePicker").value();
        var salesDateTo = $("#dtSalesDateTo").data("kendoDatePicker").value();

        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: false,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            // pageSize: 10,
            transport: {
                read: {
                    url: '../Sale/GetAllUnrecognizedSale/?invoiceNo=' + invoiceNo + "&salesDateFrom=" + kendo.toString(salesDateFrom,"MM/dd/yyyy") + "&salesDateTo=" + kendo.toString(salesDateTo,"MM/dd/yyyy"),
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
                        WarrantyStartDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        },
                        FirstPayDate: {
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

    SearchUnrecognizedSalesSummaryByParam: function () {
        var data = UnrecognizedSaleSummaryManager.gridDataSource();
        var grid = $("#gridUnrecognizedSale").data("kendoGrid");
        grid.setDataSource(data);
    },
};

var UnrecognizedSaleSummaryHelper = {
   

    GenerateUnrecognizedSaleColumns: function () {
        return columns = [
            { field: "Invoice", title: "Invoice No", width: 60 },
            { field: "WarrantyStartDate", title: "Sales Date", width: 50, template: '#=kendo.toString(WarrantyStartDate,"dd/MM/yyyy")#', filterable: false },
            { field: "FirstPayDate", title: "First Pay Date", width: 50, template: '#=kendo.toString(FirstPayDate,"dd/MM/yyyy")#', filterable: false },
            { field: "ACustomer.CustomerCode", title: "Cusomer Code", width: 70 },
            { field: "ACustomer.Name", title: "Customer Name", width: 100 },
            { field: "ACustomer.BranchId", title: "BranchId", width: 100, hidden: true },
            { field: "ACustomer.BranchCode", title: "BranchCode", width: 40 },
            { field: "ACustomer.Phone", title: "Mobile No", width: 60 },
            { field: "AProduct.ProductName", title: "Product Name", width: 80, filterable: false },
            { field: "AProduct.Model", title: "Model", width: 50, filterable: false },
            { field: "Price", title: "Price", width: 50, filterable: false },
            { field: "DownPay", title: "DP", width: 50, filterable: false },
            { field: "Installment", title: "IM", width: 30,filterable: false },
            { field: "ProductNo", hidden: true },
            { field: "CustomerId", hidden: true },
            { field: "SaleId", hidden: true },
            { field: "State", hidden: true },
            { field: "TempState", hidden: true },
            { field: "Comments", title: "Comments", width: 120, },
            { field: "IsActive", hidden: true },
            { field: "ADiscount.DiscountTypeId", hidden: true },
            { field: "TypeOfUnRecognized", title: "Type", template: "#=UnrecognizedSaleSummaryHelper.ShowUnrecognizeType(data) #", width: 70, filterable: false },
            { field: "Edit", title: "Edit Sale", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="UnrecognizedSaleSummaryHelper.clickEventForEditButton()"/>', sortable: false },
            
        ];
    },
    ShowUnrecognizeType: function (data) {
        if (data.TypeOfUnRecognized == 1) {
            return "SD";
        }
        if (data.TypeOfUnRecognized == 2) {
            return "DP";
        }
        if (data.TypeOfUnRecognized == 3) {
            return "SMS Error";
        }
        if (data.TypeOfUnRecognized == 4) {
            return "Others";
        }
        if (data.TypeOfUnRecognized == 5) {
            return "2nd Sale By SMS";
        }
        //if (data.IsSpecialDiscount == 1) {
        //    return "SD";
        //}
    },
    
    clickEventForEditButton: function () {
        var entityGrid = $("#gridUnrecognizedSale").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            $("#popupUnrecognizedSaleDetails").data("kendoWindow").open().center();
            UnrecognizedSaleDetailsHelper.FillUnrecognizedSaleDetailsForm(selectedItem);
        }
      

    },

    SalesDateGenerate: function () {

        $("#dtSalesDateFrom").kendoDatePicker({
            depth: "year",
            format: "dd-MM-yyyy",
        });

        $("#dtSalesDateTo").kendoDatePicker({
            depth: "year",
            format: "dd-MM-yyyy",
        });

        //$("#dtSalesDateFrom").data("kendoDatePicker").value(new Date());
        //$("#dtSalesDateTo").data("kendoDatePicker").value(new Date());
    }
};