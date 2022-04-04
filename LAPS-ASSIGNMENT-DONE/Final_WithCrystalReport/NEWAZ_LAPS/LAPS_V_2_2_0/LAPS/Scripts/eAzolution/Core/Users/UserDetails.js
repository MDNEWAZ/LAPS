var userDetailsManager = {
    SaveUserInfo: function () {
        if(userDetailsHelper.validateUserDetaisForm()) {
            
            var objUser = userInfoHelper.CreateUserInformationForSaveData();

            objUser = groupMembershipHelper.CreateGroupMemberForSaveData(objUser);
            
            var objUserinfo = JSON.stringify(objUser).replace(/&/g, "^");
            var jsonParam = 'strobjUserInfo=' + objUserinfo;
            var serviceUrl = "../Users/SaveUser/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);

        }
        function onSuccess(jsonData) {
            
            //var js = jsonData.split('"');
            if (jsonData == "Success") {
                
                AjaxManager.MsgBox('success', 'center', 'Success:', 'User Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            userDetailsHelper.clearUserForm();
                            $("#gridUser").data("kendoGrid").dataSource.read();
                        }
                    }]);

            }
            else {
                AjaxManager.MsgBox('warning', 'center', 'Failed', jsonData,
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

var userDetailsHelper = {
    clearUserForm: function () {
        userInfoHelper.clearUserInfoForm();
        groupMembershipHelper.clearGroupMembershipForm();
        groupMembershipHelper.GetGroupByCompanyId(CurrentUser.CompanyId);
        var tabStrip = $("#tabstrip").kendoTabStrip().data("kendoTabStrip");
        tabStrip.select(0);
    },
    
    createTab: function () {
        $("#tabstrip").kendoTabStrip({});
    },
    
    validateUserDetaisForm: function () {
        var mes = false;
        var res = userInfoHelper.ValidateUserInfoForm();
        if(res == false) {
            return mes;
        }
        mes = true;
        return mes;
    }
    
    

};