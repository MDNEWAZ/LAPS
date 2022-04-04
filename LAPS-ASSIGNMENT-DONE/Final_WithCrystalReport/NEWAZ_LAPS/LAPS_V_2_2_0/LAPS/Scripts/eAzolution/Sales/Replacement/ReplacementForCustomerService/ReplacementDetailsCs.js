var gbSelectedItemsArray = [];

var ReplacementDetailsCsManager = {
    SaveReplacementForCustomerService: function () {
        var validator = $("#replacementCsDetailsDiv").kendoValidator().data("kendoValidator"),
           status = $(".status");
        if (validator.validate()) {
         var replacementObj = ReplacementDetailsCsHelper.CreateReplacementObject();
         var itemList = gbSelectedItemsArray;
         var objReplacementCs = JSON.stringify(replacementObj);
         var objItemList = JSON.stringify(itemList);
         var jsonParam = "objReplacementCs:" + objReplacementCs + ",objItemList:" + objItemList;
         var serviceUrl = "../Replacement/SaveReplacementForCustomerService/";
         AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }
         function onSuccess(jsonData) {
             if (jsonData == "Success") {
               
                     AjaxManager.MsgBox('success', 'center', 'Success', 'Replacement Save Successfully.',
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();
                            
                           }
                       }]);
                

             }
             else if (jsonData == "Exists") {
                 AjaxManager.MsgBox('warning', 'center', 'Alresady Exist:', 'already exist.',[{addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {$noty.close();}}]);
             }
             else {
                 AjaxManager.MsgBox('error', 'center', 'Error', jsonData,[{addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {$noty.close(); }}]);
             }
         }

         function onFailed(error) {
             window.alert(error.statusText);
         }
     },
     
     gridDataSource: function (url) {
         var gridDataSource = new kendo.data.DataSource({

             type: "json",
             serverPaging: true,
             serverSorting: false,
             serverFiltering: false,
             allowUnsort: true,
             pageSize: 10,

             transport: {
                 read: {
                     url: url,// '../Product/GetAllProductItemByModelId/?modelId=' + modelId,
                     type: "POST",
                     dataType: "json",
                     cache: false,
                     async: false,
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
                             type: "number", validation: { required: true },
                             min: 0,
                             editable: false
                         },
                         Price: {
                             type: "number",
                             editable: false
                         },
                         SalesQty: {
                             type: "number",

                         },
                         ItemName: {
                             type: "text",
                             editable: false
                         },
                         ItemModel: {
                             type: "text",
                             editable: false
                         },

                     }
                 },
                 data: "Items", total: "TotalCount"
             }
         });


         return gridDataSource;
     },
    
     CheckStockBalance: function (salesQty, itemId) {
         var branchId = $("#cmbBranch").data("kendoComboBox").value();
         var existStockOfItem = "";
         var stockCategory = 2; //Replacement
         var jsonParam = "";
         var serviceUrl = "../Replacement/checkExistStockBalanceByItemId/?itemId=" + itemId + "&stockCategoryId=" + stockCategory + "&branchId=" + branchId;
         AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

         function onSuccess(jsonData) {
             existStockOfItem = jsonData;
         }
         function onFailed(error) {
             window.alert(error.statusText);
         }
         return existStockOfItem;
     },
};


