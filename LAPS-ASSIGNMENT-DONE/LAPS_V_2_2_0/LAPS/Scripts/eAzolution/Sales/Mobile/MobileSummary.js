var MobileSummaryManager = {
    gridDataSource: function () {
        
        var gridData = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    url: '../Mobile/MobileInfoGrid/',
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
        return gridData;
    }
};
var MobileSummaryHelper = {
    GenerateMobileSummaryGrid: function () {

        $("#mblSummaryDiv").kendoGrid({

            dataSource: MobileSummaryManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: MobileSummaryHelper.GenerateMobileColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"
        })

    },

    GenerateMobileColumns: function () {
        return columns = [


            { field: "MobileId", title: "MobileId", hidden: true,  width: 100 },
            { field: "ModelName", title: "ModelName", width: 80 },
           // { field: "BrandId", title: "Brand", width: 100 },
            { field: "Brand", title: "Brand", width: 80 },
            { field: "BrandId", title: "BrandId", hidden:true, width: 80 },

            { field: "Color", title: "Color", width: 80 },
            { field: "ColorId", title: "ColorId", hidden: true, width: 80 },

           // { field: "ColorId", title: "Color", width: 100 },
            { field: "Price", title: "Price" ,width: 80 ,sortable: true },
            { field: "Is5G", title: "Is5G", width: 60, sortable: false },
            { field: "IsSmart", title: "IsSmart",width: 60, sortable: false, },
            { field: "Edit", title: "Edit", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Edit" id="btnEdit" onClick="MobileSummaryHelper.clickEventForEditButton()" ><span class="k-icon k-i-pencil"></span></button>', sortable: false },
            { field: "Delete", title: "Delete", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Delete" id="btnDelete"  onClick="MobileSummaryHelper.clickEventForDeleteButton()" ><span class="k-icon k-i-pencil"></span></button>', sortable: false },

        ];
    },


    clickEventForEditButton: function () {
        var initPopup = $("#popup").kendoWindow({
            title: 'Details',
            resizeable: false,
            width: "30%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
        });
        initPopup.data("kendoWindow").open().center();
        $("#popup").show();

        $("#btnSave2").text("Update");

        var gridData = $("#mblSummaryDiv").data("kendoGrid");
        var selectedData = gridData.dataItem(gridData.select());
        $("#hdnMobileId2").val(selectedData.MobileId);
        $("#txtModelName2").val(selectedData.ModelName);
                                                                       
        $("#cmbBrand2").data("kendoComboBox").value(selectedData.BrandId);
        $("#cmbColor2").data("kendoComboBox").value(selectedData.ColorId);
        //$("#cmbColor").data("kendoComboBox").value(selectedData.ColorId);
        //$("#cmbBrand").data("kendoComboBox").value(selectedData.BrandId);
        $("#txtPrice2").val(selectedData.Price);
        if (selectedData.IsSmart == 1)
            $("#chkIsSmart2").prop("checked", true);
        else
            $("#chkIsSmart2").prop("checked", false);

        if (selectedData.Is5G == 1)
        {
            $("#rd5G2").prop("checked", true);
        }
        else
        {
            $("#rdnot5G2").prop("checked", true);

        }
        
    },
    clickEventForDeleteButton: function () {

        var gridData = $("#mblSummaryDiv").data("kendoGrid");
        var selectedData = gridData.dataItem(gridData.select());

     

        if (selectedData != null) {
            MobileDetailsManager.Delete(selectedData.MobileId);
        }

    },


    SaveMobileInfo: function () {
     
    
        var object = MobileDetailsHelper.CreateObjectFromFields2();

            var obj = JSON.stringify(object);
            var jsonParam = 'mobile:' + obj;
            var serviceUrl = "../Mobile/SaveMobileInfo/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        




        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Mobile Info Update Successfully',
                    [
                        {
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#mblSummaryDiv").data("kendoGrid").dataSource.read();
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

    },

};