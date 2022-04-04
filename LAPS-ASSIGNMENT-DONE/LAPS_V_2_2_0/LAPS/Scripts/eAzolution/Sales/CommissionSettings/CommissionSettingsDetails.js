
var CommissionSettingsDetailsManager = {
    SaveCommissionSettings: function () {
        var validator = $("#CommissionDetailsDiv").kendoValidator().data("kendoValidator"),
       status = $(".status");
        if (validator.validate()) {
            var objCommission = CommissionSettingsDetailsHelper.CreateCommissionObj();
            var jsonParam = "objCommission:" + JSON.stringify(objCommission);
            var serverUrl = "../CommissionSettings/SaveCommissionSettings";
            AjaxManager.SendJson2(serverUrl, jsonParam, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Commission Settings Save Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            CommissionSettingsDetailsHelper.ClearCommissionSettingsForm();
                            $("#commissionSummaryGrid").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Exists") {

                AjaxManager.MsgBox('warning', 'center', 'Already Exists:', 'Commission Settings Already Exist',
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
    }

};

var CommissionSettingsDetailsHelper = {
    InitCommissionSettingsDetails: function() {
        empressCommonHelper.PopulateSaleRepType("cmbSaleRepType");
        CommissionSettingsDetailsHelper.PopulateSaleTypeCombo();
        $("#txtComissionAmount").kendoNumericTextBox({});
     
        $("#btnSave").click(function() {
            CommissionSettingsDetailsManager.SaveCommissionSettings();
        });

    },

    FillCommissionSettingsDetailsForm: function(obj) {
        $("#hdnCommissionId").val(obj.CommissionId);
        $("#cmbSaleRepType").data("kendoComboBox").value(obj.SaleRepTypeId);
        $("#cmbSaleType").data("kendoComboBox").value(obj.SaleTypeId);
        $("#txtComissionAmount").data("kendoNumericTextBox").value(obj.ComissionAmount);
        if (obj.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }
     
    },
    CreateCommissionObj: function() {
        var obj = new Object();
        obj.CommissionId = $("#hdnCommissionId").val();
        obj.SaleRepTypeId = $("#cmbSaleRepType").data("kendoComboBox").value();
        obj.SaleTypeId = $("#cmbSaleType").data("kendoComboBox").value();
        obj.ComissionAmount = $("#txtComissionAmount").data("kendoNumericTextBox").value();
        obj.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;
        return obj;
    },

    ClearCommissionSettingsForm: function() {
        $("#hdnCommissionId").val(0);
        $("#cmbSaleRepType").data("kendoComboBox").value("");
        $("#txtComissionAmount").data("kendoNumericTextBox").value("");
        $("#cmbSaleType").data("kendoComboBox").value("");
   
        $("#CommissionDetailsDiv > form").kendoValidator();
        $("#CommissionDetailsDiv").find("span.k-tooltip-validation").hide();
        $("#chkIsActive").removeProp('checked', 'checked');
        var status = $(".status");
        status.text("").removeClass("invalid");
    },

    PopulateSaleTypeCombo: function() {
        $("#cmbSaleType").kendoComboBox({
            placeholder: "Select Type",
            dataTextField: "SaleType",
            dataValueField: "SaleTypeId",
            dataSource: [{ SaleTypeId: 1, SaleType: "Installment Sale" }, { SaleTypeId: 2, SaleType: "Cash Sale" }],

            filter: "contains",
            suggest: true
        });
    }
};