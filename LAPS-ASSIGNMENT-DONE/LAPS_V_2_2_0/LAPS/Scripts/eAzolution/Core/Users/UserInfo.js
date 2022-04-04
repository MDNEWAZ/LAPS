var userInfoManager = {
    //GetMotherCompany: function () {
    //    var objCompany = "";
    //    var jsonParam = "";
    //    var serviceUrl = "../Company/GetMotherCompany/";
    //    AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

    //    function onSuccess(jsonData) {
    //        objCompany = jsonData;
    //    }
    //    function onFailed(error) {
    //        window.alert(error.statusText);
    //    }
    //    return objCompany;
    //},
    //GetEmployeeByCompanyId: function (companyId) {
    //    var objEmployee = "";
    //    var jsonParam = "companyId=" + companyId;
    //    var serviceUrl = "../Employee/GetEmployeeByCompanyIdForUser/";
    //    AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

    //    function onSuccess(jsonData) {
    //        objEmployee = jsonData;
    //    }
    //    function onFailed(error) {
    //        window.alert(error.statusText);
    //    }
    //    return objEmployee;
    //},
    //GetEmployeeByCompanyIdInUser: function (companyId) {
    //    var objEmployee = "";
    //    var jsonParam = "companyId=" + companyId;
    //    var serviceUrl = "../Employee/GetEmployeeByCompanyIdInUserUpdate/";
    //    AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

    //    function onSuccess(jsonData) {
    //        objEmployee = jsonData;
    //    }
    //    function onFailed(error) {
    //        window.alert(error.statusText);
    //    }
    //    return objEmployee;
    //},
    
    //----------------------ashraf
    GetMotherCompany: function () {
        var objCompany = "";
        var jsonParam = "";

        var serviceUrl = "../../Company/GetMotherCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCompany;
    },

    GetEmployeeType: function () {
        var objEmployeeType = "";
        var jsonParam = "";

        var serviceUrl = "../../Employee/GetEmployeeTypeForCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objEmployeeType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployeeType;
    },

    GenerateBranchCombo: function (companyId) {
        var objBranch = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../../Branch/GetBranchByCompanyIdForCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objBranch = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objBranch;
    },
    GetDepartmentByCompanyId: function (companyId) {
        var objDepartment = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../../Department/GetDepartmentByCompanyId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objDepartment = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objDepartment;
    },

    GetEmployeeByCompanyIdAndBranchIdAndDepartmentId: function (companyId, branchId, departmentId) {
        var objEmployee = "";
        var jsonParam = "companyId=" + companyId + "&branchId=" + branchId + "&departmentId=" + departmentId;
        var serviceUrl = "../../Employee/GetEmployeeByCompanyIdAndBranchIdAndDepartmentId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objEmployee = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployee;
    },

};

