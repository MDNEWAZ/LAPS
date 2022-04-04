
var SalesRollbackDetailManager = {

    SaleRollbackDetailsInit: function () {

        $("#btnSearchSaleInfo").click(function () {
            SalesRollbackSummaryHelper.PopulateSalesInforamtionSummaryGrid();
        });

        $("#txtCustomerCode").keypress(function (event) {
            if (event.keyCode == 13) {
                SalesRollbackSummaryHelper.PopulateSalesInforamtionSummaryGrid();
            }
        });



    },

    MakeInActive: function (operationType) {
        var rollbackInfoObj = SalesRollbackInfoHelper.CreateRollebackInfoObject(operationType);
            var rollbackInfo = JSON.stringify(rollbackInfoObj);

            var jsonParam = "rollbackInfo:" + rollbackInfo + ",operationType:" + operationType;
            var serviceUrl = '../SalesRollback/MakeInactive/';
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success', 'Sale Rollback Successfully',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                             $("#salesRollbackPopupDiv").data("kendoWindow").close();
                             SalesRollbackInfoHelper.ClearSaleRollbackInfoForm();
                             $("#gridSaleInforamtion").data("kendoGrid").dataSource.read();
                         }
                     }]);
            }
            else if (jsonData == "AccessDenied") {
                AjaxManager.MsgBox('warning', 'center', 'Warning', 'Not Permitted to inactive',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                             $("#salesRollbackPopupDiv").data("kendoWindow").close();
                             SalesRollbackInfoHelper.ClearSaleRollbackInfoForm();
                             $("#gridSaleInforamtion").data("kendoGrid").dataSource.read();
                         }
                     }]);
            }

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    }
};

var SalesRollbackDetailHelper = {

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