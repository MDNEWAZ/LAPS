
var gbStockAdjustmentArray = [];
var gbExistStockOfItem;
var adjustmentType = 0;

var stockAdjustmentManager = {
    SaveStockAdjustment: function () {
        if (true) {
            var stockAdjustmentList = stockAdjustmentHelper.CreateStockAdjustmentList();
            var stockCategoryId = $("#cmbStockCategory").data().kendoComboBox.value();
         
            if (stockAdjustmentList.length != 0) {
                var objStockAdjustmentList = JSON.stringify(stockAdjustmentList).replace('&', '^');
                var branchId = $("#cmbBranch").data('kendoComboBox').value() == "" ? 0 : $("#cmbBranch").data('kendoComboBox').value();
                
                var jsonParam = 'objStockAdjList:' + objStockAdjustmentList + ",stockCategoryId:" + JSON.stringify(stockCategoryId) + ",branchId:" + branchId;
                var serviceUrl = "../Stock/SaveStockAdjustment/";
                AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select Items',
               [{
                   addClass: 'btn btn-primary',
                   text: 'Ok',
                   onClick: function ($noty) {
                       $noty.close();
                   }
               }]);
            }

        }
        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                //productDetailsHelper.clearProductForm();
                AjaxManager.MsgBox('success', 'center', 'Success', 'Stock Adjust Successfully.',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                          stockAdjustmentHelper.ClearStockAdjustmentForm();
                      }
                  }]);
                //$("#gridProduct").data("kendoGrid").dataSource.read();
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();
                           }
                       }]);
            }
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    GetProductModel: function () {
        var objModel = "";
        var jsonParam = "";
        var serviceUrl = "../Product/GetAllProdeuctModel/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objModel = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objModel;
    },

    StockAdjustmentByModel: function () {
        var modelId = $('#cmbModelStockAdjustment').data().kendoComboBox.value();
        $("#gridStockAdjustment").data("kendoGrid").dataSource.data([]);

        var jsonParam = "modelId=" + modelId;
        var serviceUrl = "../Product/GetAProductModel";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            if (jsonData != null) {
                stockAdjustmentHelper.fillStockAdjustmentForm(jsonData);
            }
            else {
                AjaxManager.MsgBox('warning', 'center', 'Warning', 'Data Not Found!!!',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();

                         }
                     }]);
            }


        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
    },

    gridDataSource: function (modelId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    // url: '../Stock/GetAllStockModelId/?modelId=' + modelId,

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

                        ProductItems: { defaultValue: { ItemId: 0, ItemName: "--Select--" } },
                        AdjustmentType: {
                            defaultValue: { AdjustmentTypeId: 0, AdjustmentTypeName: "--Select--" },
                            validation: { required: true }
                        },
                        Quantity: {
                            type: "number", validation: { required: true, min: 0 },

                        },
                        StockId: {
                            type: "number",
                        },
                        //ReceiveDate: {
                        //    type: "date",
                        //    template: '#= kendo.toString("MM/dd/yyyy") #'
                        //},
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });


        return gridDataSource;


    },

    GetProductItems: function () {
      
        //var modelId = $('#hdnModelId').val();
        var branchId = $('#cmbBranch').data().kendoComboBox.value();
        branchId = branchId == "" ? 0 : branchId;
        var modelId = $('#cmbPackage').data().kendoComboBox.value();
        if (modelId > 0) {
            var objCcType = "";
            var jsonParam = "";
            var serviceUrl = "../Product/GetStockedProductItemByModelId/?modelId=" + modelId + "&branchId=" + branchId;
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select A Product Model.',
               [{
                   addClass: 'btn btn-primary',
                   text: 'Ok',
                   onClick: function ($noty) {
                       $noty.close();
                       //$("#gridStockProductItems").data("kendoGrid").dataSource.data([]);
                   }
               }]);
        }


        function onSuccess(jsonData) {
            if (jsonData == "") {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Out of Stock !',
               [{
                   addClass: 'btn btn-primary',
                   text: 'Ok',
                   onClick: function ($noty) {
                       $noty.close();
                   }
               }]);
            }
            objCcType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCcType;
    },

    CheckStock: function () {
        var res = false;
        //var modelId = $("#hdnModelId").val();
        var branchId = $('#cmbBranch').data().kendoComboBox.value();
        branchId = branchId == "" ? 0 : branchId;
        var modelId = $('#cmbPackage').data().kendoComboBox.value();
        var stockCategoryId = $("#cmbStockCategory").data().kendoComboBox.value();
        if (modelId > 0 && stockCategoryId>0) {
            var jsonParam = "modelId=" + modelId + "&branchId=" + branchId + "&stockCategoryId=" + stockCategoryId;
            var serviceUrl = "../Stock/GetAProductStock";
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please Select a Package & Stock Category !',
               [{
                   addClass: 'btn btn-primary',
                   text: 'Ok',
                   onClick: function ($noty) {
                       $noty.close();
                       res = false;
                   }
               }
               ]);
        }
      
        function onSuccess(jsonData) {

            if (jsonData.TotalCount == 0) {
                AjaxManager.MsgBox('warning', 'center', 'Warning', 'You have no Stock to adjustment !',
                [ {
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            res = false;
                        }
                    }
                ]);
            } else {
                res = true;
            }
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }

        return res;
    }
};


