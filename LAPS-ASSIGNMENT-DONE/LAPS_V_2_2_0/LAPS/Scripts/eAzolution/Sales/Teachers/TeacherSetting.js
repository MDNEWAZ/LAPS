$(document).ready(function () {
    debugger;
    TeacherDetailsHelper.DepartmentNameCombo();
    TeacherDetailsHelper.sectionNameDrop();

    $("#btnSave").click(function () {
        debugger;
        TeacherDetailsManager.SaveTeacherInformation();
    });
    $("#btnClearAll").click(function () {
        TeacherDetailsHelper.clearTeacherForm();
    });

    $("#btnPrint").click(function () {
        TeacherDetailsHelper.PrintTransferReport();
    });

    TeacherSummaryHelper.GenerateTeacherGrid();
    TeacherEducationInfoDetailsHelper.GenerateTeacherEducationGrid(0);



});
