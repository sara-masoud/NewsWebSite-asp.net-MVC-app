using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsWebSite.FileHandlers;
using NewsWebSite.Models;
using NewsWebSite.Models.Repository;
using PagedList;

namespace NewsWebSite.Controllers
{
    [HandleError]
    public class UserNewsController : baseController
    {

        NewsRepository newsRepo;
        private FileValidation fileValidation;
        public UserNewsController()
        {
            newsRepo = new NewsRepository(new NewsDB());
            fileValidation = new FileValidation();
        }
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            int pageSize = 3;
            int pageNumber = page ?? 1;
            var news = GetNewsBasedOnSortOrder(sortOrder);
            return View(news.ToPagedList(pageNumber, pageSize));
        }

        private List<News> GetNewsBasedOnSortOrder(string sortOrder)
        {
            var news = newsRepo.GetNewsByUserId(ClientInfo.userId);
            switch (sortOrder)
            {
                case "Date":
                    news = news.OrderBy(s => s.Dateofoccurrence).ToList();
                    break;
                case "date_desc":
                    news = news.OrderByDescending(s => s.Dateofoccurrence).ToList();
                    break;
            }
            return news;

        }

        public ActionResult Details(int? id)
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
            FillCatogriesDDl(id);
            return View(news);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                News oldNews = newsRepo.GetNewsById(news.Id);
                oldNews = prepareNewsInfo(oldNews, news);
                newsRepo.EditNews(oldNews);
                return RedirectToAction("Index");
            }
            FillCatogriesDDl(news.Category_Id);
            return View(news);
        }


        #region method :FillCatogriesDDl
        private void FillCatogriesDDl(int? SelectedCategoryId)
        {
            ViewBag.Category_Id = new SelectList(newsRepo.GetAllNewsCategories(), "ID", "categoryName", SelectedCategoryId);
        }
        #endregion
        #region method:prepareNewsInfo 
        private News prepareNewsInfo(News oldNews, News news)
        {
            oldNews.title = news.title;
            oldNews.decription = news.decription;
            oldNews.Category_Id = news.Category_Id;
            oldNews.Dateofoccurrence = news.Dateofoccurrence;

            return oldNews;

        }
        #endregion
        #region Action : Delete News
        public ActionResult Delete(int id)
        {
            News news = newsRepo.GetNewsById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            newsRepo.DeleteNews(id);
            TempData["deleted"] = "true";
            return RedirectToAction("Index");
        }
        #endregion


        #region Action:: Create News
        public ActionResult Create()
        {
            FillCatogriesDDl(null);
            return View();
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                string virtualDirectoryPath = string.Empty;
                string UniqueFileName = string.Empty;

                if (!fileValidation.checkIfFileEmpty(image))
                {

                    if (!fileValidation.CheckFileValidaity(image, new string[] { ".jpg", ".jpeg", ".gif", ".png" }))
                    {
                        ViewBag.fileError = "File extension not supported   you should upload jpg or jpeg or gif or png";
                        return View();

                    }
                    virtualDirectoryPath = "~/attachment/";
                    UniqueFileName = fileValidation.GetUniqueFileName(Server.MapPath(virtualDirectoryPath), image.FileName);
                    string filePath = string.Format("{0}{1}", Server.MapPath(virtualDirectoryPath), UniqueFileName);
                    image.SaveAs(filePath);

                }
                user usr = ClientInfo;
                news.userId = usr.userId;
                news.image = virtualDirectoryPath + UniqueFileName;
                newsRepo.AddNews(news);
                return RedirectToAction("Index");
            }
            FillCatogriesDDl(news.Category_Id);
            return View(news);
        }


        #region Actions::updateImage
        public ActionResult updateImage(int id)
        {
            var newsInfo = newsRepo.GetNewsById(id);
            if (newsInfo == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        public ActionResult updateImage(int? id, HttpPostedFileBase image)
        {
            if (id == null)
            {
                return View("BadRequest");
            }
            var newsInfo = newsRepo.GetNewsById((int)id);
            if (newsInfo == null)
            {
                return HttpNotFound();
            }

            string virtualDirectoryPath = string.Empty;
            string UniqueFileName = string.Empty;

            if (!fileValidation.checkIfFileEmpty(image))
            {

                if (!fileValidation.CheckFileValidaity(image, new string[] { ".jpg", ".jpeg", ".gif", ".png" }))
                {
                    ViewBag.fileError = "File extension not supported   you should upload jpg or jpeg or gif or png";
                    return View();

                }
                virtualDirectoryPath = "~/attachment/";
                UniqueFileName = fileValidation.GetUniqueFileName(Server.MapPath(virtualDirectoryPath), image.FileName);
                string filePath = string.Format("{0}{1}", Server.MapPath(virtualDirectoryPath), UniqueFileName);
                image.SaveAs(filePath);

            }


            // string filePath = string.Format("{0}{1}", PhysicalDirectoryPath, UniqueFileName);
            // string Virtualpath = GetFilePath(image);
            // image.SaveAs(Server.MapPath(filePath));



            // string Virtualpath = GetFilePath(image);
            //  image.SaveAs(filePath);
            newsInfo.image = virtualDirectoryPath + UniqueFileName;
            newsRepo.EditNews(newsInfo);
            return RedirectToAction("Details", new { id = id });

        }
        #endregion
        #region method : UpdateSessionInfo
        private void UpdateSessionInfo(user userInfo)
        {
            Session["clientInfo"] = userInfo;
        }
        #endregion

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
