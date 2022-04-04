
var CustomerRatingManager = {

    //GenerateRedCustomer: function () {
       
    //    $("#topTenRedCustomerGrid").kendoGrid({           
    //        dataSource:[],
    //        //dataSource: pendingRedCustomerManager.gridDataSource(),
    //        pageable: {
    //            refresh: true,
    //            serverPaging: true,
    //            serverFiltering: true,
    //            serverSorting: true
    //        },
    //        filterable: true,
    //        sortable: true,
    //        columns: pendingRedCustomerManagerHellper.GenerateColumns(),
    //        editable: false,
    //        navigatable: true,
    //        selectable: "row",
    //    });
    //},

    //gridDataSource: function (invoice) {
    //    var gridDataSource = new kendo.data.DataSource({
    //        type: "json",
    //        serverPaging: true,
    //        serverSorting: true,
    //        serverFiltering: true,
    //        allowUnsort: true,
    //        pageSize: 10,
    //        transport: {
    //            read: {
    //                url: '../Dashboard/GetTenRedCustomer/?invoice='+ invoice,
    //                type: "POST",
    //                dataType: "json",
    //                contentType: "application/json; charset=utf-8"
    //            },
    //            parameterMap: function (options) {
    //                return JSON.stringify(options);
    //            }
    //        },
    //        schema: { data: "Items", total: "TotalCount" }
    //    });
       
    //    return gridDataSource;
    //},

    GetDuePercentByInvoiceNoData: function (invoice) {//Please consider this function name and its working area changed 
        //var invoiceNo = $("#txtInvoiceProInfo").val();
        var getDuePercentByInvoiceNoData = "";
        var jsonParam = 'invoiceNo=' + invoice;
        var serviceUrl = "../Collection/GetDuePercentByInvoiceNo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);


        function onSuccess(jsonData) {
            getDuePercentByInvoiceNoData = jsonData;
        }

        function onFailed(error) {

        }

        return getDuePercentByInvoiceNoData;
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
    
    getCustomerRatingByCompanyId: function (companyId) {
        
      //  var customerId = CurrentUser.CompanyId;
        var customerRatingData = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../Customer/GetCustomerRatingByCompanyId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        

        function onSuccess(jsonData) {
            customerRatingData = jsonData;
        }

        function onFailed(error) {

        }

        return customerRatingData;
    },
    
    CustomerDueGridDataSet: function (invoice) {
        var data = CustomerRatingManager.GetDuePercentByInvoiceNoData(invoice);
        //var outStandingAmt = Math.round();

        $("#OutstandingAmountlbl").val(data.OutStandingAmount);
        $("#DuePercentlbl").val(data.TotalDuePercentTillDate);
        $("#DueAmountlbl").val(data.DueAmountTillDate);
        $("#DueRatinglbl").html(CustomerRatingHellper.RatingSetforCustomer(data));
    }
    

    //CustomerDueGridDataSet: function (invoice) {
       
    //    var dataSource = pendingRedCustomerManager.gridDataSource(invoice);
       
    //    dataSource.fetch(function () {
    //        var data = dataSource.data();
    //        var gridDataObj = data[0];
    //        $("#OutstandingAmountlbl").val(gridDataObj.Amount);
    //        $("#DueAmountlbl").val(gridDataObj.DueAmount);
    //        $("#DuePercentlbl").val(gridDataObj.DuePercent);
    //        $("#DueRatinglbl").html(CustomerRatingHellper.RatingSetforCustomer(gridDataObj));
            
    //    });
        
    //    //pendingRedCustomerManagerHellper.RatingSetforCustomer(data);
    //}

};
var CustomerRatingHellper = {
    //GenerateColumns: function () {
    //    return columns = [
    //        { field: "Name", title: "Name", width: 100 },
    //        { field: "Address", title: "Address", width: 100 },
    //        { field: "Phone", title: "Mobile No", width: 100 },
    //        { field: "Amount", title: "Outstanding Amount", width: 100 },
    //        { field: "DueAmount", title: "Due Amount", width: 100 },
    //        { field: "DuePercent", title: "Due (%)", width: 100 },
    //        //{ field: "Status", title: "Status", width: 100, template: "#=Status==1?'Paid':'Unpaid'#" },
    //        { field: "Edit", title: "Rating", width: 60, template: "#=pendingRedCustomerManagerHellper.RatingSetforCustomer(data)#" }
    //    ];
    //},
    //RatingColorforCustomer: function (data) {
    //    if (data.DuePercent < 10) {
    //        return '<label for="Red" class="centerAlign" style="background-color: green ; color: #fffff0; border-radius: 2px">Green</label>';

    //    }
    //    if (data.DuePercent >= 25) {
    //        return '<label for="Red" class="centerAlign" style="background-color: red ; color: #fffff0; border-radius: 2px">Red</label>';
    //    }
    //    else {
    //        return '<label for="Red" class="centerAlign" style="background-color: yellow ; color: #9932cc; border-radius: 2px">Yellow</label>';
    //    }
    //},

    RatingSetforCustomer: function (data) {
        debugger;
        var customerRating = CustomerRatingManager.getCustomerRatingByCompanyId(data.CompanyId);
      
        for (var i = 0; i < customerRating.length; i++) {
            if (data.TotalDuePercent < 1 && data.IsRelease != 1) {
                return '<label for="WaitingForRelease" class="centerAlign" style="background-color: white ; color: black; border-radius: 2px">Waiting For Release</label>';
            }
            if (data.TotalDuePercent < 1 && data.IsRelease == 1) {
                return '<label for="Released" class="centerAlign" style="background-color: white ; color: black; border-radius: 2px">Released</label>';
            }
            if ((parseFloat(customerRating[i].FromDue) <= data.TotalDuePercentTillDate) && (parseFloat(customerRating[i].ToDue) >= data.TotalDuePercentTillDate)) {
                return '<label for="'+customerRating[i].Color+'" class="centerAlign" style="background-color: '+customerRating[i].Color+' ; color: #fffff0; border-radius: 2px">'+customerRating[i].Color+'</label>';
            }
         
        }

        //if (data.DuePercent < 10) {
        //    return '<label for="Green" class="centerAlign" style="background-color: green ; color: #fffff0; border-radius: 2px">Green</label>';

        //}
        //if (data.DuePercent >= 25) {
        //    return '<label for="Red" class="centerAlign" style="background-color: red ; color: #fffff0; border-radius: 2px">Red</label>';
        //}
        //else {
        //    return '<label for="Yellow" class="centerAlign" style="background-color: yellow ; color: #9932cc; border-radius: 2px">Yellow</label>';
        //}
    },
}