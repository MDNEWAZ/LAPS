

var StockProductItemsManager = {
    gridDataSource: function (modelId) {
        
        var url = "";
        if (modelId != 0) {
            url = '../Stock/GetAllStockModelId/?modelId=' + modelId;
        }
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                   // url: '../Stock/GetAllStockModelId/?modelId=' + modelId,

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

                        ProductItems: { defaultValue: { ItemId: 0, ItemName: "--Select--" } },

                        Quantity: {
                            type: "number", validation: { required: true, min: 0 },
                           
                        },
                        StockId: {
                            type: "number",
                        },
                        //ReceiveDate: {
                        //    type: "date",
                        //    template: '#= kendo.toString("MM/dd/yyyy") #'
                        //},


                    }
                },
                data: "Items", total: "TotalCount"
            }
        });


        return gridDataSource;


    },
    
    getProductItemGridDataSource: function (modelId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    //url: '../Stock/GetAllStockModelId/?modelId=' + modelId,
                    url: '../Stock/GetAllStockModelId/?modelId=' + modelId,

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

                        //ProductItems: { defaultValue: { ItemId: 0, ItemName: "--Select--" } },

                        //Quantity: {
                        //    type: "number", validation: { required: true, min: 0 },

                        //},
                        //StockId: {
                        //    type: "number",
                        //},
                        //ReceiveDate: {
                        //    type: "date",
                        //    template: '#= kendo.toString("MM/dd/yyyy") #'
                        //},


                    }
                },
                data: "Items", total: "TotalCount"
            }
        });


        return gridDataSource;


    },

    GetProductItems: function () {
     
        //var modelId = $('#hdnModelId').val();
        var modelId = $('#cmbPackage').data().kendoComboBox.value();
        if (modelId > 0) {
            var objCcType = "";
            var jsonParam = "";
            var serviceUrl = "../Product/GetProductItemByModelId/?modelId=" + modelId;
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select A Product Model.',
               [{
                   addClass: 'btn btn-primary',
                   text: 'Ok',
                   onClick: function ($noty) {
                       $noty.close();
                       $("#gridStockProductItems").data("kendoGrid").dataSource.data([]);
                   }
               }]);
        }
      

        function onSuccess(jsonData) {
            objCcType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCcType;
    },
};


var StockProductItemsHelper = {
    GenerateStockItemsGrid: function (modelId) {
     
        var objItems = StockProductItemsManager.gridDataSource(modelId);
        var grid = $("#gridStockProductItems").data('kendoGrid');
        if (grid != undefined) {
            grid.destroy();
            grid.wrapper.empty();
        }
        
        

        $("#gridStockProductItems").kendoGrid({
            dataSource: objItems,
            pageable: false,
            toolbar: ["create"],
            //toolbar: ["create", { template: kendo.template($("#template").html()) }],
            columns: [
                { field: "ItemId", width: "100px", hidden: true },
                { field: "ProductItems", title: "Item Name", width: "60px", editor: StockProductItemsHelper.ProductItemDropDownEditor, template: "#=ProductItems.ItemName#" },
                //{ field: "ReceiveDate", title: "Receive Date", width: "60px", format: "{0:MM/dd/yyyy}" },
                { field: "Quantity", title: "Quantity", width: "50px", },
              //{ command: "destroy", title: "Action", width: "60px" },
              //{ command: "destroy", title: "Action", width: "40px", template: '<input type="button" class="k-button" id="btnDelete" onClick="EmploymentSummaryHelper.deleteRow()"/>' },
             // { field: "Edit", title: "Delete", filterable: false, width: 70, template: '<input type="button" class="k-button" value="Delete" id="btnDelete" onClick="ProductItemHelper.deleteRow()"/>', sortable: false }
            ],
            editable: {
                confirmation: false
            },

            selectable: true,


        });
    },
    GenerateStockItemsGridWithData: function (modelId) {
        var grid = $("#gridStockProductItems").data('kendoGrid');
        if (grid != undefined) {
            grid.destroy();
            grid.wrapper.empty();
        }
        $("#gridStockProductItems").kendoGrid({
            dataSource: StockProductItemsManager.getProductItemGridDataSource(modelId),
                pageable: {
                    refresh: true,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true
                },
                filterable: true,
                sortable: true,
                columns: StockProductItemsHelper.GenerateStockProductItemsColumns(),
                editable: false,
                navigatable: true,
                selectable: "row",
            });
        },

    GenerateStockProductItemsColumns: function () {
        return columns = [
                { field: "ItemId", width: "100px", hidden: true },
                { field: "ItemName", title: "Item Name", width: "60" },
                { field: "ReceiveDate", title: "Receive Date", width: "60", format: "{0:MM/dd/yyyy}",hidden:true },
                { field: "BundleQuantity", title: "Quantity", width: "50", }
    //{ field: "Edit", title: "Edit", filterable: false, width: 30, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="accessControlSummaryHelper.clickEventForEditButton()"  />', sortable: false }
        ];
        
        },

    ProductItemDropDownEditor: function (container, options) {
        $('<input required data-text-field="ItemName" data-value-field="ItemId" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                optionLabel: '--Select--',
                dataSource: StockProductItemsManager.GetProductItems(),
                placeholder: "Please Select",
                //change: function () {
                //    EmploymentSummaryHelper.checkExistCostCentre();
                //}
            });
    },

};