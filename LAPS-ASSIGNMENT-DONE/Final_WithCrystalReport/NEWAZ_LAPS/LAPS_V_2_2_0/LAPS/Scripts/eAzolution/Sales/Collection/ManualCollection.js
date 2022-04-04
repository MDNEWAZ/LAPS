$(document).ready(function () {
   
    ManualCollectionHelper.GenerateDatePicker();
    ManualCollectionHelper.GeneratePaymentTypeCombo();
    $("#txtReceiveAmount").kendoNumericTextBox({format:"#",min:0});
    $("#btnSaveManualCollection").click(function () {
        ManualCollectionManager.SaveManualPaymentCollection();
    });

    $("#btnSearchInstallment").click(function () {
        ManualCollectionManager.GetNextInstallmentByInvoiceNo();
    });
});


var ManualCollectionManager = {
    SaveManualPaymentCollection: function () {
        var validator = $("#collectionDetailsDiv").kendoValidator().data("kendoValidator"),
           status = $(".status");
        if (validator.validate()) {
        var objColleciton = ManualCollectionHelper.CreateCollectionObject();

        var objPaycollection = JSON.stringify(objColleciton);
        var jsonParam = "objCollection:" + objPaycollection;
        var serviceUrl = "../Collection/UpdatePaymentStatus";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }
        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success', 'Payment Successfully.',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                            
                          //$("#gridInstalment").data("kendoGrid").dataSource.read();
                          //$("#collectionPopupWindow").data("kendoWindow").close();
                          ManualCollectionHelper.ClearFields();
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
    
    GetNextInstallmentByInvoiceNo:function () {
        var invoiceNo = $("#txtInvoiceNo").val();
     
        var jsonParam = "invoiceNo="+invoiceNo;
        var serviceUrl = "../Collection/GetNextInstallmentByInvoiceNo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
          
            ManualCollectionHelper.FillManualCollectionForm(jsonData);
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        
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
    }
};

var ManualCollectionHelper= {
    GenerateDatePicker: function () {
        $("#txtDueDate").kendoDatePicker({
            format: "dd-MMM-yyyy"
        });


        $("#txtPaymentDate").kendoDatePicker({
            format: "dd-MMM-yyyy",
            value:new Date()
        });
    },
    
    GeneratePaymentTypeCombo: function () {
        $("#cmbPaymentType").kendoComboBox({
            placeholder: "All",
            dataTextField: "Type",
            dataValueField: "TypeId",
            index:0,
            dataSource: ManualCollectionManager.GetPaymentTypeData()

        });
    },
    
    ClearFields: function () {
        $("#hdnInstallmentId").val(0);
    
        $("#txtInstallmentNo").val("");
        $("#txtDueDate").val("");
        $("#txtPaymentDate").val("");
        $("#txtReceiveAmount").data("kendoNumericTextBox").value("");
        $("#cmbPaymentType").data(kendoComboBox).value("");
        $("#txtTransactionNo").val("");
        $("#collectionDetailsDiv > form").kendoValidator();
        $("#collectionDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");
        status.text("").removeClass("invalid");
    },
    
    CreateCollectionObject: function () {
        var obj = new Object();

        obj.InstallmentId = $("#hdnInstallmentId").val();
     
        
        obj.SaleInvoice = $("#txtInvoiceNo").val();
        obj.InstallmentNo = $("#txtInstallmentNo").val();
        obj.TransectionId = $("#txtTransactionNo").val();
        obj.Amount = $("#txtReceiveAmount").val();
        obj.DueDate = $("#txtDueDate").val();
        obj.PayDate = $("#txtPaymentDate").val();
        //obj.AProduct = {
        //    ModelId: $("#hdnModelId").val(),
        //};
        
        obj.PaymentType = $("#cmbPaymentType").val();
        return obj;
    },
    
    FillManualCollectionForm: function (obj) {
      
         $("#hdnInstallmentId").val( obj.InstallmentId);
         $("#txtInstallmentNo").val(obj.Number);
         $("#txtReceiveAmount").data("kendoNumericTextBox").value(obj.Amount);
         $("#txtDueDate").val(obj.DueDate);
         //$("#hdnModelId").val(obj.AProduct.ModelId);
        
    }


}