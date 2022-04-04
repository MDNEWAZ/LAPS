/// <reference path="../../jquery-1.7.1.min.js" />
/// <reference path="postify.js" />
/// <reference path="../../json2.js" />
/// <reference path="../../../UIFramework/MessageBox/js/noty/jquery.noty.js" />


var LoggedInUserName = '';
var serviceRoot = "..";
var CurrentUser = null;
var AjaxManager = {
    getMultiDateFormat: function () {
        return ['dd.MM.yyyy', 'dd.MM.yy', 'dd/MM/yyyy', 'dd/MM/yy', 'ddMMyyyy', 'ddMMyy'];

    },
    getMultiMonthFormat: function () {
        return ['MM.yyyy', 'MMM yyyy', 'MMM yy', 'MMyyyy', 'MMMyyyy', 'MMyy', 'MMMyy'];

    },

    isValidDate: function (ctrlId) {
        var res = false;
        var dateTo = $("#"+ctrlId).val();
        if (!AjaxManager.isDate(dateTo)) {
            AjaxManager.MsgBox('warning', 'center', 'Invalid Date', 'Invalid Date. e.g.: MM/dd/yyyy', [{
                addClass: 'btn btn-primary',
                text: 'Ok',
                onClick: function ($noty) {
                    $noty.close();
                    $("#" + ctrlId).val('');
                    $("#" + ctrlId).focus();
                }
            }
            ]);
            res = false;
        } else {
            res = true;
        }
        return res;
    },



    //isValidItem: function (ctrlId, isClear) {
    //    var cmbBox = $("#" + ctrlId).data("kendoComboBox");

    //    if (cmbBox.value() != "" && cmbBox.value() == cmbBox.text()) {
    //        AjaxManager.MsgBox('warning', 'center', 'Invalid Item:', 'No Item matched with your Input data as like "[' + cmbBox.text() + ']"!', [{
    //            addClass: 'btn btn-primary',
    //            text: 'Ok',
    //            onClick: function ($noty) {
    //                $noty.close();
    //                cmbBox.focus();
    //                if (isClear)
    //                    cmbBox.value('');


    //            }
    //        }
    //        ]);
    //        return false;
    //    } else {
    //        return true;
    //    }
    //},
    //MVC call

    //    function getCredentials() {
    //var paramList = "";
    //$.ajax({
    //    type: "POST",
    //    async: false,
    //    url: "Company/returnJsonData",
    //    data: "{}",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (msg) {
    //        var data = msg;
    //        debugger;
    //        alert(data);
    //        paramList = "{'CompanyName':'" + data[0].CompanyName + "','CompanyId':'" + data[0].CompanyId + "'}";
    //    },
    //    error: function (jqXHR, textStatus, errorThrown) {
    //        alert("get session failed " + errorThrown);
    //    }
    //});
    //alert("returnValue"+paramList);
    //return paramList;
    //};


    //MVC call
    GetJsonResult: function (serviceUrl, jsonParams, isAsync, isCache, successCallback, errorCallback) {
        $.ajax({
            type: "GET",
            async: isAsync,
            cache: isCache,
            url: serviceUrl,
            data: jsonParams,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: successCallback,
            error: errorCallback
        });
    },

    GetDataSource: function (serviceUrl, jsonParams) {
        var objResult = new Object();
        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: serviceUrl,
            data: jsonParams,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(jsonResult) {
                objResult = jsonResult;
            },
            error: function (error) {
                window.alert(error.statusText);
            }
        });

        return objResult;
    },

    GetJson: function (serviceUrl, jsonParams, successCallback, errorCallback) {
        jQuery.ajax({
            url: serviceUrl,
            data: jsonParams,
            type: "POST",
            processData: true,
            contentType: "application/json",
            dataType: "json",
            success: successCallback,
            error: errorCallback
        });
    },
    SendJson: function (serviceUrl, jsonParams, successCallback, errorCallback) {
        jQuery.ajax({
            url: serviceUrl,
            data: jsonParams,
            async: true,
            cache: false,
            type: "POST",
            success: successCallback,
            error: errorCallback
        });
    },
    
    //SendJson: function (serviceUrl, jsonParams, successCallback, errorCallback) {
    //    jQuery.ajax({
    //        url: serviceUrl,
    //        type: "POST",
    //        data: jsonParams,
    //        contentType: 'application/json',
    //        success: successCallback,
    //        error: errorCallback
            

            
    //    });
    //},
    SendJson2: function (serviceUrl, jsonParams, successCallback, errorCallback) {
        jQuery.ajax({
            url: serviceUrl,
            async: false,
            type: "POST",
            data: "{"+jsonParams+"}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: successCallback,
            error: errorCallback
        });
    },
    GetReport: function (serviceUrl, jsonParams, errorCallback) {
        
        jQuery.ajax({
            url: serviceUrl,
            async: false,
            type: "POST",
            data: "{" + jsonParams + "}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                window.open('../Reports/CReportViewer.aspx', '_newtab');
                //window.open("../Reports/ReportViewer.aspx", 'mywindow', 'fullscreen=yes, scrollbars=auto',);
            },
            error: errorCallback
        });

    },
    OpenAReport: function (serviceUrl, jsonParams, errorCallback) {

        jQuery.ajax({
            url: serviceUrl,
            async: false,
            type: "POST",
            data: "{" + jsonParams + "}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                window.open('../Reports/CReportViewer.aspx', '_newtab');
                //window.open("../Reports/ReportViewer.aspx", 'mywindow', 'fullscreen=yes, scrollbars=auto',);
            },
            error: errorCallback
        });
    },



    //Custome Message Box designed By Mazhar
    //======================================================================================
    MsgBox: function (messageBoxType,displayPosition, messageBoxHeaderText, messageText, buttonsArray) {
        var n = noty({
            textHeader: messageBoxHeaderText,
            text: messageText,
            type: messageBoxType,
            modal: true,
            dismissQueue: true,
            layout: displayPosition,
            theme: 'defaultTheme',
            buttons: buttonsArray
            //buttons: [
            //  {
            //      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
            //          $noty.close();
            //         // noty({ dismissQueue: false, force: false, layout: layout, theme: 'defaultTheme', text: 'You clicked "Ok" button', type: 'success' });
            //      }
            //  },
            //  {
            //      addClass: 'btn btn-danger', text: 'Cancel', onClick: function ($noty) {
            //          $noty.close();
            //         // noty({ dismissQueue: false, force: false, layout: layout, theme: 'defaultTheme', text: 'You clicked "Cancel" button', type: 'error' });
            //      }
            //  },
            //    {
            //        addClass: 'btn btn-danger', text: 'Test', onClick: function ($noty) {
            //            $noty.close();
            //            // noty({ dismissQueue: false, force: false, layout: layout, theme: 'defaultTheme', text: 'You clicked "Cancel" button', type: 'error' });
            //        }
            //    }
            //]
        });
        $(".btn-primary").focus();
        //console.log('html: ' + n.options.id);
    },
    //======================================================================================



    getGridConfig: function (opt, urllink, sortColumnName, orderBy) {

        return $.extend(true, {
            url: urllink,
            datatype: 'json',
            mtype: 'GET',
            pager: jQuery('#pager'),
            rowNum: 10,
            rowList: [5, 10, 15, 20, 50, 100],
            sortname: sortColumnName,
            sortorder: orderBy, //"DESC" OR ASC,
            viewrecords: true,
            jsonReader: {
                root: "Data",
                page: "PageIndex",
                total: "TotalPages",
                records: "TotalCount",
                repeatitems: false
            },
            loadBeforeSend: function (xhr) {
                xhr.setRequestHeader("content-type", "application/json");
            },
            prmNames: { page: 'pageIndex', rows: 'pageSize', sort: 'orderByField', order: 'orderByType' },
            height: 'auto'
        }, opt);
    },
    multilineGridColumn: function (el, cellval, opts) {
        $(el).attr('style', 'white-space: normal;');
        $(el).html(cellval);
        //return 'style="white-space: normal;'
    },
    disablePopup: function (popupDivName, backgroundDivName) {
        $(popupDivName).fadeOut("slow");
        $(backgroundDivName).fadeOut("slow");
    },

    centerPopup: function (popupDivName) {
        var windowWidth = document.documentElement.clientWidth;
        var windowHeight = document.documentElement.clientHeight;
        var popupHeight = $(popupDivName).height();
        var popupHeight = popupHeight;
        var popupWidth = $(popupDivName).width();

        $(popupDivName).css({
            "position": "absolute",
            "top": windowHeight / 2 - popupHeight / 2,
            "left": windowWidth / 2 - popupWidth / 2,
            "height": popupHeight
        });

        $('#backgroundPopup').css({
            "height": windowHeight
        });

    },
    
    PopupWindow: function (ctrId, title, width) {
      
        $("#" + ctrId).kendoWindow({
            
            title: title,
            resizeable: false,
            width: width,
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false

        });
        $("#" + ctrId).data("kendoWindow").open().center();

    },

    initPopupWindow: function (ctrId, title, width) {

        $("#" + ctrId).kendoWindow({

            title: title,
            resizeable: false,
            width: width,
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false

        });
       

    },

    showlink: function (el, cellval, opts) {
        var op = { baseLinkUrl: opts.baseLinkUrl, showAction: opts.showAction, addParam: opts.addParam };
        if (!isUndefined(opts.colModel.formatoptions)) {
            op = $.extend({}, op, opts.colModel.formatoptions);
        }
        idUrl = op.baseLinkUrl + op.showAction + '?id=' + opts.rowId + op.addParam;
        if (isString(cellval)) {	//add this one even if its blank string
            $(el).html("<a class=\"aColumn\" href=\"#\"" + "onclick=\"Page.Test(' " + opts.rowId + "')\">" + cellval + "</a>");
        } else {
            $.fn.fmatter.defaultFormat(el, cellval);
        }
    },
    jqGridDate: function (el, cellval, opts) {
        if (!isEmpty(cellval) && cellval != "/Date(-62135596800000)/")
            $(el).html(AjaxManager.changeDateFormat(cellval, 0));
    },
    jqGridDateTime: function (el, cellval, opts) {

        if (!isEmpty(cellval) && cellval != "/Date(-62135596800000)/")
            $(el).html(AjaxManager.changeDateFormat(cellval, 1));
    },
    changeDateFormat: function (value, isTime) {
        var time = value.replace(/\/Date\(([0-9]*)\)\//, '$1');
        var date = new Date();
        date.setTime(time);
        if (isTime == 0) {
            return (date.getDate().toString().length == 2 ? date.getDate() : '0' + date.getDate()) + '-' + ((date.getMonth() + 1).toString().length == 2 ? (date.getMonth() + 1) : '0' + (date.getMonth() + 1)) + '-' + date.getFullYear();
        }
        else {
            return (date.getDate().toString().length == 2 ? date.getDate() : '0' + date.getDate()) + '-' + ((date.getMonth() + 1).toString().length == 2 ? (date.getMonth() + 1) : '0' + (date.getMonth() + 1)) + '-' + date.getFullYear()
       + '<br> ' + (date.getHours().toString().length == 2 ? date.getHours() : '0' + date.getHours()) + ':' + (date.getMinutes().toString().length == 2 ? date.getMinutes() : '0' + date.getMinutes()) + ':' + (date.getSeconds().toString().length == 2 ? date.getSeconds() : '0' + date.getSeconds());
        }
    },
    getCurrentDateTime: function () {
        var date = new Date();
        var day = (date.getDate().toString().length == 2 ? date.getDate() : '0' + date.getDate()).toString();
        var month = ((date.getMonth() + 1).toString().length == 2 ? (date.getMonth() + 1) : '0' + (date.getMonth() + 1)).toString();
        var year = date.getFullYear().toString();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var suffix = "AM";
        if (hours >= 12) {
            suffix = "PM";
            hours = hours - 12;
        }
        if (hours == 0) {
            hours = 12;
        }

        if (minutes < 10)
            minutes = "0" + minutes
        //var CurrentDateTime = day + "/" + month + "/" + year + " " + hours + ":" + minutes + " " + suffix;
        //var CurrentDateTime = day + "/" + month + "/" + year + " " + hours + ":" + minutes;
        var CurrentDateTime = day + "-" + month + "-" + year;
        return CurrentDateTime;
    },
    changeToSQLDateFormat: function (value, isTime) {
        if (value != "/Date(-62135596800000)/") {
            var time = value.replace(/\/Date\(([0-9]*)\)\//, '$1');
            var date = new Date();
            date.setTime(time);
            var dd = (date.getDate().toString().length == 2 ? date.getDate() : '0' + date.getDate()).toString();
            var mm = ((date.getMonth() + 1).toString().length == 2 ? (date.getMonth() + 1) : '0' + (date.getMonth() + 1)).toString();
            var yyyy = date.getFullYear().toString();
            var timeformat = "";
            if (isTime == 1) {
                timeformat = (date.getHours().toString().length == 2 ? date.getHours() : '0' + date.getHours()) + ':' + (date.getMinutes().toString().length == 2 ? date.getMinutes() : '0' + date.getMinutes()) + ':' + (date.getSeconds().toString().length == 2 ? date.getSeconds() : '0' + date.getSeconds());
            }
            var sqlFormatedDate = mm + '/' + dd + '/' + yyyy + ' ' + timeformat;
            return sqlFormatedDate;
        }
        else {
            return "";
        }

    },
    changeReverseDateFormat: function (value) {
        dtvalue = value.split('-');
        var datetime = dtvalue[1] + "/" + dtvalue[0] + "/" + dtvalue[2];
        return datetime;
    },
    changeFormattedDate: function (value, format) {
        var date = new Date(value);
        if (format == "DDMMYYYY") {
            return (date.getDate().toString().length == 2 ? date.getDate() : '0' + date.getDate()) + '-' + ((date.getMonth() + 1).toString().length == 2 ? (date.getMonth() + 1) : '0' + (date.getMonth() + 1)) + '-' + date.getFullYear();
        }
        if (format == "MMDDYYYY") {
            return ((date.getMonth() + 1).toString().length == 2 ? (date.getMonth() + 1) : '0' + (date.getMonth() + 1)) + '-' + (date.getDate().toString().length == 2 ? date.getDate() : '0' + date.getDate()) + '-' + date.getFullYear();
        }
    },
    getDayDifference: function (date1, date2) {

        // The number of milliseconds in one day
        var ONE_DAY = 1000 * 60 * 60 * 24;

        // Convert both dates to milliseconds
        var date1_ms = new Date(date1).getTime();
        var date2_ms = new Date(date2).getTime();

        // Calculate the difference in milliseconds
        var difference_ms = Math.abs(date1_ms - date2_ms);

        // Convert back to days and return
        return Math.round(difference_ms / ONE_DAY);

    },
    hideMasterDetailsForPrint: function () {
        $("#header").hide();
        $("#dynamicmenu").hide();
        $("#divWelcome").hide();
        $("#content").hide();
        $("#main").css({
            "background-color": "#ffffff"
        });
        $("body").css({
            "background-color": "#ffffff"
        });
        $("#footer").hide();
    },
    showMasterDetailsForPrint: function () {
        $("#header").show();
        $("#dynamicmenu").show();
        $("#divWelcome").show();
        $("#content").show();
        $("#main").css({
            "background-color": "#A6D77B"
        });
        $("body").css({
            "background-color": "#A6D77B"
        });
        $("#footer").show();
    },
    daysBetween: function (date1, date2) {

        // The number of milliseconds in one day
        var ONE_DAY = 1000 * 60 * 60 * 24;

        // Convert both dates to milliseconds
        var date1_ms = new Date(date1).getTime();
        var date2_ms = new Date(date2).getTime();

        // Calculate the difference in milliseconds
        var difference_ms = Math.abs(date1_ms - date2_ms);

        // Convert back to days and return
        return Math.round(difference_ms / ONE_DAY);

    },
    
    isFloat: function (s) {
        return /^\s*(\d+)?(\.(\d+))?\s*$/.test(s);
    },
    isDate: function (str) {
        var m = str.match(/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/);
        return (m) ? true : false;
    },
    isDigit: function (s) {
        return /^\s*\d+\s*$/.test(s);
    },
    
    isEmpty: function (s) {
        return !((s != null) && /^\s*(\S+(\s+\S+)*)\s*$/.test(s));
    },
    checkSpecialCharacters: function (id) {
     
        var checkString = $("#" + id).val();
   
        var regex = /[^\w\s&]/gi;
        
        
        if (checkString != "") {
            if (regex.test(checkString)) {
                AjaxManager.MsgBox('warning', 'center', 'Special Characters:', 'Your search string contains illegal characters.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                              return (false);
                          }
                      }]);

            }
            else {
                return true;
            }
        }
        
    },
    replaceSingleQoute: function (id) {

        var checkString = $("#" + id).val();
        checkString = checkString.replace(/'/g, "''");
        return checkString;

    },
    

    validator: function (divId) {
       
        var validator = divId.kendoValidator().data("kendoValidator"),
           status = $(".status");

        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },

    Trim: function (s) {
        //return s.replace(s,"/^ *(\w+ ?)+ *$/", "");
        return (s.replace(/\s+/g, ' ')).trim();
    },
    isValidItem: function (ctrlId, isClear) {

        var cmbBox = $("#" + ctrlId).data("kendoComboBox");

        if (cmbBox.value() != "" && cmbBox.value() == cmbBox.text()) {
            AjaxManager.MsgBox('warning', 'center', 'Invalid Item:', 'No Item matched with your Input data as like "[' + cmbBox.text() + ']"!', [{
                addClass: 'btn btn-primary',
                text: 'Ok',
                onClick: function ($noty) {
                    $noty.close();
                    //cmbBox.focus();
                    if (isClear)
                        cmbBox.value(-1);


                }
            }
            ]);
            return false;
        } else {
            return true;
        }
    },
    toTitleCase: function (str)
    {
        return str.replace(/\w\S*/g, function(txt){return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();});
    },
    //gridDateFormatter:function (field) {
    //    return kendo.toString(kendo.parseDate(field, 'dd/MM/yyyy'), 'dd/MM/yyyy');
    //}
};                       //End AjaxManager

