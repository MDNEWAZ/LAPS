var TeacherSummary = "";
var TeacherSummaryManager = {
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
                    url: '../Teachers/GetTeacherSummary/',

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    }

};
var TeacherSummaryHelper = {

    GenerateTeacherGrid: function () {
        debugger;
        $("#TeacherSummaryDiv").kendoGrid({
            dataSource: TeacherSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: TeacherSummaryHelper.GeneratedTeacherColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });
    },
    GeneratedTeacherColumns: function () {
        return columns = [
        { field: "TeacherId", hidden: true },
        { field: "TeacherName", title: "Teacher Name", width: 50 },
        { field: "RegNo", title: "  Reg No", width: 50, sortable: false },
        { field: "TeacherGender", title: "Gender", width: 50, sortable: true },
        { field: "DepartmentName", title: "Department Name", width: 50, sortable: false },
        { field: "Exam", title: "Exam", width: 50, sortable: false },
        { field: "TeacherSection", title: "Section Name", width: 50, sortable: false },
        { field: "IsActive", title: "Status", width: 50, sortable: false, template: "#=IsActive==1?'Active':'In Active'#" },
        { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Edit" id="btnEdit" onClick="TeacherSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-pencil"></span></button>', sortable: false }

        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#TeacherSummaryDiv").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            TeacherDetailsHelper.populateTeacherDetails(selectedItem);
        }


    }
};