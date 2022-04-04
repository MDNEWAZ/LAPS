$(document).ready(function () {
    OperationModeSettingsHelper.InitOperationSettings();

});


var OperationModeSettingsManager = {
    SaveOperationModeSettings: function () {
        var objOperationMode = OperationModeSettingsHelper.CreateOperationModeSettingsObj();
        if (OperationModeSettingsHelper.Validation()) {

            var operationModeObj = JSON.stringify(objOperationMode);
            var jsonParam = "operationModeObj:" + operationModeObj;
            var serviceUrl = "../OperationModeSettings/SaveOperationModeSettings/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }
        else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Plese Select Operation Mode',
                   [{
                       addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                           $noty.close();

                       }
                   }]);
        }

        function onSuccess(jsonData) {
                if (jsonData == "Success") {
                    AjaxManager.MsgBox('success', 'center', 'Success', 'Configured Successfully',
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
    
    GetOperationModeSettings:function() {
        var jsonParam = "";
        var serviceUrl = "../OperationModeSettings/GetOperationModeSettings/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            if (jsonData != null) {
                OperationModeSettingsHelper.FillOperationModeSettingsForm(jsonData);
            }
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    }
};


var OperationModeSettingsHelper = {
    InitOperationSettings: function () {
        OperationModeSettingsHelper.ChekcBoxClickEvent();
        $("#btnSaveOperationMode").click(function () {
            OperationModeSettingsManager.SaveOperationModeSettings();
        });
        $("#btnClear").click(function () {
            OperationModeSettingsHelper.ClearForm();
        });
        
        $("#btnRefresh").click(function () {
            OperationModeSettingsManager.GetOperationModeSettings();
        });
        OperationModeSettingsManager.GetOperationModeSettings();
    },

    ChekcBoxClickEvent: function () {
        $('#chkAutomatic').live('click', function (e) {
            var $cb = $(this);
            if ($cb.is(":checked")) {
                $("#autoOperationMode").show();
                $("#chkAutoSale").prop('checked', true);
                $("#chkAutoCollection").prop('checked', true);
                $("#chkAutoInventoryChecking").prop('checked', true);
            } else {
                $("#chkAutoSale").prop('checked', false);
                $("#chkAutoCollection").prop('checked', false);
                $("#chkAutoInventoryChecking").prop('checked', false);
                $("#autoOperationMode").hide();
            }

        });

        $('#chkManual').live('click', function (e) {
            var $cb = $(this);
            if ($cb.is(":checked")) {
                $("#manualOperationMode").show();
                $("#chkManualSale").prop('checked', true);
                $("#chkManualCollection").prop('checked', true);
                $("#chkManualInventoryChecking").prop('checked', true);
            } else {
                $("#chkManualSale").prop('checked', false);
                $("#chkManualCollection").prop('checked', false);
                $("#chkManualInventoryChecking").prop('checked', false);
                $("#manualOperationMode").hide();
            }

        });
    },

    ClearForm: function () {
        $("#chkAutomatic").prop('checked', false);
        $("#chkManual").prop('checked', false);
        $("#chkAutoSale").prop('checked', false);
        $("#chkAutoCollection").prop('checked', false);
        $("#chkManualSale").prop('checked', false);
        $("#chkManualCollection").prop('checked', false);
        $("#autoOperationMode").hide();
        $("#manualOperationMode").hide();
        $("#chkAutoInventoryChecking").prop('checked', false);
        $("#chkManualInventoryChecking").prop('checked', false);
    },

    Validation: function () {
        var obj = OperationModeSettingsHelper.CreateOperationModeSettingsObj();
        if (obj.AutoOperation != 1 && obj.ManualOperation != 1) {
            return false;
        } else {
            return true;

        }

    },

    CreateOperationModeSettingsObj: function () {
        var obj = new Object();
        obj.OperationModeId = $("#hdnOperationModeId").val();
        obj.AutoOperation = $("#chkAutomatic").is(":checked") == true ? 1 : 0;
        obj.ManualOperation = $("#chkManual").is(":checked") == true ? 1 : 0;
        obj.AutoSale = $("#chkAutoSale").is(":checked") == true ? 1 : 0;
        obj.AutoCollection = $("#chkAutoCollection").is(":checked") == true ? 1 : 0;
        obj.ManualSale = $("#chkManualSale").is(":checked") == true ? 1 : 0;
        obj.ManualCollection = $("#chkManualCollection").is(":checked") == true ? 1 : 0;
        obj.AutoInventoryChecking = $("#chkAutoInventoryChecking").is(":checked") == true ? 1 : 0;
        obj.ManualInventoryChecking = $("#chkManualInventoryChecking").is(":checked") == true ? 1 : 0;
        return obj;
    },
    
    FillOperationModeSettingsForm: function (obj) {
        $("#hdnOperationModeId").val(obj.OperationModeId);
        $("#autoOperationMode").show();
        $("#manualOperationMode").show();
        
        if (obj.AutoOperation == 1) {
            $("#chkAutomatic").prop('checked', 'checked');
        } else {
            $("#chkAutomatic").removeProp('checked', 'checked');
        }
        
        if (obj.ManualOperation == 1) {
            $("#chkManual").prop('checked', 'checked');
        } else {
            $("#chkManual").removeProp('checked', 'checked');
        }
        

        if (obj.AutoSale == 1) {
            $("#chkAutoSale").prop('checked', 'checked');
        } else {
            $("#chkAutoSale").removeProp('checked', 'checked');
        }
        
        if (obj.AutoCollection == 1) {
            $("#chkAutoCollection").prop('checked', 'checked');
        } else {
            $("#chkAutoCollection").removeProp('checked', 'checked');
        }
        
        if (obj.ManualSale == 1) {
            $("#chkManualSale").prop('checked', 'checked');
        } else {
            $("#chkManualSale").removeProp('checked', 'checked');
        }
        
        if (obj.ManualCollection == 1) {
            $("#chkManualCollection").prop('checked', 'checked');
        } else {
            $("#chkManualCollection").removeProp('checked', 'checked');
        }

        if (obj.AutoInventoryChecking == 1) {
            $("#chkAutoInventoryChecking").prop('checked', 'checked');
        } else {
            $("#chkAutoInventoryChecking").removeProp('checked', 'checked');
        }

        if (obj.ManualInventoryChecking == 1) {
            $("#chkManualInventoryChecking").prop('checked', 'checked');
        } else {
            $("#chkManualInventoryChecking").removeProp('checked', 'checked');
        }

    }
};
