var currentUserlevel = [];
var branchUpgradeSummaryManager = {

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
                    url: '../Branch/GetBranchSummary/?companyID=' + companyId,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../Branch/GetBranchSummary/?companyID=' + companyId,
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

var branchUpgradeSummaryHelper = {
    GenerateBranchGrid: function (companyId) {

        $("#gridBranchUpgradeSummary").kendoGrid({
            dataSource: branchUpgradeSummaryManager.gridDataSource(companyId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: branchUpgradeSummaryHelper.GeneratedBranchColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
            dataBound: function () {
            var dataView = this.dataSource.view();
            for (var i = 0; i < dataView.length; i++) {
                var uid = dataView[i].uid;
                if (dataView[i].IsUpgraded === 1) {
                    $("#gridBranchUpgradeSummary tbody").find("tr[data-uid=" + uid + "]").css("background-color", "#a2a8ff");
                   // $("#gridBranchUpgradeSummary tbody").find("tr[data-uid=" + uid + "]").css("color", "#006633");
                }
               
            }
        },
            //selectable: false

        });

    },

    GeneratedBranchColumns: function () {
        return columns = [
        { filed: "BranchId", title: "BranchId", width: 50, hidden: true },
        { field: "BranchName", title: "Branch Name", width: 100, sortable: true },
        { field: "BranchDescription", title: "Branch Description", width: 100, sortable: false },
        { field: "BranchCode", title: "Branch Code", width: 100, sortable: true },
        { field: "IsUpgraded", title: "Status", width: 100, sortable: true, template: "#=IsUpgraded==1?'Upgraded':'Not Upgraded'#" },
        { field: "Edit", title: "Edit", filterable: false, width: 70, template: '<button type="button" value="Edit" id="btnEdit" onClick="branchUpgradeSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },

    GenerateMotherCompanyCombo: function () {
        var objCompany = new Object();
        objCompany = branchUpgradeSummaryManager.GetMotherCompany();

        $("#cmbCompanyNameForSummary").kendoComboBox({
            index: 0,
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

    CompanyIndexChangeEvent: function () {
        var companyData = $("#cmbCompanyNameForSummary").data("kendoComboBox");
        var companyId = companyData.value();

        $("#gridBranchUpgradeSummary").empty();
        $("#gridBranchUpgradeSummary").kendoGrid();

        branchUpgradeSummaryHelper.GenerateBranchGrid(companyId);
    },

    clickEventForEditButton: function () {

        var entityGrid = $("#gridBranchUpgradeSummary").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            branchUpgradeDetailsHelper.populateBranchDetails(selectedItem);
        }
    }
};