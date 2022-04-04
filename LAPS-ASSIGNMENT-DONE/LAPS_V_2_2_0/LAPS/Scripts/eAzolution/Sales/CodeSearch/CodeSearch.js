
$(document).ready(function () {
    CodeSearchHelper.initCodeSearch();
    CodeSearchHelper.GenerateCustomerWithCodeGrid();

  
    //$("#btnCodeSearch").click(function () {
    //    $("input[type='text']").val("");
    //    $("#CodeSearchPopUp").data('kendoWindow').open().center();
    //    CodeSearchHelper.GenerateCustomerWithCodeGrid();
    //});

});



var CodeSearchManager = {
    ResendSms: function (selectedItem) {
        var objResendSms = JSON.stringify(selectedItem).replace(/&/g, "^");
        var jsonParam = 'objResendSms:' + objResendSms;
        var serviceUrl = "../Collection/ResendLicenseCodeSms/";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);


        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'License Code Resend Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();

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
    },

    customerWithCodeGridDataSource: function (customerCode, smsMobileNumber) {
        
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 15,

            transport: {
                read: {
                    url: '../Dashboard/GetCustomerWithCodeSummary/?customerCode=' + customerCode + '&smsMobileNumber=' + smsMobileNumber,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    async: false,
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: {
                model: {
                    fields: {

                        IssueDate: {
                            type: "date", template: "formate:'dd/MM/yyyy'"

                        },


                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    },

};

var CodeSearchHelper = {
    initCodeSearch: function () {

        $("#CodeSearchPopUp").kendoWindow({
            title: "Code Information",
            resizeable: false,
            width: "70%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
            visible: false,
        });
        $("#txtCustomerId").keypress(function (event) {
            if (event.keyCode == 13) {
                var customerCode = $("#txtCustomerId").val();
                // var objCustomerId = { CustomerId:customerId};
                CodeSearchHelper.SearchForCustomerWithCode(customerCode, "0");
            }
        });
        $("#txtSMSMobileNumber").keypress(function (event) {
            if (event.keyCode == 13) {
                var smsMobileNumber = $("#txtSMSMobileNumber").val();
                //var objSmsMobileNumber = { Phone: smsMobileNumber };
                CodeSearchHelper.SearchForCustomerWithCode("0", smsMobileNumber);
            }
        });

    },

    SearchForCustomerWithCode: function (customerCode, smsMobileNumber) {
        var customerWithCodeGrid = $("#divCustomerWithCodeGrid").data("kendoGrid");
        var data = CodeSearchManager.customerWithCodeGridDataSource(customerCode, smsMobileNumber);
        customerWithCodeGrid.setDataSource(data);
      
        if (data._view[0] == undefined) {
         
            AjaxManager.MsgBox('warning', 'center', 'Warning:', 'No license code found for this customer!',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();

                        }
                    }]);
            return false;
        }
       
       
    },
    GenerateCustomerWithCodeGrid: function () {
        $("#divCustomerWithCodeGrid").kendoGrid({
            dataSource: [],//CodeSearchManager.customerWithCodeGridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
                    
            },
            filterable: true,
            sortable: true,
            columns: CodeSearchHelper.GenerateCustomerWithCodeColumns(),
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },


    GenerateCustomerWithCodeColumns: function () {
        return columns = [
            { field: "Name", title: "Customer Name", width: 150 },
            { field: "BranchCode", title: "Branch Code", width: 60 },
            { field: "Phone2", title: "SMS Mobile Number", width: 150 },
            { field: "IssueDate", title: "Month", width: 80, template: '#= kendo.toString(IssueDate, "d")#' },//template: '#= kendo.parseDate(kendo.toString(IssueDate,"dd/MM/yyyy"),"dd/MM/yyyy")#'
            { field: "LType", title: "License Type", width: 80, template: '#= CodeSearchHelper.dynamicType(data)#' },
            { field: "Number", title: "Number", width: 80 },
            { field: "CustomerCode", title: "CustomerCode", width: 150, hidden: true },
          

            //{ field: "Name", title: "Name", width: 150 },
             { field: "Edit", title: "Resend", filterable: false, width: 100, template: '<input type="button" class="k-button" value="Resend" id="btnResendSms" onClick="CodeSearchHelper.clickEventForResendButton()"  />', sortable: false }
        ];

    },

    dynamicType: function (data) {
        if (data.LType == 1) {
            return "Initial License";
        }
        else if (data.LType == 2) {
            return "Renewal License";
        }
        else if (data.LType == 3) {
            return "Release License";
        }
    },

    clickEventForResendButton: function () {
        var entityGrid = $("#divCustomerWithCodeGrid").data("kendoGrid");
        var selectedItem = entityGrid.dataItem(entityGrid.select());
        CodeSearchManager.ResendSms(selectedItem);
    },
};