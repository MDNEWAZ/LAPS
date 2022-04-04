var productItemsArray = [];
var removeItemArray = [];
var totalPrice = 0;


var ProductItemManager = {
    gridDataSource: function (modelId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Product/GetAllProductItemByModelId/?modelId=' + modelId,

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
                        BundleQuantity: {
                            type: "number",
                            validation: { required: true, min: 0 },
                        },
                        Price: {
                            type: "number",
                            validation: { required: true, min: 0 },

                        },
                        ManufacturingDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        },
                        IsLisenceRequired: {
                            type: "boolean",

                        },
                        IsPriceApplicable: {
                            type: "boolean",

                        },
                        //ItemCode: {
                        //    type: "text",
                        //    validation: { required: true},
                        //},

                        ItemCodeType: { defaultValue: { ItemCodeId: 0, ItemCode: "--Select--" } },
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });


        return gridDataSource;


    },

    GetProductItemCodeData: function () {
        var objItemCode = "";
        var jsonParam = "";
        var serviceUrl = "../Product/GetProductItemCodeData/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objItemCode = jsonData;
        }
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objItemCode;
    }
};

var ProductItemHelper = {
    GenerateProductItemGrid: function (modelId) {

        var objItems = ProductItemManager.gridDataSource(modelId);

        $("#gridProductItems").kendoGrid({
            dataSource: objItems,
            pageable: false,
            toolbar: [{ name: "create", text: "Add New Item" }],
            columns: [
               
                { field: "ItemId", width: "100px", hidden: true },
                { field: "ItemName", title: "Item Name", width: "80px" },
                { field: "ItemModel", title: "Item Model", width: "50px", },
                { field: "BundleQuantity", title: "Quantity", width: "40px", },
                { field: "Price", title: "Price", width: "40px", editor: ProductItemHelper.PriceTextBoxEditor },
                { field: "WarrantyPeriod", title: "Warranty <br/> Period", width: "40px", editor: ProductItemHelper.WarrantyPeriodTextBoxEditor },
                { field: "ItemCodeType", title: "Item<br/>Code", width: "50px", editor: ProductItemHelper.ItemCodeDropDownEditor, template: "#= ItemCodeType.ItemCode #" },
                { field: "IsLisenceRequired", title: "Replaceable", width: "50px", template: "#= IsLisenceRequired?'Yes':'No'#" },
                { field: "IsPriceApplicable", title: "Price<br/>Applicable", width: "50px", template: "#= IsPriceApplicable?'Yes':'No'#" },
                { field: "Edit", title: "Delete", filterable: false, width: 30, template: '<button type="button" value="Delete" id="btnEdit" onClick="ProductItemHelper.deleteRow()" ><span class="k-icon k-i-close"></span></button>', sortable: false }
            ],
            editable: {
                confirmation: false
            },

            selectable: true,


        });
    },


    ItemCodeDropDownEditor: function (container, options) {
        $('<input required data-text-field="ItemCode" data-value-field="ItemCodeId" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                optionLabel: '--Select--',
                dataSource: ProductItemManager.GetProductItemCodeData(),//[{ 'ItemCodeId': 'LT', 'ItemCode': 'LT' }],
                placeholder: "Please Select",

            });
    },

    CreateProductItemsList: function () {

        productItemsArray = [];

        var gridProductItems = $("#gridProductItems").data("kendoGrid");
        var gridData = gridProductItems.dataSource.data();
        for (var i = 0; i < gridData.length; i++) {
            var items = gridData[i];

            var objitems = new Object();
            objitems.ItemId = items.ItemId == "" ? 0 : items.ItemId;
            objitems.ItemModel = items.ItemModel;
            objitems.ItemName = items.ItemName;
            objitems.BundleQuantity = items.BundleQuantity == "" ? 0 : items.BundleQuantity;
            objitems.ManufacturingDate = items.ManufacturingDate;
            objitems.Price = items.Price;
            objitems.IsLisenceRequired = items.IsLisenceRequired;
            objitems.IsPriceApplicable = items.IsPriceApplicable;
            objitems.WarrantyPeriod = items.WarrantyPeriod;
            objitems.ItemCodeType = {
                ItemCode: items.ItemCodeType.ItemCode
            },
            productItemsArray.push(objitems);
        }
        return productItemsArray;
    },

    PriceTextBoxEditor: function (container, options) {

        $('<input required type="text" value=0" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                autoBind: false,
                placeholder: "Please Select",
                //change: function () {
                //    ProductItemHelper.calculateTotalPrice();
                //}
            });

    },

    WarrantyPeriodTextBoxEditor: function (container, options) {

        $('<input required type="text" value=0" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                autoBind: false,
                placeholder: "Please Select",

            });

    },
    //calculateTotalPrice: function () {

    //    totalPrice = 0;
    //    var grid = $("#gridProductItems").data("kendoGrid");
    //    var gridData = grid.dataSource.data();
    //    for (var i = 0; i < gridData.length; i++) {
    //        var price = gridData[i].Price;
    //        totalPrice += price;
    //    }
    //    $("#lblTotalPrice").val(totalPrice);
    //},

    deleteRow: function () {

        var gridProductItems = $("#gridProductItems").data("kendoGrid");
        var selectedItem = gridProductItems.dataItem(gridProductItems.select());
        removeItemArray.push(selectedItem);
        var price = selectedItem.Price;
        totalPrice = totalPrice - price;
        $("#lblTotalPrice").val(totalPrice);
        gridProductItems.dataSource.remove(selectedItem);

        return removeItemArray;
    },


    ItemCodeInit: function () {
        $("#itemCodePopup").kendoWindow({
            title: "Stock Details",
            resizable: false,
            modal: true,
            width: "30%",
            draggable: true,
            open: function (e) {
                this.wrapper.css({ top: 50 });
            }
        });


        $("#btnSaveItemCode").click(function () {
            ProductItemHelper.SaveItemCode();
        });
    },

    OpenItemCodePopup: function () {
        $("#itemCodePopup").data("kendoWindow").open().center();
    },
    

    SaveItemCode: function () {
        var jsonParam = "itemCode:" + JSON.stringify($("#txtItemCode").val());
        var serviceUrl = "../Product/SaveItemCode";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success', 'Item Code Save Successfully.',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                          $("#itemCodePopup").data("kendoWindow").close();
                         // $("#gridProduct").data("kendoGrid").dataSource.read();
                          $("#gridProductItems").data("kendoGrid").dataSource.read();
                      }
                  }]);
            }
            else if (jsonData == "Exists") {
                AjaxManager.MsgBox('warning', 'center', 'Warning', 'Item Code Already Exist',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                         
                      }
                  }]);
            }

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    }

};

