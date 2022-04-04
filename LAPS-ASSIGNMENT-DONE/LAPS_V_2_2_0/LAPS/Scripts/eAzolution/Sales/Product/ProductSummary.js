

var productSummaryManager = {
    GenerateProductGrid: function () {
        $("#gridProduct").kendoGrid({
            dataSource: productSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: productSummaryHelper.GenerateProductColumns(),
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
                    url: '../Product/GetAllProduct/',
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

var productSummaryHelper = {
    GenerateProductColumns: function () {
        return columns = [
            { field: "Model", title: "Package", width: 50 },
            { field: "ProductName", title: "Name", width: 80 },
            { field: "Description", title: "Description", width: 100 },
            { field: "Capacity", title: "Capacity", width: 80, hidden: true },
            { field: "TotalPrice", title: "Total Price", width: 50, hidden: false },
            { field: "ModelId", hidden: true },
            { field: "ModelItemID", hidden: false, width: 30 },
            { field: "Type", width: 70 },
            { field: "Color", hidden: true },
            { field: "ManufactureDate", hidden: true },
            { field: "Flag", hidden: true },
            { field: "IsActive", hidden: true },
            { field: "FiscalYearStart", hidden: true },
            { field: "TotalStock", hidden: true },
            { field: "TotalSale", hidden: true },
            { field: "CurrentStock", hidden: true },
            { field: "Edit", title: "Edit", filterable: false, width: 30, template: '<button type="button" value="Edit" id="btnEdit" onClick="productSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
    },
    clickEventForEditButton: function () {
        var entityGrid = $("#gridProduct").data("kendoGrid");
        var productObj = entityGrid.dataItem(entityGrid.select());

        if (productObj != null) {
            productHelper.FillProductDetailsInForm(productObj);
            ProductItemHelper.GenerateProductItemGrid(productObj.ModelId);
        }
      
      
    },
    
    GeneratePackageTypeCombo: function () {
        var dropdown = $("#cmbPackageType").kendoDropDownList({

            dataTextField: "text",
            dataValueField: "value",
            autoBind: false,
            //optionLabel: "All",
            index: 2,
            dataSource: [
                { text: "All" },
                { text: "Package", value: "1" },
                { text: "Item", value: "2" }

            ],
            change: function () {

                var value = this.value();
                if (value == 1 || value == 2) {
                    $("#gridProduct").data("kendoGrid").dataSource.filter({ field: "PackageType", operator: "eq", value: parseInt(value) });

                } else {
                    $("#gridProduct").data("kendoGrid").dataSource.filter({});

                }
            },

        }).data('kendoDropDownList');
        dropdown.value(0);

    },
};