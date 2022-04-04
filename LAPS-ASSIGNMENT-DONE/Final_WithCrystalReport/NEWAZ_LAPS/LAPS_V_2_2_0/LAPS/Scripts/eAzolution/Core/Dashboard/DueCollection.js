


var DueCollectionManager = {

    gridDataSource: function (companyId, branchId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Dashboard/GetDueCollectionCustomerGridData/?companyId=' + companyId + '&branchId=' + branchId,
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
    },

};

var DueCollectionHelper = {
    GenerateDueCollectionGrid: function () {

        $("#dueCollectionSummaryGrid").kendoGrid({
            dataSource: [],
            //dataSource: pendingRedCustomerManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: DueCollectionHelper.GenerateColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    GenerateColumns: function () {
        return columns = [
            { field: "CustomerCode", title: "Customer ID", width: 100 },
            { field: "Name", title: "Name", width: 100 },
            { field: "BranchCode", title: "Branch Code", width: 100 },
            { field: "Address", title: "Address", width: 100,hidden:true },
            { field: "Phone2", title: "Mobile No", width: 100 },
            { field: "ProductName", title: "Package Name", width: 100 },
            { field: "Model", title: "Model", width: 100, hidden: true },
            { field: "ProductTypeName", title: "Type", width: 80 },
            
            { field: "DueAmountTillDate", title: "Past Dues", width: 80 },
            { field: "OutStandingAmount", title: "Balance", width: 110, hidden: false },
            { field: "TotalDuePercentTillDate", title: "Due (%)", width: 70, hidden: true },
            //{ field: "Status", title: "Status", width: 100, template: "#=Status==1?'Paid':'Unpaid'#" },
            { field: "Edit", title: "Status", width: 80, template: "#=DueCollectionHelper.RatingSetforCustomer(data)#" }
        ];
    },
    RatingSetforCustomer: function (data) {

        return CustomerRatingHellper.RatingSetforCustomer(data);
    },
    

    DueCollectionGridDataSet: function (companyId,branchId) {
        
        var data = DueCollectionManager.gridDataSource(companyId, branchId);
        $("#dueCollectionSummaryGrid").data().kendoGrid.setDataSource(data);
    }
};