var stockAdjustmentHelper = {

    CreateStockObject: function () {
        var objStock = new Object();
        objStock.AProduct = { ModelId: $('#cmbModelStock').val() };

        objStock.Quantity = $("#txtQuantityStock").val();
        objStock.Status = $("#chkIsActiveStock").is(":checked") == true ? 1 : 0;
        return objStock;
    },

    CreateStockAdjustmentList: function () {

        gbStockAdjustmentArray = [];

        var gridAdjustmentItems = $("#gridStockAdjustment").data("kendoGrid");
        var gridData = gridAdjustmentItems.dataSource.data();
        
        var branchId = $("#hdnBranchId").val();;
        branchId = branchId == "" ? 0 : branchId;
        
        
        for (var i = 0; i < gridData.length; i++) {
            var items = gridData[i];

            var objStockAdj = new Object();
            objStockAdj.StockId = items.StockId == "" ? 0 : items.StockId;
            objStockAdj.ItemId = items.ProductItems.ItemId;
            objStockAdj.ModelId = $("#hdnModelIdStock").val();
            objStockAdj.AdjustmentQuantity = items.Quantity == "" ? 0 : items.Quantity;
            objStockAdj.AdjustmentTypeId = items.AdjustmentType.AdjustmentTypeId;
            objStockAdj.Description = items.Description;
            objStockAdj.BranchId = branchId;
            gbStockAdjustmentArray.push(objStockAdj);
        }
        return gbStockAdjustmentArray;
    },

    GenerateModelCombo: function () {
        var objModel = stockDetailsManager.GetProductModel();
        $("#cmbModelStockAdjustment").kendoComboBox({
            placeholder: "Select Model",
            dataTextField: "Model",
            dataValueField: "ModelId",
            dataSource: objModel,
            suggest: true,
            change: function () {
                AjaxManager.isValidItem("cmbModelStockAdjustment", true);
            }
        });
    },


        //fillStockAdjustmentForm: function (objProduct) {
        //    $('#cmbModelStockAdjustment').data().kendoComboBox.value(objProduct.ModelId);
        //    $("#txtNameStockAdj").val(objProduct.ProductName);

        //},
    ClearStockAdjustmentForm: function () {
        $('#hdnModelIdStock').val('');
        //$("#cmbModelStockAdjustment").data("kendoComboBox").value("");
        $("#cmbModelStockAdjustment").val('');
        $("#txtNameStockAdj").val('');

        $("#gridStockAdjustment").data("kendoGrid").dataSource.data([]);


    },
    //ClearStockAdjForm: function () {
    //    $('#hdnModelIdStock').val('');
    //    $("#cmbModelStockAdjustment").data("kendoComboBox").value("");
    //    $("#txtNameStockAdj").val('');


    //},


    GenerateStockAdjustmentGrid: function (modelId) {

        var objItems = stockAdjustmentManager.gridDataSource(modelId);

        $("#gridStockAdjustment").kendoGrid({
            dataSource: objItems,
            pageable: false,
            toolbar: ["create"],
            //toolbar: ["create", { template: kendo.template($("#template").html()) }],
            columns: [
                { field: "ItemId", width: "100px", hidden: true },
                { field: "ProductItems", title: "Item Name", width: "60px", editor: stockAdjustmentHelper.ProductItemDropDownEditor, template: "#=ProductItems.ItemName#" },
                //{ field: "ReceiveDate", title: "Receive Date", width: "60px", format: "{0:MM/dd/yyyy}" },
                { field: "AdjustmentType", title: "Adjustment Type", width: "50px", editor: stockAdjustmentHelper.AdjustmentTypeDropDownEditor, template: "#=AdjustmentType.AdjustmentTypeName#" },
                { field: "Quantity", title: "Adjustment Quantity", width: "50px", editor: stockAdjustmentHelper.AdjustmentQtyNumericTextBoxEditor },
                { field: "Description", title: "Reason", width: "50px", },
              //{ command: "destroy", title: "Action", width: "60px" },
              //{ command: "destroy", title: "Action", width: "40px", template: '<input type="button" class="k-button" id="btnDelete" onClick="EmploymentSummaryHelper.deleteRow()"/>' },
             // { field: "Edit", title: "Delete", filterable: false, width: 70, template: '<input type="button" class="k-button" value="Delete" id="btnDelete" onClick="ProductItemHelper.deleteRow()"/>', sortable: false }
            ],
            editable: {
                confirmation: false
            },

            selectable: true,


        });
    },

    ProductItemDropDownEditor: function (container, options) {
       
        $('<input required data-text-field="ItemName" data-value-field="ItemId" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                optionLabel: '--Select--',
                dataSource: stockAdjustmentManager.GetProductItems(),
                placeholder: "Please Select",

                change: function () {
                    var itemId = this.value();
                 
                    stockAdjustmentHelper.checkExistStockBalance(itemId);
                    options.model.Quantity = 0;
                    
                }
            });
    },

    AdjustmentTypeDropDownEditor: function (container, options) {
        
        $('<input required data-text-field="AdjustmentTypeName" data-value-field="AdjustmentTypeId" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                optionLabel: '--Select--',
                dataSource: [
                   { AdjustmentTypeName: "Addition", AdjustmentTypeId: "1" },
                   { AdjustmentTypeName: "Deduction", AdjustmentTypeId: "2" }
                ],
                placeholder: "Please Select",
                change: function () {
                    adjustmentType = this.value();
                    options.model.Quantity = 0;
                     
                }
            });
    },

    AdjustmentQtyNumericTextBoxEditor: function (container, options) {
        $('<input required type="text" value=0" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                autoBind: false,
                placeholder: "Please Select",
                change: function (e) {
                    if (adjustmentType == 2) {
                        var stockItem = gbExistStockOfItem.StockBalanceQty;
                        if (this.value() > stockItem) {
                            options.model.Quantity = 0;
                            e.preventDefault();
                            AjaxManager.MsgBox('warning', 'center', 'Warning', 'Cross Stock Balance!',
                                    [{
                                        addClass: 'btn btn-primary',
                                        text: 'Ok',
                                        onClick: function ($noty) {
                                            $noty.close();

                                        }
                                    }
                                     ]);
                        }
                    }
                }
            });
    },

   

    checkExistStockBalance: function (itemId) {
   
        var stockCategoryId = $("#cmbStockCategory").data().kendoComboBox.value();
        var branchId = $("#cmbBranch").data('kendoComboBox').value() == "" ? 0 : $("#cmbBranch").data('kendoComboBox').value();
        gbExistStockOfItem = "";
        var jsonParam = "";
        var serviceUrl = "../Stock/checkExistStockBalanceByItemId/?itemId=" + itemId + "&stockCategoryId=" + stockCategoryId + "&branchId=" + branchId;
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
          
            gbExistStockOfItem = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return gbExistStockOfItem;
    }

}