
var ReplacementDetailsManager = {
    InitReplacementDetails: function () {
       
        $("#popupReplacementDetails").kendoWindow({

            title: "Replacement Details",
            resizeable: false,
            width: "40%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
        });
       
        ReplacementDetailsHelper.ChangeEventForItemSlNo();

        $("#btnClearAllReplacement").click(function() {
            ReplacementDetailsHelper.ClearAllSaleDetailsForm();
        });

        $("#txtReplacementInvoiceNo").change(function () {
            ReplacementDetailsHelper.GetReplacementInvoice();
        });


        $("#btnCancel").click(function () {
          
            $("#popupReplacementDetails").data("kendoWindow").close();
            ReplacementDetailsHelper.ClearReplacementDetailsForm();
        });
        $("#btnClearWarrantyInfo").click(function () {
       
            ReplacementDetailsHelper.ClearReplacementDetailsForm();
        });
        ReplacementDetailsHelper.GenerateProductTypeCombo();
        ReplacementDetailsHelper.GenerateDatePicker();

        $("#btnSaveReplace").click(function () {
            ReplacementDetailsManager.SaveReplacement();
        });

    },


    GetProductType: function () {
        var objProduct = "";
        var jsonParam = "";
        var serviceUrl = "../Product/GetProductType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objProduct = jsonData;
        }
      
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objProduct;
    },

    SaveReplacement: function () {

        var validator = $("#replacementDetailsDiv").kendoValidator().data("kendoValidator"),
        status = $(".status");
        if (validator.validate()) {
            var isToUpdateOrCreate = $("#hdnReplacementId").val();
            var objReplacementInfo = ReplacementDetailsHelper.createReplacementInfoObject();
            var jsonParam = "objReplacementInfo:" + JSON.stringify(objReplacementInfo);
            var serviceUrl = "../Replacement/ReplaceProduct";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                //customerDetailsHelper.ClearCustomerForm();
                if (isToUpdateOrCreate == 0) {
                    AjaxManager.MsgBox('success', 'center', 'Success', 'Replace Product Successfully.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                              ReplacementDetailsHelper.ClearReplacementDetailsForm();
                              $("#popupReplacementDetails").data("kendoWindow").close();
                              $("#replacementGrid").data("kendoGrid").dataSource.read();
                          }
                      }]);
                } else {
                    AjaxManager.MsgBox('success', 'center', 'Update', 'Replacement Information Updated Successfully.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
                }
                
            }
            else if (jsonData == "Exists") {


                AjaxManager.MsgBox('warning', 'center', 'Alresady Exist:', 'already exist.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
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

   

    GetCustomerAndSaleInfoByCustomerCode: function (customerCode) {
        var objsaleInfo = "";
        var jsonParam = "customerCode=" + customerCode;
        var serviceUrl = "../Replacement/GetCustomerAndPackageInfoByCustomerCode/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objsaleInfo = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objsaleInfo;
    }
};

var ReplacementDetailsHelper = {

    ProductCustomerSaleInfoFill: function () {

        var invoiceNo = $('#txtInvoiceSale').val() == "" ? 0 : $('#txtInvoiceSale').val();
        //var month = $('#cmbFromDate').data().kendoDatePicker.value();
        ReplacementproductInfoManager.ProductCustomerInfoFill(invoiceNo);
    },
    GenerateSaleDatePicker: function() {
        $("#cmbSaleDate").kendoDatePicker({
            format: "dd-MM-yyyy"
        });
    },

    GenerateProductTypeCombo: function () {
        var objProduct = ReplacementDetailsManager.GetProductType();
        $("#cmbProductType").kendoComboBox({
            placeholder: "Select Product...",
            dataValueField: "TypeId",
            dataTextField: "Type",
            dataSource: objProduct,
            filter: "contains",
            suggest: true,
            change: function () {
                var value = this.value();
                AjaxManager.isValidItem("cmbProductType", true);
            }

        
        });
    },
    GenerateProductModelCombo: function () {
        var objModel = ReplacementproductInfoManager.GetProductModel();
        $("#cmbWarrantyProductModel").kendoComboBox({
            placeholder: "Select Model",
            dataTextField: "Model",
            dataValueField: "ModelId",
            dataSource: objModel,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem("cmbModelProInfo", true);
            }
        
        });
    },
    GenerateDatePicker: function () {
        var date = new Date();
        $("#txtManufacturingDate").kendoDatePicker({ format: "dd/MM/yyyy", value: date });
        $("#txtReplacementDate").kendoDatePicker({ format: "dd/MM/yyyy", value: date });
        $("#txtInstallationDate").kendoDatePicker({ format: "dd/MM/yyyy", value: date });
        $("#txtwarrantyEndDate").kendoDatePicker({ format: "dd/MM/yyyy", value: date });
    },
    createReplacementInfoObject: function() {
        debugger;
        $("#txtwarrantyEndDate").data("kendoDatePicker").value(gbWarrantyEndDate);
        var obj = new Object();
        obj.ReplacementId = $("#hdnReplacementId").val();
        obj.SaleId = $("#hdnSaleId").val();
      
        obj.SaleInvoice = $("#hdnSaleInvoiceNo").val();
        obj.AProduct = {
            ModelId: $("#cmbModelProInfo").data("kendoComboBox").value(),
            WarrantyEndDate: $("#txtwarrantyEndDate").data("kendoDatePicker").value()
        };
        obj.ALicense = { Number: $('#txtLicenseProInfo').val() };
        obj.ACustomer = {
            CustomerId: $("#hdnCustomerId").val(),
            CompanyId : $("#hdnCompanyId").val(),
            BranchId : $("#hdnBranchId").val()
        };
        obj.ReplacedItemSLNo = $("#txtreplacedItemSLNo").val();
        obj.ReplacedChalanNo = $("#txtReplacementChalan").val();
        obj.InstallmentDate = $("#txtInstallationDate").data("kendoDatePicker").value();
        obj.ReplacementDate = $("#txtReplacementDate").data("kendoDatePicker").value();
        obj.RefItemId = $("#hdnItemId").val();
        obj.SalesItemId = $("#hdnSalesItemId").val();
        return obj;
    },
    ClearReplacementDetailsForm:function () {
        $("#hdnItemId").val(0);
        $("#hdnReplacementId").val(0);
        $("#hdnSaleId").val(0);
        $("#hdnCustomerId").val(0),

        $("#txtreplacedItemSLNo").val("");
        $("#txtReplacementInvoiceNo").val("");
        
       $("#txtReplacementChalan").val("");
     
       $("#txtManufacturingDate").data("kendoDatePicker").value("");
       $("#txtInstallationDate").data("kendoDatePicker").value("");
       $("#txtReplacementDate").data("kendoDatePicker").value("");
        $("#txtwarrantyEndDate").data("kendoDatePicker").value("");
       $("#replacementDetailsDiv > form").kendoValidator();
       $("#replacementDetailsDiv").find("span.k-tooltip-validation").hide();
       var status = $(".status");
       status.text("").removeClass("invalid");
        

    },
    
    ChangeEventForItemSlNo: function () {
        $('#cmbwarrantyItemSLNo').change(function () {
          
            var elapsedTime = (new Date()).valueOf();
           var code = "RE_"+CurrentUser.CompanyId + "" + CurrentUser.BranchId + "" + CurrentUser.UserId + "_" + elapsedTime;
           $("#txtreplacedItemSLNo").val(code);
        });
    },


    ClearAllSaleDetailsForm: function () {

        $("#hdnSaleId").val(0);
        $('#hdnProductId').val("");
        $('#hdnModelIdProInfo').val('');
        $('#txtNameProInfo').val('');
        $("#cmbModelProInfo").data("kendoComboBox").value("");
        $("#hdnCustomerId").val('');
        $('#txtCodeCustInfo').val('');
        $('#txtNameCustInfo').val('');
        $("#gridProductItemsInfo").data("kendoGrid").dataSource.data([]);
        $("#txtCustomerCodeRe").val("");
        $("#txtCustomerNameRe").val("");
        $("#txtInstallDateRe").data("kendoDatePicker").value("");
        $("#hdnSaleInvoiceNo").val("");
    },


    ClickEventForReplaceButton: function () {
       
        $("#popupReplacementDetails").data("kendoWindow").open().center();
        var entityGrid = $("#gridProductItemsInfo").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            ReplacementDetailsHelper.FillReplacementDetailsField(selectedItem);
        }
       
    },
    FillReplacementDetailsField: function (obj) {
       
        var elapsedTime = (new Date()).valueOf();
        var replacedItemSlNo = "RE_" + CurrentUser.CompanyId + "" + CurrentUser.BranchId + "" + CurrentUser.UserId + "_" + elapsedTime;
        $("#txtreplacedItemSLNo").val(replacedItemSlNo);
        $("#hdnItemId").val(obj.ItemId);
        $("#hdnSalesItemId").val(obj.SalesItemId);
        $("#txtwarrantyEndDate").data("kendoDatePicker").value(gbWarrantyEndDate);
    },


    GetReplacementInvoice: function () {
        var invoice = $("#txtReplacementInvoiceNo").val();
        var objSale = "";
        var jsonParam = "invoice=" + invoice;
        var serviceUrl = "../Replacement/GetReplacementInvoice/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
        
            objSale = jsonData;
            $("#txtReplacementInvoiceNo").val(objSale);
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objSale;
    },

    

};

