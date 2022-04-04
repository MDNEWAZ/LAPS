var dueSummaryManager = {
    InitDueDetails: function () {
        dueSummaryManager.GenerateDueGrid();
        empressCommonHelper.GenerareHierarchyCompanyCombo("cbmDueCompanyName");
        
        if (CurrentUser != null) {
            $("#cbmDueCompanyName").data("kendoComboBox").value(CurrentUser.CompanyId);
        }
        
        dueDetailsHelper.populateColorCombo();
        //dueSummaryManager.SetDataGridDue();
    },
    GenerateDueGrid: function () {
        $("#gridDue").kendoGrid({
            dataSource: dueSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: dueSummaryHelper.GenerateDueColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },
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
                    url: '../RatingConfiguration/GetACompayDueKpi/',
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
    },
    SetDataGridDue: function () {
        debugger;
        var data = dueSummaryManager.gridDataSource();
        var gridDue = $("#gridDue").kendoGrid();
        gridDue.setDataSource(data);
    }
   
};

var dueSummaryHelper = {
    GenerateDueColumns: function () {
        return columns = [
               { field: "DueId", hidden: true },
               { field: "ACompany.CompanyName", title: "Company Name", width: 100 },
               { field: "FromDue", title: "From Due(%)", width: 80 },
               { field: "ToDue", title: "UpTo Due(%)", width: 80 },
               { field: "Color", title: "Color", width: 70 },
               { field: "EntryDate", title: "Valid From", width: 100 },
               { field: "Status", title: "Status", width: 70,template:"#=Status==1?'Active':'InActive'#" },
            { field: "Edit", title: "Edit Due", filterable: false, width: 60, template: '<button type="button" value="Edit" id="btnEdit" onClick="dueSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {
        var entityGrid = $("#gridDue").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        dueDetailsHelper.FillDueDetailsInForm(selectedItem);
    }
};