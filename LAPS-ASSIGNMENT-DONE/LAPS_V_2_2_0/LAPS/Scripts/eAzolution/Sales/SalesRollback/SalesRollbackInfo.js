
var SalesRollbackInfoManager = {
    
    GenerateSalesRollback:function() {
        var operationType = $("#OperationType input[type='radio']:checked").val();//Value 1=Make InActive, 2=Branch Switch
        if (operationType > 0) {
            if (operationType == 1) {
                SalesRollbackDetailManager.MakeInActive(operationType);
            }
            else {
                var changedCompanyId = $("#cmbChangedCompany").data("kendoComboBox").value();
                var changedBranchId = $("#cmbChangedBranch").data("kendoComboBox").value();
                
                if (changedCompanyId > 0 && changedBranchId > 0) {
                    SalesRollbackDetailManager.MakeInActive(operationType);
                } else {
                    AjaxManager.MsgBox('warning', 'center', 'Warning', 'Select Company & Branch!',
                   [{
                       addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                           $noty.close();
                       }
                   }]);
                }

            }
        }
        else {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please Select Operation Type',
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();
                           }
                       }]);
        }
    },

    populateStaffcombo: function (salesRepType,changedCompanyId, changedBranchId) {
            $("#cmbStaffId").kendoComboBox({
                placeholder: "Select Staff ID",
                dataTextField: "SalesRepId",
                dataValueField: "SalesRepId",
                dataSource: empressCommonManager.GetAllSalesRepresentatorComboByCompanyBranchAndType(salesRepType, changedCompanyId, changedBranchId),
                suggest: true,

            });
    },
};

