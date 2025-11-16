using BanDoGiaDung.Models;
using BanDoGiaDung.Models.Account;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BanDoGiaDung.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private GiaDungDbContext db = new GiaDungDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Accounts.FirstOrDefault(x => x.Email == model.Email
                                                    && x.password == model.Password);

            if (user == null)
            {
                ViewBag.ThongBao = "Sai email hoặc mật khẩu!";
                return View(model);
            }
            // Lưu Session
            Session["UserID"] = user.account_id;   // số
            Session["UserName"] = user.Name;       // chuỗi

            Session["UserEmail"] = user.Email;
            Session["Role"] = user.Role;

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var check = db.Accounts.FirstOrDefault(x => x.Email == model.Email);
            if (check != null)
            {
                ViewBag.ThongBao = "Email đã tồn tại!";
                return View(model);
            }
            Accounts acc = new Accounts()
            {
                Name = model.Name,
                Email = model.Email,
                password = Crypto.Hash(model.Password),
                Phone = model.PhoneNumber,
                Role = 1
            };

            db.Configuration.ValidateOnSaveEnabled = false;
            db.Accounts.Add(acc);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string email = Session["UserEmail"]?.ToString();

            if (email == null)
                return RedirectToAction("Login");

            var user = db.Accounts.FirstOrDefault(x => x.Email == email);

            if (user.password != model.OldPassword)
            {
                ViewBag.ThongBao = "Mật khẩu cũ không đúng!";
                return View(model);
            }

            user.password = model.NewPassword;
            db.SaveChanges();

            ViewBag.ThongBao = "Đổi mật khẩu thành công!";
            return View();
        }

        // ========================= RESET PASSWORD =========================

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = db.Accounts.FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
            {
                ViewBag.ThongBao = "Email không tồn tại!";
                return View(model);
            }

            user.password = model.NewPassword;
            db.SaveChanges();

            ViewBag.ThongBao = "Reset mật khẩu thành công!";
            return View();
        }

        // ========================= LOGOUT =========================
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public ActionResult Profile()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int id = (int)Session["UserID"];

            var user = db.Accounts.FirstOrDefault(x => x.account_id == id);

            return View(user);
        }

    }
}