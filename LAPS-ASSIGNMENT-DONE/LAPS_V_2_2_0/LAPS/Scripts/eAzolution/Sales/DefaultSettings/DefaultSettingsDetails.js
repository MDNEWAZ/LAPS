var gbInterestName = "";

var DefaultSettingsDetailsManager = {
    InitDefaultSettingsDetails: function () {
        $("#txtDefaultCashDiscount").kendoNumericTextBox({ format: "{0:#.##}" });
        $("#txtDefaultCashDiscountInPercentage").kendoNumericTextBox({ format: "{0:#.##}" });
        empressCommonHelper.GenerareHierarchyCompanyCombo("cmbInterestCompanyName");
        //if (CurrentUser != null) {
        //    $("#cmbInterestCompanyName").data("kendoComboBox").value(CurrentUser.CompanyId);
        //}

        $("#cmbInterestCompanyName").change(function() {
            DefaultSettingsDetailsHelper.ClearForms();
        });
    },
    SaveDefaultSettings: function () {
        var objInterest = DefaultSettingsDetailsManager.GetAInterest_Object();
        if (objInterest.Interests !="" && objInterest.DownPay!="") {
            var objInterestInfo = JSON.stringify(objInterest).replace(/&/g, "^");
            var jsonParam = 'objInterestInfo:' + objInterestInfo;
            var serviceUrl = "../DefaultSettings/SaveDefaultSettings/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Minimum Requirement:', 'Please Enter Interest And DownPayment Information.',
                   [{
                       addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                           $noty.close();
                           return false;
                       }
                   }]);
        }
        
        function onSuccess(jsonData) {
            
            if (jsonData == "Success") {
                DefaultSettingsDetailsHelper.clearDefaultSettingsForm();
            
                AjaxManager.MsgBox('success', 'center', 'Success', 'Default Settings Saved Successfully.',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                          $("#gridInterest").data("kendoGrid").dataSource.read();
                      }
                  }]);
               
            }
            else if (jsonData == "Exists") {
                AjaxManager.MsgBox('warning', 'center', 'Already Exist:', 'Settings already configured for this Company!',
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
    GetAInterest_Object: function () {
        var objInterest = new Object();
        objInterest.InterestId = $('#hdnInterestId').val();
        objInterest.ACompany = { CompanyId: $('#cmbInterestCompanyName').val() };
        objInterest.Interests = $("#txtInterests").val();
        objInterest.DownPay = $("#txtDownPay").val();
        objInterest.DefaultInstallmentNo = $("#txtDefaultInstallmentNo").val();
        objInterest.Status = $("#chkIsActiveInterest").is(":checked") == true ? 1 : 0;
        objInterest.DefaultCashDiscount = $("#txtDefaultCashDiscount").val();
        objInterest.CashDiscountPercentage = $("#txtDefaultCashDiscountInPercentage").val();
        return objInterest;
    }
};

var DefaultSettingsDetailsHelper = {
    FillDefaultSettingsDetailsInForm: function (objInterest) {
        DefaultSettingsDetailsHelper.clearDefaultSettingsForm();
       
        $('#hdnInterestId').val(objInterest.InterestId);
        $("#cmbInterestCompanyName").data('kendoComboBox').value(objInterest.ACompany.CompanyId);
        $("#txtInterests").val(objInterest.Interests);
        $("#txtDownPay").val(objInterest.DownPay);
        $("#txtDefaultInstallmentNo").val(objInterest.DefaultInstallmentNo);
        if (objInterest.Status == 1) {
            $("#chkIsActiveInterest").prop('checked', 'checked');
        } else {
            $("#chkIsActiveInterest").removeProp('checked', 'checked');
        }
        $("#txtDefaultCashDiscount").data("kendoNumericTextBox").value(objInterest.DefaultCashDiscount);
        $("#txtDefaultCashDiscountInPercentage").data("kendoNumericTextBox").value(objInterest.CashDiscountPercentage);
        
    },
   
    
    clearDefaultSettingsForm: function () {
        $('#cmbInterestCompanyName').data('kendoComboBox').value('');
        DefaultSettingsDetailsHelper.ClearForms();
    },
    
    ClearForms:function() {
        $('#hdnInterestId').val('');
        $('#txtInterests').val('');
        $("#txtDownPay").val('');
        $("#txtDefaultInstallmentNo").val('');
        $('#chkIsActiveInterest').removeAttr('checked', 'checked');
        $("#txtDefaultCashDiscount").data("kendoNumericTextBox").value("");
        $("#txtDefaultCashDiscountInPercentage").data("kendoNumericTextBox").value("");
     
    }
   
};