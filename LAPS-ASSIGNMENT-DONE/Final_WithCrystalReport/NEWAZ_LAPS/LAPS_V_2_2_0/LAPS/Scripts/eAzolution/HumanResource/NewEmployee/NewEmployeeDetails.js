var EmployeeDetailsManager = {

    EducationDataSource: function (employeeId) {

        if (employeeId === 0) {
            return [];
        }
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            batch: true,
            //autoSync: true,

            transport: {

                read: {
                    url: '../NewEmployee/GetEducationSummary/?id=' + employeeId,
                    type: "POST",
                    dataType: "json",
                    cache: false,
                    async: false,
                    contentType: "application/json; charset=utf-8"

                },


                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                    return JSON.stringify(options);
                }
            },

            schema: {
                model: {
                    id: "EducationID",
                    fields: {
                        EducationID: { type: 'number', editable: true },
                        Exam: { editable: true },
                        Year: { editable: true },
                        Institute: { editable: true },
                        Result: { editable: true },
                        EmployeeId: { editable: false }

                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    },

    ProductDataSource: function () {

       
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            batch: true,
            //autoSync: true,

            transport: {

                read: {
                    url: '../NewEmployee/GetEducationSummary/',
                    type: "POST",
                    dataType: "json",
                    cache: false,
                    async: false,
                    contentType: "application/json; charset=utf-8"

                },


                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                    return JSON.stringify(options);
                }
            },

            schema: {
                model: {
                    id: "ProductID",
                    fields: {
                        ProductID: { type: 'number', editable: true },
                        ProductName: { editable: true },
                        Type: { editable: true },
                        Price: { editable: true },
                        Amount: { editable: true }

                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    },

    ExperienceDataSource: function (employeeId) {

        if (employeeId === 0) {
            return [];
        }
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            batch: true,
            //autoSync: true,

            transport: {

                read: {
                    url: '../NewEmployee/GetExperienceSummary/?id=' + employeeId,
                    type: "POST",
                    dataType: "json",
                    cache: false,
                    async: false,
                    contentType: "application/json; charset=utf-8"

                },


                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                    return JSON.stringify(options);
                }
            },

            schema: {
                model: {
                    id: "ExperienceID",
                    fields: {
                        ExperienceID: { type: 'number', editable: true },
                        Company: { editable: true },
                        FromDate: { editable: true },
                        ToDate: { editable: true },
                        Remarks: { editable: true },
                        EmployeeId: { editable: false }

                    }
                },
                data: "Items", total: "TotalCount"
            }
        });
        return gridDataSource;
    },
    SaveEmployeeInformation: function () {


        var obj = EmployeeDetailsHelper.CreateEmployeeObject();

        var objInfo = JSON.stringify(obj).replace(/&/g, "^");
        var jsonParam = 'employee=' + objInfo;
        var serviceUrl = "../NewEmployee/NewSaveEmployee/";
        AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);


        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Employee Saved Successfully',
                [
                {
                    addClass: 'btn btn-primary',
                    text: 'Ok',
                    onClick: function($noty) {
                        $noty.close();
                        EmployeeDetailsHelper.clearEmployeeForm();
                       
                            $("#divgridEmployeeSummary").data("kendoGrid").dataSource.read();
                        }
                    }]);
            }
            else if (jsonData == "Employee Already Exist") {

                AjaxManager.MsgBox('warning', 'center', 'Already Exists:', 'Employee Already Exist !',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
        }
    }
};
var EmployeeDetailsHelper = {
   

    GenerateEducationGrid: function (employeeId) {

        $("#gridEducationEmp").kendoGrid({
            dataSource: EmployeeDetailsManager.EducationDataSource(employeeId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: EmployeeDetailsHelper.GenerateGridColumns(),
            editable: true,
            toolbar: ["create"],
            //navigatable: true,
            selectable: "row,multiple",
            dataBound: function (e) {

            }

        });
    },

    GenerateProductGridForCheckedBox: function () {

        $("#gridProductInfo").kendoGrid({
            dataSource: EmployeeDetailsManager.ProductDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: EmployeeDetailsHelper.GenerateProductGridColumns(),
            editable: true,
            toolbar: ["create"],
            //navigatable: true,
            selectable: "row",
            dataBound: function (e) {

            }

        });
    },
    GenerateProductGridColumns: function () {
        return columns = [

            { field: "ProductName", title: "Product Name", width: 100 },
             { field: "Type", title: " Type", width: 100 },
             { field: "Price", title: "Price", width: 100 },
            { field: "Amount", title: "Amount", width: 150 }

        ];

    },
    GenerateExperienceGrid: function (employeeId) {
        $("#gridExperienceEmp").kendoGrid({
            dataSource: EmployeeDetailsManager.ExperienceDataSource(employeeId),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: EmployeeDetailsHelper.GenerateExperienceGridColumns(),
            editable: "row",
            //toolbar: ["create"],
            //navigatable: true,
            selectable: "row",
            dataBound: function(e) {

            }
            
        });
    },
    GenerateGridColumns: function () {
        return columns = [

            { field: "Exam", title: "Exam", width: 100 },
             { field: "Year", title: " Year", width: 100 },
             { field: "Institute", title: "Institute", width: 100 },
            { field: "Result", title: "Result", width: 150 }

        ];

    },

    GenerateExperienceGridColumns: function() {
        return columns = [
        { field: "Company", title: "Company", width: 100 },
        { field: "FromDate", title: " From", width: 100, template: '#=kendo.toString(kendo.parseDate(FromDate), "dd-MMM-yy")#' },
        { field: "ToDate", title: "To", width: 100 , template: '#= kendo.toString(kendo.parseDate(ToDate),"dd-MMM-yy") #' },
        { field: "Remarks", title: "Remarks", width: 150 },
        { field: "Edit", title: "Edit", filterable: false, width: 30, template: '<button type="button" value="Edit" id="btnEdit" onClick="EmployeeDetailsHelper.clickEventForEditButton()" ><span class="k-icon k-i-search"></span></button>', sortable: false }
        ];
        
    },

     

    populateExperienceDetails: function(obj) {
        $("#hdnExperienceID").val(obj.ExperienceID);
        $("#txtCompany").val(obj.Company);
        //var dt = kendo.parseDate(obj.FromDate, "dd-MMM-yy");
        $("#txtFromDate").data("kendoDatePicker").value(obj.FromDate);
        $("#txtToDate").data("kendoDatePicker").value(obj.ToDate);
        $("#txtRemarks").val(obj.Remarks);
    },
    clearEmployeeForm: function () {
        $("#hdnEmployeeId").val("0");
        $("#txtEmployeeName").val("");
        $("#txtMobile").val("");
        $("#txtEmail").val("");
        $("#txtDesignation").val("");
        $("#EmployeeDetailsDiv > form").kendoValidator();
        $("#EmployeeDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");
        status.text("").removeClass("invalid");
        $("#gridEducationEmp").data("kendoGrid").dataSource.data([]);
        $("#gridExperienceEmp").data("kendoGrid").dataSource.data([]);
    },

    clearExperienceForm: function () {
        $("#hdnExperienceID").val("0");
        $("#txtCompany").val("");
        $("#txtFromDate").val("");
        $("#txtToDate").val("");
        $("#txtRemarks").val("");
        $("#EmployeeDetailsDiv > form").kendoValidator();
        $("#EmployeeDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");
        status.text("").removeClass("invalid");
    },
    CreateEmployeeObject: function () {
        var obj = new Object();
        obj.EmployeeId = $("#hdnEmployeeId").val();
        obj.EmployeeName = $("#txtEmployeeName").val();
        obj.Mobile = $("#txtMobile").val();
        obj.Email = $("#txtEmail").val();
        obj.Designation = $("#txtDesignation").val();

        var edu = $("#gridEducationEmp").data('kendoGrid').dataSource.data();
        obj.education = edu;

        var exp = $("#gridExperienceEmp").data('kendoGrid').dataSource.data();
        obj.Experiences = exp;

        obj.Employees = gbemployeegrid;
        return obj;
    },

    AddExperienceObjectInGrid: function () {
        var obj = new Object();
        obj.ExperienceID = $("#hdnExperienceID").val() === "" ? "0" : $("#hdnExperienceID").val();
        obj.Company = $("#txtCompany").val();
        obj.FromDate = $("#txtFromDate").data("kendoDatePicker").value();
        obj.ToDate = $("#txtToDate").data("kendoDatePicker").value();
        obj.Remarks = $("#txtRemarks").val();

        var gridData = $("#gridExperienceEmp").data('kendoGrid');
        //EmployeeDetailsHelper.DeleteRowsFromExperienceGrid();

        var selectedItem = gridData.dataItem(gridData.select());
        if (obj.ExperienceID != 0) {
            if (selectedItem.ExperienceID == obj.ExperienceID) {
                var item = gridData.dataSource.get(obj.ExperienceID);
                gridData.dataSource.remove(item);
            }
        }
        gridData.dataSource.add(obj);

        


        return obj;
    },
    populateEmployeeDetails: function (obj) {
       
        EmployeeDetailsHelper.clearEmployeeForm();
        $("#hdnEmployeeId").val(obj.EmployeeID);
        $("#txtEmployeeName").val(obj.EmployeeName);
        $("#txtMobile").val(obj.Mobile);
        $("#txtEmail").val(obj.Email);
        $("#txtDesignation").val(obj.Designation);

        EmployeeDetailsHelper.GenerateEducationGrid(obj.EmployeeID);

        EmployeeDetailsHelper.GenerateExperienceGrid(obj.EmployeeID);
        EmployeeSummaryManager.GetEmployeeInfoData(obj.EmployeeID);

        EmployeeSummaryHelper.GenerateCheckedBoxEmployeeGrid();

    }
};