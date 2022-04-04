var StudentEducationInfo = "";
var StudentEducationInfoDetailsManager = {
    StudentEducationInfoDataSource: function (studentId) {

        if (studentId === 0) {
            return [];
        }
        debugger;
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            batch: true,
            //autoSync: true,

            transport: {

                read: {
                    url: '../Students/GetStudentEducationinfoSummary/?id=' + studentId,
                    type: "POST",
                    dataType: "json",
                    cache: false,
                    async: false,
                    contentType: "application/json; charset=utf-8"

                },


                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                    return JSON.stringify(options);
                }
            },

            schema: {
                model: {
                    id: "StudentEduInfiId",
                    fields: {
                        StudentEduInfiId: { type: 'number', editable: true },
                        Exam: { editable: true },
                        Year: { type: 'date', editable: true },
                        Institute: { editable: true },
                        Result: { editable: true },
                        EmployeeId: { editable: false }

                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    },

};

var StudentEducationInfoDetailsHelper = {
    GenerateStudentEducationGrid: function (studentId) {

        $("#StudentEducationInfoDetailsDiv").kendoGrid({
            dataSource: StudentEducationInfoDetailsManager.StudentEducationInfoDataSource(studentId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: StudentEducationInfoDetailsHelper.GenerateGridColumns(),
            editable: true,
            toolbar: ["create"],
            //navigatable: true,
            selectable: "row,multiple",
            dataBound: function (e) {

            }

        });
    },


    GenerateGridColumns: function () {
        return columns = [
            { field: "Exam", title: "Exam", width: 100 },
            { field: "Year", title: " Year", width: 150, format: '{0:dd-MMM-yyyy}' },
            { field: "Institute", title: "Institute", width: 100 },
            { field: "Result", title: "Result", width: 150 }
        ];

    }






};