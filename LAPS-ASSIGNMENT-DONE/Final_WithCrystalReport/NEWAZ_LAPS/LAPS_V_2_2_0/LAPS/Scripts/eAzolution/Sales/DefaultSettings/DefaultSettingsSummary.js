var DefaultSettingsSummaryManager = {
    GenerateDefaultSettingsSummaryGrid: function () {
        $("#gridInterest").kendoGrid({
            dataSource: DefaultSettingsSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: DefaultSettingsSummaryHelper.GenerateDefaultSettingsColumns(),
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
                    url: '../DefaultSettings/GetDefaultSettingsSummary/',
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

var DefaultSettingsSummaryHelper = {
    GenerateDefaultSettingsColumns: function () {
        return columns = [
               { field: "ACompany.CompanyId", hidden: true },
               { field: "InterestId", hidden: true },
               { field: "ACompany.CompanyName", title: "Company Name", width: 120 },
               { field: "DownPay", title: "Down</br>Payment</br>(%)", width: 40 },//
               { field: "Interests", title: "Interest</br>(%)", width: 40 },
                { field: "DefaultInstallmentNo", title: "Default </br>Installment No", width: 40 },
               { field: "EntryDate", title: "Valid From", width: 60 },
               { field: "Status", title: "Status", width: 40,template:"#=Status==1?'Active':'InActive'#" },
              { field: "Edit", title: "Edit", filterable: false, width: 30, template: '<button type="button" value="" id="btnEdit" onClick="DefaultSettingsSummaryHelper.clickEventForEditButton()"><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {
        var entityGrid = $("#gridInterest").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        DefaultSettingsDetailsHelper.FillDefaultSettingsDetailsInForm(selectedItem);
    }
};