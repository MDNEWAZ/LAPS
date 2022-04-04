var BankBranchSummaryManager = {
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
                    url: '../BankBranch/GetBankBranchSummary/',

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                //update: {
                //    url: '../Function/GetFunctionSummary/?companyID=' + companyId,
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
};

var BankBranchSummaryHelper = {
    GenerateBankBranchGrid:function () {
        $("#gridBranchSummary").kendoGrid({
            dataSource: BankBranchSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: BankBranchSummaryHelper.GeneratedBankBranchColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });

    },
    
    GeneratedBankBranchColumns:function () {
        return columns = [
     { field: "BranchId", title: "BranchId", width: 50, hidden: true },
     { field: "BranchCode", title: "Branch Code", width: 100 },
     { field: "BranchName", title: "Branch Name", width: 100, sortable: true },
     { field: "BankName", title: "BankName", width: 100, sortable: false },
     { field: "Address", title: "Branch Address", width: 100, sortable: false },
     { field: "Edit", title: "Edit", filterable: false, width: 70, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="BankBranchSummaryHelper.clickEventForEditFunction()"/>', sortable: false }
        ];
    },
    
    clickEventForEditFunction: function () {

        var entityGrid = $("#gridBranchSummary").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());

        BankBranchDetailsHelper.populateBankBranchDetails(selectedItem);

    },

};