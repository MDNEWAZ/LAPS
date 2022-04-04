var gbDueName = "";
var dueDetailsManager = {

    SaveDue: function () {
        var objDue = dueDetailsManager.GetADue_Object();
        if (objDue.FromDue != "" && objDue.ToDue!="") {
            var objDueInfo = JSON.stringify(objDue).replace(/&/g, "^");
            var jsonParam = 'objDueInfo:' + objDueInfo;
            var serviceUrl = "../RatingConfiguration/DueSave/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        } else {
            AjaxManager.MsgBox('warning', 'center', 'Minimum Requirement:', 'Please Enter Due Percent.',
                 [{
                     addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                         $noty.close();
                         return false;
                     }
                 }]);
        }
        function onSuccess(jsonData) {

            if (jsonData == "Success") {
                dueDetailsHelper.clearDueForm();
                AjaxManager.MsgBox('success', 'center', 'Success', 'Rating Saved Or Updated Successfully.',
                  [{
                      addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                          $noty.close();
                      }
                  }]);
                $("#gridDue").data("kendoGrid").dataSource.read();
            }
            else if (jsonData == "Already Exist") {
                AjaxManager.MsgBox('warning', 'center', 'Alresady Exist:', 'Rating Name already exist.',
                      [{
                          addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                              $noty.close();
                          }
                      }]);
            }
            else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                       [{
                           addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                               $noty.close();
                           }
                       }]);
            }
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },
    GetADue_Object: function () {
        var objDue = new Object();
        objDue.DueId = $('#hdnDueId').val();
        objDue.ACompany = { CompanyId: $('#cbmDueCompanyName').val() };
        objDue.FromDue = $("#txtFromDue").val();
        objDue.ToDue = $("#txtToDue").val();
        objDue.AAllType = { TypeId: $("#cmbColor").data('kendoComboBox').value() };
        objDue.Status = $("#chkIsActiveDue").is(":checked") == true ? 1 : 0;
        return objDue;
    }
};

var dueDetailsHelper = {
    populateColorCombo: function () {
        $("#cmbColor").kendoComboBox({
            placeholder: "Select Color",
            dataTextField: "Name",
            dataValueField: "Id",
            dataSource: [
                { Name: "Green", Id: 1 },
                { Name: "Yellow", Id: 2 },
                { Name: "Orenge", Id: 3 },
                { Name: "Red", Id: 4 }],

            filter: "contains",
            suggest: true
            //dataBound: function (e) {
              
            //    // handle the event
            //    //var combo = $("#cmbColor").data().kendoComboBox();
            //    var data = this.dataSource.data();
            //    debugger;
            //    for (var i = 0; i < data.length; i++) {
            //        //this.list.css({ 'background-color': data[i].Name });
            //        this.list.append("<span style='background-color:red'></span>");
            //        //this.input.css({ 'background-color': data[i].Name });

            //    }
               
            //}
        });
    },
    FillDueDetailsInForm: function (objDue) {
           
            dueDetailsHelper.clearDueForm();
            $('#hdnDueId').val(objDue.DueId);
            $("#cbmDueCompanyName").data("kendoComboBox").value(objDue.ACompany.CompanyId);
            $("#txtFromDue").val(objDue.FromDue);
            $("#txtToDue").val(objDue.ToDue);
            $('#cmbColor').data('kendoComboBox').value(objDue.AAllType.TypeId);
            if (objDue.Status == 1) {
                $('#chkIsActiveDue').attr('checked', 'checked');
            } else {
                $('#chkIsActiveDue').removeAttr('checked', 'checked');
            } 
           

    },
    clearDueForm: function () {
        $('#hdnDueId').val(0);
        $('#cbmDueCompanyName').data('kendoComboBox').value('');
        $('#txtFromDue').val('');
        $('#txtToDue').val('');
        $('#cmbColor').data("kendoComboBox").value("");
        $('#chkIsActiveDue').removeAttr('checked', 'checked');
    }
};