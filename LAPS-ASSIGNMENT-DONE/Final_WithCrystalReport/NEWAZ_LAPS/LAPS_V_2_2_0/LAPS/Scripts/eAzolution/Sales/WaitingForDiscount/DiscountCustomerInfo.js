

var DiscountCustomerInfoManager = {
    GenerateSalesInfoGrid: function () {

        $("#gridSalesDetails").kendoGrid({
            dataSource: [],
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },

            detailInit: DiscountCustomerInfoHelper.detailInitForSalesInfo,
            dataBound: function () {
              
            },

            filterable: true,
            sortable: true,
            columns: DiscountCustomerInfoHelper.GenerateSalesInfoColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },

    gridDataSource: function (customerId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Sale/GetSalesDetailsInfoByCustomerId/?customerId=' + customerId,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: {
                data: "Items", total: "TotalCount",
                model: {
                    fields: {
                        ProductId: {
                            type: "number"
                        }
                    }
                }
            }
        });
        return gridDataSource;
    },

    GetCustomerSalesItemInfo: function (salesItemurl) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 20,

            transport: {
                read: {
                    //url: '../SurveyAnswer/GenerateAnswerSummary/?surveyId=' + surveyId + '&surveyQuestionId=' + surveyQuestionId,
                    url: salesItemurl,

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

                        //Quantity: {
                        //    type: "number", validation: { required: true, min: 0 },

                        //},
                        ALicense: {
                            IssueDate: {
                                type: "date",
                                template: '#= kendo.toString("MM/dd/yyyy") #'
                            },
                        }



                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    }
};

var DiscountCustomerInfoHelper = {
    FillCustomerSalesDetails: function (objCustomer) {
        var salesData = DiscountCustomerInfoManager.gridDataSource(objCustomer.CustomerId);
        var salesGrid = $("#gridSalesDetails").data("kendoGrid");
        salesGrid.setDataSource(salesData);
    },

    GenerateSalesInfoColumns: function () {
        return columns = [
            { field: "AProduct.Code", title: "Product Code", hidden: true },
            { field: "AProduct.ProductName", title: "Product Name" },
            { field: "AProduct.Model", title: "Model" },
            { field: "AProduct.ProductName", title: "Product Name" },
            { field: "Invoice", title: "SaleInvoice", hidden: true },
            { field: "AProduct.TotalPrice", title: "Total Price" },
           // { field: "ALicense.Number", title: "Last License", hidden: false },
           // { field: "AType.Type", title: "License Type", hidden: false },
            //{ field: "ALicense.IssueDate", title: "Install Date", template: '#=kendo.toString(ALicense.IssueDate,"dd-MMM-yyyy")#' },
            { field: "ALicense.Status", title: "Status", hidden: true },
            { field: "Edit", title: "", filterable: false, template: '<button id="btnSearchCustInfo" class="k-i-search" type="submit" onclick="SalesInfoSummaryHelper.clickEventForEditButton()"><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },

    detailInitForSalesInfo: function (e) {

        var salesItemurl = "../Sale/GetSalesItemInfoBySaleId/?saleId= " + e.data.SaleId;
        $("<div id='gridSalesItem'/>").appendTo(e.detailCell).kendoGrid({

            dataSource: DiscountCustomerInfoManager.GetCustomerSalesItemInfo(salesItemurl),
            scrollable: false,
            sortable: true,
            pageable: false,
            batch: true,
            editable: true,
            selectable: "multiple, row",
            columns: [
                 { field: "ProductItems.ItemName", title: "Name", width: 50, },
                 { field: "ProductItems.ItemModel", title: "Model", width: 50, },
                 { field: "ItemPrice", title: "Item Price", width: 50, },
                 { field: "ItemQuantity", title: "Quantity", width: 50, },
                 { field: "ItemSLNo", title: "Product Id", width: 50, }
            ]
        });

    },
};