var ReplacementDetailsCsHelper = {

    InitReplacementDetailsCs: function () {
        empressCommonHelper.GenerateBranchCombo(0, "cmbBranch");
        empressCommonHelper.GenerateModelCombo("cmbPackage");
        ReplacementDetailsCsHelper.GenerateDatePicker();

        $("#btnSaveReplacementCs").click(function() {
            ReplacementDetailsCsManager.SaveReplacementForCustomerService();
        });
        
        ReplacementDetailsCsHelper.GeneratePackageItemsGrid();
        
        $("#cmbPackage").change(function () {
            var modelId = $("#cmbPackage").data("kendoComboBox").value();
            ReplacementDetailsCsHelper.PopulatePackageDetails(modelId);
          
        });

        ReplacementDetailsCsHelper.ItemGridChangeEvent();
        
        $("#btnClear").click(function () {
            ReplacementDetailsCsHelper.ClearCustomerServiceReplaceForm();
        });
        

    },

    GenerateDatePicker: function () {
        $("#txtReplacementDate").kendoDatePicker({
            value: new Date(),
            format: "dd/MM/yyyy"
        });
    },
    CreateReplacementObject: function () {
        var obj = new Object();
        obj.ReplacementCsId = $("#hdnReplacementCsId").val();
        obj.BranchId = $("#cmbBranch").data("kendoComboBox").value();
        obj.ModelId = $("#cmbPackage").data("kendoComboBox").value();
        obj.ReplacementDate = $("#txtReplacementDate").data("kendoDatePicker").value();
        return obj;
    },
    
    PopulatePackageDetails: function (modelId) {
        var dataSource = ReplacementDetailsCsManager.gridDataSource('../Product/GetAllProductItemByModelId/?modelId=' + modelId);
        $("#gridProductItemsInfo").data().kendoGrid.setDataSource(dataSource);
    },
    GeneratePackageItemsGrid: function() {
        $("#gridProductItemsInfo").kendoGrid({
            dataSource: [],
            pageable: {
                refresh: false,
                serverPaging: false,
                serverFiltering: false,
                serverSorting: false
            },
          
            filterable: false,
            sortable: false,
            columns: ReplacementDetailsCsHelper.GenerateProductItemInfoColumns(),
            editable: true,
            navigatable: true,
            selectable: "row",
        });
    },
    GenerateProductItemInfoColumns: function () {
        return columns = [
                { field: "check_row", title: "", width: 30, template: "<input class='check_row' type='checkbox' />", headerTemplate: "<input type='checkbox' id='chkSelectAll'/>", filterable: false, sortable: false },
                { field: "ItemId", hidden: true },
                { field: "ItemName", title: "Name", width: "80px", editable: false },
                { field: "ItemModel", title: "Model", width: "50px", },
                { field: "BundleQuantity", title: "Bundle Qty", width: "60px" },
                { field: "IsLisenceRequired", title: "IsLisenceRequired", width: "60px", hidden: true },
                { field: "IsPriceApplicable", title: "Price App", width: "60px", hidden: true },
                { field: "Price", title: "Price (BDT)", width: "50px", editable: false, hidden: true },
                { field: "ReplacedItemQty", title: "Replace Item Qty", width: "60px", editor: ReplacementDetailsCsHelper.ItemQtyNumericTextBoxEditor }
               
        ];
    },
 
    ItemQtyNumericTextBoxEditor: function (container, options) {

            $('<input required type="text" id="txtItemId"' + options.model.ItemId + '"   min="1" value="0" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoNumericTextBox({
                    autoBind: false,
                    placeholder: "Please Input Qty",
                    change: function (e) {
                        var replaceItemQty = options.model.ReplacedItemQty;
                        if (replaceItemQty > 0) {
                            var replaceItemQtyObj = options.model;
                            var itemId = replaceItemQtyObj.ItemId;
                            var stock = ReplacementDetailsCsManager.CheckStockBalance(replaceItemQty, itemId);
                            if (stock != null) {
                                if (stock.StockBalanceQty > 0) {

                                    if (replaceItemQty > stock.StockBalanceQty) {

                                        options.model.ReplacedItemQty = 0;
                                        e.preventDefault();
                                        AjaxManager.MsgBox('warning', 'center', 'Warning', 'Out Of Stock ! Your stock is : ' + stock.StockBalanceQty,
                                            [
                                                {
                                                    addClass: 'btn btn-primary',
                                                    text: 'Ok',
                                                    onClick: function($noty) {
                                                        $noty.close();

                                                    }
                                                }
                                            ]);
                                    }
                                }
                            } else {
                                options.model.ReplacedItemQty = 0;
                                e.preventDefault();
                                AjaxManager.MsgBox('warning', 'center', 'Warning', 'Out Of Stock ! ',
                                    [
                                        {
                                            addClass: 'btn btn-primary',
                                            text: 'Ok',
                                            onClick: function ($noty) {
                                                $noty.close();

                                            }
                                        }
                                    ]);
                            }
                           
                        }
                    }
                });

        
    },
    
    ItemGridChangeEvent: function () {
       
        $('#gridProductItemsInfo').on('change', '.check_row', function (e) {
           
            var $target = $(e.currentTarget);
            var grid = $("#gridProductItemsInfo").data("kendoGrid");
            var dataItem = grid.dataItem($(this).closest('tr'));
            if ($target.prop("checked")) {
                if (dataItem.Invoice != "") {
                    gbSelectedItemsArray.push(dataItem);
                } else {
                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Input Invoice No First.!',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            $("#gridProductItemsInfo tbody tr input:checkbox").removeAttr("checked", true);
                            $('tr.k-state-selected', '#gridProductItemsInfo').removeClass('k-state-selected');
                        }
                    }]);
                }

            } else {
                for (var i = 0; i < gbSelectedItemsArray.length; i++) {
                    if (gbSelectedItemsArray[i].SaleId == dataItem.SaleId) {
                        gbSelectedItemsArray.splice(i, 1);
                        break;
                    }
                }

            }

        });

        $('#gridProductItemsInfo').on('change', '#chkSelectAll', function (e) {
            gbSelectedItemsArray = [];
            var gridItems = $("#gridProductItemsInfo").data("kendoGrid");
            var selectAll = document.getElementById("chkSelectAll");
            if (selectAll.checked == true) {
                $("#gridProductItemsInfo tbody input:checkbox").attr("checked", true);
                $("#gridProductItemsInfo table tr").addClass('k-state-selected');
                var gridData = gridItems.dataSource.data();

                for (var i = 0; i < gridData.length; i++) {
                    var itemInfo = gridData[i];
                    if (itemInfo.ItemId >0) {
                        gbSelectedItemsArray.push(itemInfo);
                    } else {
                        $("#gridProductItemsInfo tbody tr input:checkbox").removeAttr("checked", true);
                        $("#gridProductItemsInfo table tr").removeClass('k-state-selected');
                    }

                }

            } else {
                $("#gridProductItemsInfo tbody input:checkbox").removeAttr("checked", this.checked);
                $("#gridProductItemsInfo table tr").removeClass('k-state-selected');
                gbSelectedItemsArray = [];
            }
        });
    },
    
    ClearCustomerServiceReplaceForm:function() {
        $("#hdnReplacementCsId").val(0);
        $("#cmbBranch").data("kendoComboBox").value("");
        $("#cmbPackage").data("kendoComboBox").value("");
        $("#txtReplacementDate").data("kendoDatePicker").value("");
        
        $("#replacementCsDetailsDiv > form").kendoValidator();
        $("#replacementCsDetailsDiv").find("span.k-tooltip-validation").hide();

        var status = $(".status");
        status.text("").removeClass("invalid");
        
        var grid = $("#gridProductItemsInfo").data("kendoGrid");
        if (grid != undefined) {
            $("#gridProductItemsInfo").data("kendoGrid").dataSource.data([]);
        }
    },
}