using System.Web;
using System.Web.Optimization;

namespace BlogFall
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //debug aşamasında include dakini yani locali kullanır. publish edip yayınlarken adresi verilen dosyayı kullanır.
            bundles.UseCdn = true;//yayınlanırken cdn geliştirilirken lokaldeki demek oluyor

            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://code.jquery.com/jquery-3.4.1.min.js").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/fontawesome.css",
                      "~/Content/site.css"));

            //cdn üzerinden yapmak daha iyi bir yöntem bu aşağıdaki kod olmadan cdn deki kullanılamıyor
            //derleyiciye hangisinin alınıp hangisinin alınmayacağını söylerler işlem öncesi direktif denir bunlara
        #if DEBUG
                    BundleTable.EnableOptimizations = false;
        #else
                    BundleTable.EnableOptimizations = true;//bu yayınlanırken aktif olmalı realase modda bu çalışır
        #endif

        }
    }
}
