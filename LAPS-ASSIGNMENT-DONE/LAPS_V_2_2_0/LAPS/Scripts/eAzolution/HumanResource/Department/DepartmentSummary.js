

var departmentSummaryManager = {
    gridDataSource: function (companyId) {

        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                autoSync: true,
                read: {
                    url: '../Department/GetDepartmentSummary/?companyID=' + companyId,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../Department/GetDepartmentSummary/?companyID=' + companyId,
                    dataType: "json"
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    },
    GetMotherCompany: function () {
        var objCompany = "";
        var jsonParam = "";
        var serviceUrl = "../Company/GetMotherCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCompany;
    }
};

var departmentSummaryHelper = {
    GenerateDepartmentSummaryGrid: function (companyId) {
        $("#gridDepartment").kendoGrid({
            dataSource: departmentSummaryManager.gridDataSource(companyId),
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,

            },
            xheight: 450,
            filterable: true,
            //sortable: true,
            columns: departmentSummaryHelper.GenerateDepartmentColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"
        });
    },
    GenerateDepartmentColumns: function () {
        return columns = [
            { field: "DepartmentName", title: "Department Name", width: 100 },
            { field: "CompanyName", title: "Company Name", width: 100 },
            { field: "DepartmentHeadName", title: "Department Head", width: 100, sortable: false },
            { field: "DepartmentId", hidden: true },
            { field: "CompanyId", hidden: true },
            { field: "DepartmentHeadId", hidden: true },
            { field: "Edit", title: "Edit", filterable: false, width: 60, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="departmentSummaryHelper.clickEventForEditButton()"  />', sortable: false }
        ];
    },
    GenerateMotherCompanyCombo: function () {
        var objCompany = new Object();
        objCompany = departmentSummaryManager.GetMotherCompany();
        $("#cmbCompanyNameForSummary").kendoComboBox({
            placeholder: "Select Company...",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany
        });
        if (CurrentUser != null) {
            var cmbComp = $("#cmbCompanyNameForSummary").data("kendoComboBox");
            cmbComp.value(CurrentUser.CompanyId);
        }
    },
    CompanyIndexChangeEvent: function () {
        var companyData = $("#cmbCompanyNameForSummary").data("kendoComboBox");
        var companyId = $("#cmbCompanyNameForSummary").val();
        var companyName = companyData.text();
        if (companyId == companyName) {
            $("#cmbCompanyNameForSummary").val("");
            return false;
        }

        $("#gridDepartment").empty();
        $("#gridDepartment").kendoGrid();

        departmentSummaryHelper.GenerateDepartmentSummaryGrid(companyId);
    },
    clickEventForEditDepartment: function () {
        $('#gridDepartment table tr').live('dblclick', function () {
            var entityGrid = $("#gridDepartment").data("kendoGrid");

            var selectedItem = entityGrid.dataItem(entityGrid.select());

            departmentDetailsHelper.populateDepartmentDetails(selectedItem);

        });
    },
    clickEventForEditButton: function () {
        var entityGrid = $("#gridDepartment").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());

        departmentDetailsHelper.populateDepartmentDetails(selectedItem);
    },
};