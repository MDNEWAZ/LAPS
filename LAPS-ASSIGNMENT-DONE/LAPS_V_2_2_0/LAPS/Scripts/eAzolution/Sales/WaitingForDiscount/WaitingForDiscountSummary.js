var WaitingForDiscountSummaryManager = {

    gridDataSource: function () {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    url: '../WaitingForDiscount/GetWaitingForDiscountSummary/',

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
                        WarrantyStartDate: { type: "date", template: "formate:'dd/MM/yyyy'" },
                        DiscountAmount: { type: "number", },

                        DiscountedAmount: { DiscountedAmount: "text", editable: false, defaultValue: { DiscountedAmount: "" } },
                    },
                    Edit: {
                      
                        editable: false
                    },
                    Price: {
                        type: "number",
                        editable: false
                    },
                    CustomerName: {
                        type: "text",
                        editable: false
                    },
                    CustomerCode: {
                        type: "text",
                        editable: false
                    },
                    BranchCode: {
                        type: "text",
                        editable: false
                    },
                }, data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    }
};
var WaitingForDiscountSummaryHelper = {

    InitWaitingForDiscount:function() {
        DiscountCustomerInfoManager.GenerateSalesInfoGrid();
        WaitingForDiscountSummaryHelper.FillWaitingForDiscountGrid();
    },

    GenerateWaitingForDiscountGrid: function () {
        $("#gridWaitingForDiscount").kendoGrid({
            dataSource:[],// WaitingForDiscountSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
           
            filterable: true,
            sortable: true,
            columns: WaitingForDiscountSummaryHelper.GenerateWaitingForDiscountColumns(),
            editable: true,
            navigatable: true,
            selectable: "row",
        });
    },

    FillWaitingForDiscountGrid:function() {
        var dataSource = WaitingForDiscountSummaryManager.gridDataSource();
        $("#gridWaitingForDiscount").data().kendoGrid.setDataSource(dataSource);
    },

    GenerateWaitingForDiscountColumns: function () {
        return columns = [
             { field: "SaleId", title: "SaleId", width: 50, hidden: true },
            { field: "Invoice", title: "Invoice", width: 45, hidden: false },
            { field: "CustomerCode", title: "Customer<br/> Code", width: 45, hidden: false },
            { field: "CustomerName", title: "Customer Name", width: 90, hidden: false },
            { field: "BranchId", title: "BranchId", hidden: true },
            { field: "BranchCode", title: "Branch<br/>Code", width: 30 },
            { field: "SalesRepId", title: "Sales Rep Id", width: 40, hidden: false },
            { field: "WarrantyStartDate", title: "Sale Date", width: 40, hidden: false, template: "#= kendo.toString(kendo.parseDate(WarrantyStartDate,'dd/MM/yyyy'),'dd/MM/yyyy') #" },
            { field: "Price", title: "Price", width: 35, hidden: false },
            { field: "DiscountOptionId", title: "Discount <br/>Type", width: 50, template: "#=DiscountOptionId==1?'Fixed Amount':'Percentage Wise'#" },
            { field: "CustomerId", title: "Customer Id", hidden: true },
            { field: "IsActive", title: "Is Active", hidden: true },
            { field: "CompanyId", title: "Company Id", hidden: true },
            { field: "IsSpecialDiscount", title: "Is Special Discount", width: 50, hidden: true },
            { field: "IsDPFixedAmount", title: "IsDPFixedAmount", width: 50, hidden: true },
            { field: "StaffId", title: "StaffId", width: 50, hidden: true },
            { field: "DiscountAmount", title: "Discount <br/> Amount/(%)", width: 60, hidden: false, editor: WaitingForDiscountSummaryHelper.discountAmountTextBox },
            { field: "check_rowForIsPercentage", title: "Is Percentage", width: 40,filterable: false, sortable: false, template: '#= WaitingForDiscountSummaryHelper.checkedDataForIsPercentage(data) #' },//, headerTemplate: '<input type="checkbox" id="checkAllForCompany" />' , template: '#= WaitingForDiscountSummaryHelper.checkedDataForIsPercentage(data) #' 
            { field: "DiscountedAmount", title: "Discounted <br/>Amount", width: 40, hidden: false },//, template: '#= WaitingForDiscountSummaryHelper.checkedDataForDiscountedAmount(data) #'
            
            { field: "Edit", title: "Edit",  width: 70, template: '<input type="button" class="k-button" value="View" id="btnView" onClick="WaitingForDiscountSummaryHelper.clickEventForViewButton()"  /><input type="button" class="k-button" value="Approve" id="btnApprove" onClick="WaitingForDiscountSummaryHelper.clickEventForEditButton()"  /> ', sortable: false,filterable: false }
        ];

    },


    clickEventForEditButton: function () {

        var entityGrid = $("#gridWaitingForDiscount").data("kendoGrid");
        var dpApplicableStage = $("#dpApplicableStage input[type='radio']:checked").val();
        if (dpApplicableStage > 0) {
            var selectedItem = entityGrid.dataItem(entityGrid.select());
            if (selectedItem != null && gbIsViewer != 1) {
                WaitingForDiscountDetailsManager.SaveWaitingForDiscountInformation(selectedItem);
            }

        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please Select When Discount Will Be Applicable!',
                   [{
                       addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                           $noty.close();
                       }
                   }]);
        }


    },

    clickEventForViewButton: function () {

        $("#divCustomerInfo").data("kendoWindow").open().center();
        var entityGrid = $("#gridWaitingForDiscount").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());

      
        if (selectedItem != null) {
            DiscountCustomerInfoHelper.FillCustomerSalesDetails(selectedItem);
        }

    },

    discountAmountTextBox: function (container, options) {

        $('<input required type="text" id="txtItemId"' + options.model.ItemId + '"   min="0" value="0" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                autoBind: false,
                placeholder: "Please Select",
                // format: "p0",
                //format: "# \\%",
                change: function (e) {
                    var grid = $('#gridWaitingForDiscount').data('kendoGrid');

                    var isChecked = $("#check_rowForIsPercentage" + options.model.CustomerId).is(':checked');
                    var price = options.model.Price;
                    var discountedAmt = 0;
                    var discountAmt = options.model.DiscountAmount;

                    if (isChecked == true) {
                        if (discountAmt < 100) {
                            discountedAmt = price - (price * (discountAmt / 100));
                            options.model.DiscountedAmount = Math.round(discountedAmt);
                            grid.refresh();
                            $("#check_rowForIsPercentage" + options.model.CustomerId).prop('checked', true);
                        } else {
                            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Discount percentage can not greater than 100',
                                [{
                                    addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                        $noty.close();
                                        $("#check_rowForIsPercentage" + options.model.CustomerId).prop('checked', false);
                                        options.model.DiscountAmount = 0;
                                        options.model.DiscountedAmount = price;
                                    }
                                }]);
                        }
                    } else {
                        discountedAmt = price - discountAmt;
                        options.model.DiscountedAmount = Math.round(discountedAmt);
                        grid.refresh();
                        $("#check_rowForIsPercentage" + options.model.CustomerId).prop('checked', false);
                    }

                }


            });
    },
    checkedDataForIsPercentage: function (data) {
        return '<input id="check_rowForIsPercentage' + data.CustomerId + '" class="check_rowForIsPercentage" type="checkbox"/>';
    },


    GeRowDataForDiscountGrid: function () {

        $('.check_rowForIsPercentage').live('click', function (e) {

            var $cb = $(this);
            var gridSummary = $("#gridWaitingForDiscount").data("kendoGrid");
            var selectedItem = gridSummary.dataItem(gridSummary.select());//$kgrid.attr('k-state-selected');
            if (selectedItem.DiscountOptionId == 2) {


                var dpApplicableStage = $("#dpApplicableStage input[type='radio']:checked").val();
                if (dpApplicableStage > 0) {
                    if (selectedItem != null) {
                        var discountedAmt = 0;
                        var price = selectedItem.Price;
                        var discountAmt = selectedItem.DiscountAmount; //in this case it will be percentage
                        if ($cb.is(':checked')) {
                            if (selectedItem != null) {
                                if (discountAmt < 100) {

                                    if (dpApplicableStage == 1) { //Before DP
                                        discountedAmt = price - ((price * discountAmt) / 100);
                                        selectedItem.DiscountedAmount = Math.round(discountedAmt);
                                    } else if (dpApplicableStage == 2) { //After DP
                                        // var netPrice = price - ((price * selectedItem.DownPayPercent) / 100);
                                        discountedAmt = price - ((price * discountAmt) / 100);
                                        selectedItem.DiscountedAmount = Math.round(discountedAmt);
                                    }


                                    gridSummary.refresh();

                                    $("#check_rowForIsPercentage" + selectedItem.CustomerId).prop('checked', true);
                                } else {
                                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Discount percentage can not greater than 100',
                                        [{
                                            addClass: 'btn btn-primary',
                                            text: 'Ok',
                                            onClick: function ($noty) {
                                                $noty.close();
                                                $("#check_rowForIsPercentage" + selectedItem.CustomerId).prop('checked', false);
                                            }
                                        }]);

                                }
                            }
                        } else {
                            if (selectedItem != null) {
                                discountedAmt = price - discountAmt;
                                selectedItem.DiscountedAmount = Math.round(discountedAmt);
                                gridSummary.refresh();

                                $("#check_rowForIsPercentage" + selectedItem.CustomerId).prop('checked', false);
                            }
                        }
                    }
                } else {
                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select When Discount will be applicable',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#check_rowForIsPercentage" + selectedItem.CustomerId).prop('checked', false);
                            }
                        }]);
                }


            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Percentage is not applicable',
                       [{
                           addClass: 'btn btn-primary',
                           text: 'Ok',
                           onClick: function ($noty) {
                               $noty.close();
                               $("#check_rowForIsPercentage" + selectedItem.CustomerId).prop('checked', false);
                           }
                       }]);
            }

        });//Indivisual row selection


    },
    ClickEventForDpAbblicableRadio: function () {
       
        $("#dpApplicableStage input[type='radio']").click(function () {
            $('#gridWaitingForDiscount').data('kendoGrid').dataSource.read();
        });
    }
};