
var empressCommonManager = {
    
    GetHierarchyCompany: function () {
        var objCompany = "";
        var jsonParam = "";
        var serviceUrl = "../Company/GetMotherCompany/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompany = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCompany;
    },
    
    GenerateBranchCombo: function (companyId) {
        var objBranch = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../../Branch/GetBranchByCompanyIdForCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objBranch = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objBranch;
    },
    GenerateAllBranchCombo: function (rootCompanyId) {
        var objBranch = "";
        var jsonParam = "rootCompanyId=" + rootCompanyId;
        var serviceUrl = "../../Branch/GetAllBranchByCompanyIdForCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objBranch = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objBranch;
    },
    
    GetDepartmentByCompanyId: function (companyId) {
        var objDepartment = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../../Department/GetDepartmentByCompanyId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objDepartment = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objDepartment;
    },

    GetEmployeeByCompanyIdAndBranchIdAndDepartmentId: function (companyId, branchId, departmentId) {
        var objEmployee = "";
        var jsonParam = "companyId=" + companyId + "&branchId=" + branchId + "&departmentId=" + departmentId;
        var serviceUrl = "../../Employee/GetEmployeeByCompanyIdAndBranchIdAndDepartmentId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objEmployee = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployee;
    },

    GetEmployeeByDepartmentId: function (departmentId) {
        var objEmployee = "";
        var jsonParam = "departmentId=" + departmentId;
        var serviceUrl = "../../Employee/GenerateEmployeeByDepartmentId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objEmployee = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployee;
    },
    
    GetEmployeeType: function () {
        var objEmployeeType = "";
        var jsonParam = "";

        var serviceUrl = "../../Employee/GetEmployeeTypeForCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objEmployeeType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployeeType;
    },
    
    GetSalaryStatus: function () {
        var objEmployeeType = "";
        var jsonParam = "";

        var serviceUrl = "../../Status/GetSalaryStatus/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objEmployeeType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployeeType;
    },
    
    GetPayrollStatus: function () {
        var objEmployeeType = "";
        var jsonParam = "";

        var serviceUrl = "../../Status/GetPayrollStatus/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objEmployeeType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployeeType;
    },
    
    GetBonusStatus: function () {
        var objEmployeeType = "";
        var jsonParam = "";

        var serviceUrl = "../../Status/GetBonusStatus/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objEmployeeType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployeeType;
    },
    
    GetProjectStatus: function () {
    
        var objEmployeeType = "";
        var jsonParam = "";

        var serviceUrl = "../Status/GetProjectStatus/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objEmployeeType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployeeType;
    },
    
    GetCoffStatus: function () {
        var objStatus = "";
        var jsonParam = "";
        var serviceUrl = "../Status/GetCoffStatus/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objStatus = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objStatus;
    },
    
    GetLeaveForwadingStatus: function () {
        var objStatus = "";
        var jsonParam = "";
        var serviceUrl = "../Status/GetLeaveForwadingStatus/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objStatus = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objStatus;
    },
    
    GetAccessPermissionForCurrentUserForHrAccountsModule: function () {
        var objEmployeeType = "";
        var jsonParam = "";

        var serviceUrl = "../../Status/GetAccessPermissionForCurrentUserForHrAccountsModule/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objEmployeeType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objEmployeeType;
    },
    
    GetActionButtonByState: function (stateId) {
        var objAction = "";
        var jsonParam = "stateId=" + stateId;
        var serviceUrl = "../Status/GetActionByStateIdAndUserId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objAction = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objAction;
    },

    
    GetClient: function (companyId) {
        var objClient = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../Client/GetClient/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objClient = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objClient;
    },
    
    GenerateDesignationCombo: function (companyId) {
        var objDesignation = "";
        var jsonParam = "companyId=" + companyId + "&status=" + 1;
        var serviceUrl = "../Designation/GetAllDesignationByCompanyIdAndStatus/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objDesignation = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objDesignation;
    },
    
    GenerateDesignationByDepartmentIdCombo: function (departmentId) {
        var objDesignation = "";
        var jsonParam = "departmentId=" + departmentId + "&status=" + 1;
        var serviceUrl = "../Designation/GenerateDesignationByDepartmentIdCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objDesignation = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objDesignation;
    },
   
    //Deleted
    GetGradeByCompanyAndPayroll: function (companyId, payrollTypeId) {
        var objGrade = "";
        var jsonParam = "companyId=" + companyId + "&payrollTypeId=" + payrollTypeId;
        var serviceUrl = "../GradeSettings/GetGradeByCompanyAndPayroll/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objGrade = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objGrade;
    },
    
    GenerateGradeComboByCompanyId: function (companyId) {
        var objGrade = "";
        var jsonParam = "companyId=" + companyId;
        var serviceUrl = "../Grade/GenerateGradeComboByCompanyId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objGrade = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objGrade;
    },
    
    GetPayrollSettingsByGradeId: function (gradeId) {
        var objGrade = "";
        var jsonParam = "gradeId=" + gradeId;
        var serviceUrl = "../GradeSettings/GetPayrollSettingsByGradeId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objGrade = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objGrade;
    },
    
    GetCertificateType: function () {
        var objCertificateType = "";
        var jsonParam = "";
        var serviceUrl = "../CertificateType/LoadActiveCertificateType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCertificateType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCertificateType;
    },
    
    GetIncidentType:function () {
        var objIncidentType = "";
        var jsonParam = "";
        var serviceUrl = "../Incident/GetIncidentType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objIncidentType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objIncidentType;
    },
    
    GetAgencyByAgencyType: function (agencyType) {
        var objAgency = "";
        var jsonParam = "agencyType=" + agencyType;
        var serviceUrl = "../CNF/GetAgencyByAgencyType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objAgency = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objAgency;
    },
    
    GetAgentInformationByAgencyId: function (agencyId) {
        var objAgent = "";
        var jsonParam = "agencyId=" + agencyId;
        var serviceUrl = "../CNF/GetAgentInformationByAgencyId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objAgent = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objAgent;
    },
    
    GetTaskTypeInformation: function () {
        var objTaskType = "";
        var jsonParam = "";
        var serviceUrl = "../ProjectManagement/GetTaskTypeInformation/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objTaskType = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return objTaskType;
    },
    
    GetLeaveAccumulationType: function () {
        var obj = "";
        var jsonParam = "";
        var serviceUrl = "../LeaveEncashmentForwarding/GetLeaveAccumulationType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    GetLeaveTypeByAccumulationType: function (accumulationType) {
        var obj = "";
        var jsonParam = "accumulationType=" + accumulationType;
        var serviceUrl = "../LeaveEncashmentForwarding/GetLeaveTypeByAccumulationType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    GetActiveLeaveType: function () {
        var obj = "";
        var jsonParam = "";
        var serviceUrl = "../LeavePolicy/GetActiveLeaveType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    GetPlanedLeaveType: function (hrRecordId) {
        var obj = "";
        var jsonParam = "";
        var serviceUrl = "../LeavePlan/GetPlanedLeaveType/?hrRecordId="+hrRecordId;
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    GenerateBonusType: function () {
        var obj = "";
        var jsonParam = "";
        var serviceUrl = "../Bonus/GenerateBonusType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    GetCtcInformation: function () {
        var obj = "";
        var jsonParam = "";
        var serviceUrl = "../Payroll/GetCtcInformation/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    GetCtcTypes: function () {
        var obj = "";
        var jsonParam = "";
        var serviceUrl = "../Payroll/GetCtcTypes/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    GetLoanTypes: function () {
        var obj = "";
        var jsonParam = "";
        var serviceUrl = "../LoanAdvancedisburseSchedule/GetLoanTypes/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    populateRepostingType: function () {
        var obj = "";
        var jsonParam = "";
        var serviceUrl = "../TransferPromotion/GetRepostingType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            obj = jsonData;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    GenerateFunctionCombo: function () {
        var objFunction = "";
        var JsonParam = "";
        var serviceUrl = "../Function/GetFunctionDataForCombo/";
        AjaxManager.GetJsonResult(serviceUrl, JsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {

            objFunction = jsonData;
        }
        function onFailed() {
            window.alert(error.statusText);
        }

        return objFunction;
    },
    
    GenerateShiftComboByCompanyAndBranch: function (companyId, branchId) {
        var objShift = "";
        var jsonParam = "companyId=" + companyId + "&branchId=" + branchId;
        var serviceUrl = "../Calender/GenerateShiftComboByCompanyAndBranch/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objShift = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objShift;
    },
    
    GetCtcCategoryInformation: function () {
        var obj = "";
        var JsonParam = "";
        var serviceUrl = "../PayrollAdjustment/GetCtcCategory";
        AjaxManager.GetJsonResult(serviceUrl, JsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {

            obj = jsonData;
        }
        function onFailed() {
            window.alert(error.statusText);
        }

        return obj;
    },
    GetGradeType: function () {
        var obj = "";
        var JsonParam = "";
        var serviceUrl = "../GradeType/GetGradeTypeForCombo";
        AjaxManager.GetJsonResult(serviceUrl, JsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {

            obj = jsonData;
        }
        function onFailed() {
            window.alert(error.statusText);
        }

        return obj;
    },
    GetTrainingType: function () {
        var obj = "";
        var JsonParam = "";
        var serviceUrl = "../TrainingInfo/GetTrainingTypeForCombo";
        AjaxManager.GetJsonResult(serviceUrl, JsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {

            obj = jsonData;
        }
        function onFailed() {
            window.alert(error.statusText);
        }

        return obj;
    },

    GetDataForAnyCombo: function (serviceUrl) {
        var obj = "";
        var JsonParam = "";
        //var serviceUrl = "../TrainingInfo/GetTrainingTypeForCombo";
        AjaxManager.GetJsonResult(serviceUrl, JsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {

            obj = jsonData;
        }
        function onFailed() {
            window.alert(error.statusText);
        }

        return obj;
    },
    
    GenerateCommonGrid: function (ctlId,url,columns) {
        $("#" + ctlId).kendoGrid({
            dataSource: empressCommonManager.gridDataSource(url),
            pageable: {
                refresh: true,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
            },
            filterable: true,
            sortable: true,
            columns: columns,
            editable: false,
            navigatable: true,
            selectable: "row",
        });
    },

    gridDataSource: function (url) {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,

            serverSorting: true,

            serverFiltering: true,

            allowUnsort: true,

            pageSize: 10,

            transport: {
                read: {
                    //url: '../AccessControl/GetAccessControlSummary/',
                    url:url,

                    type: "POST",

                    dataType: "json",

                    contentType: "application/json; charset=utf-8"
                },
                update: {
                    //url: '../AccessControl/GetAccessControlSummary/',
                    url:url,
                    dataType: "json"
                },

                parameterMap: function (options) {

                    return JSON.stringify(options);

                }
            },
            schema: { data: "Items", total: "TotalCount" }
        });
        return gridDataSource;
    },
    
    GetFiscalYear: function (companyId) {
        var objFiscale = "";
        var jsonParam = "";
        var serviceUrl = "../../Company/GetFiscalYear/?companyId=" + companyId;
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objFiscale = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objFiscale;
    },
    

    GetBranchInfoByBranchId: function (branchId) {
        var objBranch = "";
        var jsonParam = "branchId=" + branchId;
        var serviceUrl = "../../Branch/GetBranchInfoByBranchId/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objBranch = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objBranch;
    },
    
    GetSalesRepresentatorType: function () {
        var objSalesRep = "";
        var jsonParam = "";
        var serviceUrl = "../../SalesRepresentator/GetSalesRepresentatorType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objSalesRep = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objSalesRep;
    },
    

    GetAllSalesRepresentatorCombo: function (salesRepType) {
        var objSalesRep = "";
        var jsonParam = "salesRepType=" + salesRepType;
        var serviceUrl = "../../SalesRepresentator/GetAllSalesRepresentatorCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objSalesRep = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objSalesRep;
    },
    
    GetAllSalesRepresentatorComboByCompanyBranchAndType: function (salesRepType,companyId,branchId) {
        var objSalesRep = "";
        var jsonParam = "salesRepType=" + salesRepType +"&companyId="+companyId +"&branchId="+branchId;
        var serviceUrl = "../../SalesRepresentator/GetSalesRepComboByCompanyBranchAndType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objSalesRep = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objSalesRep;
    },
    
    GetSaleRepresentatorByCompanyAndBranch: function (companyId, branchId) {
        var objSalesRep = "";
        var jsonParam = "companyId=" + companyId + "&branchId=" + branchId;
        var serviceUrl = "../../SalesRepresentator/GetSalesRepresentatorByCompanyAndBranch/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objSalesRep = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objSalesRep;
    },
    
    GetDiscountAmountByType: function (discountType) {
        var objDiscount = "";
        var jsonParam = "discountType=" + discountType;
        var serviceUrl = "../../Discount/GetDiscountInfoByType/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {

            objDiscount = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objDiscount;
    },
    
    GetDiscountTypeCombo: function () {
        var objDiscountType = "";
        var jsonParam = "";
        var serviceUrl = "../../Discount/GetDiscountTypeCombo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objDiscountType = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objDiscountType;
    },
    
    GetProductModel: function () {
        var objModel = "";
        var jsonParam = "";
        var serviceUrl = "../Product/GetAllPackageByCompany/?packageType="+0;
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            objModel = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objModel;
    },
    
    GetUserLevel: function (module) {
        var isRootLevelUser = "";
        var jsonParam = "module="+module;
        var serviceUrl = "../Common/CheckIsRootLevelAdmin/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            isRootLevelUser = jsonData;
        }
        function onFailed(error) {
        }
        return isRootLevelUser;
    },
};


var empressCommonHelper = {
    
    initePanelBer: function (ctlDivId) {
        var original = $("#" + ctlDivId).clone(true);
        original.find(".k-state-active").removeClass("k-state-active");

        $(".configuration input").change(function () {
            var panelBar = $("#" + ctlDivId),
                clone = original.clone(true);

            panelBar.data("kendoPanelBar").collapse($("#" + ctlDivId + " .k-link"));

            panelBar.replaceWith(clone);

            initPanelBar();
        });

        var initPanelBar = function () {
            $("#" + ctlDivId).kendoPanelBar({ animation: { expand: { duration: 500, } } });
        };

        initPanelBar();

    },
    GenerareHierarchyCompanyCombo: function (identity) {
        var objCompany = empressCommonManager.GetHierarchyCompany();
        $("#" + identity).kendoComboBox({
            placeholder: "Select Company",
            dataTextField: "CompanyName",
            dataValueField: "CompanyId",
            dataSource: objCompany,
            filter: "contains",
            suggest: true
        });
    },
    
    GenerateBranchCombo: function (companyId, identity) {
        var objBranch = empressCommonManager.GenerateBranchCombo(companyId);
        $("#" + identity).kendoComboBox({
            placeholder: "All",
            dataTextField: "BranchName",
            dataValueField: "BranchId",
            dataSource: objBranch,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem(identity, true);
            }
        });
    },
    GenerateAllBranchCombo: function (rootCompanyId, identity) {
        var objBranch = empressCommonManager.GenerateAllBranchCombo(rootCompanyId);
        $("#" + identity).kendoComboBox({
            placeholder: "All",
            dataTextField: "BranchName",
            dataValueField: "BranchId",
            dataSource: objBranch,
            filter: "contains",
            suggest: true
        });
    },
    
    GenerateBonusType: function (identity, placeholder) {
        var obj = new Object();

        obj = empressCommonManager.GenerateBonusType();

        $("#" + identity).kendoComboBox({
            placeholder: placeholder,
            dataTextField: "BONUSTYPENAME",
            dataValueField: "BONUSTYPEID",
            dataSource: obj
        });
    },
    
    GetDepartmentByCompanyId: function (companyId, identity) {
        var objDepartment = new Object();

        objDepartment = empressCommonManager.GetDepartmentByCompanyId(companyId);

        $("#" + identity).kendoComboBox({
            placeholder: "Select Department",
            dataTextField: "DepartmentName",
            dataValueField: "DepartmentId",
            dataSource: objDepartment
        });

    },
    
    GetTaskType: function (identity,placeHolder) {
        var objTaskType = new Object();

        objTaskType = empressCommonManager.GetTaskTypeInformation();
     
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "Task_Type_Name",
            dataValueField: "Task_Type_Id",
            dataSource: objTaskType
        });

    },
    
    GetLeaveAccumulationType: function (identity, placeHolder) {
        var obj = new Object();

        obj = empressCommonManager.GetLeaveAccumulationType();

        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "ACCUMULATIONTYPENAME",
            dataValueField: "ACCUMULATIONTYPEID",
            dataSource: obj
        });

    },
    
    GetLeaveTypeByAccumulationType: function (identity, placeHolder,accumulationType) {
        var obj = new Object();

        obj = empressCommonManager.GetLeaveTypeByAccumulationType(accumulationType);

        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "TYPENAME",
            dataValueField: "LeaveTypeId",
            dataSource: obj
        });

    },
    
    GetActiveLeaveType: function (identity, placeHolder) {
        var obj = new Object();

        obj = empressCommonManager.GetActiveLeaveType();

        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "TYPENAME",
            dataValueField: "LeaveTypeId",
            dataSource: obj
        });

    },
    
    PopulatePlanedLeaveTypeCombo: function (identity, placeHolder,hrRecordId) {
        var obj = new Object(); 

        obj = empressCommonManager.GetPlanedLeaveType(hrRecordId);
        
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "TypeName",
            dataValueField: "LeaveType",
            dataSource: obj
        });

    },
    
    GenerateEmployeeByCompanyId: function (companyId, branchId, departmentId, identity) {
        var objEmp = new Object();
        if (departmentId == 0) {
            objEmp = null;
        }
        else {
            objEmp = empressCommonManager.GetEmployeeByCompanyIdAndBranchIdAndDepartmentId(companyId, branchId, departmentId);
        }
        $("#" + identity).kendoComboBox({
            placeholder: "All",
            dataTextField: "FullName",
            dataValueField: "HRRecordId",
            dataSource: objEmp
        });
    },

    GenerateEmployeeByDepartmentId: function (departmentId, identity) {
        var objEmp = new Object();
        if (departmentId == 0) {
            objEmp = [];
        }
        else {
            objEmp = empressCommonManager.GetEmployeeByDepartmentId(departmentId);
            
        }



        $("#" + identity).kendoComboBox({
            placeholder: "All",
            dataTextField: "FullName",
            dataValueField: "HRRecordId",
            dataSource: objEmp
        });
    },
    
    GenerateEmployeeMultySelectByCompanyId: function (companyId, branchId, departmentId, identity) {
        var objEmp = new Object();
        if (departmentId == 0) {
            objEmp = null;
        }
        else {
            objEmp = empressCommonManager.GetEmployeeByCompanyIdAndBranchIdAndDepartmentId(companyId, branchId, departmentId);
        }
        $("#" + identity).kendoComboBox({
            placeholder: "All",
            dataTextField: "FullName",
            dataValueField: "HRRecordId",
            dataSource: objEmp
        });
        
        
    },
    
    EmployeeTypeCombo: function (identity) {
        var objEmployeeType = new Object();
        objEmployeeType = empressCommonManager.GetEmployeeType();
        $("#" + identity).kendoComboBox({
            placeholder: "All",
            dataTextField: "EmployeeTypeName",
            dataValueField: "EmployeeTypeId",
            dataSource: objEmployeeType
        });
    },

    GetSalaryStatus: function (identity,placeHolder) {
        var objStatus = new Object();
        objStatus = empressCommonManager.GetSalaryStatus();
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "StateName",
            dataValueField: "WFStateId",
            dataSource: objStatus
        });
    },
    
    GetPayrollStatus: function (identity, placeHolder) {
        var objStatus = new Object();
        objStatus = empressCommonManager.GetPayrollStatus();
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "StateName",
            dataValueField: "WFStateId",
            dataSource: objStatus
        });
    },
    
    GetBonusStatus: function (identity, placeHolder) {
        var objStatus = new Object();
        objStatus = empressCommonManager.GetBonusStatus();
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "StateName",
            dataValueField: "WFStateId",
            dataSource: objStatus
        });
    },
    
    GetCoffStatus: function (identity, placeHolder) {
        var objStatus = new Object();
        objStatus = empressCommonManager.GetCoffStatus();
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "StateName",
            dataValueField: "WFStateId",
            dataSource: objStatus
        });
    },
    
    GetLeaveForwadingStatus: function (identity, placeHolder) {
        var objStatus = new Object();
        objStatus = empressCommonManager.GetLeaveForwadingStatus();
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "StateName",
            dataValueField: "WFStateId",
            dataSource: objStatus
        });
    },
        
    GetProjectStatus: function (identity, placeHolder) {
        var objStatus = new Object();
        objStatus = empressCommonManager.GetProjectStatus();
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "StateName",
            dataValueField: "WFStateId",
            dataSource: objStatus
        });
    },
    
    populateClientCombo: function (companyId, identity) {
        var objClient = new Object();
        
        objClient = empressCommonManager.GetClient(companyId);
        $("#" + identity).kendoComboBox({
            placeholder: "Select Client...",
            dataTextField: "ClientName",
            dataValueField: "ClientCode",
            dataSource: objClient
        });

    },
    
    checkApproverUser: function (accessArray) {
        var approver = false;
        for (var i = 0; i < accessArray.length; i++) {
            if (accessArray[i].ReferenceID == 4) {
                approver = true;
                break;
            }
        }
        return approver;
    },
    
    checkIsPossibleClosedStatus: function (statusArray,stateId) {
        var isPosibleClosed = false;
        for (var i = 0; i < statusArray.length; i++) {
            if (statusArray[i].WFStateId == stateId) {
                if (statusArray[i].IsClosed == 1) {
                    isPosibleClosed = true;
                }
                break;
            }
        }
        return isPosibleClosed;
    },
    
    checkIsClosedStatus: function (statusArray, stateId) {
        var isClosed = false;
        for (var i = 0; i < statusArray.length; i++) {
            if (statusArray[i].WFStateId == stateId) {
                if (statusArray[i].IsClosed == 2) {
                    isClosed = true;
                }
                break;
            }
        }
        return isClosed;
    },
    
    GenerateDesignationCombo: function (companyId,identity) {
        var objDesignation = new Object();
        if (companyId != 0) {
            objDesignation = empressCommonManager.GenerateDesignationCombo(companyId);
        }
        else {
            objDesignation = null;
        }
        
        $("#" + identity).kendoComboBox({
            placeholder: "Select Designation",
            dataTextField: "DesignationName",
            dataValueField: "DesignationId",
            dataSource: objDesignation
        });
    },
    
    GenerateDesignationByDepartmentIdCombo: function (departmentId, identity) {
        var objDesignation = new Object();
        if (departmentId != 0) {
            objDesignation = empressCommonManager.GenerateDesignationByDepartmentIdCombo(departmentId);
        }
        else {
            objDesignation = null;
        }

        $("#" + identity).kendoComboBox({
            placeholder: "Select Designation",
            dataTextField: "DesignationName",
            dataValueField: "DesignationId",
            dataSource: objDesignation
        });
    },
    

    //Will be deleted
    GenerateGradeComboByCompanyAndType: function (companyId, payrollTypeId,identity) {

        var objGrade = null;
        if (companyId != 0) {
            objGrade = empressCommonManager.GetGradeByCompanyAndPayroll(companyId, payrollTypeId);
        }

        $("#" + identity).kendoComboBox({
            placeholder: "Select Grade Name",
            dataTextField: "GradeName",
            dataValueField: "GradeSettingsId",
            dataSource: objGrade,
            //change: PayrollHelper.onChangeGradeType
        });

        $("#" + identity).parent().css('width', "20.6em");
    },
    
    //New 
    
    GenerateGradeComboByCompanyId: function (companyId, identity) {

        var objGrade = null;
        if (companyId != 0) {
            objGrade = empressCommonManager.GenerateGradeComboByCompanyId(companyId);
        }

        $("#" + identity).kendoComboBox({
            placeholder: "Select Grade Name",
            dataTextField: "GradeName",
            dataValueField: "GradeId",
            dataSource: objGrade,
            //change: PayrollHelper.onChangeGradeType
        });

        $("#" + identity).parent().css('width', "17.4em");
    },

    GenerateCertificateTypeCombo: function (identity) {
        var objCertificateType = new Object();
        objCertificateType = empressCommonManager.GetCertificateType();

        $("#" + identity).kendoComboBox({
            placeholder: "Select Certificate Type",
            dataTextField: "CertificateTypeName",
            dataValueField: "CertificateTypeId",
            dataSource: objCertificateType
        });
    },
    
    populateMonthCombo: function (identity) {
        $("#" + identity).kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
               { text: "January", value: "1" },
            { text: "February", value: "2" },
            { text: "March", value: "3" },
            { text: "April", value: "4" },
            { text: "May", value: "5" },
            { text: "June", value: "6" },
            { text: "July", value: "7" },
            { text: "August", value: "8" },
            { text: "September", value: "9" },
            { text: "October", value: "10" },
            { text: "November", value: "11" },
            { text: "December", value: "12" }
            ],
            filter: "contains",
            suggest: true
        });
        var month = new Date().getMonth() + 1;
        var monthCombo = $("#" + identity).data("kendoComboBox");
        monthCombo.value(month);
    },

    GenerateYearCombo: function (identity) {
        $("#" + identity).kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "2010", value: "2010" },
                { text: "2011", value: "2011" },
                { text: "2012", value: "2012" },
                { text: "2013", value: "2013" },
                { text: "2014", value: "2014" },
                { text: "2015", value: "2015" },
                { text: "2016", value: "2016" },
                { text: "2017", value: "2017" },
                { text: "2018", value: "2018" },
                { text: "2019", value: "2019" },
                { text: "2020", value: "2020" }
            ],
            filter: "contains",
            suggest: true
        });

        var year = new Date().getFullYear();
        var yearCombo = $("#" + identity).data("kendoComboBox");
        yearCombo.value(year);
    },
    
    populateMovementTypeCombo: function (identity) {
        $("#" + identity).kendoComboBox({
            placeholder: "Select Movement Type",
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "On Prayer", value: "1" },
                { text: "At Lunch", value: "2" },
                { text: "On Client Visit", value: "3" },
                { text: "Short Leave", value: "5" }
            ],
            filter: "contains",
            suggest: true
        });
    },
    
    populateEncashmentTypeCombo: function (identity) {
        $("#" + identity).kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "None", value: "0" },
                { text: "Full", value: "1" },
                { text: "Half", value: "2" }
            ],
            filter: "contains",
            suggest: true,
            index: 0
        });
    },
    
    populateValuTypeCombo: function (identity) {

        $("#" + identity).kendoComboBox({
            placeholder: "Select Value Type",
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Fixed", value: "0" },
                { text: "Percentage", value: "1" }
            ],
            filter: "contains",
            suggest: true
        });

    },
    
    populateCtcCombo: function (identity,placeholderText) {


        var obj = new Object();
        obj = empressCommonManager.GetCtcInformation();
        $("#" + identity).kendoComboBox({
            placeholder: placeholderText,
            dataTextField: "CtcName",
            dataValueField: "CtcId",
            dataSource:obj,
            filter: "contains",
            suggest: true
        });

    },
    
    populateCtcTypeCombo: function (identity, placeholderText) {


        var obj = new Object();
        obj = empressCommonManager.GetCtcTypes();
        $("#" + identity).kendoComboBox({
            placeholder: placeholderText,
            dataTextField: "CtcTypeName",
            dataValueField: "CtcTypeId",
            dataSource: obj,
            filter: "contains",
            suggest: true
        });

    },
    
    populateCtcOperatorCombo: function (identity) {

        $("#" + identity).kendoComboBox({
            placeholder: "Select Value Type",
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Adition", value: "1" },
                { text: "Deduction", value: "2" }
            ],
            filter: "contains",
            suggest: true
        });

    },
    
    populateLoanType: function (identity, placeholderText) {


        var obj = new Object();
        obj = empressCommonManager.GetLoanTypes();
        $("#" + identity).kendoComboBox({
            placeholder: placeholderText,
            dataTextField: "LoanTypeName",
            dataValueField: "LoanTypeId",
            dataSource: obj,
            filter: "contains",
            suggest: true
        });

    },
    
    populateEmiType: function (identity) {

        $("#" + identity).kendoComboBox({
            placeholder: "Select Emi Type",
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Monthly", value: "1" },
                { text: "Quaterly", value: "2" },
                { text: "HalfYearly", value: "3" },
                { text: "Yearly", value: "4" }
            ],
            filter: "contains",
            suggest: true
        });

    },
    
    populateRepostingType: function (identity,placeholderText) {
        
        var obj = new Object();
        obj = empressCommonManager.populateRepostingType();
        $("#" + identity).kendoComboBox({
            placeholder: placeholderText,
            dataTextField: "PostingTypeName",
            dataValueField: "PostingTypeId",
            dataSource: obj,
            filter: "contains",
            suggest: true
        });

    },
    
    GenerateFunctionCombo: function (identity, placeholderText) {
        var objFunction = new Object();

        objFunction = empressCommonManager.GenerateFunctionCombo();
        $("#" + identity).kendoComboBox({
            placeholder: placeholderText,
            dataTextField: "Function_Name",
            dataValueField: "Func_Id",
            dataSource: objFunction
        });

    },
    
    GenerateShiftComboByCompanyAndBranch: function (companyId, branchId, identity,placeHolderText) {
        var objShift = new Object();
        if (companyId != 0) {
            objShift = empressCommonManager.GenerateShiftComboByCompanyAndBranch(companyId, branchId);
        }
        else {
            objShift = null;
        }


        $("#" + identity).kendoComboBox({
            placeholder: placeHolderText,
            dataTextField: "ShiftName",
            dataValueField: "ShiftId",
            dataSource: objShift
        });
    },
    
    populateCtcCategoryCombo: function (identity,placeHolder) {
        var obj = new Object();

        obj = empressCommonManager.GetCtcCategoryInformation();
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "CtcCategoryName",
            dataValueField: "CtcCategoryId",
            dataSource: obj
        });

    },
    
    populateGradeTypeCombo: function (identity) {
        var obj = new Object();
        obj = empressCommonManager.GetGradeType();
        $("#" + identity).kendoComboBox({
            placeholder: "Please Select a Grade",
            dataTextField: "GradeTypeName",
            dataValueField: "GradeTypeId",
            dataSource: obj
        });
    },
    populateTrainingTypeCombo: function (identity) {
        var obj = new Object();
        obj = empressCommonManager.GetTrainingType();
        $("#" + identity).kendoComboBox({
            placeholder: "Please Select a Training Type",
            dataTextField: "TrainingTypeName",
            dataValueField: "TrainingTypeId",
            dataSource: obj
        });
    },
    populateSurveyQuestionCategoryCombo: function (identity) {
        var obj = new Object();
        obj = empressCommonManager.GetDataForAnyCombo("../SurveyQuestion/GetSurveyQuestionDataForCombo");
        $("#" + identity).kendoComboBox({
            placeholder: "Please Select a Survey Question",
            dataTextField: "QuestionCategoryDescription",
            dataValueField: "QuestionCategoryId",
            dataSource: obj
            //index: 0
        });
    },
    populateSurveyCategoryCombo: function (identity) {
        var obj = new Object();
        obj = empressCommonManager.GetDataForAnyCombo("../PublishSurvey/GetSurveyCategoryDataForCombo");
        $("#" + identity).kendoComboBox({
            placeholder: "Please Select a Survey Category",
            dataTextField: "SurveyCategoryDescription",
            dataValueField: "SurveyCategoryId",
            dataSource: obj
            //index: 0
        });
    },
    commonValidator: function (ctlId) {
        var data = [];
        var validator = $("#" + ctlId).kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }

    },
    
    populateFiscalCombo: function (identity, placeHolder,companyId) {
        var objFiscalYear = new Object();
        objFiscalYear = empressCommonManager.GetFiscalYear(companyId);
        $("#" + identity).kendoComboBox({
            placeholder: placeHolder,
            dataTextField: "FiscalYearName",
            dataValueField: "FiscalYearId",
            dataSource: objFiscalYear,
            index: 0
        });
    },
    initePanelBer: function (ctlDivId) {
        var original = $("#" + ctlDivId).clone(true);
        original.find(".k-state-active").removeClass("k-state-active");

        $(".configuration input").change(function () {
            var panelBar = $("#" + ctlDivId),
                clone = original.clone(true);

            panelBar.data("kendoPanelBar").collapse($("#" + ctlDivId + " .k-link"));

            panelBar.replaceWith(clone);

            initPanelBar();
        });

        var initPanelBar = function () {
            $("#" + ctlDivId).kendoPanelBar({ animation: { expand: { duration: 500, } } });
        };

        initPanelBar();

    },
    
    populateDiscountTypecombo: function (identity) {
        $("#" + identity).kendoComboBox({
            placeholder: "Select Discount Type",
            dataTextField: "DiscountTypeName",
            dataValueField: "DiscountTypeCode",
            dataSource: empressCommonManager.GetDiscountTypeCombo(),
          //  index: 0,
            filter: "contains",
            suggest: true
        });
    },

    PopulateSaleRepType: function (identity) {
        $("#"+identity).kendoComboBox({
            placeholder: "Select Type",
            dataTextField: "SalesRepTypeName",
            dataValueField: "SalesRepTypeId",
            dataSource: empressCommonManager.GetSalesRepresentatorType(),
            filter: "contains",
            suggest: true,

        });
    },
    
    GenerateModelCombo: function (identity) {
        var objModel = empressCommonManager.GetProductModel();
        $("#" + identity).kendoComboBox({
            placeholder: "Select Model",
            dataTextField: "Model",
            dataValueField: "ModelId",
            dataSource: objModel,
            filter: "contains",
            suggest: true,
            change: function () {
                AjaxManager.isValidItem(identity, true);
            }
        });
    },
};