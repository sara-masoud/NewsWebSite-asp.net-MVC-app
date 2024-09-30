using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsWebSite.Models;

namespace NewsWebSite.Models.Repository
{
   
    public class UserRepository :IDisposable
    {
       readonly NewsDB db ;

        public UserRepository(NewsDB db)
        {
            this.db = db;

        }

        public user GetUserById(int id)
        {
            var user = db.users.Find(id);
            return user;

        }

        public void EditUserInfo(UserInfo userInfo)
        {
            var user = GetUserById(userInfo.userId);
            user= PrepareUserInfo(user, userInfo);
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            save();

        }

        private user PrepareUserInfo(user ExistUser, UserInfo newUserInfo)
        {
            ExistUser.firstName = newUserInfo.firstName;
            ExistUser.lastName = newUserInfo.lastName;
            ExistUser.brief = newUserInfo.brief;
            ExistUser.Confirm_password = ExistUser.pass_word;
            return ExistUser;
        }

        public void Add(user user)
        {
            db.users.Add(user);
            save();

        }

        public void ChangePassword(user user)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            save();

        }
        public user GetUserByEmail(string email)
        {
         var user=   db.users.Where(x => x.email.Equals(email)).FirstOrDefault();
            return user;
        }
        // db.users.Where(x => x.email.Equals(data.Email)).FirstOrDefault();

        public void UpdateUserPhoto(user userInfo)
        {
            db.Entry(userInfo).State = System.Data.Entity.EntityState.Modified;
            save();

        }

        public user GetUserByLoginInfo(LoginData loginData)
        {

            var user = db.users.Where(x => x.email.Equals(loginData.Email) && x.pass_word.Equals(loginData.Password)).FirstOrDefault();
            return user;
        }

        private void save()
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