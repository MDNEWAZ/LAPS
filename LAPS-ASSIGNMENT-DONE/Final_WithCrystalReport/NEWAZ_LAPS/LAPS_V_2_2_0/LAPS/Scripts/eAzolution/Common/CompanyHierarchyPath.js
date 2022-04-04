
var gbLblRootCompany = "";
var gbLblMotherCompany = "";
var gbLblCompany = "";
var gbLblBranch = "";

var CompanyHeirarchyPathManager = {
    GetCompanyHeirarchyPathData: function (companyId, branchId) {
        var objCompanyHeirarchy = "";
        var jsonParam = "companyId=" + companyId + "&branchId=" + branchId ;
        var serviceUrl = "../Common/GetCompanyHeirarchyPathData/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objCompanyHeirarchy = jsonData;
         
            var pathName = window.location.pathname;
            var pageName = pathName.substring(pathName.lastIndexOf('/') + 1);
            if (pageName == "Sale") {
                SaleHelper.LoadCompanyHeirarchyPath(jsonData);
            }
            else if (pageName == "SimplePaymentCollectionSettings") {
                SimplePaymentHelper.LoadCompanyHeirarchyPath(jsonData);
            }
            
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objCompanyHeirarchy;
    },
};

var CompanyHeirarchyPathHelper = {
  
};