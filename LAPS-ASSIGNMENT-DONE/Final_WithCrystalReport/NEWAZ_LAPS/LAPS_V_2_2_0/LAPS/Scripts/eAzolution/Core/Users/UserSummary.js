var userSummaryManager = {
    
    gridDataSource: function (companyId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    url: '../Users/GetUserSummary/?companyID=' + companyId,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../Users/GetUserSummary/?companyID=' + companyId,
                    dataType: "json"
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    },
    
    ResetPasswordByCompanyIdAndUserId: function (companyId, userId) {
        var jsonParam = 'companyId=' + companyId + "&userId=" + userId;
        var serviceUrl = "../Users/ResetPassword/";
        AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Password Reset Successfully',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                            
                         }
                     }]);

            }
            else {
                alert(jsonData);
            }
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    GetMotherCompany: function () {
        var objCompany = "";
        var jsonParam = "";
        var serviceUrl = "../Company/GetMotherCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCompany;
    }
};

var userSummaryHelper = {    
    GenerateUserSummaryGrid: function(companyId) {
        $("#gridUser").kendoGrid({
            dataSource: userSummaryManager.gridDataSource(companyId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: userSummaryHelper.GenerateUsersColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"
        });
    },

    GenerateUsersColumns: function() {
        return columns = [
            { field: "UserName", title: "User Name", width: 120 },
            { field: "LoginId", title: "Login ID", width: 60 },
             { field: "IsNotify", title: "Is Notify", width: 60, template: "#= IsNotify ? 'Notifed' : 'Not Notifed' #" },
            { field: "IsActive", title: "Is Active", width: 60, template: "#= IsActive ? 'Active' : 'Inactive' #" },
            { field: "ResetPassword", title: "Reset Password", filterable: false, width: 80, template: '<input type="button" class="k-button" value="Reset Password" id="btnResetPassword"/>', sortable: false },
            { field: "UserId", hidden: true },
            { field: "EmployeeId", hidden: true },
            { field: "CompanyId", hidden: true },
             { field: "CompanyName", hidden: true },
            { field: "Password", hidden: true },
            { field: "FailedLoginNo", hidden: true },
            { field: "IsExpired", hidden: true },
            { field: "LastLoginDate", hidden: true },
            { field: "CreatedDate", hidden: true },
            { field: "BranchId", hidden: true },
            { field: "BranchName", hidden: true },
            { field: "DepartmentId", hidden: true },
           // { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="userSummaryHelper.clickEventForEditButton()"  />', sortable: false },
           { field: "Edit", title: "Edit", filterable: false, width: 60, template: '<button type="button" value="Edit" id="btnEdit" onClick="userSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    
    clickEventForResetPassword: function () {
        $('#btnResetPassword').live('click', function () {
            var entityGrid = $("#gridUser").data("kendoGrid");
         
            var selectedItem = entityGrid.dataItem(entityGrid.select());
            
            userSummaryHelper.resetPassword(selectedItem);

        });
    },
    
    clickEventForEditButton: function () {
        var entityGrid = $("#gridUser").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());
     
        userInfoHelper.populateUserInformationDetails(selectedItem);

        groupMembershipHelper.populateGroupMember(selectedItem);
    },
    
    resetPassword: function (items) {
        userSummaryManager.ResetPasswordByCompanyIdAndUserId(items.CompanyId, items.UserId);
    },
    
    clickEventForEditUser: function () {
        $('#gridUser table tr').live('dblclick', function () {
            var entityGrid = $("#gridUser").data("kendoGrid");

            var selectedItem = entityGrid.dataItem(entityGrid.select());

            userInfoHelper.populateUserInformationDetails(selectedItem);
            
            groupMembershipHelper.populateGroupMember(selectedItem);

        });
    },
    
    GenerateMotherCompanyCombo: function () {
        var objCompany = new Object();
        objCompany = userSummaryManager.GetMotherCompany();

        $("#cmbCompanyNameForSummary").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany
        });
        if (CurrentUser != null) {
            var cmbComp = $("#cmbCompanyNameForSummary").data("kendoComboBox");
            cmbComp.value(CurrentUser.CompanyId);
        }
    },
    
    CompanyIndexChangeEvent: function (e) {
        var companyData = $("#cmbCompanyNameForSummary").data("kendoComboBox");
        var companyId = companyData.value();
        $("#gridUser").empty();
        $("#gridUser").kendoGrid();
        userSummaryHelper.GenerateUserSummaryGrid(companyId);

    },

};