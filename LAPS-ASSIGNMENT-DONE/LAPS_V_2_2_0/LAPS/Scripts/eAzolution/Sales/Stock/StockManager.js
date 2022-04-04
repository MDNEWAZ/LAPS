
var gbIsViewer;
$(document).ready(function () {
    
    if (CurrentUser != null) { gbIsViewer = CurrentUser.IsViewer; }
    if (gbIsViewer == 1) { HideAllOperationalButton.HideAllOperationalButton(); }
    
    stockDetailsHelper.GeneratePackageCombo(null);
    $("#txtQty").kendoNumericTextBox({ formate: '#' });
    
    
    //empressCommonHelper.GenerateAllBranchCombo(CurrentUser.CompanyType == "MotherCompany" ? CurrentUser.CompanyId : CurrentUser.RootCompanyId, "cmbBranch");
    empressCommonHelper.GenerateAllBranchCombo(0, "cmbBranch");
   stockManagerHelper.showAndHide();
    //stockManager.GenerateStockGrid(modelId);

    $("#StockBy input[type='radio']").change(function () {
        var checked = $("#StockBy input[type='radio']:checked").val();
        if (checked == 1) {
            $("#liQty").show();
        }
        else if (checked == 2) {
            $("#liQty").hide();
        }

        stockManagerHelper.GenerateGrid();
    });
    stockManagerHelper.initPopUp();
    stockDetailsHelper.InitStockDetails();

    $("#btnAddNewStock").click(function () {
        var validator = $("#stockDetailsDiv").kendoValidator().data("kendoValidator"),
           status = $(".status");
        if (validator.validate()) {
            stockDetailsManager.UpdateStock();
        }
    });

    $("#btnStockView").click(function () {
        stockHelper.clickEventForViewStock();
    });
    
    $("#btnStockAdjustment").click(function () {
        stockHelper.clickEventForStockAdjustment();
    });
    
    $("#btnClearStockDetails").click(function () {
        stockDetailsHelper.ClearStockDetails();
    });


});

var stockManagerHelper = {
    showAndHide: function () {
        $("#StockType input[type='radio']").change(function () {
            var checked = $("#StockType input[type='radio']:checked").val();
            if (checked == 1) {
                $("#liBranch").hide();
                $("#cmbBranch").data('kendoComboBox').value("");
            }
            else if (checked == 2) {
                $("#liBranch").show();
            }
        });
    },
    
    openStockAdjustmentPopup: function () {
        $("#divProductStockAdjustment").kendoWindow({
            title: "Stock Details",
            resizable: false,
            modal: true,
            width: "60%",
            draggable: true,
            open: function (e) {
                this.wrapper.css({ top: 50 });
            }
        });
    },
    initPopUp: function () {

        $("#stockDetailsPopupWindo").kendoWindow({
            title: "Stock Details",
            resizable: false,
            modal: true,
            width: "60%",
            draggable: true,
            open: function (e) {
                this.wrapper.css({ top: 50 });
            }
        });
    },

    GenerateGrid: function () {

        var modelId = $("#cmbPackage").data('kendoComboBox').value();
   
        var packageWiseOrItemWise = $("#StockBy input[type='radio']:checked").val();
        if (packageWiseOrItemWise == 1) {//1 mean Package Wise
            StockProductItemsHelper.GenerateStockItemsGridWithData(modelId);
        }
        else if (packageWiseOrItemWise == 2) {//2 means item wise
            StockProductItemsHelper.GenerateStockItemsGrid(modelId);
        }
    },
    
    HideAllOperationalButton: function () {
        $("#btnAddNewStock").hide();
        $("#btnStockAdjustment").hide();
       
    }
};