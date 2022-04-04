

var gbCustomerName = "";
var customerDetailsManager = {

    InitCustomerDetails: function () {

        customerDetailsHelper.populateGenderCombo();
        customerDetailsHelper.GenerateDateOfBirth();
        customerDetailsHelper.CustomerTypeCombo();
        customerDetailsHelper.DistrictCombo();

        empressCommonHelper.GenerareHierarchyCompanyCombo("cmbCompanyName");
        empressCommonHelper.GenerateBranchCombo(0, "cmbBranchName");

        $("#cmbCompanyName").change(function () {
            customerDetailsManager.CompanyChange();
        });

        $("#cmbBranchName").change(function () {
            customerDetailsHelper.changeEventForBranch();
        });

        customerDetailsHelper.CustomerDetailsEvent();


    },
    CompanyChange: function () {

        var companyNames = $("#cmbCompanyName");
        companyNames.change(function () {
            var comboboxbranch = $("#cmbBranchName").data("kendoComboBox");
            var companyData = $("#cmbCompanyName").data("kendoComboBox");
            var companyId = companyData.value();
            var companyName = companyData.text();

            if (companyId == companyName) {
                companyData.value('');
                comboboxbranch.value('');
                comboboxbranch.destroy();
                empressCommonHelper.GenerateBranchCombo(0, "cmbBranchName");
                return false;
            }
            if (comboboxbranch != undefined) {
                comboboxbranch.value('');
                comboboxbranch.destroy();
            }

            empressCommonHelper.GenerateBranchCombo(companyId, "cmbBranchName");
            customerDetailsHelper.changeEventForBranch();
            return false;
        });
    },

    CustomerCodeChange: function (codeOrNationalIdOrPhone, customerId, ctrlId) {
        if (codeOrNationalIdOrPhone != "") {

            var controlId = JSON.stringify(ctrlId);
            //var objcustomerCode = JSON.stringify(customerCode);
            var jsonParam = 'codeOrNationalIdOrPhone:' + JSON.stringify(codeOrNationalIdOrPhone) + ',customerId:' + customerId + ',ctrlId:' + controlId;
            var serviceUrl = "../Customer/GetCustomerResult/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData == "Exists") {
                if (ctrlId == "NationalId") {
                    AjaxManager.MsgBox('warning', 'center', 'Already Exist:', "NationalID Already Exists!!!",
                   [{
                       addClass: 'btn btn-primary',
                       text: 'Ok',
                       onClick: function ($noty) {
                           $noty.close();
                           $("#txtNationalId").val('');
                           var smsMobileNumber = $("#txtPhone").val();
                           if (smsMobileNumber != "") {
                               $("#txtCustomerCode").val("88" + smsMobileNumber);
                           } else {
                               $("#txtCustomerCode").val(new Date().getTime());
                           }

                       }
                   }]);
                }

                else if (ctrlId == "SMSMobileNumber") {
                    AjaxManager.MsgBox('warning', 'center', 'Already Exist:', "Mobile No Already Exists!!!",
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            $("#txtPhone").val('');
                            var nationalId = $("#txtNationalId").val();
                            if (nationalId != "") {
                                $("#txtCustomerCode").val(nationalId);
                            } else {
                                $("#txtCustomerCode").val(new Date().getTime());
                            }
                        }
                    }]);
                }
                else if (ctrlId == "CustomerCode") {
                    AjaxManager.MsgBox('warning', 'center', 'Already Exist:', "Customer Code Already Exists!!!",
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            $("#txtCustomerCode").val(new Date().getTime());

                        }
                    }]);
                }
            }

            else if (jsonData == "Success") {
                $("#cmbGender").data("kendoComboBox").value(1);
                $('#chkIsActive').attr('checked', 'checked');
            }
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    SaveCustomer: function () {
        var isToUpdateOrCreate = $("#hdnCustomerId").val();
        if (customerDetailsHelper.validator()) {
          
            var objCustomer = customerDetailsHelper.GetDataFromCustomerDetailsObject();
            if (objCustomer.CustomerCode != "" && objCustomer.Phone2 != "") {
                var objCustomerInfo = JSON.stringify(objCustomer).replace(/&/g, "^");
                var jsonParam = 'strObjCustomerInfo:' + objCustomerInfo;
                var serviceUrl = "../Customer/SaveCustomer/";
                AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
            } else {
                AjaxManager.MsgBox('warning', 'center', 'Minimum Requirement:', 'Please Enter Customer Code And Mobile No 2.',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            return false;
                        }
                    }]);
            }
        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                customerDetailsHelper.ClearCustomerForm();
                if (isToUpdateOrCreate == 0) {
                    AjaxManager.MsgBox('success', 'center', 'Success', 'New Customer Saved Successfully.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                              //$("#CustomerDetailsPopUpWindow").data("kendoWindow").close().center();

                              if (grid != undefined) {
                                  $("#gridCustomer").data("kendoGrid").dataSource.read();
                              }
                          }
                      }]);
                } else {
                    AjaxManager.MsgBox('success', 'center', 'Update', 'Customer Information Updated Successfully.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
                }
                $("#gridCustomer").data("kendoGrid").dataSource.read();
            }
            else if (jsonData == "Exists") {


                AjaxManager.MsgBox('warning', 'center', 'Alresady Exist:', 'Customer Code already exist.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
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
    },
    GetCustomerType: function () {
        var objCustomer = "";
        var jsonParam = "";
        var serviceUrl = "../Customer/GetCustomerType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCustomer = jsonData;
        }

        //function onFailed(error) {
        //    window.alert(error.statusText);
        //}
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objCustomer;
    },
    GetMotherCustomerForEditCustomerCombo: function (customerId) {
        var objCustomer = "";
        var jsonParam = "CustomerId=" + customerId;
        var serviceUrl = "../Customer/GetMotherCustomerForEditCustomerCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objCustomer = jsonData;
        }

        //function onFailed(error) {
        //    window.alert(error.statusText);
        //}
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objCustomer;
    },

    getSmsMobileNumberByBranchId: function (branchId) {
        var smsMobileNumber = "";
        var jsonParam = "branchId=" + branchId;
        var serviceUrl = "../Customer/GetSmsMobileNumberByBranchId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {

            smsMobileNumber = jsonData.BranchSmsMobileNumber;
        }

        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return smsMobileNumber;
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

                         }
                     }]);
        }

        function onSuccess(jsonData) {

            objCustomerCode = jsonData;
            if (objCustomerCode != "") {
                AjaxManager.MsgBox('information', 'center', 'Warning:', 'Customer Code Already Exist.',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();

                        }
                    }
                    ]);

            }
            else {
                    AjaxManager.MsgBox('warning', 'center', 'Message:', 'Customer Not Found. Please Enter National Id or Phone No.!',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                          // $("#txtNidCustInfo").focus();

                      }
                  }]);
                


            }
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objCustomerCode;
    },
};



