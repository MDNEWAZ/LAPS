var installmentReceicveArray = [];

var CollectionDetailsManager = {
    InitPaymentCollection: function () {
       
        CollectionDetailsHelper.CollectionPopUpWindow();
        CollectionDetailsHelper.GenerateDatePicker();
        CollectionDetailsHelper.GeneratePaymentTypeCombo();

        $("#txtRecAmount").kendoNumericTextBox();
        $("#btnSavePayment").click(function () {
            CollectionDetailsManager.SavePaymentCollection();
        });
        $("#btnCancelPopup").click(function () {
            CollectionDetailsHelper.ClearFields();
            $("#collectionPopupWindow").data("kendoWindow").close();
        });

        $("#btnClearCollection").click(function () {
            CollectionDetailsHelper.ClearAllFields();
        });
    },

    GetPaymentTypeData: function () {
        var objpaymentType = "";
        var jsonParam = "";
        var serviceUrl = "../Collection/GetPaymentType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objpaymentType = jsonData;
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objpaymentType;
    },
    
    getPreviousDownPayment: function (invoiceNo) {
        var objPreviousDownPaymentInfo = "";
        if (invoiceNo != 0) {
            var jsonParam = "invoiceNo=" + invoiceNo;
            var serviceUrl = "../Collection/GetDownpaymentByInvoiceNo";
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData != null) {
                objPreviousDownPaymentInfo = jsonData;
            }
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objPreviousDownPaymentInfo;
    },
    

    //-------------------------------------------Save Collection-----------------------------------------
    SavePaymentCollection: function () {
      
        var validator = $("#collectionDetailsDivpopup").kendoValidator().data("kendoValidator"),
        status = $(".status");
        if (validator.validate()) {
            var paymentObj = CollectionDetailsHelper.CreatePaymentObject();
            var objPaycollection = JSON.stringify(paymentObj);
            var jsonParam = "paymentInfoObj:" + objPaycollection;
            var serviceUrl = "../Collection/GetPaymentAndCollect";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
   
                var saleInvoice = $("#txtInvoiceProInfo").val();

                //CollectionDetailsManager.SendSmsByCustomerRating(saleInvoice);

                AjaxManager.MsgBox('success', 'center', 'Success', 'Payment Received Successfully.',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                          installmentReceicveArray = [];
                        
                          $("#gridInstalment").data("kendoGrid").dataSource.read();
                          CustomerRatingManager.CustomerDueGridDataSet(saleInvoice);
                          CollectionDetailsHelper.fillDownpaymentInfo(saleInvoice);
                          $("#collectionPopupWindow").data("kendoWindow").close();
                          
                       
                      }
                  }]);
                
            } else if (jsonData == "Over") {
                AjaxManager.MsgBox('warning', 'center', 'Warning', 'You give more than due exceed amount.',
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
    
    SendSmsByCustomerRating: function (saleInvoice) {
        var obj = "";
        var jsonParam = "saleInvoice=" + saleInvoice;
        var serviceUrl = "../Collection/SendSmsByCustomerRating/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return obj;

    },


};


var CollectionDetailsHelper = {

    GeneratePaymentTypeCombo: function () {
        var dataSourec = CollectionDetailsManager.GetPaymentTypeData();
        $("#cmbPayType").kendoComboBox({
            placeholder: "All",
            dataTextField: "Type",
            dataValueField: "TypeId",
            dataSource: dataSourec, //[{ Type: "Cash", TypeId: 1 }, { Type: "Cheque", TypeId: 2 }]
            index: 0,
            filter: "contains",
            suggest: true
        });
    },

    CreateCollectionObject: function () {
        var obj = new Object();

        obj.InstallmentId = $("#hdnInstallmentId").val();
        obj.InstallmentNo = $("#txtInstallNo").val();
        obj.TransectionId = $("#txtTransacNo").val();
        obj.ReceiveAmount = $("#txtRecAmount").val();
        var installmentAmount = $("#hdnInstallmentAmount").val();
        var alreadyReceiveAmount = $("#hdnAlreadyReceiveAmout").val();
       // var previousDueAmount = $("#hdnPreviousDueAmout").val();
        obj.DueAmount = (parseInt(installmentAmount) - (parseInt(alreadyReceiveAmount) + parseInt(obj.ReceiveAmount)));
        if(obj.DueAmount<0) {
            alert("Total Receive Amount is over of your Installment Amount");
            return false;
        }
        obj.DueDate = $("#txtDueDatee").val();
        obj.PayDate = $("#txtPayDate").val();
        obj.AProduct = {
            ModelId: $("#hdnModelId").val(),
        };
        var path = window.location.pathname;
        if(path=="/Sale/Sale") {
            obj.SaleInvoice=$("#txtInvoiceSale").val();
        }
        else {
            obj.SaleInvoice = $("#txtInvoiceProInfo").val();
        }
        obj.PaymentType = $("#cmbPayType").val();
        return obj;
    },
    
    CreatePaymentObject: function () {
    
        var obj = new Object();
        obj.CustomerId = $("#hdnCustomerId").val();
        obj.CustomerCode = $("#txtCodeCustInfo").val();
        obj.SaleInvoice = $("#txtInvoiceProInfo").val();
        obj.ReceiveAmount = $("#txtRecAmount").val();// need to calculate for total due
        //obj.CollectionType = $("#cmbPayType").data('kendoComboBox').value();
        obj.TransactionType = $("#cmbPayType").data('kendoComboBox').value();
        obj.TransectionId = $("#txtTransacNo").val();
        obj.Phone = $("#txtMobileNo1").val();
        obj.Phone2 = $("#txtMobileNo2").val();
        obj.PayDate = $("#txtPayDate").data("kendoDatePicker").value();
        obj.BranchSmsMobileNumber = $("#hdnBranchSmsMobNo").val();
        obj.IsCustomerUpgraded = $("#hdnIsCustomerUpgraded").val();
        obj.BranchCode = $("#hdnBranchCode").val();
        obj.SaleTypeId = $("#hdnSaleTypeId").val();
        obj.CustomerName = $("#txtNameCustInfo").val();
        return obj;
    },


    FillPaymentCollectionForm: function (obj) {

        $("#collectionPopupWindow").data("kendoWindow").open().center();
        $("#hdnInstallmentId").val(obj.InstallmentId);

        $("#hdnInstallmentAmount").val(obj.Amount);
        $("#hdnAlreadyReceiveAmout").val(obj.ReceiveAmount);
        $("#hdnPreviousDueAmout").val(obj.DueAmount);

        $("#txtInstallNo").val(obj.Number);
        $("#txtDueDatee").val(obj.DueDate);
        $("#hdnProductId").val(obj.AProduct.ProductId);
        $("#txtRecAmount").data("kendoNumericTextBox").value(obj.DueAmount);
    
        var path = window.location.pathname;
        if (path == "/Sale/Sale") {
            $("#txtInvoiceSale").val(obj.SInvoice);
        }
    },

    GenerateDatePicker: function () {
        var date = new Date();
        $("#txtDueDatee").kendoDatePicker({
            format: "dd-MMM-yyyy"
        });

        $("#txtPayDate").kendoDatePicker({
            format: "dd-MMM-yyyy",
            value: date
        });
    },

    ClearFields: function () {

        $("#hdnInstallmentId").val(0);
        $("#txtInstallNo").val("");
        $("#txtDueDatee").val("");
        
        $("#hdnInstallmentAmount").val(0);
        $("#hdnAlreadyReceiveAmout").val(0);
        $("#hdnPreviousDueAmout").val(0);

        $("#txtTransacNo").val("");
        $("#txtRecAmount").data("kendoNumericTextBox").value("");

        $("#collectionDetailsDivpopup > form").kendoValidator();
        $("#collectionDetailsDivpopup").find("span.k-tooltip-validation").hide();
        var status = $(".status");
        status.text("").removeClass("invalid");
    },
    ClearAllFields:function () {

        $("#txtInvoiceProInfo").val("");
        $('#hdnModelIdProInfo').val('');
        $('#txtCodeProInfo').val('');
        $('#txtNameProInfo').val('');
        $("#txtManuDateProInfo").val('');
        $('#txtLicenseProInfo').val('');
        $("#txtLicenseTypeProInfo").val('');
        $("#cmbModelProInfo").data("kendoComboBox").value("");

        $("#hdnCustomerId").val('');
        $('#txtCodeCustInfo').val('');
        $('#txtPhoneCustInfo').val('');
        $('#txtNameCustInfo').val('');
        $("#txtNidCustInfo").val('');
        $("#ddlDobCustInfo").val('');
        $("#txtFatherNameCustInfo").val('');
        $("#txtGenderCustInfo").val('');
        $("#txtAddressCustInfo").val('');
        $("#ddlDistrictCustInfo").val('');
        

        $("#OutstandingAmountlbl").val('');
        $("#DueAmountlbl").val('');
        $("#DuePercentlbl").val('');
        $("#DueRatinglbl").html(" ");
        $("#gridInstalment").data("kendoGrid").dataSource.data([]);
        
    },

    CollectionPopUpWindow: function () {
        $("#collectionPopupWindow").kendoWindow({
            title: "Collection Details",
            resizeable: false,
            width: "50%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
        });
    },
    
    clickEventForReceiveButton: function () {
        CollectionDetailsHelper.ClearFields();
        $("#collectionPopupWindow").data("kendoWindow").open().center();
        
    },
    
    
    
    clickEventForSave: function () {

        CollectionDetailsManager.SavePaymentCollection();
        
    },
    
    fillDownpaymentInfo: function (invoiceNo) {
        var downpayment = CollectionDetailsManager.getPreviousDownPayment(invoiceNo);

        $("#txtDownpayment").val(downpayment.DownPay - downpayment.ReceiveAmount);
        $("#txtDownpaymentPaid").val(downpayment.ReceiveAmount);
        
    },
    
    FillCustomerAndProductDetails: function (obj) {
      
        $("#hdnCustomerId").val(obj.CustomerId);
        $("#txtInvoiceProInfo").val(obj.Invoice);
        $("#txtCodeCustInfo").val(obj.CustomerCode);
        $("#txtNidCustInfo").val(obj.NID);
        $("#txtMobileNo1").val(obj.Phone);
        $("#txtMobileNo2").val(obj.Phone2);
        $("#txtNameCustInfo").val(obj.Name);
        $("#txtReferenceId").val(obj.ReferenceId);
        $("#hdnBranchSmsMobNo").val(obj.BranchSmsMobNo);
        if (obj.IsStaff == 1) {
            $("#chkStaff").prop('checked', 'checked');
        } else {
            $("#chkStaff").removeProp('checked', 'checked');
        }

        $("#hdnBranchCode").val(obj.BranchCode);
        $("#hdnIsCustomerUpgraded").val(obj.IsUpgraded);
        $("#hdnSaleTypeId").val(obj.SaleTypeId);
        $("#txtPidCustInfo").val(obj.ProductId);
    },

};


