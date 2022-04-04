
var DiscountInfoManager = {
    GetDiscountInfo: function (saleId) {
        var objDiscountInfo = "";
        var jsonParam = "saleId="+saleId;
        var serviceUrl = "../Discount/GetDiscountInfo/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            objDiscountInfo = jsonData;
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
        return objDiscountInfo;
    },
    

   
};


var DiscountInfoHelper = {

    InitDiscountInfo: function () {
        DiscountInfoHelper.populateDiscountOptioncombo();
      //  DiscountInfoHelper.populateDiscountTypecomboWithoutData();
         empressCommonHelper.populateDiscountTypecombo("cmbDiscountType");
         DiscountInfoHelper.DiscountTypeChangeEvent();
        DiscountInfoHelper.PopulateDiscountAmount();
        $("#btnDiscountAmountPercentage").hide();
    },

    populateDiscountOptioncombo: function () {

        $("#cmbDiscountOption").kendoComboBox({
            placeholder: "Select Discount Option",
            dataTextField: "DiscountOptionName",
            dataValueField: "DiscountOptionId",
            dataSource: [{ DiscountOptionName: "Fixed Amount Discount", DiscountOptionId: 1 },
                         { DiscountOptionName: "Percentage wise Discount", DiscountOptionId: 2 }],
            //index:0,
            suggest: true,
            change: function () {
              
                DiscountInfoHelper.ChangeEventForDiscountOptionCombo(this.value());
            }
        });
    },

    populateDiscountTypecomboWithoutData: function () {
        $("#cmbDiscountType").kendoComboBox({
            placeholder: "Select Discount Type",
            dataTextField: "DiscountTypeName",
            dataValueField: "DiscountTypeId",
            dataSource: [],
           // index: 0,
            suggest: true,

        });
    },

    DiscountTypeChangeEvent: function () {
        $("#cmbDiscountType").change(function () {
            DiscountInfoHelper.PopulateDiscountAmount();
            
        });
    },
    
    PopulateDiscountAmount: function () {
    
        var discountInfo = null;

        var discountOption = $("#cmbDiscountOption").data("kendoComboBox").value();
        var discountType = $("#cmbDiscountType").data("kendoComboBox").value();
            discountType.trim();

        if (discountOption != 0) {

            if (discountOption == 1) {//Fixed Amount Discount
                if (discountType == "01") { //Cash Discount
                    discountInfo = empressCommonManager.GetDiscountAmountByType(discountType);
                    $("#txtDiscountAmount").val(discountInfo.DefaultCashDiscount);
                } else if (discountType == "02") { //Special Discount
                    $("#txtDiscountAmount").val("Need Approval");
                }
                
                DiscountInfoHelper.DiscountCalculation(discountOption,discountType);
            }


            else if (discountOption == 2) {//Percentage Wise Discount

                if (discountType == "01") { //Cash Discount
                    discountInfo = empressCommonManager.GetDiscountAmountByType(discountType);
                    $("#txtDiscountAmount").val(discountInfo.CashDiscountPercentage);

                } else if(discountType=="02") { //Special Discount
                    $("#txtDiscountAmount").val("Need Approval");
                }
                
                DiscountInfoHelper.DiscountCalculation(discountOption,discountType);
            }

        }
    },

    ChangeEventForDiscountOptionCombo: function (type) {
      
        if (type == 2) {
            $("#btnDiscountAmountPercentage").show();
        } else {
            $("#btnDiscountAmountPercentage").hide();
        }
        
        $("#cmbDiscountType").data("kendoComboBox").value("");
        $("#txtDiscountAmount").val("");
      
        empressCommonHelper.populateDiscountTypecombo("cmbDiscountType");
        DiscountInfoHelper.PopulateDiscountAmount();

    },
    
    FillDiscountInfoForm: function (discountInfo) {
        $("#hdnIsApproveSpecialDiscount").val(discountInfo.IsApprovedSpecialDiscount);
        $("#hdnDiscountId").val(discountInfo.DiscountId);
        $("#cmbDiscountOption").data("kendoComboBox").value(discountInfo.DiscountOptionId);
        empressCommonHelper.populateDiscountTypecombo("cmbDiscountType");
        $("#cmbDiscountType").data("kendoComboBox").value(discountInfo.DiscountTypeCode.trim());
        if (discountInfo.DiscountTypeCode == "02") {
            if (discountInfo.DiscountAmount > 0) {
                $("#txtDiscountAmount").val(discountInfo.DiscountAmount);
            } else {
                $("#txtDiscountAmount").val("Need Approval");
            }
           
        }
        else if (discountInfo.DiscountTypeId == "02" && discountInfo.IsApprovedSpecialDiscount == 1) {
            $("#txtDiscountAmount").val(discountInfo.DiscountAmount);
        } else {
            $("#txtDiscountAmount").val(discountInfo.DiscountAmount);
        }
       
    },
    
    DiscountCalculation: function (discountOption,discountType) {
   
        var salesObject = saleDetailsHelper.GetSalesObj();
        var packagePrice = $("#txtPackagePrice").html();
      
        if (packagePrice != "") {
            if (discountOption == 1) {
                if (discountType == "01") {
                    var discountAmount = $("#txtDiscountAmount").val();
                    salesObject.price = (packagePrice - discountAmount);
                } else {
                    salesObject.price = packagePrice;
                }
            }
            else if (discountOption == 2) {
                if (discountType == "01") {
                    var discountPercent = $("#txtDiscountAmount").val();
                    var discountAmt = (packagePrice * discountPercent) / 100;
                    salesObject.price = (packagePrice - discountAmt);
                } else {
                    salesObject.price = packagePrice;
                }
            }
            ProductItemInfoHelper.CalculateNetTotalPrice(salesObject);
            saleDetailsHelper.InstallmentData(salesObject);
        }
        
    }



};

