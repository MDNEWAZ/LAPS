var PersonSummary = "";
var PersonSummaryManager = {

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
                    url: '../PersonalDetails/GetPersonalSummary/',
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: {
                model: {
                    fields: {
                        DateOfBirth: { type: "date" }
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    }

};
var PersonalDetailsSummaryHelper = {
    GeneratePersonalDetailsGrid: function () {

        $("#PersonalSummaryDiv").kendoGrid({
            dataSource: PersonSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: PersonalDetailsSummaryHelper.GeneratedPersonalDetailsColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });
    },
    GeneratedPersonalDetailsColumns: function () {
        return columns = [
        { field: "PersonalDetailsId", hidden: true },
        { field: "FirstName", title: "First Name", width: 50 },
        { field: "LastName", title: "Last Name", width: 50 },
        { field: "FatherName", title: "Father's Name", width: 50 },
        { field: "MotherName", title: "Mother's Name", width: 50 },
        { field: "DateOfBirth", title: "Date Of Birth", width: 50 ,format:'{0:dd-MMM-yyyy}'},
        { field: "GenderName", title: "Gender", width: 50, sortable: true },
        { field: "MaritalstatusName", title: "Marital Status", width: 50, sortable: false },
        { field: "NationalIdNo", title: "National Id No", width: 50 },
        { field: "ReligionName", title: "Religion", width: 50 },
        { field: "Mobile", title: "Mobile", width: 50 },
        { field: "Address", title: "Address", width: 50, sortable: true },
        { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Edit" id="btnEdit" onClick="PersonalDetailsSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-pencil"></span></button>', sortable: false }


        ];
    },

    clickEventForEditButton: function () {

        var entityGrid = $("#PersonalSummaryDiv").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            PersonDetailHelper.populatePersonDetail(selectedItem);
        }

    }

};

