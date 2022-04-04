var gbIsViewer;
$(document).ready(function () {
    if (CurrentUser != null) {
        var isViewer = CurrentUser.IsViewer;
        gbIsViewer = isViewer;
    }
    if (isViewer == 1) {
        CollectionHelper.HideAllOperationalButton();
    }

    CollectionHelper.HideButton();
    productInfoHelper.GenerateModelCombo(); // Make Model Combo
  
    
    CollectionSummaryManager.GenerateCollectionGrid();
    CollectionDetailsManager.InitPaymentCollection();// All needed things for collection 

   
    InstallmentDetailsManager.InitiateInstallmentDetails();
    
    CollectionHistoryHelper.InitCollectionHistory();//Collection History
    
    productInfoManager.InitProductInfo();
   
    $("#btnCloseCollection").click(function () {
        $("#collectionMainDetailsDiv").hide();
        $("#divInstallmentInfoGrid").hide();
        $("#divCollectionSummaryGrid").show();
        CollectionDetailsHelper.ClearAllFields();
    });

    $("#btnReceive").show();
    $("#btnReceive").click(function () {
        CollectionDetailsHelper.clickEventForReceiveButton();
      
    });
    
    $("#btnReceivePayment").click(function() {
        CollectionDetailsHelper.clickEventForSave();
    });
    
});

var CollectionHelper = {
    HideButton: function () {
        $("#btnAddProInfo").hide();
        $("#liSaleInvoice").show();

    },
    HideAllOperationalButton: function () {
        $("#btnReceivePayment").hide();
        $("#btnReceive").hide();
       
    }
};