var gbProductName = "";
var productDetailsManager = {

    InitislProductDetails: function () {
        //productDetailsHelper.GenerateManufuctureDateCombo();


        productDetailsHelper.GenerateProductCombo();

        var objProduct = productDetailsManager.GetProductType();
        $("#cmbType").data('kendoComboBox').setDataSource(objProduct);


        //---------------Item code -------------------------
        ProductItemHelper.ItemCodeInit();
        $("#btnAddItemCode").click(function() {
            ProductItemHelper.OpenItemCodePopup();
        });

    },
    SaveProduct: function () {
        var objProduct = productDetailsManager.GetDataFromCortolsAsA_Object();
        ProductItemHelper.CreateProductItemsList();
      

        if (objProduct.Model != "" && objProduct.ProductName != "") {
            var objProductInfo = JSON.stringify(objProduct).replace(/&/g, "^");
            var prodItemList = JSON.stringify(productItemsArray).replace(/&/g, "^");
            var removeItemList = JSON.stringify(removeItemArray);
            var jsonParam = 'strObjProductInfo:' + objProductInfo + ',productItemList:' + prodItemList + ',removeItemList:' + removeItemList;
            var serviceUrl = "../Product/SaveProduct/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }
        else {
            AjaxManager.MsgBox('warning', 'center', 'Minimum Requirement:', 'Please Enter Model And Package Name.',
                [{
                    addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                        $noty.close();
                        return false;
                    }
                }]);
        }
        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                productDetailsHelper.clearProductForm();
                AjaxManager.MsgBox('success', 'center', 'Success', 'Package Saved Or Updated Successfully.',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                          productItemsArray = [];
                          removeItemArray = [];
                      }
                  }]);
                $("#gridProduct").data("kendoGrid").dataSource.read();
            }
            else if (jsonData == "Exists") {
                AjaxManager.MsgBox('warning', 'center', 'Alresady Exist:', 'Package Model Already Exist.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
            }
            else if (jsonData == "Failed") {
                AjaxManager.MsgBox('warning', 'center', 'Alresady Exist:', 'Package Model Already Exist.',
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
            window.alert(error.statusText);
        }
    },
    GetDataFromCortolsAsA_Object: function () {

        var objProduct = new Object();
        objProduct.ModelId = $('#hdnModelId').val();
        objProduct.Model = $('#txtModel').val();
        objProduct.ProductName = $("#txtProductName").val();
        objProduct.TypeId = $("#cmbType").val();
        objProduct.Description = $("#txtDescription").val();
        objProduct.Capacity = $("#txtCapacity").val();
        objProduct.IsActive = $("#chkIsActive").is(":checked") == true ? 1 : 0;

        //Region New Filed added on 29/06/2016
        objProduct.IsDPFixedAmount = $("#chkIsDPFixedAmount").is(":checked") == true ? 1 : 0;
        objProduct.DefaultInstallmentNo = $("#txtDefaultInstallmentNo").val();
        //End Region

        // objProduct.CompanyId = CurrentUser.CompanyId;
        if (CurrentUser.CompanyType == "MotherCompany") {
            objProduct.CompanyId = CurrentUser.CompanyId;
        } else {
            objProduct.CompanyId = CurrentUser.RootCompanyId;
        }


        //objProduct.CompanyId = CurrentUser.RootCompanyId == 0 ? CurrentUser.CompanyId : CurrentUser.RootCompanyId;
        objProduct.TotalPrice = $("#lblTotalPrice").data('kendoNumericTextBox').value();
        objProduct.DownPayPercent = $("#txtDownPayPercent").val();
        objProduct.PackageType = $("#packageType input[type='radio']:checked").val();
        objProduct.ModelItemID = $("#txtItemId").val();
        return objProduct;
    },
    GetProductType: function () {
        var objProduct = "";
        var jsonParam = "";
        var serviceUrl = "../Product/GetProductType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objProduct = jsonData;
        }
        //function onFailed(error) {
        //    window.alert(error.statusText);
        //}
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objProduct;
    },
    GetMotherProductForEditProductCombo: function (productCode) {
        var objProduct = "";
        var jsonParam = "productCode=" + productCode;
        var serviceUrl = "../Product/GetMotherProductForEditProductCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objProduct = jsonData;
        }

        //function onFailed(error) {
        //    window.alert(error.statusText);
        //}
        function onFailed(jqXHR, textStatus, errorThrown) {
            window.alert(errorThrown);
        }
        return objProduct;
    },
    
};

var productDetailsHelper = {
    validator: function () {
        var data = [];
        var validator = $("#productDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }

    },
   
    GenerateProductCombo: function () {


        $("#cmbType").kendoComboBox({
            placeholder: "Select Product...",
            dataValueField: "TypeId",
            dataTextField: "Type",
            //dataSource: objProduct,
            filter: "contains",
            suggest: true,
            change: function () {
                var value = this.value();
                AjaxManager.isValidItem("cmbType", true);

            }
        });
    },
    GetMotherProductForEditProductCombo: function (productId) {
     
    },
    clearProductForm: function () {
        $('#txtModel').val('');
        $('#txtProductName').val('');
        $('#cmbType').data().kendoComboBox.value('');

        $('#hdnModelId').val('');
        $('#chkIsActive').removeAttr('checked', 'checked');

        $("#gridProductItems").data("kendoGrid").dataSource.data([]);
        $("#lblTotalPrice").data('kendoNumericTextBox').value('');
        $("#txtDownPayPercent").val("");
        $("#txtDescription").val("");
        $("input:radio").attr("checked", false);
        $('#chkIsDPFixedAmount').removeAttr('checked', 'checked');
        $("#txtDefaultInstallmentNo").val("");
        $("#txtItemId").val("");
        
        productItemsArray = [];
        removeItemArray = [];
        $("#percentSign").html(" (%)");
    }
};