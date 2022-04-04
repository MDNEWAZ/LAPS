/// <reference path="../../Sales/Dashborad/PendingCollection.js" />
/// <reference path="../../Sales/Dashborad/TopTenRedCustomer.js" />
/// <reference path="../../Sales/Collection/CollectionCustometRating.js" />
/// <reference path="DashboardSettings.js" />
/// <reference path="DueCollection.js" />
/// <reference path="../../Sales/Collection/CollectionDetails.js" />
/// <reference path="ReleaseLicenseGenerate.js" />
/// <reference path="../../Common/EmpressCommon.js" />
/// <reference path="../../Sales/Customer/CustomerDetails.js" />
/// <reference path="CodeSearch.js" />
/// <reference path="../../Common/OrganogramTree.js" />
/// <reference path="Notification.js" />


$(document).ready(function () {

    //$("#divReleaseLicenseGenerate").hide();
    empressCommonHelper.initePanelBer("ulIdentityPanelOrganogram");
    organogramTreeHelper.populateOrganogramTree();
    organogramTreeHelper.initiatTreeSerch();
    DashboardSettingsHelper.checkUserLevel();//Check for root user to permit some options or not
    //dashboardManager.GetAccessPermisionForCurrentUser();
    //dashboardHelper.showHideRequestChart();
    $("#cmbFromDate").kendoDatePicker({ format: "MM/dd/yyyy" });
    $("#cmbToDate").kendoDatePicker({ format: "MM/dd/yyyy" });

    
    

    var invoice = "0";
    //pendingRedCustomerManager.GenerateRedCustomer();
    //pendingRedCustomerManager.CustomerDueGridDataSet(invoice);
    DashboardSettingsHelper.GenerateRedCustomer();
    DashboardSettingsHelper.CustomerDueGridDataSet(invoice, 0, 0);
    pendingCollecttonHellper.pendingCollectionChart();
    DashboardSettingsHelper.generateCharts(0, 0, null, null);

    DueCollectionHelper.GenerateDueCollectionGrid();
    DueCollectionHelper.DueCollectionGridDataSet(0, 0);//To set data on Due collection grid




    //releaseLicenseHelper.ReleaseLicensePopUpWindow();
    //$("#btnGenerateReleaseLicense").click(function() {
    //    releaseLicenseManager.GenerateReleaseLisence();
    //});


    //empressCommonHelper.GenerareHierarchyCompanyCombo("cmbCompanyName");
    //empressCommonHelper.GenerateBranchCombo(0, "cmbBranchName");
    //if (CurrentUser != null) {

    //    $("#cmbCompanyName").data("kendoComboBox").value(CurrentUser.CompanyId);
    //    empressCommonHelper.GenerateBranchCombo(CurrentUser.CompanyId, "cmbBranchName");
    //}


    //$("#cmbCompanyName").change(function () {

    //    dashboardHelper.CompanyChange();
    //});
    //$("#cmbBranchName").change(function () {

    //    dashboardHelper.branchChange();
    //});

    OfficeTimeWithClockManager.startServerClock();
    //toDoHelper.GenerateToDoGrid();
    //toDoHelper.GeRowDataOfToDoMenuGrid();

    //$('input:radio[name=varificationtype]').click(function () {
    //    $("#divReleaseButton").show();
    //});
    //CodeSearchHelper.initCodeSearch();
    //$("#btnCodeSearch").click(function () {
    //    $("input[type='text']").val("");
    //    $("#CodeSearchPopUp").data('kendoWindow').open().center();
    //    CodeSearchHelper.GenerateCustomerWithCodeGrid();
    //});
    //pendingCollectionManager.GeneratePendingTable();

    

    $("#cmbToDate").change(function () {
        dashboardHelper.changeEventForToDate();
    });
    //dashboardHelper.initiatTreeSerch();

    notificationHelper.notificationInit();
    
    var treeview = $("#treeviewOrganogram").data("kendoTreeView");
    treeview.expand(".k-item");
});

var accessArray = [];

var dashboardManager = {
    GetAccessPermisionForCurrentUser: function () {
        var objStatus = "";
        var jsonParam = "";
        var serviceUrl = "../Group/GetAccessPermisionForCurrentUser/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            dashboardHelper.PopulateAccessArray(jsonData);
        }
        function onFailed(error) {

            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText, [{ addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) { $noty.close(); } }]);
        }
    },

};

