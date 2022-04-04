
var gbUserLevel = 0;

var SmsSummaryHelper = {
    GenerateSmsSummary: function () {
        SmsSummaryHelper.GenerateSmsSummaryGrid();
    },

    initSmsDate: function () {
        SmsSummaryHelper.SmsDateGenerate();
    },

    GenerateSmsSummaryGrid: function () {
        $("#smsSummary").kendoGrid({
            dataSource: SmsSummaryManager.SmsGridDataSource(),
            filterable: true,
            sortable: true,
            columns: SmsSummaryHelper.GetSmsColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    GetSmsColumns: function () {
        return columns = [
            { field: "SalesSmsId", hidden: true },
            { field: "SmsDate", title: "SMS Recieve Date", width: 70, template: '#=kendo.toString(SmsDate,"dd/MM/yyyy")#', filterable: false },
            { field: "BranchCode", title: "Branch Code", width: 50, filterable: false },
            { field: "SalesRepId", title: "Sales Rep. ID", width: 50, filterable: false },
            { field: "CustomerName", title: "Cusomer Name", width: 90, filterable: false },
            { field: "CustomerNid", title: "NID", width: 70, filterable: false },
            { field: "MobileNo1", title: "Mobile #1", width: 60, filterable: false },
            { field: "MobileNo2", title: "Mobile #2", width: 60, filterable: false },
            { field: "Package", title: "Package", width: 40, filterable: false },
            { field: "ExtraLight", title: "Extra Light", width: 50, filterable: false },
            { field: "ExtraSwitch", title: "Extra Switch", width: 50, filterable: false },
            { field: "DownPayment", title: "Down Payment", width: 50, filterable: false },
            { field: "InstallmentNo", title: "IM", width: 50, filterable: false },
            { field: "IsSd", title: "Is SD", template: "#=SmsSummaryManager.ShowIsSd(data) #", width: 30, filterable: false },
            { field: "Edit", title: "Edit Sale", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Edit" id="btnEdit" onClick="SmsSummaryManager.clickEventForEditSms()"/>', sortable: false },

        ];
    },


    SmsDateGenerate: function () {

        $("#dtSmsDateFrom").kendoDatePicker({
            depth: "year",
            format: "dd-MM-yyyy",
        });

        $("#dtSmsDateTo").kendoDatePicker({
            depth: "year",
            format: "dd-MM-yyyy",
        });

        //$("#dtSalesDateFrom").data("kendoDatePicker").value(new Date());
        //$("#dtSalesDateTo").data("kendoDatePicker").value(new Date());
    }
};

var SmsSummaryManager = {
    clickEventForEditSms: function () {
        var entityGrid = $("#smsSummary").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            $("#smsPopup").data("kendoWindow").open().center();
            SmsDetailsHelper.FillUnrecognizedSmsIntoPopip(selectedItem);
        }


    },
    ShowIsSd: function (data) {
        if (data.IsSd == 1) {
            return "YES";
        } else {
            return "NO";
        }
    },
    SmsGridDataSource: function () {

        var smsDateFrom = $("#dtSmsDateFrom").data("kendoDatePicker").value();
        var smsDateTo = $("#dtSmsDateTo").data("kendoDatePicker").value();

        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: false,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            transport: {
                read: {
                    url: '../Sms/GetAllUnrecognizedSms/?smsDateFrom=' + kendo.toString(smsDateFrom, "MM/dd/yyyy") + "&smsDateTo=" + kendo.toString(smsDateTo, "MM/dd/yyyy"),
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
                        SmsDate: {
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