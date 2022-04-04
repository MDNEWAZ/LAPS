var actionArray = [];

var actionPermisionArray = [];


var actionManager = {
    
    GetActionByStatusId: function (statusId) {

        var objActionlistList = "";
        var jsonParam = "statusId=" + statusId;
        var serviceUrl = "../Status/GetActionByStatusIdForGroup/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objActionlistList = jsonData;
        }
        function onFailed(error) {

            window.alert(error.statusText);
        }
        return objActionlistList;
    }

};

var actionHelper = {
    GetActionByStatusId: function (statusId) {
        actionArray = [];

        var objActionList = actionManager.GetActionByStatusId(statusId);
        var link = "";

        for (var i = 0; i < objActionList.length; i++) {
            link += "<div class='actionCheck'><input type=\"checkbox\" class=\"chkBox\" id=\"chkAction" + objActionList[i].WFActionId + "\" onclick=\"actionHelper.populateActionArray(" + objActionList[i].WFActionId + ", '" + objActionList[i].WFStateId + "', this.id)\"/> " + objActionList[i].ActionName + "</div>";
            stateArray.push(objActionList[i]);
        }
        $("#checkboxActionPermision").html(link);
        actionHelper.checkedExistingActionChangeByStatus(statusId);

    },
    populateActionArray: function (actionId,statusId) {
        if ($("#chkAction" + actionId).is(':checked') == true) {
            var obj = new Object();
            obj.ReferenceID = actionId;
            obj.ParentPermission = statusId;
            obj.PermissionTableName = "Action";
            actionPermisionArray.push(obj);
        }
        else {
            for (var i = 0; i < actionPermisionArray.length; i++) {
                if (actionPermisionArray[i].ReferenceID == actionId) {
                    actionPermisionArray.splice(i, 1);
                    break;
                }
            }
        }
    },
    checkedExistingActionChangeByStatus: function (statusId) {
        
        for (var i = 0; i < actionPermisionArray.length; i++) {
            if (actionPermisionArray[i].PermissionTableName == "Action" && actionPermisionArray[i].ParentPermission == statusId) {
                $('#chkAction' + actionPermisionArray[i].ReferenceID).attr('checked', true);
            }
        }
    },
    CreateActionPermision: function (objGroupInfo) {
        objGroupInfo.ActionList = actionPermisionArray;
        return objGroupInfo;
    },

    clearActionPermision: function () {
        actionPermisionArray = [];
        $('.chkBox').attr('checked', false);
    },
    populateExistingActionInArray: function (objGroupPermision) {
        actionPermisionArray = [];
        
        for (var i = 0; i < objGroupPermision.length; i++) {
            if (objGroupPermision[i].PermissionTableName == "Action") {
                var obj = new Object();
                obj.ReferenceID = objGroupPermision[i].ReferenceID;
                obj.ParentPermission = objGroupPermision[i].ParentPermission;
                obj.PermissionTableName = "Action";
                actionPermisionArray.push(obj);
            }
        }
    },
    
    RemoveActionByStatusId: function (statusId) {
        for (var i = 0; i < actionPermisionArray.length; i++) {
            if (actionPermisionArray[i].ParentPermission == statusId) {
                actionPermisionArray.splice(i, 1);
                i = i - 1;
            }
        }
        $("#checkboxActionPermision").html("");
    }
};

