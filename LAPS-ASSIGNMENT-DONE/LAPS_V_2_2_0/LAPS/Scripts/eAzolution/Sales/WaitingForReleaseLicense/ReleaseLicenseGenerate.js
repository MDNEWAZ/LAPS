
var gbReleaseLisenceObj = "";

$(document).ready(function () {
  
   
    releaseLicenseHelper.GenerateReleaseLicenseGrid();
    
    releaseLicenseHelper.releaseLicenseGridDataSet(0, 0);
    
    releaseLicenseHelper.ReleaseLicensePopUpWindow();
    $("#btnGenerateReleaseLicense").click(function() {
        releaseLicenseManager.GenerateReleaseLisence();
    });
    
    $('input:radio[name=varificationtype]').click(function () {
        $("#divReleaseButton").show();
    });

});

var releaseLicenseManager = {
    gridDataSource: function (companyId, branchId) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Dashboard/GetCustomerForReleaseLisenceGridData/?companyId=' + companyId + '&branchId=' + branchId,
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
                        DueDate: { type: 'date', template: '#= kendo.toString("MM/dd/yyyy") #' }
                    }
                },
                data: "Items", total: "TotalCount"
            }
        });

        return gridDataSource;
    },
   
    releaseLisenceGridDataSource: function (invoiceNo) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: false,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            //pageSize: 5,
            transport: {
                read: {
                    url: '../Sale/GetAllInstalmentByInvoiceNo/?invoiceNo=' + invoiceNo,
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
        gridDataSource.filter({ field: "Status", operator: "eq", value: parseInt(0) });
        return gridDataSource;
    },
    
    GenerateReleaseLisence: function () {
        
        var varificationType = $('input[name=varificationtype]:checked').val();

        var objReleaseLicense = JSON.stringify(gbReleaseLisenceObj).replace(/&/g, "^");// this global value is filling by (clickEventForDetailsButton) function

        var jsonParam = 'strobjReleaseLicenseInfo:' + objReleaseLicense + ",varificationType:" + varificationType;
        var serviceUrl = "../Collection/GenerateReleaseLisenceFromRootUser/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        
        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Release License Generated and SMS Send Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            $("#releaseLicenseGenerateGrid").data("kendoGrid").dataSource.read();
                            $("#divInstallmentPopup").data('kendoWindow').close();
                        }
                    }]);

            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Failed', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Failed', error.statusText,
                         [{
                             addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                 $noty.close();
                             }
                         }]);
        }
    }
};


var releaseLicenseHelper = {
    GenerateReleaseLicenseGrid: function () {

        $("#releaseLicenseGenerateGrid").kendoGrid({
            dataSource: [],
            //dataSource: pendingRedCustomerManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: releaseLicenseHelper.GenerateColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },
    GenerateColumns: function () {
        return columns = [
            { field: "SInvoice", title: "SInvoice", width: 100, hidden: true },
            { field: "Status", title: "Status", width: 100, hidden: true },
            { field: "Number", title: "Number", width: 100, hidden: true },
            { field: "DueDate", title: "DueDate", width: 100, hidden: true, template: '#=kendo.toString(DueDate,"dd-MMM-yyyy")#' },
            { field: "SaleId", title: "SaleId", width: 100, hidden: true },
            { field: "CustomerId", title: "CustomerId", width: 100, hidden: true },
            { field: "CustomerCode", title: "Customer ID", width: 100, hidden: false },
            { field: "Name", title: "Name", width: 100 },
            { field: "Phone", title: "Phone", width: 100 },
            { field: "BranchCode", title: "Branch Code", width: 100 },

             { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<input type="button" class="k-button" value="Details" id="btnEdit" onClick="releaseLicenseHelper.clickEventForDetailsButton()"  />', sortable: false }



            //{ field: "Status", title: "Status", width: 100, template: "#=Status==1?'Paid':'Unpaid'#" },
            //{ field: "Edit", title: "Rating", width: 80, template: "#=DueCollectionHelper.RatingSetforCustomer(data)#" }
        ];
    },
  
    clickEventForDetailsButton: function () {
        
        gbReleaseLisenceObj = "";
        var entityGrid = $("#releaseLicenseGenerateGrid").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());

        gbReleaseLisenceObj = selectedItem;

        $("#divInstallmentPopup").data('kendoWindow').open().center();
        releaseLicenseHelper.GenerateInstalmentGrid(selectedItem.SInvoice);
    },
    ReleaseLicensePopUpWindow: function () {
        $("#divInstallmentPopup").kendoWindow({
            title: "Release License Details",
            resizeable: false,
            width: "50%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
        });
    },
    GenerateInstalmentGrid: function (sInvoice) {
        $("#installmentGrid").kendoGrid({
            dataSource: releaseLicenseManager.releaseLisenceGridDataSource(sInvoice),
            filterable: true,
            sortable: true,
            columns: releaseLicenseHelper.GenerateInstalmentColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
            dataBound: function () {
                var dataView = this.dataSource.view();
                for (var i = 0; i < dataView.length; i++) {
                    var uid = dataView[i].uid;

                    if (dataView[i].Status === 1) {
                        //$("#gridInstalment tbody").find("tr[data-uid=" + uid + "]").css("background-color", "#ff4040");
                        $("#installmentGrid tbody").find("tr[data-uid=" + uid + "]").css("color", "#006633");
                    }
                    if (dataView[i].Status === 2) {
                        //$("#gridInstalment tbody").find("tr[data-uid=" + uid + "]").css("background-color", "#FFAE00");
                        $("#installmentGrid tbody").find("tr[data-uid=" + uid + "]").css("color", "#FFAE00");
                    }

                }
            },
        });

    },
    GenerateInstalmentColumns: function () {
        return columns = [
           { field: "ProductNo", title: "ProductNo", hidden: true },
           { field: "Number", title: "Inst. No", width: 70 },
           { field: "SInvoice", title: "Invoice No", },
           { field: "DueDate", title: "Due Date", template: '#=kendo.toString(DueDate,"dd-MMM-yyyy")#' },
           { field: "Amount", title: "Amount", },
           { field: "ReceiveAmount", title: "Recvd. Amount.", },
           { field: "DueAmount", title: "Due Amount", },
           //{ field: "Status", title: "Status", template: "#= Status==0?'Unpaid':'Paid'#" },
            { field: "Status", title: "Status", template: "#= releaseLicenseHelper.dynamicStatus(data) #" }
           //{ field: "Edit", title: "Action", filterable: false, template: "#=Status==0?InstallmentDetailsHelper.buttonTemplate():''#", sortable: false }//#=Status==1?"InActive":"Active"#


        ];
    },
    dynamicStatus: function (data) {
        if (data.Status == 0) {
            return "Unpaid";
        }
        else if (data.Status == 1) {
            return "Paid";
        }
        else if (data.Status == 2) {
            return "Partial Paid";
        }
    },
    releaseLicenseGridDataSet: function (companyId, branchId) {

        var data = releaseLicenseManager.gridDataSource(companyId, branchId);
        $("#releaseLicenseGenerateGrid").data().kendoGrid.setDataSource(data);
    },

   
};


