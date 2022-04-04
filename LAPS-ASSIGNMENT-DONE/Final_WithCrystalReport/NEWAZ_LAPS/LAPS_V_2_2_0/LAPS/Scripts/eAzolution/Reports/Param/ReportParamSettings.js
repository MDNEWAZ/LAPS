$(document).ready(function () {
    ReportParamDetailsManager.GetParamDataSource();
    ReportParamDetailsHelper.PopulateZoneCombo();
    ReportParamDetailsHelper.PopulateRegionCombo();
    ReportParamDetailsHelper.PopulateBranchCombo("cmbBranch");
    ReportParamDetailsHelper.PopulatePackageCombo("cmbPackage");

    ReportParamDetailsHelper.PopulateSaleRepType("cmbSalesRep");
    ReportParamDetailsHelper.ShowReport("btnShowReport");
    ReportParamDetailsHelper.CreteDateTime();
})

