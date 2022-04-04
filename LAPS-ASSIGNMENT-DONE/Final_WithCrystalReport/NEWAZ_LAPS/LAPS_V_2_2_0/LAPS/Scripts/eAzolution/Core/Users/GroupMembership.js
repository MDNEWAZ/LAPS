var groupPermisionArray = [];//This array also clear from Organograme Tree's change event
var groupArray = [];

var groupMembershipManager = {
    
    GetGroupByCompanyId: function (companyId) {
        
        var objGroupList = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../Group/GetGroupByCompanyId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            
            objGroupList = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objGroupList;
    },
    
    GetGroupMemberByUserId: function (userId) {
        
        var objGroupMemberList = "";
        var jsonParam = "userId=" + userId;
        var serviceUrl = "../Users/GetGroupMemberByUserId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            
            objGroupMemberList = jsonData;
        }
        function onFailed(error) {
            
            window.alert(error.statusText);
        }
        return objGroupMemberList;
    }
    
};

var groupMembershipHelper = {
    
    GetGroupByCompanyId: function (companyId) {
        groupArray = [];
        
        var objGroupList = groupMembershipManager.GetGroupByCompanyId(companyId);
        var link = "";
        
        for (var i = 0; i < objGroupList.length; i++) {
            link += "<div><input type=\"checkbox\" class=\"chkBox\" id=\"chkGroup" + objGroupList[i].GroupId + "\" onclick=\"groupMembershipHelper.populateGroupPermisionArray(" + objGroupList[i].GroupId + ", '" + objGroupList[i].GroupName + "', this.id)\"/> " + objGroupList[i].GroupName + "</div>";
            groupArray.push(objGroupList[i]);
        }
        $("#checkboxGroup").html(link);
    },
    
    populateGroupPermisionArray: function (groupId, groupName) {
        
        if ($("#chkGroup" + groupId).is(':checked') == true) {
            
            var obj = new Object();
            obj.GroupId = groupId;
            obj.GroupName = groupName;
            groupPermisionArray.push(obj);
        }
        else {
            for (var i = 0; i < groupPermisionArray.length; i++) {
                if (groupPermisionArray[i].GroupId == groupId) {
                    groupPermisionArray.splice(i, 1);
                }
            }
        }
    },
    
    clearGroupMembershipForm: function () {
        $("#checkboxGroup").html("");
        $('.chkBox').attr('checked', false);
        groupPermisionArray = [];
    },
    
    CreateGroupMemberForSaveData: function (objUser) {
        objUser.GroupMembers = groupPermisionArray;
        return objUser;
    },
    
    populateGroupMember: function(objUser) {
        
        groupPermisionArray = [];
        var memberList = groupMembershipManager.GetGroupMemberByUserId(objUser.UserId);
        for(var i=0; i<memberList.length; i++) {
            $('#chkGroup' + memberList[i].GroupId).attr('checked', true);
            var obj = new Object();
            obj.GroupId = memberList[i].GroupId;
            obj.GroupName = "";
            groupPermisionArray.push(obj);
        }
    }

};