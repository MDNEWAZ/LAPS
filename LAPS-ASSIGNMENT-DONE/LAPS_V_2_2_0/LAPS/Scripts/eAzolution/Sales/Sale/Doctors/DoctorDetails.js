var DoctorName = "";
var DoctorDetailsManager = {


    SaveDoctorInformation: function () {
        debugger;
        if (DoctorDetailsHelper.ValidateDoctorInfoForm()) {
            var objDoctor = DoctorDetailsHelper.CreateDoctorObject();
            var objDoctorInfo = JSON.stringify(objDoctor);
            var jsonParam = 'doctor:' + objDoctorInfo;
            var serviceUrl = "../Doctors/SaveDoctor/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }



        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Doctor Save Successfully',
                [
                    {
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            $("#DoctorSummaryDiv").data("kendoGrid").dataSource.read(); DoctorDetailsHelper.clearDoctorForm();
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
    }


};

var DoctorDetailsHelper = {
    DepartmentNameCombo: function () {

        var objDoctor = new Object;
        objDoctor = AjaxManager.GetDataSource("../Doctors/GetAllDepartmentNameForCombo");
        $("#txtDepartmentName").kendoComboBox({
            placeholder: "Select Department Name",
            dataTextField: "DepartmentName",
            dataValueField: "DepartmentId",
            dataSource: objDoctor,
            filter: "contains",
            suggest: true,
            change: function () {

            }
        });

    },


    sectionNameDrop: function () {
        debugger;
        var commonData = [{ SectionId: 1, text: 'A' }, { SectionId: 2, text: 'B' }, { SectionId: 3, text: 'C' }, { SectionId: 4, text: "D" }];
        $("#SectionDrop").kendoDropDownList({
            optionLabel: "Select Section..",
            dataTextField: "text",
            dataValueField: "SectionId",
            dataSource: commonData
        });

    },

    clearDoctorForm: function () {
        $("#hdnDoctorId").val("0");
        $("#txtDoctorName").val("");
        $("#txtRegNo").val("");
        $("#txtExam").val("");
        $("#txtYear").data("kendoDatePicker").value("");
        $("#rdoMale").prop("checked", true);
        $("#rdoFemale").prop("checked", false);
        $("#rdoOther").prop("checked", false);
        $("#txtDepartmentName").data("kendoComboBox").value("");
        $("#SectionDrop").data("kendoDropDownList").value("");
        $('#chkIsActive').attr('checked', false);
        $("#DoctorEducationInfoDetailsDiv").data('kendoGrid').dataSource.data("");
        $("#DoctorDetailsDiv > form").kendoValidator();
        $("#DoctorDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");

    },

    CreateDoctorObject: function () {
        var objDoctor = new Object();
        objDoctor.DoctorId = $("#hdnDoctorId").val();
        objDoctor.DoctorName = $("#txtDoctorName").val();
        objDoctor.RegNo = $("#txtRegNo").val();
        objDoctor.Exam = $("#txtExam").val();
        objDoctor.Year = $("#txtYear").data("kendoDatePicker").value();
        objDoctor.Gender = $('input[name="rdoGender"]:checked').val();
        objDoctor.DepartmentId = $("#txtDepartmentName").data("kendoComboBox").value();
        objDoctor.SectionId = $("#SectionDrop").data("kendoDropDownList").value();
        objDoctor.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;

        //var doctorEduInfo = $("#DoctorEducationInfoDetailsDiv").data('kendoGrid').dataSource.data();
        //objDoctor.DoctorEducationInfo = doctorEduInfo;


        return objDoctor;
    },
    ValidateDoctorInfoForm: function () {
        var data = [];

        var validator = $("#DoctorDetailsDiv").kendoValidator().data("kendoValidator"),
            status = $(".status");
        if (validator.validate()) {
            status.text("").addClass("valid");
            return true;
        } else {
            status.text("Oops! There is invalid data in the form.").addClass("invalid");
            return false;
        }
    },
    PrintTransferReport: function () {


        var jsonParam = "";

        var url = "../Doctors/GetDoctorInfoReport";

        AjaxManager.SendJson2(url, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
         
            if (jsonData == "Success") {
                window.open('../Reports/LapsRepot.aspx', '_blank');
            }
        }
        function onFailed(error) {

            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
                        [{
                            addClass: 'btn btn-primary',
                            text: 'Ok',
                            onClick: function ($noty) {
                                $noty.close();
                            }
                        }]);
        }
    },
    populateDoctorDetails: function (objDoctor) {

        //DoctorDetailsHelper.clearDoctorForm();
        $("#hdnDoctorId").val(objDoctor.DoctorId);
        $("#txtDoctorName").val(objDoctor.DoctorName);
        $("#txtRegNo").val(objDoctor.RegNo);
        $("#txtExam").val(objDoctor.Exam);
        $("#txtYear").data("kendoDatePicker").value(objDoctor.Year);
        $("#rdoGender").val(objDoctor.Gender);
        if (objDoctor.Gender == 1) {
            $("#rdoMale").prop("checked", true);
        }
        if (objDoctor.Gender == 2) {
            $("#rdoFemale").prop("checked", true);
        }
        if (objDoctor.Gender == 3) {
            $("#rdoOther").prop("checked", true);
        }
        $("#txtDepartmentName").data("kendoComboBox").value(objDoctor.DepartmentId);
        $("#SectionDrop").data("kendoDropDownList").value(objDoctor.SectionId);
        if (objDoctor.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }

        DoctorEducationInfoDetailsHelper.GenerateDoctorEducationGrid(objDoctor.DoctorId);


    }


};