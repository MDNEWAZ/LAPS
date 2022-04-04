

var CollectionSummaryManager = {
    GenerateCollectionGrid: function () {

        $("#gridCollectionSummary").kendoGrid({
            dataSource: CollectionSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: CollectionSummaryHelper.GenerateCollectionColumns(),
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
                    url: '../Collection/GetAllCollection/',
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

var CollectionSummaryHelper = {
    GenerateCollectionColumns: function () {
        return columns = [
            { field: "CustomerCode", title: "Customer ID", width: 50 },
            { field: "Invoice", title: "Invoice No", width: 50 },
            { field: "Name", title: "Name", width: 80 },
            { field: "BranchCode", title: "Branch Code", width: 40 },
            { field: "BranchName", title: "Branch Name", width: 80 },
            { field: "Phone", title: "Mobile No", width: 50 },
            { field: "Phone2", title: "Mobile No 2", width: 50 },
            { field: "PaidAmount", title: "Paid Amount", width: 40 },
            { field: "DueAmount", title: "Due Amount", width: 40 },
            { field: "ReferenceId", title: "Reference Id", width: 50 },
           // { field: "DuePercent", title: "Due (%)", width: 70 },
           // { field: "Edit", title: "Rating", width: 80, template: "#=pendingRedCustomerManagerHellper.RatingSetforCustomer(data)#" },
            { field: "Edit", title: "Collecte", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Collect" id="collectionbtnEdit" onClick="CollectionSummaryHelper.clickEventForEditButton()"/>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#gridCollectionSummary").data("kendoGrid");
        var selecteItem = entityGrid.dataItem(entityGrid.select());

        $("#topTenRedCustomerGrid").hide();
        $("#collectionMainDetailsDiv").show();
        $("#divInstallmentInfoGrid").show();
        $("#divCollectionSummaryGrid").hide();
       
        CollectionDetailsHelper.ClearAllFields();
        if (selecteItem != null) {
            //productInfoManager.FillCustomerProductForm(ivoiceObj.Invoice);
            CollectionDetailsHelper.FillCustomerAndProductDetails(selecteItem);
            InstallmentDetailsManager.FillInstallmentGrid(selecteItem.Invoice);
            CustomerRatingManager.CustomerDueGridDataSet(selecteItem.Invoice);
            CollectionDetailsHelper.fillDownpaymentInfo(selecteItem.Invoice);
        }


    }
};