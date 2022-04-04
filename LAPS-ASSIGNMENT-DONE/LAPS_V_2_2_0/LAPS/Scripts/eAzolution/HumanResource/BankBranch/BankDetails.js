
var BankDetailsManager = {
    SaveBankInformation: function () {
        var validator = $("#bankDetailsDiv").kendoValidator().data("kendoValidator"),
          status = $(".status");
        if(validator.validate()) {
            var objBank = BankDetailsHelper.createBankObject();
            var bankObj = JSON.stringify(objBank).replace('&', '^');
            var jsonParam = "bankObj:" + bankObj;
            var serviceUrl = "../BankBranch/SaveBank";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }
       
        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Bank Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            $("#popupWindowBankDetails").data("kendoWindow").close();
                            BankBranchDetailsHelper.GenerateBankCombo();
                            BankDetailsHelper.clearBankDetails();
                        }
                    }]);
            }
            else if (jsonData == "Exist") {

                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Bank Name or Code Already Exist!',
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
            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
        }
    },

};

var BankDetailsHelper = {
    bankDetailsInit:function () {
        $("#btnSaveBank").click(function () {
            BankDetailsManager.SaveBankInformation();
        }); 
        
        $("#btnClose").click(function () {
            $("#popupWindowBankDetails").data("kendoWindow").close();
        }); 
        
    },

    createBankObject: function () {
       
        var obj = new Object();
        obj.BankId = $("#hdnBankId").val();
        obj.BankName = $("#txtBankName").val();
        obj.BankCode = $("#txtBankCode").val();
      
        obj.IsActive = $("#chkIsActiveBank").is(":checked") == true ? 1 : 0;
        return obj;
    },
    
    clearBankDetails:function () {
        $("#hdnBankId").val(0);
        $("#txtBankName").val("");
        $("#txtBankCode").val("");
        $("#chkIsActiveBank").removeProp('checked','checked');
    }
}