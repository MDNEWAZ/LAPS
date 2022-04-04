var gbSaleName = "";
var gbobjInterest = 0;
var gbDownPayPercent = 0;
var gbDownPayPercentage = 0;
var gbNetPrice = 0;


var gbUnrecogSaleObj = [];
var type = "";



var saleDetailsManager = {

    InitislSaleDetails: function () {
        saleDetailsHelper.GenerateSaleTypeCombo();
        saleDetailsHelper.GenerateNumericTextBox();

        // This is Global value, used by different location
        gbobjInterest = saleDetailsManager.GetobjInterest();
        saleDetailsHelper.ChangeEventForSaleType();

        //TypeId (D-Type Or R-Type)

        saleDetailsHelper.GenerateProductCombo();
        var objProduct = productDetailsManager.GetProductType();
        $("#cmbType").data('kendoComboBox').setDataSource(objProduct);
    },

    GetSaleType: function () {
        var objSale = "";
        var jsonParam = "";
        var serviceUrl = "../Sale/GetSaleType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objSale = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objSale;
    },

    GetobjInterest: function () {
        var objInterest = "";
        var jsonParam = "";
        var serviceUrl = "../Sale/GetAInterest/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objInterest = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objInterest;
    },

    GetDefaultInstallmentNo: function () {
        var objInstallmentNo = "";
        var jsonParam = "";
        var serviceUrl = "../Sale/GetDefaultInstallmentNo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objInstallmentNo = jsonData;

            $("#txtInstallment").data("kendoNumericTextBox").value(objInstallmentNo);
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objInstallmentNo;
    },

    GetDefaultInstallmentNoByPackageWise: function (modelId) {
        var objInstallmentNo = "";
        var jsonParam = "modelId=" + modelId;
        var serviceUrl = "../Sale/GetPackageWiseDefaultInstallmentNoByModelId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objInstallmentNo = jsonData;

            $("#txtInstallment").data("kendoNumericTextBox").value(objInstallmentNo);
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objInstallmentNo;
    },

    //*************************************************Save Sale *******************************************************

    SaveAsBooked: function (state) {
        var saleId = $("#hdnSaleId").val();
        if (saleId > 0) {

            if ($("#txtInvoiceSale").val() != "") {
                AjaxManager.MsgBox('information', 'center', 'Confirm Update:', 'Are You Confirm to Update Sale Information ? ',
                 [{
                     addClass: 'btn btn-primary',
                     text: 'Yes',
                     onClick: function ($noty) {
                         $noty.close();
                         saleDetailsManager.SaveOrUpdateSale(state);
                     }
                 },
              {
                  addClass: 'btn',
                  text: 'No',
                  onClick: function ($noty) {
                      $noty.close();

                  }
              }]);

            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please Enter Invoice No!',
                     [{
                         addClass: 'btn btn-primary',
                         text: 'Ok',
                         onClick: function ($noty) {
                             $noty.close();
                             return false;
                         }
                     }]);

            }


        }
        else if ($("#txtInvoiceSale").val() != "") {

            saleDetailsManager.SaveOrUpdateSale(state);
        }
        else {
            AjaxManager.MsgBox('warning', 'center', 'Minimum Requirment:', 'Please Enter Minimum Requirement.',
                       [{
                           addClass: 'btn btn-primary',
                           text: 'Ok',
                           onClick: function ($noty) {
                               $noty.close();
                               return false;
                           }
                       }]);
        }

    },

    SaveOrUpdateSale: function (state) {

        var customerInfoObj = saleDetailsHelper.CreateCustomerInfoObject();
        var objCustomerInfo = JSON.stringify(customerInfoObj).replace(/&/g, "^");

        var saleObject = saleDetailsManager.GetSaleObject();
        saleObject.State = state;
        var objSaleInfo = JSON.stringify(saleObject).replace(/&/g, "^");

        var objItemInformationList = ProductItemInfoHelper.GetProductItemInfoGridList();
        var objItemInfoList = JSON.stringify(objItemInformationList);

        var objItemDetailsInfoList = JSON.stringify(gbItemDetailsInfoArray);

        var objInstallmentList = saleDetailsManager.GetInstallmentGridList();
        var objInstallList = JSON.stringify(objInstallmentList);

        var downPaymentCollectinObj = saleDetailsHelper.DownPaymentCollectionObj();
        var objDownPayCollection = JSON.stringify(downPaymentCollectinObj);

        var dicountInfoObj = saleDetailsHelper.CreateDiscountInfoObject();
        var objDiscountInfo = JSON.stringify(dicountInfoObj).replace(/&/g, "^");

        var packageId = $("#cmbModelProInfo").data("kendoComboBox").value();
        if (packageId > 0) {
            //var phoneNo = $("#txtMobileNo1").val();
            var customerId = $("#txtCustomerCode").val();

            if (customerId != "") {
                var jsonParam = 'strObjSaleInfo:' + objSaleInfo + ",objInstallmentList:" + objInstallList + ",objItemInfoList:" + objItemInfoList +
                    ",objItemDetailsInfoList:" + objItemDetailsInfoList + ",objDownPayCollection:" + objDownPayCollection + ",objCustomerInfo:" + objCustomerInfo + ",objDiscountInfo:" + objDiscountInfo;
                var serviceUrl = "../Sale/SaveSale/";
                AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
            }
            else {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Enter NID or Phone No 1 !',
                     [{
                         addClass: 'btn btn-primary',
                         text: 'Ok',
                         onClick: function ($noty) {
                             $noty.close();
                         }
                     }]);
            }

        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select a Package !',
                      [{
                          addClass: 'btn btn-primary',
                          text: 'Ok',
                          onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                // saleDetailsHelper.ClearAllSaleDetailsForm();
                if (true) {
                    AjaxManager.MsgBox('success', 'center', 'Success', 'New Sale Saved Successfully.',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                saleDetailsHelper.ClearAllSaleDetailsForm();
                            }
                        }]);
                }
                $("#gridSale").data("kendoGrid").dataSource.read();
            } else if (jsonData == "Exists") {
                AjaxManager.MsgBox('warning', 'center', 'Empty Inventory Info:', 'Invoice No Already Exist!',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }]);
            }
            else if (jsonData == "EmptyInventoryInfo") {
                AjaxManager.MsgBox('warning', 'center', 'Exist:', 'Stock Is Not Avilable!',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }]);
            } else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }]);
            }
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    SaveAsDraft: function () {
        var bookedSaleObj = gbSelectiveBookedSaleArray;
        if (bookedSaleObj.length > 0) {
            AjaxManager.MsgBox('information', 'center', 'Confirm Update:', 'Are You Confirm to Sale Save As Draft? ',
               [{
                   addClass: 'btn btn-primary',
                   text: 'Yes',
                   onClick: function ($noty) {
                       $noty.close();
                       saleDetailsManager.SaleSaveAsDraft(bookedSaleObj);
                   }
               },
               {
                   addClass: 'btn',
                   text: 'No',
                   onClick: function ($noty) {
                       $noty.close();
                   }
               }]);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'No Booked Sales to Submit!',
           [{
               addClass: 'btn btn-primary',
               text: 'OK',
               onClick: function ($noty) {
                   $noty.close();
               }
           }]);
        }


    },

    SaleSaveAsDraft: function (bookedSaleObj) {
        var objSalesObjList = JSON.stringify(bookedSaleObj);
        if (objSalesObjList != "") {
            var jsonParam = "objSalesObjList:" + objSalesObjList;
            var serviceUrl = "../Sale/SaveSaleAsDraft/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Data No Found!',
                      [{
                          addClass: 'btn btn-primary',
                          text: 'Ok',
                          onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
        }


        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                saleDetailsHelper.ClearAllSaleDetailsForm();
                if (true) {
                    AjaxManager.MsgBox('success', 'center', 'Success', 'Save As Draft Successfully.',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#gridBookedSale").data("kendoGrid").dataSource.read();
                                gbSelectiveBookedSaleArray = [];
                            }
                        }]);
                }

            } else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            gbSelectiveBookedSaleArray = [];
                        }
                    }]);
            }
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    SubmitSalesFinnaly: function () {
        var draftedSaleObjList = gbSelectiveDraftedSaleArray;

        if (draftedSaleObjList.length > 0) {
            AjaxManager.MsgBox('information', 'center', 'Confirm Update:', 'Are You Confirm to Sumbit Sales Finally? ',
               [{
                   addClass: 'btn btn-primary',
                   text: 'Yes',
                   onClick: function ($noty) {
                       $noty.close();
                       saleDetailsManager.FinalSubmitSales(draftedSaleObjList);
                   }
               },
               {
                   addClass: 'btn',
                   text: 'No',
                   onClick: function ($noty) {
                       $noty.close();

                   }
               }]);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'No Drafted Sales to Submit!',
           [{
               addClass: 'btn btn-primary',
               text: 'OK',
               onClick: function ($noty) {
                   $noty.close();

               }
           }]);
        }


    },

    FinalSubmitSales: function (draftedSaleObjList) {

        var objSalesObjList = JSON.stringify(draftedSaleObjList);

        if (objSalesObjList != "") {

            var jsonParam = "objSalesObjList:" + objSalesObjList;
            var serviceUrl = "../Sale/SaveFinalSale/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Data No Found!',
                      [{
                          addClass: 'btn btn-primary',
                          text: 'Ok',
                          onClick: function ($noty) {
                              $noty.close();

                          }
                      }]);
        }


        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                saleDetailsHelper.ClearAllSaleDetailsForm();
                if (true) {
                    AjaxManager.MsgBox('success', 'center', 'Success', 'Final Submit Successfully.',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#gridSale").data("kendoGrid").dataSource.read();
                                gbSelectiveDraftedSaleArray = [];
                            }
                        }]);
                }

            }
            else if (jsonData == "DPNotFound") {
                saleDetailsHelper.ClearAllSaleDetailsForm();
                if (true) {
                    AjaxManager.MsgBox('warning', 'center', 'Warning', 'Downpayment is not collected yet!',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#gridSale").data("kendoGrid").dataSource.read();
                                gbSelectiveDraftedSaleArray = [];
                            }
                        }]);
                }

            }
            else if (jsonData == "SuccessDPNotFound") {
                saleDetailsHelper.ClearAllSaleDetailsForm();
                if (true) {
                    AjaxManager.MsgBox('success', 'center', 'Success', 'Final Sale Submit Successfully And Downpayment is not collected rest of Sale',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#gridSale").data("kendoGrid").dataSource.read();
                                gbSelectiveDraftedSaleArray = [];
                            }
                        }]);
                }

            } else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }]);
            }
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    SaveAsUnrecognized: function () {

        AjaxManager.MsgBox('information', 'center', 'Confirm Update:', 'Are You Confirm to Unrecognized ? ',
               [{
                   addClass: 'btn btn-primary',
                   text: 'Yes',
                   onClick: function ($noty) {
                       $noty.close();

                       $("#divAddCommentsPopup").data("kendoWindow").open().center();

                       //saleDetailsManager.MakeUnrecognized(saleObj, type);
                   }
               },
            {
                addClass: 'btn',
                text: 'No',
                onClick: function ($noty) {
                    $noty.close();

                }
            }]);

    },

    MakeUnrecognized: function (saleObj, type) {
        saleObj.Comments = $("#txtComments").val();
        var jsonParam = 'sale:' + JSON.stringify(saleObj);

        var serviceUrl = "../Sale/MakeUnRecognized/";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success', 'Unrecognized Successfully.',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            if (type == "draft") {
                                $("#gridSale").data("kendoGrid").dataSource.read();
                                $("#gridBookedSale").data("kendoGrid").dataSource.read();
                                gbUnrecogSaleObj = [];
                                type = "";
                            } else {
                                $("#divAddCommentsPopup").data("kendoWindow").close();
                                $("#gridBookedSale").data("kendoGrid").dataSource.read();
                                gbUnrecogSaleObj = [];
                                type = "";
                            }

                        }
                    }]);
            }
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    GetSaleInvoice: function () {

        var invoice = $("#txtInvoiceSale").val();
        if (invoice.length > 6) {
            invoice = invoice.substr(6);
        }
        var objSale = "";
        var jsonParam = "invoice=" + invoice;
        var serviceUrl = "../Sale/GetInvoice/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objSale = jsonData;
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objSale;
    },

    GetSaleObject: function () {

        var objSale = new Object();
        objSale.SaleId = $("#hdnSaleId").val();
        objSale.Invoice = $("#txtInvoiceSale").val();
        objSale.WarrantyStartDate = kendo.parseDate($('#cmbSaleDate').val(), "dd-MM-yyyy");
        objSale.FirstPayDate = $("#cmbPayDate").data("kendoDatePicker").value();
        objSale.SaleTypeId = $("#cmbSaleType").data().kendoComboBox.value();
        objSale.ChallanNo = $("#txtChallan").val();
        objSale.Price = $("#txtPrice").data("kendoNumericTextBox").value();
        objSale.DownPay = $("#txtDownPay").data("kendoNumericTextBox").value();
        objSale.Installment = $("#txtInstallment").val();
        objSale.NetPrice = $("#txtOutstanPrice").data("kendoNumericTextBox").value();
        objSale.ALicense = { Number: $('#txtLicenseProInfo') };
        objSale.IsActive = 0;

        var saleRepcombo = $("#cmbSaleRepresentator").data("kendoComboBox");
        if (saleRepcombo != undefined) {
            objSale.SalesRepId = $("#cmbSaleRepresentator").data("kendoComboBox").value();
        }
        objSale.ACustomer = {
            CustomerId: $("#hdnCustomerId").val(),
            Phone: $("#txtPhone").val(),
            BranchSmsMobileNumber: $("#txtBranchSMSMobileNumber").val(),
        };
        objSale.AProduct = {
            ModelId: $("#hdnModelIdProInfo").val(),
            ModelItemID: $("#hdnModelItemIdProInfo").val(),
        };

        return objSale;
    },

    ProductCustomerSaleInfoFill: function () {

        var invoiceNo = $('#txtInvoiceSale').val() == "" ? 0 : $('#txtInvoiceSale').val();
        productInfoManager.ProductCustomerInfoFill(invoiceNo);
    },

    GetInstallmentGridList: function () {
        var gridData = $("#gridInstalment").data("kendoGrid").dataSource.data();
        return gridData;
    },

    GetMotherSaleForEditSaleCombo: function (saleId) {
        var objSale = "";
        var jsonParam = "SaleId=" + saleId;
        var serviceUrl = "../Sale/GetMotherSaleForEditSaleCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objSale = jsonData;
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objSale;
    },

    GetDownPaymentCollectionInfo: function (saleId) {
        var objDownPaymentCollection = "";

        var jsonParam = "saleId=" + saleId;
        var serviceUrl = "../Sale/GetDownPaymentCollectionInfoBySaleId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            debugger;
            objDownPaymentCollection = jsonData;
            if (objDownPaymentCollection != null) {
                saleDetailsHelper.FillDownPaymentCollectionInfo(objDownPaymentCollection);
            }


        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objDownPaymentCollection;
    },

    CheckExistCustomerCode: function (customerCode) {

        var objCustomerCode = "";
        if (customerCode != "") {
            var jsonParam = "customerCode=" + customerCode;
            var serviceUrl = "../Customer/CheckExistCustomerByCode/";
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Enter Customer ID!',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                             $("#btnSaveAsDraft").show();
                             saleDetailsHelper.ClearAllSaleDetailsForm();
                         }
                     }]);
        }


        function onSuccess(jsonData) {

            objCustomerCode = jsonData;
            if (objCustomerCode != "") {// Juste un comment for Multiple Sale
                //AjaxManager.MsgBox('information', 'center', 'Warning:', 'Customer Code Already Exist.<br/> Are you sure want to search all information?',
                //    [{
                //        addClass: 'btn btn-primary',
                //        text: 'Ok',
                //        onClick: function ($noty) {
                //            $noty.close();

                //            var custCode = $("#txtCodeCustInfo").val();
                //            saleDetailsHelper.ClearAllSaleDetailsForm();
                //            //var branchId = CurrentUser.ChangedBranchId == 0 ? CurrentUser.BranchId : CurrentUser.ChangedBranchId;
                //            //custCode = branchId + custCode;
                //            var data = saleDetailsManager.GetCustomerAndSaleInfoByCustomerCode(custCode);
                //            if (data != null) {
                //                var invoiceNo = data.Invoice;
                //                var saleId = data.SaleId;
                //                if (invoiceNo != "") {
                //                    $("#txtInvoiceSale").val(invoiceNo);
                //                    $("#btnSaveAsDraft").hide();

                //                    ProductItemInfoHelper.GenerateProductItemInfoGrid(0);
                //                    productInfoManager.ProductCustomerInfoFill(invoiceNo, saleId);
                //                    saleDetailsManager.GetDownPaymentCollectionInfo(saleId);
                //                }
                //            }

                //        }
                //    },
                //        {
                //            addClass: 'btn',
                //            text: 'No',
                //            onClick: function ($noty) {
                //                $noty.close();
                //                $("#txtCodeCustInfo").focus();
                //                saleDetailsHelper.ClearForm();
                //            }
                //        }]);

                // New  Requirement 28/12/2015 Multiple Sale Not Allowed -- 

                AjaxManager.MsgBox('warning', 'center', 'Message:', 'Cutomer Already Exist! Not Allowed to Multiple Sale',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            saleDetailsHelper.ClearAllSaleDetailsForm();
                        }
                    }]);



            }
            else {
                var pid = $("#txtPidCustInfo").val();
                var nid = $("#txtNidCustInfo").val();
                var phone1 = $("#txtMobileNo1").val();

                if (pid == "" && nid == "" && phone1 == "") {
                    AjaxManager.MsgBox('warning', 'center', 'Message:', 'Customer Not Found. Please Enter Product Id or National Id or Phone No.!',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                // saleDetailsHelper.ClearForm();
                                //$("#txtNidCustInfo").focus();
                                $('#txtMobileNo2').focus();
                            }
                        }]);
                }
            }
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objCustomerCode;
    },

    GetCustomerAndSaleInfoByCustomerCode: function (customerCode) {
        var objsaleInfo = "";
        var jsonParam = "customerCode=" + customerCode;
        var serviceUrl = "../Sale/GetCustomerAndSaleInfoByCustomerCode/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objsaleInfo = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objsaleInfo;
    },

    SaveSaleForSpecialPackage: function () {

        var customerInfoObj = saleDetailsHelper.CreateCustomerInfoObject();
        var objCustomerInfo = JSON.stringify(customerInfoObj).replace(/&/g, "^");

        var saleObject = saleDetailsManager.GetSaleObject();
        //saleObject.State = state;
        var objSaleInfo = JSON.stringify(saleObject).replace(/&/g, "^");

        var objItemInformationList = ProductItemInfoHelper.GetProductItemInfoGridList();
        var objItemInfoList = JSON.stringify(objItemInformationList);

        var objItemDetailsInfoList = JSON.stringify(gbItemDetailsInfoArray);

        var objInstallmentList = saleDetailsManager.GetInstallmentGridList();
        var objInstallList = JSON.stringify(objInstallmentList);

        var downPaymentCollectinObj = saleDetailsHelper.DownPaymentCollectionObj();
        var objDownPayCollection = JSON.stringify(downPaymentCollectinObj);

        var dicountInfoObj = saleDetailsHelper.CreateDiscountInfoObject();
        var objDiscountInfo = JSON.stringify(dicountInfoObj).replace(/&/g, "^");

        var packageId = $("#cmbModelProInfo").data("kendoComboBox").value();
        if (packageId > 0) {
            //var phoneNo = $("#txtMobileNo1").val();
            var customerId = $("#txtCodeCustInfo").val();
            var saleRepId = $("#cmbSaleRepresentator").data("kendoComboBox").value();
            debugger;
            if (customerId != "" && saleRepId != "") {
                var jsonParam = 'strObjSaleInfo:' + objSaleInfo + ",objInstallmentList:" + objInstallList + ",objItemInfoList:" + objItemInfoList +
                    ",objItemDetailsInfoList:" + objItemDetailsInfoList + ",objDownPayCollection:" + objDownPayCollection + ",objCustomerInfo:" + objCustomerInfo + ",objDiscountInfo:" + objDiscountInfo;
                var serviceUrl = "../Sale/SaveSaleForSpecialPackage/";
                AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
            }
            else {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Sale Representator Or Customer ID cannot be empty!',
                     [{
                         addClass: 'btn btn-primary',
                         text: 'Ok',
                         onClick: function ($noty) {
                             $noty.close();
                         }
                     }]);
            }

        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select a Package !',
                      [{
                          addClass: 'btn btn-primary',
                          text: 'Ok',
                          onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                // saleDetailsHelper.ClearAllSaleDetailsForm();
                if (true) {
                    AjaxManager.MsgBox('success', 'center', 'Success', 'New Sale Saved Successfully.',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                saleDetailsHelper.ClearAllSaleDetailsForm();
                            }
                        }]);
                }
                $("#gridSale").data("kendoGrid").dataSource.read();
            }
            else if (jsonData == "Exists") {
                AjaxManager.MsgBox('warning', 'center', 'Exist:', 'Invoice No Already Exist!',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }]);
            } else if (jsonData == "EmptyInventoryInfo") {
                AjaxManager.MsgBox('warning', 'center', 'Exist:', 'Stock Is Not Avilable!',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }]);
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }]);
            }
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },


    GetProductModelbyTypeIdAndPackageId: function (typeId, packageType) {
        var objModel = "";
        var jsonParam = "typeId=" + typeId + "&packageType=" + packageType;
        //var serviceUrl = "../Product/GetAllProdeuctModel/";
        var serviceUrl = "../Product/GetAllPackageByTypeIdAndPackageId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objModel = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objModel;
    },


};