var dashboardHelper = {
    //initiatTreeSerch: function () {
    //    var tv = $('#treeviewOrganogram').data('kendoTreeView');
    //    $('#search-term').on('keyup', function () {
            
    //        $('span.k-in > span.highlight').each(function () {
    //            $(this).parent().text($(this).parent().text());
    //        });

    //        // ignore if no search term
    //        if ($.trim($(this).val()) == '') { return; }

    //        var term = this.value.toUpperCase();
    //        var tlen = term.length;

    //        $('#treeviewOrganogram span.k-in').each(function (index) {
    //            var text = $(this).text();
    //            var html = '';
    //            var q = 0;
    //            while ((p = text.toUpperCase().indexOf(term, q)) >= 0) {
    //                html += text.substring(q, p) + '<span class="highlight">' + text.substr(p, tlen) + '</span>';
    //                q = p + tlen;
    //            }

    //            if (q > 0) {
    //                html += text.substring(q);
    //                $(this).html(html);

    //                $(this).parentsUntil('.k-treeview').filter('.k-item').each(
    //                    function (index, element) {
    //                        tv.expand($(this));
    //                        $(this).data('search-term', term);
    //                    }
    //                );
    //            }
    //        });

    //        $('#treeviewOrganogram .k-item').each(function () {
    //            if ($(this).data('search-term') != term) {
    //                tv.collapse($(this));
    //            }
    //        });
    //    });


    //},
    changeEventForToDate: function () {

        var fDate = $("#cmbFromDate").data('kendoDatePicker').value();
        var fromDate = kendo.toString(fDate, "d");
        var tDate = $("#cmbToDate").data('kendoDatePicker').value();
        var toDate = kendo.toString(tDate, "d");
        if (fromDate != null && toDate != null) {
            dashboardHelper.changeDashboardByDateRange(fromDate, toDate);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please select from date & to date both',
                 [{
                     addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                         $noty.close();
                         $("#cmbFromDate").data('kendoDatePicker').value("");
                         $("#cmbToDate").data('kendoDatePicker').value("");
                     }
                 }]);

        }
    },
    changeDashboardByDateRange: function (fromDate, toDate) {

        DashboardSettingsHelper.showMonthWiseCharts(0, 0, fromDate, toDate);
        DashboardSettingsHelper.generateCharts(0, 0, fromDate, toDate);
    },
    startAutoRefresh: function (autoRefreshTime) {

        var refreshInerval = autoRefreshTime * 1000;
        timerID = self.setInterval(function () {
            todaysAttendanceHelper.GenerateTodaysAttendanceChart();
            //MenuManager.getCurrentUser(false);
            //MyEofficeManager.checkOfficeTime();
            //MyEofficeManager.checkShortLeaveAndHalfDayLeaveDurarion();
        },
    refreshInerval);
    },

    PopulateAccessArray: function (jsonData) {
        accessArray = [];
        for (var i = 0; i < jsonData.length; i++) {
            accessArray.push(jsonData[i]);
        }
    },

    checkApproverUser: function () {
        var approver = false;
        for (var i = 0; i < accessArray.length; i++) {
            if (accessArray[i].ReferenceID == 4) {
                approver = true;
                break;
            }
        }
        return approver;
    },

    checkRecomanderUser: function () {
        var recomander = false;

        for (var i = 0; i < accessArray.length ; i++) {
            if (accessArray[i].ReferenceID == 3) {
                recomander = true;
                break;
            }
        }
        return recomander;
    },

    showHideRequestChart: function () {
        var isrecomander = dashboardHelper.checkRecomanderUser();

        var isapprover = dashboardHelper.checkApproverUser();

        if (isrecomander == false && isapprover == false) {
            $("#divRecomandationRequest").hide();
            $("#divForGraphForApprovalRequest").hide();
        }
        else if (isrecomander == true && isapprover == false) {
            $("#divRecomandationRequest").show();
            //recomandationRequestHelper.GenerateRecomandationRequestChart();
            $("#divForGraphForApprovalRequest").hide();
        }
        else if (isrecomander == false && isapprover == true) {
            $("#divRecomandationRequest").hide();
            $("#divForGraphForApprovalRequest").show();
            //ApprovalRequestHelper.GenerateApprovalRequestChart();
        }
        else if (isapprover == true) {
            $("#divRecomandationRequest").show();
            $("#divForGraphForApprovalRequest").show();

            //recomandationRequestHelper.GenerateRecomandationRequestChart();
            //ApprovalRequestHelper.GenerateApprovalRequestChart();
        }
    },
    //CompanyChange: function () {

    //    //var companyNames = $("#cmbCompanyName");
    //    //companyNames.change(function () {
    //        var comboboxbranch = $("#cmbBranchName").data("kendoComboBox");
    //        var companyData = $("#cmbCompanyName").data("kendoComboBox");
    //        var companyId = companyData.value();
    //        var companyName = companyData.text();

    //        if (companyId == companyName) {
    //            companyData.value('');
    //            comboboxbranch.value('');
    //            comboboxbranch.destroy();
    //            empressCommonHelper.GenerateBranchCombo(0, "cmbBranchName");
    //            return false;
    //        }
    //        if (comboboxbranch != undefined) {
    //            comboboxbranch.value('');
    //            comboboxbranch.destroy();
    //        }

    //        empressCommonHelper.GenerateBranchCombo(companyId, "cmbBranchName");
    //        //customerDetailsHelper.changeEventForBranch();
    //        DueCollectionHelper.DueCollectionGridDataSet(companyId, 0);
    //        DashboardSettingsHelper.CustomerDueGridDataSet("0", companyId, 0);
    //        releaseLicenseHelper.releaseLicenseGridDataSet(companyId, 0);
    //        DashboardSettingsHelper.generateCharts(companyId, 0);
    //        DashboardSettingsHelper.showMonthWiseCharts(companyId, 0);
    //        return false;
    //    //});
    //},
    //branchChange:function () {
    //    var companyData = $("#cmbCompanyName").data("kendoComboBox");
    //    var companyId = companyData.value();
    //    var comboboxbranch = $("#cmbBranchName").data("kendoComboBox");
    //    var branchId = comboboxbranch.value();
    //    DueCollectionHelper.DueCollectionGridDataSet(companyId, branchId);
    //    DashboardSettingsHelper.CustomerDueGridDataSet("0", companyId, branchId);
    //    releaseLicenseHelper.releaseLicenseGridDataSet(companyId, branchId);
    //    DashboardSettingsHelper.generateCharts(companyId, branchId);
    //    DashboardSettingsHelper.showMonthWiseCharts(companyId, branchId);
    //},
};