var customerDetailsHelper = {
    fillCustomerCodeAfterExists: function (customerCode) {

        var nationalId = $('#txtNidCustInfo').val();
        var smsMobileNumber = $('#txtMobileNo2').val();
        if (nationalId == customerCode && smsMobileNumber != "") {
            //$("#txtCustomerCode").val("88" + smsMobileNumber);
            //customerDetailsHelper.chnageEventForCustomerCode();

            $.when($("#txtCustomerCode").val("88" + smsMobileNumber)).done(function () {
                customerDetailsHelper.chnageEventForCustomerCode();
            });
        }
        else if (smsMobileNumber == customerCode) {
            //$("#txtCustomerCode").val(nationalId);
            //customerDetailsHelper.chnageEventForCustomerCode();
        } else {
            $("#txtCustomerCode").val(new Date().getTime());
            customerDetailsHelper.chnageEventForCustomerCode();
        }
    },

    validator: function () {
        var validator = $("#CustomerDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }

    },

    GetDataFromCustomerDetailsObject: function () {
       
        var objCustomer = new Object();
        objCustomer.CustomerId = $("#hdnCustomerId").val();
        objCustomer.PrantId = $("#hdnPrantId").val();
        objCustomer.Flag = $("#hdnFlag").val();
        objCustomer.CustomerCode = $("#txtCodeCustInfo").val();
        objCustomer.Name = $("#txtNameCustInfo").val();
        objCustomer.FatherName = $("#txtCustomerFatherName").val();
        objCustomer.CustomerType = "";
        objCustomer.Address = $("#txtAddress").val();
        objCustomer.District = $("#cmbDistrict").val();
        objCustomer.Thana = $("#txtThana").val();
        objCustomer.Phone = $("#txtMobileNo1").val();
        objCustomer.Phone2 = $("#txtMobileNo2").val();
        objCustomer.Dob = $("#txtDateOfBirth").val();
        objCustomer.Nid = $("#txtNidCustInfo").val();
        objCustomer.Gender = $("#cmbGender").data("kendoComboBox").text();
        objCustomer.CompanyId = $("#cmbCompanyName").data("kendoComboBox").value();
        objCustomer.BranchId = $("#cmbBranchName").data("kendoComboBox").value();
        objCustomer.TypeId = 1;
        objCustomer.IsActive = $("#chkIsActive").is(":checked") == true ? 1 : 0;
        objCustomer.ProductId = $("#txtPidCustInfo").val();
        return objCustomer;

    },

    CustomerTypeCombo: function () {
        var objCustomer = customerDetailsManager.GetCustomerType();

        $("#cmbCustomerType").kendoComboBox({
            placeholder: "Select Type",
            dataTextField: "Type",
            dataValueField: "TypeId",
            dataSource: objCustomer,
            filter: "contains",
            suggest: true,
            change: function () {

                var value = this.value();
                AjaxManager.isValidItem("cmbCustomerType", true);
            }
        });
    },
    DistrictCombo: function () {
        var objDistrict = DistrictManager.GetDistrictInformation();
        $("#cmbDistrict").kendoComboBox({
            placeholder: "Select District...",
            dataSource: objDistrict,
            suggest: true,
        });
    },

    ClearCustomerForm: function () {

        //$('#hdnCustomerId').val(0);
        //$('#hdnPrantId').val(0);
        //$('#hdnFlag').val(0);
        //$("#txtNameCustInfo").val("");
        //$("#txtNidCustInfo").val("");
        //$("#txtMobileNo1").val("");
        //$("#txtMobileNo2").val("");
        //$("#txtCustomerCode").val("");

        $("#cmbCompanyName").data("kendoComboBox").value('');
        $("#cmbBranchName").data("kendoComboBox").value('');

        $("#txtCustomerFatherName").val("");
        $("#cmbCustomerType").data("kendoComboBox").value('');

        $("#txtAddress").val("");
        $("#cmbDistrict").data("kendoComboBox").value("");
        $("#txtThana").val("");
        $("#txtDateOfBirth").val("");
        $("#cmbGender").data("kendoComboBox").value("");
        $('#chkIsActive').removeAttr('checked', 'checked');
        $("#CustomerDetailsDiv > form").kendoValidator();
        $("#CustomerDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");
        status.text("").removeClass("invalid");
        $("#txtBranchSmsMobileNumber").val("");


    },
    FillCustomerDetailsInForm: function (objCustomer) {

        $("#txtNameCustInfo").val(objCustomer.Name);
        $("#txtCodeCustInfo").val(objCustomer.CustomerCode);
        $("#txtCustomerFatherName").val(objCustomer.FatherName);
        $("#hdnCustomerId").val(objCustomer.CustomerId);
        $("#txtAddress").val(objCustomer.Address);
        $("#cmbDistrict").data("kendoComboBox").text(objCustomer.District);

        $("#txtMobileNo1").val(objCustomer.Phone);
        $("#txtMobileNo2").val(objCustomer.Phone2);

        $("#txtDateOfBirth").val(objCustomer.Dob);
        $("#txtNidCustInfo").val(objCustomer.NId);
        $("#txtPidCustInfo").val(objCustomer.ProductId);
        $("#cmbGender").data("kendoComboBox").text(objCustomer.Gender);
        $("#txtThana").val(objCustomer.Thana);
        $("#cmbCustomerType").data("kendoComboBox").value('');
        var cmbCompany = $("#cmbCompanyName").data("kendoComboBox");
        cmbCompany.value(objCustomer.CompanyId);

        empressCommonHelper.GenerateBranchCombo(objCustomer.CompanyId, "cmbBranchName");

        $("#cmbBranchName").data("kendoComboBox").value(objCustomer.BranchId);
        $("#txtBranchSmsMobileNumber").val(objCustomer.BranchSmsMobileNumber);
        $("#hdnFlag").val(objCustomer.Flag);
        if (objCustomer.IsActive == 1) {
            $('#chkIsActive').attr('checked', 'checked');
        }
        else {
            $('#chkIsActive').removeAttr('checked', 'checked');
        }
        
        // -----------------Fill customer sales information-----------------------

        customerDetailsHelper.FillCustomerSalesDetails(objCustomer);
    },
    
    FillCustomerSalesDetails: function (objCustomer) {
        var salesData = SalesInfoSummaryManager.gridDataSource(objCustomer.CustomerId);
        var salesGrid = $("#gridSalesDetails").data("kendoGrid");
        salesGrid.setDataSource(salesData);
    },

    populateGenderCombo: function () {
        $("#cmbGender").kendoComboBox({
            placeholder: "Select Gender...",
            dataTextField: "Name",
            dataValueField: "Id",
            dataSource: [{ Name: "Male", Id: 1 },
                         { Name: "Female", Id: 2 }],

            filter: "contains",
            suggest: true

        });
    },
    GenerateDateOfBirth: function () {
        $("#txtDateOfBirth").kendoDatePicker({
            format: "dd-MMM-yyyy"
        });

    },
    changeEventForBranch: function () {

        var branchId = $("#cmbBranchName").data("kendoComboBox").value();
        var branchData = $("#cmbBranchName").data("kendoComboBox");
        var brId = branchData.value();
        var branchName = branchData.text();

        if (brId == branchName) {
            return false;
        } else {
            if (branchId != "") {

                var branchSmsMObileNumber = customerDetailsManager.getSmsMobileNumberByBranchId(branchId);
                $("#txtBranchSmsMobileNumber").val(branchSmsMObileNumber);
                //$("#liBranchSmsMobileNumber").removeClass('displayNone');
            } else {
                $("#txtBranchSmsMobileNumber").val("");
                //$('#liBranchSmsMobileNumber').addClass('displayNone');
            }
        }



    },
    chnageEvent: function (number, ctrlId) {

        //var customerCode = $("#txtCustomerCode").val();
        var customerId = $("#hdnCustomerId").val() == "" ? "" : $("#hdnCustomerId").val();
        customerDetailsManager.CustomerCodeChange(number, customerId, ctrlId);
    },



    CustomerDetailsEvent: function () {

        $("#txtCodeCustInfo").change(function () {
            var customerCode = $("#txtCodeCustInfo").val();
            customerDetailsManager.CheckExistCustomerCode(customerCode);
        });

        $("#btnSearchCustInfo").click(function () {
            var customerCode = $("#txtCodeCustInfo").val();
            customerDetailsManager.CheckExistCustomerCode(customerCode);
        });

        $("#txtPidCustInfo").change(function () {
            var productId = $('#txtPidCustInfo').val();
            var nationalId = $('#txtNidCustInfo').val();
            var mobileNo1 = $('#txtMobileNo1').val();

            if (productId != "") {
                $("#txtCodeCustInfo").val(productId);
                customerDetailsManager.CheckExistCustomerCode(nationalId);
            }
           else if (nationalId != "") {
                $("#txtCodeCustInfo").val(nationalId);
                customerDetailsManager.CheckExistCustomerCode(nationalId);
            } else if (mobileNo1 != "") {
                $("#txtCodeCustInfo").val(mobileNo1);
            }
        });

        $('#txtNidCustInfo').change(function () {
 
            var nationalId = $('#txtNidCustInfo').val();
            var mobileNo1 = $('#txtMobileNo1').val();
            if (nationalId != "") {
                $("#txtCodeCustInfo").val(nationalId);
                customerDetailsManager.CheckExistCustomerCode(nationalId);
            } else if (mobileNo1 != "") {
                $("#txtCodeCustInfo").val(mobileNo1);
            }
        });

        $('#txtMobileNo1').blur(function () {

            var mobileNo1 = $('#txtMobileNo1').val();
            var nationalId = $('#txtNidCustInfo').val();
            if (nationalId == "") {
                $("#txtCodeCustInfo").val(mobileNo1);
                customerDetailsManager.CheckExistCustomerCode(mobileNo1);
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
                }
            }

        });

        $('#txtMobileNo2').change(function () {
            var mobileNo2 = $('#txtMobileNo2').val();
            var res = customerInfoManager.CheckExistMobileNumber(mobileNo2, 2);
            if (res) {
                AjaxManager.MsgBox('warning', 'center', 'Message:', 'Mobile No-2 Already Exist!',
                   [{
                       addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                           $noty.close();
                           $('#txtMobileNo2').val("");
                       }
                   }]);
            }
        });


    }

};