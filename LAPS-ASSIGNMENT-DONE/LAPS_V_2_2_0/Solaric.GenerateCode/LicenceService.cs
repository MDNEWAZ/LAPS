using System;
using System.Collections.Generic;
using Azolution.Entities.Sale;
namespace Solaric.GenerateCode
{
    public class LicenceService
    {

        public static string GetLicence()
        {
            return "";
        }

        /// <summary>
        /// License for Sale
        /// </summary>
        /// <param name="saleObj"></param>
        /// <param name="branchCode"></param>
        /// <param name="installmentNo"></param>
        public Sale GetLicence(Sale saleObj, string branchCode, int installmentNo)
        {
            string twoDigitInstallmentNumber = String.Format("{0:00}", installmentNo);
            var lType = 0;
            var mobileNo = saleObj.ACustomer.Phone;
            if (mobileNo == "")
            {
                mobileNo = saleObj.ACustomer.BranchSmsMobileNumber;
            }
            //var month = (Convert.ToDateTime(saleObj.WarrantyStartDate)).ToString("MM/dd/yyyy").Split('/')[0];//only month
            if (saleObj.SaleTypeId == 0) { }
            string code = branchCode.Substring(branchCode.Length - 2) + twoDigitInstallmentNumber;
            if (saleObj.SaleTypeId == 1)//Installment
            {
                lType = 0;// for code generator
                saleObj.ALicense.LType = 1;//1=Initial Lisence

            }
            else if (saleObj.SaleTypeId == 2)//Cash
            {
                lType = 2; // for code generator
                saleObj.ALicense.LType = 3;//3=Release Lisence
            }

            var aGenerator = new LicenceService();

            saleObj.ALicense.Number = aGenerator.GenerateLicence(mobileNo, code, lType);

            saleObj.ALicense.IssueDate = Convert.ToDateTime(saleObj.FirstPayDate);
            return saleObj;
        }
        public string GenerateLicence(string mobileNo, string code, int lType)
        {
            var aGenaration = new CodeGenaration.CodeGenaration();
            return aGenaration.GetCode(mobileNo, code, lType);
        }

        public string GetLicence(string mobileNo, string companyCode, int lType)
        {
            var aGenaration = new CodeGenaration.CodeGenaration();
            return aGenaration.GetCode(mobileNo, companyCode, lType);
        }

        public List<License> GetLicenseObject(List<Sale> salesList)
        {
            var licenseListObj = new List<License>();
            var licenceService = new LicenceService();

            foreach (var saleObj in salesList)
            {
                var installmentNo = saleObj.Installment;
                var branchCode = saleObj.ACustomer.BranchCode;
                Sale saleObject = licenceService.GetLicence(saleObj, branchCode, installmentNo);
                licenseListObj.Add(saleObject.ALicense);
            }
            return licenseListObj;
        }

        public List<License> GetLicenseObjectNew(List<Sale> salesList)
        {
            var licenseListObj = new List<License>();
            var licenceService = new LicenceService();


            foreach (var saleObj in salesList)
            {
                var installmentNo = saleObj.Installment;
                var branchCode = saleObj.ACustomer.BranchCode;
                Sale saleObject = licenceService.GetLicenceCode(saleObj, branchCode, installmentNo);
                licenseListObj.Add(saleObject.ALicense);
            }
            return licenseListObj;
        }

        public License GetLicenseObjectNew(Sale saleObj)
        {
            var licenseObj = new License();
            var installmentNo = saleObj.Installment;
            var branchCode = saleObj.ACustomer.BranchCode;
            Sale saleObject = GetLicenceCode(saleObj, branchCode, installmentNo);
            licenseObj = saleObject.ALicense;

            return licenseObj;
        }

        public Sale GetLicenceCode(Sale saleObj, string branchCode, int installmentNo)
        {
            var aGenerator = new LicenceService();
            var firstParam = "";
            var secondParam = "";
            var lType = 0;

            lType = 0;//GetLicenseTypeForInitial(saleObj, lType);
            saleObj.ALicense.LType = 1;

            if (saleObj.ACustomer.IsUpgraded == 1) //old Customer
            {
                string twoDigitInstallmentNumber = String.Format("{0:00}", saleObj.IIM);

                var mobileNo = saleObj.ACustomer.Phone;
                if (mobileNo == "") { mobileNo = saleObj.ACustomer.BranchSmsMobileNumber; }
                string code = branchCode.Substring(branchCode.Length - 2) + twoDigitInstallmentNumber;

                firstParam = mobileNo;
                secondParam = code;


            }
            else//new customer
            {
                if (branchCode.Length >= 4)
                {
                    string brCode = "";
                    string cusCode = "";

                        if (saleObj.ItemSlNo != "" && saleObj.ItemSlNo.Length == 8)
                        {
                            brCode = saleObj.ItemSlNo.Substring(0, 4);
                            cusCode = saleObj.ItemSlNo.Substring(saleObj.ItemSlNo.Length - 4);
                        }

                    var customerCode = cusCode == "" ? saleObj.ACustomer.CustomerCode : cusCode;
                    firstParam = customerCode.Substring(customerCode.Length - 4);

                    secondParam = brCode == "" ? branchCode.Substring(branchCode.Length - 4) : brCode;
                }
                else
                {
                    throw new Exception("Please Upgrade Branch");
                }


            }
            saleObj.ALicense.Number = aGenerator.GenerateLicence(firstParam, secondParam, lType);

            saleObj.ALicense.IssueDate = Convert.ToDateTime(saleObj.FirstPayDate);
            return saleObj;
        }

        private static int GetLicenseTypeForInitial(Sale saleObj, int lType)
        {
            if (saleObj.SaleTypeId == 1) //Installment
            {
                lType = 0; // for code generator
                saleObj.ALicense.LType = 1; //1=Initial Lisence
            }
            else if (saleObj.SaleTypeId == 2) //Cash
            {
                lType = 2; // for code generator
                saleObj.ALicense.LType = 3; //3=Release Lisence
            }
            return lType;
        }
    }
}
