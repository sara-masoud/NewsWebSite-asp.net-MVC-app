using NewsWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebSite.Controllers
{
    public abstract class baseController : Controller
    {
        protected user ClientInfo
        {
            get
            {
                var usr = Session["clientInfo"] as user;
                return usr;
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

          //  user usrInfo = Session["clientInfo"] as user;
            if (ClientInfo == null)
            {
                filterContext.Result = RedirectToAction("login", "auth");

            }
            base.OnActionExecuting(filterContext);
        }



    }
}