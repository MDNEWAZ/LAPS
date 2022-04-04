
var MenuSortingManager = {
    
    gridDataSource: function () {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",

            serverPaging: true,
            serverSorting: true,

            pageSize: 100,
            //page: 1,

            transport: {
                read: {

                    url: '../Menu/SelectAllMenuForSorting/',

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: {
                data: "Items", total: "TotalCount",
                model: {
                    fields: {
                        MenuName: { editable: false },
                        ParentMenuName: { editable: false },
                    }
                }

            }


        });
        return gridDataSource;
    },
    
    UpdateMenuSorting: function (menuList) {
        var objMenuInfo = JSON.stringify(menuList);
        var jsonParam = 'strobjMenuInfo=' + objMenuInfo;
        var serviceUrl = "../Menu/UpdateMenuSorting/";
        AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
        
        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Menu Saved Successfully',
                   [{
                       addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                           $noty.close();
                           $("#gridMenu").data("kendoGrid").dataSource.read();
                           $("#gridMenuSorting").data("kendoGrid").dataSource.read();
                       }
                   }]);
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Login Failed', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Login Failed', error.statusText,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
        }
    }
};

var MenuSortingHelper = {
    
    GenerateMenuGridForSorting: function () {
        $("#gridMenuSorting").kendoGrid({

            dataSource: MenuSortingManager.gridDataSource(),

            autoBind: true,

            filterable: false,
            sortable: false,
            columns: MenuSortingHelper.GeneratedMenuColumnsForSorting(),
            editable: true,
            scrollable: true,
            navigatable: true,
            selectable: "multiple row",

        });
    },
    
    GeneratedMenuColumnsForSorting: function () {
        return columns = [
            { field: "ParentMenuName", title: "Parent Menu", width: 200, editable: false },
            { field: "MenuName", title: "Menu Name", width: 200, editable: false },
            { field: "MenuId", hidden: true },
            { field: "SortOrder", title: "Sort Order", editable: true }
        ];
    },
    

    UpdateMenuSorting: function () {
        var gridMenuSummary = $("#gridMenuSorting").data("kendoGrid");
        
        var menuArray = [];
        
        for (var i = 0; i < gridMenuSummary._data.length; i++) {
            var objMenu = new Object();
            objMenu.MenuId = gridMenuSummary._data[i].MenuId;
            objMenu.SortOrder = gridMenuSummary._data[i].SortOrder;
            if (AjaxManager.isDigit(objMenu.SortOrder)) {
                menuArray.push(objMenu);
            }
            
        }
        
        MenuSortingManager.UpdateMenuSorting(menuArray);
        
    }

};