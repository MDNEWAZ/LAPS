

var WaitingForDiscountDetailsManager = {
    SaveWaitingForDiscountInformation: function (selectedItem) {
        if (WaitingForDiscountDetailsHelper.validator(selectedItem)) {

            AjaxManager.MsgBox('information', 'center', 'Confirmation:', 'Are You Sure to Approve Special Discount ? ',
              [{
                  addClass: 'btn btn-primary',
                  text: 'Yes',
                  onClick: function ($noty) {
                      $noty.close();
                      var dpApplicableStage = $("#dpApplicableStage input[type='radio']:checked").val();
                      var objWaitingForDiscount = JSON.stringify(selectedItem).replace(/&/g, "^");
                      var jsonParam = 'objWaitingForDiscount:' + objWaitingForDiscount + ",dpApplicabeStage:" + dpApplicableStage;
                      var serviceUrl = "../WaitingForDiscount/ApproveWaitingForDiscount/";
                      AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
                  }
              }, {
                  addClass: 'btn', text: 'No', onClick: function ($noty) { $noty.close(); }
              }]);

        }
        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                AjaxManager.MsgBox('success', 'center', 'Success:', 'Discounted Successfully',
                    [{
                        addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                            $noty.close();
                            $('#gridWaitingForDiscount').data('kendoGrid').dataSource.read();
                        }
                    }]);
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Failed', jsonData,
                        [{
                            addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
            }
        }

        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Failed', error.statusText,
                         [{
                             addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                                 $noty.close();
                             }
                         }]);
        }
    }
};
var WaitingForDiscountDetailsHelper = {

    InitWaitingForSpecialDiscount:function() {
        WaitingForDiscountDetailsHelper.CustomerInfoPopup();
    },

    validator: function (selectedItem) {

        //if ((selectedItem.DiscountAmount == 0 && selectedItem.DiscountedAmount == 0) || (selectedItem.DiscountAmount > 100 && $("#check_rowForIsPercentage" + selectedItem.CustomerId).is(':checked') == true) || selectedItem.DiscountedAmount < 0) {
        //    AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please make some discount',
        //             [{
        //                 addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
        //                     $noty.close();
        //                     return false;
        //                 }
        //             }]);
        //    return false;
        //} else {
        //    return true;
        //}



        if ( (selectedItem.DiscountAmount > 100 && $("#check_rowForIsPercentage" + selectedItem.CustomerId).is(':checked') == true) || selectedItem.DiscountedAmount < 0) {
            AjaxManager.MsgBox('warning', 'center', 'Warning', 'Please make some discount',
                     [{
                         addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                             $noty.close();
                             return false;
                         }
                     }]);
            return false;
        } else {
            return true;
        }

    },


    CustomerInfoPopup: function () {
        $("#divCustomerInfo").kendoWindow({
            title: "Sale Details",
            resizable: false,
            modal: true,
            width: "60%",
            draggable: true,
            open: function (e) {
                this.wrapper.css({ top: 50 });
            }
        });
    },



    InitTreeView: function () {
        WaitingForDiscountDetailsHelper.LoadTreeView();

        $("#treeview .k-in").on("dblclick", function (e) {
            var tv = $('#treeview').data('kendoTreeView');
            var selected = tv.select();
            var item = tv.dataItem(selected);

            $("#txtText").val(item.Name);

        });

    },

    LoadTreeView: function () {

        $("#treeview").kendoTreeView({
            animation: false,
            dataTextField: "Name",
            dataValueField: "Id",
            dataSource: [
                { id: 1, Name: "foo" },
                { id: 2, Name: "bar" }
            ],

        });
    },



  

};