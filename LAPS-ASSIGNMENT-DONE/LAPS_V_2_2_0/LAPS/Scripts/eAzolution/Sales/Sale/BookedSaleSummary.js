var gbSelectiveBookedSaleArray = [];



var BookedSaleSummaryManager = {
    InitBookedSaleSummary: function () {
        $("#divAddCommentsPopup").kendoWindow({
            title: "Add Comments",
            resizeable: false,
            width: "40%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
        });

        BookedSaleSummaryHelper.GenerateDatePicker();
   
        $("#btnSearchBookedSaleInfo").click(function () {
            BookedSaleSummaryManager.SearchBookedSalesSummaryByParam();
        });
        $("#txtBookedDateFrom").change(function () {
            BookedSaleSummaryManager.SearchBookedSalesSummaryByParam();
        });
        $("#txtBookedDateTo").change(function () {
            BookedSaleSummaryManager.SearchBookedSalesSummaryByParam();
        });
        
        $("#txtBookedInvoice").change(function () {
            BookedSaleSummaryManager.SearchBookedSalesSummaryByParam();
        });

        BookedSaleSummaryHelper.BookedSaleGridChangeEvent();

        $("#btnSaveAsDraft").click(function() {
            saleDetailsManager.SaveAsDraft();
        });
        
        $("#btnMakeUnrecognize").click(function () {
            saleDetailsManager.MakeUnrecognized(gbUnrecogSaleObj, type);
        });
        $("#btnClose").click(function () {
            $("#divAddCommentsPopup").data("kendoWindow").close();
        });
    },

    GenerateBookedSaleGrid: function () {
        $("#gridBookedSale").kendoGrid({
            dataSource: BookedSaleSummaryManager.gridDataSource(),
            //pageable: {
            //    refresh: true,
            //    serverPaging: true,
            //    serverFiltering: true,
            //    serverSorting: true
            //},

            dataBound: function () {

                var dataView = this.dataSource.view();
                for (var i = 0; i < dataView.length; i++) {
                    var uid = dataView[i].uid;

                    if (dataView[i].ADiscount.DiscountTypeId === 3) {
                        $("#gridBookedSale tbody").find("tr[data-uid=" + uid + "]").css("background-color", "cornflowerblue");
                        $("#gridBookedSale tbody").find("tr[data-uid=" + uid + "]").css("color", "black");
                    }
                    if (dataView[i].ADiscount.DiscountTypeId === 2) {
                        $("#gridBookedSale tbody").find("tr[data-uid=" + uid + "]").css("background-color", "#FFAE00");
                        $("#gridBookedSale tbody").find("tr[data-uid=" + uid + "]").css("color", "#006633");
                    }

                }
            },

            filterable: false,
            sortable: true,
            columns: BookedSaleSummaryHelper.GenerateBookedSaleColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },

    gridDataSource: function () {
        var entryDateFrom = $("#txtBookedDateFrom").val();
        var entryDateTo = $("#txtBookedDateTo").val();
        var invoiceNo = $("#txtBookedInvoice").val();

        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: false,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            // pageSize: 10,
            transport: {
                read: {
                    url: '../Sale/GetAllBookedSale/?invoiceNo=' + invoiceNo + "&entryDateFrom=" + entryDateFrom + "&entryDateTo=" + entryDateTo,
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

    SearchBookedSalesSummaryByParam: function () {
        var data = BookedSaleSummaryManager.gridDataSource();
        var grid = $("#gridBookedSale").data("kendoGrid");
        grid.setDataSource(data);
    },
};

var BookedSaleSummaryHelper = {
    GenerateDatePicker: function () {
        $("#txtBookedDateFrom").kendoDatePicker({
            value: new Date(),
            
        });
        
        $("#txtBookedDateTo").kendoDatePicker({
            value: new Date(),
            
        });
    },

    GenerateBookedSaleColumns: function () {
        return columns = [
            { field: "check_row", title: "", width: 30, template: "<input class='check_row' type='checkbox' />", headerTemplate: "<input type='checkbox' id='chkSelectAllbook'/>", filterable: false, sortable: false },
            { field: "Invoice", title: "Invoice No", width: 60 },
            { field: "WarrantyStartDate", title: "Sales Date", width: 60, template: '#=kendo.toString(WarrantyStartDate,"dd/MM/yyyy")#' },
            { field: "FirstPayDate", title: "First Pay Date", width: 60, template: '#=kendo.toString(FirstPayDate,"dd/MM/yyyy")#' },
            { field: "ACustomer.CustomerCode", title: "Customer ID", width: 60 },
            { field: "ACustomer.Name", title: "Customer Name", width: 80 },
            { field: "ACustomer.BranchId", title: "BranchId", width: 50, hidden: true },
            { field: "ACustomer.BranchCode", title: "Branch<br/> Code", width: 40 },
            { field: "ACustomer.Phone2", title: "SMS Mob No", width: 70 },
           // { field: "AProduct.ProductNo", title: "Product Code", width: 70 },
            { field: "AProduct.ProductName", title: "Package", width: 90 },
            { field: "AProduct.Model", title: "Model", width: 40 },
            { field: "Price", title: "Price", width: 40 },
            { field: "DownPay", title: "Down Pay", width: 50 },
            { field: "Installment", title: "Inst. No", width: 40 },
            { field: "ProductNo", hidden: true },
            { field: "CustomerId", hidden: true },
            { field: "SaleId", hidden: true },
            { field: "Flag", hidden: true },
            { field: "IsActive", hidden: true },
            { field: "ADiscount.DiscountTypeId", hidden: true },
            { field: "Edit", title: "Edit Sale", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="BookedSaleSummaryHelper.clickEventForEditButton()"/>', sortable: false },
            { field: "Edit", title: "Unrecognize", filterable: false, width: 80, template: '<input type="button" class="k-button" value="Make Unrecognized" id="btnUnrecognize" onClick="BookedSaleSummaryHelper.clickEventForUnrecognizeButton()"/>', sortable: false }
        ];
    },
    
    clickEventForEditButton: function () {

        var entityGrid = $("#gridBookedSale").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            $("#bookedSaleSummaryDiv").hide();
            $("#saleDetailsMainDiv").show();

            if (selectedItem.IsActive == 1) {
                $("#btnSaveAsDraft").hide();
                $("#btnSave").hide();
                $("#btnClearAll").hide();
            } else {
                // $("#btnSaveAsDraft").show();
                if (gbIsViewer != 1) {
                    $("#btnSave").show();
                }
                $("#btnClearAll").show();
            }
            saleDetailsHelper.ClearAllSaleDetailsForm();
            $("#txtInvoiceSale").val(selectedItem.Invoice);
            ProductItemInfoHelper.GenerateProductItemInfoGrid(0);
            productInfoManager.ProductCustomerInfoFill(selectedItem.Invoice, selectedItem.SaleId);
            saleDetailsManager.GetDownPaymentCollectionInfo(selectedItem.SaleId);
        }
    },
    
    clickEventForUnrecognizeButton: function () {
        var entityGrid = $("#gridBookedSale").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());

        if (selectedItem != null && gbIsViewer != 1) {
            saleDetailsManager.SaveAsUnrecognized();
            gbUnrecogSaleObj = selectedItem;
            type = "booked";
        }
    },
    
    BookedSaleGridChangeEvent: function () {
     
        $('#gridBookedSale').on('change', '.check_row', function (e) {
         
            var $target = $(e.currentTarget);
            var grid = $("#gridBookedSale").data("kendoGrid");
            var dataItem = grid.dataItem($(this).closest('tr'));
            if ($target.prop("checked")) {
                if (dataItem.Invoice != "") {
                    gbSelectiveBookedSaleArray.push(dataItem);
                } else {
                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Input Invoice No First.!',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            $("#gridBookedSale tbody tr input:checkbox").removeAttr("checked", true);
                            $('tr.k-state-selected', '#gridBookedSale').removeClass('k-state-selected');

                        }
                    }]);
                }

            } else {
                for (var i = 0; i < gbSelectiveBookedSaleArray.length; i++) {
                    if (gbSelectiveBookedSaleArray[i].SaleId == dataItem.SaleId) {
                        gbSelectiveBookedSaleArray.splice(i, 1);
                        break;
                    }
                }

            }

        });


        $('#gridBookedSale').on('change', '#chkSelectAllbook', function (e) {
          
            gbSelectiveBookedSaleArray = [];
            var gridBookedSaleSummary = $("#gridBookedSale").data("kendoGrid");
            var selectAll = document.getElementById("chkSelectAllbook");
            if (selectAll.checked == true) {
                $("#gridBookedSale tbody input:checkbox").attr("checked", true);
                $("#gridBookedSale table tr").addClass('k-state-selected');
                var gridData = gridBookedSaleSummary.dataSource.data();

                for (var i = 0; i < gridData.length; i++) {
                    var sale = gridData[i];
                    if (sale.Invoice != "") { //escape where invoice no is null
                        gbSelectiveBookedSaleArray.push(sale);
                    } else {
                        $("#gridBookedSale tbody tr input:checkbox").removeAttr("checked", true);
                        $("#gridBookedSale table tr").removeClass('k-state-selected');
                    }

                }

            } else {
                $("#gridBookedSale tbody input:checkbox").removeAttr("checked", this.checked);
                $("#gridBookedSale table tr").removeClass('k-state-selected');
                gbSelectiveBookedSaleArray = [];


            }
        });



    },
    
    ReloadEntryDate: function () {
        var newDate = new Date();
        
       $("#txtBookedDateFrom").data("kendoDatePicker").value(newDate);
       $("#txtBookedDateTo").data("kendoDatePicker").value(newDate);
        $("#txtBookedInvoice").val("");
    }
};