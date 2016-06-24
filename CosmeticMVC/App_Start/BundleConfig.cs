using System.Web;
using System.Web.Optimization;


namespace CosmeticMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/csscore").Include(
                "~/Assets/Client/css/bootstrap.min.css",
                "~/Assets/Client/css/boostrap-social.css",
                "~/Assets/Client/css/font-awesome.min.css",
                "~/Assets/Client/css/bootstrap-theme.min.css",
                "~/Assets/Client/css/jquery-ui.css",
                "~/Assets/Client/css/style.css",
                "~/Assets/Client/css/slider.css",
                "~/Assets/Client/js/plugins/fancybox/jquery.fancybox-1.3.4.css",
                "~/Assets/Client/css/ngan-luong.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jscore").Include(
                "~/Assets/Client/js/jquery-ui.min.js",
                "~/Assets/Client/js/bootstrap.min.js",
                "~/Assets/Client/js/move-top.js",
                "~/Assets/Client/js/easing.js",
                "~/Assets/Client/js/startstop-slider.js",
                "~/Assets/Client/js/controller/BaseController.js", 
                "~/Assets/CommonScript.js"

                ));

            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include(
                "~/Assets/Client/js/jquery-1.11.3.min.js",
                "~/Assets/Client/js/jquery-migrate-1.2.1.min.js",
                "~/Assets/Client/js/plugins/fancybox/jquery.fancybox-1.3.4.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/cssAdmin").Include(
                "~/Assets/Admin/bower_components/bootstrap/dist/css/bootstrap.min.css",
                "~/Assets/Client/css/font-awesome.min.css",
                "~/Assets/Admin/bower_components/metisMenu/dist/metisMenu.min.css",
                "~/Assets/Admin/dist/css/sb-admin-2.css",
                "~/Assets/Admin/bower_components/font-awesome/css/font-awesome.min.css",
                "~/Assets/Client/js/plugins/fancybox/jquery.fancybox-1.3.4.css"));
        }
    }
}
