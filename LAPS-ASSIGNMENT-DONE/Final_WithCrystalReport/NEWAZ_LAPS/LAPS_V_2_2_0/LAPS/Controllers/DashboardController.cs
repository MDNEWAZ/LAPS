using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Dashboard.Service.Interface;
using Azolution.Dashboard.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Azolution.HumanResource.Service.Interface;
using Laps.Collection.CollectionService.Interface;
using Laps.Collection.CollectionService.Service;
using Laps.Sale.SaleService.Service;
using Laps.Stock.Service.Interface;
using Laps.Stock.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class DashboardController : Controller
    {
       ICollectionRepository _collectionRepository = new CollectionService();
        private IDashboardRepository _dashBoardRepository = new DashBoardService();
        public ActionResult Dashboard()
        {
            if (Session["CurrentUser"] != null)
            {
               return View();
            }
            return RedirectToAction("Logoff", "Home");
        }

        public ActionResult WaitingForRelease()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("../WaitingForRelease/WaitingForRelease");
            }
            return RedirectToAction("Logoff", "Home");
        }


        public ActionResult CodeSearch()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("../CodeSearch/CodeSearch");
            }
            return RedirectToAction("Logoff", "Home");
        }


        IGroupRepository _groupRepository = new GroupService();
        public string GetServerTime()
        {
            return DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        public JsonResult GetAllPendingCollectionsForDashBoard(int companyId, int branchId, DateTime? fromDate, DateTime? toDate)
        {
            var user = (Users)(Session["CurrentUser"]);
           
            companyId = companyId == 0 ? user.ChangedCompanyId : companyId;
            branchId = branchId == 0 ? user.ChangedBranchId : branchId;

            var dashboardModule = ConfigurationSettings.AppSettings["dashboardModuleId"];
            int moduleId = Convert.ToInt32(dashboardModule);

            Users objUser = ((Users)(Session["CurrentUser"]));
            var res = _groupRepository.GetAccessPermisionForCurrentUser(moduleId, objUser.UserId);

            var isAdministrator = false;

            CommonController commonController = new CommonController();
            string companies = "";
            foreach (var groupPermission in res)
            {
                if (groupPermission.ReferenceID == 22 || groupPermission.ReferenceID == 4 || groupPermission.ReferenceID == 24 || groupPermission.ReferenceID == 27)
                {
                    isAdministrator = true;

                    companies = commonController.GetCompaniesByHierecyFromSession(objUser);
                    break;
                }
            }
            return Json(_dashBoardRepository.GetAllPendingCollectionsForDashBoard(objUser, companyId, branchId, isAdministrator, fromDate, toDate, companies), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetAllPendingCollections(int companyId, int branchId, DateTime? fromDate, DateTime? toDate)
        //{
        //    //var user = (Users)(Session["CurrentUser"]);
        //    //var userId = user.UserId;

        //    var dashboardModule = ConfigurationSettings.AppSettings["dashboardModuleId"];
        //    int moduleId = Convert.ToInt32(dashboardModule);

        //    Users objUser = ((Users)(Session["CurrentUser"]));

        //    var res = _groupRepository.GetAccessPermisionForCurrentUser(moduleId, objUser.UserId);

        //    var isAdministrator = false;



        //    CommonController commonController = new CommonController();
        //    string companies = "";
        //    foreach (var groupPermission in res)
        //    {
        //        if (groupPermission.ReferenceID == 22 || groupPermission.ReferenceID == 4 || groupPermission.ReferenceID == 24 || groupPermission.ReferenceID == 27)
        //        {
        //            //22 = Comapny MD, 2 = Supper User, 24= region/division/zone, 27 = HO Accounts
        //            isAdministrator = true;

        //            //if (groupPermission.ReferenceID == 4)
        //            //{
        //            //    companyId = objUser.CompanyId;
        //            //}

        //            companies = commonController.GetCompaniesByHierecyFromSession(objUser);
        //            break;
        //        }
        //    }
        //    return Json(_collectionRepository.GetAllPendingCollections(objUser, companyId, branchId, isAdministrator, fromDate, toDate, companies), JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetMonthWiseCollectionData(int companyId, int branchId, DateTime? fromDate, DateTime? toDate)
        {
            var user = (Users)(Session["CurrentUser"]);
            var userId = user.UserId;
            //var userLevel = CheckUserLevel();
            //if (userLevel == "CompanyUser" && companyId == 0)
            //{
            //    companyId = user.CompanyId;
            //}

            CommonController commonController = new CommonController();
            string companies = "";
            companies = commonController.GetCompaniesByHierecyFromSession(user);


            return Json(_dashBoardRepository.GetMonthWiseCollectionData(userId, companyId, branchId, fromDate, toDate, companies), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMonthWiseSalesData(int companyId, int branchId, DateTime? fromDate, DateTime? toDate)
        {
            var user = (Users)(Session["CurrentUser"]);
            var userId = user.UserId;
            //var userLevel = CheckUserLevel();
            //if (userLevel == "CompanyUser" && companyId == 0)
            //{
            //    companyId = user.CompanyId;
            //}
            CommonController commonController = new CommonController();
            string companies = "";
            companies = commonController.GetCompaniesByHierecyFromSession(user);

            return Json(_dashBoardRepository.GetMonthWiseSalesData(userId, companyId, branchId, fromDate, toDate, companies), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTenRedCustomer(GridOptions options, string invoice, int companyId, int branchId)
        {
            //var user = (Users)(Session["CurrentUser"]);
            //var userId = user.UserId;

            //IGroupRepository _groupRepository = new GroupService();
            try
            {


                var salesModule = ConfigurationSettings.AppSettings["salesModuleId"];
                int moduleId = Convert.ToInt32(salesModule);

                Users objUser = ((Users)(Session["CurrentUser"]));

                var res = _groupRepository.GetAccessPermisionForCurrentUser(moduleId, objUser.UserId);

                var isAdministrator = false;

                CommonController commonController = new CommonController();
                string companies = "";
                foreach (var groupPermission in res)
                {
                    if (groupPermission.ReferenceID == 22 || groupPermission.ReferenceID == 4 || groupPermission.ReferenceID == 24 || groupPermission.ReferenceID == 27)
                    {
                        //22 = Comapny MD, 2 = Supper User, 24= region/division/zone, 27 = HO Accounts
                        isAdministrator = true;

                        //if (groupPermission.ReferenceID == 4)
                        //{
                        //    companyId = objUser.CompanyId;
                        //}

                        companies = commonController.GetCompaniesByHierecyFromSession(objUser);
                        break;
                    }
                }

                var data = _dashBoardRepository.GetCusomerRatingInfoData(options, invoice, isAdministrator, objUser, companyId, branchId, companies);
                return Json(data, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public JsonResult GetDueCollectionCustomerGridData(GridOptions options, int companyId, int branchId)
        {
            IGroupRepository _groupRepository = new GroupService();

            try
            {
                var salesModule = ConfigurationSettings.AppSettings["salesModuleId"];
                int moduleId = Convert.ToInt32(salesModule);

                Users objUser = ((Users)(Session["CurrentUser"]));

                var res = _groupRepository.GetAccessPermisionForCurrentUser(moduleId, objUser.UserId);

                var isAdministrator = false;

                CommonController commonController = new CommonController();
                string companies = "";
                foreach (var groupPermission in res)
                {
                    if (groupPermission.ReferenceID == 22 || groupPermission.ReferenceID == 4 || groupPermission.ReferenceID == 24 || groupPermission.ReferenceID == 27)
                    {
                        //22 = Comapny MD, 2 = Supper User, 24= region/division/zone, 27 = HO Accounts
                        isAdministrator = true;

                        companies = commonController.GetCompaniesByHierecyFromSession(objUser);
                        break;
                    }
                }

                return Json(_dashBoardRepository.GetDueCollectionCustomerGridData(options, isAdministrator, objUser, companyId, branchId, companies), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public JsonResult CheckIsRootLevelAdmin()
        {
            IGroupRepository _groupRepository = new GroupService();

            try
            {


                var dashboardModule = ConfigurationSettings.AppSettings["dashboardModuleId"];
                int moduleId = Convert.ToInt32(dashboardModule);

                Users objUser = ((Users)(Session["CurrentUser"]));

                var res = _groupRepository.GetAccessPermisionForCurrentUser(moduleId, objUser.UserId);

                //var isAdministrator = false;
                var userLevel = 0;


                foreach (var groupPermission in res)
                {
                    userLevel = groupPermission.ReferenceID;
                    //if (groupPermission.ReferenceID == 22 || groupPermission.ReferenceID == 4)
                    //{
                    //    isAdministrator = true;
                    //    break;
                    //}
                    
                    
                }
                return Json(userLevel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public string CheckUserLevel()
        //{
        //    var salesModule = ConfigurationSettings.AppSettings["salesModuleId"];
        //    int moduleId = Convert.ToInt32(salesModule);

        //    Users objUser = ((Users)(Session["CurrentUser"]));

        //    var res = _groupRepository.GetAccessPermisionForCurrentUser(moduleId, objUser.UserId);

        //    var userLevel = "";



        //    foreach (var groupPermission in res)
        //    {
        //        if (groupPermission.ReferenceID == 22)
        //        {
        //            userLevel = "SupperUser";
        //            break;
        //        }
        //        else if (groupPermission.ReferenceID == 4)
        //        {
        //            userLevel = "CompanyUser";
        //            break;
        //        }

        //    }
        //    return userLevel;
        //}
        public JsonResult GetCustomerForReleaseLisenceGridData(GridOptions options, int companyId, int branchId)
        {
            Users objUser = ((Users)(Session["CurrentUser"]));
            //var userLevel = CheckUserLevel();

            //if (userLevel == "CompanyUser" && companyId == 0)
            //{
            //    companyId = objUser.CompanyId;
            //}
            //string companyId = "";
            string companies = "";
            if (companyId == 0)
            {

                var companyList = objUser.CompanyList;
                foreach (var company in companyList)
                {

                    companies += company.CompanyId;
                    companies += ",";
                }
                companies = companies.Remove(companies.Length - 1);

                //companyId = objUser.RootCompanyId;
            }
            //if (branchId == 0)
            //{
            //    branchId = objUser.ChangedBranchId;
            //}
            return Json(_collectionRepository.GetCustomerForReleaseLisenceGridData(options, companies, branchId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerWithCodeSummary(GridOptions options, string customerCode, string smsMobileNumber)
        {
            //Please condiser this option was in Dashboard, So we make this method on Dashboard controller and leter it move to separate UI on emergency basis

            return Json(_collectionRepository.GetCustomerWithCodeSummary(options, customerCode, smsMobileNumber), JsonRequestBehavior.AllowGet);

        }
        public JsonResult ChangeSessionOnBranchChange(int companyId, int branchId)
        {
            string res = "";
            try
            {
                Users objUser = ((Users)(Session["CurrentUser"]));
                UsersController users = new UsersController();
                var branchData = users.GetBranchCodeByCompanyIdAndBranchId(companyId, branchId);
                objUser.ChangedCompanyId = companyId;
                objUser.ChangedBranchId = branchId;
                objUser.ChangedBranchCode = branchData.BranchCode;
                objUser.ChangedCompanyStock = branchData.CompanyStock;
                objUser.ChangedCompanyType = branchData.CompanyType;
                objUser.ChangedRootCompanyId=branchData.RootCompanyId;
               
                Session["CurrentUser"] = objUser;
                res = Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSaleStockData(int companyId)
        {
            IStockRepository _stockRepository = new StockService();
            Users objUser = ((Users)(Session["CurrentUser"]));
            return Json(_stockRepository.GetSaleStockData(objUser,companyId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReplacementStockData(int companyId)
        {
            IStockRepository _stockRepository = new StockService();
            Users objUser = ((Users)(Session["CurrentUser"]));
            return Json(_stockRepository.GetReplacementStockData(objUser,companyId), JsonRequestBehavior.AllowGet);
        }

       

    }
}

