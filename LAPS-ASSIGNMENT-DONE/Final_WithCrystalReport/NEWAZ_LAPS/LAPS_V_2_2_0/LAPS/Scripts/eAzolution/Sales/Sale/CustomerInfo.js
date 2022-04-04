var customerInfoManager = {

    InitCustomerInfoDetails: function () {
        // $('#txtCodeCustInfo').mask('9999999999999');
        // $('#txtNidCustInfo').mask('9999999999999');
        
       // $('#txtMobileNo1').mask('99999999999');
       // $("#txtMobileNo2").mask('99999999999');


        customerInfoHelper.CustomerInfoEvent();

    },

    CheckExistMobileNumber: function (mobileno, noOfMobile) {
        var res = "";
        var jsonParam = "mobileNo=" + mobileno + "&noOfMobile=" + noOfMobile;
        var serviceUrl = "../Customer/GetExistMobileNo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            res = jsonData;
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return res;
    },


    //----------------Search Customer info------------------
    CustomerInfoFill: function (customerCode, phoneNo) {

        if (customerCode != "_____________") {
            var jsonParam = "customerCode=" + customerCode + "&phoneNo=" + phoneNo;
            var serviceUrl = "../Customer/GetCustomerByCustomerCode/";
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData != null) {
                customerInfoHelper.FillCustomerInfoInForm(jsonData);
            }
            else {
                AjaxManager.MsgBox('warning', 'center', 'Warning', 'Data Not Found!!!',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                             customerInfoHelper.clearCustomerForm();
                         }
                     }]);
            }

        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
    },

    getProductInformationByCustomer: function (customerId) {
        if (customerId != "") {
            var jsonParam = "customerId=" + customerId;
            var serviceUrl = "../Customer/GetProductInfoByCustomerId/";
            AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        }
        function onSuccess(jsonData) {
            if (jsonData != null) {

                AjaxManager.MsgBox('warning', 'center', 'Warning', 'You have already Sold a Model in this Customer!!',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                             customerInfoHelper.clearCustomerForm();
                         }
                     }]);


                // customerInfoHelper.fillProductInfoByCustomer(jsonData);
            }
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
    }


};



