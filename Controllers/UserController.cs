using NewsWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsWebSite.FileHandlers;
using NewsWebSite.Models.Repository;

namespace NewsWebSite.Controllers
{
    [HandleError]
    public class UserController : baseController
    {
         UserRepository  userRepo;
        FileValidation fileValidation ;
        public UserController()
        {
         userRepo = new UserRepository(new NewsDB());
         fileValidation = new FileValidation();
    }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult profile()
        {
            return View(ClientInfo);
        }

        #region Action :: EditProfile
        [HttpGet]
        public ActionResult EditProfile(int userId)
        {
            var user = userRepo.GetUserById(userId);
            if (user == null)
            {
                return RedirectToAction("login");
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
               // var user = userRepo.GetUserById(userInfo.userId);
                userRepo.EditUserInfo(userInfo);
                UpdateSessionInfo();
                return RedirectToAction("viewOperationResult", new { message = "Data updated successfully." });
            }
            return View();
        }
        #endregion

        public ActionResult viewOperationResult(string message)
        {
            ViewBag.message = message;
            return View();
        }


        #region Action:: updatePersonalPhoto
        public ActionResult updatePersonalPhoto(int id)
        {
            var newsInfo = userRepo.GetUserById(id);
            if (newsInfo == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        public ActionResult updatePersonalPhoto(int? id, HttpPostedFileBase Photo)
        {
            if (id == null)
            {
                return View("BadRequest");
            }
            var UserInfo = userRepo.GetUserById((int)id);
            if (UserInfo == null)
            {
                return HttpNotFound();
            }
            string virtualDirectoryPath = string.Empty;
            string UniqueFileName = string.Empty;

            if (!fileValidation.checkIfFileEmpty(Photo))
            {

                if (!fileValidation.CheckFileValidaity(Photo, new string[] { ".jpg", ".jpeg", ".gif", ".png" }))
                {
                    ViewBag.fileError = "File extension not supported   you should upload jpg or jpeg or gif or png";
                    return View();

                }
                virtualDirectoryPath = "~/attachment/";
                UniqueFileName = fileValidation.GetUniqueFileName(Server.MapPath(virtualDirectoryPath), Photo.FileName);
                string filePath = string.Format("{0}{1}", Server.MapPath(virtualDirectoryPath), UniqueFileName);
                Photo.SaveAs(filePath);

            }

            string PhysicalDirectoryPath = Server.MapPath("~/attachment/");
           // string UniqueFileName = fileValidation.GetUniqueFileName(PhysicalDirectoryPath, Photo.FileName);
            //string filePath = string.Format("{0}{1}", PhysicalDirectoryPath, UniqueFileName);
           // SaveFile(Photo, filePath);
            UserInfo.photo = virtualDirectoryPath +UniqueFileName;
            UserInfo.Confirm_password = UserInfo.pass_word;
            userRepo.UpdateUserPhoto(UserInfo);
            UpdateSessionInfo();
            return RedirectToAction("Profile");

        }

        #endregion

        #region method : UpdateSessionInfo
        private void UpdateSessionInfo()
        {
            var user = Session["clientInfo"] as user;
            var userInfo = userRepo.GetUserById(user.userId);
            Session["clientInfo"] = userInfo;
        }
        #endregion

       
    }
}