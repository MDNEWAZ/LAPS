var menuArray = [];
var allmenuArray = [];
var gbmoduleId = 0;
var menuPermisionManager = {
    
    RenderMenuByModuleId: function (moduleId) {
        var objMenu = "";
        var jsonParam = "moduleId=" + moduleId;
        var serviceUrl = "../Menu/GetMenuByModuleId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false,false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objMenu = jsonData;
            allmenuArray = [];
            for(var i=0; i<objMenu.length; i++) {
                allmenuArray.push(objMenu[i]);
            }
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objMenu;
        
    }
};

var menuPermisionHelper = {
    
    PopulateMenuTreeByModuleId: function (moduleId) {
        
        $("#treeview").remove();

        $('#menuContent').html('<div id="treeview"></div>');
        var objMenuList = new Object();
        var newMenuArray = [];
        objMenuList = menuPermisionManager.RenderMenuByModuleId(moduleId);
        gbmoduleId = moduleId;
        var treeview = $("#treeview").kendoTreeView({
            checkboxes: {
                checkChildren: true,
                //template: "<input type='checkbox' name='checkedFiles#= item.id #' id='chkMenu#= item.id #' onclick='menuPermisionHelper.onSelect(#= item.id #,Event)' />"
                template: "<input type='checkbox' id='chkMenu#= item.id #' onclick='menuPermisionHelper.onSelect(#= item.id #,event)' />"
            },
            select: menuPermisionHelper.changeMenu,
            dataSource: {},
        }).data("kendoTreeView");

        treeview.remove();


        var chiledMenuArray = [];
        
        for (var i = 0; i < objMenuList.length; i++) {
            
            if (objMenuList[i].ParentMenu == null || objMenuList[i].ParentMenu == 0) {
                chiledMenuArray = [];
                var objMenu = new Object();
                objMenu.id = objMenuList[i].MenuId;
                objMenu.itemId = objMenuList[i].MenuId;
                objMenu.text = objMenuList[i].MenuName;
                objMenu.value = objMenuList[i].MenuId;
                objMenu.items = chiledMenuArray;
                objMenu.items = menuPermisionHelper.chiledMenu(objMenu, objMenuList[i].MenuId, objMenuList);
                if (objMenu.items.length > 0) {
                    objMenu.expanded = true;
                    //objMenu.spriteCssClass = "folder"
                    //    ;
                }
                else {
                    objMenu.spriteCssClass = "html";
                    objMenu.items = [];
                    
                }
                objMenu.itemValue = objMenuList[i].MenuId;
                newMenuArray.push(objMenu);
                
            }
        }

        var dataSource = new kendo.data.HierarchicalDataSource({
            data: newMenuArray
        });

        $("#treeview").data("kendoTreeView").setDataSource(dataSource);
        
        menuPermisionHelper.autoSelectExistingMenu();

            
    },
    onSelect:function (menuId,e) {

        
        
        if ($("#chkMenu" + menuId).is(':checked') == true) {
            var alreadyadded = menuPermisionHelper.checkAlreadyAddedthisMenu(menuId);
            if (alreadyadded == false) {
                menuPermisionHelper.createMenuArray(menuId);
                
            }
            
            //Parent Menu Id Add in Array
            for(var p=0;p<allmenuArray.length;p++) {
                if (allmenuArray[p].MenuId == menuId) {
                    if (allmenuArray[p].ParentMenu != null) {
                        alreadyadded = menuPermisionHelper.checkAlreadyAddedthisMenu(allmenuArray[p].ParentMenu);
                        if (alreadyadded == false) {
                            menuPermisionHelper.createMenuArray(allmenuArray[p].ParentMenu);
                        }
                    }
                    break;
                        
                }
            }
            
            //Chiled Menu Add in Array

            menuPermisionHelper.checkChiledMenuArray(menuId);
            stateHelper.GetStatusByMenuId(menuId);
            


        }
        else {
            for (var j = 0; j < menuArray.length; j++) {
                if (menuArray[j].ReferenceID == menuId) {
                    stateHelper.RemoveStatusByMenuId(menuId);

                    menuArray.splice(j, 1);
                }
            }
            
            //Remove chiled Menu
            menuPermisionHelper.removeChiledMenuArray(menuId);
            

        }

    },
    
    changeMenu: function (e) {
        
        var menuName = e.node.textContent.trim();
        var menuId = 0;
        for (var i = 0; i < allmenuArray.length; i++) {
            if (menuName == allmenuArray[i].MenuName.trim()) {
                menuId = allmenuArray[i].MenuId;
            }
        }

        menuPermisionHelper.onSelect(menuId);
    },
    
    checkChiledMenuArray: function (menuId) {
        for (var ch = 0; ch < allmenuArray.length; ch++) {
            if (allmenuArray[ch].ParentMenu == menuId) {
                var alreadyadded = menuPermisionHelper.checkAlreadyAddedthisMenu(allmenuArray[ch].MenuId);
                if (alreadyadded == false) {
                    menuPermisionHelper.createMenuArray(allmenuArray[ch].MenuId);
                    menuPermisionHelper.checkChiledMenuArray(allmenuArray[ch].MenuId);
                }
            }
        }
    },
    
    removeChiledMenuArray: function (menuId) {
        for (var r = 0; r < allmenuArray.length; r++) {
            if (allmenuArray[r].ParentMenu == menuId) {
                for (var cr = 0; cr < menuArray.length; cr++) {
                    if (menuArray[cr].ReferenceID == allmenuArray[r].MenuId) {
                        
                        stateHelper.RemoveStatusByMenuId(menuArray[cr].ReferenceID);
                        menuArray.splice(cr, 1);
                        menuPermisionHelper.removeChiledMenuArray(allmenuArray[r].MenuId);
                    }
                }
            }
        }
    },
    
    createMenuArray: function (menuId) {
        
        var module = $("#cmbApplicationForModule").data("kendoComboBox");
        var obj = new Object();
        obj.ReferenceID = menuId;
        obj.ParentPermission = gbmoduleId;
        obj.PermissionTableName = "Menu";
        menuArray.push(obj);
    },

    checkAlreadyAddedthisMenu: function (menuId) {
        var alreadyadded = false;
        for (var i = 0; i < menuArray.length; i++) {
            if (menuArray[i].ReferenceID == menuId) {
                alreadyadded = true;
                break;
            }
        }
        return alreadyadded;
    },
    
    autoSelectExistingMenu: function () {
        
        for(var i=0; i<allmenuArray.length; i++) {
            for(var j=0; j<menuArray.length;j++) {
                if(allmenuArray[i].MenuId == menuArray[j].ReferenceID) {
                    $('#chkMenu' + menuArray[j].ReferenceID).attr('checked', true);
                    break;
                }
            }
        }

    },

    clearMenuPermision: function () {
        menuArray = [];
        gbmoduleId = 0;
        menuPermisionHelper.PopulateMenuTreeByModuleId(0);
    },

    chiledMenu: function (objMenuOrginal, menuId, objMenuList) {
        
        var chiledMenuArray = [];
        var newMenuArray = [];
        for (var j = 0; j < objMenuList.length; j++) {
            if (objMenuList[j].ParentMenu == menuId) {
                var objMenu = new Object();
                objMenu = objMenuOrginal;
                var objChiledMenu = new Object();
                objChiledMenu.id = objMenuList[j].MenuId;
                objChiledMenu.itemId = objMenuList[j].MenuId;
                objChiledMenu.text = objMenuList[j].MenuName;
                objChiledMenu.itemValue = objMenuList[j].MenuId;
                //objChiledMenu.spriteCssClass = "html";
                chiledMenuArray = objMenuOrginal.items;
                if(chiledMenuArray == undefined || chiledMenuArray.length==0) {
                    chiledMenuArray = [];
                }
                else {
                    objChiledMenu.expanded = true;
                    //objChiledMenu.spriteCssClass = "folder";
                }
                newMenuArray = menuPermisionHelper.chiledMenu(objChiledMenu, objMenuList[j].MenuId, objMenuList);
                chiledMenuArray.push(objChiledMenu);
                objMenu.items = chiledMenuArray;
            }
        }
        return chiledMenuArray;
    },
    
    CreateMenuPermision: function (objGroup) {
        
        objGroup.MenuList = menuArray;
        return objGroup;
    },
    
    PopulateExistingMenuInArray: function (objGroupPermision) {
        menuArray = [];
        for (var i = 0; i < objGroupPermision.length; i++) {
            if (objGroupPermision[i].PermissionTableName == "Menu") {
                var obj = new Object();
                obj.ReferenceID = objGroupPermision[i].ReferenceID;
                obj.ParentPermission = objGroupPermision[i].ParentPermission;
                obj.PermissionTableName = "Menu";
                menuArray.push(obj);
            }
        }
    },
    
    DeleteFormMenuByModuleId: function (moduleId) {
        $("#treeview").remove();

        $('#menuContent').html('<div id="treeview"></div>');
        for (var i = 0; i < menuArray.length; i++) {
            if (menuArray[i].ParentPermission == moduleId) {
                stateHelper.RemoveStatusByMenuId(menuArray[i].ReferenceID);
                menuArray.splice(i, 1);
                i = i - 1;
            }
        }
    },
    
    

};