var gbIsViewer;
$(document).ready(function () {
    if (CurrentUser != null) { gbIsViewer = CurrentUser.IsViewer; }
   
    WaitingForDiscountSummaryHelper.GenerateWaitingForDiscountGrid();
    WaitingForDiscountSummaryHelper.GeRowDataForDiscountGrid();
    WaitingForDiscountSummaryHelper.ClickEventForDpAbblicableRadio();
    //WaitingForDiscountDetailsHelper.InitTreeView();
    WaitingForDiscountSummaryHelper.InitWaitingForDiscount();
    WaitingForDiscountDetailsHelper.InitWaitingForSpecialDiscount();
});

