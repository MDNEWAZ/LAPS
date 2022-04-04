
var stateArray = [];
var statePermisionArray = [];

var stateManager = {
    GetStatusByMenuId: function (menuId) {

        var objStatusList = "";
        var jsonParam = "menuId=" + menuId;
        var serviceUrl = "../Status/GetStatusByMenuId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objStatusList = jsonData;
        }
        function onFailed(error) {

            window.alert(error.statusText);
        }
        return objStatusList;
    }
    
};

var stateHelper = {
    GetStatusByMenuId: function (menuId) {
        stateArray = [];

        var objStatusList = stateManager.GetStatusByMenuId(menuId);
        var link = "";

        for (var i = 0; i < objStatusList.length; i++) {
            link += "<div><input type=\"checkbox\" class=\"chkBox\" id=\"chkStatus" + objStatusList[i].WFStateId + "\" onclick=\"stateHelper.populateStateArray(" + objStatusList[i].WFStateId + ", '" + objStatusList[i].MenuID + "', this.id)\"/>" +
                "<a class=\"alinkGroup\" title=\"View Action Permision\" href=\"#\" id=\"astatus" + objStatusList[i].WFStateId + "\" onclick=\"stateHelper.populateStateArray(" + objStatusList[i].WFStateId + ", '" + objStatusList[i].MenuID + "', this.id)\">" + objStatusList[i].StateName + "</a></div>";
            stateArray.push(objStatusList[i]);
        }
        $("#checkboxStatePermision").html(link);
        stateHelper.checkedExistingStatusChangeByMenu(menuId);

    },
    populateStateArray: function (stateId,menuId) {
      
        
        if($("#chkStatus" + stateId).is(':checked') == true) {
            var obj = new Object();
            obj.ReferenceID = stateId;
            obj.ParentPermission = menuId;
            obj.PermissionTableName = "Status";
            statePermisionArray.push(obj);
            $(".alinkGroup").removeClass("stateBackground");
            $("#astatus" + stateId).addClass("stateBackground");
      
            actionHelper.GetActionByStatusId(stateId);
        }
        else {
            for (var i = 0; i < statePermisionArray.length; i++) {
                if (statePermisionArray[i].ReferenceID == stateId) {
                    
                    actionHelper.RemoveActionByStatusId(stateId);

                    statePermisionArray.splice(i, 1);
                    break;
                }
            }
        }
    },
    
    checkedExistingStatusChangeByMenu: function (menuId) {
        
        for (var i = 0; i < statePermisionArray.length; i++) {
            if (statePermisionArray[i].PermissionTableName == "Status" && statePermisionArray[i].ParentPermission == menuId) {
                $('#chkStatus' + statePermisionArray[i].ReferenceID).attr('checked', true);
            }
        }
    },
    
    CreateStatusPermision: function (objGroupInfo) {
        objGroupInfo.StatusList = statePermisionArray;
        return objGroupInfo;
    },
    
    clearStatusPermision: function () {
        statePermisionArray = [];
        $('.chkBox').attr('checked', false);
    },
    populateExistingStatusInArray: function (objGroupPermision) {
        statePermisionArray = [];
        for (var i = 0; i < objGroupPermision.length; i++) {
            if (objGroupPermision[i].PermissionTableName == "Status") {
                var obj = new Object();
                obj.ReferenceID = objGroupPermision[i].ReferenceID;
                obj.ParentPermission = objGroupPermision[i].ParentPermission;
                obj.PermissionTableName = "Status";
                statePermisionArray.push(obj);
            }
        }
    },
    
    RemoveStatusByMenuId: function (menuId) {
        for (var i = 0; i < statePermisionArray.length; i++) {
            if (statePermisionArray[i].ParentPermission == menuId) {
                actionHelper.RemoveActionByStatusId(statePermisionArray[i].ReferenceID);
                statePermisionArray.splice(i, 1);
                i = i - 1;
            }
        }
        $("#checkboxStatePermision").html("");
    }

};