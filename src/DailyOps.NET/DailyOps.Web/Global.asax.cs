using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DailyOps.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            // Will cause the demo data to be removed on deploy
            string schemaFile = HttpRuntime.AppDomainAppPath.TrimEnd('\\') + "\\App_Data\\DailyOps.ReadModels.Schema.mysql";

            if(!System.IO.File.Exists(schemaFile))
            {
                Wiring.Proxy.GenerateSchemaToFile(schemaFile);
                Wiring.Proxy.CreateReadModelDB();
            }


            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}