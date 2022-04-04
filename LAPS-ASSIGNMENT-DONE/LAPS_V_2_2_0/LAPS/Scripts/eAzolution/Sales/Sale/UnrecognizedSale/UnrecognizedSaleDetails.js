
var UnrecognizedSaleDetailsManager = {

    InitUnrecognizedSaleDetails: function () {
        empressCommonHelper.GenerateModelCombo("cmbPackage");
        // $("#txtCollectedAmount").kendoNumericTextBox();
        $("#txtTransactionDate").kendoDatePicker();

        $("#btnSubmit").click(function () {
            UnrecognizedSaleDetailsManager.SaveUnrecognizedSale();
        });
        $("#btnClear").click(function () {
            UnrecognizedSaleDetailsHelper.ClearUnrecognizedSaleDetailsForm();
        });
        


       
    },

    SaveUnrecognizedSale: function () {
        var unrecognizeSaleObj = UnrecognizedSaleDetailsHelper.CreateUnrecognizedSaleObj();
        var unrecogSaleObject = JSON.stringify(unrecognizeSaleObj);
        var jsonParam = "unrecogSaleObject:" + unrecogSaleObject;
        var serviceUrl = "../Sale/SaveUnrecognizedSale/";

        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                $("#gridUnrecognizedSale").data("kendoGrid").dataSource.read();
                UnrecognizedSaleDetailsHelper.ClearUnrecognizedSaleDetailsForm();
                $("#popupUnrecognizedSaleDetails").data("kendoWindow").close();
                AjaxManager.MsgBox('success', 'center', 'Success', 'Data Updated Successfully',
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
    }


};



var UnrecognizedSaleDetailsHelper = {
    FillUnrecognizedSaleDetailsForm: function (obj) {
        
        UnrecognizedSaleDetailsHelper.ClearUnrecognizedSaleDetailsForm();
        //have to set user level for zonal manager  24
        if (gbUserLevel == 24 && (obj.TypeOfUnRecognized == 1 || obj.TypeOfUnRecognized == 2)) {
            $("#btnSubmit").hide();
            $("#btnClear").hide();
        } else {
            if (gbIsViewer != 1) {
                $("#btnSubmit").show();
            }
            $("#btnClear").show();
        }

        $("#hdnSaleId").val(obj.SaleId);
        $("#txtPrice").val(obj.Price);
        $("#txtSInvoiceNo").html(obj.Invoice);
        $("#txtCustomerCode").html(obj.ACustomer.CustomerCode);
        $("#txtCustomerName").html(obj.ACustomer.Name);
        $("#txtNationalId").html(obj.ACustomer.Nid == null ? "Not Available" : obj.ACustomer.Nid);
        $("#txtMobileNo1").val(obj.ACustomer.Phone);
        $("#txtMobileNo2").val(obj.ACustomer.Phone2);
        $("#cmbPackage").data("kendoComboBox").value(obj.AProduct.ModelId);
        $("#txtBranchCode").val(obj.ACustomer.BranchCode);
        $("#txtExpectedDownPayment").val(obj.DownPay);
        $("#txtCollectedAmount").val(obj.ReceiveAmount);

       // $("#txtTransactionId").val(obj.TransectionId);
        $("#txtTransactionDate").data("kendoDatePicker").value(obj.PayDate);
        $("#txtState").val(obj.State);
        $("#txtTempState").val(obj.TempState);
        $("#txtTypeOfUnRecognized").val(obj.TypeOfUnRecognized);
        if (obj.TypeOfUnRecognized == 1) { //SD 
            $("#txtNewAmount").attr("readonly", true);
        }
        if (obj.TypeOfUnRecognized == 2) { //DP
            $("#txtNewAmount").attr("readonly", false).css("background-color:Red");
        }
        if (obj.TypeOfUnRecognized == 3) { //SMS Error
            $("#txtNewAmount").attr("readonly", true);
        }
        if (obj.TypeOfUnRecognized == 4) { //Others
            $("#txtNewAmount").attr("readonly", true);
        }
        if (obj.TypeOfUnRecognized == 5) { //2nd Sale By SMS
            $("#txtNewAmount").attr("readonly", true);
        }
        $("#txtIsSpecialDiscount").val(obj.IsSpecialDiscount);
        $("#txtIsApprovedSpecialDiscount").val(obj.IsApprovedSpecialDiscount);

        $("#txtComments").val(obj.Comments);
    },

    ClearUnrecognizedSaleDetailsForm: function () {
        $("#hdnSaleId").val(0);
        $("#txtPrice").val(0);
        $("#txtSInvoiceNo").html("");
        $("#txtCustomerCode").html("");
        $("#txtCustomerName").html("");
        $("#txtNationalId").html("");
        $("#txtMobileNo1").val("");
        $("#txtMobileNo2").val("");
        $("#cmbPackage").data("kendoComboBox").value("");
        $("#txtBranchCode").val("");
        $("#txtExpectedDownPayment").val("");
        $("#txtCollectedAmount").val("");

       // $("#txtTransactionId").val("");
        $("#txtTransactionDate").data("kendoDatePicker").value("");

        $("#txtState").val("");
        $("#txtTempState").val("");
        $("#txtNewAmount").val("");
        $("#txtTypeOfUnRecognized").val("");
        $("#txtIsSpecialDiscount").val("");
        $("#txtIsApprovedSpecialDiscount").val("");
    },

    CreateUnrecognizedSaleObj: function () {
        var obj = new Object();
        obj.SaleId = $("#hdnSaleId").val();
        obj.Price=$("#txtPrice").val();
        obj.Invoice = $("#txtSInvoiceNo").html();
        obj.CustomerCode = $("#txtCustomerCode").html();
        obj.NationalId = $("#txtNationalId").html();
        obj.MobileNo1 = $("#txtMobileNo1").val();
        obj.MobileNo2 = $("#txtMobileNo2").val();
        obj.ModelId = $("#cmbPackage").val();
        obj.BranchCode = $("#txtBranchCode").val();
        obj.DownPay = $("#txtExpectedDownPayment").val();
        obj.ReceiveAmount = $("#txtCollectedAmount").val();
       // obj.TransectionId = $("#txtTransactionId").val();
        obj.TransactionDate = $("#txtTransactionDate").data("kendoDatePicker").value();
        obj.State = $("#txtState").val();
        obj.TempState = $("#txtTempState").val();
        obj.TypeOfUnRecognized=$("#txtTypeOfUnRecognized").val();
        obj.NewCollectedAmount = $("#txtNewAmount").val() == "" ? 0 : $("#txtNewAmount").val();
        obj.IsSpecialDiscount = $("#txtIsSpecialDiscount").val();
        obj.IsApprovedSpecialDiscount= $("#txtIsApprovedSpecialDiscount").val();
        return obj;

    },

};


