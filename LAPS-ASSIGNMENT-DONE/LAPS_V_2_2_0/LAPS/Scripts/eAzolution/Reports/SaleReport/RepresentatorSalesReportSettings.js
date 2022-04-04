$(document).ready(function () {
    RepresentatorSaleReportDetailsHelper.PopulateBranchCombo("cmbBranch");
    RepresentatorSaleReportDetailsHelper.PopulateRepresentatorCombo("cmbSalesRep");
    RepresentatorSaleReportDetailsHelper.CreteDateTime();
    RepresentatorSaleReportDetailsHelper.ShowReport("btnShowReport");
})

