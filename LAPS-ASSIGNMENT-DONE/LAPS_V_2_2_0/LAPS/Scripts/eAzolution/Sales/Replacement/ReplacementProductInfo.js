var code;

var gbProductInfo;

var ReplacementproductInfoManager = {

    InitProductInfo: function () {
        $("#txtInvoiceProInfo").keypress(function (event) { if (event.keyCode == 13) { ReplacementproductInfoManager.FillCustomerProductForm(); } });
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

    GetItemSLNo: function (salesItemId) {
        var objModel = "";
        var jsonParam = "salesItemId=" + salesItemId;
        var serviceUrl = "../Product/GetItemSlNoBySalesItemId/";
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
                ReplacementproductInfoHelper.FillProductInfoInForm(jsonData);
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

    ProductSaleInfoFillByModel: function () {
        var modelId = $('#cmbModelProInfo').data().kendoComboBox.value();
        var customerCode = $("#txtCustomerCodeRe").val();
        var jsonParam = "modelId=" + modelId + "&customerCode=" + customerCode;
        var serviceUrl = "../Replacement/GetPackageSaleInfo";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            if (jsonData != null) {
                ReplacementproductInfoHelper.FillProductInfoByModel(jsonData);
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
            ReplacementproductInfoManager.ProductCustomerInfoFill(invoiceNo);
        else
            return false;
        //$('#txtInvoiceProInfo').val(invoiceNo);
        return false;
    },

    ProductCustomerInfoFill: function (invoiceNo, saleId) {

        if (invoiceNo != 0) {
            var jsonParam = "invoiceNo=" + invoiceNo + "&saleId=" + saleId;
            var serviceUrl = "../Product/GetProductCustomerInfoByInvoiceNo";
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please Input a Invoice No!',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                             //saleDetailsHelper.ClearAllSaleDetailsForm();
                         }
                     }]);
        }


        function onSuccess(jsonData) {
            if (jsonData != null) {

                gbProductInfo = jsonData;
                ReplacementproductInfoHelper.FillProductCustomerInfoForm(gbProductInfo);

            }
            else {

                ReplacementDetailsHelper.ClearAllSaleDetailsForm();


            }

        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
    },

    InstallmentInfoFill: function (invoiceNo) {
        var objAllInstallmentInfo = "";
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
var ReplacementproductInfoHelper = {

    InitReplaceProductInfo: function () {
        ReplacementproductInfoHelper.GenerateModelCombo();
        ReplacementproductInfoHelper.ProductModelChange();
    },

    GenerateModelCombo: function () {
        $("#cmbModelProInfo").kendoComboBox({
            placeholder: "Select Model",
            dataTextField: "ProductName",
            dataValueField: "ModelId",
            dataSource: [],
            index: 0,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem("cmbModelProInfo", true);
            }
        });
    },


    GenerateItemSLNoCombo: function (salesItemId) {
        var objSlNo = ReplacementproductInfoManager.GetItemSLNo(salesItemId);
        $("#cmbwarrantyItemSLNo").kendoComboBox({
            placeholder: "Select SL NO",
            dataTextField: "ItemSLNo",
            dataValueField: "SalesItemDetailsId",
            dataSource: objSlNo,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem("cmbwarrantyItemSLNo", true);
            }
        });
    },

    FillProductInfoInForm: function (objProduct) {
        ReplacementproductInfoHelper.clearProductForm();
        $('#hdnModelIdProInfo').val(objProduct.ModelId);
        $('#txtCodeProInfo').val(objProduct.Code);
        $('#txtNameProInfo').val(objProduct.ProductName);
        $("#txtManuDateProInfo").val(objProduct.ManufactureDate);
        $('#txtLicenseProInfo').val(objProduct.ALicense.Number);
        $("#txtLicenseTypeProInfo").val(objProduct.ALicense.LType);
    },


    //  ----------------------------------------Product Model Change Event ------------------------------------------
    ProductModelChange: function () {

        $('#cmbModelProInfo').change(function () {
            ReplacementproductInfoManager.ProductSaleInfoFillByModel();
        });
    },




    FillProductInfoByModel: function (obj) {


        $("#hdnSaleId").val(obj.SaleId);
        $("#hdnCustomerId").val(obj.CustomerId);
        $("#hdnCompanyId").val(obj.CompanyId);
        $("#hdnBranchId").val(obj.BranchId);
        $('#txtNameProInfo').val(obj.PackageInfos.ProductName);
        $("#txtInstallDateRe").data().kendoDatePicker.value(obj.WarrantyStartDate);

        ReplacementProductItemInfoHelper.FillItemDetailsInformationBySaleId(obj.SaleId);
        ReplacementProductItemInfoHelper.FillItemInfoGridBySaleId(obj.SaleId);


    },

    FillProductItemsInfo: function (objProduct) {
        var dataSource = ProductIntemInfoManager.gridDataSource('../Product/GetAllProductItemByModelId/?modelId=' + objProduct.ModelId);
        $("#gridProductItemsInfo").data().kendoGrid.setDataSource(dataSource);
    },


    clearProductForm: function () {

        gbTotalPrice = 0;
        gbItemDetailsInfoArray = [];
        itemDetailsArray = [];
        gbitemInfoArray = [];

        $('#hdnModelIdProInfo').val('');
        $('#txtCodeProInfo').val('');
        $('#txtNameProInfo').val('');
        $("#txtManuDateProInfo").val('');
        $('#txtLicenseProInfo').val('');
        $("#txtLicenseTypeProInfo").val('');
    },



    FillProductCustomerInfoForm: function (obj) {

        $("#hdnModelIdProInfo").val(obj.AProduct.ModelId);
        $("#hdnCustomerId").val(obj.ACustomer.CustomerId);
        $("#cmbModelProInfo").data("kendoComboBox").value(obj.AProduct.ModelId);
        $("#txtNameProInfo").val(obj.AProduct.ProductName);
        $("#txtLicenseProInfo").val(obj.ALicense.Number);
        $("#txtLicenseTypeProInfo").val(obj.AType.Type);
        $("#txtCodeCustInfo").val(obj.ACustomer.CustomerCode);
        $("#txtNameCustInfo").val(obj.ACustomer.Name);

        ReplacementproductInfoHelper.FillSalesDetails(obj);
        ReplacementProductItemInfoHelper.FillItemDetailsInformationBySaleId(obj.SaleId);
        ReplacementProductItemInfoHelper.FillItemInfoBySaleId(obj.SaleId);
        ReplacementSummaryManager.FillRplacementGrid(obj.Invoice);

    },
    FillSalesDetails: function (objSales) {

        var totalPrice = objSales.NetPrice + objSales.DownPay;
        var inst = objSales.Installment / 12;

        $("#hdnSaleId").val(objSales.SaleId);
        $("#txtChallan").val(objSales.ChallanNo);
        $("#cmbSaleDate").data("kendoDatePicker").value(objSales.EntryDate);
        $("#cmbPayDate").val(objSales.FirstPayDate);
        $("#cmbSaleType").data("kendoComboBox").value(objSales.SaleTypeId);
        $("#txtPrice").data("kendoNumericTextBox").value(objSales.Price);
        $("#txtDownPay").data("kendoNumericTextBox").value(objSales.DownPay);
        $("#txtTotalPrice").data("kendoNumericTextBox").value(totalPrice);
        $("#txtOutstanPrice").data("kendoNumericTextBox").value(objSales.NetPrice);
        $("#txtInstallment").data("kendoNumericTextBox").value(inst);
    },



}