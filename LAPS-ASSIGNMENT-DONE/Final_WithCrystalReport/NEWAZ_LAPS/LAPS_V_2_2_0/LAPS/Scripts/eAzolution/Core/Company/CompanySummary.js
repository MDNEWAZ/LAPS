var companySummaryManager = {
    GenerateCompanyGrid: function() {
        $("#gridCompany").kendoGrid({
            dataSource: companySummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: companySummaryHelper.GenerateCompanyColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },
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
                    url: '../Company/GetCompanySummary/',

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../Company/LoadAllCompanies/',
                    dataType: "json"
                },

                parameterMap: function(options) {
                    
                    return JSON.stringify(options);

                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    }
};

var companySummaryHelper = {
 GenerateCompanyColumns: function () {
     return columns = [
            { field: "CompanyCode", title: "CompanyCode", width: 80 },
            { field: "CompanyName", title: "Company Name", width: 100 },
         { field: "CompanyType", title: "Company Type", width: 100 },
         { field: "RootCompanyId", title: "RootCompanyId", width: 100,hidden:true },
         { field: "CompanyStock", title: "Company Stock", width: 100, template: "#= CompanyStock==1?'Company Wise':'Branch Wise' #" ,hidden:true},
            { field: "Phone", title: "Phone", width: 100 },
            { field: "Email", title: "Email", width: 100, hidden: true },
            { field: "CompanyId", hidden: true },
            { field: "Address", hidden: true },
            { field: "Fax", hidden: true },
            { field: "ShortLogoPath", hidden: true },
            { field: "FullLogoPath", hidden: true },
            { field: "PrimaryContact", hidden: true },
            { field: "FiscalYearStart", hidden: true },
            { field: "MotherId", hidden: true },
         { field: "Edit", title: "Edit", filterable: false, width: 60, template: '<button type="button" value="Edit" id="btnEdit" onClick="companySummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
 },
 clickEventForEditButton: function () {
     
        var entityGrid = $("#gridCompany").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            companyHelper.FillCompanyDetailsInForm(selectedItem);
        }
       
    }
};