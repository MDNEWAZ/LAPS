var ReScheduleDetailHelper = {
   
  
    CreateFirstPayDateCalender: function (ctrlId) {
        $("#" + ctrlId).kendoDatePicker({
            animation: false
        });
    },

    ChangeEventForCompanyCombo: function () {
        var comboboxbranch = $("#cmbBranch").data("kendoComboBox");
        var companyData = $("#cmbCompany").data("kendoComboBox");
        var companyId = companyData.value();
        var companyName = companyData.text();

        if (companyId == companyName) {
            companyData.value('');
            comboboxbranch.value('');
            comboboxbranch.destroy();
            empressCommonHelper.GenerateBranchCombo(0, "cmbBranch");
            return false;
        }
        if (comboboxbranch != undefined) {
            comboboxbranch.value('');
            comboboxbranch.destroy();
        }
        empressCommonHelper.GenerateBranchCombo(companyId, "cmbBranch");
    },
};
var ReScheduleDetailManager = {
  
    MakeReSchedule: function () {

        var installmentNo = $("#txtNewIm").val();
        var gridData = $("#gridInstallment").data("kendoGrid").dataSource.data();
      
        if (gridData.length > 0) {
            if (installmentNo != "") {
                var companyId = JSON.stringify($("#cmbCompany").val());
                var branchId = JSON.stringify($("#cmbBranch").val());
                var customerCode = JSON.stringify($("#txtCustomerCode").val());
                var ims = JSON.stringify(installmentNo);
                var firsyPayDate = JSON.stringify($("#txtFirstPayDate").val());

                var jsonParam = "companyId:" + companyId + ",branchId:" + branchId + ",customerCode:" + customerCode + ",ims:" + ims + ",firsyPayDate:" + firsyPayDate;
                var serviceUrl = '../Sale/MakeReSchedule/';
                AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please Enter Installment No !',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function($noty) {
                            $noty.close();
                            $("#txtNewIm").focus();
                        }
                    }]);
            }

        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'No Installment Data for Rechedule!',
                   [{
                       addClass: 'btn btn-primary',
                       text: 'Ok',
                       onClick: function ($noty) {
                           $noty.close();
                          
                       }
                   }]);
        }
       
        

      
        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success', 'Installments Updated Successfully',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                             $("#gridInstallment").data("kendoGrid").dataSource.read();
                         }
                     }]);
            }

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    }
};