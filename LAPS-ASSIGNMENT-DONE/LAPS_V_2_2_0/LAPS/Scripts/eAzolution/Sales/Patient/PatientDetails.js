var PatientName = "";
var PatientDetailsManager = {


    SavePatientInformation: function () {

        if (StudentDetailsHelper.ValidateStudentInfoForm()) {
            var objStudent = StudentDetailsHelper.CreateStudentObject();
            var objStudentInfo = JSON.stringify(objStudent);
            var jsonParam = 'student:' + objStudentInfo;
            var serviceUrl = "../Students/SaveStudent/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }



        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Student Save Successfully',
                [
                    {
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                            // $("#gridBranchUpgradeSummary").data("kendoGrid").dataSource.read();
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

var PatientDetailsHelper = {
    DepartmentNameCombo: function () {

        var objPatient = new Object;
        objPatient = AjaxManager.GetDataSource("../Students/GetAllDepartmentNameForCombo/");
        $("#txtDepartmentName").kendoComboBox({
            placeholder: "Select Department Name",
            dataTextField: "DepartmentName",
            dataValueField: "DepartmentId",
            dataSource: objStudent,
            filter: "contains",
            suggest: true
        });

    },

    clearPatientForm: function () {
        $("#hdnPatientId").val("0");
        $("#txtPatientName").val("");
        $("#txtPatientRegNo").val("");
        $("#rdoMale").prop("checked", false);
        $("#rdoFemale").prop("checked", false);
        $("#rdoOther").prop("checked", false);
        $("#txtPatientAddress").val("");
        $("#txtNationalId").val("");
        $("#txtDepartmentName").data("kendoComboBox").value("");        
        $("#txtDoctorName").data("kendoComboBox").value("");
        $("#txtAppointmentDate").val("");
        $("#PatientDetailsDiv > form").kendoValidator();
        $("#PatientDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");

    },

    CreatePatientObject: function () {
        var objPatient = new Object();
        objPatient.PatientId = $("#hdnPatientId").val();
        objPatient.PatientName = $("#txtPatientName").val();
        objPatient.PatientRegNo = $("#txtPatientRegNo").val();        
        objPatient.Gender = $('input[name="rdoGender"]:checked').val();
        objPatient.PatientAddress = $("#txtPatientAddress").val();
        objPatient.NationalId = $("#txtNationalId").val();
        objPatient.DepartmentId = $("#txtDepartmentName").data("kendoComboBox").value();
        objPatient.DoctorId = $("#txtDoctorName").data("kendoComboBox").value();
        objPatient.AppointmentDate = $("#txtAppointmentDate").val();

        //var studentEduInfo = $("#StudentEducationInfoDetailsDiv").data('kendoGrid').dataSource.data();
        //objPatient.StudentEducationInfo = studentEduInfo;


        return objPatient;
    },
    ValidatePatientInfoForm: function () {
        var data = [];

        var validator = $("#PatientDetailsDiv").kendoValidator().data("kendoValidator"),
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

        debugger;
        var jsonParam = "";

        var url = "../Students/GetStudentInfoReport";

        AjaxManager.SendJson2(url, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            debugger;
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
    populatePatientDetails: function (objPatient) {

        PatientDetailsHelper.clearPatientForm();
        $("#hdnPatientId").val(objPatient.PatientIdId);
        $("#txtPatientName").val(objPatient.PatientIdName);
        $("#txtPatientRegNo").val(objPatient.PatientIdRegNo);
        $("#rdoGender").val(objPatient.Gender);
        if (objPatient.Gender == 1) {
            $("#rdoMale").prop("checked", true);
        }
        if (objPatient.Gender == 2) {
            $("#rdoFemale").prop("checked", true);
        }
        if (objPatient.Gender == 3) {
            $("#rdoOther").prop("checked", true);
        }
        $("#txtPatientId").val(objPatient.PatientId);
        $("#txtPatientNationalId").val(objPatient.PatientNationalId);
        $("#txtDepartmentName").data("kendoComboBox").value(objPatient.DepartmentId);
        StudentEducationInfoDetailsHelper.GenerateStudentEducationGrid(objStudent.StudentId);


    }


};