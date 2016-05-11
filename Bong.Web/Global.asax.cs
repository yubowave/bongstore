using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Bong.Core.Configuration;
using Bong.Core.Helper;
using Bong.Core.Infrastructure;
using Bong.Data;
using Bong.Services.Install;

namespace Bong.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Initlize system and register from DependecyResolver of Bong.Web
            TheSystem.Init();
            TheSystem.Current.UpdateContainer(new Bong.Web.Infrastructure.DependencyRegister());

            // Install sample data if database is empty
            BongConfig config = TheSystem.Current.Resolve<BongConfig>();
            if (config.IsInstallData)
            {
                IInstallService installService = TheSystem.Current.Resolve<IInstallService>();
                installService.InstallData();
            }

            // Initlize Mvc stuff
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            // handle errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var webHelper = TheSystem.Current.Resolve<IWebHelper>();
                if (!webHelper.IsStaticResource(this.Request))
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

                    // Call target Controller and pass the routeData.
                    IController errorController = TheSystem.Current.Resolve<Bong.Web.Controllers.CommonController>();

                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Common");
                    routeData.Values.Add("action", "NoPageFound");

                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
            else if (httpException != null)
            {
                Response.Clear();
                Server.ClearError();
                Response.TrySkipIisCustomErrors = true;

                // Call target Controller and pass the routeData.
                IController errorController = TheSystem.Current.Resolve<Bong.Web.Controllers.CommonController>();

                var routeData = new RouteData();
                routeData.Values.Add("controller", "Common");
                routeData.Values.Add("action", "Error");
                routeData.Values.Add("error", httpException);
                errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            }
        }
    }
}
