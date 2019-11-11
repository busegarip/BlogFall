using BlogFall.Areas.Admin.ViewModel;
using BlogFall.Attributes;
using BlogFall.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BlogFall.Areas.Admin.Controllers
{
    [Breadcrumb("Yazılar")]
    public class PostsController : AdminBaseController
    {
        // GET: Admin/Posts
        [Breadcrumb("İndeks")]
        public ActionResult Index()
        {
            return View(db
                .Posts
                .OrderByDescending(x=>x.CreationTime)
                .ToList());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var post = db.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            return Json(new { success=true});
        }

        [Breadcrumb("Düzenle")]
        public ActionResult Edit(int id)
        {
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "Id", "CategoryName");

            PostEditViewModel vm = db.Posts.Select(x => new PostEditViewModel
            {
                Id = x.Id,
                CategoryId=x.CategoryId,
                Content=x.Content,
                Title=x.Title
            }).FirstOrDefault(x=>x.Id==id);

            return View(vm);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Düzenle")]
        public ActionResult Edit(PostEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = db.Posts.Find(model.Id);

                post.Content = model.Content;
                post.CategoryId = model.CategoryId;
                post.Title = model.Title;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "Id", "CategoryName");
            return View();
        }

        [Breadcrumb("Yeni")]
        public ActionResult New()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "Id", "CategoryName");


            return View("Edit", new PostEditViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Yeni")]
        [ValidateInput(false)]//html taglerini kontrol güvenliğini iptal etme
        public ActionResult New(PostEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = new Post
                {
                    Title=model.Title,
                    Content=model.Content,
                    CategoryId=model.CategoryId,
                    AuthorId=User.Identity.GetUserId(),
                    CreationTime=DateTime.Now
                };

                db.Posts.Add(post);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            

            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "Id", "CategoryName");

            return View("Edit", new PostEditViewModel());
        }

        [HttpPost]
        public ActionResult AjaxImageUpload(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0 || !file.ContentType.StartsWith("image/"))//yüklenen dosya resim olmalı,
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //diğer durumlarda dosya yüklenir
            var saveFolderPath = Server.MapPath("~/Upload/Posts");//harddisk te fiziksel olarak klasörün bulunduğu yolu döndürür.
            var ext = Path.GetExtension(file.FileName);//uzantısı ne onu alır
            var saveFileName = Guid.NewGuid().ToString() + ext;
            var saveFilePath = Path.Combine(saveFolderPath, saveFileName);
            file.SaveAs(saveFilePath);//kaydediyor. buse.jpg gibi bir yol elde ediyoruz bu adımlarla

            return Json(new { url = Url.Content("~/Upload/Posts/" + saveFileName) });
        }
    }
}