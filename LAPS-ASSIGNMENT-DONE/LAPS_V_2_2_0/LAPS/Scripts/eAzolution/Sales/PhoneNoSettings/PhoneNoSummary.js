
var PhoneNoSummaryManager = {
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
                    url: '../PhoneNoSettings/GetAllPhoneSettings/',

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

var PhoneNoSummaryHelper = {
    InitPhoneNoSummary: function () {
        PhoneNoSummaryHelper.GenerateSmsSummaryGrid();
    },
    
    GenerateSmsSummaryGrid: function () {
        $("#gridPhoneSettingsSummary").kendoGrid({
            dataSource: PhoneNoSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: PhoneNoSummaryHelper.GeneratePhoneNoSummaryColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },

    GeneratePhoneNoSummaryColumns: function () {
        return columns = [
               { field: "PhoneSettingsId", title: "PhoneSettingsId", width: 80, hidden: true },
               { field: "PhoneNumber", title: "Phone Number", width: 80, },
               { field: "IsActive", title: "IsActive", width: 80, template: "#=IsActive==1?'Active':'InActive'#" },
               { field: "Edit", title: "Edit", filterable: false, width: 60, template: '<button type="button" value="Edit" id="btnEdit" onClick="PhoneNoSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#gridPhoneSettingsSummary").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        PhoneNoDetailsHelper.FillPhoneNoDetailsForm(selectedItem);
    }
};