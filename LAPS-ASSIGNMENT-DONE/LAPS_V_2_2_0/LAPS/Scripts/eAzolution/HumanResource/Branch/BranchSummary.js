var currentUserlevel = [];
var branchSummaryManager = {

    gridDataSource: function (companyId) {
     
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    url: '../Branch/GetBranchTwoSummary/?companyID=' + companyId,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    url: '../Branch/GetBranchTwoSummary/?companyID=' + companyId,
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

var branchSummaryHelper = {
    GenerateBranchGrid: function (companyId) {
      
        $("#divgridBranchSummary").kendoGrid({
            dataSource: branchSummaryManager.gridDataSource(companyId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: branchSummaryHelper.GeneratedBranchColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });
    },

    GeneratedBranchColumns: function () {
        return columns = [
        { filed: "BranchId", title: "BranchId", width: 50, hidden: true },
        { field: "BranchName", title: "Branch Name", width: 80, sortable: true },
        { field: "BranchDescription", title: "Branch Description", width: 100, sortable: false },
        { field: "Edit", title: "Edit", filterable: false, width: 30, template: '<button type="button" value="Edit" id="btnEdit" onClick="branchSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    
    GenerateMotherCompanyCombo: function () {
        var objCompany = new Object();
        objCompany = branchSummaryManager.GetMotherCompany();

        $("#cmbCompanyNameForSummary").kendoComboBox({
            index: 0,
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
        debugger;
        var companyData = $("#cmbCompanyNameForSummary").data("kendoComboBox");
        var companyId = companyData.value();

        $("#divgridBranchSummary").empty();
        $("#divgridBranchSummary").kendoGrid();

        branchSummaryHelper.GenerateBranchGrid(companyId);
    },
    
    clickEventForEditBranch: function () {
        $('#divgridBranchSummary table tr').live('dblclick', function () {
            var entityGrid = $("#divgridBranchSummary").data("kendoGrid");

            var selectedItem = entityGrid.dataItem(entityGrid.select());
            if (selectedItem != null) {
                branchDetailsHelper.populateBranchDetails(selectedItem);
            }
            
        });
    },
    
    clickEventForEditButton: function () {
       
        var entityGrid = $("#divgridBranchSummary").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            branchDetailsHelper.populateBranchDetails(selectedItem);
        }
      
        currentUserlevel = [];
        currentUserlevel = DashboardSettingsManager.userLevel();
         
        var isBranchCodeUsed = branchDetailsManager.GetIsBranchCodeUsed(selectedItem.BranchCode);
        if (currentUserlevel == 23 && isBranchCodeUsed == false) { //23 means HO
            $("#txtBranchCode").prop('disabled', false);
        } else {
            $("#txtBranchCode").prop('disabled', true);
        }
    }
};