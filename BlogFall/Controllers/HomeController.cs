using BlogFall.Models;
using BlogFall.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogFall.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index(int? cid, int page = 1)
        {
            int pageSize = 5;

            ViewBag.SubTitle = "Yazılarım";

            IQueryable<Post> result = db.Posts;//sorgulanabilir liste filtreleme

            Category cat = null;

            if (cid != null)
            {
                cat = db.Categories.Find(cid);
                if (cat == null)
                {
                    return HttpNotFound();
                }
                ViewBag.SubTitle = cat.CategoryName;
                result = result.Where(x => x.CategoryId == cid);//Filtrelediğimizi yolladık.
            }

            ViewBag.page = page;
            ViewBag.pageCount = Math.Ceiling(result.Count() / (decimal)pageSize);
            ViewBag.nextPage = page + 1;
            ViewBag.prevPage = page - 1;
            ViewBag.cid = cid;

            return View(result
                .OrderByDescending(x => x.CreationTime)
                .Skip((page - 1) * pageSize)//20 atla
                .Take(pageSize)//3 tane al
                .ToList());//sıraladık
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CategoriesPartial()
        {
            return PartialView("_CategoriesPartial", db.Categories.ToList());//
        }

        public ActionResult ShowPost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post==null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SendComment(SendCommentViewModel model)
        {
            if (ModelState.IsValid)//veriler hatasız gemnişse
            {
                Comment comment = new Comment
                {
                    AuthorId = User.Identity.GetUserId(),
                    AuthorName=model.AuthorName,
                    AuthorEmail= model.AuthorEmail,
                    Content=model.Content,
                    CreationTime=DateTime.Now,
                    ParentId=model.ParentId,
                    PostId=model.PostId
                };
                db.Comments.Add(comment);
                db.SaveChanges();

                return Json(comment);//ajax metoduyla kullanıcıya geri döndürüyoruz yorum yapmak için
            }
            var errorList = ModelState.Values.SelectMany(m => m.Errors)//model state deki hata mesajlaını toptan vermeye yarar
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { Errors=errorList});
        }
    }
}