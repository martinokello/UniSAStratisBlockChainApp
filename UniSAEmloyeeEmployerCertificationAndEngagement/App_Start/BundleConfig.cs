using System.Web;
using System.Web.Optimization;

namespace UniSAEmloyeeEmployerCertificationAndEngagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/assets/js").Include(
                        "~/assets/js/util.js",
                        "~/assets/js/main.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
        }
    }
}
