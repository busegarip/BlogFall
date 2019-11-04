using BlogFall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogFall.Controllers
{
    public abstract class BaseController : Controller//bütün controllerlarda ne kullanılıcaksa onları buraya ekle
    {
        protected ApplicationDbContext db = new ApplicationDbContext();//sadece çocukları erişebilsin.

        protected override void Dispose(bool disposing)//çöpe atılacağı zaman çalışır
        {
            base.Dispose(disposing);

            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}