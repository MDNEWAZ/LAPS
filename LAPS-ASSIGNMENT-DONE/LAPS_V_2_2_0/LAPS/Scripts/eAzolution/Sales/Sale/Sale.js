var gbIsViewer;
 
$(document).ready(function () {

    CompanyHeirarchyPathManager.GetCompanyHeirarchyPathData(0, 0);

    if (CurrentUser != null) {
        var isViewer = CurrentUser.IsViewer;
        gbIsViewer = isViewer;
    }
    if (isViewer == 1) {
        SaleHelper.HideAllOperationalButton();
    }
   
   
    $("#txtCollectionDate").kendoDatePicker();
    customerInfoManager.InitCustomerInfoDetails();
    saleDetailsManager.InitislSaleDetails();
    saleDetailsHelper.GenerateSaleDatePicker();
    InstallmentDetailsManager.GenerateInstalmentGrid();
    InstallmentDetailsHelper.GenerateStatusCombo();
    


    productInfoHelper.InitProductInfoHelper();
    ProductIntemInfoManager.InitProductItemInfo();

    DiscountInfoHelper.InitDiscountInfo();
    RepresentatorInfoHelper.populateSalesRepresentatorCombo();


    $("#btnSearchInvoiceSale").click(function () {
        saleDetailsManager.ProductCustomerSaleInfoFill();
    });

    SaleManager.SaleDetailsEvent();

    $("#txtDownpayment").hide();
    $("#lblDownpayment").hide();
    

    BookedSaleSummaryManager.InitBookedSaleSummary(); 

    $("#btnPreviewSales").click(function () {
        $("#bookedSaleSummaryDiv").show();
        $("#saleDetailsMainDiv").hide();
        // saleDetailsHelper.ClearAllSaleDetailsForm();
        BookedSaleSummaryHelper.ReloadEntryDate();
        BookedSaleSummaryManager.GenerateBookedSaleGrid();
    });
    $("#btnCloseSummary").click(function () {
        
        $("#bookedSaleSummaryDiv").hide();
        $("#saleDetailsMainDiv").show();
        saleDetailsHelper.ClearAllSaleDetailsForm();

    });

    saleSummaryManager.InitSalesSummary();
    $("#btnPreviewSaveAsDraft").click(function () {
        $("#saveAsDraftedSaleSummaryDiv").show();
        $("#saleDetailsMainDiv").hide();
      //  saleDetailsHelper.ClearAllSaleDetailsForm();
        saleSummaryManager.GenerateSaleGrid();
    });

    $("#btnCloseDraftedSummary").click(function () {
        $("#saveAsDraftedSaleSummaryDiv").hide();
        $("#saleDetailsMainDiv").show();
        SaleSummaryHelper.ReloadEntryDate();
        saleDetailsHelper.ClearAllSaleDetailsForm();

    });
  
    $("#txtInvoiceSale").change(function () {

        var saleObj = saleDetailsHelper.GetSalesObj();
        if (saleObj.customerCode == "") {
            AjaxManager.MsgBox('warning', 'center', 'Notify', 'Please Select Customer',
                [{
                    addClass: 'btn btn-primary',
                    text: 'Ok',
                    onClick: function ($noty) {
                        $noty.close();
                        return;
                    }
                }]);
        }
       
        ProductItemInfoHelper.CalculateNetTotalPrice(saleObj);
        saleDetailsHelper.InstallmentData(saleObj);

    });


    $("#txtInvoiceSale").keypress(function (event) {
        if (event.keyCode == 13) {
            saleDetailsManager.ProductCustomerSaleInfoFill();
        }
    });


    $("#cmbStatus").parent().css('width', "10em");
    
    $("#btnSearchSaleInfo").click(function () {
        saleSummaryManager.SearchSalesSummaryByParam();
    });

    $("#cmbSaleDate").change(function () {
        saleDetailsHelper.ChangeEventForSaleDate();
    });
    
    $("#btnSave").click(function () {
        saleDetailsManager.SaveAsBooked(1);//1 = draft
    });

    $("#btnSaveAsBook").click(function () {
        saleDetailsManager.SaveAsBooked(2);//2 = Booked
    });

    $("#btnFinalSubmitPrepaid").click(function () {

        AjaxManager.MsgBox('information', 'center', 'Confirm Update:', 'Are You Confirm to Sumbit Sales Finally? ',
     [{
         addClass: 'btn btn-primary',
         text: 'Yes',
         onClick: function ($noty) {
             $noty.close();
             saleDetailsManager.SaveSaleForSpecialPackage();// Special Package like D-Type & R-Type
         }
     },
     {
         addClass: 'btn',
         text: 'No',
         onClick: function ($noty) {
             $noty.close();

         }
     }]);
      
    });

   
});


var SaleManager = {

    SaleDetailsEvent: function () {
        //$("#cmbFromDate").change(function () {
        //    saleSummaryManager.SearchSalesSummaryByParam();
        //});
        //$("#txtInvoice").change(function () {
        //    saleSummaryManager.SearchSalesSummaryByParam();
        //});


    }

};

var SaleHelper = {    
    HideAllOperationalButton:function() {
        $("#btnSaveAsBook").hide();
        $("#btnSaveAsDraft").hide();
        $("#btnFinalSubmitPrepaid").hide();
       
    },


    LoadCompanyHeirarchyPath: function (obj) {

        var rootCompany = obj.RootCompanyName == null ? "" : obj.RootCompanyName +">>";
        var motherCompany = obj.MotherCompanyName == null ? "" : obj.MotherCompanyName + ">>";
        var company = obj.CompanyName == null ? "" : obj.CompanyName + ">>";
        var branchName = obj.BranchName == null ? "" : obj.BranchName;
     
       // var sitepath = "Company: " + rootCompany + " >> " + motherCompany + " >> " + company + " >> " + "   Branch:" + branchName;
        var sitepath = " " + rootCompany + "" + motherCompany + " " + company + "" +  branchName + " (Branch)";
        $("#lblSiteMapPath").html(sitepath);
    }

};
