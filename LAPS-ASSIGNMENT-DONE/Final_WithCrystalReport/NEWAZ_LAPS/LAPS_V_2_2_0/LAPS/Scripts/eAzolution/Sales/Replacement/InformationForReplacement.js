
var InformationForReplacementManager={};


var InformationForReplacementHelper = {
    
    GenerateInstallDate:function() {
        $("#txtInstallDateRe").kendoDatePicker();
    },
    

    FillReplacementAllInformation: function (customerCode) {
        if (customerCode != "") {
          var customerAndPackageInfo = ReplacementDetailsManager.GetCustomerAndSaleInfoByCustomerCode(customerCode);
          if (customerAndPackageInfo != null) {
              InformationForReplacementHelper.FillCustomerInfo(customerAndPackageInfo);
              InformationForReplacementHelper.FillPackageCombo(customerAndPackageInfo.PackageInfos);
            
          } else {
              AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Data Not Found!',
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();
                               ReplacementDetailsHelper.ClearAllSaleDetailsForm();
                               ReplacementDetailsHelper.ClearReplacementDetailsForm();
                           }
                       }]);

          }

      } else {
          AjaxManager.MsgBox('warning', 'center', 'Warning:', 'Please Enter Customer Code!',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                             ReplacementDetailsHelper.ClearAllSaleDetailsForm();
                             ReplacementDetailsHelper.ClearReplacementDetailsForm();
                         }
                     }]);
      }
       
   
       

    },
    
    FillCustomerInfo: function (obj) {
        $("#txtCustomerNameRe").val(obj.Name);
        
    },
    
    FillPackageCombo: function (packageInfo) {
        var combo = $("#cmbModelProInfo").data("kendoComboBox");
        combo.setDataSource(packageInfo);
        
        ReplacementproductInfoManager.ProductSaleInfoFillByModel();

    }
};