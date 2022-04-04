$(document).ready(function () {

    PersonDetailHelper.dropReligion();
    PersonDetailHelper.maritalStatusDrop();
    
   // PersonSummaryHelper.GeneratePersonGrid();

    $("#btnSave").click(function () {

        PersonDetailManager.SavePersonInformation();
    });

    $("#txtDateOfBirth").kendoDatePicker({
        format: 'dd-MMM-yyyy'
    });

    $("#btnClearAll").click(function () {
        PersonDetailHelper.clearPersonalForm();
    });

    $("#btnDelete").click(function () {

        PersonDetailManager.DeletePersonalInfo();
    });

    PersonalDetailsSummaryHelper.GeneratePersonalDetailsGrid(0);

    });


