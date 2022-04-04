using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;

using Azolution.BulkUploadService.Interface;
using Azolution.BulkUploadService.Service;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.HumanResource;
using Utilities;

namespace LAPS.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult UserSettings()
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

        IAuditHendler hendler = new IAuditHendler();
        AuditTrailDataService aService = new AuditTrailDataService();
        IUsersRepository _usersRepository = new UsersService();

        public string UpdateTheme(string themeName)
        {

            if (Session["CurrentUser"] != null)
            {
                Users usr = (Users)Session["CurrentUser"];

                //var user = _usersRepository.GetUserByEmployeeId(usr.EmployeeId);

                var user = _usersRepository.GetUserByEmployeeId(usr.UserId);




                user.Theme = themeName;
                var res = _usersRepository.UpdateTheme(user);
                Session["themeName"] = null;
                Session["themeName"] = themeName;

                //Audittail
                var audit = hendler.GetAuditInfo(usr.UserId, "Update Theme", "Update", res);
                aService.SendAudit(audit);

                return res;

            }

            return "error";
        }

        [HttpPost]

        public string SaveUser(string strobjUserInfo)
        {
            string res = "";
            Users usr = (Users)Session["CurrentUser"];
            var userId = 0;
            if (usr != null)
            {
                strobjUserInfo = strobjUserInfo.Replace("^", "&");
               
                var users = (Users)Newtonsoft.Json.JsonConvert.DeserializeObject(strobjUserInfo, typeof(Users));

                users.UserName = users.UserName.Replace("'", "''");
                res = _usersRepository.SaveUser(users);
                userId = users.UserId;

                //Audittail
                var audit = hendler.GetAuditInfo(usr.UserId, "Save/Update User", userId, res);
                aService.SendAudit(audit);

            }
             
            return res;
        }

        public ActionResult GetUserSummary(int companyID, int skip, int take, int page, int pageSize, List<AzFilter.GridSort> sort, AzFilter.GridFilters filter)
        {

            var userList = _usersRepository.GetUserSummary(companyID, skip, take, page, pageSize, sort, filter);

            //var results = new
            //{
            //    Items = userList,
            //    TotalCount = userList.Count > 0 ? userList[0].TotalCount : 0
            //};

            return Json(userList);
        }

        public ActionResult GetGroupMemberByUserId(int userId)
        {

            var res = _usersRepository.GetGroupMemberByUserId(userId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public string ResetPassword(int companyId, int userId)
        {

            var res = _usersRepository.ResetPassword(companyId, userId);
            //AuditRecord
            Users usr = (Users)Session["CurrentUser"];
            var audit = hendler.GetAuditInfo(usr.UserId, "Reset Password", "Reset Password", res);
            aService.SendAudit(audit);
            return res;
        }

        public ActionResult UploadUserExcel(IEnumerable<HttpPostedFileBase> files)
        {

            var uploadStatus = "";
            Users objUser = ((Users)(Session["CurrentUser"]));
            if (files != null)
            {
                try
                {
                    string uploadLocation = "";

                    var companyId = objUser.CompanyId;
                    //Logo Store Location 
                    //Virtual Directory
                    var logoPathWillbe = @"~/UploadUser/Users/" + companyId;
                    //Creating Directory If Not exist
                    uploadLocation = Utilities.Common.Helper.Utility.GetUploadPath(logoPathWillbe);
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var physicalPath = Path.Combine(Server.MapPath(logoPathWillbe), DateTime.Now.Ticks + fileName);
                        file.SaveAs(physicalPath);

                        Session["UploadUser"] = physicalPath;
                        uploadStatus = logoPathWillbe.Replace("~", "..") + "/" + fileName;
                    }


                }
                catch (Exception ex)
                {
                    Session["UploadUser"] = null;
                    uploadStatus = ex.Message;
                }
                var audit = hendler.GetAuditInfo(objUser.UserId, "Upload User Excel", "Upload", uploadStatus);
                aService.SendAudit(audit);
            }

            // Return an empty string to signify success
            return Json(uploadStatus);
        }

        public ActionResult RemoveUserExcel(string[] fileNames)
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
                    var logoPathWillbe = @"~/UploadUser/Users/" + companyId;
                    foreach (var fullName in fileNames)
                    {
                        var fileName = Path.GetFileName(fullName);
                        var physicalPath = Path.Combine(Server.MapPath(logoPathWillbe), fileName);

                        // TODO: Verify user permissions

                        if (System.IO.File.Exists(physicalPath))
                        {
                            // The files are  actually removed from stored location
                            System.IO.File.Delete(physicalPath);
                            uploadStatus = logoPathWillbe.Replace("~", "..") + "/" + fileName;
                        }
                    }

                }
                catch (Exception ex)
                {
                    uploadStatus = ex.Message;
                }
                var audit = hendler.GetAuditInfo(objUser.UserId, "Remove User Excel", "Remove", uploadStatus);
                aService.SendAudit(audit);


            }

            // Return an empty string to signify success
            return Content(uploadStatus);
        }

        public ActionResult ImportUplodedData()
        {
            var res = "";
            Users objUser = ((Users)(Session["CurrentUser"]));
            try
            {

                var importFilePath = Session["UploadUser"].ToString();

                IUserUploadRepository userUploadRepository = new BulkUserUploadService();
                res = userUploadRepository.ImportUserUplodedData(importFilePath, objUser.UserId);

            }
            catch (Exception exception)
            {
                res = exception.Message;
            }
            var audit = hendler.GetAuditInfo(objUser.UserId, "Import Uploded Data", "Insert", res);
            aService.SendAudit(audit);

            return Json(res, JsonRequestBehavior.AllowGet);
        }



        public CompanyBranchInfo GetBranchCodeByCompanyIdAndBranchId(int companyId, int branchId)
        {
           return _usersRepository.GetBranchCodeByCompanyIdAndBranchId(companyId, branchId);
        }
    }
}
