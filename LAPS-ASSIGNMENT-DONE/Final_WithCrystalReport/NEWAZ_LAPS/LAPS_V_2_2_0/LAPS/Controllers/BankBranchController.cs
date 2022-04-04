using System.Web.Mvc;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.Service.Interface;
using Azolution.HumanResource.Service.Service;
using Utilities;
using Utilities.Common.Json;

namespace LAPS.Controllers
{
    public class BankBranchController : Controller
    {
        //
        // GET: /BankBranch/
        private IBankBranchRepository _bankBranchRepository = new BankBranchService();
        JsonHelper _jsonHelper = new JsonHelper();
        public ActionResult BankBranchSettings()
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

        public ActionResult SaveBank(Bank bankObj)
        {
            return Json(_bankBranchRepository.SaveBank(bankObj),JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveBankBranch(BankBranch bankBranchObj)
        {
            return Json(_bankBranchRepository.SaveBankBranch(bankBranchObj),JsonRequestBehavior.AllowGet);
        }

        public string GetAllBank()
        {
            var data=_bankBranchRepository.GetAllBank();
            return _jsonHelper.GetJson(data);
        }

        public string GetBankBranchSummary(GridOptions option)
        {
            var data = _bankBranchRepository.GetBankBranchSummary(option);
            return data;
        }

    }
}