var userInfoHelper = {
   
    initiateConveyanceReport: function () {
        //userInfoHelper.populateCompany();

        //userInfoHelper.GenerateEmployeeTypeCombo();
        //userInfoHelper.EmployeeTypeCombo();

    },
   

    //populateCompany: function () {
    //    var objCompany = new Object();
    //    objCompany = userInfoManager.GetMotherCompany();
    //    $("#cmbCompanyNameDetails").kendoComboBox({
    //        placeholder: "All",
    //        dataTextField: "CompanyName",
    //        dataValueField: "CompanyId",
    //        dataSource: objCompany
    //    });

    //    if (CurrentUser.CompanyId != null) {
    //        var companyData = $("#cmbCompanyNameDetails").data("kendoComboBox");
    //        companyData.value(CurrentUser.CompanyId);
    //        userInfoHelper.changeCompanyName();
    //    }
    //},
    //GenerateBranchCombo: function (companyId) {
    //    var objBranch = new Object();

    //    objBranch = userInfoManager.GenerateBranchCombo(companyId);

    //    $("#cmbBranchDetails").kendoComboBox({
    //        placeholder: "All",
    //        dataTextField: "BranchName",
    //        dataValueField: "BranchId",
    //        dataSource: objBranch
    //    });

    //    if (CurrentUser != null) {
    //        var comboboxbranch = $("#cmbBranchDetails").data("kendoComboBox");
    //        comboboxbranch.value(CurrentUser.BranchId);
    //        userInfoHelper.changeBranchName();
    //    }
    //},

    //GetDepartmentByCompanyId: function (companyId) {
    //    var objDepartment = new Object();

    //    objDepartment = userInfoManager.GetDepartmentByCompanyId(companyId);

    //    $("#cmbDepartmentNameDetails").kendoComboBox({
    //        placeholder: "All",
    //        dataTextField: "DepartmentName",
    //        dataValueField: "DepartmentId",
    //        dataSource: objDepartment
    //    });
    //},

    //GenerateEmployeeByCompanyId: function (companyId, branchId, departmentId) {
    //    var objEmp = new Object();
    //    if (departmentId == 0) {
    //        objEmp = null;
    //    }
    //    else {
    //        objEmp = userInfoManager.GetEmployeeByCompanyIdAndBranchIdAndDepartmentId(companyId, branchId, departmentId);
    //    }
    //    $("#cmbEmployee").kendoComboBox({
    //        placeholder: "All",
    //        dataTextField: "FullName",
    //        dataValueField: "HRRecordId",
    //        dataSource: objEmp
    //    });
    //},

    //EmployeeTypeCombo: function () {
    //    var objEmployeeType = new Object();
    //    objEmployeeType = userInfoManager.GetEmployeeType();
    //    $("#cmbEmployeeType").kendoComboBox({
    //        placeholder: "All",
    //        dataTextField: "EmployeeTypeName",
    //        dataValueField: "EmployeeType",
    //        dataSource: objEmployeeType
    //    });

    //    //if (CurrentUser.CompanyId != null) {
    //    //    var companyData = $("#cmbEmployeeType").data("kendoComboBox");
    //    //    companyData.value(CurrentUser.CompanyId);
    //    //    userInfoHelper.changeEmployeeType();
    //    //}
    //},

    //changeCompanyName: function () {

    //    var comboboxbranch = $("#cmbBranchDetails").data("kendoComboBox");
    //    var comboboxDep = $("#cmbDepartmentNameDetails").data("kendoComboBox");
    //    //var comboboxEmp = $("#cmbEmployee").data("kendoComboBox");


    //    var companyData = $("#cmbCompanyNameDetails").data("kendoComboBox");
    //    var companyId = companyData.value();
    //    var companyName = companyData.text();
    //    if (companyId == companyName) {
    //        companyData.value('');
    //        comboboxbranch.value('');
    //        comboboxbranch.destroy();

    //        comboboxDep.value('');
    //        comboboxDep.destroy();
    //        //comboboxEmp.value('');
    //        //comboboxEmp.destroy();

    //        userInfoHelper.GenerateBranchCombo(0);
    //        userInfoHelper.GetDepartmentByCompanyId(0);
    //        userInfoHelper.GenerateDesignationCombo(0);
    //       // userInfoHelper.GenerateEmployeeByCompanyId(0, 0, 0);
         
    //        return false;
    //    }
    //    if (comboboxbranch != undefined) {
    //        comboboxbranch.value('');
    //        comboboxbranch.destroy();
    //    }
    //    if (comboboxDep != undefined) {
    //        comboboxDep.value('');
    //        comboboxDep.destroy();
    //    }

    //    //if (comboboxEmp != undefined) {
    //    //    comboboxEmp.value('');
    //    //    comboboxEmp.destroy();
    //    //}

    
    //    userInfoHelper.GenerateBranchCombo(companyId);
    //    userInfoHelper.GetDepartmentByCompanyId(companyId);
    //   // userInfoHelper.GenerateEmployeeByCompanyId(companyId, 0, 0);
    //    groupMembershipHelper.GetGroupByCompanyId(companyId);

    //},

    //changeBranchName: function () {

    //    var comboboxbranch = $("#cmbBranchDetails").data("kendoComboBox");
    //    var comboboxDep = $("#cmbDepartmentNameDetails").data("kendoComboBox");
    //   // var comboboxEmp = $("#cmbEmployee").data("kendoComboBox");


    //    var companyData = $("#cmbCompanyNameDetails").data("kendoComboBox");
    //    var companyId = companyData.value();
    //    var companyName = companyData.text();

    //    var branchId = comboboxbranch.value();
    //    var branchName = comboboxbranch.text();
    //    if (branchId == branchName) {
    //        comboboxbranch.value('');
    //        if (comboboxDep != undefined) {
    //            comboboxDep.value('');
    //            comboboxDep.destroy();
    //        }
            
    //        //if (comboboxEmp != undefined) {
    //        //    comboboxEmp.value('');
    //        //    comboboxEmp.destroy();
    //        //}
      
    //        userInfoHelper.GetDepartmentByCompanyId(companyId);
    //      //  userInfoHelper.GenerateEmployeeByCompanyId(companyId, 0, 0);
    //        return false;
    //    }

    //    if (comboboxDep != undefined) {
    //        comboboxDep.value('');
    //        comboboxDep.destroy();
    //    }

    //    //if (comboboxEmp != undefined) {
    //    //    comboboxEmp.value('');
    //    //    comboboxEmp.destroy();
    //    //}

    //    userInfoHelper.GetDepartmentByCompanyId(companyId);
    //  //  userInfoHelper.GenerateEmployeeByCompanyId(companyId, branchId, 0);



    //},

    //changeDepartmentName: function () {

    //    var companyData = $("#cmbCompanyNameDetails").data("kendoComboBox");
    //    var companyId = companyData.value();
    //    var companyName = companyData.text();

    //    var comboboxbranch = $("#cmbBranchDetails").data("kendoComboBox");
    //    var branchId = comboboxbranch.value();
    //    var branchName = comboboxbranch.text();

    //    var comboboxDep = $("#cmbDepartmentNameDetails").data("kendoComboBox");
    //    var departmentId = comboboxDep.value();
    //    var departmentName = comboboxDep.text();

    //    var comboboxEmp = $("#cmbEmployee").data("kendoComboBox");

    //    if (departmentId == departmentName) {
    //        if (comboboxEmp != undefined) {
    //            comboboxEmp.value('');
    //            comboboxEmp.destroy();
    //        }

           
    //       // userInfoHelper.GenerateEmployeeByCompanyId(companyId, branchId, 0);
    //        return false;
    //    }


    //    if (comboboxEmp != undefined) {
    //        comboboxEmp.value('');
    //        comboboxEmp.destroy();
    //    }
       
    //   // userInfoHelper.GenerateEmployeeByCompanyId(companyId, branchId, departmentId);



    //},

    //end of ashraf
    
    //GenerateMotherCompanyCombo: function () {
    //    var objCompany = new Object();
    //    objCompany = userInfoManager.GetMotherCompany();

    //    $("#cmbCompanyName").kendoComboBox({
    //        placeholder: "Select Company...",
    //        dataTextField: "CompanyName",
    //        dataValueField: "CompanyId",
    //        dataSource: objCompany
    //    });
    //},
    
    //changeCompanyName: function () {
    //    var companyId = $("#cmbCompanyName").val();
    //    var comboboxforCompany = $("#cmbCompanyName").data("kendoComboBox");
    //    var companyName = comboboxforCompany.text();
    //    if (companyId == companyName) {
    //        return false;
    //    }
        
    //    var combobox = $("#cmbEmployee").data("kendoComboBox");
    //    combobox.destroy();

    //    groupMembershipHelper.GetGroupByCompanyId(companyId);
    //    userInfoHelper.GetEmployeeByCompanyId(companyId);
    //},
    
    //GetEmployeeByCompanyId: function (companyId) {
    //    var objEmployee = new Object();
    //    $('#cmbEmployee').val("");
    //    objEmployee = userInfoManager.GetEmployeeByCompanyId(companyId);
        
    //    $("#cmbEmployee").kendoComboBox({
    //        placeholder: "Select Employee...",
    //        dataTextField: "FullName",
    //        dataValueField: "HRRecordId",
    //        dataSource: objEmployee
    //    });
    //},
    //GetEmployeeByCompanyIdInUser: function (companyId) {
    //    var objEmployee = new Object();
    //    objEmployee = userInfoManager.GetEmployeeByCompanyIdInUser(companyId);

    //    $("#cmbEmployee").kendoComboBox({
    //        placeholder: "Select Employee...",
    //        dataTextField: "FullName",
    //        dataValueField: "HRRecordId",
    //        dataSource: objEmployee
    //    });
    //},
    
    clearUserInfoForm: function () {
        $("#hdnUserId").val("0");
        //$("#cmbCompanyName").val("");
        //userInfoHelper.populateCompany();
        $("#txtLoginId").val("");
        $("#txtNewPassword").val("");
        $("#txtConfirmPassword").val("");
        $("#txtUserName").val("");
        $("#txtEmail").val("");
        $("#lblCompanyName").html("");
        $("#hdnCompanyId").val("0");

        $("#lblBranchName").html("");
        $("#hdnBranchId").val("0");
      // $("#cmbEmployee").val("");
        
        //var branch = $("#cmbBranchDetails").data("kendoComboBox");
        //branch.destroy();

        //empressCommonHelper.GenerateBranchCombo(CurrentUser.CompanyId, "cmbBranchDetails");
        

        //var department = $("#cmbDepartmentNameDetails").data("kendoComboBox");
        //department.destroy();
       // userInfoHelper.GetDepartmentByCompanyId(CurrentUser.CompanyId);
        
        //var combobox = $("#cmbEmployee").data("kendoComboBox");
        //combobox.destroy();
        //userInfoHelper.GenerateEmployeeByCompanyId(CurrentUser.CompanyId, CurrentUser.BranchId, 0);
        
        $('.chkBox').attr('checked', false);
        
        $("#divUserInfo > form").kendoValidator();
        $("#divUserInfo").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");

    },
    
    ValidateUserInfoForm: function () {
        var data = [];
        
        var validator = $("#divUserInfo").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            //var companyId = $("#cmbCompanyNameDetails").val();
            //var comboboxforCompany = $("#cmbCompanyNameDetails").data("kendoComboBox");
            //var companyName = comboboxforCompany.text();
            //if (companyId == companyName) {
            //    status.text("Oops! Company Name is invalid.").addClass("invalid");
            //    $("#cmbCompanyName").val("");
            //    userInfoHelper.GenerateMotherCompanyCombo();
            //    return false;
            //}
           var companyId = $("#hdnCompanyId").val();
            
           var branchId = $("#hdnBranchId").val();
           if (companyId == "0" || branchId == "0") {
               status.text("Oops! Please Select Company & Branch from Organogram").addClass("invalid");
               
               return false;
            }
            var userId = $("#hdnUserId").val();
            if(userId =="0")
            {
                if ($("#txtNewPassword").val() == "") {
                    
                }
                if ($("#txtConfirmPassword").val() == "") {
                    status.text("Oops! Please Insert Confirm Password.").addClass("invalid");
                    $("#txtConfirmPassword").val("");
                    return false;
                }
                if ($("#txtNewPassword").val() != $("#txtConfirmPassword").val()) {
                    status.text("Oops! Password and Confirm password not match.").addClass("invalid");
                    $("#txtNewPassword").val("");
                    $("#txtConfirmPassword").val("");
                    return false;
                }
            }
            
            //var employeeId = $("#cmbEmployee").val();
            //var comboboxforEmployee = $("#cmbEmployee").data("kendoComboBox");
            //var employeeName = comboboxforEmployee.text();
            //if (employeeId == employeeName) {
            //    status.text("Oops! Employee Name is invalid.").addClass("invalid");
            //    $("#cmbEmployee").val("");
            //    userInfoHelper.GetEmployeeByCompanyId(companyId);
            //    return false;
            //}

            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
    
    CreateUserInformationForSaveData: function () {
        var objUser = new Object();
        objUser.UserId = $("#hdnUserId").val();
        //objUser.CompanyId = $("#cmbCompanyNameDetails").val();
        objUser.CompanyId = $("#hdnCompanyId").val();
        //objUser.BranchId = $("#cmbBranchDetails").val();
        objUser.BranchId = $("#hdnBranchId").val();
        objUser.LoginId = $("#txtLoginId").val();
        objUser.UserName = $("#txtUserName").val();
        //objUser.EmailAddress = $("#txtEmail").val();
        // objUser.EmployeeId = 0;//$("#cmbEmployee").val();
        objUser.Password = $("#txtNewPassword").val();
        
        if ($("#chkIsActive").is(':checked') == true) {
            objUser.IsActive = true;
        }
        else {
            objUser.IsActive = false;
        }

        objUser.IsNotify = $("#chkIsNotify").is(":checked") == true ? 1 : 0;
        return objUser;
    },
    fillCompanyAndBranch: function (companyId, companyName, branchId, branchName) {
        $("#lblCompanyName").html("");
        $("#hdnCompanyId").val("0");

        $("#lblBranchName").html("");
        $("#hdnBranchId").val("0");
        

        $("#lblCompanyName").html(companyName);
        $("#hdnCompanyId").val(companyId);

        $("#lblBranchName").html(branchName);
        $("#hdnBranchId").val(branchId);
    },
    
    ClearFields:function() {
        $("#lblCompanyName").html("");
        $("#hdnCompanyId").val("0");

        $("#lblBranchName").html("");
        $("#hdnBranchId").val("0");

    },
    populateUserInformationDetails: function (objUser) {
      
        userInfoHelper.clearUserInfoForm();
        $("#hdnUserId").val(objUser.UserId);
        
        //var company = $("#cmbCompanyNameDetails").data("kendoComboBox");
        //company.value(objUser.CompanyId);
        
        //var branch = $("#cmbBranchDetails").data("kendoComboBox");
        //branch.destroy();


        //empressCommonHelper.GenerateBranchCombo(objUser.CompanyId, "cmbBranchDetails");
        //if(objUser.BranchId!=0) {
        //    branch = $("#cmbBranchDetails").data("kendoComboBox");
        //    branch.value(objUser.BranchId);
        //}
      
        
        //var department = $("#cmbDepartmentNameDetails").data("kendoComboBox");
        //department.destroy();

        //userInfoHelper.GetDepartmentByCompanyId(objUser.CompanyId);
        //if (objUser.DepartmentId != 0) {
        //    department = $("#cmbDepartmentNameDetails").data("kendoComboBox");
        //    department.value(objUser.DepartmentId);
        //}
        //var combobox = $("#cmbEmployee").data("kendoComboBox");
        //combobox.destroy();
        //userInfoHelper.GenerateEmployeeByCompanyId(objUser.CompanyId, objUser.BranchId, objUser.DepartmentId);
        
        //combobox = $("#cmbEmployee").data("kendoComboBox");
        //combobox.value(objUser.EmployeeId);
       
        $("#lblCompanyName").html(objUser.CompanyName);
        $("#hdnCompanyId").val(objUser.CompanyId);
        
        $("#lblBranchName").html(objUser.BranchName);
        $("#hdnBranchId").val(objUser.BranchId);
        
        $("#txtLoginId").val(objUser.LoginId);
        $("#txtUserName").val(objUser.UserName);
        //$("#txtEmail").val(objUser.EmailAddress);
        //$("#txtNewPassword").val(objUser.Password);
        //$("#txtConfirmPassword").val(objUser.Password);
        $('#chkIsActive').attr('checked', objUser.IsActive);
 
        if (objUser.IsNotify == 1) {
            $("#chkIsNotify").prop('checked', 'checked');
        } else {
            $("#chkIsNotify").removeProp('checked', 'checked');
        }
        //var employee = $("#cmbEmployee").data("kendoComboBox");
        //employee.value(objUser.EmployeeId);

        groupMembershipHelper.GetGroupByCompanyId(objUser.CompanyId);
    }
    
    

};