var PersonName = "";
var PersonDetailManager = {
    SavePersonInformation: function () {

       
        var obj = PersonDetailHelper.CreatePersonalInfoObject();

        var objPersonal = JSON.stringify(obj);
        var jsonParam = 'objPerson:' + objPersonal;
        var serviceUrl = "../PersonalDetails/SavePersonDetails/";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);


        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Person Save Successfully',
                [
                    {
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            $("#PersonalSummaryDiv").data("kendoGrid").dataSource.read();
                        }
                    }
                ]);

            } else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                [
                    {
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }
                ]);
            }

        }
        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
          [{
              addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                  $noty.close();
              }
          }]);
        }
    },
    DeletePersonalInfo: function () {
        var obj = PersonDetailHelper.CreatePersonalInfoObject();
        var objPersonal = JSON.stringify(obj);
        var jsonParam = 'objPerson:' + objPersonal;
        var serviceUrl = "../PersonalDetails/DeletePersonalInfo/";
        AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Person Deleted Successfully',
                [
                    {
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            $("#PersonalSummaryDiv").data("kendoGrid").dataSource.read();
                        }
                    }
                ]);

            } else {
                AjaxManager.MsgBox('error', 'center', 'Error', jsonData,
                [
                    {
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }
                ]);
            }

        }
        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
          [{
              addClass: 'btn btn-primary', text: 'Ok', onClick: function ($noty) {
                  $noty.close();
              }
          }]);
        }

    },


};





var PersonDetailHelper = {
    dropReligion: function () {
        var commonData = [{ ReligionId: 1, text: 'Islam' }, { ReligionId: 2, text: 'Hinduism' },
        { ReligionId: 3, text: 'Christianity' }, { ReligionId: 4, text: "Buddhism" }, { ReligionId: 5, text: 'Others' }];
        $("#dropReligion").kendoDropDownList({
            optionLabel: "Select Religion..",
            dataTextField: "text",
            dataValueField: "ReligionId",
            dataSource: commonData
        });
    },

    maritalStatusDrop: function () {
        var commonData = [{ MaritalStatusId: 1, text: 'Married' }, { MaritalStatusId: 2, text: 'Unmarried' }];
        $("#MaritalStatusDrop").kendoDropDownList({
            optionLabel: "Select Marital Status..",
            dataTextField: "text",
            dataValueField: "MaritalStatusId",
            dataSource: commonData
        });

    },

    txtDateOfBirth: function () {
        $("#txtDateOfBirth").kendoDatePicker({
            format:'dd-MMM-yyyy'
        });

    },
    CreatePersonalInfoObject: function () {

        var objPerson = new Object();
        objPerson.PersonalDetailsId = $("#hdnPersonalInformationDetails").val();
        objPerson.FirstName = $("#txtFirstName").val();
        objPerson.LastName = $("#txtLastName").val();
        objPerson.FatherName = $("#txtFatherName").val();
        objPerson.MotherName = $("#txtMotherName").val();
        objPerson.DateOfBirth = $("#txtDateOfBirth").data("kendoDatePicker").value();
        objPerson.Gender = $('input[name="rdoGender"]:checked').val();
        objPerson.Maritalstatus = $("#MaritalStatusDrop ").data("kendoDropDownList").value();
        objPerson.NationalIdNo = $("#txtNationalIdNo").val();
        objPerson.Religion = $("#dropReligion").data("kendoDropDownList").value();
        objPerson.Mobile = $("#txtMobile").val();
        objPerson.Address = $("#txtAddress").val();



        return objPerson;
    },

    clearPersonalForm: function () {

        $("#hdnPersonalInformationDetails").val("0");
        $("#txtFirstName").val("");
        $("#txtLastName").val("");
        $("#txtFatherName").val("");
        $("#txtMotherName").val("");
        $("#txtDateOfBirth").val("");
        $("#rdoMale").prop("checked", false);
        $("#rdoFemale").prop("checked", false);
        $("#rdoOther").prop("checked", false);
        $("#MaritalStatusDrop").data("kendoDropDownList").value("");
        $("#txtNationality").val("");
        $("#txtNationalIdNo").val("");
        $("#dropReligion").data("kendoDropDownList").value("");
        $("#txtMobile").val("");
        $("#txtAddress").val("");

    },

    populatePersonDetail: function (objPerson) {
       
        PersonDetailHelper.clearPersonalForm();
        $("#hdnPersonalInformationDetails").val(objPerson.PersonalDetailsId);
        $("#txtFirstName").val(objPerson.FirstName);
        $("#txtLastName").val(objPerson.LastName);
        $("#txtFatherName").val(objPerson.FatherName);
        $("#txtMotherName").val(objPerson.MotherName);
        $("#txtDateOfBirth").data('kendoDatePicker').value(objPerson.DateOfBirth);
        $("#rdoGender").val(objPerson.Gender);
        if (objPerson.Gender == 1) {
            $("#rdoMale").prop("checked", true);
        }
        if (objPerson.Gender == 2) {
            $("#rdoFemale").prop("checked", true);
        }
        if (objPerson.Gender == 3) {
            $("#rdoOther").prop("checked", true);
        }
        $("#MaritalStatusDrop").data("kendoDropDownList").value(objPerson.Maritalstatus);
        $("#txtNationalIdNo").val(objPerson.NationalIdNo);
        $("#dropReligion").data("kendoDropDownList").value(objPerson.Religion);
        $("#txtMobile").val(objPerson.Mobile);
        $("#txtAddress").val(objPerson.Address);          
        
        
    },

   



};
