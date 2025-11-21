using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BanDoGiaDung.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        public ActionResult BackToHome()
        {

            return Redirect("~/Home/Index");

        }


        public ActionResult Logout()
        {
            // Clear session data
            Session.Clear();
            Session.Abandon();

            // Redirect to the Logout action in the AccountController
            return RedirectToAction("Logout", "Account", new { area = "" });
        }
    }
}