var SalesRollbackInfoHelper = {

    initSalesRollbackHelper: function () {
        $("#changedCompanyBranchDiv").hide();
        $("input:radio").attr("checked", false);
        
        empressCommonHelper.GenerareHierarchyCompanyCombo("cmbChangedCompany");
        empressCommonHelper.GenerateBranchCombo(0, "cmbChangedBranch");
        $("#cmbChangedCompany").change(function () {
            SalesRollbackInfoHelper.ChangeEventForCompanyCombo();
        });

        $("#cmbChangedBranch").change(function () {
            SalesRollbackInfoHelper.ChangeEventForChangedBranchCombo();

        });

        SalesRollbackInfoHelper.OperationTypeChangeEvent();

        $("#btnClose").click(function () {
            $("#salesRollbackPopupDiv").data("kendoWindow").close();
            SalesRollbackInfoHelper.ClearSaleRollbackInfoForm();
        });
        
        //'''''''''''''''''''''''''''''''''''Roll Back ''''''''''''''''''''''''''''''''''''''

        $("#btnRollback").click(function () {
            SalesRollbackInfoManager.GenerateSalesRollback();
        });

        SalesRollbackInfoHelper.IsStaffChangeEvent();

    },

    ChangeEventForCompanyCombo: function () {
        var comboboxbranch = $("#cmbChangedBranch").data("kendoComboBox");
        var companyData = $("#cmbChangedCompany").data("kendoComboBox");
        var companyId = companyData.value();
        var companyName = companyData.text();
        
        $("#chkStaff").removeProp('checked', 'checked');
        var saleRepCombo = $("#cmbSaleRepresentator").data("kendoComboBox");
        var staffCombo = $("#cmbStaffId").data("kendoComboBox");
        if (saleRepCombo != undefined) {
            saleRepCombo.value('');
            saleRepCombo.destroy();
        }
        if (staffCombo != undefined) {
            staffCombo.value('');
            staffCombo.destroy();
        }


        if (companyId == companyName) {
            companyData.value('');
            comboboxbranch.value('');
            comboboxbranch.destroy();
            empressCommonHelper.GenerateBranchCombo(0, "cmbChangedBranch");
            return false;
        }
        if (comboboxbranch != undefined) {
            comboboxbranch.value('');
            comboboxbranch.destroy();
        }
        empressCommonHelper.GenerateBranchCombo(companyId, "cmbChangedBranch");
    },
    
    ChangeEventForChangedBranchCombo: function () {

        $("#chkStaff").removeProp('checked', 'checked');
        //$("#staffcomboli").hide();

        var comboboxbranch = $("#cmbChangedBranch").data("kendoComboBox");
        var branchId = comboboxbranch.value();
        var branchName = comboboxbranch.text();


        var saleRepCombo = $("#cmbSaleRepresentator").data("kendoComboBox");
        var staffCombo = $("#cmbStaffId").data("kendoComboBox");


        if (branchId == branchName) {
            if (saleRepCombo != undefined) {
                saleRepCombo.value('');
                saleRepCombo.destroy();
            }
            if (staffCombo != undefined) {
                staffCombo.value('');
                staffCombo.destroy();
            }
            return false;
        }

        if (saleRepCombo != undefined) {
            saleRepCombo.value('');
            saleRepCombo.destroy();
        }
        if (staffCombo != undefined) {
            staffCombo.value('');
            staffCombo.destroy();
        }

        var changedCompanyId = $("#cmbChangedCompany").data("kendoComboBox").value();
        var changedBranchId = branchId;
        if (changedCompanyId > 0) {
            SalesRollbackInfoHelper.populateSalesRepresentatorCombo(changedCompanyId, changedBranchId);
        }
    },

    OperationTypeChangeEvent: function () {
        $("#txtBranchSwitch").change(function () {
            if ($("#txtBranchSwitch").attr("checked")) {
                //$("#cmbChangedCompany").data("kendoComboBox").enable(true);
                //$("#cmbChangedBranch").data("kendoComboBox").enable(true);
                SalesRollbackInfoHelper.populateSalesRepresentatorComboWithoutData();
                $("#changedCompanyBranchDiv").show();
            }
        });

        $("#txtInActive").change(function () {
            if ($("#txtInActive").attr("checked")) {
                $("#changedCompanyBranchDiv").hide();
                //$("#cmbChangedCompany").data("kendoComboBox").enable(false);
                //$("#cmbChangedBranch").data("kendoComboBox").enable(false);
            }
        });
    },

    IsStaffChangeEvent: function () {
        $('#chkStaff').live('click', function (e) {
            var $cb = $(this);
            if ($cb.is(":checked")) {
                var changedCompanyId = $("#cmbChangedCompany").data("kendoComboBox").value();
                var changedBranchId = $("#cmbChangedBranch").data("kendoComboBox").value();
                if (changedCompanyId > 0 && changedBranchId > 0) {
                    $("#staffcomboli").show();
                    SalesRollbackInfoManager.populateStaffcombo(3, changedCompanyId, changedBranchId); //3 mean branch staff
                } else {
                    AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please Select Company & Branch',
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            $("#chkStaff").removeProp('checked', 'checked');
                        }
                        }]);
                }
            } else {
                $("#staffcomboli").hide();
            }

        });
    },

    CreateRollebackInfoObject: function (operationType) {

        var rollbackInfoData = $("#gridSaleInforamtion").data("kendoGrid").dataSource.data();
        
        var rollbackInfoObj = new Object();
        for (var i = 0; i < rollbackInfoData.length; i++) {
            rollbackInfoObj.CustomerCode = rollbackInfoData[i].ACustomer.CustomerCode;
            rollbackInfoObj.Phone = rollbackInfoData[i].ACustomer.Phone;
            rollbackInfoObj.Phone2 = rollbackInfoData[i].ACustomer.Phone2;
            // rollbackInfoObj.BranchCode = rollbackInfoData[i].ACustomer.BranchCode;
            rollbackInfoObj.InvoiceNo = rollbackInfoData[i].Invoice;
            rollbackInfoObj.SaleId = rollbackInfoData[i].SaleId;
            rollbackInfoObj.IsCustomerUpgraded = rollbackInfoData[i].ACustomer.IsUpgraded;
            rollbackInfoObj.ModelId = rollbackInfoData[i].AProduct.ModelId;
            rollbackInfoObj.CompanyId = rollbackInfoData[i].ACustomer.CompanyId;
            rollbackInfoObj.BranchId = rollbackInfoData[i].ACustomer.BranchId;
            rollbackInfoObj.FirstPayDate = kendo.toString(kendo.parseDate(kendo.toString(rollbackInfoData[i].FirstPayDate, "MM/dd/yyyy"), "MM/dd/yyyy"), "MM/dd/yyyy");
            if (operationType != 1) {
                rollbackInfoObj.ChangedCompanyId = $("#cmbChangedCompany").data("kendoComboBox").value();
                rollbackInfoObj.ChangedBranchId = $("#cmbChangedBranch").data("kendoComboBox").value();
                rollbackInfoObj.IsStaff = $("#chkStaff").is(":checked") == true ? 1 : 0;
                if (rollbackInfoObj.IsStaff > 0) {
                    rollbackInfoObj.StaffId = $("#cmbStaffId").data("kendoComboBox").value();
                }
                rollbackInfoObj.SalesRepId = $("#cmbSaleRepresentator").data("kendoComboBox").value();
            }
            rollbackInfoObj.DownPay = rollbackInfoData[i].DownPay;
          
        }
        return rollbackInfoObj;
    },

    populateSalesRepresentatorCombo: function (changedCompanyId, changedBranchId) {
        $("#cmbSaleRepresentator").kendoComboBox({
            placeholder: "----------Select----------",
            dataTextField: "SalesRepId",
            dataValueField: "SalesRepId",
            dataSource: empressCommonManager.GetSaleRepresentatorByCompanyAndBranch(changedCompanyId, changedBranchId),
            filter: "contains",
            suggest: true

        });
    },

    populateSalesRepresentatorComboWithoutData: function () {
        $("#cmbSaleRepresentator").kendoComboBox({
            placeholder: "----------Select----------",
            dataTextField: "SalesRepId",
            dataValueField: "SalesRepId",
            dataSource: [],
            filter: "contains",
            suggest: true

        });
    },

    populateStaffcomboWithoutData: function () {
        $("#cmbStaffId").kendoComboBox({
            placeholder: "Select Staff ID",
            dataTextField: "SalesRepId",
            dataValueField: "SalesRepId",
            dataSource: [],
            filter: "contains",
            suggest: true
        });
    },

   

    ClearSaleRollbackInfoForm: function () {
        $("#cmbChangedCompany").data("kendoComboBox").value("");
        $("#cmbChangedBranch").data("kendoComboBox").value("");
        var repCombo = $("#cmbSaleRepresentator").data("kendoComboBox");
        if (repCombo != undefined) {
            repCombo.value("");
        }
        var stafCombo = $("#cmbStaffId").data("kendoComboBox");
        if (stafCombo != undefined) {
            stafCombo.value("");
        }
        $("#chkStaff").removeProp('checked', 'checked');
        $("input:radio").attr("checked", false);
        
        $("#changedCompanyBranchDiv").hide();
    },

};