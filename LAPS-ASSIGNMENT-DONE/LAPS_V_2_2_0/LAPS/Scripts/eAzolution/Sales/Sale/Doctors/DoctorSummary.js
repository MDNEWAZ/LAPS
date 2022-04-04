var DoctorSummary = "";
var DoctorSummaryManager = {
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
                    url: '../Doctors/GetDoctorSummary/',

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
var DoctorSummaryHelper = {

    GenerateDoctorGrid: function () {
   
        $("#DoctorSummaryDiv").kendoGrid({
            dataSource: DoctorSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: DoctorSummaryHelper.GeneratedDoctorColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"

            //selectable: false

        });
    },
    GeneratedDoctorColumns: function () {
        return columns = [
        { field: "DoctorId", hidden: true },
        { field: "DoctorName", title: "Doctor Name", width: 50 },
        { field: "RegNo", title: "  Reg No", width: 50, sortable: false },
        { field: "DoctorGender", title: "Gender", width: 50, sortable: true },
        { field: "DepartmentName", title: "Department Name", width: 50, sortable: false },
        { field: "Exam", title: "Exam", width: 50, sortable: false },
        { field: "Year", title: "Year", width: 50, sortable: false, template: "#=kendo.toString(kendo.parseDate(Year,'dd MMMM yyyy'),'dd MMMM yyyy') == '01 January 0001' ? '' : kendo.toString(kendo.parseDate(Year,'dd-MMM-yyyy'),'dd-MMM-yyyy')#" },
        { field: "DoctorSection", title: "Section Name", width: 50, sortable: false },
        { field: "IsActive", title: "Status", width: 50, sortable: false, template: "#=IsActive==1?'Active':'In Active'#" },
        { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Edit" id="btnEdit" onClick="DoctorSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-pencil"></span></button>', sortable: false },
        { field: "Delete", title: "Delete", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Delete" id="btnDelete" onClick="DoctorSummaryHelper.clickEventForDeleteButton()" ><span class="k-icon k-i-trask"></span></button>', sortable: false }

        ];
    },
    clickEventForEditButton: function () {

        var entityGrid = $("#DoctorSummaryDiv").data("kendoGrid");

        var selectedItem = entityGrid.dataItem(entityGrid.select());
        if (selectedItem != null) {
            DoctorDetailsHelper.populateDoctorDetails(selectedItem);
        }

        },
        clickEventForDeleteButton: function () {

            var entityGrid = $("#DoctorSummaryDiv").data("kendoGrid");

            var selectedItem = entityGrid.dataItem(entityGrid.select());
            if (selectedItem != null) {
          
            DoctorSummaryHelper.Delete(selectedItem.DoctorId);

            }

        } ,

        Delete: function (DoctorId) {
           

            var jsonParam = 'DoctorId:' + DoctorId;
                var serviceUrl = "../Doctors/DeleteDoctor/";
                AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
  

            function onSuccess(jsonData) {
                if (jsonData == "Success") {

                    AjaxManager.MsgBox('success', 'center', 'Success:', 'Doctor Delete Successfully',
                    [
                        {
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#DoctorSummaryDiv").data("kendoGrid").dataSource.read(); DoctorDetailsHelper.clearDoctorForm();
                            }
                        }
                    ]);
                } else {
                    AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                    [
                        {
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                            }
                        }
                    ]);
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