var customerInfoHelper = {

    FillCustomerInfoInForm: function (objCustomer) {

        customerInfoHelper.clearCustomerForm();

        $("#hdnCustomerId").val(objCustomer.CustomerId);
        $('#txtCodeCustInfo').val(objCustomer.CustomerCode);
        $('#txtMobileNo1').val(objCustomer.Phone);

        $('#txtNameCustInfo').val(objCustomer.Name);
        $("#txtNidCustInfo").val(objCustomer.NId);

        $("#hdnCompanyCode").val(objCustomer.CompanyCode);
        //customerInfoHelper.fillProductInfoByCustomer(objCustomer.CustomerId);

        //----------------Fill Product and Product Item Fields ---------------
        customerInfoManager.getProductInformationByCustomer(objCustomer.CustomerId);
    },
    fillProductInfoByCustomer: function (productObj) {

        $("#cmbModelProInfo").data('kendoComboBox').value(productObj.AProduct.ModelId);
        $("#txtNameProInfo").val(productObj.AProduct.ProductName);
        $("#txtLicenseProInfo").val(productObj.ALicense.Number);
        $("#txtLicenseTypeProInfo").val(productObj.ALicense.LType);

        // customerInfoHelper.generateProductItemInfoGridById(productObj.AProduct.ModelId);
    },

    clearCustomerForm: function () {
        $("#hdnCustomerId").val('');
        $('#txtCodeCustInfo').val('');
        $('#txtMobileNo1').val('');
        $('#txtNameCustInfo').val('');
        $("#txtNidCustInfo").val('');
        $("#txtMobileNo1").removeAttr("disabled");
    },

    //CustomerDetailsPopUpWindow: function () {
    //    $("#CustomerDetailsPopUpWindow").kendoWindow({
    //        title: "Collection Details",
    //        resizeable: false,
    //        width: "60%",
    //        actions: ["Pin", "Refresh", "Maximize", "Close"],
    //        modal: true,
    //        visible: false,
    //    });
    //},

    populateStaffcombo: function (salesRepType) {

        $("#cmbStaffId").kendoComboBox({
            placeholder: "Select Staff ID",
            dataTextField: "SalesRepId",
            dataValueField: "SalesRepId",
            dataSource: empressCommonManager.GetAllSalesRepresentatorCombo(salesRepType),

            suggest: true,

        });
    },


    //=============================================================
    CustomerInfoEvent: function () {

        $("#txtCodeCustInfo").change(function () {
            var customerCode = $("#txtCodeCustInfo").val();
            saleDetailsManager.CheckExistCustomerCode(customerCode);
        });

        //$("#btnSearchCustInfo").click(function () {
        //    var customerCode = $("#txtCodeCustInfo").val();
        //    saleDetailsManager.CheckExistCustomerCode(customerCode);
        //});

        $('#txtPidCustInfo').change(function () {

            var productId = $('#txtPidCustInfo').val();
            if (productId != "") {
                $("#txtCodeCustInfo").val(productId);
                $("#txtNidCustInfo").val("");
                $("#txtNidCustInfo").attr("disabled", "disabled");
                $("#txtMobileNo1").val("");
                $("#txtMobileNo1").attr("disabled", "disabled");
                saleDetailsManager.CheckExistCustomerCode(productId);
            } else {
                $("#txtNidCustInfo").removeAttr("disabled");
                $("#txtMobileNo1").removeAttr("disabled");
            }
        });

        $('#txtNidCustInfo').change(function () {

            var nationalId = $('#txtNidCustInfo').val();
            if (nationalId != "") {
                $("#txtCodeCustInfo").val(nationalId);
                $("#txtMobileNo1").val("");
                $("#txtMobileNo1").attr("disabled", "disabled");

                saleDetailsManager.CheckExistCustomerCode(nationalId);
            } else {
                $("#txtMobileNo1").removeAttr("disabled");
            }
        });


        $('#txtMobileNo1').blur(function () {
            var mobileNo1 = $('#txtMobileNo1').val();
            var mob2 = $('#txtMobileNo2').val();
            var nationalId = $('#txtNidCustInfo').val();
            if (nationalId == "") {
                $("#txtCodeCustInfo").val(mobileNo1);
                if (mob2 == "") {
                    $('#txtMobileNo2').val(mobileNo1);
                    customerInfoHelper.GenerateReferenceId(mobileNo1);
                }
                saleDetailsManager.CheckExistCustomerCode(mobileNo1);
            } else {
                var res = customerInfoManager.CheckExistMobileNumber(mobileNo1, 1);

                if (res) {
                    AjaxManager.MsgBox('warning', 'center', 'Message:', 'Mobile No-1 Already Exist!',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();

                         }
                     }]);
                } else {
                    $('#txtMobileNo2').val(mobileNo1);
                    $('#txtMobileNo2').focus();
                }
            }

        });

        $('#txtMobileNo2').blur(function () {

            var mobileNo2 = $('#txtMobileNo2').val();
            var res = customerInfoManager.CheckExistMobileNumber(mobileNo2, 2);
            if (res) {
                AjaxManager.MsgBox('warning', 'center', 'Message:', 'Mobile No-2 Already Exist!',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();

                        }
                    }]);
            } else {
                customerInfoHelper.GenerateReferenceId(mobileNo2);
            }
        });



        $('#chkStaff').live('click', function (e) {
            var $cb = $(this);
            if ($cb.is(":checked")) {
                $("#staffcomboli").show();
                customerInfoHelper.populateStaffcombo(3);//3 mean branch staff
            } else {
                $("#staffcomboli").hide();
            }

        });

    },
    GenerateReferenceId: function (mobileNo2) {

       // var branchCode = CurrentUser.ChangedBranchCode == null ? CurrentUser.BranchCode : CurrentUser.ChangedBranchCode;
        //if (mobileNo2 != "" && branchCode != "") {
        //    var referenceId = branchCode.substr(branchCode.length - 4) + mobileNo2.substr(mobileNo2.length - 4);
        //    $("#txtReferenceId").val(referenceId);
        //}
        
        if (mobileNo2 != "") {
            var referenceId = mobileNo2;
            $("#txtReferenceId").val(referenceId);
        }

       
    }

};