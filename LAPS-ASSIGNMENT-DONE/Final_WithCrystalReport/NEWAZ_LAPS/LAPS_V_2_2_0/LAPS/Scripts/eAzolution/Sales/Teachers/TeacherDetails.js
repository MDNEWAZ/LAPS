var TeacherName = "";
var TeacherDetailsManager = {


    SaveTeacherInformation: function () {
        debugger;
        if (TeacherDetailsHelper.ValidateTeacherInfoForm()) {
            var objTeacher = TeacherDetailsHelper.CreateTeacherObject();
            var objTeacherInfo = JSON.stringify(objTeacher);
            var jsonParam = 'Teacher:' + objTeacherInfo;
            var serviceUrl = "../Teachers/SaveTeacher/";
            AjaxManager.SendJson2(serviceUrl, jsonParam, onSuccess, onFailed);
        }



        function onSuccess(jsonData) {
            if (jsonData == "Success") {

                AjaxManager.MsgBox('success', 'center', 'Success:', 'Teacher Save Successfully',
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

var TeacherDetailsHelper = {
    DepartmentNameCombo: function () {
        debugger;
        var objStudent = new Object;
        objTeacher = AjaxManager.GetDataSource("../Teachers/GetAllDepartmentNameForCombo");
        $("#txtDepartmentName").kendoComboBox({
            placeholder: "Select Department Name",
            dataTextField: "DepartmentName",
            dataValueField: "DepartmentId",
            dataSource: objTeacher,
            filter: "contains",
            suggest: true,
            change: function () {
                var val = this.value();
                DepartmentNameCombo(val);
            }
        });

    },


    sectionNameDrop: function () {
        var commonData = [{ SectionId: 1, text: 'A' }, { SectionId: 2, text: 'B' }, { SectionId: 3, text: 'C' }, { SectionId: 4, text: "D" }];
        $("#SectionDrop").kendoDropDownList({
            optionLabel: "Select Section..",
            dataTextField: "text",
            dataValueField: "SectionId",
            dataSource: commonData
        });

    },

    clearTeacherForm: function () {
        $("#hdnTeacherId").val("0");
        $("#txtTeacherName").val("");
        $("#txtRegNo").val("");
        $("#rdoMale").prop("checked", true);
        $("#rdoFemale").prop("checked", false);
        $("#rdoOther").prop("checked", false);
        $("#txtDepartmentName").data("kendoComboBox").value("");
        $("#SectionDrop").data("kendoDropDownList").value("");
        $('#chkIsActive').attr('checked', false);
        $("#TeacherEducationInfoDetailsDiv").data('kendoGrid').dataSource.data("");
        $("#TeacherDetailsDiv > form").kendoValidator();
        $("#TeacherDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");

    },

    CreateTeacherObject: function () {
        var objTeacher = new Object();
        objTeacher.TeacherId = $("#hdnTeacherId").val();
        objTeacher.TeacherName = $("#txtTeacherName").val();
        objTeacher.RegNo = $("#txtRegNo").val();
        objTeacher.Gender = $('input[name="rdoGender"]:checked').val();
        objTeacher.DepartmentId = $("#txtDepartmentName").data("kendoComboBox").value();
        objTeacher.SectionId = $("#SectionDrop").data("kendoDropDownList").value();
        objTeacher.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;

        var TeacherEduInfo = $("#TeacherEducationInfoDetailsDiv").data('kendoGrid').dataSource.data();
        objTeacher.TeacherEducationInfo = teacherEduInfo;


        return objTeacher;
    },
    ValidateTeacherInfoForm: function () {
        var data = [];

        var validator = $("#TeacherDetailsDiv").kendoValidator().data("kendoValidator"),
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

        var url = "../Teachers/GetTeacherInfoReport";

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
    populateTeacherDetails: function (objTeacher) {

        TeacherDetailsHelper.clearTeacherForm();
        $("#hdnTeacherId").val(objTeacher.TeacherId);
        $("#txtTeacherName").val(objTeacher.TeacherName);
        $("#txtRegNo").val(objStudent.RegNo);
        $("#rdoGender").val(objTeacher.Gender);
        if (objTeacher.Gender == 1) {
            $("#rdoMale").prop("checked", true);
        }
        if (objTeacher.Gender == 2) {
            $("#rdoFemale").prop("checked", true);
        }
        if (objTeacher.Gender == 3) {
            $("#rdoOther").prop("checked", true);
        }
        $("#txtDepartmentName").data("kendoComboBox").value(objTeacher.DepartmentId);
        $("#SectionDrop").data("kendoDropDownList").value(objTeacher.SectionId);
        if (objTeacher.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }

        TeacherEducationInfoDetailsHelper.GenerateTeacherEducationGrid(objTeacher.TeacherId);


    }


};