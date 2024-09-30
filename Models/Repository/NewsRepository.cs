using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsWebSite.Models;

namespace NewsWebSite.Models.Repository
{
    public class NewsRepository :IDisposable
    {
        readonly NewsDB db;
        public NewsRepository(NewsDB db)
        {
            this.db = db;
        }

        public List<News> GetAllNews()
        {
            var list = db.News.OrderByDescending(x => x.Dateofoccurrence).ToList();
            return list;
        }

        public List<Category> GetAllNewsCategories()
        {
            var cats = db.Categories.ToList();
            return cats;
        }

        public List<News> GetNewsByUserId(int userId)
        {
            var lst = db.News.Where(x => x.userId == userId).ToList();
            return lst;

        }
        public List<News> GetNewsByUserIdAsceding(int userId)
        {
            var lst = db.News.Where(x => x.userId == userId).OrderBy(x => x.Dateofoccurrence).ToList();
            return lst;

        }
        public List<News> GetNewsByUserIdDesceding(int userId)
        {
            var lst = db.News.Where(x => x.userId == userId).OrderByDescending(x => x.Dateofoccurrence).ToList();
            return lst;

        }

        public List<News> GetNewsByCategoryId(int nCateoryId)
        {
            var list = db.News.Where(x => x.Category_Id == nCateoryId).OrderByDescending(x => x.Dateofoccurrence).ToList();
            return list;
        }

        public News GetNewsById(int id)
        {
            var newsInfo = db.News.Find(id);
            return newsInfo;
        }

        public void AddNews(News newsInfo)
        {
            db.News.Add(newsInfo);
            save();
        }

        public void DeleteNews(int id)
        {
            var newsInfo = GetNewsById(id);
            db.News.Remove(newsInfo);
            save();
        }

        public void EditNews(News newsInfo)
        {
            var newsInfo1 = GetNewsById(newsInfo.Id);
            db.Entry(newsInfo1).State = System.Data.Entity.EntityState.Modified;
            save();

        }


        public void save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}