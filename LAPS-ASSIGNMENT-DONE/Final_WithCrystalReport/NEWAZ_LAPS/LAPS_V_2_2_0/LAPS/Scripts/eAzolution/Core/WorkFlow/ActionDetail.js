var actionDetailManager = {

    gridDataSource: function (stateId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",

            serverPaging: true,
            serverSorting: true,

            pageSize: 100,
            //page: 1,

            transport: {
                read: {

                    url: '../Status/GetActionByStatusId/?statusId=' + stateId,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: {
                data: "Items", total: "TotalCount",
                model: {
                }

            }


        });

        return gridDataSource;

    },
    

    SaveActionInfo: function () {
        if (actionDetailHelper.ActionDetailsValidator()) {

            //var stateID = $("#stateID").val();
            //alert(stateID);
            //var stateName = $("#txtStateName_Action").val();
            
            var action = new Object();
            
            action.WFActionId = $("#actionID").val() == '' ? '0' : $("#actionID").val();
            action.ActionName = $("#txtActionName").val();
            action.WFStateId = $("#stateID").val() == '' ? '0' : $("#stateID").val();
            action.StateName = "";
            action.NextStateId = $("#cmbNextState").val() == '' ? '0' : $("#cmbNextState").val();
            action.NextStateName = "";
            
            if ($("#chkIsEmail").is(':checked') == true) {
                action.EmailNotification = 1;
            } else {
                action.EmailNotification = 0;
            }
            if ($("#chkIsSms").is(':checked') == true) {
                action.SMSNotification = 1;
            } else {
                action.SMSNotification = 0;
            }

            action.IsDefaultStart = "0";
            action.IsClosed = "0";


            var ActionObj = JSON.stringify(action).replace(/&/g, "^");
            var jsonParam = 'ActionObj=' + ActionObj;
            var serviceUrl = "../Status/SaveAction/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
            //$("#txtStateName_Action").val(stateName);
        }

        function onSuccess(jsonData) {
            //var js = jsonData.split('"');
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Operation Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            actionDetailManager.clearActionForm();
                            $("#gridAction").data("kendoGrid").dataSource.read();
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
    
    DeleteActionById: function (actionId) {
        
        var confirmed = window.confirm("Are you sure to Delete this action?");
        if (confirmed) {

            var jsonParam = "actionId=" + actionId;
            var serviceURL = "../Status/DeleteStatusByActionId";

            AjaxManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);
        }
        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success', "Action Delete Successfully!",
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function($noty) {
                            $noty.close();
                            $("#gridAction").data("kendoGrid").dataSource.read();
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

    clearActionForm: function () {
        //alert("test Clear button click");

        $("#txtStateName_Action").val('');
        $("#txtActionName").val('');

        try {
            $("#cmbNextState").data("kendoComboBox").text('');
        } catch (e) {

        } 

        $('#chkIsEmail').attr('checked', false);
        $('#chkIsSms').attr('checked', false);

        //$("#stateID").val('');
        $("#actionID").val('');

        actionDetailManager.clearActionValidatorMsg();
    },
    clearActionValidatorMsg: function () {
        //debugger;
        //var win = $("#divStateDetails").kendoWindow({
        //    visible: true,
        //    width: 500
        //}).data("kendoWindow");
        $("#ActionDetaildivInfo > form").kendoValidator();
        $("#ActionDetaildivInfo").find("span.k-tooltip-validation").hide();
        var status = $(".status");
        status.text("").removeClass("invalid");

        //win.open();
        //$("#openEntry").click(function () {
        //    win.element.find("span.k-tooltip-validation").hide();
        //    win.open();
        //});
    }


};



var actionDetailHelper = {
    GenerateActionGrid: function (stateId) {
        //var objDataSource = actionDetailManager.gridDataSource(stateId);
        $("#gridAction").kendoGrid({
            dataSource: actionDetailManager.gridDataSource(stateId),
            

            autoBind: true,

            filterable: false,
            sortable: false,
            columns: actionDetailHelper.GenerateActionColumns(),
            editable: false,
            scrollable: true,
            navigatable: true,
            selectable: "row"

        });
    },
    
    GenerateActionColumns: function () {
        return columns = [
            { field: "WFActionId", hidden: true },
            { field: "ActionName", title: "Action Name", width: 80 },
            { field: "WFStateId", hidden: true },
             //{ field: "StateName", title: "State Name", width: 60 },
             { field: "StateName", hidden: true },
            
            { field: "NextStateId", hidden: true },
             { field: "NextStateName", title: "Next State", width: 80 },
           
            { field: "EmailNotification", title: "Email", width: 60, template: "#= EmailNotification == 1 ? 'Yes' : 'No' #" },
            { field: "SMSNotification", title: "SMS", width: 60, template: "#= SMSNotification == 1 ? 'Yes' : 'No' #" },
            //{ field: "EmailNotification", title: "Email", width: 60 },
            //{ field: "SMSNotification", title: "SMS", width: 60 },
            
            { field: "IsDefaultStart", hidden: true },
            { field: "IsClosed", hidden: true },
            { field: "Edit", title: "#", filterable: false, width: 110, template: '<input type="button" class="k-button" value="Edit" id="btnEditAction" onClick="actionDetailHelper.clickEventForEditActionButton()"  /><input type="button" class="k-button" value="Delete" id="btnDeleteAction" onClick="actionDetailHelper.clickEventForDeleteActionButton()"  />' }
        ];
    },
    
    LoadNextStateCombo: function (MenuID) {

        //return objEmployee;
        //var States = actionDetailManager.GetAllStates(menuID);
        var States = "";
        var serviceUrl = "../Status/GetStatusByMenuId/?menuId=" + MenuID;
        var jesonParem = "";
        AjaxManager.GetJsonResult(serviceUrl, jesonParem, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            States = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }


        //$("#cmbNextState").kendoComboBox({ dataSource: null });
            

        $("#cmbNextState").kendoComboBox({
            placeholder: "Select Next State...",
            dataTextField: "StateName",
            dataValueField: "WFStateId",
            dataSource: States
        });
    },

    clickEventForEditActionButton: function () {
        //$('#btnEdit').live('click', function () {

        //});
        var entityGrid = $("#gridAction").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        actionDetailHelper.EditActionItem(selectedItem);

    },
    
    clickEventForDeleteActionButton: function () {
        var entityGrid = $("#gridAction").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        actionDetailManager.DeleteActionById(selectedItem.WFActionId);
    },

    EditActionItem: function (items) {
        //workFlowManager.ResetPasswordByCompanyIdAndworkFlowId(items.CompanyId, items.workFlowId);
        //$('#cmbMenu').val = items.MenuID;
        var nextState = $("#cmbNextState").data("kendoComboBox");
        nextState.value(items.NextStateId);
        $("#txtStateName_Action").val(items.StateName);
        $("#txtActionName").val(items.ActionName);

        $('#chkIsEmail').attr('checked', items.EmailNotification == 1 ? true : false);
        $('#chkIsSms').attr('checked', items.SMSNotification == 1 ? true : false);

        $("#actionID").val(items.WFActionId);

    },

    clickEventForEditAction: function () {
        //alert("Double Clicked");
        $('#gridAction table tr').live('dblclick', function () {

            var entityGrid = $("#gridAction").data("kendoGrid");
            var selectedItem = entityGrid.dataItem(entityGrid.select());
            //workFlowInfoHelper.populateworkFlowInformationDetails(selectedItem);

            //groupMembershipHelper.populateGroupMember(selectedItem);

            actionDetailHelper.EditActionItem(selectedItem);

        });
    },

    ActionDetailsValidator: function () {
        var data = [];
        var validator = $("#ActionDetaildivInfo").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");

            //var nextState = $("#cmbNextState").data("kendoComboBox").value();
            var nextState = $("#cmbNextState").val();
            var stateid = $("#stateID").val() == '' ? '0' : $("#stateID").val();
            if (!AjaxManager.isDigit(nextState)) {
                alert("Please select Valid Next State.");
                return false;
            }
            else if (stateid == '0') {
                alert("Please select Valid State before save.");
                return false;
            }


            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },



};