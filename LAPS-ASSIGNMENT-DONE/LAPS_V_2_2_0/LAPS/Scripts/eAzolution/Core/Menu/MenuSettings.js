$(document).ready(function () {
    
    MenuInformationHelper.createTab();
    menuDetailsManager.GetAllModule();
    menuSummaryManager.GenerateMenuGrid();
    menuSummaryHelper.GeRowDataOfMenuGrid();
    menuDetailsManager.GetAllMenuByModuleId(0);
    MenuSortingHelper.GenerateMenuGridForSorting();
 
    //$(".k-state-active").addClass('k-state-active');
    $("#btnSaveSortOrder").click(function () { MenuSortingHelper.UpdateMenuSorting(); });

 
 
});

var menuSettingsManager = {
    
};

var menuSettingsHelper = { };





