var gbCompanyName = "";
var companyDetailsManager = {

    SaveCompany: function () {
        //===============================================================
        //If the value of
        //isToUpdateOrCreate =0 then ----Create New company
        //isToUpdateOrCreate =1 then ----Update Company Information
        //===============================================================
        var isToUpdateOrCreate = $("#hfCompanyId").val();
        if (companyDetailsHelper.validator()) {
            var objCompany = companyDetailsManager.GetDataFromCortolsAsA_Object();
            var objCompanyInfo = JSON.stringify(objCompany).replace(/&/g, "^");
            //var jsonParam = 'isToDoUpdateOrCreate=' + isToUpdateOrCreate + '&strObjCompanyInfo=' + objCompanyInfo;
            var jsonParam = 'strObjCompanyInfo=' + objCompanyInfo;
            var serviceUrl = "../Company/Create/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
        }

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                companyDetailsHelper.clearCompanyForm();
                if (isToUpdateOrCreate == 0) {
                    AjaxManager.MsgBox('success', 'center', 'Success', 'New Company Saved Successfully.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
                } else {
                    AjaxManager.MsgBox('success', 'center', 'Update', 'Company Information Updated Successfully.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
                }
                $("#gridCompany").data("kendoGrid").dataSource.read();
            }
            else if (jsonData == "Already Exist") {


                AjaxManager.MsgBox('warning', 'center', 'Alresady Exist:', 'Company Name or Company Code already exist.',
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

    GetDataFromCortolsAsA_Object: function () {
        var objCompany = new Object();
        objCompany.CompanyId = $('#hfCompanyId').val();
        objCompany.CompanyCode = $("#txtCompanyCode").val();
        objCompany.CompanyName = $("#txtCompanyName").val();
        objCompany.CompanyType = $("#companyType input[type='radio']:checked").val();
        objCompany.CompanyStock = $("#CompanyStock input[type='radio']:checked").val();
        objCompany.Address = $("#txtAddress").val();
        objCompany.Phone = $("#txtPhone").val();
        objCompany.Fax = $("#txtFax").val();
        objCompany.Email = $("#txtEmail").val();
        objCompany.PrimaryContact = $("#txtPrimaryContact").val();
        objCompany.RootCompanyId = $("#cmbRootCompany").data('kendoComboBox').value();

        var motherId = $("#cmbMotherCompany").val();
        if (motherId == -1 || motherId == "" || objCompany.CompanyId == motherId) {
            //Do nothing the EF will consider as Null vall
        } else {
            objCompany.MotherId = motherId;
        }

        var ddlFiscalYearStart = $("#ddlFiscalYearStart").data("kendoDropDownList");
        objCompany.FiscalYearStart = ddlFiscalYearStart.value();
        objCompany.FullLogoPath = $("#hfFullLogoPath").val();
        objCompany.IsActive = $("#chkIsActive").is(":checked") == true ? 1 : 0;
        return objCompany;
    },

    GetMotherCompany: function (rootCompanyId) {
        var objCompany = "";
        var jsonParam = "rootCompanyId=" + rootCompanyId;
        var serviceUrl = "../Company/GetMotherCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }

        //function onFailed(error) {
        //    window.alert(error.statusText);
        //}
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objCompany;
    },
    GetRootMotherCompany: function () {
        var objRootCompany = "";
        var jsonParam = "";
        var serviceUrl = "../Company/GetRootMotherCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objRootCompany = jsonData;
        }

        //function onFailed(error) {
        //    window.alert(error.statusText);
        //}
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objRootCompany;
    },

    GetMotherCompanyForEditCompanyCombo: function (companyId) {
        var objCompany = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../Company/GetMotherCompanyForEditCompanyCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }

        //function onFailed(error) {
        //    window.alert(error.statusText);
        //}
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objCompany;
    },

    logoUpload: function () {


        $("#files").kendoUpload({
            upload: onUpload,
            multiple: false,
            success: onSuccess,
            error: onError,
            select: onSelect,

            async: {
                saveUrl: "../Company/save",
                removeUrl: "../Company/remove",
                autoUpload: true,
            },
            localization: {
                select: "Browse",
                uploadSelectedFiles: "Upload Banner"
            }
        });

        function onUpload(e) {
            // Array with information about the uploaded files
            var files = e.files;
            if (companyDetailsHelper.validator()) {

               // Check the extension of each file and abort the upload if it is not .jpg
                $.each(files, function () {
 
                    if ((this.extension.toLowerCase() != ".jpg") && (this.extension.toLowerCase() != ".png")) {
                        if (!(Math.ceil(this.size / 1024) <= 500)) {
                            AjaxManager.MsgBox('warning', 'center', 'Warning', "Only .jpg/.png files and up to file size 500 kb can be uploaded as Company logo.",
                            [
                                {
                                    addClass: 'btn btn-primary',
                                    text: 'Ok',
                                    onClick: function ($noty) {
                                        $noty.close();
                                    }
                                }
                            ]);
                        }
                        e.preventDefault();
                    }
                });

            } else {
                var msg = "Please input required company information then upload your comapny banner.";
                AjaxManager.MsgBox('warning', 'center', 'Warning', msg,
                  [
                      {
                          addClass: 'btn btn-primary',
                          text: 'Ok',
                          onClick: function ($noty) {
                              $noty.close();
                          }
                      }
                  ]);

                e.preventDefault();

            }
        }

        function onSuccess(e) {
            // Array with information about the uploaded files

            var files = e.files;
            if (e.operation == "upload") {
                var msg = "";
                if (e.response == "Success") {
                    msg = "Company Banner Successfully uploaded. Thank you.";
                    AjaxManager.MsgBox('success', 'center', 'File Upload', msg,
                    [
                        {
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                            }
                        }
                    ]);
                } else {
                    msg = "Failed to uploaded " + files.length + " files";
                    AjaxManager.MsgBox('warning', 'center', 'Warning', msg,
                    [
                        {
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                            }
                        }
                    ]);
                }


                e.preventDefault();
            }
        }
        function onError(e) {
            // Array with information about the uploaded files
            var files = e.files;

            if (e.operation == "upload") {
                var msg = "Failed to uploaded " + files.length + " files";

                AjaxManager.MsgBox('error', 'center', 'Error', msg,
                [
                    {
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }
                ]);
            }
        }
        function onSelect(e) {
          
        }

    }
};

