

var customerSummaryManager = {
    initCustomerSummary: function () {
        empressCommonHelper.GenerareHierarchyCompanyCombo("cmbCompanyNameS");
        if (CurrentUser != null) {
            $("#cmbCompanyNameS").data("kendoComboBox").value(CurrentUser.CompanyId);
            empressCommonHelper.GenerateBranchCombo(CurrentUser.CompanyId, "cmbBranchNameS");
            
            $("#cmbCompanyName").data("kendoComboBox").value(CurrentUser.CompanyId);
            empressCommonHelper.GenerateBranchCombo(CurrentUser.CompanyId, "cmbBranchName");
        }
        $("#cmbCompanyNameS").change(function () {
           
            customerDetailsHelper.ClearCustomerForm();
            var companyData = $("#cmbCompanyNameS").data("kendoComboBox");
            var comboboxbranch = $("#cmbBranchNameS").data("kendoComboBox");
            var companyId = companyData.value();
           // $("#cmbCompanyNameS").data("kendoComboBox").value(companyId);
            var companyName = companyData.text();

            if (companyId == companyName) {
                companyData.value('');
                comboboxbranch.value('');
                empressCommonHelper.GenerateBranchCombo(0, "cmbBranchNameS");
                return false;
            }
            if (comboboxbranch != undefined) {
                comboboxbranch.value('');
            }
           
            empressCommonHelper.GenerateBranchCombo(companyId, "cmbBranchNameS");
          //  empressCommonHelper.GenerateBranchCombo(companyId, "cmbBranchName");
            return true;
        });

        customerSummaryManager.GenerateCustomerGrid();


        //Searching Customer

        $("#btnSearchCustomer").click(function () {
            customerSummaryManager.SearchCustomerSummary();
        });
        $("#txtCustomerCodeS").keypress(function (event) {
            if (event.keyCode == 13) {
                customerSummaryManager.SearchCustomerSummary();
            }
        });

        $("#cmbCompanyNameS").change(function () {
            customerSummaryManager.SearchCustomerSummary();
        });
        $("#cmbBranchNameS").change(function () {
            customerSummaryManager.SearchCustomerSummary();
        });


    },
    SearchCustomerSummary: function () {

        var companyId = $("#cmbCompanyNameS").val() == "" ? 0 : $("#cmbCompanyNameS").val();
        var branchId = $("#cmbBranchNameS").val() == "" ? 0 : $("#cmbBranchNameS").val();
        var customerCode = $("#txtCustomerCodeS").val();
        var customerData = customerSummaryManager.gridDataSource(companyId, branchId, customerCode);
        var customerGrid = $("#gridCustomer").data("kendoGrid");
        customerGrid.setDataSource(customerData);
    },

    GenerateCustomerGrid: function () {
        var companyId = $("#cmbCompanyNameS").val() == "" ? 0 : $("#cmbCompanyNameS").val();
        var branchId = $("#cmbBranchName").val() == "" ? 0 : $("#cmbBranchName").val();
        var customerCode = $("#txtCustomerCodeS").val();

        $("#gridCustomer").kendoGrid({
            dataSource: customerSummaryManager.gridDataSource(companyId, branchId, customerCode),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: customerSummaryHelper.GenerateCustomerColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });

    },
    gridDataSource: function (companyId, branchId, customerCode) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Customer/GetAllCustomer/?companyId=' + companyId + "&branchId=" + branchId + "&customerCode=" + customerCode,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                //update: {
                //    url: '../Customer/GetAllCustomer/',
                //    dataType: "json"
                //},
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    }
};

var customerSummaryHelper = {
    GenerateCustomerColumns: function () {
        return columns = [
            { field: "CustomerId", title: "ID", hidden: true },
            { field: "CustomerCode", title: "Customer Code", width: 90, },
            { field: "ReferenceId", title: "Reference Id", width: 90, },
            { field: "Name", title: "Name", width: 100},
            { field: "NId", title: "National ID", width: 90, },
            { field: "Phone", title: "Phone", width: 65},
            { field: "Address", title: "Address", hidden: true },
            { field: "Type", hidden: true },
            { field: "District", hidden: true },
            { field: "EntryDate", hidden: true },
            { field: "Updated", hidden: true },
            { field: "Flag", hidden: true },
            { field: "IsActive", hidden: true },
            { field: "BranchSmsMobileNumber", title: "Branch SMS Mobile Number", hidden: true },
            { field: "", title: "", filterable: false, width: 35, template: '<button id="btnSearchCustInfo" class="k-i-search" type="submit" onclick="customerSummaryHelper.clickEventForEditButton()"><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {
        var entityGrid = $("#gridCustomer").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            customerDetailsHelper.FillCustomerDetailsInForm(selectedItem);
        }
       

    },


};