var gridData = [];
var salesQty = 0;

var gbItemDetailsInfoArray = [];
var itemDetailsArray = [];

var gbTotalPrice = 0;
var gbitemInfoArray = [];


var gbWarrantyEndDate = "";

var ReplacementProductItemInfoManager = {
    InitProductItemInfo: function () {
        $("#ItemSerialNoPopupwindow").kendoWindow({

            title: "Item Si No Settings",
            resizeable: false,
            width: "90%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
            // content: "../Function/FunctionSettings"

        });

        $("#ItemsOldSerialNoPopupwindow").kendoWindow({

            title: "List of Items Old Serial No.",
            resizeable: false,
            width: "30%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,

        });
        ReplacementProductItemInfoHelper.GenerateProductItemInfoGrid();
      

        $("#btnCloseOldSL").click(function() {
            $("#ItemsOldSerialNoPopupwindow").data("kendoWindow").close();
        });

    },


    gridDataSource: function (url) {//modelId
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: url,// '../Product/GetAllProductItemByModelId/?modelId=' + modelId,
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


    GetItemDetailsInformationBySaleId: function (saleId) {
        gbItemDetailsInfoArray = [];
        var jsonParam = "saleId=" + saleId;
        var serviceUrl = "../Sale/GetItemDetailsInformationBySaleId/";
       
        gbItemDetailsInfoArray = AjaxManager.GetDataSource(serviceUrl, jsonParam);
        return gbItemDetailsInfoArray;
    },


    GetItemsOldSLno: function (saleItemId) {
        var objOldSLNo = "";
        var jsonParam = "salesItemId=" + saleItemId;
        var serviceUrl = "../Product/GetItemsOldSLNo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objOldSLNo = jsonData;
        }
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objOldSLNo;
    }


};

