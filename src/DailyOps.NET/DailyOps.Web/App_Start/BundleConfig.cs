using System.Web;
using System.Web.Optimization;

namespace DailyOps.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/react").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/react/browser.min.js",
                        "~/Scripts/react/react.js",
                        "~/Scripts/react/react-dom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/sammy-{version}.js"));


            bundles.Add(new StyleBundle("~/bundles/bootstrap").Include(
                          "~/Content/bootstrap.css",
                          "~/Content/bootstrap-theme.css"));

            bundles.Add(new StyleBundle("~/bundles/dailyops").Include(
                          "~/Content/dailyops-*"));

            /*
             
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" />
    <link rel="stylesheet" href="http://swizkon.github.io/dailyops/frontend/css/DailyOps-app.css" />
             */



        }
    }
}