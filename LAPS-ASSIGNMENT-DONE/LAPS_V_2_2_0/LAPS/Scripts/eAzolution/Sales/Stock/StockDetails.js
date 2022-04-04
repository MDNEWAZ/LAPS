var gbStockItemsArray = [];

var stockDetailsManager = {

    UpdateStock: function () {

        var model = "";
        var packageWiseOrItemWise = $("#StockBy input[type='radio']:checked").val();
        if (packageWiseOrItemWise == 1) {
            var qty = $("#txtQty").val();
            model = $("#cmbPackage").val();

            if (model != "") {
                if (qty != "") {
                    AjaxManager.MsgBox('information', 'center', 'Confirm Update:', 'Are You Confirm to Update Stock ? ',
                       [{
                           addClass: 'btn btn-primary',
                           text: 'Yes',
                           onClick: function ($noty) {
                               $noty.close();
                               stockDetailsManager.SaveStock();
                           }
                       },
                       {
                           addClass: 'btn',
                           text: 'No',
                           onClick: function ($noty) {
                               $noty.close();

                           }
                       }]);

                } else {
                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Enter Package Quantity!',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();

                            }
                        }]);
                }

            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select a Package !',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();

                        }
                    }]);
            }
        } else {
            model = $("#cmbPackage").val();
            if (model != "") {
                stockDetailsManager.SaveStock();

            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select a Package !',
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

    SaveStock: function () {

        if (true) {
            var stockItemList = stockDetailsHelper.CreateStockItemList();

            if (stockItemList.length != 0) {
                var objStockItemList = JSON.stringify(stockItemList);
                var branchId = $("#cmbBranch").data('kendoComboBox').value() == "" ? 0 : $("#cmbBranch").data('kendoComboBox').value();
          
                var jsonParam = 'objStockItemList:' + objStockItemList +",branchId:"+branchId;
                var serviceUrl = "../Stock/SaveStock/";
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
                AjaxManager.MsgBox('success', 'center', 'Success', 'Stock Saved Successfully.',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                          stockHelper.ClearStockReceiveFields();
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



    GetProductType: function () {
        var objProduct = "";
        var jsonParam = "";
        var serviceUrl = "../Product/GetProductType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objProduct = jsonData;
        }
        //function onFailed(error) {
        //    window.alert(error.statusText);
        //}
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objProduct;
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


    GetDataForPackageCombo: function () {
        var objPackage = "";
        var packageType = 0;
        var jsonParam = "packageType=" + packageType;
        var serviceUrl = "../Product/GetAllPackageByCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objPackage = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objPackage;
    },

    ProductInfoFillByModel: function () {
        var modelId = $('#cmbModelStock').data().kendoComboBox.value();
        $("#gridStockProductItems").data("kendoGrid").dataSource.data([]);

        var jsonParam = "modelId=" + modelId;
        var serviceUrl = "../Product/GetAProductModel";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            if (jsonData != null) {
                stockHelper.fillStockDetailsInForm(jsonData);
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

    GetProductModelbyTypeIdAndPackageId: function (typeId, packageType) {
        var objModel = "";
        var jsonParam = "typeId=" + typeId + "&packageType=" + packageType;
        //var serviceUrl = "../Product/GetAllProdeuctModel/";
        var serviceUrl = "../Product/GetAllPackageByTypeIdAndPackageId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objModel = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objModel;
    },

};
var stockDetailsHelper = {

    InitStockDetails: function () {
        $("#txtDeliveryDate").kendoDatePicker();

        stockDetailsHelper.PopulateStockCategoryCombo();

        stockDetailsHelper.GenerateProductCombo();
        var objProduct = stockDetailsManager.GetProductType();
        $("#cmbType").data('kendoComboBox').setDataSource(objProduct);
    },

    CreateStockObject: function () {
        var objStock = new Object();
        objStock.AProduct = { ModelId: $('#cmbModelStock').val() };

        objStock.Quantity = $("#txtQuantityStock").val();
        objStock.Status = $("#chkIsActiveStock").is(":checked") == true ? 1 : 0;
        return objStock;
    },

    CreateStockItemList: function () {

        gbStockItemsArray = [];

        var gridProductItems = $("#gridStockProductItems").data("kendoGrid");
        var gridData = gridProductItems.dataSource.data();
        var packageWiseOrItemWise = $("#StockBy input[type='radio']:checked").val();
        var branchId = $("#cmbBranch").data('kendoComboBox').value();
        branchId = branchId == "" ? 0 : branchId;
        for (var i = 0; i < gridData.length; i++) {
            var items = gridData[i];

            var objStockitems = new Object();

            if (packageWiseOrItemWise == 1) {
                objStockitems.StockId = items.StockId == "" ? 0 : items.StockId;
                objStockitems.ItemId = items.ItemId;



                var quantity = $("#txtQty").val();
                var itemQuantity = items.BundleQuantity * quantity;
                objStockitems.Quantity = itemQuantity == "" ? 0 : itemQuantity;
                objStockitems.ReceiveDate = new Date();
            }
            else if (packageWiseOrItemWise == 2) {
                objStockitems.StockId = items.StockId == "" ? 0 : items.StockId;
                objStockitems.ItemId = items.ProductItems.ItemId;

                objStockitems.Quantity = items.Quantity == "" ? 0 : items.Quantity;
                objStockitems.ReceiveDate = new Date();
            }

            objStockitems.ModelId = $('#cmbPackage').val();
            objStockitems.BranchId = branchId;

            objStockitems.DeliveryChalanNo = $("#txtDeliveryChalanNo").val();
            objStockitems.DeliveryOrderNo = $("#txtDeliveryOrderNo").val();
            objStockitems.DeliveryDate = $("#txtDeliveryDate").val();
            objStockitems.QBInvoiceNo = $("#txtQBInvoiceNo").val();
            objStockitems.StockCategoryId = $("#cmbStockCategory").val();

            gbStockItemsArray.push(objStockitems);
        }
        return gbStockItemsArray;
    },

    GenerateModelCombo: function () {
        var objModel = stockDetailsManager.GetProductModel();
        $("#cmbModelStock").kendoComboBox({
            placeholder: "Select Model",
            dataTextField: "Model",
            dataValueField: "ModelId",
            dataSource: objModel,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem("cmbModelStock", true);
            }
        });
    },
    FillProductInfoByModel: function (objProduct) {
        //productInfoHelper.clearProductForm();
        $('#hdnModelIdProInfo').val(objProduct.ModelId);
        $('#txtCodeProInfo').val(code);
        $('#txtNameProInfo').val(objProduct.ProductName);
        $("#txtManuDateProInfo").val(objProduct.ManufactureDate);
    },
    GeneratePackageCombo: function (data) {
        //var objModel = stockDetailsManager.GetDataForPackageCombo();
        var objModel =data;
        $("#cmbPackage").kendoComboBox({
            placeholder: "Select Model",
            dataTextField: "Model",
            dataValueField: "ModelId",
            dataSource: objModel,
            suggest: true,
            change: function () {
                var modelId = $("#cmbPackage").data('kendoComboBox').value();
                //StockProductItemsHelper.GenerateStockItemsGrid(modelId);
                var packageWiseOrItemWise = $("#StockBy input[type='radio']:checked").val();
                if (packageWiseOrItemWise == 1) {//1 mean Package Wise
                    StockProductItemsHelper.GenerateStockItemsGridWithData(modelId);
                }
                else if (packageWiseOrItemWise == 2) {//2 means item wise
                    StockProductItemsHelper.GenerateStockItemsGrid(modelId);
                }


            }
        });
    },

    PopulateStockCategoryCombo: function () {
        $("#cmbStockCategory").kendoComboBox({
            placeholder: "Select Model",
            dataTextField: "StockCategoryName",
            dataValueField: "StockCategoryId",
            dataSource: [{ StockCategoryName: 'Sale', StockCategoryId: '1' }, { StockCategoryName: 'Replacement', StockCategoryId: '2' }],
            filter: "contains",
            suggest: true
        });
    },

    ClearStockDetails: function () {
        $("#txtDeliveryChalanNo").val("");
        $("#txtDeliveryOrderNo").val("");
        $("#txtDeliveryDate").val("");
        $("#txtQBInvoiceNo").val("");
        $("#cmbStockCategory").data("kendoComboBox").value("");
        $('#cmbPackage').data("kendoComboBox").value("");
        $("#txtQty").data("kendoNumericTextBox").value("");
        
        var grid = $("#gridStockProductItems").data("kendoGrid");
        if (grid != undefined) {
            $("#gridStockProductItems").data("kendoGrid").dataSource.data([]);
        }


        $("#stockDetailsDiv > form").kendoValidator();
        $("#stockDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");
        status.text("").removeClass("invalid");
        $("#cmbBranch").data('kendoComboBox').value("");
        $("#liBranch").hide();
        $("#companyWise").attr("checked", true);
        $("#branchWise").attr("checked", false);
    },

    GenerateProductCombo: function () {

        $("#cmbType").kendoComboBox({
            placeholder: "Select Product...",
            dataValueField: "TypeId",
            dataTextField: "Type",
            //dataSource: objProduct,
            filter: "contains",
            suggest: true,
            change: function () {
                var typeId = this.value();
                var packageWiseOrItemWise = $("#StockBy input[type='radio']:checked").val();
                var data = stockDetailsManager.GetProductModelbyTypeIdAndPackageId(typeId, packageWiseOrItemWise);
                $("#cmbPackage").data("kendoComboBox").value("");
                stockDetailsHelper.GeneratePackageCombo(data);
            }
        });
    },
}