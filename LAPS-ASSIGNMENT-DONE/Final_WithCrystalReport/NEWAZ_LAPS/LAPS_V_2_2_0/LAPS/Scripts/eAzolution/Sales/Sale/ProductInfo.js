var code;

var gbProductInfo;

var productInfoManager = {

    InitProductInfo: function () {
      
        $("#txtInvoiceProInfo").keypress(function (event) { if (event.keyCode == 13) { productInfoManager.FillCustomerProductForm(); } });
    },

    GetProductModel: function (packageType) {
        var objModel = "";
        var jsonParam = "packageType=" + packageType;
        //var serviceUrl = "../Product/GetAllProdeuctModel/";
        var serviceUrl = "../Product/GetAllPackageByCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objModel = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objModel;
    },

    OpenItemSelectProductWindow: function () {
        var orderIndex = 'Product Details';
        var kendoOrderWindow = $("<div id='windowSurveyPopup' ></div>").kendoWindow({
            title: "Order " + orderIndex,
            resizable: false,
            modal: true,
            width: "60%",
            draggable: false,
            content: "../Product/ProductPartial",
            open: function (e) {
                this.wrapper.css({ top: 50 });
            },
            close: function (e) {

                this.destroy();
            }
        });
        kendoOrderWindow.data("kendoWindow").open().center();
       
       

    },

    ProductInfoFill: function () {
    
        var productCode = $('#txtCodeProInfo').val();
        var jsonParam = "productCode=" + productCode;
        var serviceUrl = "../Product/GetAProduct";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            if (jsonData != null) {
                productInfoHelper.FillProductInfoInForm(jsonData);
               
            }
            else {
                AjaxManager.MsgBox('warning', 'center', 'Update', 'Data Not Found!!!',
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
    ProductInfoFillByModel: function () {
        var modelId = $('#cmbModelProInfo').data().kendoComboBox.value();
        var jsonParam = "modelId=" + modelId;
        var serviceUrl = "../Product/GetAProductModel";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            if (jsonData != null) {
                productInfoHelper.FillProductInfoByModel(jsonData);
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
    
    FillCustomerProductForm: function (invoiceNo) {
       
        $('#txtInvoiceProInfo').val(invoiceNo);
        if (invoiceNo != "")
            productInfoManager.ProductCustomerInfoFillByInvoice(invoiceNo);
        else
            return false;
        //$('#txtInvoiceProInfo').val(invoiceNo);
        return false;
    },
    
    ProductCustomerInfoFillByInvoice: function (invoiceNo, saleId) {

        var jsonParam = "invoiceNo=" + invoiceNo ;
        var serviceUrl = "../Product/GetProductCustomerInfoByInvoiceNo";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            if (jsonData != null) {
               
                gbProductInfo = jsonData;
                productInfoHelper.FillProductCustomerInfoForm(gbProductInfo);

            }
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
    },

    ProductCustomerInfoFill: function (invoiceNo,saleId) {
        
        var jsonParam = "invoiceNo=" + invoiceNo + "&saleId=" + saleId;
            var serviceUrl = "../Product/GetProductCustomerInfoByInvoiceNo";
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
       
        function onSuccess(jsonData) {
            if (jsonData != null) {
                
                gbProductInfo = jsonData;
                productInfoHelper.FillProductCustomerInfoForm(gbProductInfo);

            }
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
    },
    
    InstallmentInfoFill: function (invoiceNo) {
        var objAllInstallmentInfo="";
        if (invoiceNo != 0) {
            var jsonParam = "invoiceNo=" + invoiceNo;
            var serviceUrl = "../Collection/GetAllInstallmentByInvoiceId";
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        } 

        function onSuccess(jsonData) {
            if (jsonData != null) {
                objAllInstallmentInfo = jsonData;
            }
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objAllInstallmentInfo;
    },
 
};


var productInfoHelper = {
    InitProductInfoHelper: function () {
        productInfoHelper.GeneratePackageTypeCombo();
        productInfoHelper.GenerateModelCombo([]);
        productInfoHelper.ProductModelChange();
       
        
        var downPay =  "0 %";
        $("#btndownPayPercent").html(downPay);
    },
    
    GeneratePackageTypeCombo:function() {

        $("#cmbPackageType").kendoComboBox({
            placeholder: "Select Type",
            dataTextField: "PackageTypeName",
            dataValueField: "PackageTypeId",
            dataSource: [{ PackageTypeId: 1, PackageTypeName: 'Package' },
                         { PackageTypeId: 2, PackageTypeName: 'Item' }],
            suggest: true,
            change: function () {
   
                //var packageType = this.value();
              
                //var data = productInfoManager.GetProductModel(packageType);
                //$("#cmbModelProInfo").data("kendoComboBox").value("");
                //productInfoHelper.GenerateModelCombo(data);
            }
        });
    },
    
    GenerateModelCombo: function (data) {
        var objModel = data;
        $("#cmbModelProInfo").kendoComboBox({
            placeholder: "Select Model",
            dataTextField: "Model",
            dataValueField: "ModelId",
            dataSource: objModel,
            suggest: true,
            change: function () {
                AjaxManager.isValidItem("cmbModelProInfo", true);
            }
        });
    },
    FillProductInfoInForm: function (objProduct) {
     
        productInfoHelper.clearProductForm();
        $('#hdnModelIdProInfo').val(objProduct.ModelId);
        $('#txtNameProInfo').val(objProduct.ProductName);
        $('#txtLicenseProInfo').val(objProduct.ALicense.Number);
        $("#txtLicenseTypeProInfo").val(objProduct.ALicense.LType);
    },


    // ----------------------------------------Product Model Change Event ------------------------------------------
    ProductModelChange: function () {
        $('#cmbModelProInfo').change(function() {
            var customercode = $("#txtCodeCustInfo").val();
            var comboModelInfoData = $('#cmbModelProInfo').data('kendoComboBox');
            var data = comboModelInfoData.dataItem(comboModelInfoData.select());

            if (customercode != "") {
                
                //var modelId = $('#cmbModelProInfo').val();
                // Comments for avoid Stock Balance.....24/01/2015-------------------------------------------------
               // var res = productInfoHelper.CheckStockBalanceByModel(modelId);
               
                if (true) {
                    productInfoManager.ProductInfoFillByModel();
  
                    if (data.TypeId == 3) {
                        //New Code for Package Type D-Type (Prepaid) = 3 & R-Type (Postpaid) = 4
                        $("#btnSaveAsBook").hide();
                        $("#btnPreviewSales").hide();
                        $("#btnPreviewSaveAsDraft").hide();
                        $("#btnFinalSubmit").show();

                    } else {
                        $("#btnSaveAsBook").show();
                        $("#btnPreviewSales").show();
                        $("#btnPreviewSaveAsDraft").show();
                        $("#btnFinalSubmit").hide();
                    }

                }
                //else {
                //    AjaxManager.MsgBox('warning', 'center', 'Warning', 'This Model is Out of Stock!',
                //    [{
                //            addClass: 'btn btn-primary',
                //            text: 'Ok',
                //            onClick: function($noty) {
                //                $noty.close();

                //                var grid = $("#gridProductItemsInfo").data("kendoGrid");
                //                if (grid != undefined) {
                //                    $("#gridProductItemsInfo").data("kendoGrid").dataSource.data([]);
                //                }
                //                saleDetailsHelper.ClearSalesDetailsFormForChangeModel();
                //                saleDetailsHelper.clearSaleForm();
                //                $("#txtInvoiceSale").val("");
                //            }
                //        }
                //    ]);
                //}
            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please Enter Customer ID First!',
                   [{
                       addClass: 'btn btn-primary',
                       text: 'Ok',
                       onClick: function ($noty) {
                           $noty.close();
                           $("#cmbModelProInfo").data("kendoComboBox").value("");
                           $("#btnSaveAsBook").show();
                           $("#btnPreviewSales").show();
                           $("#btnPreviewSaveAsDraft").show();
                       }
                   }
                   ]);
            }
           
            });

       
    },


    CheckStockBalanceByModel: function (modelId) {
        var objres = "";
        var jsonParam = "modelId=" + modelId;
        var serviceUrl = "../Stock/CheckExistStockByModelId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
           
            objres = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objres;
    },


    FillProductInfoByModel: function (objProduct) {
     
        productInfoHelper.clearProductForm();
        saleDetailsHelper.ClearSalesDetailsFormForChangeModel();

        $('#hdnModelIdProInfo').val(objProduct.ModelId);
        $("#hdnModelItemIdProInfo").val(objProduct.ModelItemID);
        $('#txtNameProInfo').val(objProduct.ProductName);
        $('#txtPackagePrice').html(objProduct.TotalPrice);
        
        if (objProduct.IsDPFixedAmount == 1) {
            $("#btndownPayPercent").html("Fixed");
        } else {
            // product base down pay percent
            var downPay = objProduct.DownPayPercent + " %";
            $("#btndownPayPercent").html(downPay);
        }
        gbDownPayPercentage = objProduct.DownPayPercent;

        //calculate discount 
        var discountAmount = $("#txtDiscountAmount").val();
        if (discountAmount > 0) {
            objProduct.TotalPrice = (objProduct.TotalPrice - discountAmount);
        }
        $("#txtPrice").data("kendoNumericTextBox").value(objProduct.TotalPrice);
       
        //Previous Default Installment No from Default Settings
        // saleDetailsManager.GetDefaultInstallmentNo();
        //Now default Install No from Package (Product Info)
        saleDetailsManager.GetDefaultInstallmentNoByPackageWise(objProduct.ModelId);

        productInfoHelper.FillProductItemsInfo(objProduct);

        productInfoHelper.FirstTimeFillSalesDetailsForm(objProduct);

    },

    FirstTimeFillSalesDetailsForm: function (objProduct) {
    
        //auto generate invoice no first time
        var invoice = saleDetailsManager.GetSaleInvoice();
        $("#txtInvoiceSale").val(invoice);
        $("#txtChallan").val(invoice);
        saleDetailsHelper.SetSaleDateCombo();
        var saleObj = saleDetailsHelper.GetSalesObj();
        saleObj.CollectedAmount = saleObj.downPayForCollectedAmount;
        saleObj.IsDPFixedAmount = objProduct.IsDPFixedAmount;
        ProductItemInfoHelper.CalculateNetTotalPrice(saleObj);
       
        saleDetailsHelper.InstallmentData(saleObj);
    },


    FillProductItemsInfo: function (objProduct) {
       var dataSource = ProductIntemInfoManager.gridDataSource('../Product/GetAllProductItemByModelId/?modelId=' + objProduct.ModelId);
         $("#gridProductItemsInfo").data().kendoGrid.setDataSource(dataSource);

    },

    GetProductItemData: function (url,param) {
        var objRuturn = new Object();
        //var jsonParam = 'modelId=' + modelId;
        //var serviceUrl = "../Product/GetAllProductItemByModelId";
        AjaxManager.GetJsonResult(url, param, false, false, function (result) {
            objRuturn = result;
        }, function () {

        });
    },

    clearProductForm: function () {

        gbTotalPrice = 0;
        gbItemDetailsInfoArray = [];
        itemDetailsArray = [];
        gbitemInfoArray = [];

        $('#hdnModelIdProInfo').val('');
        $('#txtCodeProInfo').val('');
        $('#txtNameProInfo').val('');
        $('#txtLicenseProInfo').val('');
        $("#txtLicenseTypeProInfo").val('');
        $("#hdnModelItemIdProInfo").val('');
    },

   

    FillProductCustomerInfoForm: function (obj) {
        debugger;
        $("#hdnModelIdProInfo").val(obj.AProduct.ModelId);
        $("#hdnModelItemIdProInfo").val(obj.AProduct.ModelItemID);
        $("#txtPackagePrice").html(obj.AProduct.TotalPrice);
    
        $("#cmbPackageType").data("kendoComboBox").value(obj.AProduct.PackageType);
        
        var data = productInfoManager.GetProductModel(obj.AProduct.PackageType);
        productInfoHelper.GenerateModelCombo(data);

        var objModeldata = saleDetailsManager.GetProductModelbyTypeIdAndPackageId(obj.AProduct.ModelItemID, obj.AProduct.PackageType);
        $("#cmbModelProInfo").data("kendoComboBox").setDataSource(objModeldata);

        $("#hdnCustomerId").val(obj.ACustomer.CustomerId);
        $("#cmbModelProInfo").data("kendoComboBox").value(obj.AProduct.ModelId);
        $("#txtNameProInfo").val(obj.AProduct.ProductName);
       
        $("#txtLicenseProInfo").val(obj.ALicense.Number);
        $("#txtLicenseTypeProInfo").val(obj.AType.Type);
        $("#txtCodeCustInfo").val(obj.ACustomer.CustomerCode);
        $("#txtMobileNo1").val(obj.ACustomer.Phone);
        $("#txtMobileNo2").val(obj.ACustomer.Phone2);

        $("#txtNameCustInfo").val(obj.ACustomer.Name);
        $("#txtNidCustInfo").val(obj.ACustomer.NId);
        $("#txtReferenceId").val(obj.ACustomer.ReferenceId);
        $("#txtPidCustInfo").val(obj.ACustomer.ProductId);
        $("#cmbType").data("kendoComboBox").value(obj.AProduct.ProductTypeName);


        var path = window.location.pathname;

        if (obj.ACustomer.IsStaff == 1) {
            $("#chkStaff").prop('checked', 'checked');
            $("#staffcomboli").show();
           
            customerInfoHelper.populateStaffcombo(3);
            $("#cmbStaffId").data("kendoComboBox").value(obj.ACustomer.StaffId);
            
            
        } else {
            $("#chkStaff").removeProp('checked', 'checked');
        }

        if (path == "/Sale/Sale") {
            productInfoHelper.FillSalesDetails(obj);

            ProductItemInfoHelper.FillItemInfoBySaleId(obj.SaleId);
            ProductItemInfoHelper.FillItemDetailsInformationBySaleId(obj.SaleId);

            InstallmentDetailsManager.FillInstallmentGrid(obj.Invoice);
            var discountInfo = DiscountInfoManager.GetDiscountInfo(obj.SaleId);
            DiscountInfoHelper.FillDiscountInfoForm(discountInfo);
        }

        else if (path == "/Collection/Collection") {
            InstallmentDetailsManager.FillInstallmentGrid(obj.Invoice);
            CustomerRatingManager.CustomerDueGridDataSet(obj.Invoice);
            CollectionDetailsHelper.fillDownpaymentInfo(obj.Invoice);
        }
    },
    FillSalesDetails: function (objSales) {
       
        var totalPrice = objSales.NetPrice + objSales.DownPay;
        
        $("#hdnSaleId").val(objSales.SaleId);
        $("#txtChallan").val(objSales.ChallanNo);
        $("#cmbSaleDate").data("kendoDatePicker").value(objSales.WarrantyStartDate);
        $("#cmbPayDate").data("kendoDatePicker").value(objSales.FirstPayDate);
    
        $("#cmbSaleType").data("kendoComboBox").value(objSales.SaleTypeId);
        $("#txtPrice").data("kendoNumericTextBox").value(objSales.Price);
        $("#txtDownPay").data("kendoNumericTextBox").value(objSales.DownPay);
        $("#txtTotalPrice").data("kendoNumericTextBox").value(totalPrice);
        $("#txtOutstanPrice").data("kendoNumericTextBox").value(objSales.NetPrice);
        $("#txtInstallment").data("kendoNumericTextBox").value(objSales.Installment);
      
        $("#cmbSaleRepresentator").data("kendoComboBox").value(objSales.SalesRepId);
        
        RepresentatorInfoHelper.ChangeEventForSaleRepresentatorCombo(objSales.SalesRepId);

    },



}