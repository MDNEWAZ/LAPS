using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Laps.AdminSettings.Service.Interface;
using Laps.AdminSettings.Service.Service;

namespace LAPS.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/

       public string GetCompaniesByHierecyFromSession(Users objUser)
       {
           //Users objUser = ((Users)(Session["CurrentUser"]));
           string companies = "";
           var companyList = objUser.CompanyList;
           if (companyList != null)
           {
               foreach (var company in companyList)
               {

                   companies += company.CompanyId;
                   companies += ",";
               }
               companies = companies.Remove(companies.Length - 1);
               
           }
           return companies;
       }

       public JsonResult CheckIsRootLevelAdmin(string module)
       {
           IGroupRepository _groupRepository = new GroupService();

           try
           {


               var unrecognizeModuleId = ConfigurationSettings.AppSettings[module];
               int moduleId = Convert.ToInt32(unrecognizeModuleId);

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

       public JsonResult GetCompanyHeirarchyPathData(int companyId, int branchId)
       {
           ICommonRepository _commonRepository = new CommonService();
           Users objUser = ((Users)(Session["CurrentUser"]));
           if (companyId == 0 && branchId == 0)
           {
               if (objUser.ChangedCompanyId != 0 && objUser.ChangedBranchId != 0)
               {
                   companyId = objUser.ChangedCompanyId;
                   branchId = objUser.ChangedBranchId;
               }
               else
               {
                   companyId = objUser.CompanyId;
                   branchId = objUser.BranchId;
               }
              
           }
         
           return Json(_commonRepository.GetCompanyHeirarchyPathData(companyId, branchId), JsonRequestBehavior.AllowGet);
        }
     
    }
}
