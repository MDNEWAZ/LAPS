
var gbSelectiveDraftedSaleArray = [];

var saleSummaryManager = {
    InitSalesSummary: function () {
        SaleSummaryHelper.GenerateDatePicker();
        
        SaleSummaryHelper.DraftedSaleGridChangeEvent();
        
        $("#btnFinalSubmit").click(function () {

            saleDetailsManager.SubmitSalesFinnaly();
        });
        
        $("#cmbFromDate").change(function () {
            saleSummaryManager.SearchSalesSummaryByParam();
        });
        
        $("#cmbToDate").change(function () {
            saleSummaryManager.SearchSalesSummaryByParam();
        });
        $("#txtInvoice").change(function () {
            saleSummaryManager.SearchSalesSummaryByParam();
        });

      
    },
    
    GenerateSaleGrid: function () {
        $("#gridSale").kendoGrid({
            dataSource: saleSummaryManager.gridDataSource(),
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

                    if (dataView[i].IsDownPayCollected === 0) {
                        $("#gridSale tbody").find("tr[data-uid=" + uid + "]").css("background-color", "#ED3749");
                        $("#gridSale tbody").find("tr[data-uid=" + uid + "]").css("color", "#0D0C0C");
                    }
                    if (dataView[i].IsDownPayCollected === 1) {
                        $("#gridSale tbody").find("tr[data-uid=" + uid + "]").css("background-color", "#89E381");
                        $("#gridSale tbody").find("tr[data-uid=" + uid + "]").css("color", "#0D0C0C");
                    }

                }
            },
            
            filterable: false,
            sortable: true,
            columns: SaleSummaryHelper.GenerateSaleColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
            scrollable:true
        });
    },
    
    gridDataSource: function () {
        var entryDateFrom = $("#cmbFromDate").val();
        var entryDateTo = $("#cmbToDate").val();
        var invoiceNo = $("#txtInvoice").val();
        
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: false,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
           // pageSize: 10,
            transport: {
                read: {
                    url: '../Sale/GetAllSaleByMonth/?invoiceNo=' + invoiceNo + "&entryDateFrom=" + entryDateFrom + "&entryDateTo=" + entryDateTo,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                parameterMap: function(options) {
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

    SearchSalesSummaryByParam: function () {
       
        var data = saleSummaryManager.gridDataSource();
        var grid = $("#gridSale").data("kendoGrid");
        grid.setDataSource(data);
        
    },
};


var SaleSummaryHelper = {
    GenerateDatePicker: function () {
        $("#cmbFromDate").kendoDatePicker({
            //start: "year", depth: "year", format: "MMM/yyyy", 
            value: new Date()
        });
        $("#cmbToDate").kendoDatePicker({
            value: new Date()
        });
    },
    GenerateSaleColumns: function () {
        return columns = [
            { field: "check_row", title: "", width: 30, template: "<input class='check_row' type='checkbox' />", headerTemplate: "<input type='checkbox' id='chkSelectAll'/>", filterable: false, sortable: false },
            { field: "Invoice", title: "Invoice No", width: 60 },
            { field: "WarrantyStartDate", title: "Sales Date", width: 55, template: '#=kendo.toString(WarrantyStartDate,"dd/MM/yyyy")#' },
            { field: "FirstPayDate", title: "First Pay Date", width: 55, template: '#=kendo.toString(FirstPayDate,"dd/MM/yyyy")#' },
            { field: "ACustomer.CustomerCode", title: "Customer ID", width: 70 },
            { field: "ACustomer.Name", title: "Customer Name", width: 80 },
            { field: "ACustomer.BranchId", title: "BranchId", width: 50, hidden: true },
            { field: "ACustomer.BranchCode", title: "Branch<br/> Code", width: 40 },
            { field: "ACustomer.Phone2", title: "SMS Mob No", width: 65 },
           // { field: "AProduct.ProductNo", title: "Product Code", width: 70 },
            { field: "AProduct.ProductName", title: "Product Name", width: 90 },
            { field: "AProduct.Model", title: "Model", width: 50 },
            { field: "Price", title: "Price", width: 50 },
          //  { field: "DownPay", title: "Down Pay", width: 50 },
            { field: "ReceiveAmount", title: "Received Amount", width: 50 },
            { field: "Installment", title: "Inst. No", width: 40 },
            { field: "ProductNo", hidden: true },
            { field: "CustomerId", hidden: true },
            { field: "SaleId", hidden: true },
            { field: "Flag", hidden: true },
            { field: "State", hidden: true },
            { field: "TempState", hidden: true },
            { field: "IsActive", hidden: true },
            { field: "ADiscount.DiscountTypeId", hidden: true },
            { field: "View", title: "View Details", filterable: false, width: 50, template: '<input type="button" class="k-button" value="View" id="btnEdit" onClick="SaleSummaryHelper.clickEventForEditButton()"/>', sortable: false },
            { field: "Edit", title: "Unrecognize", filterable: false, width: 60, template: '<input type="button" class="k-button" value="Make Unrecognized" id="btnUnrecognize" onClick="SaleSummaryHelper.clickEventForUnrecognizeButton()"/>', sortable: false }
        ];
    },
    
    clickEventForEditButton: function () {
     
        var entityGrid = $("#gridSale").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            $("#btnSaveAsBook").hide();
            $("#saveAsDraftedSaleSummaryDiv").hide();
            $("#saleDetailsMainDiv").show();

            if (selectedItem.IsActive == 1) {
                $("#btnSaveAsBook").hide();
                $("#btnSave").hide();
                $("#btnClearAll").hide();
            } else {
              
                if (gbIsViewer != 1) {
                    $("#btnFinalSubmit").show();
                }
                $("#btnSave").show();
                $("#btnClearAll").show();
            }

         
            saleDetailsHelper.ClearAllSaleDetailsForm();
            $("#txtInvoiceSale").val(selectedItem.Invoice);
            
            ProductItemInfoHelper.GenerateProductItemInfoGrid(0);
            
            productInfoManager.ProductCustomerInfoFill(selectedItem.Invoice, selectedItem.SaleId);

            saleDetailsManager.GetDownPaymentCollectionInfo(selectedItem.SaleId);
        }
       
    },
    
    clickEventForUnrecognizeButton:function() {
        var entityGrid = $("#gridSale").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
       
        if (selectedItem != null && gbIsViewer !=1) {
            saleDetailsManager.SaveAsUnrecognized();
            gbUnrecogSaleObj = selectedItem;
            type = "draft";
        }
    },
    
    DraftedSaleGridChangeEvent: function () {

        $('#gridSale').on('change', '.check_row', function (e) {
           var $target = $(e.currentTarget);
            var grid = $("#gridSale").data("kendoGrid");
            var dataItem = grid.dataItem($(this).closest('tr'));
            if ($target.prop("checked")) {
                if (dataItem.Invoice != "") {
                    gbSelectiveDraftedSaleArray.push(dataItem);
                } else {
                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Input Invoice No First.!',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            $("#gridSale tbody tr input:checkbox").removeAttr("checked", true);
                            $('tr.k-state-selected', '#gridSale').removeClass('k-state-selected');

                        }
                    }]);
                }

            } else {
                for (var i = 0; i < gbSelectiveDraftedSaleArray.length; i++) {
                    if (gbSelectiveDraftedSaleArray[i].SaleId == dataItem.SaleId) {
                        gbSelectiveDraftedSaleArray.splice(i, 1);
                        break;
                    }
                }

            }

        });


        $('#gridSale').on('change', '#chkSelectAll', function (e) {
         
            gbSelectiveDraftedSaleArray = [];
            var gridSummary = $("#gridSale").data("kendoGrid");
            var selectAll = document.getElementById("chkSelectAll");
            if (selectAll.checked == true) {
                $("#gridSale tbody input:checkbox").attr("checked", true);
                $("#gridSale table tr").addClass('k-state-selected');
                var gridData = gridSummary.dataSource.data();

                for (var i = 0; i < gridData.length; i++) {
                    var sale = gridData[i];
                    if (sale.Invoice != "") { //escape where invoice no is null
                        gbSelectiveDraftedSaleArray.push(sale);
                    } else {
                        $("#gridSale tbody tr input:checkbox").removeAttr("checked", true);
                        $("#gridSale table tr").removeClass('k-state-selected');
                    }

                }

            } else {
                $("#gridSale tbody input:checkbox").removeAttr("checked", this.checked);
                $("#gridSale table tr").removeClass('k-state-selected');
                gbSelectiveDraftedSaleArray = [];


            }
        });



    },
    
    ReloadEntryDate: function () {
        var newDate = new Date();
        $("#cmbFromDate").data("kendoDatePicker").value(newDate);
        $("#cmbToDate").data("kendoDatePicker").value(newDate);
        $("#txtInvoice").val("");
    }
};