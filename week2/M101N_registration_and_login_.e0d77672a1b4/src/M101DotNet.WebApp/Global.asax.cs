using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using M101DotNet.WebApp.App_Start;

namespace M101DotNet.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BsonConfig.ConfigureBson();            
        }
    }
}
