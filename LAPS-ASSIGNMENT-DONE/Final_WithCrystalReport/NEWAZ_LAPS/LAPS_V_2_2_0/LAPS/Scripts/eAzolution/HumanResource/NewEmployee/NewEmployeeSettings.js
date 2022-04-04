$(document).ready(function () {

    EmployeeSummaryHelper.initForEmployeeGrid();
    //EmployeeDetailsHelper.initForGrid();
    //EmployeeSummaryHelper.clickEventForEditButton();
    EmployeeDetailsHelper.GenerateEducationGrid(0);
    EmployeeDetailsHelper.GenerateExperienceGrid(0);
    EmployeeDetailsHelper.GenerateProductGridForCheckedBox();

    $("#txtFromDate").kendoDatePicker({
        format: "dd-MMM-yyyy"
    }); $("#txtToDate").kendoDatePicker({
        format: "dd-MMM-yyyy"
    });
     
    $("#experiencePopupWindow").kendoWindow({

        title: "Employee Experience",
        resizeable: false,
        width: "40%",
        actions: ["Pin", "Refresh", "Maximize", "Close"],
        modal: true,
        visible: false
    });

    $("#btnAddNewForExperience").click(function () {
        $("#experiencePopupWindow").data("kendoWindow").open().center();

    });


    $("#btnAddExperience").click(function () {
        EmployeeDetailsHelper.AddExperienceObjectInGrid();
        EmployeeDetailsHelper.clearExperienceForm();

    });
    $("#btnCancelPopup").click(function () {
        $("#experiencePopupWindow").data("kendoWindow").close();
    });

    

  
});