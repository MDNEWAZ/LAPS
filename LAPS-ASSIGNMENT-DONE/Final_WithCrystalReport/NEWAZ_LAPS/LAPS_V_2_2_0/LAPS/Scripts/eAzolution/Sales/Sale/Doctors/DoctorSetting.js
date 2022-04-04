$(document).ready(function () {

    DoctorDetailsHelper.DepartmentNameCombo();
    DoctorDetailsHelper.sectionNameDrop();

    $("#btnSave").click(function () {
    
        DoctorDetailsManager.SaveDoctorInformation();
    });
    $("#btnClearAll").click(function () {
        DoctorDetailsHelper.clearDoctorForm();
    });

    $("#btnPrint").click(function () {
       DoctorDetailsHelper.PrintTransferReport();
   });

    DoctorSummaryHelper.GenerateDoctorGrid();
   // DoctorEducationInfoDetailsHelper.GenerateDoctorEducationGrid(0);

    //"MM/dd/yyyy HH:mm"
    $("#txtYear").kendoDatePicker({
        value: new Date(),
        dateInput: true,
        //formate: "dd yyyy"

        format: "dd-MMM-yyyy"
    });

});