var companyDetailsHelper = {

    validator: function () {
        var data = [];
        var validator = $("#companyDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }

    },

    GenerateFiscalYearCombo: function () {
        var data = [
            { text: "January", value: "1" },
            { text: "February", value: "2" },
            { text: "March", value: "3" },
            { text: "April", value: "4" },
            { text: "May", value: "5" },
            { text: "June", value: "6" },
            { text: "July", value: "7" },
            { text: "August", value: "8" },
            { text: "September", value: "9" },
            { text: "October", value: "10" },
            { text: "November", value: "11" },
            { text: "December", value: "12" }
        ];

        $("#ddlFiscalYearStart").kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: data
        });
    },

    GenerateMotherCompanyCombo: function (rootCompanyId) {
        var objCompany = new Object();
        objCompany = companyDetailsManager.GetMotherCompany(rootCompanyId);

        $("#cmbMotherCompany").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany,
            suggest: true,
            change: function () {

                var value = this.value();
                AjaxManager.isValidItem("cmbMotherCompany", true);
            }
        });

    },
    GenerateRootCompanyCombo: function () {
        var objRootCompany = new Object();
        objRootCompany = companyDetailsManager.GetRootMotherCompany();

        $("#cmbRootCompany").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objRootCompany,
            suggest: true,
            change: function () {

                var value = this.value();
                AjaxManager.isValidItem("cmbRootCompany", true);
                var rootCompanyId = $("#cmbRootCompany").data('kendoComboBox').value();
                companyDetailsHelper.GenerateMotherCompanyCombo(rootCompanyId);
            }
        });

    },

    GetMotherCompanyForEditCompanyCombo: function (companyId) {
        var objCompany = new Object();
        objCompany = companyDetailsManager.GetMotherCompanyForEditCompanyCombo(companyId);

        $("#cmbMotherCompany").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany,
            suggest: true,
            change: function () {

                var value = this.value();
                AjaxManager.isValidItem("cmbMotherCompany");
            }
        });

    },

    clearCompanyForm: function () {

        $('#hfCompanyId').val('0');
        $('#txtCompanyCode').val('');
        $('#txtCompanyName').val('');
        // $('#cmbMotherCompany').val('0');
        $('#txtAddress').val('');
        $('#txtPhone').val('');
        $('#txtFax').val('');
        $('#txtEmail').val('');
        $('#txtPrimaryContact').val('');
        
        $('input:radio[name=radio]').attr('checked', false);
        // $('#ddlFiscalYearStart').val('');
        // $('#fBannerFileUploade').val('1');
        var cmbMotherCompany = $("#cmbMotherCompany").data("kendoComboBox");
        cmbMotherCompany.value('');
        var combobox = $("#cmbMotherCompany").data("kendoComboBox");
        combobox.destroy();
        
        
        //var rootCombobox = $("#cmbRootCompany").data("kendoComboBox");
        //rootCombobox.destroy();
        
        companyDetailsHelper.GenerateMotherCompanyCombo(0);

        var ddlFiscalYearStart = $("#ddlFiscalYearStart").data("kendoDropDownList");
        ddlFiscalYearStart.value(0);

        $("#hfFullLogoPath").val('');

        $("#companyDetailsDiv > form").kendoValidator();
        $("#companyDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");


    },
    
    changeEventOfCompanyType:function () {
      
        var val = $("#companyType input[type='radio']:checked").val();
        var prevValue = $("#txtCompanyName").val();
        var split = prevValue.split('(');
        var newValue = split[0] + "(" + val + ")";
        $("#txtCompanyName").val(newValue);
    },

};