using Azolution.Entities.Sale;
using Laps.Mobile.Service.Interface;
using Laps.Mobile.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace LAPS.Controllers
{
    public class MobileController : Controller
    {
        //
        // GET: /Mobile/
        private IMobileRepository mobileRepo = new MobileService();
        public IMobileRepository featureRepo = new MobileService();
        public ActionResult Index()
        {
            if (Session["CurrentUser"] != null)
            {
                return View("~/Views/Sale/Mobile/MobileSettings.cshtml");
            }
            return RedirectToAction("Logoff", "Home");          
        }

        public ActionResult PopulateColorCombo()
        {
            try
            {
                var data = mobileRepo.PopulateColorCombo();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult PopulateColorCombo2()
        {
            try
            {
                var data = mobileRepo.PopulateColorCombo2();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Delete(int MobileId)
        {
            
                var data = mobileRepo.Delete(MobileId);
                return Json(data, JsonRequestBehavior.AllowGet);
           
        }

       
        public ActionResult PopulateBrandCombo()
        {
            try
            {
                var datas = mobileRepo.PopulateBrandCombo();
                return Json(datas, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public ActionResult PopulateBrandCombo2()
        {
            try
            {
                var datas = mobileRepo.PopulateBrandCombo2();
                return Json(datas, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult SaveMobileInfo(MobileInfo mobile)
        {
            

            return Json(mobileRepo.SaveMobileInfo(mobile), JsonRequestBehavior.AllowGet);

        }

        //public ActionResult clearMobileForm(MobileInfo mobile)
        //{


        //    return Json(mobileRepo.clearMobileForm(mobile), JsonRequestBehavior.AllowGet);
        //}

        public ActionResult MobileInfoGrid(GridOptions options)
        {
            return Json(mobileRepo.MobileInfoGrid(options), JsonRequestBehavior.AllowGet);
        }


        //ChkPopup
        public ActionResult SaveFeature(MobileFeature feature)
        {


            return Json(featureRepo.SaveFeature(feature), JsonRequestBehavior.AllowGet);

        }
        public ActionResult MobileFeatureGrid(GridOptions options)
        {
            return Json(featureRepo.MobileFeatureGrid(options), JsonRequestBehavior.AllowGet);
        }

	}
}