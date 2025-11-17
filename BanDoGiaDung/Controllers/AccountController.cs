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
        [ValidateAntiForgeryToken] // Nhớ thêm cái này
        public ActionResult Login(LoginViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Accounts.FirstOrDefault(x => x.Email.ToLower() == model.Email.ToLower());

            if (user == null)
            {
                ModelState.AddModelError("", "Sai email hoặc mật khẩu!");
                return View(model);
            }

            if (Crypto.VerifyHashedPassword( user.password,model.Password))
            {
                // === ĐĂNG NHẬP THÀNH CÔNG ===

                // Lưu Session (Code của bà làm đúng rồi)
                Session["UserID"] = user.account_id;   // số
                Session["UserName"] = user.Name;       // chuỗi

                Session["UserEmail"] = user.Email;
                Session["Role"] = user.Role;

                // Chuyển về trang chủ
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Sai email hoặc mật khẩu!");


            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Register(RegisterViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Thêm .ToLower() ở cả 2 vế để nó so sánh chữ thường
            var check = db.Accounts.FirstOrDefault(x => x.Email.ToLower() == model.Email.ToLower());
            if (check != null)
            {
                ViewBag.ThongBao = "Email đã tồn tại!";
                return View(model);
            }
            Accounts acc = new Accounts()
            {
                Name = model.Name,
                Email = model.Email,
                password = Crypto.HashPassword(model.Password),
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

            var viewModel = new EditProfileViewModels
            {
                Name = user.Name,
                Email = user.Email, // Để email ở đây cho người ta xem
                PhoneNumber = user.Phone,
                Avatar = user.Avatar
            };

            return View(user);
        }

        // [POST] PROFILE - HÀM LƯU THAY ĐỔI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(EditProfileViewModels model)
        {
            // Check 1: Bắt buộc đăng nhập
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            // Check 2: Kiểm tra validation (như [Required]...)
            if (!ModelState.IsValid)
            {
                return View(model); // Lỗi thì trả về, báo lỗi
            }

            // Lấy ID từ Session và tìm lại user
            int id = (int)Session["UserID"];
            var userToUpdate = db.Accounts.Find(id);

            // Cập nhật thông tin từ model (form) vào user (database)
            userToUpdate.Name = model.Name;
            userToUpdate.Phone = model.PhoneNumber;
            userToUpdate.update_at = DateTime.Now; // Cập nhật ngày
            userToUpdate.update_by = userToUpdate.Email;

            // (Code xử lý Upload file Avatar thì phức tạp hơn, mình sẽ làm sau)

            // Lưu thay đổi xuống database
            db.SaveChanges();

            // Cập nhật lại Session["UserName"] nếu họ đổi tên
            Session["UserName"] = userToUpdate.Name;

            // Gửi thông báo thành công và tải lại trang
            TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
            return RedirectToAction("Profile");
        }

    }
}