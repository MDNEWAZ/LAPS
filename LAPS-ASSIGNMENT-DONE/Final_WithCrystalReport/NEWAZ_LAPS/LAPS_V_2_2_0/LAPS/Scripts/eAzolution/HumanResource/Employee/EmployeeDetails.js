var EmployeeDetailsManager = {
    SaveEmployeeInformation: function () {


        var objBranch = EmployeeDetailsHelper.CreateEmployeeObject();

            var objBranchInfo = JSON.stringify(objBranch).replace(/&/g, "^");
            var jsonParam = 'employee=' + objBranchInfo;
            var serviceUrl = "../Employee/SaveEmployee/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);

        
        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Employee Saved Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            EmployeeDetailsHelper.clearEmployeeForm();
                            $("#divgridEmployeeSummary").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Employee Already Exist") {

                AjaxManager.MsgBox('warning', 'center', 'Already Exists:', 'Employee Already Exist !',
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
    }
};
var EmployeeDetailsHelper = {
    clearEmployeeForm: function () {
        $("#hdnEmployeeId").val("0");
        $("#txtEmployeeName").val("");
        $("#txtMobile").val("");
        $("#txtEmail").val("");
        $("#txtCompanyName").val("");
        $("#txtDesignation").val("");
        $("#txtSalary").val("0");
        $("#EmployeeDetailsDiv > form").kendoValidator();
        $("#EmployeeDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");
        status.text("").removeClass("invalid");
    },
    CreateEmployeeObject: function () {
        var objbranch = new Object();
        objbranch.EmployeeId = $("#hdnEmployeeId").val();
        objbranch.EmployeeName = $("#txtEmployeeName").val();
        objbranch.Mobile = $("#txtMobile").val();
        objbranch.Email = $("#txtEmail").val();
        objbranch.CompanyName = $("#txtCompanyName").val();
        objbranch.Designation = $("#txtDesignation").val();
        objbranch.Salary = $("#txtSalary").val();


        return objbranch;
    },
    populateEmployeeDetails: function (objBranch) {
        debugger;
        branchDetailsHelper.clearBranchForm();
        $("#hdnEmployeeId").val(objBranch.EmployeeId);
        $("#txtEmployeeName").val(objBranch.EmployeeName);
        $("#txtMobile").val(objBranch.Mobile);
        $("#txtEmail").val(objBranch.Email);
        $("#txtCompanyName").val(objBranch.CompanyName);
        $("#txtDesignation").val(objBranch.Designation);
        $("#txtSalary").val(objBranch.Salary);
    },
};