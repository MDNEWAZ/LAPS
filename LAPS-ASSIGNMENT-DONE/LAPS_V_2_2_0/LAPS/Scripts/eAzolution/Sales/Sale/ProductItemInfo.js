var gridData = [];
var salesQty = 0;

var gbItemDetailsInfoArray = [];
var itemDetailsArray = [];

var gbTotalPrice = 0;
var gbitemInfoArray = [];



var ProductIntemInfoManager = {
    InitProductItemInfo: function () {
        $("#ItemSerialNoPopupwindow").kendoWindow({

            title: "Item Si No Settings",
            resizeable: false,
            width: "60%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
        });
        ProductItemInfoHelper.GenerateProductItemInfoGrid();
     
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
                    url:url,// '../Product/GetAllProductItemByModelId/?modelId=' + modelId,
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
                        Edit: {
                           // type: "text",
                            editable: false
                        },
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });


        return gridDataSource;
    },


    GetItemDetailsInformationBySaleId: function (saleId) {
        gbItemDetailsInfoArray = [];
        var jsonParam = "saleId=" + saleId;
        var serviceUrl = "../Sale/GetItemDetailsInformationBySaleId/";
       
        gbItemDetailsInfoArray = AjaxManager.GetDataSource(serviceUrl, jsonParam);
        return gbItemDetailsInfoArray;
    },



};

