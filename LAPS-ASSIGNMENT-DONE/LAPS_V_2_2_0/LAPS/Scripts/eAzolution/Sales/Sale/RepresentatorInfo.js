

var RepresentatorInfoHelper = {

    populateSalesRepresentatorCombo: function () {

        $("#cmbSaleRepresentator").kendoComboBox({
            placeholder: "----------Select----------",
            dataTextField: "SalesRepId",
            dataValueField: "SalesRepId",
            dataSource: empressCommonManager.GetAllSalesRepresentatorCombo(0),

            suggest: true,
            change:function() {
                RepresentatorInfoHelper.ChangeEventForSaleRepresentatorCombo(this.value());
            } 
        });
    },
    
    ChangeEventForSaleRepresentatorCombo: function (salesRepId) {
        var objSalesRep = "";
        var jsonParam = "salesRepId=" + salesRepId;
        var serviceUrl = "../SalesRepresentator/GetAllSalesRepresentatorById/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            if (jsonData != null) {
                objSalesRep = jsonData;
                $("#txtRepresentatorType").val(objSalesRep.SalesRepTypeName);
                $("#txtRepresentatorCode").val(objSalesRep.SalesRepCode);
            }
          
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objSalesRep;

      
    }   
};