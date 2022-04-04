var gbbranchCode = "";
var SalesRepresentatorDetailsManager = {

    SaveSalesRepresentator: function () {
        var validator = $("#salesRepDetailsDiv").kendoValidator().data("kendoValidator"),
           status = $(".status");
        if (validator.validate()) {

            var companyId = $("#hdnCompanyId").val();
            var branchId = $("#hdnBranchId").val();

            if (companyId != 0 && branchId != 0) {
                var objSalesRep = SalesRepresentatorDetailsHelper.CreateSalesRepresentatorObject();
                var salesRepobj = JSON.stringify(objSalesRep);
                var jsonParam = "objSalesRepresentator:" + salesRepobj;
                var serviceUrl = "../SalesRepresentator/SaveSalesRepresentator";
                AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select Company & Branch Appropriately',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
            }


        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Sales Representator Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            SalesRepresentatorDetailsHelper.ClearSalesRepresentatorForm();
                            $("#gridSalesRepSummary").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Exist") {

                AjaxManager.MsgBox('warning', 'center', 'Already Exists:', 'Sales Representator Code Already Exist',
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
            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
        }
    },

    GetCommissionAmountBySaleRepType: function (salesRepTypeId) {
        var objCommission = "";
        var jsonParam = "salesRepTypeId=" + salesRepTypeId;
        var serviceUrl = "../CommissionSettings/GetCommissionAmountBySaleRepType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objCommission = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCommission;
    }

};

var SalesRepresentatorDetailsHelper = {

    InitSalesRepDetails: function () {

        $("#divIncentiveSummary").kendoWindow({

            title: "Incentive Settings",
            resizeable: false,
            width: "80%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
           
        });

        SalesRepresentatorDetailsHelper.PopulateSaleRepType();
        SalesRepresentatorDetailsHelper.SalesRepTypeChangeEvent();  //Restriction for SalesRepresentator Type  //Now restriction withdraw 23-07-2016
       // SalesRepresentatorDetailsHelper.SalesRepCodeChangeEvent();   //Restriction for SalesRepresentator Code  //Now restriction withdraw 23-07-2016

        $("#txtFixedAmount").kendoNumericTextBox();

        $("#btnSave").click(function () {
            SalesRepresentatorDetailsManager.SaveSalesRepresentator();
        });
        $("#btnClearAll").click(function () {
            SalesRepresentatorDetailsHelper.ClearSalesRepresentatorForm();
        });

        $("#btnViewIncentive").click(function () {

            $("#divIncentiveSummary").data("kendoWindow").open().center();
            IncentiveSettingsSummaryHelper.InitIncentiveSummary();
            var grid = $("#incentiveSummaryGrid").data("kendoGrid");
            grid.hideColumn("Edit");
        });
        

    },

    PopulateSaleRepType: function () {
        $("#cmbSalesRepType").kendoComboBox({
            placeholder: "Select Type",
            dataTextField: "SalesRepTypeName",
            dataValueField: "SalesRepTypeId",
            dataSource: empressCommonManager.GetSalesRepresentatorType(),

            filter: "contains",
            suggest: true

        });
    },

    SalesRepTypeChangeEvent: function () {
        $("#cmbSalesRepType").change(function () {
            SalesRepresentatorDetailsHelper.ValidateFields();

            var salesRepTypeId = $("#cmbSalesRepType").data("kendoComboBox").value();

            var commissionObj = SalesRepresentatorDetailsManager.GetCommissionAmountBySaleRepType(salesRepTypeId);
            $("#txtInsSaleCommission").val(commissionObj.InstallmentSaleCommission);
            $("#txtCahsSaleCommission").val(commissionObj.CashSaleCommission);

            $("#txtSalesRepCode").val("");
            $("#txtSalesRepId").val("");
            $("#txtSalesRepCode").removeAttr("disabled");//lblSalesRepCode

            //Previous Code /Restriction Withdraw 23-07-2016

            //if (salesRepTypeId == 1) {//Agent
            //    $("#lblSalesRepCode").html("0001 to 7999");
            //}
            //else if (salesRepTypeId == 2) {//SMO
            //    $("#lblSalesRepCode").html("8000 to 8999");
            //}
            //if (salesRepTypeId == 3) {//Branch Staff
            //    $("#lblSalesRepCode").html("9000 to 9999");
            //}

        });

        $("#txtSalesRepCode").change(function () {

            if ($("#txtSalesRepCode").val() != "") {
                var repId = gbbranchCode + $("#txtSalesRepCode").val();
                $("#txtSalesRepId").val(repId);
            }
        });

    },
    SalesRepCodeChangeEvent: function () {

        $("#txtSalesRepCode").change(function () {

            if (SalesRepresentatorDetailsHelper.isNumber(this.value)) {
                var salesRepTypeId = $("#cmbSalesRepType").data("kendoComboBox").value();
                var saleRepCode = $("#txtSalesRepCode").val();
                if (salesRepTypeId > 0) {
                    if (salesRepTypeId == 1) {
                        if (saleRepCode < 1 || saleRepCode > 7999) {

                            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Enter Code Between 0001 to 7999',
                                [{
                                    addClass: 'btn btn-primary',
                                    text: 'Ok',
                                    onClick: function($noty) {
                                        $noty.close();
                                        $("#txtSalesRepCode").val("");
                                        $("#txtSalesRepId").val("");
                                    }
                                }]);

                        }
                    } else if (salesRepTypeId == 2) {
                        if (saleRepCode < 8000 || saleRepCode > 8999) {

                            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Enter Code Between 8000 to 8999',
                                [{
                                    addClass: 'btn btn-primary',
                                    text: 'Ok',
                                    onClick: function($noty) {
                                        $noty.close();
                                        $("#txtSalesRepCode").val("");
                                        $("#txtSalesRepId").val("");
                                    }
                                }]);

                        }
                    } else if (salesRepTypeId == 3) {
                        if (saleRepCode < 9000 || saleRepCode > 9999) {

                            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Enter Code Between 9000 to 9999',
                                [{
                                    addClass: 'btn btn-primary',
                                    text: 'Ok',
                                    onClick: function($noty) {
                                        $noty.close();
                                        $("#txtSalesRepCode").val("");
                                        $("#txtSalesRepId").val("");
                                    }
                                }]);
                        }
                    }


                    if ($("#txtSalesRepCode").val() != "") {
                        var repId = gbbranchCode + $("#txtSalesRepCode").val();
                        $("#txtSalesRepId").val(repId);
                    }


                } else {
                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select Representator Type First',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function($noty) {
                                $noty.close();
                                $("#txtSalesRepCode").val("");

                            }
                        }]);
                }

            } else {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Invalid Character!',
                         [{
                             addClass: 'btn btn-primary',
                             text: 'Ok',
                             onClick: function ($noty) {
                                 $noty.close();
                                 $("#txtSalesRepCode").val("");

                             }
                         }]);
            }
        });


    },

    CreateSalesRepresentatorObject: function () {
        var obj = new Object();

        obj.Id = $("#hdnsalesRepId").val();
        obj.SalesRepType = $("#cmbSalesRepType").data("kendoComboBox").value();
        obj.SalesRepId = $("#txtSalesRepId").val();
        obj.SalesRepCode = $("#txtSalesRepCode").val();
        obj.Address = $("#txtAddress").val();
        obj.SalesRepSmsMobNo = $("#txtSalesRepSMSMobNo").val();
        obj.SalesRepBkashNo = $("#txtSalesRepbKashNo").val();
        obj.FixedAmount = $("#txtFixedAmount").data("kendoNumericTextBox").value();
        obj.CompanyId = $("#hdnCompanyId").val();
        obj.BranchId = $("#hdnBranchId").val();
        obj.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;
        obj.IsIncentiveActive = $("#chkIsIncentiveActive").is(':checked') == true ? 1 : 0;
        obj.IsCommissionActive = $("#chkIsCommissionActive").is(':checked') == true ? 1 : 0;
        obj.IsSalesRepSmsSent = $("#chkIsSalesRepSmsSent").is(':checked') == true ? 1 : 0;
        return obj;
    },


    fillCompanyAndBranchForRep: function (companyId, companyName, branchId, branchName) {

        if (branchId != 0) {
           
            var branchInfo = empressCommonManager.GetBranchInfoByBranchId(branchId);
            if (branchInfo.IsUpgraded == 0 || branchInfo.BranchCode.length < 4) {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Upgrade Branch First!',
                         [{
                             addClass: 'btn btn-primary',
                             text: 'Ok',
                             onClick: function ($noty) {
                                 $noty.close();
                                 $("#lblCompanyName").html("");
                                 $("#hdnCompanyId").val("0");

                                 $("#lblBranchName").html("");
                                 $("#hdnBranchId").val("0");
                                
                             }
                         }]);
            } else {
                gbbranchCode = branchInfo.BranchCode;
                
                $("#lblCompanyName").html("");
                $("#hdnCompanyId").val("0");

                $("#lblBranchName").html("");
                $("#hdnBranchId").val("0");


                $("#lblCompanyName").html(companyName);
                $("#hdnCompanyId").val(companyId);

                $("#lblBranchName").html(branchName);
                $("#hdnBranchId").val(branchId);

                branchCode = "";
                $("#txtSalesRepCode").val("");
                $("#txtSalesRepId").val("");
                $("#cmbSalesRepType").data("kendoComboBox").value("");
            }
           
        }

      
        
    },


    ClearSalesRepresentatorForm: function () {
        $("#hdnsalesRepId").val(0);
        $("#cmbSalesRepType").data("kendoComboBox").value("");
        $("#txtSalesRepId").val("");
        $("#txtSalesRepCode").val("");
        $("#txtAddress").val("");
        $("#txtSalesRepSMSMobNo").val("");
        $("#txtSalesRepbKashNo").val("");
        $("#txtFixedAmount").data("kendoNumericTextBox").value("");
        $("#hdnCompanyId").val(0);
        $("#hdnBranchId").val(0);
        $("#chkIsActive").removeProp('checked', 'checked');

        $("#chkIsIncentiveActive").removeProp('checked', 'checked');
        $("#chkIsCommissionActive").removeProp('checked', 'checked');
        $("#chkIsSalesRepSmsSent").removeProp('checked', 'checked');

        $("#lblSalesRepCode").html("");
        gbbranchCode = "";

        $("#salesRepDetailsDiv > form").kendoValidator();
        $("#salesRepDetailsDiv").find("span.k-tooltip-validation").hide();

        var status = $(".status");
        status.text("").removeClass("invalid");

        $("#lblCompanyName").html("");
        $("#lblBranchName").html("");

        $("#txtInsSaleCommission").val("");
        $("#txtCahsSaleCommission").val("");
    },

    FillSalesRepresentatorForm: function (obj) {

        SalesRepresentatorDetailsHelper.ClearSalesRepresentatorForm();
        $("#hdnsalesRepId").val(obj.Id);
        $("#cmbSalesRepType").data("kendoComboBox").value(obj.SalesRepType);
        $("#txtSalesRepId").val(obj.SalesRepId);
        $("#txtSalesRepCode").val(obj.SalesRepCode);
        $("#txtAddress").val(obj.Address);
        $("#txtSalesRepSMSMobNo").val(obj.SalesRepSmsMobNo);
        $("#txtSalesRepbKashNo").val(obj.SalesRepBkashNo);
        $("#txtFixedAmount").data("kendoNumericTextBox").value(obj.FixedAmount);
        $("#hdnCompanyId").val(obj.CompanyId);
        $("#hdnBranchId").val(obj.BranchId);

        if (obj.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }

        if (obj.IsIncentiveActive == 1) {
            $("#chkIsIncentiveActive").prop('checked', 'checked');
        } else {
            $("#chkIsIncentiveActive").removeProp('checked', 'checked');
        }

        if (obj.IsCommissionActive == 1) {
            $("#chkIsCommissionActive").prop('checked', 'checked');
        } else {
            $("#chkIsCommissionActive").removeProp('checked', 'checked');
        }

        if (obj.IsSalesRepSmsSent == 1) {
            $("#chkIsSalesRepSmsSent").prop('checked', 'checked');
        } else {
            $("#chkIsSalesRepSmsSent").removeProp('checked', 'checked');
        }


        $("#lblCompanyName").html(obj.CompanyName);
        $("#lblBranchName").html(obj.BranchName);
        var objbranch = empressCommonManager.GetBranchInfoByBranchId(obj.BranchId);
        gbbranchCode = objbranch.BranchCode;


        var commissionObj = SalesRepresentatorDetailsManager.GetCommissionAmountBySaleRepType(obj.SalesRepType);
        $("#txtInsSaleCommission").val(commissionObj.InstallmentSaleCommission);
        $("#txtCahsSaleCommission").val(commissionObj.CashSaleCommission);

    },

    ValidateFields: function () {
        var companyId = $("#hdnCompanyId").val();
        var branchId = $("#hdnBranchId").val();
        if (companyId == 0) {
            if (branchId == 0) {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select Company & Branch',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            $("#cmbSalesRepType").data("kendoComboBox").value("");
                        }
                    }]);
            }

        } else {
            if (branchId == 0) {
                AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Select Branch',
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            $("#cmbSalesRepType").data("kendoComboBox").value("");
                        }
                    }]);
            }
        }


    },



    isNumber: function (n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }



};
