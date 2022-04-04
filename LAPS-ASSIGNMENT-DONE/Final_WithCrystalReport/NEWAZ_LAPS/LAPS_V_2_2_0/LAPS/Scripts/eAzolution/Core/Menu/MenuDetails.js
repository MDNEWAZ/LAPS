

var menuDetailsManager = {
    
    SaveMenuInformation: function () {
        if (menuDetailsHelper.validator()) {

            var objMenu = menuDetailsHelper.CreateMenuInformationForSaveData();

            var objMenuInfo = JSON.stringify(objMenu).replace(/&/g, "^");
            var jsonParam = 'strobjMenuInfo=' + objMenuInfo;
            var serviceUrl = "../Menu/SaveMenu/";
            AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);

        }
        function onSuccess(jsonData) {
            
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Menu Saved Successfully',
                   [{
                       addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                           $noty.close();
                           menuDetailsHelper.clearMenuForm();
                           $("#gridMenu").data("kendoGrid").dataSource.read();
                           $("#gridMenuSorting").data("kendoGrid").dataSource.read();
                       }
                   }]);
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Failed', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Failed', error.statusText,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
        }
    },
    
    GetAllModule: function () {
        var jsonParam = "";
        var serviceUrl = "../Module/SelectModule/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        
        function onSuccess(jsonData) {
            
            var objModuleData = new Object();
            objModuleData = jsonData;
            $("#cmbModuleName").kendoComboBox({
                placeholder: "Select Module...",
                dataTextField: "ModuleName",
                dataValueField: "ModuleId",
                dataSource: objModuleData
            });
        }
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
    },
    
    GetAllMenuByModuleId: function (moduleId) {
        var jsonParam = "moduleId=" + moduleId;
        var serviceUrl = "../Menu/GetMenuByModuleId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            var objmenuData = new Object();
            objmenuData = jsonData;
            $("#cmbParentMennu").kendoComboBox({
                placeholder: "Select Parent Menu...",
                dataTextField: "MenuName",
                dataValueField: "MenuId",
                dataSource: objmenuData,
                change: function () {

                    if (this.value() == this.text()) {
                        this.value('');

                    }  
                }
            });
        }
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
    },

};

var menuDetailsHelper = {
    
    validator: function () {
        var data = [];
        var validator = $("#menuDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            
            var moduleId = $("#cmbModuleName").val();
            var comboboxforModule = $("#cmbModuleName").data("kendoComboBox");
            var moduleName = comboboxforModule.text();
            if (moduleId == moduleName) {
                status.text("Oops! Module Name is invalid.").addClass("invalid");
                $("#cmbModuleName").val("");
                return false;
            }

            var chkspMnName = AjaxManager.checkSpecialCharacters("txtMenuName");
            if(!chkspMnName) {
                status.text("Oops! There is invalid data in the form.").addClass("invalid");
                return false;
            }
           

            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
    
    clearMenuForm: function () {
        $("#hdMenuId").val("0");
        $("#hdSorOrder").val("0");
        $("#cmbModuleName").val("");
        menuDetailsManager.GetAllModule();
        $("#txtMenuName").val("");
        $("#txtMenuPath").val("");
        $("#cmbParentMennu").val("");
        //menuDetailsManager.GetAllMenu();
        $('.chkBox').attr('checked', false);
        
        var combobox = $("#cmbParentMennu").data("kendoComboBox");
        combobox.destroy();
        
        menuDetailsManager.GetAllMenuByModuleId(0);
        
        $("#menuDetailsDiv > form").kendoValidator();
        $("#menuDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");

    },
    
    CreateMenuInformationForSaveData: function () {
        
        var objMenu = new Object();
        objMenu.MenuId = $("#hdMenuId").val();
        objMenu.MenuName = $("#txtMenuName").val();
        objMenu.ModuleId = $("#cmbModuleName").val();
        objMenu.MenuPath = $("#txtMenuPath").val();
        //var perMnu = $("#cmbParentMennu").val();

        var cmbParentMenu = $("#cmbParentMennu").data('kendoComboBox');
        objMenu.ParentMenu = cmbParentMenu.value();

       
       
        if (objMenu.ParentMenu == "") {
            objMenu.ParentMenu = 0;
        }
        
        if ($("#chkIsQuickLink").is(':checked') == true) {
            objMenu.ToDo = 1;
        }
        else {
            objMenu.ToDo = 0;
        }
        
        objMenu.SortOrder = $("#hdSorOrder").val();

        return objMenu;
    },
    
    FillMenuDetailsInForm: function (objMenu) {

        menuDetailsHelper.clearMenuForm();

        var comboboxForParentMenu = $("#cmbParentMennu").data("kendoComboBox");
        comboboxForParentMenu.destroy();

        $('#hdMenuId').val(objMenu.MenuId);
        $('#hdSorOrder').val(objMenu.SortOrder);
        $("#txtMenuName").val(objMenu.MenuName);
        
        var cmbModule = $("#cmbModuleName").data("kendoComboBox");
        if (objMenu.ModuleId != 0) {
            cmbModule.value(objMenu.ModuleId);
            //var combobox = $("#cmbParentMennu").data("kendoComboBox");
            //combobox.destroy();
            menuDetailsManager.GetAllMenuByModuleId(objMenu.ModuleId);
        }
        
       

        $("#txtMenuPath").val(objMenu.MenuPath);
        
        var cmbParentMenu = $("#cmbParentMennu").data("kendoComboBox");
        if (objMenu.ParentMenu != 0) {
            cmbParentMenu.value(objMenu.ParentMenu);
        }
        
        if (objMenu.ToDo == 1) {
            $('#chkIsQuickLink').attr('checked', true);
        }
        else {
            $('#chkIsQuickLink').attr('checked', false);
        }

    },
    
    changeModuleName: function () {
        var moduleId = $("#cmbModuleName").val();
        var comboboxforModule = $("#cmbModuleName").data("kendoComboBox");
        var moduleName = comboboxforModule.text();
        if (moduleId == moduleName) {
            return false;
        }
        var combobox = $("#cmbParentMennu").data("kendoComboBox");
        combobox.value("");
        combobox.destroy();
        menuDetailsManager.GetAllMenuByModuleId(moduleId);
    }


};
