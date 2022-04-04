var workFlowManager = {

    gridDataSource: function () {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Status/GetWorkFlowSummary/',
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                //update: {
                //    url: '../Status/GetWorkFlowSummary/',
                //    dataType: "json"
                //},
                parameterMap: function (options) {

                    return JSON.stringify(options);
                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    }
};

var workFlowHelper = {
    GenerateworkFlowSummaryGrid: function () {
        //var objDataSource = workFlowManager.gridDataSource();

        $("#gridworkFlow").kendoGrid({
            dataSource: workFlowManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: workFlowHelper.GenerateworkFlowsColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"
        });
    },

    GenerateworkFlowsColumns: function () {
        return columns = [
            { field: "StateName", title: "State Name", width: 120 },
            { field: "MenuName", title: "Manu", width: 60 },
            { field: "IsDefaultStart", title: "Is Default", width: 60, template: "#= IsDefaultStart ? 'Yes' : 'No' #" },
            { field: "WFStateId", hidden: true },
            { field: "MenuID", hidden: true },
            { field: "IsClosed", hidden: true },
            { field: "Edit", title: "Edit", filterable: false, width: 60, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="workFlowHelper.clickEventForEditButton()"  />', sortable: false }
        ];
    },

    clickEventForEditButton: function () {
        //$('#btnEdit').live('click', function () {

        //});
        var entityGrid = $("#gridworkFlow").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        workFlowHelper.EditItem(selectedItem);

    },

    EditItem: function (items) {
        //workFlowManager.ResetPasswordByCompanyIdAndworkFlowId(items.CompanyId, items.workFlowId);
        //$('#cmbMenu').val = items.MenuID;
        stateDetailManager.clearForm();
        var employee = $("#cmbMenu").data("kendoComboBox");
        employee.value(items.MenuID);
        $("#txtStateName").val(items.StateName);
        $('#chkIsDefault').attr('checked', items.IsDefaultStart);
        var isClose = $("#cmbIsClose").data("kendoComboBox");
        isClose.value(items.IsClosed);

        $("#stateID").val(items.WFStateId);
        //actionDetailHelper.GenerateActionGrid($("#stateID").val());
        

        var combobox = $("#cmbNextState").data("kendoComboBox");

        // detach events
        combobox.destroy();

        actionDetailHelper.LoadNextStateCombo(items.MenuID);
        actionDetailHelper.GenerateActionGrid(items.WFStateId);

        //$("#actionID").val("TESting");
        actionDetailManager.clearActionForm();
        $("#txtStateName_Action").val(items.StateName);

    },

    clickEventForEditworkFlow: function () {
        //alert("Double Clicked");
        $('#gridworkFlow table tr').live('dblclick', function () {

            var entityGrid = $("#gridworkFlow").data("kendoGrid");
            var selectedItem = entityGrid.dataItem(entityGrid.select());
            //workFlowInfoHelper.populateworkFlowInformationDetails(selectedItem);

            //groupMembershipHelper.populateGroupMember(selectedItem);
            
            workFlowHelper.EditItem(selectedItem);

        });
    }


};