var ProductItemInfoHelper = {
    GenerateProductItemInfoGrid: function () {
     
        $("#gridProductItemsInfo").kendoGrid({
            dataSource: [],
            pageable: {
                refresh: false,
                serverPaging: false,
                serverFiltering: false,
                serverSorting: false
            },
            dataBinding: function (e) {
               
                var dataItems = e.items;
                for (var i = 0; i < dataItems.length; i++) {
                    if (e.items[i].SalesQty == 0 || e.items[i].SalesQty==null) {
                        e.items[i].SalesQty = dataItems[i].BundleQuantity;
                    } 
                }
            },
            filterable: false,
            sortable: false,
            columns: ProductItemInfoHelper.GenerateProductItemInfoColumns(),
            editable: true,
            navigatable: true,
            selectable: "row",
        });

    },

    GenerateProductItemInfoColumns: function () {
        return columns = [
                { field: "SalesItemId", hidden: true },
                { field: "ItemId", hidden: true },
                { field: "ItemName", title: "Name", width: "80px", editable: false },
                { field: "ItemModel", title: "Model", width: "50px", },
                { field: "BundleQuantity", title: "Bundle Qty", width: "60px" },
                { field: "IsLisenceRequired", title: "IsLisenceRequired", width: "60px", hidden: true },
                { field: "IsPriceApplicable", title: "Price App", width: "60px", hidden: true },
                { field: "Price", title: "Price (BDT)", width: "50px", editable: false, hidden: true },// editor: ProductItemHelper.PriceTextBoxEditor
                { field: "SalesQty", title: "Sale Qty", width: "50px", editable: true, editor: ProductItemInfoHelper.ItemSaleQtyTextBoxEditor },//editor: ProductItemInfoHelper.ItemSaleQtyTextBoxEditor  template: "#= ProductItemInfoHelper.saleQty(data) #"
                { field: "Edit", title: "SI No", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Set SL." id="btnEdit" onClick="ProductItemInfoHelper.clickEventForSetSLNoButton()"/>', sortable: false }
                // { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="productSummaryHelper.clickEventForEditButton()"/>', sortable: false }
        ];
    },
    saleQty: function (data) {
       
        return $('<input type="number" class="" value="' + data.BundleQuantity + '" data-bind="value:' + data.BundleQuantity + '"/>').kendoNumericTextBox({
            autoBind: false,
            placeholder: "Please Select",
        });
    },


    detailInitForItemsSINo: function (e) {
        // var urlWithPeram = "../SurveyAnswer/GetEmployeeInfoForAnswers/?surveyId= " + e.data.SurveyId + " &surveyQuestionId= " + e.data.SurveyQuestionId + "&sQAnswerId=" + e.data.SQAnswerId;
        $("<div id='gridItemSino'/>").appendTo(e.detailCell).kendoGrid({

            dataSource: {
                schema: {
                    model: {
                        id: "Id",
                        fields: {
                            Id: { editable: false, nullable: true },
                            ItemSiNo: { type: "text", editable: true, },

                        }
                    },

                }
            },
            scrollable: false,
            sortable: true,
            pageable: false,
            batch: true,
            editable: true,
            selectable: "multiple, row",
            columns: [
                 { field: "ItemSiNo", title: "Item Si No", width: 50, }//editor: ProductItemInfoHelper.ItemSiNoTextBoxEditor


            ]
        });

    },

    ItemSaleQtyTextBoxEditor: function (container, options) {
       
        if (options.model.IsPriceApplicable) {

            $('<input required type="text" id="txtItemId"' + options.model.ItemId + '"   min="1" value="0" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoNumericTextBox({
                    autoBind: false,
                    placeholder: "Please Select",

                    change: function(e) {
                        salesQty = options.model.SalesQty;
                       
                        if (salesQty > 0) {

                            var salesQtyObj = options.model;

                            var isPriceApp = salesQtyObj.IsPriceApplicable;


                            var itemId = salesQtyObj.ItemId;
                           // var stock = ProductItemInfoHelper.CheckStockBalance(salesQty, itemId);
                           // if (stock.StockBalanceQty > 0) {
                                
                                //Comment for avoid stock availability 24/01/2015

                                //if (salesQty > stock.StockBalanceQty) {

                                //    options.model.SalesQty = 0;
                                //    e.preventDefault();
                                //    AjaxManager.MsgBox('warning', 'center', 'Warning', 'Out Of Stock ! Your stock is : ' + stock.StockBalanceQty,
                                //        [
                                //            {
                                //                addClass: 'btn btn-primary',
                                //                text: 'Ok',
                                //                onClick: function($noty) {
                                //                    $noty.close();

                                //                }
                                //            }
                                //        ]);
                                //} else {

                                    if (isPriceApp) {

                                        $("#txtTotalPrice").data("kendoNumericTextBox").value("");
                                        $("#txtOutstanPrice").data("kendoNumericTextBox").value("");
                                       // $("#txtInstallment").data("kendoNumericTextBox").value("");
                                        //  ProductItemInfoHelper.CalculateTotalPrice(salesQtyObj, salesQty);//not in use
                                        ProductItemInfoHelper.CalculateTotalPriceWhenChangeQty(salesQtyObj, salesQty);

                                       
                                    }

                                //}
                           // }

                        }
                    }
                });

        }
    },

    CalculateTotalPrice: function (salesQtyObj, salesQty) {
      
        var objItemId = new Object();
        objItemId.ItemId = salesQtyObj.ItemId;
       
        var bundleQty = salesQtyObj.BundleQuantity;
        var price = salesQtyObj.Price;
        var unitPrice = (price / bundleQty);
        var salesItemPrice = (salesQty * parseFloat(unitPrice));
        
        if (gbitemInfoArray.length > 0) {
            for (var i = 0; i < gbitemInfoArray.length; i++) {
                if (gbitemInfoArray[i].ItemId == salesQtyObj.ItemId) {

                    gbTotalPrice = gbTotalPrice - gbitemInfoArray[i].Price;
                    gbTotalPrice += salesItemPrice;

                    for (var m = 0; m < gbitemInfoArray.length; m++) {
                        if (gbitemInfoArray[m].ItemId == salesQtyObj.ItemId) {
                            gbitemInfoArray.splice(m, 1);
                            break;
                        }
                    }

                    objItemId.Price = salesItemPrice;
                    gbitemInfoArray.push(objItemId);

                } else {

                    for (var p = 0; p < gbitemInfoArray.length; p++) {
                        if (gbitemInfoArray[p].ItemId == salesQtyObj.ItemId) {
                            gbTotalPrice = gbTotalPrice - gbitemInfoArray[p].Price;
                            gbitemInfoArray.splice(p, 1);
                           
                            break;
                        } 
                    }
                  
                    objItemId.Price = salesItemPrice;
                    gbitemInfoArray.push(objItemId);
                    gbTotalPrice += salesItemPrice;
                }
            }


        } else {

            objItemId.Price = salesItemPrice;
            gbitemInfoArray.push(objItemId);

            gbTotalPrice += salesItemPrice;
        }
       
        $("#txtPrice").data("kendoNumericTextBox").value(gbTotalPrice);
    
        //-------------------calculation Down Pay-------------------
        var downpayment = parseFloat(gbTotalPrice) * (parseFloat(gbobjInterest.DownPay) / 100);
        $("#txtDownPay").data("kendoNumericTextBox").value(parseFloat(downpayment));

        //-------------------Total Price-------------------

        //var totalPrice = (gbTotalPrice - parseFloat(downpayment)) +((gbTotalPrice - parseFloat(downpayment)) * (parseFloat(gbobjInterest.Interests) / 100));
        //$("#txtTotalPrice").data("kendoNumericTextBox").value(parseFloat(totalPrice));

    },

    //change event of credit period year/installment
    CalculateNetTotalPrice: function (saleObj) {
     
        if (saleObj.price != 0 && saleObj.saleMode == 1)//1=Installment 
        {
            var installmentNo = $("#txtInstallment").val();
            var installmentYear = installmentNo / 12;
            saleObj.insNumber = installmentNo;
            var interestbyYear = gbobjInterest.Interests * installmentYear;
            
           // var downPayPercent = gbobjInterest.DownPay;
            var downPayPercent = gbDownPayPercentage;
            //New Code by Rubel on 29/06/2016 for fixed DP or Pacentage DP

            var downPay = 0;
            if (saleObj.IsDPFixedAmount == 0) {
                 downPay = ((parseFloat(saleObj.price) * parseFloat(downPayPercent)) / 100);
            } else {
                downPay = downPayPercent;
            }
            var priceWithoutDownPay = (saleObj.price - downPay);
            var totalPrice = parseFloat(priceWithoutDownPay) + (parseFloat(priceWithoutDownPay) * parseFloat((interestbyYear / 100)));
            var netprice = totalPrice + downPay;

            $("#txtDownPay").data("kendoNumericTextBox").value(downPay);
            $("#txtTotalPrice").data("kendoNumericTextBox").value(netprice);
            $("#txtOutstanPrice").data("kendoNumericTextBox").value(totalPrice);
            
            $("#txtPrice").data("kendoNumericTextBox").value(saleObj.price);
            //New Requirment

            $("#txtDownPayment").val(downPay);
            var saleDate= $("#cmbSaleDate").data("kendoDatePicker").value();
            $("#txtCollectionDate").data("kendoDatePicker").value(saleDate);
            
            var previousCollectedAmtInput = $("#txtCollectedAmount").val();
            //if (previousCollectedAmtInput == "") {
            //    $("#txtCollectedAmount").val(downPay);

            //}
            $("#txtCollectedAmount").val(downPay);
            
           

            saleObj.NetPrice = netprice;
        } else {

            var newsaleObj = saleDetailsHelper.DiscountCalculationForCashSale();

            $("#txtPrice").data("kendoNumericTextBox").value(newsaleObj.price);
            $("#txtTotalPrice").data("kendoNumericTextBox").value(newsaleObj.price);
            
            $("#txtDownPay").data("kendoNumericTextBox").value(0);
        }

        
    },
    
    CalculateTotalPriceWhenChangeQty: function (salesQtyObj, salesQty) {
        var objItemId = new Object();
        objItemId.ItemId = salesQtyObj.ItemId;
   
        var bundleQty = salesQtyObj.BundleQuantity;
        var unitPrice = salesQtyObj.Price;
        var packagePrice = salesQtyObj.PackagePrice;
        if (bundleQty != salesQty) {
            packagePrice = parseFloat(packagePrice) + parseFloat((parseFloat(unitPrice) * parseInt(salesQty - bundleQty)));
            $("#txtPrice").data("kendoNumericTextBox").value(packagePrice);
            
        } else {
            $("#txtPrice").data("kendoNumericTextBox").value(salesQtyObj.PackagePrice);
        }

        var saleMode = $("#cmbSaleType").data("kendoComboBox").value();
        if (saleMode == "1") {
            var obj = new Object();
            obj.price = packagePrice;
            obj.insNumber = 0;
            obj.saleMode = 1; //installment
            ProductItemInfoHelper.CalculateNetTotalPrice(obj);
            var saleObj = saleDetailsHelper.GetSalesObj();
            saleDetailsHelper.InstallmentData(saleObj);
            
        } else {
            $("#txtTotalPrice").data("kendoNumericTextBox").value(packagePrice);
            $("#txtOutstanPrice").data("kendoNumericTextBox").value(packagePrice);
        }
       
    },

    ItemSiNoTextBoxEditor: function (container, options) {

        $('<input required type="text" id="txtSerialNumber" value=0" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                autoBind: false,
                placeholder: "Please Select",
                change: function () {

                    ProductItemInfoHelper.CheckExistSiNo(this.value);
                }
            });

    },

    clickEventForSetSLNoButton: function () {
       
        var entityGrid = $("#gridProductItemsInfo").data("kendoGrid");
        var salesQtyObj = entityGrid.dataItem(entityGrid.select());
        var salesQuantity = salesQtyObj.SalesQty;

        var link = "";
        if (salesQtyObj.SalesQty > 0) {
            for (var i = 1; i <= salesQuantity ; i++) {
                link += "<li><label for='SLNO' class='lbl widthSize10_per rightAlign'>Item " + i + " SL No : </label><input type='text' id='txtSiNo" + i + "' class='k-textbox' name='SLNO' />" +
                    //"<label for='MfgDate' class='lbl widthSize10_per rightAlign'> Mfg. Date : </label><input type='text' id='txtMfgDate" + i + "' data-role='datepicker' name='MfgDate' />" +
                    "<label for='Warranty' class='lbl widthSize20_per rightAlign'>Warranty Period : </label><input type='text' id='txtWarrantyPeriod" + i + "' value=" + salesQtyObj.WarrantyPeriod + "  class='k-textbox' name='Warranty' /></li>";

            }
            $("#hdnItemId").val(salesQtyObj.ItemId);

            ProductItemInfoHelper.CreateSiNoTextBox(link);

            // --------------------
          
            if (gbItemDetailsInfoArray.length != 0) {
               
                for (var j = 0; j < gbItemDetailsInfoArray.length; j++) {
                   
                    var data = gbItemDetailsInfoArray[j];
                    if (data.ItemId == salesQtyObj.ItemId) {
                       // $("#txtSalesInvoiceNo").val(data.SalesInvoiceNo);
                      
                        for (var k = 0; k < salesQuantity; k++) {
                            $("#txtSiNo" + (k+1)).val(data.ItemDetails[k].ItemSLNo);
                            //$("#txtMfgDate" + (k+1)).data('kendoDatePicker').value(data.ItemDetails[k].ItemManufactureDate);
                            $("#txtWarrantyPeriod" + (k + 1)).val(data.ItemDetails[k].ItemWarrantyPeriod);
                        }
                    }

                }

            }



        }
        else {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please Input Sale Quantity',
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

    },

    CreateSiNoTextBox: function (link) {

        $("#ItemSerialNoPopupwindow").data("kendoWindow").open().center();
        $("#ulSerialNo").html(link);
        kendo.init($("#ulSerialNo"));

    },

    ClickEventForOkButton: function () {
        $("#ItemSerialNoPopupwindow").data("kendoWindow").close();

        itemDetailsArray = [];

        var entityGrid = $("#gridProductItemsInfo").data("kendoGrid");
        var salesQtyObj = entityGrid.dataItem(entityGrid.select());

        var salesitemDetailsObj = new Object();
        salesitemDetailsObj.ItemId = salesQtyObj.ItemId;
        salesitemDetailsObj.SalesInvoiceNo = $("#txtSalesInvoiceNo").val();

        for (var i = 1; i <= salesQtyObj.SalesQty; i++) {
            var sl = $("#txtSiNo" + i).val();
            if (sl != "") {
                var objItemDetails = new Object();


                var mfgdate = $("#txtMfgDate" + i).val();
                var warrantyPeriod = $("#txtWarrantyPeriod" + i).val();

                objItemDetails.ItemSLNo = sl;
                objItemDetails.ItemManufactureDate = mfgdate;
                objItemDetails.ItemWarrantyPeriod = warrantyPeriod;

                itemDetailsArray.push(objItemDetails);

            }

        }


        salesitemDetailsObj.ItemDetails = itemDetailsArray;
       
        for (var p = 0; p < gbItemDetailsInfoArray.length; p++) {
            if (gbItemDetailsInfoArray[p].ItemId == salesQtyObj.ItemId) {
                gbItemDetailsInfoArray.splice(p, 1);
                break;
            }
        }

     

        gbItemDetailsInfoArray.push(salesitemDetailsObj);

     


    },


    CheckStockBalance: function (salesQty, itemId) {

        var existStockOfItem = "";
        var stockCategory = 1; //Sale
        var jsonParam = "";
        var serviceUrl = "../Stock/checkExistStockBalanceByItemId/?itemId=" + itemId + "&stockCategoryId=" + stockCategory;
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            existStockOfItem = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return existStockOfItem;
    },


    GetProductItemInfoGridList: function () {
        var gridData = $("#gridProductItemsInfo").data("kendoGrid").dataSource.data();
        return gridData;
    },

    FillItemInfoBySaleId: function (saleId) {
       
        var dataSource = ProductIntemInfoManager.gridDataSource('../Sale/GetSalesItemDataBySaleId/?saleId=' + saleId);
        $("#gridProductItemsInfo").data().kendoGrid.setDataSource(dataSource);
    },

    FillItemsInfoGrid: function (objItemInfo) {
        var itemInfoGrid = $("#gridProductItemsInfo").data("kendoGrid");
        itemInfoGrid.setDataSource(objItemInfo);

    },

    FillItemDetailsInformationBySaleId:function(saleId) {
        ProductIntemInfoManager.GetItemDetailsInformationBySaleId(saleId);
    }

};