var groupDetailsManager = {
    SaveGroup: function () {
        if(groupDetailsHelper.validateGroupForm()) {
            
            var objGroupInfo = groupInfoHelper.CreateGroupInfo();
            objGroupInfo = groupPermisionHelper.CreateModulePermision(objGroupInfo);
            objGroupInfo = menuPermisionHelper.CreateMenuPermision(objGroupInfo);
            objGroupInfo = accessControlHelper.CreateAccessPermision(objGroupInfo);
            objGroupInfo = stateHelper.CreateStatusPermision(objGroupInfo);
            objGroupInfo = actionHelper.CreateActionPermision(objGroupInfo);
            
            var strGroupInfo = JSON.stringify(objGroupInfo).replace(/&/g, "^");
            var jsonParam = 'strGroupInfo=' + strGroupInfo;
            var serviceUrl = "../Group/SaveGroup/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);

        }
        function onSuccess(jsonData) {

            //var js = jsonData.split('"');
            if (jsonData == "Success") {
               
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Group Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            groupDetailsHelper.clearGroupForm();
                            $("#gridGroup").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Exist") {

                AjaxManager.MsgBox('warning', 'center', 'Warning', "Group Name already Exist for this company",
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Failed', jsonData,
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();
                           }
                       }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Failed', error.statusText,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
        }


    }
};

var groupDetailsHelper = {
    
    createTab: function () {
        
        $("#tabstrip").kendoTabStrip({});
    },
    clearGroupForm: function () {
        groupInfoHelper.clearGroupInfo();
        groupPermisionHelper.clearGroupPermisionForm();
        menuPermisionHelper.clearMenuPermision();
        accessControlHelper.clearAccessPermision();
        stateHelper.clearStatusPermision();
        actionHelper.clearActionPermision();
    },
    validateGroupForm: function () {
        var mess = false;
        mess = groupInfoHelper.validateForm();
        if(!mess) {
            return mess;
        }
        
        mess = true;
        return mess;
    }
};