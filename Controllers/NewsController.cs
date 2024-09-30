using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsWebSite.Models;
using NewsWebSite.Models.Repository;

namespace NewsWebSite.Controllers
{
    [HandleError]
    public class NewsController : Controller
    {
        
      readonly  NewsRepository newsRepo;
        public NewsController()
        {
            newsRepo = new NewsRepository(new NewsDB());
                
        }

        public ActionResult Index()
        {
            var news = newsRepo.GetAllNews();
            return View(news);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news =newsRepo.GetNewsById((int)id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = newsRepo.GetNewsById((int)id);
            if (news == null)
            {
                return HttpNotFound();
            }
            FillCatogriesDDl(news.Category_Id);
            return View(news);
        }

        public ActionResult GetAllCategories()
        {
            ViewBag.categories = newsRepo.GetAllNewsCategories();
            return PartialView();
        }

        public ActionResult GetNewsByCategoryId(int id)
        {
            ViewBag.CategoryNews = newsRepo.GetNewsByCategoryId(id);
            return PartialView();

        }
        public ActionResult getNewsDetails(int id)
        {
            ViewBag.newsInfo = newsRepo.GetNewsById(id); ;
            return PartialView();

        }

        private void FillCatogriesDDl(int? SelectedCategoryId)
        {
            ViewBag.Category_Id = new SelectList(newsRepo.GetAllNewsCategories(), "ID", "categoryName", SelectedCategoryId);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
