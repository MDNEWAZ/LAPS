using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using AuditTrail.Entity.DataService;

using Azolution.Common.Helper;
using Azolution.Common.Validation;
using Azolution.Core.Service.Interface;
using Azolution.Core.Service.Service;
using Azolution.Entities.Core;
using Azolution.Login.Service.Service;
using Azolution.Security;
using Laps.Sale.SaleService.Service;

namespace LAPS.Controllers
{
    public class HomeController : Controller
    {

        ISystemSettingsRepository _systemSettingsRepository = new SystemSettingsService();
        IUsersRepository _usersRepository = new UsersService();

        public ActionResult Index()
        {
            if (Session["CurrentUser"] == null)
            {
               return View("Login");
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult Login()
        {
            
            return View("Login");
            
        }
        [HttpGet]
        public ActionResult Logoff()
        {
            Session["CurrentUser"] = null;
            return View("Login");
        }

        public ActionResult ResetPassword()
        {
            return View("ResetPassword");
        }
        public ActionResult ChangePassword()
        {
            if (Session["CurrentUser"] == null)
            {   
                return View("Login");
            }
            else
            {
                return View("ChangePassword");
            }
        }
        //These two line is called for work with audit trail
        IAuditHendler hendler = new IAuditHendler();
        AuditTrailDataService aService = new AuditTrailDataService();

        //[HttpPost]
        public string ValidateUserLogin(string loginId, string password)
        {
            var res = "";
            string user = "";

            
            try
            {
              
                var loginService = new LoginService();
                
               
                user = loginService.ValidateUserLogin(loginId, password);

                if ((user.Split('^')[0] == "Success") || (user.Split('^')[0] == "CHANGESHORT") || (user.Split('^')[0] == "CHANGELEAVE") || (user.Split('^')[0] == "CHANGESuccess") || (user.Split('^')[0] == "LATE") || (user.Split('^')[0] == "SHORT") || (user.Split('^')[0] == "LEAVE"))
                {
                    var currentUser = loginService.GetCurrentUser(user);
                    
                    Session["themeName"] = currentUser.Theme;
                    Session["CurrentUser"] = currentUser;


                    Users strusers = ((Users)(Session["CurrentUser"]));
                    if (strusers != null)
                    {
                    var companyId = 0;

                    companyId = strusers.CompanyId;
                    IOrganogramRepository _organogramRepository = new OrganogramService();

                    var organogramList = _organogramRepository.GetOrganogramTreeData(companyId);

                    //modefied by Ashraful 29/12/2014
                    // get all hierarchay company id and load session
                    List<Company> companies = organogramList;
                    strusers.CompanyList = companies;
                    //end of Ashraful
                    }

                }
                else
                {
                    Session["CurrentUser"] = null;
                }
                res = "Success"; //For Audit trail

            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            Users struser = ((Users)(Session["CurrentUser"]));
            
            if (struser != null)
            {
                
                //Audittail
                var audit = hendler.GetAuditInfo(struser.UserId, struser.UserName + " is try to login", "User Login",
                                                 res);
                aService.SendAudit(audit);
            }
            return user.Split('^')[0];
        }



        [HttpPost]
        public string ResetUserPassword(string loginId, string oldpassword, string newpassword)
        {
            var res = "";
            try
            {
                res = ResetUserPasswordValidate(loginId, oldpassword, newpassword);
                res = res.Split('^')[0];
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            //Audittail
          
            var audit = hendler.GetAuditInfo(0, "Reset User Password by Anonymous", "Reset Password", res);
            aService.SendAudit(audit);
            return res;
        }


        public string ResetUserPasswordValidate(string loginId, string oldpassword, string newpassword)
        {
            var validator = new ValidationHelper();
            
            var sb = new StringBuilder();

          
            var user = _usersRepository.GetUserByLoginId(loginId);

            //CHECKING IF USER EXISTS
            if (user == null)
            {
                return "Incorrect Login ID or Passward!";
            }

            //CHECKING IF OLD PASSWORD MATCH WITH DATABASE PASSWORD
            if (ValidationHelper.ValidateLoginPassword(oldpassword, user.Password, true))
            {

                var objSystemSettings = _systemSettingsRepository.GetSystemSettingsDataByCompanyId(user.CompanyId);

                //CHECKING IF NEW PASSWORD EXIST IN PASSWORD HISTORY
                IQueryable<PasswordHistory> passwordHistory = _usersRepository.GetPasswordHistory(user.UserId, objSystemSettings.OldPassUseRestriction);
                if (passwordHistory.Count() != 0)
                {
                    for (int i = 0; i < passwordHistory.Count(); i++)
                    {
                        if (passwordHistory.ElementAt(i).OldPassword == newpassword)
                        {
                            return "You have already used your new password! \nPlease try with another one.";
                        }
                    }
                }

                //CHECKING IF NEW PASSWORD COMPLY WITH PASSWORD POLICY
                string validate = validator.ValidateUser("", objSystemSettings.MinLoginLength, newpassword, objSystemSettings.MinPassLength, objSystemSettings.PassType, objSystemSettings.SpecialCharAllowed);
                if (validate == "Valid")
                {
                    string encPass = EncryptDecryptHelper.Encrypt(newpassword);
                    user.LastLoginDate = DateTime.Now;
                    user.LastUpdateDate = DateTime.Now;
                    user.IsExpired = false;
                    user.Password = encPass;

                    var passHistory = new PasswordHistory
                    {
                        HistoryId = 0,
                        UserId = user.UserId,
                        OldPassword = oldpassword,
                        PasswordChangeDate = DateTime.Now
                    };

                    var message = _usersRepository.UpdateUser(user, passHistory);
                    var license = new AzolutionLicense();
                    var expiryDate = license.GetExpiryDate();
                    var licUserNo = license.GetNumberOfUser();
                    //sb.AppendFormat("{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}^{8}", message, user.UserId, user.LoginId, user.UserName, user.CompanyID, user.EmployeeId, user.CompanyName, user.FullLogoPath, user.LogHourEnable);
                    sb.AppendFormat("{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}^{8}^{9}^{10}^{11}", message, user.UserId, user.LoginId, user.UserName, user.CompanyId, user.EmployeeId, user.CompanyName, user.FullLogoPath, user.LogHourEnable, expiryDate, licUserNo, user.FiscalYearStart, user.Theme);
                    return sb.ToString();
                }
                else
                {
                    return validate;
                }
            }
            else
            {
                return "Incorrect Login ID or Passward!";
            }
            
        }




        [HttpPost]
        public string ChangePassword(string password)
        {
            var res = "";
            Users objUser = ((Users)(Session["CurrentUser"]));
            try
            {
              

                var objSystemSettings = _systemSettingsRepository.GetSystemSettingsDataByCompanyId(objUser.CompanyId);

                res = _usersRepository.ChangePassword(password, objUser, objSystemSettings);
                if (res == "Success")
                {
                    objUser.IsFirstLogin = "No";
                }
                Session["CurrentUser"] = objUser;
                //return res;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            //Audittail
            var audit = hendler.GetAuditInfo(objUser.UserId, objUser.UserName + " is try to Change Password", "Change Password", res);
            aService.SendAudit(audit);

            return res;
        }

        //[HttpPost]
        public JsonResult GetCurrentUser()
        {
            Users user = ((Users)(Session["CurrentUser"]));
            return Json(user, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public string ForgetEmail(string emailaddress)
        {
            string result = "Success";
            try
            {
                var MailServer = ConfigurationSettings.AppSettings["MailServer"];
                var smtpPort = ConfigurationSettings.AppSettings["smtpPort"];
                var Sender = ConfigurationSettings.AppSettings["emailSender"];
                var emailpassword = ConfigurationSettings.AppSettings["emailpassword"];
                var Subject = ConfigurationSettings.AppSettings["Subject"];
                var mailBody = ConfigurationSettings.AppSettings["MailBody"];
                var sslEnable = Convert.ToBoolean(ConfigurationSettings.AppSettings["EnableSsl"]);
                var EmailTo = emailaddress;


                
                var objUser = _usersRepository.GetUserByEmailAddress(emailaddress);

                

                if (objUser!= null)
                {
                    var newPass = EncryptDecryptHelper.Decrypt(objUser.Password);

                    var Mailbody = "Dear " + objUser.UserName + ", <br>" + mailBody + "<br> Login ID : <b>" + objUser.LoginId + "</b> <br> Password : <b>" + newPass + "</b>.<br><br>Thank You,<br> SalesReS";
                    SmtpClient SmtpServer = null;
                    SmtpServer = emailpassword != string.Empty ?
                        new SmtpClient(MailServer, int.Parse(smtpPort))
                        {
                            Credentials = new System.Net.NetworkCredential(Sender, emailpassword)
                        }
                            : new SmtpClient(MailServer);
                    var mail = new MailMessage(Sender, EmailTo, Subject, Mailbody);
                    mail.IsBodyHtml = true;
                    SmtpServer.EnableSsl = sslEnable;
                    SmtpServer.Send(mail);
                    result = "Success";
                }
                else
                {
                    result = "Invalid Email Address";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;// "Failed";
            }
            //Audittail
            var audit = hendler.GetAuditInfo(0, "Recover Password by Email Id : " + emailaddress, "Forget Password", result);
            aService.SendAudit(audit);
            return result;
        }

       

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetUserTypeByUserId()
        {
            Users user = ((Users)(Session["CurrentUser"]));
            return Json(_usersRepository.GetUserTypeByUserId(user.UserId), JsonRequestBehavior.AllowGet);

        }

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
       
    }
}
