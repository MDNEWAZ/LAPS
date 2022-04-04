var SimplePaymentCollectionManager = {
    GetCustomerInfoWithInstallment: function (customerCode) {
        var objModel = "";
        var jsonParam = "customerCode=" + customerCode;
        var serviceUrl = "../SimplePaymentCollection/GetCustomerInfoWithInstallmentAmount/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objModel = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objModel;
    },

    SaveAsDraftPaymentCollection: function () {
        if (SimplePaymentCollectionHelper.validator()) {

            var obj = SimplePaymentCollectionHelper.CreateObjSaveAsDraftData();

            obj = JSON.stringify(obj).replace(/&/g, "^");
            var jsonParam = 'objPaymentInfo:' + obj;
            var serviceUrl = "../SimplePaymentCollection/SaveAsDraftPayment/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Payment Saved As Draft',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            SimplePaymentCollectionHelper.clearAll();
                        }
                    }]);

            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Failed', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Failed', error.statusText,
                         [{
                             addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                 $noty.close();
                             }
                         }]);
        }
    },

    FinalSavePaymentCollection: function (collectionData) {

        var obj = JSON.stringify(collectionData).replace(/&/g, "^");
        var jsonParam = 'objPaymentInfo:' + obj;
        var serviceUrl = "../SimplePaymentCollection/FinalSavePaymentCollection/";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);


        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Payment Collection Finally Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            $("#divPopupPreview").data("kendoWindow").close();
                        }
                    }]);

            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Failed', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Failed', error.statusText,
                         [{
                             addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                 $noty.close();
                             }
                         }]);
        }
    },


    DraftedPaymentsGridDataSource: function (collectionDate) {
        //debugger;
        //if (collectionDate == undefined) {
        //    //collectionDate = $("#txtCollectionDate").data("kendoDatePicker").value();
        //    collectionDate.value = new Date();
        //    collectionDate = kendo.toString(collectionDate, "MM-dd-yyyy");
        //}
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: false,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            //pageSize: 5,
            transport: {
                read: {
                    url: '../SimplePaymentCollection/GetDraftedPaymentDataForGrid/?collectionDate=' + collectionDate,
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
        gridDataSource.filter({ field: "Status", operator: "eq", value: parseInt(0) });
        return gridDataSource;
    },


};


var SimplePaymentCollectionHelper = {
    CreateObjSaveAsDraftData: function () {
        var obj = new Object();
        obj.SimplePaymentCollectionId = $("#hdnSimplePaymentCollection").val();
        obj.CustomerCode = $("#txtCodeCustInfo").val();
        obj.SInvoice = $("#hdnSInvoice").val();
        obj.Amount = $("#txtInstallmentAmount").val();
        obj.Name = $("#txtCustomerName").val();
        obj.Phone = $("#hdnPhone").val();
        obj.CompanyId = $("#hdnCompanyId").val();
        obj.BranchId = $("#hdnBranchId").val();
        obj.BranchSmsMobileNumber = $("#hdnBranchSmsMobileNumber").val();
        obj.ReceiveAmount = $("#txtAmount").data('kendoNumericTextBox').value();
        obj.CollectionType = 1;
        obj.TransectionId = $("#txtCashMemoNo").val();
      
        var payDate = $("#txtCashMemoDate").data('kendoDatePicker').value();
        obj.PayDate = kendo.parseDate(kendo.toString(payDate, "dd/MM/yyyy"), "dd/MM/yyyy");



        return obj;
    },
    changeEventOfCustomerCode: function () {
        var customerCode = $("#txtCodeCustInfo").val();

        if (customerCode != "") {
            var data = SimplePaymentCollectionManager.GetCustomerInfoWithInstallment(customerCode);
            if (data != null) {
                $("#txtCustomerName").val(data.Name);
                $("#txtInstallmentAmount").val(data.Amount);

                $("#hdnSInvoice").val(data.SInvoice);
                $("#hdnBranchSmsMobileNumber").val(data.BranchSmsMobileNumber);

                $("#hdnPhone").val(data.Phone);
                $("#hdnCompanyId").val(data.CompanyId);
                $("#hdnBranchId").val(data.BranchId);


            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Data Not Found or Already have drafted Data!',
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();

                           }
                       }]);
            }
        }



    },
    GenerateCashMemoDatePicker: function () {
        var date = new Date();

        $("#txtCashMemoDate").kendoDatePicker({
            format: "dd-MMM-yyyy",
            value: date
        });
    },

    GenerateCollectionDatePicker: function () {
        var date = new Date();

        $("#txtCollectionDate").kendoDatePicker({
            format: "dd-MMM-yyyy",
            value: date
        });
    },

    clickEventForPreviewButton: function () {
        $("#divPopupPreview").data("kendoWindow").open().center();
        var collectionDate = $("#txtCollectionDate").data("kendoDatePicker").value();
        collectionDate.value = new Date();
        collectionDate = kendo.toString(collectionDate, "MM-dd-yyyy");

        var data = SimplePaymentCollectionManager.DraftedPaymentsGridDataSource(collectionDate);
        $("#divDraftedPaymentGrid").data("kendoGrid").setDataSource(data);

    },

    iniPopupWindow: function () {
        $("#divPopupPreview").kendoWindow({
            title: "Preview",
            resizeable: false,
            width: "80%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
        });
    },

    GenerateDraftedPaymentGrid: function () {
        $("#divDraftedPaymentGrid").kendoGrid({
            dataSource:[],// SimplePaymentCollectionManager.DraftedPaymentsGridDataSource(),
            filterable: true,
            sortable: true,
            columns: SimplePaymentCollectionHelper.GenerateDraftedPaymentsColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",



        });

    },

    GenerateDraftedPaymentsColumns: function () {
        return columns = [
           { field: "SimplePaymentCollectionId", title: "SimplePaymentCollectionId", hidden: true },
           { field: "CustomerCode", title: "CustomerCode", width: 70 },
           { field: "SInvoice", title: "Invoice No", },
           { field: "Phone", title: "Phone", },
           { field: "Name", title: "Name" },//format: "{0:d}" 
           { field: "Amount", title: "Amount", hidden: true },
            { field: "CompanyId", title: "CompanyId", hidden: true },
            { field: "BranchId", title: "BranchId", width: 40, hidden: true },
            { field: "BranchSmsMobileNumber", title: "BranchSmsMobileNumber", hidden: true },
            { field: "ReceiveAmount", title: "ReceiveAmount", },
            { field: "CollectionType", title: "CollectionType", hidden: true },
            { field: "TransectionId", title: "Cash Memo No.", hidden: false },
            { field: "PayDate", title: "Cash Memo Date", template: '#= kendo.toString(kendo.parseDate(kendo.toString(PayDate,"dd-MMM-yyyy"),"dd-MMM-yyyy"),"dd-MMM-yyyy")#' },
           { field: "Edit", title: "Edit", filterable: false, width: 80, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="SimplePaymentCollectionHelper.clickEventForPaymentEditButton()"  />', sortable: false }
        ];
    },

    clickEventForPaymentEditButton: function () {
        var entityGrid = $("#divDraftedPaymentGrid").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        SimplePaymentCollectionHelper.FillPaymentDetailsForm(selectedItem);
        $("#divPopupPreview").data("kendoWindow").close();
    },
    FillPaymentDetailsForm: function (selectedItem) {
        $("#hdnSimplePaymentCollection").val(selectedItem.SimplePaymentCollectionId);
        $("#txtCodeCustInfo").val(selectedItem.CustomerCode);
        $("#txtInstallmentAmount").val(selectedItem.Amount);
        $("#hdnSInvoice").val(selectedItem.SInvoice);
        $("#txtCustomerName").val(selectedItem.Name);
        $("#hdnPhone").val(selectedItem.Phone);
        $("#hdnCompanyId").val(selectedItem.CompanyId);
        $("#hdnBranchId").val(selectedItem.BranchId);
        $("#hdnBranchSmsMobileNumber").val(selectedItem.BranchSmsMobileNumber);
        $("#txtAmount").data('kendoNumericTextBox').value(selectedItem.ReceiveAmount);

        $("#txtCashMemoNo").val(selectedItem.TransectionId);

        $("#txtCashMemoDate").data('kendoDatePicker').value(selectedItem.PayDate);



    },
    clearAll: function () {
        $("input[type='text']").val('');

        $("#SimplePaymentCollectionDiv > form").kendoValidator();//Div id
        $("#SimplePaymentCollectionDiv").find("span.k-tooltip-validation").hide();

        var status = $(".status");

        status.text("").removeClass("invalid");
    },

    validator: function () {
        var data = [];
        var validator = $("#SimplePaymentCollectionDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {

            var chkspAcesName = AjaxManager.checkSpecialCharacters("txtCodeCustInfo");
            if (!chkspAcesName) {
                status.text("Oops! There is invalid data in the form.").addClass("invalid");
                return false;
            }
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },


    FinalSubmitPaymentCollection: function () {
        var collectionData = $("#divDraftedPaymentGrid").data("kendoGrid").dataSource.data();

        var collectionArray = [];
        for (var i = 0; i < collectionData.length; i++) {
            var paymentInfoObj = new Object();

            paymentInfoObj.SimplePaymentCollectionId = collectionData[i].SimplePaymentCollectionId;
            paymentInfoObj.SaleInvoice = collectionData[i].SInvoice;
            paymentInfoObj.ReceiveAmount = collectionData[i].ReceiveAmount;
            paymentInfoObj.CollectionType = collectionData[i].CollectionType;
            paymentInfoObj.TransectionId = collectionData[i].TransectionId;
            paymentInfoObj.Phone = collectionData[i].Phone;
            paymentInfoObj.BranchSmsMobileNumber = collectionData[i].BranchSmsMobileNumber;
            paymentInfoObj.PayDate = kendo.toString(kendo.parseDate(kendo.toString(collectionData[i].PayDate, "MM/dd/yyyy"), "MM/dd/yyyy"), "MM/dd/yyyy");
            paymentInfoObj.BranchCode = collectionData[i].BranchCode;
            paymentInfoObj.Phone2 = collectionData[i].Phone2;
            paymentInfoObj.IsCustomerUpgraded = collectionData[i].IsCustomerUpgraded;
            paymentInfoObj.CustomerCode = collectionData[i].CustomerCode;
            paymentInfoObj.CustomerId = collectionData[i].CustomerId;
            paymentInfoObj.CustomerName = collectionData[i].CustomerName;
            collectionArray.push(paymentInfoObj);
        }


        SimplePaymentCollectionManager.FinalSavePaymentCollection(collectionArray);

    },
    changeeventForCollectionDate: function () {




        var collectionDate = $("#txtCollectionDate").data("kendoDatePicker").value();
        collectionDate = kendo.toString(collectionDate, "MM-dd-yyyy");

        var data = SimplePaymentCollectionManager.DraftedPaymentsGridDataSource(collectionDate);
        $("#divDraftedPaymentGrid").data("kendoGrid").setDataSource(data);


    },

};