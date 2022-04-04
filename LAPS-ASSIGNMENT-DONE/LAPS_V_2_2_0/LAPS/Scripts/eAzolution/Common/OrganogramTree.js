
var organogramArray = [];

var organogramTreeManager = {

    GetOrganogramInformation: function () {

        var obj = "";
        var jsonParam = "";
        var serviceUrl = "../Organogram/GetOrganogramTreeData/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return obj;

    }

};

var organogramTreeHelper = {

    populateOrganogramTree: function () {

        var data = organogramTreeHelper.createCustomDataSource();

        var inlineDefault = new kendo.data.HierarchicalDataSource({
            cache: false,
            async: false,
            data: data

        });

        $("#treeviewOrganogram").kendoTreeView({
            dataSource: inlineDefault,
            dataTextField: "text",
            value: "Id",
            select: organogramTreeHelper.OnselectNode
        });

        // organogramTreeHelper.expandAll();
        //organogramTreeHelper.collapsAll();
    },

    createCustomDataSource: function () {

        var objOrganogramInfo = organogramTreeManager.GetOrganogramInformation();
        organogramArray = [];
        var newArray = [];

        for (var i = 0; i < objOrganogramInfo.length; i++) {
            //if (objOrganogramInfo[i].MotherId == null || objOrganogramInfo[i].MotherId == 0) {
            if (i == 0) {
                newArray = [];
                var obj = new Object();
                obj.text = objOrganogramInfo[i].CompanyName;
                obj.Id = objOrganogramInfo[i].CompanyId;
                obj.TypeName = "Company";
                //organogramArray.push(obj);

                obj = organogramTreeHelper.addChiledCompany(objOrganogramInfo[i].CompanyId, objOrganogramInfo, obj, newArray);

                //obj = organogramTreeHelper.addChiledBranch(objOrganogramInfo[i].CompanyId, objOrganogramInfo, obj, newArray);

                organogramArray.push(obj);
            } //Close of if condition
        }

        return organogramArray;
    },

    addChiledCompany: function (companyId, objOrganogramInfo, objInformation, chiledArray) {

        for (var i = 0; i < objOrganogramInfo.length; i++) {
            if (objOrganogramInfo[i].MotherId == companyId && (objOrganogramInfo[i].MotherId != null || objOrganogramInfo[i].MotherId == 0)) {
                var obj = new Object();
                obj.text = objOrganogramInfo[i].CompanyName;
                obj.Id = objOrganogramInfo[i].CompanyId;
                obj.TypeName = "Company";
                chiledArray.push(obj);

                organogramTreeHelper.addChiledCompany(objOrganogramInfo[i].CompanyId, objOrganogramInfo, obj, []);

            }
        }

        objInformation.items = chiledArray;

        organogramTreeHelper.addChiledBranch(companyId, objOrganogramInfo, objInformation, chiledArray);
        return objInformation;


    },

    addChiledBranch: function (companyId, objOrganogramInfo, objInformation, chiledArray) {

        for (var i = 0; i < objOrganogramInfo.length; i++) {
            if (objOrganogramInfo[i].CompanyId == companyId) {
                if (objOrganogramInfo[i].Branches != null) {
                    for (var j = 0; j < objOrganogramInfo[i].Branches.length; j++) {
                        var obj = new Object();
                        obj.text = objOrganogramInfo[i].Branches[j].BranchName + " (Branch)";
                        obj.Id = objOrganogramInfo[i].Branches[j].BranchId;
                        obj.TypeName = "Branch";

                        obj.items = organogramTreeHelper.addChiledDepartment(objOrganogramInfo[i].Branches[j], []);
                        chiledArray.push(obj);
                    }
                }
            }
        }

        objInformation.items = chiledArray;


        return objInformation;


    },

    addChiledDepartment: function (objInformation, chiledArray) {


        if (objInformation.Departments != null) {
            for (var i = 0; i < objInformation.Departments.length; i++) {


                var obj = new Object();
                obj.text = objInformation.Departments[i].DepartmentName + " (Department)";
                obj.Id = objInformation.Departments[i].DepartmentId;
                obj.TypeName = "Department";

                obj.items = organogramTreeHelper.addChiledDesignation(objInformation.Departments[i], []);
                chiledArray.push(obj);
            }
        }
        return chiledArray;

    },

    addChiledDesignation: function (objInformation, chiledArray) {


        if (objInformation.Designations != null) {
            for (var i = 0; i < objInformation.Designations.length; i++) {


                var obj = new Object();
                obj.text = objInformation.Designations[i].DesignationName + " (Role)";
                obj.Id = objInformation.Designations[i].DesignationId;
                obj.TypeName = "Designation";
                chiledArray.push(obj);
            }
        }
        return chiledArray;

    },

    OnselectNode: function (e) {

        //alert(e.node.textContent);
        var pathName = window.location.pathname;
        var pageName = pathName.substring(pathName.lastIndexOf('/') + 1);
        var dataItem = this.dataItem(e.node);
        var companyId = 0;
        var branchId = 0;
        var companyName = "";
        var branchName = "";
        //var fDate = $("#cmbFromDate").data('kendoDatePicker').value();
        //var fromDate = kendo.toString(fDate, "d");
        //var tDate = $("#cmbToDate").data('kendoDatePicker').value();
        //var toDate = kendo.toString(tDate, "d");


        if (pageName == "Dashboard") {

            var fDate = $("#cmbFromDate").data('kendoDatePicker').value();
            var fromDate = kendo.toString(fDate, "d");
            var tDate = $("#cmbToDate").data('kendoDatePicker').value();
            var toDate = kendo.toString(tDate, "d");

            //  debugger;

            if (dataItem.TypeName == "Company") {
                companyId = dataItem.Id;
                DueCollectionHelper.DueCollectionGridDataSet(companyId, 0);
                DashboardSettingsHelper.CustomerDueGridDataSet("0", companyId, 0);
                //releaseLicenseHelper.releaseLicenseGridDataSet(companyId, 0);
                DashboardSettingsHelper.generateCharts(companyId, 0, fromDate, toDate);
                DashboardSettingsHelper.showMonthWiseCharts(companyId, 0, fromDate, toDate);
            }
            if (dataItem.TypeName == "Branch") {
                var parent = dataItem.parentNode();
                branchId = dataItem.Id;
                companyId = parent.Id;

                //check is branch is upgraded
                var branchInfo = empressCommonManager.GetBranchInfoByBranchId(branchId);
                if (branchInfo.IsUpgraded == 0 || branchInfo.BranchCode.length < 4) {
                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Upgrade Branch First!',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                $("#staticMenuForDataEntryOperator").hide();
                            }
                        }]);
                } else {

                    DashboardSettingsManager.changeSessionOnBranchChange(companyId, branchId);
                    DueCollectionHelper.DueCollectionGridDataSet(companyId, branchId);
                    DashboardSettingsHelper.CustomerDueGridDataSet("0", companyId, branchId);
                    //releaseLicenseHelper.releaseLicenseGridDataSet(companyId, branchId);
                    DashboardSettingsHelper.generateCharts(companyId, branchId, fromDate, toDate);
                    DashboardSettingsHelper.showMonthWiseCharts(companyId, branchId, fromDate, toDate);


                    //var userLevel = DashboardSettingsManager.userLevel();
                    if (currentUserlevel == 26) { //Data Entry Operator
                        $("#staticMenuForDataEntryOperator").show();
                    }

                    organogramTreeHelper.collapsAll();
                }
            }

        }
        if (pageName == "UserSettings") {
            if (dataItem.TypeName == "Company") {
                companyId = dataItem.Id;
                companyName = dataItem.text;
                userInfoHelper.fillCompanyAndBranch(companyId, companyName, 0, "");
                groupMembershipHelper.GetGroupByCompanyId(companyId);
            }
            if (dataItem.TypeName == "Branch") {
                branchId = dataItem.Id;
                branchName = dataItem.text;
                //check is branch is upgraded
                var brancInfo = empressCommonManager.GetBranchInfoByBranchId(branchId);
                if (brancInfo.IsUpgraded == 0 || brancInfo.BranchCode.length < 4) {
                    AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Upgrade Branch First!',
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                                userInfoHelper.ClearFields();
                            }
                        }]);
                } else {


                    var parent = dataItem.parentNode();

                    companyId = parent.Id;
                    companyName = parent.text;
                    userInfoHelper.fillCompanyAndBranch(companyId, companyName, branchId, branchName);
                    groupMembershipHelper.GetGroupByCompanyId(companyId);
                    groupPermisionArray = [];

                    organogramTreeHelper.collapsAll();
                }
            }

        }
        if (pageName == "SalesRepresentatorSettings") {
            if (dataItem.TypeName == "Company") {
                companyId = dataItem.Id;
                companyName = dataItem.text;
                SalesRepresentatorDetailsHelper.fillCompanyAndBranchForRep(companyId, companyName, 0, "");

            }
            if (dataItem.TypeName == "Branch") {
                var parent = dataItem.parentNode();
                branchId = dataItem.Id;
                branchName = dataItem.text;
                companyId = parent.Id;
                companyName = parent.text;
                SalesRepresentatorDetailsHelper.fillCompanyAndBranchForRep(companyId, companyName, branchId, branchName);

                organogramTreeHelper.collapsAll();

            }

        }
        //if (pageName == "OrganogramSettings") {
        //    employeeInformationHelper.populateEmployeeSummaryGrid(dataItem.Id, dataItem.TypeName);
        //}

        //if (pageName == "PublishSurveySettings") {
        //    employeeSolutionHelper.GenerateEmployeeSummmaryGrid(dataItem.Id, dataItem.TypeName);

        //}
        //if (pageName == "NoticeSettings") {
        //    employeeSolutionForNoticeHelper.GenerateEmployeeSummmaryForNoticeGrid(dataItem.Id, dataItem.TypeName);

        //}
        //if (pageName == "TrainingScheduleSettings") {
        //    nodeTypeId = dataItem.Id;
        //    nodeTypeName = dataItem.TypeName;
        //    TrainingPublisherManager.GenerateEmployeeSummmaryGrid(dataItem.Id, dataItem.TypeName);


        //}

    },
    initiatTreeSerch: function () {
    
        var tv = $('#treeviewOrganogram').data('kendoTreeView');

        $('#search-term').on('keyup', function () {
           
            organogramTreeHelper.expandAll();
     
            $('span.k-in > span.highlight').each(function () {
                $(this).parent().text($(this).parent().text());
            });

            // ignore if no search term
            if ($.trim($(this).val()) == '') { return; }

            var term = this.value.toUpperCase();
            var tlen = term.length;

            $('#treeviewOrganogram span.k-in').each(function (index) {
                var text = $(this).text();
                var html = '';
                var q = 0;
                while ((p = text.toUpperCase().indexOf(term, q)) >= 0) {
                    html += text.substring(q, p) + '<span class="highlight">' + text.substr(p, tlen) + '</span>';
                    q = p + tlen;
                }

                if (q > 0) {
                    html += text.substring(q);
                    $(this).html(html);

                    $(this).parentsUntil('.k-treeview').filter('.k-item').each(
                        function (index, element) {
                            tv.expand($(this));
                            $(this).data('search-term', term);
                        }
                    );
                }
            });

            $('#treeviewOrganogram .k-item').each(function () {
                if ($(this).data('search-term') != term) {
                    $('#treeviewOrganogram').data('kendoTreeView').collapse($(this));
                }
            });

        });

    },

    collapsAll: function () {
        $('#treeviewOrganogram').data('kendoTreeView').collapse(".k-in");
    },

    expandAll: function () {
       
        var treeView = $('#treeviewOrganogram').data('kendoTreeView');
        treeView.expand(".k-in");
        treeView.expand(".k-in");
        treeView.expand(".k-in");
    },

  
};