/// <reference path="CompanySummary.js" />
/// <reference path="CompanyDetails.js" />
/// <reference path="../../Common/common.js" />

$(document).ready(function () {
    //$("#menu").kendoMenu();
    companyDetailsHelper.GenerateFiscalYearCombo();
    //companyDetailsHelper.GenerateMotherCompanyCombo();
    companyDetailsHelper.GenerateMotherCompanyCombo(0);
    companySummaryManager.GenerateCompanyGrid();
    companyManager.GeRowDataOfCompanyGrid();
    companyDetailsManager.logoUpload();

    $("#companyType").change(function () {
        
        companyDetailsHelper.changeEventOfCompanyType();
    });
    companyDetailsHelper.GenerateRootCompanyCombo();
    if (CurrentUser.CompanyId != 2) {//By default 2= Solaric(Main Company)
        
        //$('input:radio[id=MotherCompany]').hide();
        $("#liMotherCompany").hide();
    }




    });

var companyManager = {

    GeRowDataOfCompanyGrid: function () {
        $('#gridCompany table tr').live('dblclick', function () {
            
            var entityGrid = $("#gridCompany").data("kendoGrid");
            var selectedItem = entityGrid.dataItem(entityGrid.select());
            companyHelper.FillCompanyDetailsInForm(selectedItem);
        });

    },

    
};

var companyHelper = {
    FillCompanyDetailsInForm: function (objCompany) {

        companyDetailsHelper.clearCompanyForm();
        
        $('#hfCompanyId').val(objCompany.CompanyId);
        $("#txtCompanyCode").val(objCompany.CompanyCode);
        $("#txtCompanyName").val(objCompany.CompanyName);
        $("#txtAddress").val(objCompany.Address);
        $("#txtPhone").val(objCompany.Phone);
        $("#txtFax").val(objCompany.Fax);
        $("#txtEmail").val(objCompany.Email);
        
        $('input:radio[value= ' + objCompany.CompanyType + ']').attr('checked', true);
        $('input:radio[value= ' + objCompany.CompanyStock + ']').attr('checked', true);
        
        $("#txtPrimaryContact").val(objCompany.PrimaryContact);
        var cmbMotherCompany = $("#cmbMotherCompany").data("kendoComboBox");
        cmbMotherCompany.value("");
      
        var cmbRootCompany = $("#cmbRootCompany").data("kendoComboBox");
        cmbRootCompany.value(objCompany.RootCompanyId);
        //var list = 0;
        //for (var i = 0; i < cmbMotherCompany.dataSource._data.length;i++) {
        //    if(cmbMotherCompany.dataSource._data[i].CompanyId == objCompany.CompanyId) {
        //        list = i;
        //        break;
        //    }
        //}
        //if(i!= 0) {
        //    var itemToRemove = cmbMotherCompany.dataSource.at(list);
        //    cmbMotherCompany.dataSource.remove(itemToRemove);
        //}

        companyDetailsHelper.GetMotherCompanyForEditCompanyCombo(objCompany.CompanyId);


        if (objCompany.MotherId != 0) {
            cmbMotherCompany.value(objCompany.MotherId);
        }
       
        if(objCompany.IsActive==1) {
            $("#chkIsActive").prop('checked', 'checked');
        }else {
            $("#chkIsActive").removeProp('checked', 'checked');
        }
        
        var ddlFiscalYearStart = $("#ddlFiscalYearStart").data("kendoDropDownList");
        ddlFiscalYearStart.value(objCompany.FiscalYearStart);
        $("#hfFullLogoPath").val(objCompany.FullLogoPath);
    }
    

};


