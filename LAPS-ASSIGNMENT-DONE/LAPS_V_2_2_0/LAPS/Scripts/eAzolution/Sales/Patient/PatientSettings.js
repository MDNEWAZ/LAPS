$(document).ready(function () {

    PatientDetailsHelper.DepartmentNameCombo();
    StudentDetailsHelper.sectionNameDrop();

    $("#btnSave").click(function () {

        StudentDetailsManager.SaveStudentInformation();
    });
    $("#btnClearAll").click(function () {
        StudentDetailsHelper.clearStudentForm();
    });

    $("#btnPrint").click(function () {
        StudentDetailsHelper.PrintTransferReport();
    });

    StudentSummaryHelper.GenerateStudentGrid();
    StudentEducationInfoDetailsHelper.GenerateStudentEducationGrid(0);



});
