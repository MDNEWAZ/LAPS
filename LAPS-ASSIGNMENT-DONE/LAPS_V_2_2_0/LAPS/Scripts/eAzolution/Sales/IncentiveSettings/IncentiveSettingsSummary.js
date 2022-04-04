var IncentiveSettingsSummaryManager = {
    GenerateIncentiveSettingsSummaryGrid: function () {
        $("#incentiveSummaryGrid").kendoGrid({
            dataSource:IncentiveSettingsSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: IncentiveSettingsSummaryHelper.GenerateIncentiveSettingsColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },
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
                    url: '../IncentiveSettings/GetIncentiveSettingsSummary/',

                    type: "POST",

                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    }
};

var IncentiveSettingsSummaryHelper = {

    InitIncentiveSummary:function() {
        IncentiveSettingsSummaryManager.GenerateIncentiveSettingsSummaryGrid();
    },

    GenerateIncentiveSettingsColumns: function () {
        return columns = [
              
               { field: "IncentiveId", title: "IncentiveId",  hidden: true },
               { field: "NumberOfSale", title: "Number Of Sale Per Month", },
               { field: "IncentiveAmount", title: "Incentive Amount", },
               { field: "IsActive", title: "Status", template: "#= IsActive==1?'Active':'In Active'#" },
               { field: "Edit", title: "Edit", filterable: false, template: '<button type="button" value="Edit" id="btnEdit" onClick="IncentiveSettingsSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#incentiveSummaryGrid").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        IncentiveSettingsDetailsHelper.FillIncentiveSettingsDetailsForm(selectedItem);
    }
};