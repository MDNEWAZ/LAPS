var groupSummaryManager = {
    
    GenerateGroupGrid: function (companyID) {
        $("#gridGroup").kendoGrid({
            dataSource: groupSummaryManager.gridDataSource(companyID),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: groupSummaryHelper.GenerateGroupColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },
    
    gridDataSource: function (companyID) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    url: '../Group/GetGroupSummaryByCompanyIdWithPaging/?companyID=' + companyID,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                //update: {
                //    url: '../Company/GetGroupSummaryWithPaging/',
                //    dataType: "json"
                //},

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    },
    
    GetMotherCompany: function () {
        var objCompany = "";
        var jsonParam = "";
        var serviceUrl = "Company/GetMotherCompany/";
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

var groupSummaryHelper = {
    GenerateGroupColumns: function () {
        return columns = [
            { field: "GroupName", title: "Group Name", width: 100 },
            { field: "CompanyId", hidden: true },
            { field: "GroupId", hidden: true },
            { field: "IsDefault", hidden: true },
            { field: "Edit", title: "Edit Group", filterable: false, width: 40, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="groupSummaryHelper.clickEventForEditButton()"  />', sortable:false }
        ];
    },
    
    GenerateMotherCompanyCombo: function () {
        var objCompany = new Object();
        objCompany = groupInfoManager.GetMotherCompany();
        //objCompany = groupInfoManager.GetRootCompany();

        $("#cmbCompanyNameForSummary").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany
        });
        
        if(CurrentUser != null) {
            var cmbComp = $("#cmbCompanyNameForSummary").data("kendoComboBox");
            cmbComp.value(CurrentUser.CompanyId);
        }
        
        $("#cmbCompanyNameForSummary").parent().css('width', "17em");

    },

    CompanyIndexChangeEvent: function (e)
    {
            var companyData = $("#cmbCompanyNameForSummary").data("kendoComboBox");
            var companyID = companyData.value();

            $("#gridGroup").empty();
            $("#gridGroup").kendoGrid();
            groupSummaryManager.GenerateGroupGrid(companyID);

    },//Company Combobox Index Change Event

    clickEventForEditGroup: function () {
        $('#gridGroup table tr').live('dblclick', function () {


            groupDetailsHelper.clearGroupForm();


            var entityGrid = $("#gridGroup").data("kendoGrid");

            var selectedItem = entityGrid.dataItem(entityGrid.select());

            groupInfoHelper.populateGroupInformationDetails(selectedItem);

            groupPermisionHelper.PopulateExistingModule(selectedItem);

        });
    },
    clickEventForEditButton: function () {
        groupDetailsHelper.clearGroupForm();


        var entityGrid = $("#gridGroup").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());

        groupInfoHelper.populateGroupInformationDetails(selectedItem);

        groupPermisionHelper.PopulateExistingModule(selectedItem);
    }
};