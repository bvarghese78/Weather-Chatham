using System.Web;
using System.Web.Optimization;

namespace WeatherAppChatham
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/weathericons/weather-icons.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-ui-router.js"));

            bundles.Add(new ScriptBundle("~/bundles/weatherApp")
                .Include("~/app/init.js")
                .IncludeDirectory("~/app/Controller", "*.js")
                .IncludeDirectory("~/app/Service", "*.js"));
        }
    }
}
