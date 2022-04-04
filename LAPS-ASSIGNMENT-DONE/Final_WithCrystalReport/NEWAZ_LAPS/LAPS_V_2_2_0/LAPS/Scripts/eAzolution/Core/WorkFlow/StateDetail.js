var stateDetailManager = {

    GetAllMenu: function () {
        var menu = "";
        var serviceUrl = "../Menu/SelectAllMenu/";
        var jesonParem = "";
        AjaxManager.GetJsonResult(serviceUrl, jesonParem, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            menu = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }

        return menu;
    },
    
    SaveInfo: function () {
        if (stateDetailHelper.StateDetailsValidator()) {

            //var stateID = $("#stateID").val();
            //alert(stateID);

            var state = new Object();
            state.WFStateId = $("#stateID").val() == '' ? '0' : $("#stateID").val();
            state.StateName = $("#txtStateName").val();
            state.MenuID = $("#cmbMenu").val() == '' ? '0' : $("#cmbMenu").val();
            state.MenuName = ""; //$("#cmbMenu").data("kendoComboBox");
            state.IsClosed = $("#cmbIsClose").val();

            if ($("#chkIsDefault").is(':checked') == true) {
                state.IsDefaultStart = true;
            } else {
                state.IsDefaultStart = false;
            }

            
            var stateObj = JSON.stringify(state).replace(/&/g, "^");
            var jsonParam = 'stateObj=' + stateObj;
            var serviceUrl = "../Status/SaveState/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
            
        }

        function onSuccess(jsonData) {
                    //var js = jsonData.split('"');
                    if (jsonData == "Success") {

                        AjaxManager.MsgBox('success', 'center', 'Success:', 'Operation Successfully',
                            [{
                                addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                    $noty.close();
                                    stateDetailManager.clearForm();
                                    $("#gridworkFlow").data("kendoGrid").dataSource.read();
                                }
                            }]);

                    }
                    else {
                        AjaxManager.MsgBox('error', 'center', 'Operation Failed', jsonData,
                                [{
                                    addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                        $noty.close();
                                    }
                                }]);
                    }
                }

                function onFailed(error) {
                    AjaxManager.MsgBox('error', 'center', 'Operation Failed', error.statusText,
                                [{
                                    addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                        $noty.close();
                                    }
                                }]);
                }



    },
    
    clearForm: function () {
        
       $("#cmbMenu").data("kendoComboBox").text('');
        $("#cmbIsClose").data("kendoComboBox").text('');
        $("#txtStateName").val('');
        $('#chkIsDefault').attr('checked', false);

        $("#stateID").val('');
        //$("#actionID").val('');

        $("#gridAction").empty();
        $("#gridAction").kendoGrid();
        actionDetailManager.clearActionForm();


        stateDetailManager.clearValidatorMsg();
    },
    

    clearValidatorMsg: function () {
        //debugger;
        //var win = $("#divStateDetails").kendoWindow({
        //    visible: true,
        //    width: 500
        //}).data("kendoWindow");
        $("#divStateDetails > form").kendoValidator();
        $("#divStateDetails").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");

        //win.open();
        //$("#openEntry").click(function () {
        //    win.element.find("span.k-tooltip-validation").hide();
        //    win.open();
        //});
    }



};



var stateDetailHelper = {
    LoadMenuCombo: function () {
        
        //return objEmployee;
        var menu = stateDetailManager.GetAllMenu();
        $("#cmbMenu").kendoComboBox({
            placeholder: "Select Menu...",
            dataTextField: "MenuName",
            dataValueField: "MenuId",
            dataSource: menu
        });
    },

    LoadIsCloseCombo: function () {
        
        $("#cmbIsClose").kendoComboBox({
            placeholder: "Select from List",
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Open", value: "0" },
                { text: "Possible Close", value: "1" },
                { text: "Close", value: "2" },
                { text: "Destroyed", value: "3" }
            ],
            filter: "contains",
            suggest: true
        });
    },
    
    StateDetailsValidator: function () {
        var data = [];
        var validator = $("#divStateDetails").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            
            var menu = $("#cmbMenu").data("kendoComboBox").value();
            var stateName = $("#txtStateName").val();
            var isClosed = $("#cmbIsClose").data("kendoComboBox").value();
            if (!AjaxManager.isDigit(menu)) {
                alert("Please select Valid Manu.");
                return false;
            }
            else if (stateName == null || stateName == "" || stateName == undefined) {
                alert("Please input State Name.");
                return false;
            }
            else if (!AjaxManager.isDigit(isClosed) || isClosed > 3) {
                alert("Please select Valid Value for IsClose.");
                return false;
            }


            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },

};



