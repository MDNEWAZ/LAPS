var moduleArray = [];

var groupPermisionManager = {
    GetGroupPermisionbyGroupId: function (groupId) {
        var objModule = "";
        var jsonParam = "groupId=" + groupId;
        var serviceUrl = "../Group/GetGroupPermisionbyGroupId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objModule = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objModule;
    }
    
};

var groupPermisionHelper = {
    populateModuleCombo: function (moduleId, moduleName, controlId) {
        
        var obj = new Object();
        obj.ReferenceID = moduleId;
        obj.ModuleName = moduleName;
        obj.PermissionTableName = "Module";
        $("#cmbApplicationForModule").val('');
        
        if ($("#" + controlId).is(':checked') == true) {
            moduleArray.push(obj);
        }
        else {
            for (var i = 0; i < moduleArray.length; i++) {
                if (moduleArray[i].ReferenceID == moduleId) {
                    
                    menuPermisionHelper.DeleteFormMenuByModuleId(moduleArray[i].ReferenceID);
                    accessControlHelper.RemoveAccessPermisionByModuleId(moduleArray[i].ReferenceID);
                    moduleArray.splice(i, 1);
                }
            }
        }
        
        $("#cmbApplicationForModule").kendoComboBox({
            placeholder: "Select a module",
            dataTextField: "ModuleName",
            dataValueField: "ReferenceID",
            select: groupPermisionHelper.onSelect,
            dataSource: moduleArray
        });

    },
    
    clearGroupPermisionForm: function () {
        moduleArray = [];
        $('#cmbApplicationForModule').val('');
        $("#cmbApplicationForModule").kendoComboBox({
            placeholder: "Select a module",
            dataTextField: "ModuleName",
            dataValueField: "ReferenceID",
            dataSource: moduleArray
        });
    },
    
    onSelect: function (e) {

        var dataItem = this.dataItem(e.item.index());
        menuPermisionHelper.PopulateMenuTreeByModuleId(dataItem.ReferenceID);
        accessControlHelper.GetAllAccessControl(dataItem.ReferenceID);
        var mdl = $("#cmbApplicationForModule").data("kendoComboBox");
        mdl.value(dataItem.ReferenceID);
    },
    
    CreateModulePermision: function (objGroup) {
        objGroup.ModuleList = moduleArray;
        return objGroup;
    },

    PopulateExistingModule: function (objGroup) {
        
        var objGroupPermision = groupPermisionManager.GetGroupPermisionbyGroupId(objGroup.GroupId);
        moduleArray = [];
        
        for (var i = 0; i < objGroupPermision.length; i++) {
            if (objGroupPermision[i].PermissionTableName == "Module") {

                for (var j = 0; j < allmoduleArray.length; j++) {
                    if (allmoduleArray[j].ModuleId == objGroupPermision[i].ReferenceID) {
                        $('#chkModule' + objGroupPermision[i].ReferenceID).attr('checked', true);
                        var obj = new Object();
                        obj.ReferenceID = objGroupPermision[i].ReferenceID;
                        obj.ModuleName = allmoduleArray[j].ModuleName;
                        obj.PermissionTableName = "Module";
                        moduleArray.push(obj);
                        break;
                    }
                }
            }
        }
        $("#cmbApplicationForModule").kendoComboBox({
            placeholder: "Select a module",
            dataTextField: "ModuleName",
            dataValueField: "ReferenceID",
            select: groupPermisionHelper.onSelect,
            dataSource: moduleArray
        });
        menuPermisionHelper.PopulateExistingMenuInArray(objGroupPermision);
        accessControlHelper.PopulateExistingAccessInArray(objGroupPermision);
        stateHelper.populateExistingStatusInArray(objGroupPermision);
        actionHelper.populateExistingActionInArray(objGroupPermision);
    }
    
};