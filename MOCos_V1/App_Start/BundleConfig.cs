using System.Web;
using System.Web.Optimization;

namespace MOCos_V1
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/vendor/jquery-1.12.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                       "~/Content/js/wow.min.js",
                         "~/Content/js/jquery-price-slider.js",
                          "~/Content/js/owl.carousel.min.js",
                            "~/Content/js/jquery.scrollUp.min.js",
                            "~/Content/js/meanmenu/jquery.meanmenu.js",
                            "~/Content/js/counterup/jquery.counterup.min.js",
                            "~/Content/js/counterup/waypoints.min.js",
                            "~/Content/js/counterup/counterup-active.js",
                            "~/Content/js/scrollbar/jquery.mCustomScrollbar.concat.min.js",
                            "~/Content/js/jvectormap/jquery-jvectormap-2.0.2.min.js",
                            "~/Content/js/jvectormap/jquery-jvectormap-world-mill-en.js",
                            "~/Content/js/jvectormap/jvectormap-active.js",
                            "~/Content/js/sparkline/jquery.sparkline.min.js",
                            "~/Content/js/sparkline/sparkline-active.js",
                            "~/Content/js/flot/jquery.flot.js",
                            "~/Content/js/flot/jquery.flot.resize.js",
                            "~/Content/js/flot/curvedLines.js",
                            "~/Content/js/flot/flot-active.js",
                            "~/Content/js/knob/jquery.knob.js",
                            "~/Content/js/knob/jquery.appear.js",
                            "~/Content/js/knob/knob-active.js",
                            "~/Content/js/wave/waves.min.js",
                            "~/Content/js/wave/wave-active.js",
                            "~/Content/js/todo/jquery.todo.js",
                            "~/Content/js/plugins.js",
                            "~/Content/js/data-table/jquery.dataTables.min.js",
                            "~/Content/js/data-table/data-table-act.js",
                            "~/Content/js/chat/moment.min.js",
                            "~/Content/js/chat/jquery.chat.js",
                            "~/Content/js/main.js"));
            
                            //"~/Content/js/tawk-chat.js"

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                       "~/Content/css/font-awesome.min.css",
                        "~/Content/css/owl.carousel.css",
                         "~/Content/css/owl.theme.css",
                          "~/Content/css/owl.transitions.css",
                           "~/Content/css/meanmenu/meanmenu.min.css",
                            "~/Content/css/animate.css",
                             "~/Content/css/normalize.css",
                              "~/Content/css/scrollbar/jquery.mCustomScrollbar.min.css",
                                "~/Content/css/jvectormap/jquery-jvectormap-2.0.3.css",
                                  "~/Content/css/notika-custom-icon.css",
                                  "~/Content/css/jquery.dataTables.min.css",
                                    "~/Content/css/wave/waves.min.css",
                                     "~/Content/css/main.css",
                                      "~/Content/css/style.css",
                                         "~/Content/css/responsive.css"));
        }
    }
}
