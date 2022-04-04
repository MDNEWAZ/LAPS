
var branchDetailsManager = {
    GetMotherCompany: function () {
        var objCompany = "";
        var jsonParam = "";
        var serviceUrl = "../Company/GetMotherCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCompany;
    },
    

    SaveBranchInformation: function () {
        
        if (branchDetailsHelper.ValidateBranchInfoForm()) {

            var objBranch = branchDetailsHelper.CreateBranchObject();

            var objBranchInfo = JSON.stringify(objBranch).replace(/&/g, "^");
            var jsonParam = 'strobjBranch=' + objBranchInfo;
            var serviceUrl = "../Branch/SaveBranch/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);

        }
        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Branch Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            branchDetailsHelper.clearBranchForm();
                            $("#divgridBranchSummary").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Branch Already Exist") {

                AjaxManager.MsgBox('warning', 'center', 'Already Exists:', 'Branch Code Already Exist !',
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
    GetIsBranchCodeUsed: function (branchCode) {
        var objIsBranchCodeUsed = "";
        var jsonParam = "";
        var serviceUrl = "../Branch/GetIsBranchCodeUsed/?branchCode=" + branchCode;
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objIsBranchCodeUsed = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objIsBranchCodeUsed;
    },

};

var branchDetailsHelper = {

    populateCompany: function () {
        var objCompany = new Object();
        objCompany = branchDetailsManager.GetMotherCompany();

        $("#cmbCompanyName").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany,
            filter: "contains",
            suggest: true
        });
    },
    changeCompanyName: function () {
        var companyId = $("#cmbCompanyName").val();
        var comboboxforCompany = $("#cmbCompanyName").data("kendoComboBox");
        var companyName = comboboxforCompany.text();
        if (companyId == companyName) {
            return false;
        }

        branchDetailsHelper.GetEmployeeByCompanyId(companyId);
    },
   

    clearBranchForm: function () {
        debugger;
        $("#hdnBranchId").val("0");
        $("#cmbCompanyName").val("");
        $("#txtBranchCode").val("");
        $("#txtBranchName").val("");
        $("#txtBranchDescription").val("");
        $("#txtBranchSMSMobileNumber").val("");
        branchDetailsHelper.populateCompany();
        
        $("#branchDetailsDiv > form").kendoValidator();
        $("#branchDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");
        $("#txtBranchCode").prop('disabled', false);
        $("#chkIsSmsEligible").removeProp('checked', 'checked');
        $("#chkIsActive").removeProp('checked', 'checked');
    },
    CreateBranchObject: function () {
        var objbranch = new Object();
        objbranch.BranchId = $("#hdnBranchId").val();
        objbranch.CompanyId = $("#cmbCompanyName").val();
        objbranch.BranchCode = $("#txtBranchCode").val();
        objbranch.BranchName = $("#txtBranchName").val();
        objbranch.BranchSmsMobileNumber = $("#txtBranchSMSMobileNumber").val();
        objbranch.BranchDescription = $("#txtBranchDescription").val();
        objbranch.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;
        objbranch.IsSmsEligible = $("#chkIsSmsEligible").is(':checked') == true ? 1 : 0;
        return objbranch;
    },

    ValidateBranchInfoForm: function () {
        var data = [];
        
        var validator = $("#branchDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            var companyId = $("#cmbCompanyName").val();
            var comboboxforCompany = $("#cmbCompanyName").data("kendoComboBox");
            var companyName = comboboxforCompany.text();
            if (companyId == companyName) {
                status.text("Oops! Company Name is invalid.").addClass("invalid");
                $("#cmbCompanyName").val("");
                branchDetailsHelper.populateCompany();
                return false;
            }
            else {
                return true;
            }
            

            
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },

    populateBranchDetails: function (objBranch) {
        branchDetailsHelper.clearBranchForm();
        $("#hdnBranchId").val(objBranch.BranchId);

        var company = $("#cmbCompanyName").data("kendoComboBox");
        company.value(objBranch.CompanyId);
        
        $("#txtBranchCode").val(objBranch.BranchCode);
        $("#txtBranchName").val(objBranch.BranchName);
        $("#txtBranchDescription").val(objBranch.BranchDescription);
        $("#txtBranchSMSMobileNumber").val(objBranch.BranchSmsMobileNumber);
        if(objBranch.IsActive==1) {
            $("#chkIsActive").prop('checked','checked');
        }else {
            $("#chkIsActive").removeProp('checked','checked');
        }

        if (objBranch.IsSmsEligible == 1) {
            $("#chkIsSmsEligible").prop('checked', 'checked');
        } else {
            $("#chkIsSmsEligible").removeProp('checked', 'checked');
        }
    },

};

