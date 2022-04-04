using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;

using Azolution.Entities.Core;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using Utilities;

namespace LAPS.Controllers
{
    public class BranchController : Controller
    {
        //
        // GET: /Branch/

        public ActionResult BranchSettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        public ActionResult BranchUpgradeSettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("../BranchUpgrade/BranchUpgradeSettings");
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }


        public ActionResult BranchTwoSettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("../BranchTwo/BranchTwoSittings"); 
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
        }

        
        public ActionResult GetBranchByCompanyId(int companyId)
        {
            IBranchRepository branchService = new BranchService();
            Users objUser = ((Users)(Session["CurrentUser"]));
           var branchList = branchService.GetBranchByCompanyId(companyId,objUser);
           var results = new
           {
               Items = branchList,
               TotalCount = branchList.Count > 0 ? branchList[0].TotalCount : 0
           };

           return Json(results, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBranchByCompanyIdForCombo(int companyId)
        {
            IBranchRepository branchService = new BranchService();
            Users objUser = ((Users)(Session["CurrentUser"]));

            var branchList = branchService.GetBranchByCompanyId(companyId, objUser);


            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllBranchForCombo()
        {
            IBranchRepository branchService = new BranchService();
            Users objUser = ((Users)(Session["CurrentUser"]));

            var branchList = branchService.GetBranchByCompanyId(0, objUser);


            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllBranchByCompanyIdForCombo(int rootCompanyId)
        {
          
            IBranchRepository branchService = new BranchService();
            var branchList = branchService.GetAllBranchByCompanyIdForCombo(rootCompanyId);


            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string SaveBranch(string strobjBranch)
        {
            var res = "";
            Users objUser = ((Users)(Session["CurrentUser"]));
            strobjBranch = strobjBranch.Replace("^", "&");
                IBranchRepository branchService = new BranchService();
                var branch =
                    (Branch)Newtonsoft.Json.JsonConvert.DeserializeObject(strobjBranch, typeof(Branch));
            try
            {
                
                res = branchService.SaveBranch(branch);

            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            //Audit Trial
            new AuditTrailDataService().SendAudit(new IAuditHendler().GetAuditInfo(objUser.UserId, "Save/Update Branch", branch.BranchId, res));
            return res;
        }

        [HttpPost]
        public string SaveBranchTwo(string sBranch)
        {
            var res = "";
            Users objUsers = ((Users)(Session["CurrentUser"]));
            sBranch = sBranch.Replace("^", "&");
            IBranchRepository repository = new BranchService();
            var branch =
                    (Branch)Newtonsoft.Json.JsonConvert.DeserializeObject(sBranch, typeof(Branch));
            try
            {
                res = repository.SaveBranchTwo(branch);
            }
            catch (Exception ex)
            {
                res = ex.Message;
                throw;
            }
            new AuditTrailDataService().SendAudit(new IAuditHendler().GetAuditInfo(objUsers.UserId, "Save/Update Branch", branch.BranchId, res));
            return res;
        }

        public ActionResult GetBranchSummary(GridOptions options, int companyID)//, int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter
        {
            IBranchRepository branchService = new BranchService();

            var branchList = branchService.GetBranchSummary(options,companyID);

            //var results = new
            //{
            //    Items = branchList,
            //    TotalCount = branchList.Count > 0 ? branchList[0].TotalCount : 0
            //};


            return Json(branchList, JsonRequestBehavior.AllowGet);
        
        }

        public ActionResult GetBranchTwoSummary(GridOptions options, int companyID)
        {
            IBranchRepository branchService = new BranchService();
            var branchList = branchService.GetBranchTwoSummary(options, companyID);
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }

       

        public JsonResult GetIsBranchCodeUsed(string branchCode)
        {
            IBranchRepository branchService = new BranchService();
            return Json(branchService.GetIsBranchCodeUsed(branchCode),JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchInfoByBranchId(int branchId)
        {
            IBranchRepository branchService = new BranchService();
            return Json(branchService.GetBranchInfoByBranchId(branchId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpgradeBranch(Branch objBranch)
        {
            IBranchRepository _branchRepository = new BranchService();
            return Json(_branchRepository.UpgradeBranch(objBranch), JsonRequestBehavior.AllowGet);
        }

    }
}