var MenuManager = {
    //getMenu: function (moduleId) {
    //    var pathName = window.location.pathname;
    //    var pageName = pathName.substring(pathName.lastIndexOf('/') + 1);
    //    var serviceURL = "";
    //    //MenuManager.getCurrentUser(pageName);
    //    var jsonParam = "moduleId=" + moduleId;
    //    if (pageName.toLowerCase() == "home.mvc") {
    //        serviceURL = "./Home/SelectMenuByUserPermission";
    //        serviceRoot = ".";
    //    }
    //    else {
    //        serviceURL = "./SelectMenuByUserPermission";
    //    }
    //    AjaxManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);
    //    function onSuccess(jsonData) {

    //        MenuManager.populateMenus(jsonData);
    //        if (CurrentUser != null) {
    //            if (CurrentUser.UserName != undefined) {
    //                $("#lblWelcome").html("Welcome " + CurrentUser.UserName);
    //            }
    //        }

    //    }
    //    function onFailed(error) {
    //        window.alert(error.statusText);
    //    }
    //},
    getMenu: function (moduleId) {
        var pathName = window.location.pathname;
        var pageName = pathName.substring(pathName.lastIndexOf('/') + 1);
        var serviceURL = "../Menu/SelectMenuByUserPermission/";
        var jsonParam = "";// "moduleId=" + moduleId;
        AjaxManager.GetJsonResult(serviceURL, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            //MenuManager.populateMenus(jsonData);
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },
    getCurrentUser: function (menuRefresh) {
        var jsonParam = '';
        var pathName = window.location.pathname;
        var pageName = pathName.substring(pathName.lastIndexOf('/') + 1);
        var serviceURL = "../Home/GetCurrentUser";
        //if (pageName.toLowerCase() == "home.mvc") {
        //    serviceURL = "./Home/GetCurrentUser";
        //}
        //else {
        //    serviceURL = "./GetCurrentUser";
        //}
        AjaxManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            CurrentUser = jsonData;
            if (CurrentUser != undefined) {
                if (menuRefresh == true) {
                    MenuManager.getMenu(1);
                }
                
                $("#headerLogo").attr('style', 'background-image: url("' + CurrentUser.FullLogoPath + '") !important');
            }

        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },
    //populateMenus: function (menus) {
    //    debugger;
    //    var link = "";
    //    $(".jd_menu").html('');
    //    $.each(menus, function () {
    //        if (this.ParentMenu == 0 && this.MenuPath == "") {
    //            link = "<li><a href='#' class='accessible'>" + this.MenuName + "&raquo;</a><ul id='m" + this.MenuID + "'></ul></li>";
    //            $(".jd_menu").append(link);

    //        }
    //        else if (this.ParentMenu == 0 && this.MenuPath != "") {
    //            var param = this.MenuPath.split('?');
    //            if (param[1] != null) {
    //                var menuPath = param[0] + "?UserId=" + CurrentUser.UserId;
    //                link = "<li id='m" + this.MenuID + "'><a href='" + serviceRoot + menuPath + "' target='_self'>" + this.MenuName + "</a></li>";
    //            }
    //            else {
    //                link = "<li id='m" + this.MenuID + "'><a href='" + serviceRoot + this.MenuPath + "' target='_self'>" + this.MenuName + "</a></li>";
    //            }
    //            $(".jd_menu").append(link);

    //        }
    //    });
    //    $.each(menus, function () {
    //        if (this.ParentMenu > 0 && this.MenuPath == "") {
    //            link = "<li><a href='#' class='accessible'>" + this.MenuName + "&raquo;</a><ul id='m" + this.MenuID + "'></ul></li>";
    //            $("#m" + this.ParentMenu).append(link);
    //        }
    //        else if (this.ParentMenu > 0 && this.MenuPath != "") {
    //            var param = this.MenuPath.split('?');
    //            if (param[1] != null) {
    //                var menuPath = param[0] + "?UserId=" + CurrentUser.UserId;
    //                link = "<li id='m" + this.MenuID + "'><a href='" + serviceRoot + menuPath + "' target='_self'>" + this.MenuName + "</a></li>";
    //            }
    //            else {
    //                link = "<li id='m" + this.MenuID + "'><a href='" + serviceRoot + this.MenuPath + "' target='_self'>" + this.MenuName + "</a></li>";
    //            }
    //            $("#m" + this.ParentMenu).append(link);
    //        }
    //    });
    //    $('ul.jd_menu').jdMenu();
    //}


    IsStringEmpty: function (str) {
        if (str && str != '')
            return false;
        else
            return true;
    },

    //populateMenus: function (menus) {
    //    var dynamicmenuArray = [];
    //    var chiledMenuArray = [];
    //    for (var i = 0; i < menus.length; i++) {
    //        if (menus[i].ParentMenuId == null) {
    //            var objMenu = new Object();
    //            objMenu.text = menus[i].MenuName;
    //            if (menus[i].MenuPath == "") {
    //                objMenu.url = "";
    //            }
    //            else {
    //                objMenu.url = menus[i].MenuPath;
    //            }
    //            objMenu.spriteCssClass = "spriteClass";
    //            objMenu.items = chiledMenuArray;
    //            objMenu.items = MenuManager.addchiledMenu(objMenu, menus[i].MenuId, menus);
    //            dynamicmenuArray.push(objMenu);
    //        }
    //    }

    //    var dataSource = new kendo.data.HierarchicalDataSource({
    //        data: dynamicmenuArray
    //    });
    //    $("#menu").data("kendoMenu").setDataSource(dataSource);

    //},

    addchiledMenu: function (objMenuOrginal, menuId, objMenuList) {
        var chiledMenuArray = [];
        var newMenuArray = [];
        for (var j = 0; j < objMenuList.length; j++) {
            if (objMenuList[j].ParentMenuId == menuId) {
                var objMenu = new Object();
                objMenu = objMenuOrginal;
                var objChiledMenu = new Object();
                objChiledMenu.id = objMenuList[j].MenuId;
                objChiledMenu.itemId = objMenuList[j].MenuId;
                objChiledMenu.text = objMenuList[j].MenuName;
                if (objMenuList[j].MenuPath == "") {
                    objMenu.url = "";
                }
                else {
                    objMenu.url = objMenuList[j].MenuPath;
                }
                objChiledMenu.spriteCssClass = "html";
                chiledMenuArray = objMenuOrginal.items;
                if (chiledMenuArray == undefined || chiledMenuArray.length == 0) {
                    chiledMenuArray = [];
                }
                else {
                    objChiledMenu.expanded = true,
                    objChiledMenu.spriteCssClass = "folder";
                }
                newMenuArray = MenuManager.chiledMenu(objChiledMenu, objMenuList[j].MenuId, objMenuList);
                chiledMenuArray.push(objChiledMenu);
                objMenu.items = chiledMenuArray;
            }
        }
        return chiledMenuArray;
    }

};

var currencyConverter = {

    add_commas: function (nStr) {
        nStr += '';
        x = nStr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    },
    digitToWordConverter: function (junkVal) {
        junkVal = Math.floor(junkVal);
        var obStr = new String(junkVal);
        numReversed = obStr.split("");
        actnumber = numReversed.reverse();

        if (Number(junkVal) >= 0) {
            //do nothing
        }
        else {
            alert('wrong Number cannot be converted');
            return false;
        }
        if (Number(junkVal) == 0) {
            document.getElementById('container').innerHTML = obStr + '' + 'Rupees Zero Only';
            return false;
        }
        if (actnumber.length > 9) {
            alert('Oops!!!! the Number is too big to covertes');
            return false;
        }

        var iWords = ["Zero", " One", " Two", " Three", " Four", " Five", " Six", " Seven", " Eight", " Nine"];
        var ePlace = ['Ten', ' Eleven', ' Twelve', ' Thirteen', ' Fourteen', ' Fifteen', ' Sixteen', ' Seventeen', ' Eighteen', ' Nineteen'];
        var tensPlace = ['dummy', ' Ten', ' Twenty', ' Thirty', ' Forty', ' Fifty', ' Sixty', ' Seventy', ' Eighty', ' Ninety'];

        var iWordsLength = numReversed.length;
        var totalWords = "";
        var inWords = new Array();
        var finalWord = "";
        j = 0;
        for (i = 0; i < iWordsLength; i++) {
            switch (i) {
                case 0:
                    if (actnumber[i] == 0 || actnumber[i + 1] == 1) {
                        inWords[j] = '';
                    }
                    else {
                        inWords[j] = iWords[actnumber[i]];
                    }
                    inWords[j] = inWords[j] + ' Only';
                    break;
                case 1:
                    tens_complication();
                    break;
                case 2:
                    if (actnumber[i] == 0) {
                        inWords[j] = '';
                    }
                    else if (actnumber[i - 1] != 0 && actnumber[i - 2] != 0) {
                        inWords[j] = iWords[actnumber[i]] + ' Hundred and';
                    }
                    else {
                        inWords[j] = iWords[actnumber[i]] + ' Hundred';
                    }
                    break;
                case 3:
                    if (actnumber[i] == 0 || actnumber[i + 1] == 1) {
                        inWords[j] = '';
                    }
                    else {
                        inWords[j] = iWords[actnumber[i]];
                    }
                    if (actnumber[i + 1] != 0 || actnumber[i] > 0) {
                        inWords[j] = inWords[j] + " Thousand";
                    }
                    break;
                case 4:
                    tens_complication();
                    break;
                case 5:
                    if (actnumber[i] == 0 || actnumber[i + 1] == 1) {
                        inWords[j] = '';
                    }
                    else {
                        inWords[j] = iWords[actnumber[i]];
                    }
                    if (actnumber[i + 1] != 0 || actnumber[i] > 0) {
                        inWords[j] = inWords[j] + " Lakh";
                    }
                    break;
                case 6:
                    tens_complication();
                    break;
                case 7:
                    if (actnumber[i] == 0 || actnumber[i + 1] == 1) {
                        inWords[j] = '';
                    }
                    else {
                        inWords[j] = iWords[actnumber[i]];
                    }
                    inWords[j] = inWords[j] + " Crore";
                    break;
                case 8:
                    tens_complication();
                    break;
                default:
                    break;
            }
            j++;
        }

        function tens_complication() {
            if (actnumber[i] == 0) {
                inWords[j] = '';
            }
            else if (actnumber[i] == 1) {
                inWords[j] = ePlace[actnumber[i - 1]];
            }
            else {
                inWords[j] = tensPlace[actnumber[i]];
            }
        }
        inWords.reverse();
        for (i = 0; i < inWords.length; i++) {
            finalWord += inWords[i];
        }
        return finalWord;
    }
};

var FileManager = {
    showFilePopup: function (container, valueContainer) {
        //alert(valueContainer);
        jQuery(container).dialog("destroy");
        jQuery(container).dialog({
            height: 257,
            modal: true,
            title: "File Upload",
            width: 381,
            //bgiframe: true,            
            //autoOpen: false, 
            resizable: false

        });
    },

    getUploadedFileDetails: function (jsonData) {
        alert(jsonData.message);
        alert(jsonData.webpath);
        FileManager.closeFilePopup(container);
    },

    closeFilePopup: function (container) {
        jQuery(container).dialog("close");
        jQuery(container).dialog("destroy");
    }
};


var DistrictManager = {
    GetDistrictInformation: function () {
        var items = [["Barguna"],
            ["Barisal"],
            ["Bhola"],
            ["Jhalokati"],
            ["Patuakhali"],
            ["Pirojpur"],
            ["Bandarban"],
            ["Brahmanbaria"],
            ["Chandpur"],
            ["Chittagong"],
            ["Comilla"],
            ["Coxs Bazar"],
            ["Feni"],
            ["Khagrachhari"],
            ["Lakshmipur"],
            ["Noakhali"],
            ["Rangamati"],
            ["Dhaka"],
            ["Faridpur"],
            ["Gazipur"],
            ["Gopalganj"],
            ["Jamalpur"],
            ["Kishoreganj"],
            ["Madaripur"],
            ["Manikganj"],
            ["Munshiganj"],
            ["Mymensingh"],
            ["Narayanganj"],
            ["Narsingdi"],
            ["Netrakona"],
            ["Rajbari"],
            ["Shariatpur"],
            ["Sherpur"],
            ["Tangail"],
            ["Bagerhat"],
            ["Chuadanga"],
            ["Jessore"],
            ["Jhenaidah"],
            ["Khulna"],
            ["Kushtia"],
            ["Magura"],
            ["Meherpur"],
            ["Narail"],
            ["Satkhira"],
            ["Bogra"],
            ["Joypurhat"],
            ["Naogaon"],
            ["Natore"],
            ["Nawabganj"],
            ["Pabna"],
            ["Rajshahi"],
            ["Sirajganj"],
            ["Dinajpur"],
            ["Gaibandha"],
            ["Kurigram"],
            ["Lalmonirhat"],
            ["Nilphamari"],
            ["Panchagarh"],
            ["Rangpur"],
            ["Thakurgaon"],
            ["Habiganj"],
            ["Moulvibazar"],
            ["Sunamganj"],
            ["Sylhet"]];
        return items;
    }
};

var bloodgroupManager = {
    
    GetBloodGroupArray: function () {
        var items = [["A+"], ["A-"], ["B+"], ["B-"], ["AB+"], ["AB-"], ["O+"], ["O-"]];
        return items;
    }
};

var heightManager = {

    GetHeightArray: function () {
        var items = [["3'0\""], ["3'1\""], ["3'2\""], ["3'3\""],
            ["3'4\""], ["3'5\""], ["3'6\""], ["3'7\""], ["3'8\""],
        ["3'9\""], ["3'10\""], ["3'11\""], ["4'0\""], ["4'1\""],
        ["4'2\""], ["4'3\""], ["4'4\""], ["4'5\""], ["4'6\""], ["4'7\""],
        ["4'8\""], ["4'9\""], ["4'10\""], ["4'11\""], ["5'0\""], ["5'1\""],
        ["5'2\""], ["5'3\""], ["5'4\""], ["5'5\""], ["5'6\""], ["5'7\""], ["5'8\""],
        ["5'9\""], ["5'10\""], ["5'11\""], ["6'0\""], ["6'1\""], ["6'2\""],
        ["6'3\""], ["6'4\""], ["6'5\""], ["6'6\""], ["6'7\""], ["6'8\""],
        ["6'9\""], ["6'10\""], ["6'11\""], ["7'0\""], ["7'1\""], ["7'2\""],
        ["7'3\""], ["7'4\""], ["7'5\""], ["7'6\""], ["7'7\""], ["7'8\""],
        ["7'9\""], ["7'10\""], ["7'11\""], ["8'0\""]];
        return items;
    }
};

var CountryManager = {
    getCountryNames: function() {
        var states = [
            ["Afghanistan"],
            ["Albania"],
            ["Algeria"],
            ["Andorra"],
            ["Angola"],
            ["Antarctica"],
            ["Antigua and Barbuda"],
            ["Argentina"],
            ["Armenia"],
            ["Australia"],
            ["Austria"],
            ["Azerbaijan"],
            ["Bahamas"],
            ["Bahrain"],
            ["Bangladesh"],
            ["Barbados"],
            ["Belarus"],
            ["Belgium"],
            ["Belize"],
            ["Benin"],
            ["Bermuda"],
            ["Bhutan"],
            ["Bolivia"],
            ["Bosnia and Herzegovina"],
            ["Botswana"],
            ["Brazil"],
            ["Brunei"],
            ["Bulgaria"],
            ["Burkina Faso"],
            ["Burma"],
            ["Burundi"],
            ["Cambodia"],
            ["Cameroon"],
            ["Canada"],
            ["Cape Verde"],
            ["Central African Republic"],
            ["Chad"],
            ["Chile"],
            ["China"],
            ["Colombia"],
            ["Comoros"],
            ["Congo"], ["Democratic Republic"],
            ["Congo"], ["Republic of the"],
            ["Costa Rica"],
            ["Cote d'Ivoire"],
            ["Croatia"],
            ["Cuba"],
            ["Cyprus"],
            ["Czech Republic"],
            ["Denmark"],
            ["Djibouti"],
            ["Dominica"],
            ["Dominican Republic"],
            ["East Timor"],
            ["Ecuador"],
            ["Egypt"],
            ["El Salvador"],
            ["Equatorial Guinea"],
            ["Eritrea"],
            ["Estonia"],
            ["Ethiopia"],
            ["Fiji"],
            ["Finland"],
            ["France"],
            ["Gabon"],
            ["Gambia"],
            ["Georgia"],
            ["Germany"],
            ["Ghana"],
            ["Greece"],
            ["Greenland"],
            ["Grenada"],
            ["Guatemala"],
            ["Guinea"],
            ["Guinea-Bissau"],
            ["Guyana"],
            ["Haiti"],
            ["Honduras"],
            ["Hong Kong"],
            ["Hungary"],
            ["Iceland"],
            ["India"],
            ["Indonesia"],
            ["Iran"],
            ["Iraq"],
            ["Ireland"],
            ["Israel"],
            ["Italy"],
            ["Jamaica"],
            ["Japan"],
            ["Jordan"],
            ["Kazakhstan"],
            ["Kenya"],
            ["Kiribati"],
            ["Korea North"],
            ["Korea South"],
            ["Kuwait"],
            ["Kyrgyzstan"],
            ["Laos"],
            ["Latvia"],
            ["Lebanon"],
            ["Lesotho"],
            ["Liberia"],
            ["Libya"],
            ["Liechtenstein"],
            ["Lithuania"],
            ["Luxembourg"],
            ["Macedonia"],
            ["Madagascar"],
            ["Malawi"],
            ["Malaysia"],
            ["Maldives"],
            ["Mali"],
            ["Malta"],
            ["Marshall Islands"],
            ["Mauritania"],
            ["Mauritius"],
            ["Mexico"],
            ["Micronesia"],
            ["Moldova"],
            ["Mongolia"],
            ["Morocco"],
            ["Monaco"],
            ["Mozambique"],
            ["Namibia"],
            ["Nauru"],
            ["Nepal"],
            ["Netherlands"],
            ["New Zealand"],
            ["Nicaragua"],
            ["Niger"],
            ["Nigeria"],
            ["Norway"],
            ["Oman"],
            ["Pakistan"],
            ["Panama"],
            ["Papua New Guinea"],
            ["Paraguay"],
            ["Peru"],
            ["Philippines"],
            ["Poland"],
            ["Portugal"],
            ["Qatar"],
            ["Romania"],
            ["Russia"],
            ["Rwanda"],
            ["Samoa"],
            ["San Marino"],
            ["Sao Tome"],
            ["Saudi Arabia"],
            ["Senegal"],
            ["Serbia and Montenegro"],
            ["Seychelles"],
            ["Sierra Leone"],
            ["Singapore"],
            ["Slovakia"],
            ["Slovenia"],
            ["Solomon Islands"],
            ["Somalia"],
            ["South Africa"],
            ["Spain"],
            ["Sri Lanka"],
            ["Sudan"],
            ["Suriname"],
            ["Swaziland"],
            ["Sweden"],
            ["Switzerland"],
            ["Syria"],
            ["Taiwan"],
            ["Tajikistan"],
            ["Tanzania"],
            ["Thailand"],
            ["Togo"],
            ["Tonga"],
            ["Trinidad and Tobago"],
            ["Tunisia"],
            ["Turkey"],
            ["Turkmenistan"],
            ["Uganda"],
            ["Ukraine"],
            ["United Arab Emirates"],
            ["United Kingdom"],
            ["United States"],
            ["Uruguay"],
            ["Uzbekistan"],
            ["Vanuatu"],
            ["Venezuela"],
            ["Vietnam"],
            ["Yemen"],
            ["Zambia"],
            ["Zimbabwe"]
        ];

        return states;
    }
};

