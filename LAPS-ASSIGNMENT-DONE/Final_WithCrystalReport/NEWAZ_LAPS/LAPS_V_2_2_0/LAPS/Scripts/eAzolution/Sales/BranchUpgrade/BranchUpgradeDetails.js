
var branchUpgradeDetailsManager = {

    UpgradeBranchInformation: function () {

        var objBranch = branchUpgradobjBrancheDetailsHelper.CreateUpgradeBranchObject();
        var objBranchInfo = JSON.stringify(objBranch);
        var jsonParam = ':' + objBranchInfo;
        var serviceUrl = "../Branch/UpgradeBranch/";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);


        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Branch Ugraded Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            $("#gridBranchUpgradeSummary").data("kendoGrid").dataSource.read();
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



};

var branchUpgradeDetailsHelper = {
    
    InitBranchUpgradeDetails:function() {
        $("#btnUpgradeBranch").click(function () {
            AjaxManager.MsgBox('information', 'center', 'Confirm Update:', "Are You Confirm to Upgrade Branch Code</br> as well as Customer's Code of this Branch? ",
               [{
                   addClass: 'btn btn-primary',
                   text: 'Yes',
                   onClick: function ($noty) {
                       $noty.close();
                       branchUpgradeDetailsManager.UpgradeBranchInformation();
                   }
               },
               {
                   addClass: 'btn',
                   text: 'No',
                   onClick: function ($noty) {
                       $noty.close();

                   }
               }]);

        });
    },

    changeCompanyName: function () {
        var companyId = $("#cmbCompanyName").val();
        var comboboxforCompany = $("#cmbCompanyName").data("kendoComboBox");
        var companyName = comboboxforCompany.text();
        if (companyId == companyName) {
            return false;
        }

        branchUpgradeDetailsHelper.GetEmployeeByCompanyId(companyId);
    },

    CreateUpgradeBranchObject: function () {
        var objbranch = new Object();
        objbranch.BranchId = $("#hdnBranchId").val();
        objbranch.BranchCode = $("#txtOldBranchCode").val();
        objbranch.BranchName = $("#txtBranchName").val();
        return objbranch;
    },

    populateBranchDetails: function (objBranch) {
      
        $("#hdnBranchId").val(objBranch.BranchId);
       // $("#txtOldBranchCode").val(objBranch.BranchCode);
        $("#txtBranchName").val(objBranch.BranchName);
        $("#txtBranchDescription").val(objBranch.BranchDescription);
        //New Branch Code Generate
        
        $("#txtNewBranchCode").val(objBranch.NewBranchCode);
        $("#txtOldBranchCode").val(objBranch.BranchCode);
        if (objBranch.IsUpgraded != 1) {
            
            $("#liUpgradeButton").show();
         
         
            
        } else {
         
            $("#txtNewBranchCode").val("Already Upgraded");
            //$("#txtOldBranchCode").val(objBranch.BranchCode.substr(objBranch.BranchCode.length - 4, 2));
            
            $("#liUpgradeButton").hide();
        }
       
       
    },

};

