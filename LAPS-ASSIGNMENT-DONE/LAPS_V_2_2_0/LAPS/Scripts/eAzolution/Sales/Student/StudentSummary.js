var StudentSummary = "";
var StudentSummaryManager = {
    gridDataSource: function() {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    url: '../Students/GetStudentSummary/',

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
var StudentSummaryHelper = {
        
    GenerateStudentGrid: function () {
        debugger;
        $("#StudentSummaryDiv").kendoGrid({
            dataSource: StudentSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: StudentSummaryHelper.GeneratedStudentColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });
    },
    GeneratedStudentColumns: function () {
        return columns = [
        { field: "StudentId", hidden: true },
        { field: "StudentName", title: "Student Name", width: 50 },
        { field: "RegNo", title: "  Reg No", width: 50, sortable: false },
        { field: "StudentGender", title: "Gender", width: 50, sortable: true },
        { field: "DepartmentName", title: "Department Name", width: 50, sortable: false },
        { field: "Exam", title: "Exam", width: 50, sortable: false },
        { field: "StudentSection", title: "Section Name", width: 50, sortable: false },
        { field: "IsActive", title:"Status", width: 50, sortable: false, template: "#=IsActive==1?'Active':'In Active'#" },
        { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Edit" id="btnEdit" onClick="StudentSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-pencil"></span></button>', sortable: false }
        
        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#StudentSummaryDiv").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            StudentDetailsHelper.populateStudentDetails(selectedItem);
        }


    }
};