
var mnManager = {

    getMenu: function () {
        var objMenuList = "";

        var pathName = window.location.pathname;
        var pageName = pathName.substring(pathName.lastIndexOf('/') + 1);
        var serviceURL = "../Menu/SelectMenuByUserPermission/";



        var jsonParam = "";// "moduleId=" + moduleId;
        AjaxManager.GetJsonResult(serviceURL, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objMenuList = jsonData;

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objMenuList;
    },
    getParentMenuByMenu: function (parentMenuId) {
        var objParentMenuList = "";

        var serviceURL = "../Menu/GetParentMenuByMenu/";



        var jsonParam = "parentMenuId=" + parentMenuId;
        AjaxManager.GetJsonResult(serviceURL, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objParentMenuList = jsonData;

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objParentMenuList;
    },
    getCurrentUser: function (menuRefresh) {
        var jsonParam = '';
        var pathName = window.location.pathname;
        var pageName = pathName.substring(pathName.lastIndexOf('/') + 1);
        var serviceURL = "../Home/GetCurrentUser";
        //if (pageName.toLowerCase() == "home.mvc") {
        //    serviceURL = "../Home/GetCurrentUser";
        //}
        //else {
        //    serviceURL = "../Home/GetCurrentUser";
        //}

        //AjaxManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);

        AjaxManager.GetJsonResult(serviceURL, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {

            CurrentUser = jsonData;
            if (CurrentUser != undefined) {

                var userName = "Welcome " + CurrentUser.UserName;
                $("#lblWelcome").html(userName);
                if (CurrentUser.FullLogoPath != null) {
                    $("#headerLogo").attr('style', 'background-image: url("' + CurrentUser.FullLogoPath + '") !important');
                }
            }

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    Logoff: function () {
        var serviceURL = "../Home/Logoff";
        window.location.href = serviceURL;
    }
};

var mnHelper = {
    //initMenu: function () {
    //    //$("#menu").bind("click", function(e) {
    //    //    debugger;
    //    //    var item = $("#menu").data('kendoMenu');

    //    //});




    //    //$("#menu a").click(function (e) {
    //    //    $(".k-state-selected", this.element).removeClass("k-state-selected");
    //    //    $(".ob-selected-ancestor").removeClass("ob-selected-ancestor");

    //    //    // Select item
    //    //    $(e.item).addClass("k-state-selected");
    //    //    $(".k-state-active", this.element).addClass("ob-selected-ancestor");
    //    //});

    //    //$("#menu").click(function (e) {
    //    //    debugger;
    //    //    var items = $(".k-state-active", this.element);

    //    //    //$(".k-state-selected", this.element).removeClass("k-state-selected");
    //    //    //$(".ob-selected-ancestor").removeClass("ob-selected-ancestor");

    //    //    //// Select item
    //    //    //$(e.item).addClass("k-state-selected");
    //    //    //$(".k-state-active", this.element).addClass("ob-selected-ancestor");
    //    //});
    //},

    GetMenuInformation: function () {
        var objMenuList = mnManager.getMenu();
        mnHelper.populateMenus(objMenuList);
        mnManager.getCurrentUser(true);
    },
    youLogedInAs: function () {
        var jsonParam = '';

        var serviceURL = "../Home/GetUserTypeByUserId";

        AjaxManager.GetJsonResult(serviceURL, jsonParam, false, false, onSuccess, onFailed);
        //AjaxManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {

            var logedInAs = "";
            var groupName = "";
            if (jsonData != undefined) {
                for (var i = 0; i < jsonData.length; i++) {
                    groupName += " " + jsonData[i].GroupName + " &";
                }
                var splitlogedInAs = groupName.slice(0, -1);
                logedInAs = "| You logged in as " + splitlogedInAs;
                $("#lblLogedinAs").html(logedInAs);

            }

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },
    populateMenus: function (menus) {
        var dynamicmenuArray = [];
        var chiledMenuArray = [];
        var parentMenubyMenuId = [];
        var pathName = window.location.pathname;

        for (var j = 0; j < menus.length; j++) {
            if (menus[j].MenuPath == ".." + pathName) {
                parentMenubyMenuId = mnManager.getParentMenuByMenu(menus[j].ParentMenu);//This parent menu is to make active css to parent menu
            }
        }

        var menulink = "";

        for (var i = 0; i < menus.length; i++) {
            var haveparentMenu = 0;
            if (menus[i].ParentMenu == null || menus[i].ParentMenu == 0) {
                for (var k = 0; k < parentMenubyMenuId.length; k++) {
                    if (parentMenubyMenuId[k].MenuId == menus[i].MenuId) {
                        haveparentMenu = 1;
                    }
                }
                if (haveparentMenu == 1) {
                    menulink += "<li class='ob-selected-ancestor'>";
                } else {
                    menulink += "<li>";
                }

                if (menus[i].MenuPath == null || menus[i].MenuPath == "") {
                    menulink += menus[i].MenuName;
                }
                else {
                    menulink += "<a href='" + menus[i].MenuPath + "'>" + menus[i].MenuName + "</a>";
                }
                menulink += mnHelper.addchiledMenu(menus[i], menus[i].MenuId, menus, parentMenubyMenuId);

                menulink += "</li>";


            }
        }
       

        $("#menu").kendoMenu({
            //closeOnClick: false
        });
        var menu = $("#menu").data("kendoMenu");
        menu.append(menulink);
        $("#menu").kendoMenu({

        });

    },
    addchiledMenu: function (objMenuOrginal, menuId, objMenuList, parentMenubyMenuId) {


        var menulink = "<ul>";
        var added = false;
        var haveChildsParentMenu = 0;
        for (var j = 0; j < objMenuList.length; j++) {
            if (objMenuList[j].ParentMenu == menuId) {
                //if (parentMenubyMenuId != undefined) {
                //    for (var k = 0; k < parentMenubyMenuId.length; k++) {
                //        if (parentMenubyMenuId[k].MenuId == objMenuList[j].MenuId) {
                //            haveChildsParentMenu = 1;
                //        }
                //    }
                //}

                //if (haveChildsParentMenu == 1) {
                //    menulink += "<li id=" + objMenuList[j].MenuName + " class='ob-selected-ancestor'>";
                //}
                //else {
                //    menulink += "<li id=" + objMenuList[j].MenuName + " >";
                //}
                menulink += "<li id=" + objMenuList[j].MenuName + " >";
                if (objMenuList[j].MenuPath == null || objMenuList[j].MenuPath == "") {
                    //menulink += objMenuList[j].MenuName;

                    menulink += "<a href='#'>" + objMenuList[j].MenuName + "</a>";
                }
                else {
                    menulink += "<a href='" + objMenuList[j].MenuPath + "'>" + objMenuList[j].MenuName + "</a>";
                }
                menulink += mnHelper.addchiledMenu(objMenuList[j], objMenuList[j].MenuId, objMenuList);
                menulink += "</li>";
                added = true;
            }
        }
        menulink += "</ul>";
        if (added == false) {
            menulink = "";
        }

        return menulink;
    }
};