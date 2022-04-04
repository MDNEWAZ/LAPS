/// <reference path="ProductSummary.js" />
/// <reference path="ProductDetails.js" />
/// <reference path="LicenseHistory.js" />
/// <reference path="../../Common/common.js" />
/// <reference path="~/Scripts/eAzolution/Sales/Product/ProductSummary.js" />
/// <reference path="~/Scripts/eAzolution/Sales/Product/ProductDetails.js" />
/// <reference path="~/Scripts/eAzolution/Sales/Stock/Stock.js" />
/// <reference path="~/Scripts/eAzolution/Sales/Stock/StockDetails.js" />
/// <reference path="ProductItems.js" />
/// <reference path="../Stock/StockAdjustment.js" />

$(document).ready(function () {
    productSummaryManager.GenerateProductGrid();
  
    productDetailsManager.InitislProductDetails();
 
    $('#cmbModelStock').change(function () {
        stockDetailsManager.ProductInfoFillByModel();
    });

    $('#cmbModelStockAdjustment').change(function () {
        stockAdjustmentManager.StockAdjustmentByModel();
    });
  
    ProductItemHelper.GenerateProductItemGrid(0);

    $("#lblTotalPrice").kendoNumericTextBox({ formate: "#", min: 0 });
    
   
    productSummaryHelper.GeneratePackageTypeCombo();
    $("#cmbPackageType").parent().css('width', "10em");

    //DownPayment Fixed ChangeEvent (New Code on 29/06/2016)
    $("#percentSign").html(" (%)");
    $("#chkIsDPFixedAmount").click(function () {
        $("#txtDownPayPercent").val("");
        if ($("#chkIsDPFixedAmount").is(':checked') == true) {
            $("#percentSign").html(" (Fixed)");
        } else {
          
            $("#percentSign").html(" (%)");
        }
    });
});
var productHelper = {
    FillProductDetailsInForm: function (objProduct) {
      
        productDetailsHelper.clearProductForm();
        $('#hdnModelId').val(objProduct.ModelId);
        $('#txtModel').val(objProduct.Model);

        $("#txtProductName").val(objProduct.ProductName);
        $("#txtDescription").val(objProduct.Description);
        $("#hdnColor").val(objProduct.Color);
        $("#txtCapacity").val(objProduct.Capacity);
        $("#hdnFlag").val(objProduct.hdnFlag);
        $("#lblTotalPrice").data('kendoNumericTextBox').value(objProduct.TotalPrice);
    
        var product = $("#cmbType").data("kendoComboBox");
        product.value(objProduct.TypeId);
        if (objProduct.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }
        $("#txtDownPayPercent").val(objProduct.DownPayPercent);

      
        if (objProduct.PackageType == 1) {
            $("#rdIsPackage").prop("checked", true);
        } else if (objProduct.PackageType == 2) {
            $("#rdIsItem").prop("checked", true);
        }

        //New Filed added by Rubel on 29/06/2016
        if (objProduct.IsDPFixedAmount == 1) {
            $("#chkIsDPFixedAmount").prop('checked', 'checked');
            $("#percentSign").html(" (Fixed)");
        } else {
            $("#chkIsDPFixedAmount").removeProp('checked', 'checked');
            $("#percentSign").html(" (%)");
        }
        $("#txtDefaultInstallmentNo").val(objProduct.DefaultInstallmentNo);

        $("#txtItemId").val(objProduct.ModelItemID);
    },

};


