using System.Web.Http;

namespace TimeRegistrationSystem
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{*anything}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
