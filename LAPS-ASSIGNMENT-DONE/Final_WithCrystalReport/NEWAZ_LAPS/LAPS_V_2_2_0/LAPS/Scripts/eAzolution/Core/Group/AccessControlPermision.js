
var accessArray = [];
var accessPermisionArray = [];
var gbModuleId = 0;

var accessControlManager = {
    GetAllAccessControl: function () {

        var objAccessList = "";
        var jsonParam = "";
        var serviceUrl = "../Group/GetAllAccess/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objAccessList = jsonData;
        }
        function onFailed(error) {

            window.alert(error.statusText);
        }
        return objAccessList;
    }
};

var accessControlHelper = {
    GetAllAccessControl: function (moduleId) {
        accessArray = [];
        gbModuleId = moduleId;
        var objAccessList = accessControlManager.GetAllAccessControl();
        var link = "";

        for (var i = 0; i < objAccessList.length; i++) {
            link += "<div><input type=\"checkbox\" class=\"chkBox\" id=\"chkAccess" + objAccessList[i].AccessId + "\" onclick=\"accessControlHelper.populateAccessPermisionArray(" + objAccessList[i].AccessId + ", '" + objAccessList[i].AccessName + "', this.id)\"/> " + objAccessList[i].AccessName + "</div>";
            accessArray.push(objAccessList[i]);
        }
        $("#checkboxAccess").html(link);
        accessControlHelper.checkExistingAccessPermision(moduleId);
    },
    
    populateAccessPermisionArray: function (accessId,accessName) {
        
        if ($("#chkAccess" + accessId).is(':checked') == true) {
            var obj = new Object();
            obj.ReferenceID = accessId;
            obj.ParentPermission = gbModuleId;
            obj.PermissionTableName = "Access";
            accessPermisionArray.push(obj);
        }
        else {
            for (var i = 0; i < accessPermisionArray.length; i++) {
                if (accessPermisionArray[i].ReferenceID == accessId && accessPermisionArray[i].ParentPermission == gbModuleId) {
                    accessPermisionArray.splice(i, 1);
                    break;
                }
            }
        }
    },
    
    CreateAccessPermision: function (objGroupInfo) {
        objGroupInfo.AccessList = accessPermisionArray;
        return objGroupInfo;
    },
    
    clearAccessPermision: function () {
        accessPermisionArray = [];
        gbModuleId = 0;
        $('.chkBox').attr('checked', false);
    },
    PopulateExistingAccessInArray: function (objGroupPermision) {
        accessPermisionArray = [];
        for (var i = 0; i < objGroupPermision.length; i++) {
            if (objGroupPermision[i].PermissionTableName == "Access") {
                var obj = new Object();
                obj.ReferenceID = objGroupPermision[i].ReferenceID;
                obj.ParentPermission = objGroupPermision[i].ParentPermission;
                obj.PermissionTableName = "Access";
                accessPermisionArray.push(obj);
                //$('#chkAccess' + objGroupPermision[i].ReferenceID).attr('checked', true);
            }
        }
    },
    
    RemoveAccessPermisionByModuleId: function (moduleId) {
        for (var i = 0; i < accessPermisionArray.length; i++) {
            if (accessPermisionArray[i].ParentPermission == moduleId) {
                accessPermisionArray.splice(i, 1);
                i = i - 1;
            }
        }
    },
    checkExistingAccessPermision: function (moduleId) {
        
       
            for (var j = 0; j < accessPermisionArray.length; j++) {
                if (accessPermisionArray[j].ParentPermission == moduleId) {
                    $('#chkAccess' + accessPermisionArray[j].ReferenceID).attr('checked', true);
                    
                }
            }
        
    }

};