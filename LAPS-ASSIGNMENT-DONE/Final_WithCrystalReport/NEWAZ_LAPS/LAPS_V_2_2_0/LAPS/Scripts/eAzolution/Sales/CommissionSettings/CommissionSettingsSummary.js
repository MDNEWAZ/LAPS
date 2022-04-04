var CommissionSettingsSummaryManager = {
    GenerateCommissionSettingsSummaryGrid: function () {
        $("#commissionSummaryGrid").kendoGrid({
            dataSource:CommissionSettingsSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: CommissionSettingsSummaryHelper.GenerateCommissionSettingsColumns(),
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
                    url: '../CommissionSettings/GetCommissionSettingsSummary/',

                    type: "POST",

                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: {
                model: {
                    fields: {
                        ConfirmationSmsDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        },
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    }
};

var CommissionSettingsSummaryHelper = {
    GenerateCommissionSettingsColumns: function () {
        return columns = [
               { field: "CommissionId", title: "CommissionId", width: 80, hidden: true },
               { field: "SalesRepTypeName", title: "Sale Rep Type", width: 60 },
               { field: "SaleTypeName", title: "Sale Type", width: 60 },
               { field: "ComissionAmount", title: "Commission Amount", width: 60 },
               { field: "IsActive", title: "Status", template: "#=IsActive==1?'Active':'In Active'#", width: 60 },
             //  { field: "InstallmentSaleCommission", title: "Commission For</br>Installment Sale", width: 80 },
               //{ field: "DayOfMonth", title: "Confirmation SMS Day </br>Of Each Month", width: 80 },
               { field: "Edit", title: "Edit", filterable: false, width: 30, template: '<button type="button" value="Edit" id="btnEdit" onClick="CommissionSettingsSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#commissionSummaryGrid").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        CommissionSettingsDetailsHelper.FillCommissionSettingsDetailsForm(selectedItem);
    }
};