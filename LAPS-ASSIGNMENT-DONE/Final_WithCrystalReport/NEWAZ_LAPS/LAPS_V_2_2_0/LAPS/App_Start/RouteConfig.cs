using System.Web.Mvc;
using System.Web.Routing;

namespace LAPS.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*(CrystalImageHandler).*" });
            routes.MapRoute(
                "Default.iis",                                              // Route name
                "{controller}.mvc/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Login", id = "" }  // Parameter defaults
                //new { controller = "Incident", action = "Incident", id = "" }  // Parameter defaults
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
                //defaults: new { controller = "Incident", action = "Incident", id = UrlParameter.Optional }
            );
        }
    }
}