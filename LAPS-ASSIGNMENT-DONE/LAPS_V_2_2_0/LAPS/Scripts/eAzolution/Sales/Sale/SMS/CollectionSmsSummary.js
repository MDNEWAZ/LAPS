
var gbUserLevel = 0;

var CollectionSmsSummaryHelper = {
    GenerateCollectionSmsSummary: function () {
        
        var receiveDateFrom = $("#dtRecievedDateFrom").data("kendoDatePicker").value();
        var receiveDateTo = $("#dtRecievedDateTo").data("kendoDatePicker").value();
     
        CollectionSmsSummaryHelper.GenerateCollectionSmsSummaryGrid(receiveDateFrom, receiveDateTo);            
        
    },

    GenerateCollectionSmsSummaryGrid: function (receiveDateFrom, receiveDateTo) {
        $("#smsSummaryOfCollection").kendoGrid({
            dataSource: CollectionSmsSummaryManager.CollectionSmsGridDataSource(receiveDateFrom, receiveDateTo),
            filterable: true,
            sortable: true,
            columns: CollectionSmsSummaryHelper.GetCollectionSmsColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    GetCollectionSmsColumns: function () {
        return columns = [
            { field: "ID", hidden: true },
            { field: "SMSIndex", title: "SMS Index", width: 50, filterable: false },
            { field: "RecievedDate", title: "SMS Recieve Date", width: 70, template: '#=kendo.toString(RecievedDate,"dd/MM/yyyy")#', filterable: false },
            { field: "SMSText", title: "SMS Text", width: 150, filterable: false },
            { field: "FromMobileNumber", title: "From Mobile", width: 70, filterable: false },
            { field: "Status", title: "Status", width: 60, filterable: false, template: "#=CollectionSmsSummaryManager.ShowStatus(data) #" },
            { field: "SystemDate", title: "System Date", width: 70, template: '#=kendo.toString(SystemDate,"dd/MM/yyyy")#', filterable: false },
            { field: "Edit", title: "Edit Sale", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="CollectionSmsSummaryManager.clickEventForEditSms()"/>', sortable: false },

        ];
    }
};

var CollectionSmsSummaryManager = {
    clickEventForEditSms: function () {
        var entityGrid = $("#smsSummaryOfCollection").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            $("#smsPopup").data("kendoWindow").open().center();
            CollectionSmsDetailsHelper.FillCollectiondSmsIntoPopip(selectedItem);
        }


    },
    ShowStatus: function (data) {
        if (data.Status == 2) {
            return "Amount Excceded";
        }
        else if (data.Status == 3) {
            return "Customer Not Found";
        }
        else if (data.Status == 4) {
            return "Provider Not Found";
        }
        else if (data.Status == 5) {
            return "SMS Format is Wrong or Invalid DateTime";
        }
        else if (data.Status == 9) {
            return "SMS Format is Invalid or Mismatch Parameter";
        }
        else if (data.Status == 11) {
            return "Stock Inventory Unavilable";
        }
        else {
            return "NA";
        }
    },
    CollectionSmsGridDataSource: function (receiveDateFrom, receiveDateTo) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: false,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            transport: {
                read: {
                    url: '../Sms/GetAllUnrecognizedCollectionSms/?receiveDateFrom=' + kendo.toString(receiveDateFrom,"MM/dd/yyyy") + "&receiveDateTo=" + kendo.toString(receiveDateTo,"MM/dd/yyyy"),
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
                        RecievedDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        },
                        SystemDate: {
                            type: "date",
                            template: '#= kendo.toString("MM/dd/yyyy") #'
                        }
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    }
};