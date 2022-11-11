namespace Soundbox.Reloaded.Ui.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Templates",
                url: "Views/{*template}",
                defaults: new { controller = "Index", action = "Template" }
            );

            routes.MapRoute(
                name: "DirectiveTemplates",
                url: "Directives/{*template}",
                defaults: new { controller = "Index", action = "DirectiveTemplate" }    
            );

            routes.MapRoute(
                name: "DefaultIndex",
                url: "{action}",
                defaults: new { controller = "Index", action = "Index" }
            );
        }
    }
}
