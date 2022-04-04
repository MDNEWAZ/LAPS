var StudentName = "";
var StudentDetailsManager = {
   
  
     SaveStudentInformation: function() {

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
                        onClick: function($noty) {
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
                        onClick: function($noty) {
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

var StudentDetailsHelper = {
    DepartmentNameCombo: function() {
        
        var objStudent = new Object;
        objStudent = AjaxManager.GetDataSource("../Students/GetAllDepartmentNameForCombo");
        $("#txtDepartmentName").kendoComboBox({
            placeholder: "Select Department Name",
            dataTextField: "DepartmentName",
            dataValueField: "DepartmentId",
            dataSource: objStudent,
            filter: "contains",
            suggest: true,
            change:function() {
                var val = this.value();
                DepartmentNameCombo(val);
            }
        });

    },


    sectionNameDrop: function() {
        var commonData = [{ SectionId: 1, text: 'A' }, { SectionId: 2, text: 'B' }, { SectionId: 3, text: 'C' }, { SectionId: 4, text: "D" }];
            $("#SectionDrop").kendoDropDownList({
                optionLabel: "Select Section..",
                dataTextField: "text",
                dataValueField: "SectionId",
                dataSource: commonData
            });
        
    },

    clearStudentForm: function () {
        $("#hdnStudentId").val("0");
        $("#txtStudentName").val("");
        $("#txtRegNo").val("");
        $("#rdoMale").prop("checked", true);
        $("#rdoFemale").prop("checked", false);
        $("#rdoOther").prop("checked", false);
        $("#txtDepartmentName").data("kendoComboBox").value("");
        $("#SectionDrop").data("kendoDropDownList").value("");
        $('#chkIsActive').attr('checked', false);
        $("#StudentEducationInfoDetailsDiv").data('kendoGrid').dataSource.data("");
        $("#StudentDetailsDiv > form").kendoValidator();
        $("#StudentDetailsDiv").find("span.k-tooltip-validation").hide();
        var status = $(".status");

        status.text("").removeClass("invalid");

    },
                                        
    CreateStudentObject: function() {
        var objStudent = new Object();
        objStudent.StudentId = $("#hdnStudentId").val();
        objStudent.StudentName = $("#txtStudentName").val();
        objStudent.RegNo = $("#txtRegNo").val();
        objStudent.Gender = $('input[name="rdoGender"]:checked').val();
        objStudent.DepartmentId = $("#txtDepartmentName").data("kendoComboBox").value();
        objStudent.SectionId = $("#SectionDrop").data("kendoDropDownList").value();
        objStudent.IsActive = $("#chkIsActive").is(':checked') == true ? 1 : 0;
        
        var studentEduInfo = $("#StudentEducationInfoDetailsDiv").data('kendoGrid').dataSource.data();
        objStudent.StudentEducationInfo = studentEduInfo;


        return objStudent;
    },
    ValidateStudentInfoForm: function () {
        var data = [];

        var validator = $("#StudentDetailsDiv").kendoValidator().data("kendoValidator"),
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
    populateStudentDetails: function (objStudent) {
       
        StudentDetailsHelper.clearStudentForm();
        $("#hdnStudentId").val(objStudent.StudentId);
        $("#txtStudentName").val(objStudent.StudentName);
        $("#txtRegNo").val(objStudent.RegNo);
        $("#rdoGender").val(objStudent.Gender);
        if (objStudent.Gender == 1) {
            $("#rdoMale").prop("checked", true);
        }
        if (objStudent.Gender == 2) {
            $("#rdoFemale").prop("checked", true);
        }
        if (objStudent.Gender == 3) {
            $("#rdoOther").prop("checked", true);
        }
        $("#txtDepartmentName").data("kendoComboBox").value(objStudent.DepartmentId);
        $("#SectionDrop").data("kendoDropDownList").value(objStudent.SectionId);
        if (objStudent.IsActive == 1) {
            $("#chkIsActive").prop('checked', 'checked');
        } else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }
   
        StudentEducationInfoDetailsHelper.GenerateStudentEducationGrid(objStudent.StudentId);
        
        
    }


};