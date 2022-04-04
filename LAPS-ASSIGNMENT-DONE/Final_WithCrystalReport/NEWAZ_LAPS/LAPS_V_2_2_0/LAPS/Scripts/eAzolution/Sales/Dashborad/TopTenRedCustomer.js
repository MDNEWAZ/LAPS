
var pendingRedCustomerManager = {

    GenerateRedCustomer: function () {
       
        $("#topTenRedCustomerGrid").kendoGrid({           
            dataSource:[],
            //dataSource: pendingRedCustomerManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: pendingRedCustomerManagerHellper.GenerateColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },

    gridDataSource: function (invoice,companyId,branchId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Dashboard/GetTenRedCustomer/?invoice=' + invoice + '&companyId='+companyId+'&branchId='+branchId,
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
    
    
    getPenddingCollectionData: function () {
        var pendingCollectionData = "";
        var jsonParam = '';
        var serviceUrl = "../Dashboard/GetAllPendingCollections/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);


        function onSuccess(jsonData) {
            pendingCollectionData = jsonData;
        }

        function onFailed(error) {

        }

        return pendingCollectionData;
    },
    CustomerDueGridDataSet: function (invoice) {
        var data = pendingRedCustomerManager.gridDataSource(invoice);
        $("#topTenRedCustomerGrid").data().kendoGrid.setDataSource(data);
    }

};
var pendingRedCustomerManagerHellper = {
    GenerateColumns: function () {
        return columns = [
            { field: "Name", title: "Name", width: 100 },
            { field: "Address", title: "Address", width: 100 },
            { field: "BranchCode", title: "Branch Code", width: 100 },
            { field: "Phone", title: "Mobile No", width: 100 },
            { field: "Amount", title: "Outstanding Amount", width: 110 },
            { field: "DueAmount", title: "Due Amount", width: 80 },
            { field: "DuePercent", title: "Due (%)", width: 70 },
            //{ field: "Status", title: "Status", width: 100, template: "#=Status==1?'Paid':'Unpaid'#" },
            { field: "Edit", title: "Rating", width: 80, template: "#=pendingRedCustomerManagerHellper.RatingSetforCustomer(data)#" }
        ];
    },
    

    RatingSetforCustomer: function (data) {
        if (data.DuePercent < 10) {
            return '<label for="Red" class="centerAlign" style="background-color: green ; color: #fffff0; border-radius: 2px">Green</label>';

        }
        if (data.DuePercent >= 25) {
            return '<label for="Red" class="centerAlign" style="background-color: red ; color: #fffff0; border-radius: 2px">Red</label>';
        }
        else {
            return '<label for="Red" class="centerAlign" style="background-color: yellow ; color: #9932cc; border-radius: 2px">Yellow</label>';
        }
    },
}