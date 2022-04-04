$(document).ready(function(){
    MobileDetailsHelper.PopulateColorCombo();
    MobileDetailsHelper.PopulateBrandCombo();
    MobileDetailsHelper.PopulateColorCombo2();
    MobileDetailsHelper.PopulateBrandCombo2();

    $("#mblFeatureDiv").click(function () {
        MobileDetailsHelper.clickEventForFeatureButton();
    });
   
    $("#btnSave").click(function () {
        MobileDetailsManager.SaveMobileInfo();
    });

    $("#btnSave2").click(function () {
        MobileSummaryHelper.SaveMobileInfo();
    });


    $("#btnSave3").click(function () {
        MobileDetailsManager.SaveFeature();
    });
    //$("#btnClearAll").click(function () {
    //    MobileDetailsManager.clearMobileForm();
    //});

    $("#btnDelete").click(function () {
        MobileDetailsManager.Delete();
    });

    MobileSummaryHelper.GenerateMobileSummaryGrid();
    //ProductModelDetailsHelper.GenerateProductModelGrid(0);
    MobileDetailsHelper.GenerateMobileFeatureGrid();




    $("#btnprint").click(function () {
      MobileSummaryHelper.PrintMobileInfo();


    });
});