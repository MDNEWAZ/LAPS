$(document).ready(function () {
    ReportParamDetailsManager.GetParamDataSource();
    ReportParamDetailsHelper.PopulateZoneCombo();
    ReportParamDetailsHelper.PopulateRegionCombo();
    ReportParamDetailsHelper.PopulateBranchCombo("cmbBranch");
    ReportParamDetailsHelper.PopulateStockType("cmbStockType");
    StockDetailsHelper.ShowStockReport("btnStockShowReport");
})

