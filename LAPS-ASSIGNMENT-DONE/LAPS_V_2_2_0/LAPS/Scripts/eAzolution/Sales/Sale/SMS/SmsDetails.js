var SmsDetailsHelper = {

    FillUnrecognizedSmsIntoPopip: function (data) {
        $("#smsId").val(data.SalesSmsId);
        $("#txtBranchCode").val(data.BranchCode);
        $("#txtPackage").val(data.Package);
        $("#txtMobileNo").val(data.MobileNo1);
    },
    ClosePopUp: function () {
        $("#smsPopup").data("kendoWindow").close();
    },
    UpdateSms: function () {
        var data = SmsDetailsManager.GetSmsDataForEdit();
        var smsObj = JSON.stringify(data);
        var jsonParam = "sms:" + smsObj;
        var serviceUrl = "../Sms/EditSms/";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success', 'SMS Updated Successfully',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                      }
                  }]);
                $("#smsSummary").data("kendoGrid").dataSource.read();
                SmsDetailsHelper.ClosePopUp();
            }

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

};
var SmsDetailsManager = {
    GetSmsDataForEdit: function () {
        var data = new Object();
        data.SalesSmsId = $("#smsId").val();
        data.BranchCode = $("#txtBranchCode").val();
        data.Package = $("#txtPackage").val();
        data.MobileNo1 = $("#txtMobileNo").val();

        return data;
    }
};