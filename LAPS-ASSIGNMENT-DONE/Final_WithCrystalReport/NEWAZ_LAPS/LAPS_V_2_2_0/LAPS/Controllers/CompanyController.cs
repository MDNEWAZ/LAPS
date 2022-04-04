using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AuditTrail.Entity.DataService;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Utilities;

namespace LAPS.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/
        ICompanyRepository _companyRepository = new CompanyService();
        public ActionResult Index()
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

        public ActionResult CompanySettings()
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }
            return RedirectToAction("Logoff", "Home");
        }

        //These two line is called for work with audit trail
        IAuditHendler hendler = new IAuditHendler();
        AuditTrailDataService aService = new AuditTrailDataService();

        [HttpPost]
        public string Create(string strObjCompanyInfo)
        {
            string res;
            Users user = ((Users)(Session["CurrentUser"]));
            var CompanyId = 0;
            strObjCompanyInfo = strObjCompanyInfo.Replace("^", "&");
            try
            {
                var company = (Company)Newtonsoft.Json.JsonConvert.DeserializeObject(strObjCompanyInfo, typeof(Company));
                
               

                if (Session["FullLogoPath"] != null)
                {
                    company.FullLogoPath = Session["FullLogoPath"].ToString();
                    Session["FullLogoPath"] = null;
                }

                res = _companyRepository.SaveCompany(company);
                CompanyId = company.CompanyId;
            }
            catch (Exception exception)
            {
                res = exception.Message;
            }
            //Audittail
            var audit = hendler.GetAuditInfo(user.UserId, "Save/Update Company", CompanyId, res);
            aService.SendAudit(audit);

            return res;

        }

        [HttpGet]
        public ActionResult GetMotherCompany(int rootCompanyId=0)
        {
            
           
            var companyId = 0;

            if (Session["CurrentUser"] != null)
            {
                Users user = ((Users)(Session["CurrentUser"]));
               // companyId = rootCompanyId;
                companyId = user.CompanyId;
                IQueryable companyList = _companyRepository.GetMotherCompany(companyId);
                //return Json(companyList, JsonRequestBehavior.AllowGet);
                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                var result = new ContentResult
                {
                    Content = serializer.Serialize(companyList),
                    ContentType = "application/json"
                };

                //return Json(result, JsonRequestBehavior.AllowGet);
                return result;
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }
            
        }
        

        [HttpGet]
        public ActionResult GetMotherCompanyForEditCompanyCombo(int companyId)
        {

            if (Session["CurrentUser"] != null)
            {
                var user = ((Users)(Session["CurrentUser"]));
                var seastionCompanyId = user.CompanyId;


                IQueryable companyList = _companyRepository.GetMotherCompanyForEditCompanyCombo(companyId, seastionCompanyId);
                //return Json(companyList, JsonRequestBehavior.AllowGet);
                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                var result = new ContentResult
                {
                    Content = serializer.Serialize(companyList),
                    ContentType = "application/json"
                };

                //return Json(result, JsonRequestBehavior.AllowGet);
                return result;
            }
            else
            {
                return RedirectToAction("Logoff", "Home");
            }

        }

        public JsonResult LoadAllCompanies(int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {
            var user = ((Users)(Session["CurrentUser"]));
            int companyId = user.CompanyId;
            var companiesList = _companyRepository.GetAllCompaniesWithPaging(skip, take, page, pageSize, sort, filter, companyId);
            var results = new
                              {
                                  Items = companiesList,
                                  TotalCount = companiesList.Count>0? companiesList[0].TotalCount: 0
                              };

            return Json(results); 
        }
        public JsonResult GetCompanySummary(GridOptions options)
        {
            var user = ((Users)(Session["CurrentUser"]));
            int companyId = user.CompanyId;
            var companiesList = _companyRepository.GetCompanySummary(options, companyId);

            return Json(companiesList);
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            
            var uploadStatus = "";
            if (files != null)
            {
                Users objUser = ((Users)(Session["CurrentUser"]));
                try
                {
                    string uploadLocation = "";
               
                    var companyId = objUser.CompanyId;
                    //Logo Store Location 
                    //Virtual Directory
                    var logoPathWillbe = @"~/Images/Logo/" + companyId;
                    //Creating Directory If Not exist
                    uploadLocation = Utilities.Common.Helper.Utility.GetUploadPath(logoPathWillbe);
                    foreach (var file in files)
                    {
                        // Some browsers send file names with full path.
                        // We are only interested in the file name.
                        var fileName = Path.GetFileName(file.FileName);
                        //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                        var physicalPath = Path.Combine(Server.MapPath(logoPathWillbe), fileName);
                        //Save Logo By companyId
                        file.SaveAs(physicalPath);
                        Session["FullLogoPath"] = logoPathWillbe.Replace("~", "..") + "/" + fileName;
                        uploadStatus = "Success";
                    }
                   
                }
                catch (Exception ex)
                {
                    Session["FullLogoPath"] = null;
                    uploadStatus = ex.Message;
                }
                //Audittail
                
                var audit = hendler.GetAuditInfo(objUser.UserId, "Upload Company Logo", "Upload", uploadStatus);
                aService.SendAudit(audit);
            }

            // Return an empty string to signify success

            return Json(uploadStatus);
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            var uploadStatus = "";
            if (fileNames != null)
            {
                string uploadLocation = "";

                Users objUser = ((Users)(Session["CurrentUser"]));
                try
                {
                   
                    var companyId = objUser.CompanyId;

                    //Logo Store Location 
                    //Virtual Directory
                    var logoPathWillbe = @"~/Images/Logo/" + companyId;
                    foreach (var fullName in fileNames)
                    {
                        var fileName = Path.GetFileName(fullName);
                        var physicalPath = Path.Combine(Server.MapPath(logoPathWillbe), fileName);

                        // TODO: Verify user permissions

                        if (System.IO.File.Exists(physicalPath))
                        {
                            // The files are  actually removed from stored location
                            System.IO.File.Delete(physicalPath);
                            uploadStatus = "";
                        }
                    }

                }
                catch (Exception ex)
                {
                    uploadStatus = ex.Message;
                }


                //Audittail

                var audit = hendler.GetAuditInfo(objUser.UserId, "Remove Company Logo", "Removed", uploadStatus);
                aService.SendAudit(audit);
            }

            // Return an empty string to signify success
            return Content(uploadStatus);
        }
        public JsonResult GetRootMotherCompany()
        {
            return Json(_companyRepository.GetRootMotherCompany(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRootCompany()
        {
            return Json(_companyRepository.GetRootCompany(), JsonRequestBehavior.AllowGet);
        }
    }
}
