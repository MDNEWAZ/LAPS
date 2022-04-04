var SattelDetailsManager = {
    fillCustomerInfo: function (customerCode, phoneNo) {


        var jsonParam = "customerCode=" + customerCode + "&phoneNo=" + phoneNo;
        var serviceUrl = "../Customer/GetCustomerByCustomerCode/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);


        function onSuccess(jsonData) {


        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
    },


    SaveSetteledCollection: function () {
        var validator = $("#sattelDetailsDiv").kendoValidator().data("kendoValidator"),
           status = $(".status");

        if (validator.validate()) {
            var obj = SattelDetailsHelper.CreateObjSettelDetails();

            obj = JSON.stringify(obj).replace(/&/g, "^");
            var jsonParam = 'objSetteledCollection:' + obj;
            var serviceUrl = "../UnsatteledCollection/SaveSetteledCollection/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Payment Setteled Successfully.',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            SattelDetailsHelper.ClearAllSattelDetails();
                        }
                    }]);

            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Failed', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Failed', error.statusText,
                         [{
                             addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                 $noty.close();
                             }
                         }]);
        }
    }

};


var SattelDetailsHelper = {
    FillASattelDetailsInForm: function (selectedItem) {
        $("#hdnSetteledId").val(selectedItem.ID);
    },

    CreateObjSettelDetails: function () {
        var obj = new Object();

        obj.SetteledId = $("#hdnSetteledId").val();
        obj.SaleInvoice = $("#txtInvoiceNo").val();
        obj.Phone = $("#txtMobileNumber").val();
        obj.ReceiveAmount = $("#txtReceiveAmount").data('kendoNumericTextBox').value();
        obj.CollectionType = $("#cmbPayType").val();
        obj.TransectionId = $("#txtTransactionNo").val();

        obj.PayDate = $("#txtPayDate").data('kendoDatePicker').value();

        return obj;
    },

    ClearAllSattelDetails: function () {
        $("#hdnSetteledId").val(0);
        $("#txtInvoiceNo").val("");
        $("#txtMobileNumber").val("");
        $("#txtReceiveAmount").data('kendoNumericTextBox').value("");
      
        $("#txtTransactionNo").val("");
        $("#txtPayDate").data('kendoDatePicker').value("");
        
        $("#sattelDetailsDiv > form").kendoValidator();
        $("#sattelDetailsDiv").find("span.k-tooltip-validation").hide();

        var status = $(".status");
        status.text("").removeClass("invalid");
    }
};