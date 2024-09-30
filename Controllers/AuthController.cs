using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsWebSite.FileHandlers;
using NewsWebSite.Models;
using NewsWebSite.Models.Repository;

namespace NewsWebSite.Controllers
{
    [HandleError]
    public class AuthController : Controller
    {
      //  NewsDB db = new NewsDB();

        readonly UserRepository userRepo;
         FileValidation fileValidation;
        public AuthController()
        {
            userRepo = new UserRepository(new NewsDB());
            fileValidation = new FileValidation();

        }

        [HttpGet]
        public ActionResult login()
        {
            if (Request.Cookies["newscookie"]!=null)
            {
                LoginData loginData = new LoginData();
                loginData.Email = Request.Cookies["newscookie"].Values["email"];
                loginData.Password = Request.Cookies["newscookie"].Values["password"];
                if (CheckIfUserIsExist(loginData))
                {

                    return RedirectToAction("Index", "News");
                }
            }

            return View();
        }


        [HttpPost]
        public ActionResult login(LoginData loginData , bool remeber=false)
        {
            if (CheckIfUserIsExist(loginData))
            {
                if (remeber)
                {
                    AddCookie();
                }
                return RedirectToAction("Index", "News");
            }
            else
            {
                ViewBag.loginError = "Invalid Email or password";

            }
            return View();
        }

        private void AddCookie()
        {
            HttpCookie cookie = new HttpCookie("newscookie");
            user usr= Session["clientInfo"] as user;
            cookie.Values.Add("email", usr.email);
            cookie.Values.Add("password", usr.pass_word);
            cookie.Expires=DateTime.Now.AddDays(30);
            // store file in client device
            Response.Cookies.Add(cookie);
        }
        private bool CheckIfUserIsExist(LoginData loginData)
        {
            var user = userRepo.GetUserByLoginInfo(loginData);
            if (user!=null)
            {
                Session.Add("clientInfo", user);
            }
            return user != null;

        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(user _user ,HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                string virtualDirectoryPath = string.Empty;
                string UniqueFileName = string.Empty;

                if (!fileValidation.checkIfFileEmpty(photo))
                {

                    if (!fileValidation.CheckFileValidaity(photo, new string[] { ".jpg", ".jpeg", ".gif", ".png" }))
                    {
                        ViewBag.fileError = "File extension not supported   you should upload jpg or jpeg or gif or png";
                        return View();

                    }
                    virtualDirectoryPath = "~/attachment/";
                     UniqueFileName = fileValidation.GetUniqueFileName(Server.MapPath(virtualDirectoryPath), photo.FileName);
                    string filePath = string.Format("{0}{1}", Server.MapPath(virtualDirectoryPath), UniqueFileName);
                    photo.SaveAs(filePath);

                }
            
                user usr = _user;
                usr.photo = virtualDirectoryPath+UniqueFileName;

                userRepo.Add(usr);
                Session.Add("clientInfo", usr);
                return RedirectToAction("Profile","user",usr);
            }
            else
            {
                return View();

            }
        }

        public ActionResult checkEmail(string email)
        {
           var usr = userRepo.GetUserByEmail(email);

            return Json(usr == null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
                Session["clientInfo"] = null;
                RemoveCookie();
                return RedirectToAction("login");
        }

        private void RemoveCookie()
        {
            HttpCookie cookie = new HttpCookie("newscookie");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(LoginData data)
        {
            var usr = userRepo.GetUserByEmail(data.Email);
            if (usr !=null)
            {
                Session["userId_Changepassword"] = usr.userId;
                return RedirectToAction("changePassword");
            }
            ViewBag.EmailError = "Email does not exist.";
            return View();
        }

        public ActionResult changePassword()
        {
            return View();

        }

        [HttpPost]
        public ActionResult changePassword(ChangePassword data)
        {

            if (ModelState.IsValid && Session["userId_Changepassword"]!=null)
            {
                int userId = (int)Session["userId_Changepassword"];
                Session["userId_Changepassword"] = null;
                var usr =userRepo.GetUserById(userId);
                usr.pass_word = data.Password;
                usr.Confirm_password = data.ConfirmPassword;
                userRepo.ChangePassword(usr);
                return RedirectToAction("ViewResult",new {message= "Password changed successfully"});

            }
            return View();

        }

        public ActionResult ViewResult(string message)
        {
            ViewBag.message = message;
            return View();
        }


    }
}