var ReplacementProductItemInfoHelper = {
    GenerateProductItemInfoGrid: function () {

        $("#gridProductItemsInfo").kendoGrid({
            dataSource: [],
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },

            filterable: true,
            sortable: true,
            columns: ReplacementProductItemInfoHelper.GenerateProductItemInfoColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },

    GenerateProductItemInfoColumns: function () {
        return columns = [
            { field: "SalesItemId", hidden: true },
            { field: "ItemId", hidden: true },
            { field: "ItemName", title: "Item Name", width: "80px", editable: false },
            { field: "ItemModel", title: "Package", width: "50px", },
            { field: "BundleQuantity", title: "Bundle Qty", width: "60px" },
            { field: "IsLisenceRequired", title: "IsLisenceRequired", width: "60px", hidden: true },
            { field: "Price", title: "Price (BDT)", width: "50px", editable: false, hidden: true }, // editor: ProductItemHelper.PriceTextBoxEditor
            { field: "SalesQty", title: "Sale Qty", width: "50px", editable: false, editor: ReplacementProductItemInfoHelper.ItemSaleQtyTextBoxEditor },
            { field: "Edit", title: "Old SL No", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Details" id="btnEdit" onClick="ReplacementProductItemInfoHelper.ClickEventForDetails()"/>', sortable: false },
            { field: "Edit", title: "", filterable: false, width: 50, template: "#= ReplacementProductItemInfoHelper.GenerateReplaceButton(data)#", sortable: false }
          
        ];
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

        $('<input required type="text" value="0" min="1" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                autoBind: false,
                placeholder: "Please Select",
                change: function (e) {
                    salesQty = options.model.SalesQty;

                    if (salesQty > 0) {
                        //var entityGrid = $("#gridProductItemsInfo").data("kendoGrid");
                        var salesQtyObj = options.model;
                        //entityGrid.dataItem(entityGrid.select());

                        var itemId = salesQtyObj.ItemId;
                        var stock = ReplacementProductItemInfoHelper.CheckStockBalance(salesQty, itemId);
                        if (stock.StockBalanceQty > 0) {
                            
                            //comment for avoid stock availability
                            //if (salesQty > stock.StockBalanceQty) {

                            //    options.model.SalesQty = 0;
                            //    e.preventDefault();
                            //    AjaxManager.MsgBox('warning', 'center', 'Warning', 'Out Of Stock ! Your stock is :" ' + stock.StockBalanceQty,
                            //        [
                            //            {
                            //                addClass: 'btn btn-primary',
                            //                text: 'Ok',
                            //                onClick: function ($noty) {
                            //                    $noty.close();

                            //                }
                            //            }
                            //        ]);
                            //} else {
                                $("#txtTotalPrice").data("kendoNumericTextBox").value("");
                                $("#txtOutstanPrice").data("kendoNumericTextBox").value("");
                                $("#txtInstallment").data("kendoNumericTextBox").value("");
                                ReplacementProductItemInfoHelper.CalculateTotalPrice(salesQtyObj, salesQty);
                           // }
                        }

                    }
                }
            });

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
    CalculateNetTotalPrice: function (gbSaleObj) {

        if (gbSaleObj.price != 0 && gbSaleObj.saleMode == 1) { //1=Installment 

            var installmentYear = $("#txtInstallment").val();
            gbSaleObj.insNumber = installmentYear * 12;
            // var installMonth = installmentYear * 12;
            var interestbyYear = gbobjInterest.Interests * installmentYear;
            //    var interest = (gbobjInterest.Interests * insttYear);
            var downPayPercent = gbobjInterest.DownPay;
            var downPay = ((parseFloat(gbSaleObj.price) * parseFloat(downPayPercent)) / 100);

            // var interest = gbobjInterest.Interests;
            var totalPrice = parseFloat(gbSaleObj.price) + (parseFloat(gbSaleObj.price) * parseFloat((interestbyYear / 100)));
            var netprice = totalPrice - downPay;

            $("#txtDownPay").data("kendoNumericTextBox").value(downPay);
            $("#txtTotalPrice").data("kendoNumericTextBox").value(totalPrice);
            $("#txtOutstanPrice").data("kendoNumericTextBox").value(netprice);

            gbSaleObj.NetPrice = netprice;
        } else {
            $("#txtDownPay").data("kendoNumericTextBox").value(0);
        }
    },

    ItemSiNoTextBoxEditor: function (container, options) {

        $('<input required type="text" id="txtSerialNumber" value=0" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                autoBind: false,
                placeholder: "Please Select",
                change: function () {

                    ReplacementProductItemInfoHelper.CheckExistSiNo(this.value);
                }
            });

    },

    clickEventForSetSLNoButton: function () {
     
        var entityGrid = $("#gridProductItemsInfo").data("kendoGrid");
        var salesQtyObj = entityGrid.dataItem(entityGrid.select());
        var salesQuantity = salesQtyObj.SalesQty;

        var link = "";
        if (salesQtyObj.SalesQty > 0) {
            for (var i = 1; i <= salesQuantity; i++) {
                link += "<li><label for='SLNO' class='lbl widthSize10_per rightAlign'>Item " + i + " SL No : </label><input type='text' id='txtSiNo" + i + "' class='k-textbox' name='SLNO' />" +
                    "<label for='MfgDate' class='lbl widthSize10_per rightAlign'> Mfg. Date : </label><input type='text' id='txtMfgDate" + i + "' data-role='datepicker' name='MfgDate' />" +
                    "<label for='Warranty' class='lbl widthSize10_per rightAlign'>Warranty Period : </label><input type='text' id='txtWarrantyPeriod" + i + "' class='k-textbox' name='Warranty' /></li>";

            }
            $("#hdnItemId").val(salesQtyObj.ItemId);

            ReplacementProductItemInfoHelper.CreateSiNoTextBox(link);

            // --------------------

            if (gbItemDetailsInfoArray.length != 0) {

                for (var j = 0; j < gbItemDetailsInfoArray.length; j++) {

                    var data = gbItemDetailsInfoArray[j];
                    if (data.ItemId == salesQtyObj.ItemId) {
                        $("#txtSalesInvoiceNo").val(data.SalesInvoiceNo);

                        for (var k = 0; k < salesQuantity; k++) {
                            $("#txtSiNo" + (k + 1)).val(data.ItemDetails[k].ItemSLNo);
                            $("#txtMfgDate" + (k + 1)).data('kendoDatePicker').value(data.ItemDetails[k].ItemManufactureDate);
                            $("#txtWarrantyPeriod" + (k + 1)).val(data.ItemDetails[k].ItemWarrantyPeriod);
                        }
                    }

                }

            }


        } else {
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

    ClickEventForDetails: function () {
       
        var entityGrid = $("#gridProductItemsInfo").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
          
            var linkSl = "";
            var oldSLNo = ReplacementProductItemInfoManager.GetItemsOldSLno(selectedItem.SalesItemId);
            
            if (oldSLNo != null) {
                for (var i = 0; i < oldSLNo.length; i++) {
                    linkSl += "<li><label for='SLNO' class='lbl widthSize20_per rightAlign'> SL No " + (i + 1) + " : </label><label  id='txtSiNo" + i + "'  value=" + oldSLNo[i].ItemSLNo + " name='SLNO' >" + oldSLNo[i].ItemSLNo + "</label>";
                }

                $("#ItemsOldSerialNoPopupwindow").data("kendoWindow").open().center();

                $("#ulOldSLNo").html(linkSl);
            }
           
          
        }
        

    },

    CheckStockBalance: function (salesQty, itemId) {

        var existStockOfItem = "";
        var jsonParam = "";
        var serviceUrl = "../Stock/checkExistStockBalanceByItemId/?itemId=" + itemId;
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

    FillItemInfoGridBySaleId: function (saleId) {

        var dataSource = ReplacementProductItemInfoManager.gridDataSource('../Sale/GetSalesItemDataBySaleId/?saleId=' + saleId);
        $("#gridProductItemsInfo").data().kendoGrid.setDataSource(dataSource);

    },

    FillItemsInfoGrid: function (objItemInfo) {
        var itemInfoGrid = $("#gridProductItemsInfo").data("kendoGrid");
        itemInfoGrid.setDataSource(objItemInfo);

    },

    FillItemDetailsInformationBySaleId: function (saleId) {
        ReplacementProductItemInfoManager.GetItemDetailsInformationBySaleId(saleId);
    },


    GenerateReplaceButton: function (gridObj) {
     
        var link = "";
        var itemId = gridObj.ItemId;
        var saleDate = $("#txtInstallDateRe").data("kendoDatePicker").value();
        if (gbItemDetailsInfoArray.length != 0) {

            for (var j = 0; j < gbItemDetailsInfoArray.length; j++) {

                var itemArrayData = gbItemDetailsInfoArray[j];
                if (itemArrayData.ItemId == itemId) {

                    var itemdetails = gbItemDetailsInfoArray[j].ItemDetails;

                    for (var i = 0; i < itemdetails.length; i++) {

                        var warrantyPeriod = itemdetails[i].ItemWarrantyPeriod;

                        var warrantyDate = new Date(saleDate.setMonth(saleDate.getMonth() + warrantyPeriod));

                        gbWarrantyEndDate = warrantyDate;
                        var now = new Date();

                        if (warrantyPeriod > 0 && warrantyDate >= now) {
                            link = '<input type="button" class="k-button" value="Replace" id="btnReplacement" onClick="ReplacementDetailsHelper.ClickEventForReplaceButton();"/>';
                        }
                    }
                }
            }
        }
        return link;
    },


};