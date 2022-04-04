$(document).ready(function () {

    InformationForReplacementHelper.GenerateInstallDate();
    
    ReplacementDetailsManager.InitReplacementDetails();

    ReplacementProductItemInfoManager.InitProductItemInfo();

    ReplacementproductInfoHelper.InitReplaceProductInfo();

    ReplacementSummaryManager.GenerateReplacementGrid();

    $("#txtCustomerCodeRe").change(function () {
        var customerCode = $("#txtCustomerCodeRe").val();
        InformationForReplacementHelper.FillReplacementAllInformation(customerCode);
    });
    
    
    $("#btnSearchReplacement").click(function () {
        var customerCode = $("#txtCustomerCodeRe").val();
        InformationForReplacementHelper.FillReplacementAllInformation(customerCode);
    });


 
    
});

