using System.Web;
using System.Web.Optimization;

namespace Bong.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Bong Web
            bundles.Add(new StyleBundle("~/Content/bong").Include(
                      "~/Content/style.css",
                      "~/Content/jquery.smartmenus.bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/bong").Include(
                      "~/Scripts/bong.script.js",
                      "~/Scripts/jquery.smartmenus.js",
                        "~/Scripts/jquery.smartmenus.bootstrap.js"));

            // Bong Admin
            bundles.Add(new StyleBundle("~/Content/admin").Include(
                      "~/Areas/Admin/Content/style.css",
                      "~/Content/kendo/2014.1.318/kendo.rtl.min.css",
                      "~/Content/kendo/2014.1.318/kendo.blueopal.min.css",
                      "~/Content/kendo/2014.1.318/kendo.common.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                      "~/Scripts/kendo/2014.1.318/kendo.web.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // Bootstrap
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/respond.min.js"));

            // JQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // jQuery UI 
            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                        "~/Content/themes/base/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            // Knockout
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-3.4.0.js",
                        "~/Scripts/json2.min.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
