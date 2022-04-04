$(document).ready(function () {

    CompanyHeirarchyPathManager.GetCompanyHeirarchyPathData(0, 0);

    $("#txtCodeCustInfo").change(function () {
       
        SimplePaymentCollectionHelper.changeEventOfCustomerCode();
    });
    
    $("#btnSearch").click(function () {
       
        SimplePaymentCollectionHelper.changeEventOfCustomerCode();
    });
    
    //$("#txtCodeCustInfo").keypress(function (event) {
    //    if (event.keyCode == 13) {
    //        SimplePaymentCollectionHelper.changeEventOfCustomerCode();
    //    }
    //});
    
    SimplePaymentCollectionHelper.GenerateCashMemoDatePicker();
    SimplePaymentCollectionHelper.iniPopupWindow();
    SimplePaymentCollectionHelper.GenerateDraftedPaymentGrid();
    SimplePaymentCollectionHelper.GenerateCollectionDatePicker();
    $("#txtAmount").kendoNumericTextBox({ min: 0});
    $("#btnSaveAsDraft").click(function() {
        SimplePaymentCollectionManager.SaveAsDraftPaymentCollection();
    });
    
    $("#btnPreview").click(function () {
        SimplePaymentCollectionHelper.clickEventForPreviewButton();
    }); 
    $("#btnClearAll").click(function () {
        SimplePaymentCollectionHelper.clearAll();
    });
    $("#btnSubmit").click(function () {
        SimplePaymentCollectionHelper.FinalSubmitPaymentCollection();
    });
    $("#btnClose").click(function () {
        $("#divPopupPreview").data("kendoWindow").close();
    });
    $("#txtCollectionDate").change(function () {
        SimplePaymentCollectionHelper.changeeventForCollectionDate();
    });

});

var SimplePaymentHelper= {
    LoadCompanyHeirarchyPath: function (obj) {

        var rootCompany = obj.RootCompanyName == null ? "" : obj.RootCompanyName + " >>";
        var motherCompany = obj.MotherCompanyName == null ? "" : obj.MotherCompanyName + " >>";
        var company = obj.CompanyName == null ? "" : obj.CompanyName + " >>";
        var branchName = obj.BranchName == null ? "" : obj.BranchName;

        // var sitepath = "Company: " + rootCompany + " >> " + motherCompany + " >> " + company + " >> " + "   Branch:" + branchName;
        var sitepath = " " + rootCompany + "" + motherCompany + " " + company + "" + branchName + " (Branch)";
        $("#lblSiteMapPathCollection").html(sitepath);
    }
}