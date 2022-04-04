var ProductName = "";
var ProductsDetailsManager = {


    AddProductInformation: function () {

        if (ProductsDetailsHelper.ValidateProductsInfoForm()) {
            var objProducts = ProductsDetailsHelper.CreateProductsObject();
            var objProductsInfo = JSON.stringify(objProducts);
            var jsonParam = 'product:' + objProductsInfo;
            var serviceUrl = "../ProductType/AddProduct/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }



        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Products Save Successfully',
                    [
                        {
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#ProductsSummaryDiv").data("kendoGrid").dataSource.read();
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

var ProductsDetailsHelper = {
    ProductTypeNameCombo: function () {

        var objProducts = new Object;
        objProducts = AjaxManager.GetDataSource("../ProductType/GetAllProductType");

        $("#txtProductTypeName").kendoComboBox({
            placeholder: "Select ProductType Name",
            dataTextField: "ProductTypeName",
            dataValueField: "ProductTypeId",
            dataSource: objProducts,
            filter: "contains",
            suggest: true,
            change: function () {
                var val = this.value();
                ProductTypeNameCombo(val);
            }
        });

    },



    clearProductsForm: function () {
        $("#hdnProductsId").val("0");
        $("#txtProductName").val("");
        //$("#txtProductModel").val("");
        $("#txtProductCode").val("");
        //$("#txtProductPrice").val("");
        $("#txtProductTypeName").data("kendoComboBox").value("");
       
        $('#chkIsActive').attr('checked', false);
        $("#ProductModelDetailsDiv").data('kendoGrid').dataSource.data("");
        $("#ProductsDetailsDiv > form").kendoValidator();
        $("#ProductsDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");

    },

    CreateProductsObject: function () {
        var objProducts = new Object();
        objProducts.ProductId = $("#hdnProductsId").val();
        objProducts.ProductName = $("#txtProductName").val();
       // objProducts.ProductModel = $("#txtProductModel").val();
        objProducts.ProductCode = $("#txtProductCode").val();
        //objProducts.ProductPrice = $("#txtProductPrice").val();
       // objProducts.Gender = $('input[name="rdoGender"]:checked').val();
        objProducts.ProductTypeId = $("#txtProductTypeName").data("kendoComboBox").value();
        //objProducts.SectionId = $("#SectionDrop").data("kendoDropDownList").value();
        objProducts.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;

        var ProductModelObj = ProductsDetailsHelper.CreateDetails();
        objProducts.ProductModel = ProductModelObj;

        return objProducts;
    },

    
    CreateDetails: function () {
       var journeyDetailsObj = [];
        var gridSummary = $("#ProductModelDetailsDiv").data("kendoGrid");
        var gridData = gridSummary.dataSource.data();
        for (var i = 0; i < gridData.length; i++) {
            var obj = gridData[i];

            var objDetails = new Object();
            objDetails.ProductModelId = obj.ProductModelId == undefined ? 0 : obj.ProductModelId;
            objDetails.ProductModelName = obj.ProductModelName == undefined ? "" : obj.ProductModelName.replace(/'/g, '"');
            objDetails.ProductModelPrice = obj.ProductModelPrice == undefined ? 0 : obj.ProductModelPrice;
       

            journeyDetailsObj.push(objDetails);
        }
        return journeyDetailsObj;
    },

    ValidateProductsInfoForm: function () {
        var data = [];

        var validator = $("#ProductsDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
  
    populateProductsDetails: function (objProducts) {

        ProductsDetailsHelper.clearProductsForm();
        $("#hdnProductsId").val(objProducts.ProductId);
        $("#txtProductName").val(objProducts.ProductName);
       
        $("#txtProductCode").val(objProducts.ProductCode);
      
   
        $("#txtProductTypeName").data("kendoComboBox").value(objProducts.ProductTypeId);

        if (objProducts.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }
      
        ProductModelDetailsHelper.GenerateProductModelGrid(objProducts.ProductId );

    }


};