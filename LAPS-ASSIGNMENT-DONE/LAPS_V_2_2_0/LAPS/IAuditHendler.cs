using System.Web;
using AuditTrail.Entity;

namespace LAPS
{
    public class IAuditHendler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //write your handler implementation here.
        }


        internal AuditTrails GetAuditInfo(int UserId)
        {
           
            AuditTrails audit = new AuditTrails();
            audit.User_Id = UserId;
            audit.Client_Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            audit.Client_User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            audit.Requested_Url = HttpContext.Current.Request.UrlReferrer.AbsolutePath;
            return audit;
        }

        internal AuditTrails GetAuditInfo(int userId,string shortDesc)
        {

            AuditTrails audit = new AuditTrails();
            audit.User_Id = userId;
            audit.Client_Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            audit.Client_User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            audit.Requested_Url = HttpContext.Current.Request.UrlReferrer.AbsolutePath;
            return audit;
        }

        #endregion


        
        internal AuditTrails GetAuditInfo(int userId, string shortDesc,string type, string description)
        {
            AuditTrails audit = new AuditTrails();
            audit.User_Id = userId;
            audit.Client_Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            audit.Client_User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            audit.Shortdescription = shortDesc;
            audit.Audit_Description = description;
            audit.AudiT_Type = type;
            audit.Requested_Url= HttpContext.Current.Request.UrlReferrer.AbsolutePath;
            return audit;
        }

        internal AuditTrails GetAuditInfo(int userId, string shortDesc, int actionId, string description)
        {
            AuditTrails audit = new AuditTrails();
            audit.User_Id = userId;
            audit.Client_Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            audit.Client_User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            audit.Shortdescription = shortDesc;
            audit.Audit_Description = description;
            audit.AudiT_Type = actionId == 0 ? "Insert" : "Update";
            audit.Requested_Url = HttpContext.Current.Request.UrlReferrer.AbsolutePath;
            return audit;
        }

    }
}
