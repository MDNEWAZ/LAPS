


var stockManager = {
    GenerateStockGrid: function (modelId, branchId, stockCategoryId) {

        $("#stockGrid").kendoGrid({
            dataSource: stockManager.gridDataSource(modelId, branchId, stockCategoryId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: stockHelper.ProductStockColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    gridDataSource: function (modelId, branchId, stockCategoryId) {
        var gridData = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Stock/GetAProductStock/?modelId=' + modelId + '&branchId=' + branchId + "&stockCategoryId=" + stockCategoryId,
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
                        Sl: { type: 'number', defaultValue: 0 }
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });

        return gridData;
    },
    InitislProductStock: function () {
        stockHelper.OpenProductStockPopup();
    }
};

var stockHelper = {
    FillStock: function (objProduct) {

    },
    ProductStockColumns: function () {
        return columns = [
            //{ field: "EntryDate", title: "Receive Date", width: 70 },
            //{ field: "AProduct.Model", title: "Model", width: 70 },
            //{ field: "AProduct.ProductName", title: "Name", width: 70 },
              { field: "ItemName", title: "Item Name", width: 70 },
              { field: "ItemId", title: "Item Id", width: 70, hidden: true },
              { field: "Quantity", title: "Quantity", width: 70 },
            //{ field: "Total", hidden: true},
            //{ field: "Status", title: "Status", width: 70, template: "#= Status==0?'InActive':'Active'#" }
           // { field: "Edit", title: "Action", filterable: false, width: 50, template: '<input type="button" class="k-button" value="View" id="btnEdit" onClick="stockHelper.clickEventForViewButton()"/>', sortable: false }
        ];
    },

    //------------------------------Add Stock---------------------------------

    clickEventForAddNewStock: function () {
        var entityGrid = $("#gridProduct").data("kendoGrid");
        var objProduct = entityGrid.dataItem(entityGrid.select());
        if (objProduct != null) {
            stockDetailsHelper.GenerateModelCombo();
            stockHelper.ClearItemsStockForm();
            stockHelper.fillStockDetailsInForm(objProduct);
            StockProductItemsHelper.GenerateStockItemsGrid(0);

            $("#gridStockProductItems").data("kendoGrid").dataSource.data([]);
            $("#productStockDetailsDiv").data("kendoWindow").open().center();

        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select A Product Model.',
                [{
                    addClass: 'btn btn-primary',
                    text: 'Ok',
                    onClick: function ($noty) {
                        $noty.close();
                    }
                }]);
        }
        return true;
    },

    //------------------------------View Stock---------------------------------

    clickEventForViewStock: function () {
        //var modelId = $("#hdnModelId").val();
        var modelId = $('#cmbPackage').data().kendoComboBox.value();
        var stockCategoryId = $("#cmbStockCategory").data().kendoComboBox.value();

        var stockType = $("#StockType input[type='radio']:checked").val();

        if (modelId > 0 && stockCategoryId > 0) {
            var branchId = $('#cmbBranch').data().kendoComboBox.value() == "" ? 0 : $('#cmbBranch').data().kendoComboBox.value();
     
            var packageName = $('#cmbPackage').data().kendoComboBox.text();

            if (stockType == 2) {
                if (branchId == 0) {
                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select Branch',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function($noty) {
                                $noty.close();

                            }
                        }]);
                } else {
                    $("#stockDetailsPopupWindo").data("kendoWindow").open().center();
                    $("#lblModelName").text(packageName);
                    stockManager.GenerateStockGrid(modelId, branchId, stockCategoryId);
                }
            } else {
                $("#stockDetailsPopupWindo").data("kendoWindow").open().center();
                $("#lblModelName").text(packageName);
                stockManager.GenerateStockGrid(modelId, branchId, stockCategoryId);
            }

     
            
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select a Package & Stock Category',
               [{
                   addClass: 'btn btn-primary',
                   text: 'Ok',
                   onClick: function ($noty) {
                       $noty.close();
                   }
               }]);
        }

    },

    //------------------------------View Stock details---------------------------------

    clickEventForViewButton: function () {

        $("#stockItemsDetailsPopupWindow").data("kendoWindow").open().center();
        var entityGrid = $("#stockGrid").data("kendoGrid");
        var objStock = entityGrid.dataItem(entityGrid.select());
        StockItemsDetailsHelper.GenerateStockItemsDetailsGrid(objStock.ItemId);
    },

    //------------------------------ Stock Adjustment---------------------------------

    clickEventForStockAdjustment: function () {

        var res = stockAdjustmentManager.CheckStock();
        if (res) {
            var branchId = $('#cmbBranch').data().kendoComboBox.value();
            branchId = branchId == "" ? 0 : branchId;
            $("#hdnBranchId").val(branchId);

            //if (CurrentUser.CompanyStock == 1) {
            //    $("#lblBranchName").hide();
            //    $("#txtBranchName").hide();
            //}
            var modelId = $('#cmbPackage').data().kendoComboBox.value();
            $("#hdnModelIdStock").val(modelId);
            stockAdjustmentHelper.GenerateStockAdjustmentGrid(0);
            $("#gridStockAdjustment").data("kendoGrid").dataSource.data([]);
            stockManagerHelper.openStockAdjustmentPopup();
            $("#divProductStockAdjustment").data("kendoWindow").open().center();

            $("#cmbModelStockAdjustment").val($('#cmbPackage').data().kendoComboBox.text());
            $("#txtBranchName").val($('#cmbBranch').data().kendoComboBox.text());

        }
    },


    OpenProductStockPopup: function () {
        $("#stockDetailsPopupWindo").kendoWindow({
            title: "Product Items Stock Details",
            resizable: false,
            modal: true,
            width: 600,
            draggable: true,
            open: function (e) {
                this.wrapper.css({ top: 50 });
            }
        });

        $("#productStockDetailsDiv").kendoWindow({
            title: "Add Product Stock",
            resizable: false,
            modal: true,
            width: "60%",
            draggable: true,
            open: function (e) {
                this.wrapper.css({ top: 50 });
            }
        });

        $("#stockItemsDetailsPopupWindow").kendoWindow({
            title: "Stock Details",
            resizable: false,
            modal: true,
            width: "60%",
            draggable: true,
            open: function (e) {
                this.wrapper.css({ top: 50 });
            }
        });

        $("#productStockAdjustmentDiv").kendoWindow({
            title: "Stock Details",
            resizable: false,
            modal: true,
            width: "60%",
            draggable: true,
            open: function (e) {
                this.wrapper.css({ top: 50 });
            }
        });
    },

    OpenStockPopup: function () {
        $("#stockDetailsPopup").kendoWindow({
            title: "Collection Details",
            resizeable: false,
            width: "60%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
            content: "../Stock/StockPartial",
        });
    },
    fillStockDetailsInForm: function (objProduct) {
        $('#cmbModelStock').data().kendoComboBox.value(objProduct.ModelId);
        $("#txtNameStock").val(objProduct.ProductName);
        $("#txtQuantityStock").val(0);
        if (objProduct.IsActive == "1") {
            $("#chkIsActiveStock").prop('checked', 'checked');
        } else {
            $("#chkIsActiveStock").removeProp('checked', 'checked');
        }
    },
    ClearItemsStockForm: function () {

        $('#hdnModelIdStock').val('');
        $("#cmbModelStock").data("kendoComboBox").value("");
        $("#txtNameStock").val('');
        $("#txtQuantityStock").val(0);
        $("#chkIsActiveStock").removeProp('checked', 'checked');

    },

    ClearStockForm: function () {
        $('#hdnModelIdStock').val('');
        $("#cmbModelStock").data("kendoComboBox").value("");
        $("#txtNameStock").val('');
        var grid = $("#gridStockProductItems").data("kendoGrid");
        if (grid != undefined) {
            $("#gridStockProductItems").data("kendoGrid").dataSource.data([]);
        }

    },

    ClearStockReceiveFields: function () {
        $('#hdnModelIdStock').val('');
        $('#hdnFlagStock').val('');
        $("#cmbPackage").data("kendoComboBox").value("");

        $("#txtQty").data('kendoNumericTextBox').value('');

        $("#gridStockProductItems").data("kendoGrid").dataSource.data([]);
    }
};
