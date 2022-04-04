
var toDoManager = {
    
    gridDataSource: function () {

        var gridDataSource = new kendo.data.DataSource({
            type: "json",

            serverPaging: true,
            serverSorting: true,

            pageSize: 100,
            //page: 1,

            transport: {
                read: {

                    url: '../Menu/GetToDoList/',

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
                    //fields: {
                    //    Leave: {
                    //        type: "string"
                    //    }
                    //}
                }

            }


        });

        return gridDataSource;
    }

};

var toDoHelper = {
    GenerateToDoGrid: function () {

        $("#todoGrid").kendoGrid({

            dataSource: toDoManager.gridDataSource(),
            autoBind: true,
            
            filterable: false,
            sortable: false,
            columns: toDoHelper.GeneratedToDoColumns(),
            editable: false,
            scrollable: true,
            navigatable: true
        });

    },
    

    GeneratedToDoColumns: function () {
        return columns = [
            { field: "MenuName", title: "QUICK LINK", width: 100, sortable: false },
            { field: "MenuPath", title: "MenuPath", width: 100, hidden: true }//,
            //{ field: "MenuId", title: "MenuId", width: 100, sortable: false }
        ];
    },
    
    GeRowDataOfToDoMenuGrid: function () {
        $('#todoGrid table tr').live('click', function () {
            
            var entityGrid = $("#todoGrid").data("kendoGrid");
            var data = $("#todoGrid").data("kendoGrid").dataItem($(this).closest("tr"));
            window.location.href = data.MenuPath;
           
            
        });

    },
};
