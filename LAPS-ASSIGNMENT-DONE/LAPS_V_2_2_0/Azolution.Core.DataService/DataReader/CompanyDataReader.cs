using System.Data;
using Azolution.Common.SqlDataService;
using Azolution.Entities.Core;

namespace Azolution.Core.DataService.DataReader
{
    internal class CompanyDataReader : EntityDataReader<Company>
    {
        public int CompanyIdColumn = -1;
        public int CompanyNameColumn = -1;
        public int CompanyCodeColumn = -1;
        public int AddressColumn = -1;
        public int PhoneColumn = -1;
        public int FaxColumn = -1;
        public int EmailColumn = -1;
        public int FullLogoPathColumn = -1;
        public int PrimaryContactColumn = -1;
        public int FiscalYearStartColumn = -1;
        public int FlagColumn = -1;
        public int MotherIdColumn = -1;
        public int IsActiveColumn = -1;
        public int CompanyTypeColumn = -1;
        public int CompanyStockColumn = -1;
        public int RootCompanyIdColumn = -1;
        public CompanyDataReader(IDataReader reader)
            : base(reader)
        {
        }
        public override Company Read()
        {
            var objCompany = new Company();
           
            objCompany.CompanyId = GetInt(CompanyIdColumn);
            objCompany.CompanyCode= GetString(CompanyCodeColumn);
            objCompany.CompanyName = GetString(CompanyNameColumn);
            objCompany.Address = GetString(AddressColumn);
            objCompany.Phone = GetString(PhoneColumn);
            objCompany.Fax = GetString(FaxColumn);
            objCompany.Email = GetString(EmailColumn);
            objCompany.FullLogoPath = GetString(FullLogoPathColumn);
            objCompany.PrimaryContact = GetString(PrimaryContactColumn);
            objCompany.FiscalYearStart = GetInt(FiscalYearStartColumn);
            objCompany.Flag = GetInt(FlagColumn);
            if (objCompany.Flag == int.MinValue)
            {
                objCompany.Flag = 0;
            }
            objCompany.MotherId = GetInt(MotherIdColumn);
            if (objCompany.MotherId == int.MinValue)
            {
                objCompany.MotherId = 0;
            }
            objCompany.IsActive = GetInt(IsActiveColumn);
            objCompany.CompanyType = GetString(CompanyTypeColumn);
            objCompany.CompanyStock = GetInt(CompanyStockColumn);
            objCompany.RootCompanyId = GetInt(RootCompanyIdColumn);
            return objCompany;
        }

        protected override void ResolveOrdinal()
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i).ToUpper())
                {
                    case "COMPANYID":
                        {
                            CompanyIdColumn = i;
                            break;
                        }
                    case "COMPANYCODE":
                        {
                            CompanyCodeColumn = i;
                            break;
                        }
                    case "COMPANYNAME":
                        {
                            CompanyNameColumn = i;
                            break;
                        }
                    case "ADDRESS":
                        {
                            AddressColumn = i;
                            break;
                        }
                    case "PHONE":
                        {
                            PhoneColumn = i;
                            break;
                        }
                    case "FAX":
                        {
                            FaxColumn = i;
                            break;
                        }
                    case "EMAIL":
                        {
                            EmailColumn = i;
                            break;
                        }
                    
                    case "FULLLOGOPATH":
                        {
                            FullLogoPathColumn = i;
                            break;
                        }
                    case "PRIMARYCONTACT":
                        {
                            PrimaryContactColumn = i;
                            break;
                        }
                    case "FISCALYEARSTART":
                        {
                            FiscalYearStartColumn = i;
                            break;
                        }
                    case "FLAG":
                        {
                            FlagColumn = i;
                            break;
                        }
                    case "MOTHERID":
                        {
                            MotherIdColumn = i;
                            break;
                        }
                    case "ISACTIVE":
                        {
                            IsActiveColumn = i;
                            break;
                        }
                    case "COMPANYTYPE":
                        {
                            CompanyTypeColumn = i;
                            break;
                        }
                    case "COMPANYSTOCK":
                        {
                            CompanyStockColumn = i;
                            break;
                        }
                    case "ROOTCOMPANYID":
                        {
                            RootCompanyIdColumn = i;
                            break;
                        }
                }
            }
        }
    }
}
