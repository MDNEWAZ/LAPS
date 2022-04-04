var MobileDetailsManager = {
    SaveMobileInfo: function () {
      
  
        if (MobileDetailsManager.ValidateMobileInfoForm()) {
            var object = MobileDetailsHelper.CreateObjectFromFields();

            var obj = JSON.stringify(object);
            var jsonParam = 'mobile:' + obj;
            var serviceUrl = "../Mobile/SaveMobileInfo/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        }

        


            function onSuccess(jsonData) {
                if (jsonData == "Success") {

                    AjaxManager.MsgBox('success', 'center', 'Success:', 'Mobile Info Save Successfully',
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
    ValidateMobileInfoForm: function () {
        var data = [];

        var validator = $("#divDeailsvalidation").kendoValidator().data("kendoValidator"),
            status = $(".status");

        console.log($("#divDeailsvalidation").html())
        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
  
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
                    url: '../Mobile/MobileFeatureGrid/',
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
    },


    //clearMobileForm: function () {
    //    $("#hdnMobileId").val("0");
    //    $("#txtModelName").val("");
    //    $("#divDeailsvalidation").data('kendoGrid').dataSource.data("");
    //    $("#divDeailsvalidation > form").kendoValidator();
    //    $("#divDeailsvalidation").find("span.k-tooltip-validation").hide();
    //    var status = $(".status");

    //    status.text("").removeClass("invalid");

    //},

    SaveFeature: function () {


        
            var object = MobileDetailsHelper.CreateObjectFromFields3();

            var obj = JSON.stringify(object);
            var jsonParam = 'feature:' + obj;
            var serviceUrl = "../Mobile/SaveFeature/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        




        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Mobile Feature Save Successfully',
                    [
                        {
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#mblFeatureDiv").data("kendoGrid").dataSource.read();
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
  
    Delete: function (MobileId) {

        if (confirm('Do You Want to Delete')) {
            var jsonParam = 'mobileid:' + MobileId;
            var serviceUrl = "../Mobile/Delete";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);


           
            function onSuccess(jsonData) {
                if (jsonData == "Success") {

                    AjaxManager.MsgBox('success', 'center', 'Success:', 'Mobile Delete Successfully',
                        [
                            {
                                addClass: 'btn btn-primary',
                                text: 'Ok',
                                onClick: function ($noty) {
                                    $noty.close();

                                     $("#mblSummaryDiv").data("kendoGrid").dataSource.read();

                                    MobileSummaryManager.gridDataSource();
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
    }

};
var MobileDetailsHelper = {
    PopulateColorCombo: function () {
        var objColors = new Object();
        objColors = AjaxManager.GetDataSource("../Mobile/PopulateColorCombo");

        $("#cmbColor").kendoComboBox({
            placeholder: "Select Color",
            dataValueField: "ColorId",
            dataTextField: "Color",
            dataSource: objColors,
            filter: "contains",
            suggest: true,

        })
    },

    PopulateColorCombo2: function () {
        var objColors = new Object();
        objColors = AjaxManager.GetDataSource("../Mobile/PopulateColorCombo2");

        $("#cmbColor2").kendoComboBox({
            placeholder: "Select Color",
            dataValueField: "ColorId",
            dataTextField: "Color",
            dataSource: objColors,
            filter: "contains",
            suggest: true,

        })
    },
    //For Brand Combo
    PopulateBrandCombo: function () {
        var objBrands = new Object();
        objBrands = AjaxManager.GetDataSource("../Mobile/PopulateBrandCombo");

        $("#cmbBrand").kendoComboBox({
            placeholder: "Select Brand",
            dataValueField: "BrandId",
            dataTextField: "Brand",
            dataSource: objBrands,
            filter: "contains",
            suggest: true,
        })
    },
    PopulateBrandCombo2: function () {
        debugger;
        var objBrands = new Object();
        objBrands = AjaxManager.GetDataSource("../Mobile/PopulateBrandCombo2");

        $("#cmbBrand2").kendoComboBox({
            placeholder: "Select Brand",
            dataValueField: "BrandId",
            dataTextField: "Brand",
            dataSource: objBrands,
            filter: "contains",
            suggest: true,
        })
    },


    CreateObjectFromFields: function () {
      
        var object = new Object();

        object.MobileId = $("#hdnMobileId").val();
        object.ModelName = $('#txtModelName').val();
        object.BrandId = $('#cmbBrand').val();
        object.ColorId = $('#cmbColor').val();
        object.Price = $('#txtPrice').val();
        object.Is5G = $('input[name="Is5G"]:checked').val();
        object.IsSmart = $("#chkIsSmart").is(":checked") == true ? 1 : 0

        return object;
    },

    CreateObjectFromFields2: function () {

        var object = new Object();

        object.MobileId = $("#hdnMobileId2").val();
        object.ModelName = $('#txtModelName2').val();
        object.BrandId = $('#cmbBrand2').val();
        object.ColorId = $('#cmbColor2').val();
        object.Price = $('#txtPrice2').val();
        object.Is5G = $('input[name="Is5G2"]:checked').val();
        object.IsSmart = $("#chkIsSmart2").is(":checked") == true ? 1 : 0

        return object;
    },

    CreateObjectFromFields3: function () {

        var object = new Object();

      
        object.FeatureId = $('#hdnFeatureId').val();      
        object.Smart = $("#chkSmart").is(":checked") == true ? 1 : 0
        object.iOS = $("#chkiOS").is(":checked") == true ? 1 : 0
        object.Android = $("#chkAndroid").is(":checked") == true ? 1 : 0
        object.Support5G = $("#chkSupport5G").is(":checked") == true ? 1 : 0
        object.VRSupport = $("#chkVRSupport").is(":checked") == true ? 1 : 0

        return object;
    },

    clickEventForFeatureButton: function () {
        var initPopup = $("#popup2").kendoWindow({
            title: 'Mobile-Features',
            resizeable: false,
            width: "30%",
            actions: ["Pin", "Refresh", "Maximize", "Close"],
            modal: true,
        });
        initPopup.data("kendoWindow").open().center();
        $("#popup2").show();
    },

    GenerateMobileFeatureGrid: function () {

        $("#mblFeatureDiv").kendoGrid({

            dataSource: MobileDetailsManager.gridDataSource(),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            xheight: 450,
            filterable: true,
            sortable: true,
            columns: MobileDetailsHelper.GenerateFeatureColumns(),
            editable: false,
            navigatable: true,
            selectable: "row"
        })


    },


    GenerateFeatureColumns: function ()     {
        return columns = [


                { field: "hdnFeatureId", title: "hdnFeatureId", hidden: true, width: 100 },
                { field: "Smart", title: "Smart", width: 60, sortable: false, },
                { field: "iOS", title: "iOS", width: 60, sortable: false, },
                { field: "Android", title: "Android", width: 60, sortable: false, },
                { field: "Support5G", title: "Support5G", width: 60, sortable: false, },
                { field: "VRSupport", title: "VRSupport", width: 60, sortable: false, },
           //  { field: "Save", title: "Save", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Save" id="btnSave3"<span class="k-icon k-i-pencil"></span></button>', sortable: false },
          //  { field: "Delete", title: "Delete", filterable: false, width: 50, template: '<button type="button" class="k-button" value="Delete" id="btnDelete"  onClick="MobileSummaryHelper.clickEventForDeleteButton()" ><span class="k-icon k-i-pencil"></span></button>', sortable: false },

        ];
    },

};

