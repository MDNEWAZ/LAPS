$(document).ready(function () {

    ProductsDetailsHelper.ProductTypeNameCombo();
    //ProductsDetailsHelper.sectionNameDrop();

    $("#btnSave").click(function () {

        ProductsDetailsManager.AddProductInformation();
    });
    $("#btnClearAll").click(function () {
        ProductsDetailsHelper.clearProductsForm();
    });

    $("#btnPrint").click(function () {
        ProductsDetailsHelper.PrintTransferReport();
    });

    ProductsSummaryHelper.GenerateProductsGrid();
    ProductModelDetailsHelper.GenerateProductModelGrid(0);


});