var saleDetailsHelper = {

    GenerateSaleDatePicker: function () {
        $("#cmbSaleDate").kendoDatePicker({
            format: "dd-MM-yyyy"
        });
        $("#cmbPayDate").kendoDatePicker({
            format: "dd-MM-yyyy"
        });
        var text = gbobjInterest.Interests + " %";
        $("#btnInterrestPercent").html(text);

        //var downPay = gbobjInterest.DownPay + " %";
        //$("#btndownPayPercent").html(downPay);

    },

    SetSaleDateCombo: function () {

        var saledatepicker = $("#cmbSaleDate").data("kendoDatePicker");
        saledatepicker.value(new Date());
        var today = new Date();
        var nextMonthDay = saleDetailsHelper.AddDaysToDate(today, 30);
        var firstdatepicker = $("#cmbPayDate").data("kendoDatePicker");
        firstdatepicker.value(nextMonthDay);
        var cmbSaleType = $("#cmbSaleType").data("kendoComboBox");
        cmbSaleType.value(1);
        //  $("#txtWarranty").data("kendoNumericTextBox").value(60);
        var invoice = saleDetailsManager.GetSaleInvoice();
        $("#txtInvoiceSale").val(invoice);
    },


    GenerateSaleTypeCombo: function () {
        var objSateType = saleDetailsManager.GetSaleType();
        $("#cmbSaleType").kendoComboBox({
            dataTextField: "Type",
            dataValueField: "TypeId",
            dataSource: objSateType,
            suggest: true,
        });
    },

    GenerateNumericTextBox: function () {

        $("#txtPrice").kendoNumericTextBox({
            min: 1,
            max: 10000000,
            format: "#",
            enable: false,
            change: function () {
                var saleObject = saleDetailsHelper.GetSalesObj();
                if (saleObject.modelId == "" && saleObject.customerCode == "") {
                    AjaxManager.MsgBox('warning', 'center', 'Notify', 'Please Select Customer and Product',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                return false;
                            }
                        }]);
                }
                else {
                    saleDetailsHelper.SetTotalDownPay(saleObject);
                }
            }
        });

        $("#txtInstallment").kendoNumericTextBox({
            min: 0,
            max: 60,

            format: "#",
            change: function () {

                var salesObject = saleDetailsHelper.GetSalesObj();
                if (salesObject.customerCode == "") {
                    AjaxManager.MsgBox('warning', 'center', 'Notify', 'Please Select Customer',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                return;
                            }
                        }]);
                }

                ProductItemInfoHelper.CalculateNetTotalPrice(salesObject);
                saleDetailsHelper.InstallmentData(salesObject);
            }
        });
        $("#txtTotalPrice").kendoNumericTextBox({
            min: 0,
            max: 10000000,
            format: "#",
        });

        //$("#txtWarranty").kendoNumericTextBox({
        //    min: 0,
        //    max: 120,
        //    format: "#",
        //});
        $("#txtDownPay").kendoNumericTextBox({
            min: 0,
            max: 10000000,
            format: "#",

        });
        $("#txtOutstanPrice").kendoNumericTextBox({
            min: 0,
            max: 10000000,
            format: "#",
        });
    },

    GetSalesObj: function () {

        var saleObj = new Object();
        saleObj.price = $("#txtPrice").data("kendoNumericTextBox").value();
        // saleObj.NetPrice = $("#txtOutstanPrice").val();
        saleObj.firstPayDate = $("#cmbPayDate").data().kendoDatePicker.value();
        saleObj.saleMode = $("#cmbSaleType").data().kendoComboBox.value();
        saleObj.downPayForCollectedAmount = $("#txtDownPayment").val();
        saleObj.downPay = $("#txtDownPay").val();

        //saleObj.productNo = $('#txtCodeProInfo').val();
        saleObj.modelId = $('#hdnModelIdStock').val();
        saleObj.customerCode = $('#txtCodeCustInfo').val();
        saleObj.saleInvoice = $('#txtInvoiceSale').val();
        saleObj.insNumber = $("#txtInstallment").val();
        saleObj.CollectedAmount = $("#txtCollectedAmount").val();

        return saleObj;
    },

    DownPaymentCollectionObj: function () {
        var obj = new Object();
        obj.CollectionId = $("#hdnDownpaymentCollectionId").val();
        obj.SaleInvoice = $("#txtInvoiceSale").val();
        obj.DownPay = $("#txtDownPayment").val();
        obj.PayDate = $("#txtCollectionDate").data("kendoDatePicker").value();
        obj.ReceiveAmount = $("#txtCollectedAmount").val();
        obj.PaymentType = 1;//cash;
        return obj;
    },

    CreateCustomerInfoObject: function () {
        var objCustomer = new Object();
        objCustomer.CustomerId = $("#hdnCustomerId").val();
        objCustomer.CustomerCode = $("#txtCodeCustInfo").val();
        objCustomer.Name = $("#txtNameCustInfo").val();
        objCustomer.Phone = $("#txtMobileNo1").val();
        objCustomer.Phone2 = $("#txtMobileNo2").val();
        objCustomer.Nid = $("#txtNidCustInfo").val();
        objCustomer.IsActive = 1;
        objCustomer.IsStaff = $("#chkStaff").is(":checked") == true ? 1 : 0;

        if (objCustomer.IsStaff > 0) {
            objCustomer.StaffId = $("#cmbStaffId").data("kendoComboBox").value();
        }
        objCustomer.ReferenceId = $("#txtReferenceId").val();
        objCustomer.ProductId = $("#txtPidCustInfo").val();
        return objCustomer;
    },

    CreateDiscountInfoObject: function () {
        var obj = new Object();
        obj.DiscountId = $("#hdnDiscountId").val();
        obj.DiscountOptionId = $("#cmbDiscountOption").data("kendoComboBox").value();
        obj.DiscountTypeCode = $("#cmbDiscountType").data("kendoComboBox").value();
        obj.DiscountAmount = $("#txtDiscountAmount").val();
        obj.IsApprovedSpecialDiscount = $("#hdnIsApproveSpecialDiscount").val();
        return obj;
    },

    InstallmentData: function (saleObj) {
        var insGrid = $("#gridInstalment").data("kendoGrid");
        var firstPayDate1 = $("#cmbSaleDate").data('kendoDatePicker').value();
        var firstPayDate = new Date(firstPayDate1);
        if (saleObj.saleMode == 1) {
            var installmentGridData = [];
            var installment = saleDetailsHelper.GetInstallment();

            if (installment >= 1) {

                var date2;
                for (var i = 0; i < saleObj.insNumber; i++) {

                    date2 = saleDetailsHelper.AddDaysToDate(firstPayDate, 30);
                    var obj = new Object();
                    obj.ProductNo = saleObj.productNo;
                    obj.Number = i + 1;
                    obj.SInvoice = saleObj.saleInvoice;

                    obj.Amount = parseFloat(installment).toFixed(2);//it was math.Round
                    obj.DueAmount = 0;
                    obj.ReceiveAmount = 0;
                    obj.DueDate = date2;
                    obj.Status = 0;
                    firstPayDate = new Date(date2);
                    installmentGridData.push(obj);
                }
            }

            var gridDataSource = new kendo.data.DataSource({ data: installmentGridData });
            insGrid.setDataSource(gridDataSource);

        } else {
            var gridDataEmptySource = new kendo.data.DataSource({ data: [] });
            insGrid.setDataSource(gridDataEmptySource);
            return false;
        }
        return false;
    },

    GetInstallment: function () {

        var installment = 0;
        var installmentNo = $("#txtInstallment").val();
        var netPrice = $("#txtOutstanPrice").val();

        if (netPrice > 0 && installmentNo > 0) {

            installment = netPrice / installmentNo;
            return installment;
        }
        return installment;
    },

    AddDaysToDate: function (selectedDate, days) {

        var x = selectedDate.getDate();
        var m1 = selectedDate.getMonth();
        selectedDate.setDate(selectedDate.getDate() + days);
        var m2 = selectedDate.getMonth();
        if (m1 == m2) {
            selectedDate.setDate(selectedDate.getDate() + 1);
        }
        return new Date(selectedDate);
    },



    clearSaleForm: function () {
        $('#hdnProductId').val("");
        $("#txtInvoice").val("");
        $("#cmbSaleDate").data("kendoDatePicker").value("");
        $("#cmbSaleType").data("kendoComboBox").value("");
        $("#txtChallan").val("");
        $("#txtPrice").data("kendoNumericTextBox").value("");
        $("#txtInstallment").val("");
        $("#hdnFlag").val(0);
        $("#cmbPayDate").data("kendoDatePicker").value("");
    },

    SetTotalDownPay: function (gbSaleObj) {

        $("#txtInstallment").data("kendoNumericTextBox").value(0);
        $("#txtTotalPrice").data("kendoNumericTextBox").value(0);
        $("#txtOutstanPrice").data("kendoNumericTextBox").value(0);

        if (gbSaleObj.price != 0 && gbSaleObj.saleMode == 1) {
            //var downPayPercent = gbobjInterest.DownPay;
            var downPayPercent = gbDownPayPercentage;
            gbSaleObj.downPay = ((parseFloat(gbSaleObj.price) * parseFloat(downPayPercent)) / 100);
            $("#txtDownPay").data("kendoNumericTextBox").value(gbSaleObj.downPay);
        } else {

        }
    },




    validator: function () {
        var validator = $("#SaleDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }

    },


    GetMotherSaleForEditSaleCombo: function (saleId) {
        var objSale = saleDetailsManager.GetMotherSaleForEditSaleCombo(saleId);

        $("#cmbSaleType").kendoComboBox({
            placeholder: "Select Sale...",
            dataTextField: "SaleType",
            dataValueField: "SaleId",
            dataSource: objSale,
            suggest: true,
            change: function () {
                var value = this.value();
                AjaxManager.isValidItem("cmbSaleType");
            }
        });

    },

    ClearAllSaleDetailsForm: function () {
        if (gbIsViewer != 1) {
            $("#btnSaveAsDraft").show();
        }

        $("#hdnSaleId").val(0);

        $("#txtInvoice").val("");
        $("#txtInvoiceSale").val("");
        $("#cmbSaleDate").data("kendoDatePicker").value("");
        $("#cmbPayDate").data("kendoDatePicker").value("");
        $("#cmbSaleType").data("kendoComboBox").value("");

        $("#txtInstallment").data("kendoNumericTextBox").value("");
        $("#txtPrice").data("kendoNumericTextBox").value("");
        $("#txtTotalPrice").data("kendoNumericTextBox").value("");
        $("#txtDownPay").data("kendoNumericTextBox").value("");
        $("#txtOutstanPrice").data("kendoNumericTextBox").value("");

        $("#txtChallan").val("");
        $("#hdnFlag").val(0);
        $('#hdnModelIdProInfo').val('');
        $('#txtNameProInfo').val('');
        $('#txtLicenseProInfo').val('');
        $("#txtLicenseTypeProInfo").val('');
        $("#cmbModelProInfo").data("kendoComboBox").value("");

        $("#hdnCustomerId").val('');
        $('#txtCodeCustInfo').val('');

        $('#txtPhone').val('');
        $('#txtPhone2').val('');

        $('#txtNameCustInfo').val('');
        $("#txtNidCustInfo").val('');
        $("#gridInstalment").data("kendoGrid").dataSource.data([]);

        var grid = $("#gridProductItemsInfo").data("kendoGrid");

        if (grid != undefined) {
            $("#gridProductItemsInfo").data("kendoGrid").dataSource.data([]);
        }

        $("#txtDownPayment").val("");
        $("#txtCollectionDate").data("kendoDatePicker").value("");
        $("#txtCollectedAmount").val("");

        $("#chkStaff").removeProp('checked', 'checked');
        var staffCombo = $("#cmbStaffId").data("kendoComboBox");
        if (staffCombo != undefined) {
            $("#cmbStaffId").data("kendoComboBox").value("");
        }
        $("#cmbSaleRepresentator").data("kendoComboBox").value("");
        $("#txtRepresentatorType").val("");
        $("#txtRepresentatorCode").val("");

        $("#hdnDiscountId").val(0);
        $("#cmbDiscountOption").data("kendoComboBox").value("");
        $("#cmbDiscountType").data("kendoComboBox").value("");
        $("#txtDiscountAmount").val("");
        $("#txtMobileNo1").val("");
        $("#txtMobileNo2").val("");
        $("#txtReferenceId").val("");
        $("#txtPackagePrice").html("");

        var bookgrid = $("#gridBookedSale").data("kendoGrid");
        if (bookgrid != undefined) {

            $("#gridBookedSale tbody input:checkbox").removeAttr("checked", this.checked);
            $("#gridBookedSale tbody tr input:checkbox").removeAttr("checked", true);
            $("#gridBookedSale table tr").removeClass('k-state-selected');
            $("#gridBookedSale").data("kendoGrid").dataSource.data([]);
        }

        var draftgrid = $("#gridSale").data("kendoGrid");
        if (draftgrid != undefined) {
            $("#gridSale tbody input:checkbox").removeAttr("checked", this.checked);
            $("#gridSale tbody tr input:checkbox").removeAttr("checked", true);
            $("#gridSale table tr").removeClass('k-state-selected');

            $("#gridSale").data("kendoGrid").dataSource.data([]);
        }

        $("#cmbPackageType").data("kendoComboBox").value("");
        $("#cmbModelProInfo").data("kendoComboBox").value("");
        $("#cmbModelProInfo").data("kendoComboBox").setDataSource();
        $("#hdnIsApproveSpecialDiscount").val("");

        $("#txtMobileNo1").removeAttr("disabled");
        $("#txtPidCustInfo").val("");
        $("#cmbType").data("kendoComboBox").value("");

        $("#btnSaveAsBook").show();
        $("#btnPreviewSales").show();
        $("#btnPreviewSaveAsDraft").show();
        $("#btnFinalSubmitPrepaid").hide();
    },

    ClearSalesDetailsForCash: function () {
        $("#txtInstallment").data("kendoNumericTextBox").value("");
        $("#txtDownPay").data("kendoNumericTextBox").value("");
        //  $("#txtOutstanPrice").data("kendoNumericTextBox").value("");

        $("#txtCollectedAmount").val("");
        $("#txtDownPayment").val("");
    },

    ClearSalesDetailsFormForChangeModel: function () {
        $("#txtInstallment").data("kendoNumericTextBox").value("");
        $("#txtPrice").data("kendoNumericTextBox").value("");
        $("#txtTotalPrice").data("kendoNumericTextBox").value("");
        $("#txtDownPay").data("kendoNumericTextBox").value("");
        $("#txtOutstanPrice").data("kendoNumericTextBox").value("");

        //if (grid != undefined) {
        //    $("#gridProductItemsInfo").data("kendoGrid").dataSource.data([]);
        //}

        $("#gridInstalment").data("kendoGrid").dataSource.data([]);
    },


    ChangeEventForSaleDate: function () {

        var salesdatepicker = $("#cmbSaleDate").data("kendoDatePicker").value();
        var saledatepicker = new Date(salesdatepicker);
        var addedDate = saleDetailsHelper.AddDaysToDate(saledatepicker, 30);
        var firstdatepicker = $("#cmbPayDate").data("kendoDatePicker");
        firstdatepicker.value(addedDate);
        var saleObj = saleDetailsHelper.GetSalesObj();
        saleDetailsHelper.InstallmentData(saleObj);
    },

    FillDownPaymentCollectionInfo: function (data) {
        debugger;
        $("#hdnDownpaymentCollectionId").val(data.CollectionId);
        $("#txtDownPayment").val(data.DownPay);
        $("#txtCollectionDate").data("kendoDatePicker").value(data.PayDate);
        $("#txtCollectedAmount").val(data.ReceiveAmount);
    },


    ClearForm: function () {
        $("#hdnSaleId").val(0);

        $('#hdnProductId').val("");
        $("#txtInvoice").val("");
        $("#txtInvoiceSale").val("");

        $("#cmbSaleDate").data("kendoDatePicker").value("");
        $("#cmbPayDate").data("kendoDatePicker").value("");
        $("#cmbSaleType").data("kendoComboBox").value("");
        // $("#txtWarranty").data("kendoNumericTextBox").value("");
        $("#txtInstallment").data("kendoNumericTextBox").value("");
        $("#txtPrice").data("kendoNumericTextBox").value("");
        $("#txtTotalPrice").data("kendoNumericTextBox").value("");
        $("#txtDownPay").data("kendoNumericTextBox").value("");
        $("#txtOutstanPrice").data("kendoNumericTextBox").value("");

        $("#txtChallan").val("");
        $("#hdnFlag").val(0);
        $('#hdnModelIdProInfo').val('');
        $('#hdnModelItemIdProInfo').val('');
        $('#txtNameProInfo').val('');
        $('#txtLicenseProInfo').val('');
        $("#txtLicenseTypeProInfo").val('');
        $("#cmbModelProInfo").data("kendoComboBox").value("");

        $("#hdnCustomerId").val('');
        // $('#txtCodeCustInfo').val('');

        //$('#txtPhoneCustInfo').val('');

        $('#txtNameCustInfo').val('');
        $("#txtNidCustInfo").val('');
        $("#gridInstalment").data("kendoGrid").dataSource.data([]);


        var grid = $("#gridProductItemsInfo").data("kendoGrid");

        if (grid != undefined) {
            $("#gridProductItemsInfo").data("kendoGrid").dataSource.data([]);
        }

        //downpaymentcollection field
        $("#txtDownPayment").val("");
        $("#txtCollectionDate").data("kendoDatePicker").value("");
        $("#txtCollectedAmount").val("");

        $("#chkStaff").removeProp('checked', 'checked');
        var staffCombo = $("#cmbStaffId").data("kendoComboBox");
        if (staffCombo != undefined) {
            $("#cmbStaffId").data("kendoComboBox").value("");
        }
        $("#cmbSaleRepresentator").data("kendoComboBox").value("");
        $("#txtRepresentatorType").val("");
        $("#txtRepresentatorCode").val("");

        $("#hdnDiscountId").val(0);
        $("#cmbDiscountOption").data("kendoComboBox").value("");
        $("#cmbDiscountType").data("kendoComboBox").value("");
        $("#txtDiscountAmount").val("");

    },



    AddMonthToDate: function (selectedDate, month) {
        var date = new Date(selectedDate);
        var currentDate = selectedDate;// date.getDate();
        date.setDate(month);
        date.setMonth(date.getMonth() + month);
        var daysInMonth = new Date(date.getYear(), date.getMonth() + month, 0).getDate();
        date.setDate(Math.min(currentDate, daysInMonth));
        return date;
    },

    ChangeEventForSaleType: function () {

        $("#cmbSaleType").change(function () {

            var saleType = $("#cmbSaleType").val();
            if (saleType == 2) {
                $("#txtInstallment").data("kendoNumericTextBox").enable(false);

                var newsaleObj = saleDetailsHelper.DiscountCalculationForCashSale();

                $("#txtPrice").data("kendoNumericTextBox").value(newsaleObj.price);
                $("#txtTotalPrice").data("kendoNumericTextBox").value(newsaleObj.price);
                $("#txtOutstanPrice").data("kendoNumericTextBox").value(newsaleObj.price);
                saleDetailsHelper.ClearSalesDetailsForCash();
                $("#gridInstalment").data("kendoGrid").dataSource.data([]);
                $("#divInstallmentGridForSale").hide();

                //$("#downPaymentCollectionInfoDiv").hide();


            } else {

                // saleDetailsHelper.DiscountCalculationForInstallmentSale();


                $("#txtInstallment").data("kendoNumericTextBox").enable(true);
                $("#txtTotalPrice").data("kendoNumericTextBox").value("");
                // $("#divInstallmentGridForSale").show();

                $("#downPaymentCollectionInfoDiv").show();

                saleDetailsManager.GetDefaultInstallmentNo();
                var saleObj = saleDetailsHelper.GetSalesObj();
                saleObj.CollectedAmount = saleObj.downPayForCollectedAmount;
                ProductItemInfoHelper.CalculateNetTotalPrice(saleObj);

                saleDetailsHelper.InstallmentData(saleObj);


                $("#cmbDiscountOption").data("kendoComboBox").value("");
                $("#cmbDiscountType").data("kendoComboBox").value("");

                $("#txtDiscountAmount").val("");



            }
        });


    },

    DiscountCalculationForCashSale: function () {
        var saleobj = new Object();

        $("#cmbDiscountOption").data("kendoComboBox").value(1);
        $("#cmbDiscountType").data("kendoComboBox").value("02");
        $("#txtDiscountAmount").val("Need Approval");

        var discountOption = $("#cmbDiscountOption").val();
        var discountType = $("#cmbDiscountType").val();
        // var packagePrice = $("#txtPackagePrice").html();
        var packagePrice = $("#txtPrice").val();
        var discountAmount = $("#txtDiscountAmount").val();
        if (discountOption == 1) {
            if (discountType == "01") {

                saleobj.price = (packagePrice - discountAmount);
            } else {
                saleobj.price = packagePrice;
            }
        }
        else if (discountOption == 2) {
            if (discountType == "01") {
                var discountAmt = (packagePrice * discountAmount) / 100;
                saleobj.price = (packagePrice - discountAmt);
            } else {
                saleobj.price = packagePrice;
            }
        }

        return saleobj;
    },

    DiscountCalculationForInstallmentSale: function () {
        $("#cmbDiscountOption").data("kendoComboBox").value(1);
        $("#cmbDiscountType").data("kendoComboBox").value("01");
        var discountInfo = empressCommonManager.GetDiscountAmountByType();
        $("#txtDiscountAmount").val(discountInfo.DefaultCashDiscount);

        var packagePrice = $("#txtPackagePrice").html();
        var salePrice = (packagePrice - discountInfo.DefaultCashDiscount);
        $("#txtPrice").data("kendoNumericTextBox").value(salePrice);
    },

    GenerateProductCombo: function () {

        $("#cmbType").kendoComboBox({
            placeholder: "Select Product...",
            dataValueField: "TypeId",
            dataTextField: "Type",
            //dataSource: objProduct,
            filter: "contains",
            suggest: true,
            change: function () {

                var typeId = this.value();
                var packageType = $("#cmbPackageType").data("kendoComboBox").value();
                var data = saleDetailsManager.GetProductModelbyTypeIdAndPackageId(typeId, packageType);
                $("#cmbModelProInfo").data("kendoComboBox").value("");
                productInfoHelper.GenerateModelCombo(data);

                if (typeId == 3) {
                    //New Code for Package Type D-Type (Prepaid) = 3 & R-Type (Postpaid) = 4
                    $("#btnSaveAsBook").hide();
                    $("#btnPreviewSales").hide();
                    $("#btnPreviewSaveAsDraft").hide();
                    $("#btnFinalSubmitPrepaid").show();

                } else {
                    $("#btnSaveAsBook").show();
                    $("#btnPreviewSales").show();
                    $("#btnPreviewSaveAsDraft").show();
                    $("#btnFinalSubmitPrepaid").hide();
                }
            }
        });
    },

};

