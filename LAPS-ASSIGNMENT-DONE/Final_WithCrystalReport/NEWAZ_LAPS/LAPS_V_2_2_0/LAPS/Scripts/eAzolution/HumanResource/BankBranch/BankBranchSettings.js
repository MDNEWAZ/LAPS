$(document).ready(function () {
    $("#tabBankBranchSettings").kendoTabStrip({
        animation: {
            close: {
                effects: "fadeOut"
            },
            open: {
                effects: "fadeIn"
            }
        }
        //animation: false
    });
    $("#popupWindowBankDetails").kendoWindow({

        title: "Bank Settings",
        resizeable: false,
        width: "40%",
        actions: ["Pin", "Refresh", "Maximize", "Close"],
        modal: true,
        visible: false,
    });

    $("#btnAddNewBank").click(function () {
        $("#popupWindowBankDetails").data("kendoWindow").open().center();
    });

    BankDetailsHelper.bankDetailsInit();
    BankBranchDetailsHelper.bankBranchDetailsInit();
    BankBranchSummaryHelper.GenerateBankBranchGrid();
    
    //company bank settigns onload
    CompanyBankSettingsHelper.GenerateCompanyCombo();
    CompanyBankSettingsHelper.GenerateCompanyBankSetupGrid();
    $("#cmbCompanyName").change(function () {
        CompanyBankSettingsHelper.GenerateCompanyBankSetupGrid();
    });

    $("#btnSaveCompanyBankBranch").click(function () {
        CompanyBankSettingsManager.SaveCompanyBankBranch();
    });

});