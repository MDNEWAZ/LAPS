var UnsatteledCollectionSummaryManager = {
   

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
                    url: '../UnsatteledCollection/GetUnsatteledCollectionSummaryForGrid/',

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

var UnsatteledCollectionSummaryHelper = {
    GenerateUnsatteledCollectionGrid: function () {
        $("#unsatteledCollectionGrid").kendoGrid({
            dataSource: UnsatteledCollectionSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: UnsatteledCollectionSummaryHelper.GenerateUnsatteledCollectionColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    

    GenerateUnsatteledCollectionColumns: function () {
        return columns = [
             { field: "ID", title: "ID", width: 150,hidden:true },
            { field: "RecievedDate", title: "Recieved Date", width: 150, template: "#= kendo.toString(kendo.parseDate(RecievedDate,'dd/MM/yyyy'),'dd/MM/yyyy') #" },//kendo.toString(RecievedDate,'dd/MM/yyyy')
            { field: "SMSText", title: "SMSText", width: 150 },
            { field: "FromMobileNumber", title: "Payment From", width: 150 },
            { field: "Status", title: "Status", width: 150, template: '#= Status==3?"Unsatteled":"Sattled" #' },
            { field: "SystemDate", title: "SystemDate", width: 150, template: "#= kendo.toString(kendo.parseDate(RecievedDate,'dd/MM/yyyy'),'dd/MM/yyyy') #",hidden:true },
    
            
             { field: "Edit", title: "Action", filterable: false, width: 100, template: '<input type="button" class="k-button" value="Sattel" id="btnEdit" onClick="UnsatteledCollectionSummaryHelper.clickEventForEditButton()"  />', sortable: false }
        ];

    },
    
    clickEventForEditButton: function () {
        $("#sattelPopupWindow").data('kendoWindow').open().center();
        

      
        //var test = "My cow always gives milk";

        //var testRE = test.match("cow(.*)milk");
    
        //var valll = testRE[1];
        var entityGrid = $("#unsatteledCollectionGrid").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            SattelDetailsHelper.FillASattelDetailsInForm(selectedItem);
        }
       

    },
    
};