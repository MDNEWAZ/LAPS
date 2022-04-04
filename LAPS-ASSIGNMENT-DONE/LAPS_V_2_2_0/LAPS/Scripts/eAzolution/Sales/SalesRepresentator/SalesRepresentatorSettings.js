
$(document).ready(function () {
  empressCommonHelper.initePanelBer("ulIdentityPanelOrganogram");
  organogramTreeHelper.populateOrganogramTree();
  organogramTreeHelper.initiatTreeSerch();
    
  SalesRepresentatorDetailsHelper.InitSalesRepDetails();

    SalesRepresentatorSummaryManager.GenerateSalesRepresentatorGrid();
});