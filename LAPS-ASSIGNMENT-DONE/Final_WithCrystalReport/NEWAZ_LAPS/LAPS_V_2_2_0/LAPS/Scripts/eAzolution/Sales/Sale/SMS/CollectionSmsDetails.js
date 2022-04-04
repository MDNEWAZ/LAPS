var CollectionSmsDetailsHelper = {
    FillCollectiondSmsIntoPopip: function (data) {
       
        $("#smsId").val(data.ID);
        $("#txtRecievedDate").val(kendo.toString(data.RecievedDate, "dd/MM/yyyy"));
        $("#txtFromMobileNumber").val(data.FromMobileNumber);
        $("#txthdnStatus").val(data.Status);
        if (data.Status == 2) {
            $("#txtStatus").val("Amount Excceded");
        }
        else if (data.Status == 3) {
            $("#txtStatus").val("Customer Not Found");
        }
        else if (data.Status == 4) {
            $("#txtStatus").val("Provider Not Found");
        }
        else if (data.Status == 5) {
            $("#txtStatus").val("SMS Format is Wrong or Invalid DateTime");
        }
        else if (data.Status == 9) {
            $("#txtStatus").val("SMS Format is Invalid or Mismatch Parameter");
        }
        else if (data.Status == 11) {
            $("#txtStatus").val("Stock Inventory Unavilable");
        } else {
            $("#txtStatus").val("NA");
        }
        $("#txtSMSText").val(data.SMSText);
    },
    ClosePopUp: function () {
        $("#smsPopup").data("kendoWindow").close();
    },
    UpdateSms: function () {
        var data = CollectionSmsDetailsManager.GetCollectionSmsDataForEdit();
        if (data.Status != 3) {
            AjaxManager.MsgBox('errorr', 'center', 'Error', 'You can update only "Customer Not Found" ',
                 [{
                     addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                         $noty.close();
                     }
                 }]);
            return 0;
        }

        var smsObj = JSON.stringify(data);
        var jsonParam = "sms:" + smsObj;
        var serviceUrl = "../Sms/EditCollectionSms/";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success', 'SMS Updated Successfully',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                      }
                  }]);
                $("#smsSummaryOfCollection").data("kendoGrid").dataSource.read();
                CollectionSmsDetailsHelper.ClosePopUp();
            }

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    GenerateReceiveDate: function () {

        $("#dtRecievedDateTo").kendoDatePicker({
            depth: "year",
            format: "dd-MM-yyyy",
        });

        $("#dtRecievedDateFrom").kendoDatePicker({
            depth: "year",
            format: "dd-MM-yyyy",
        });

        //$("#dtRecievedDateFrom").data("kendoDatePicker").value(new Date());
        //$("#dtRecievedDateTo").data("kendoDatePicker").value(new Date());
    }
};
var CollectionSmsDetailsManager = {
    GetCollectionSmsDataForEdit: function () {
        var data = new Object();
        data.ID = $("#smsId").val();
        data.Status= $("#txthdnStatus").val();
        data.SMSText = $("#txtSMSText").val();
        return data;
    }
};