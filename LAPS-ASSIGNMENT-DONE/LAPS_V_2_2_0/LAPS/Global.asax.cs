using System;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using LAPS.App_Start;
using LAPS.Controllers;

namespace LAPS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_BeginRequest()
        {


            Utilities.MySystemCulture.ChangeCulture();
           // checkForInternetAndService();


        }

        // public void CallOne()
        // {
        // }
        //protected void Applcation_PreSendRequestHeaders()
        //{
        //    var Request = HttpContext.Current.Request;

        //}

        //protected void Applcation_PreSendContent()
        //{
        //    var Request = HttpContext.Current.Request;

        //}
        // protected void Application_PreRequestHandlerExecute()
        // {

        // }
        // protected void Application_AuthenticateRequest()
        // {

        // }
        // protected void Application_AuthorizeRequest()
        // {

        // }
        // protected void Application_AcquireRequestState() 
        // {

        // }
        //protected void Session_Start()
        //{
        //    Ping ping = new Ping();
        //    PingReply pingresult = ping.Send("64.233.160.0");
        //    if (pingresult.Status.ToString() == "Success")
        //    {
        //        //Response.Redirect("http://xxx.xxx.xxx.xx/sample");
        //    }
        //}


        //protected void checkForInternetAndService()
        //{

        //    var http = HttpContext.Current;
        //    if (http.Request.HttpMethod == "POST")
        //    {
        //        Ping ping = new Ping();
        //        PingReply pingresult = ping.Send("8.8.8.8");
        //        if (pingresult.Status.ToString() != "Success")
        //        {
        //            // txtPingStatus.Text = "Ping to 8.8.8.8 is succeed";
        //            //Response.Redirect("http://xxx.xxx.xxx.xx/sample");
        //            //Response.Redirect("Views/Branch/BranchSettings.cshtml");
        //            HttpContext.Current.Response.Redirect("~/Denied.html");

        //        }

        //        ServiceController sc = new ServiceController("Azolution Folder Sync");
        //        if (sc.Status != ServiceControllerStatus.Running)
        //        {
        //            //Response.End();
        //            //System.Diagnostics.Debugger.Break();
        //            //bool isAjaxCall = string.Equals("XMLHttpRequest", Context.Request.Headers["x-requested-with"], StringComparison.OrdinalIgnoreCase);
        //            //Context.ClearError();
        //            //if (isAjaxCall)
        //            //{
        //            //    Context.Response.ContentType = "application/json";
        //            //    Context.Response.StatusCode = 200;
        //            //    Context.Response.Write(
        //            //        new JavaScriptSerializer().Serialize(
        //            //            new { error = "some nasty error occured" }
        //            //        )
        //            //    );



        //            //var exception = Server.GetLastError();
        //            //// TODO: Log the exception or something
        //            //Response.Clear();
        //            //Server.ClearError();

        //            //var routeData = new RouteData();
        //            //routeData.Values["controller"] = "Home";
        //            //routeData.Values["action"] = "ResetPassword";
        //            //Response.StatusCode = 500;
        //            //IController controller = new HomeController();
        //            //var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
        //            //controller.Execute(rc);
        //            //}


        //            Context.RewritePath("~/Denied.html");



        //            //HttpContext.Current.Response.Write("sss");
        //            // HttpContext.Current.Response.Redirect("~/Denied.html");

        //        }
        //    }


        //}



    }
}
