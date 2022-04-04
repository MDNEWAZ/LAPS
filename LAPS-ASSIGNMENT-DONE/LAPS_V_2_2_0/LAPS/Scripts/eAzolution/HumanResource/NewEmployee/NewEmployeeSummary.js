var currentUserlevel = [];
var gbemployeegrid = [];
var EmployeeSummaryManager = {

    gridDataSource: function () {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    url: '../NewEmployee/GetEmployeeTwoSummary/',

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../NewEmployee/GetEmployeeTwoSummary/',
                    dataType: "json"
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
     
        //debugger;
        return gridDataSource;
    },

    GetEmployeeInfoData: function (empId) {
       
        var jsonParam = "id=" + empId;
        var serviceUrl = "../NewEmployee/GetEmployeeInfoForCheckBox/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        
        function onSuccess(jsonData) {
            for (var i = 0; i < jsonData.length; i++) {
                var obj = new Object();
                obj.EmployeeID = jsonData[i].EmployeesInfo;
                gbemployeegrid.push(obj);
            }
            
        }
        function onFailed(jqXhr, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        
    }


};
var EmployeeSummaryHelper = {

    initForEmployeeGrid: function () {
        EmployeeSummaryHelper.GenerateEmployeeGrid();
        EmployeeSummaryHelper.GenerateCheckedBoxEmployeeGrid();
        EmployeeSummaryHelper.createEmployeeList();
       
    },
    GenerateEmployeeGrid: function () {
        //debugger;
        var data = EmployeeSummaryManager.gridDataSource();
        $("#divgridEmployeeSummary").kendoGrid({
            dataSource: data,
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: EmployeeSummaryHelper.GeneratedEmployeeColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });
    },
   
   
    GeneratedEmployeeColumns: function () {
        return columns = [
        { field: "EmployeeID", title: "EmployeeID", width: 50, hidden: false },
        { field: "EmployeeName", title: "Employee  Name", width: 50, sortable: false },
        { field: "Designation", title: "Designation", width: 50, sortable: true },
        { field: "Mobile", title: "Mobile", width: 50, sortable: false },
        { field: "Edit", title: "Edit", filterable: false, width: 30, template: '<button type="button" value="Edit" id="btnEdit" onClick="EmployeeSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },

    GenerateCheckedBoxEmployeeGrid: function () {
        //debugger;
        var data = EmployeeSummaryManager.gridDataSource();
        $("#divgridCheckedBoxEmployeeSummary").kendoGrid({
            dataSource: data,
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: EmployeeSummaryHelper.GeneratedCheckedBoxEmployeeColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });
    },

    GeneratedCheckedBoxEmployeeColumns: function () {
        return columns = [
        { field: "check_rowForEmployee", title: "Select", width: 35, editable: false, filterable: false, sortable: false, template: '#= EmployeeSummaryHelper.checkedDataForEmployee(data) #', headerTemplate: '<input type="checkbox" id="checkAllForEmployee" />' },
        { field: "EmployeeID", title: "EmployeeID", width: 50, hidden: false },
        { field: "EmployeeName", title: "Employee  Name", width: 50, sortable: false },
        { field: "Designation", title: "Designation", width: 50, sortable: true },
        { field: "Mobile", title: "Mobile", width: 50, sortable: false },
       
        ];
    },

    checkedDataForEmployee: function (data) {

       

        if (gbemployeegrid.length > 0) {

            var result = gbemployeegrid.filter(function (obj) {
                return obj.EmployeeId == data.EmployeeID;
            });
            if (result.length > 0) {
                return '<input id="check_rowForEmployee" class="check_rowForEmployee" type="checkbox" checked="checked"/>';
            }
            else {
                return '<input id="check_rowForEmployee" class="check_rowForEmployee" type="checkbox"/>';
            }

        }
        else {
            return '<input id="check_rowForEmployee" class="check_rowForEmployee" type="checkbox"/>';
        }
    },

    createEmployeeList: function () {

        $('.check_rowForEmployee').live('click', function (e) {

            var $cb = $(this);
            var gridEmployeeSummary = $("#divgridCheckedBoxEmployeeSummary").data("kendoGrid");
            var selectedItem = gridEmployeeSummary.dataItem(gridEmployeeSummary.select());//$kgrid.attr('k-state-selected');
            if ($cb.is(':checked')) {
                if (selectedItem != null) {
                    var oBr = new Object();
                    oBr.EmployeeID = selectedItem.EmployeeID;
                    gbemployeegrid.push(oBr);
                }
                else {
                    $cb.removeProp('checked', false);
                }
            } else {
                gbemployeegrid = $.grep(gbemployeegrid, function (n) {
                    return n.EmployeeID != selectedItem.EmployeeID;

                });
            }
            gridEmployeeSummary.clearSelection();

        });//Individual row selection

        $('#checkAllForEmployee').live('click', function (e) {
          
            gbemployeegrid = [];

            var gridEmployeeSummary = $("#divgridCheckedBoxEmployeeSummary").data("kendoGrid");
            var selectAll = document.getElementById("checkAllForEmployee");
            if (selectAll.checked == true) {
                $("#divgridCheckedBoxEmployeeSummary tbody input:checkbox").attr("checked", this.checked);
                $("#divgridCheckedBoxEmployeeSummary table tr").addClass('k-state-selected');
                var gridData = gridEmployeeSummary.dataSource.data();
                for (var i = 0; i < gridData.length; i++) {
                    var emp = gridData[i];
                    var oBr = new Object();
                    oBr.EmployeeID = emp.EmployeeID;
                    gbemployeegrid.push(oBr);
                }


            }
            else {
                $("#divgridCheckedBoxEmployeeSummary tbody input:checkbox").removeAttr("checked", this.checked);
                $("#divgridCheckedBoxEmployeeSummary table tr").removeClass('k-state-selected');
            }
            gridEmployeeSummary.clearSelection();

        });// All Row Selection 




    },

    clickEventForEditButton: function () {

        var entityGrid = $("#divgridEmployeeSummary").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            EmployeeDetailsHelper.populateEmployeeDetails(selectedItem);
        }
        

    }
};