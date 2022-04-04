var BnakBranchDetailsManager = {
    SaveBankBranchInformation: function () {
    
        var validator = $("#bankbranchDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");

        if(validator.validate()) {
            var objBankBranch = BankBranchDetailsHelper.createBankBrachObject();
            var bankBranchObj = JSON.stringify(objBankBranch).replace('&', '^');
            var jsonParam = "bankBranchObj:" + bankBranchObj;
            var serviceUrl = "../BankBranch/SaveBankBranch";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }
   
        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Bank Branch Saved/Updated Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            BankBranchSummaryHelper.GenerateBankBranchGrid();
                            BankBranchDetailsHelper.ClearBankBranchDetails();
                        }
                    }]);
            }
            
            else if (jsonData == "Exists") {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Branch Name or Code Already Exists!',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function($noty) {
                            $noty.close();
                           
                        }
                    }]);
            } else {
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
  
    GetAllBank: function () {
        var objBank = "";
        var jsonParam = "";
        var serviceUrl = "../BankBranch/GetAllBank/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objBank = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objBank;
    },


};

var BankBranchDetailsHelper = {
    bankBranchDetailsInit: function () {
        BankBranchDetailsHelper.GenerateBankCombo();
        
        $("#btnSaveBankBranch").click(function () {
            BnakBranchDetailsManager.SaveBankBranchInformation();
        });

        $("#btnClearAll").click(function () {
            BankBranchDetailsHelper.ClearBankBranchDetails();
        });
    },
    
    GenerateBankCombo:function () {
        var objBank = new Object();
        objBank = BnakBranchDetailsManager.GetAllBank();
        $("#cmbBankName").kendoComboBox({
            placeholder: "Select Bank",
            dataTextField: "BankName",
            dataValueField: "BankId",
            dataSource: objBank
        });
    },
    
    createBankBrachObject:function () {
        var obj = new Object();
        obj.BankId = $("#cmbBankName").val();
        obj.BranchId = $("#hdnBranchId").val();
        obj.BranchName = $("#txtBranchName").val();
        obj.BranchCode = $("#txtBranchCode").val();
        obj.Address = $("#txtBranchAddress").val();
        obj.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;
        return obj;
    },
    
    ClearBankBranchDetails: function () {
        debugger;
        $("#hdnBranchId").val(0);
        $("#cmbBankName").data('kendoComboBox').value("");

        $("#txtBranchName").val("");
        $("#txtBranchCode").val("");
        $("#txtBranchAddress").val("");
        //    $("#chkIsActive").removeProp('checked', 'checked');
   
        $("#chkIsActive").attr('checked', 'checked');

        $("#bankbranchDetailsDiv > form").kendoValidator();
        $("#bankbranchDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");
        status.text("").removeClass("invalid");
        
    },
    
    populateBankBranchDetails: function (obj) {
        
        $("#hdnBranchId").val(obj.BranchId);
        $("#cmbBankName").data('kendoComboBox').value(obj.BankId);
      
        $("#txtBranchName").val(obj.BranchName);
        $("#txtBranchCode").val(obj.BranchCode);
        $("#txtBranchAddress").val(obj.Address);
        if(obj.IsActive==1) {
            $("#chkIsActive").prop('checked','checked');
        }else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }
        
    }
};