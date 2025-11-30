using BanDoGiaDung.Models;
using BanDoGiaDung.Models.Account;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BanDoGiaDung.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private readonly GiaDungDbContext db = new GiaDungDbContext();
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

            if (Crypto.VerifyHashedPassword(user.password, model.Password))
            {
                // === ĐĂNG NHẬP THÀNH CÔNG ===

                // Lưu Session (Code của bà làm đúng rồi)
                Session["UserID"] = user.account_id;   // số
                Session["UserName"] = user.Name;       // chuỗi

                Session["UserEmail"] = user.Email;
                Session["Role"] = user.Role;


                // 2. Xác định tên quyền (Mapping từ số 0 sang chữ "Admin")
                string userRole = "";
                if (user.Role == 0)
                {
                    userRole = "Admin"; // Quan trọng: Chuỗi này phải khớp với chữ trong [Authorize]
                }
                else
                {
                    userRole = "User";
                }

                // 3. Tạo vé thông hành (FormsAuthenticationTicket)
                var ticket = new System.Web.Security.FormsAuthenticationTicket(
                    1,                      // Version
                    user.Email,             // Tên user (lưu email)
                    DateTime.Now,           // Thời điểm tạo
                    DateTime.Now.AddMinutes(60), // Hết hạn sau 60 phút
                    false,                  // Lưu nhớ? (Persistent)
                    userRole                // <--- QUAN TRỌNG: Lưu chuỗi "Admin" vào đây
                );

                // 4. Mã hóa vé
                string encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);

                // 5. Tạo Cookie
                var authCookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);

                // ================================

                // Chuyển hướng
                if (user.Role == 0)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

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
            // Check 1: Bắt buộc đăng nhập
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            // Tạo 1 model rỗng để truyền ra View
            var model = new ChangePasswordViewModels();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModels model)
        {
            // Check 1: Bắt buộc đăng nhập
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            // Check 2: Kiểm tra validation (3 ô có nhập đủ không,
            // 2 pass mới có khớp không [Compare]...)
            if (!ModelState.IsValid)
            {
                return View(model); // Lỗi thì trả về, báo lỗi
            }

            // Check 3: Check mật khẩu CŨ có đúng không
            int id = (int)Session["UserID"];
            var user = db.Accounts.Find(id);

            // Dùng hàm "Verify" xịn
            if (Crypto.VerifyHashedPassword(user.password, model.OldPassword))
            {
                // === Mật khẩu cũ ĐÚNG ===

                // 4. Băm và lưu mật khẩu MỚI
                user.password = Crypto.HashPassword(model.NewPassword);
                user.update_at = DateTime.Now;
                user.update_by = user.Email;

                db.SaveChanges();

                // 5. Gửi thông báo thành công và đá về trang Profile
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("EditProfile");
            }
            else
            {
                // === Mật khẩu cũ SAI ===
                // 6. Báo lỗi
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không chính xác.");
                return View(model);
            }
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


        // [GET] PROFILE - HÀM HIỂN THỊ (Bà check lại xem đã sửa chưa)
        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int id = (int)Session["UserID"];
            var user = db.Accounts.Find(id); // 1. Tìm "Kho"

            // 2. Map từ "Kho" (user) sang "Phiếu" (viewModel)
            var viewModel = new EditProfileViewModels
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.Phone,
                Avatar = user.Avatar
            };

            return View(viewModel); // 3. Trả "Phiếu" ra View
        }

        // [POST] PROFILE - HÀM LƯU THAY ĐỔI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileViewModels model)
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
            return RedirectToAction("EditProfile");
        }



        // ==================== DANH SÁCH ĐỊA CHỈ ====================
        [HttpGet]
        public ActionResult Address()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int userId = (int)Session["UserID"];
            var addresses = db.AccountAddresses
                .Include(a => a.Provinces)
                .Include(a => a.Districts)
                .Include(a => a.Wards)
                .Where(a => a.account_id == userId)
                .OrderByDescending(a => a.isDefault)
                .ThenByDescending(a => a.account_address_id)
                .ToList();

            return View(addresses);
        }

        // ==================== THÊM ĐỊA CHỈ - GET ====================
        [HttpGet]
        public ActionResult CreateAddress()
        {
            if (Session["UserID"] == null) return RedirectToAction("Login");

            // 1. List Tỉnh (Có dữ liệu)
            ViewBag.Provinces = new SelectList(db.Provinces.OrderBy(p => p.province_name), "province_id", "province_name");

            // 2. List Huyện (RỖNG nhưng PHẢI KHỞI TẠO new List)
            ViewBag.Districts = new SelectList(new List<Districts>(), "district_id", "district_name");

            // 3. List Xã (RỖNG nhưng PHẢI KHỞI TẠO new List)
            ViewBag.Wards = new SelectList(new List<Wards>(), "ward_id", "ward_name");

            return View();
        }

        // ==================== THÊM ĐỊA CHỈ - POST ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddress(AccountAddress model, string submitType)
        {
            // Load lại dữ liệu (bắt buộc)
            ViewBag.Provinces = new SelectList(db.Provinces.OrderBy(p => p.province_name), "province_id", "province_name", model.province_id);

            ViewBag.Districts = new SelectList(
                db.Districts.Where(d => d.province_id == model.province_id),
                "district_id",
                "district_name",
                model.district_id
            );

            ViewBag.Wards = new SelectList(
                db.Wards.Where(w => w.district_id == model.district_id),
                "ward_id",
                "ward_name",
                model.ward_id
            );

            // Nếu đang chọn tỉnh/huyện → KHÔNG LƯU DB
            if (submitType == "reload")
                return View(model);

            // Khi nhấn Lưu → kiểm tra validation
            if (!ModelState.IsValid)
                return View(model);

            model.account_id = (int)Session["UserID"];
            db.AccountAddresses.Add(model);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Thêm địa chỉ thành công!";
            return RedirectToAction("Address");
        }


        // ==================== SỬA ĐỊA CHỈ - GET ====================
        [HttpGet]
        public ActionResult EditAddress(int id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int userId = (int)Session["UserID"];
            var address = db.AccountAddresses.FirstOrDefault(a => a.account_address_id == id && a.account_id == userId);

            //if (address == null)
            //{
            //    TempData["ErrorMessage"] = "Không tìm thấy địa chỉ!";
            //    return RedirectToAction("Address");
            //}

            // Load dropdown data
            ViewBag.Provinces = db.Provinces.OrderBy(p => p.province_name).ToList();
            ViewBag.Districts = db.Districts.Where(d => d.province_id == address.province_id).ToList();
            ViewBag.Wards = db.Wards.Where(w => w.district_id == address.district_id).ToList();

            return View(address);
        }

        // ==================== SỬA ĐỊA CHỈ - POST ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAddress(AccountAddress model, string reloadFlag)
        {
            int userId = (int)Session["UserID"];

            // Load dropdown Tỉnh – Huyện – Xã trước
            ViewBag.Provinces = db.Provinces.OrderBy(p => p.province_name).ToList();
            ViewBag.Districts = db.Districts
                .Where(d => d.province_id == model.province_id)
                .OrderBy(d => d.district_name)
                .ToList();
            ViewBag.Wards = db.Wards
                .Where(w => w.district_id == model.district_id)
                .OrderBy(w => w.ward_name)
                .ToList();

            // Nếu chỉ reload dropdown → KHÔNG validate, KHÔNG lưu
            if (reloadFlag == "1")
            {
                return View(model);
            }

            // Tới đây mới là nhấn nút Cập nhật thực sự
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var address = db.AccountAddresses.Find(model.account_address_id);

            if (address == null || address.account_id != userId)
            {
                TempData["ErrorMessage"] = "Không tìm thấy địa chỉ!";
                return RedirectToAction("Address");
            }

            // Nếu chọn làm mặc định
            if (model.isDefault && !address.isDefault)
            {
                var existingAddresses = db.AccountAddresses
                    .Where(a => a.account_id == userId && a.account_address_id != model.account_address_id)
                    .ToList();

                foreach (var addr in existingAddresses)
                {
                    addr.isDefault = false;
                }
            }

            // Cập nhật dữ liệu
            address.accountUsername = model.accountUsername;
            address.accountPhoneNumber = model.accountPhoneNumber;
            address.province_id = model.province_id;
            address.district_id = model.district_id;
            address.ward_id = model.ward_id;
            address.content = model.content;
            address.isDefault = model.isDefault;

            db.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật địa chỉ thành công!";
            return RedirectToAction("Address");
        }


        // ==================== XÓA ĐỊA CHỈ ====================
        [HttpPost]
        public ActionResult DeleteAddress(int id)
        {
            int userId = (int)Session["UserID"];

            var address = db.AccountAddresses
                .FirstOrDefault(a => a.account_address_id == id && a.account_id == userId);

            if (address == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy địa chỉ!";
                return RedirectToAction("Address");
            }

            db.AccountAddresses.Remove(address);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Xóa địa chỉ thành công!";
            return RedirectToAction("Address");
        }


        // ==================== ĐẶT ĐỊA CHỈ MẶC ĐỊNH ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetDefaultAddress(int id)
        {
            int userId = (int)Session["UserID"];
            var address = db.AccountAddresses.FirstOrDefault(a => a.account_address_id == id && a.account_id == userId);

            if (address == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy địa chỉ!";
                return RedirectToAction("Address");
            }

            // Bỏ default của tất cả địa chỉ khác
            var allAddresses = db.AccountAddresses.Where(a => a.account_id == userId).ToList();
            foreach (var addr in allAddresses)
            {
                addr.isDefault = false;
            }

            // Set địa chỉ này làm mặc định
            address.isDefault = true;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đặt làm địa chỉ mặc định thành công!";
            return RedirectToAction("Address");
        }

        // ==================== LOAD QUẬN/HUYỆN THEO TỈNH ====================
        [HttpGet]
        public JsonResult GetDistricts(int provinceId)
        {
            var districts = db.Districts
                .Where(d => d.province_id == provinceId)
                .OrderBy(d => d.district_name)
                .Select(d => new
                {
                    id = d.district_id,
                    name = d.district_name
                }).ToList();

            return Json(districts, JsonRequestBehavior.AllowGet);
        }

        // ==================== LOAD PHƯỜNG/XÃ THEO QUẬN ====================
        [HttpGet]
        public JsonResult GetWards(int districtId)
        {
            var wards = db.Wards
                .Where(w => w.district_id == districtId)
                .OrderBy(w => w.ward_name)
                .Select(w => new
                {
                    id = w.ward_id,
                    name = w.ward_name
                })
                .ToList();

            return Json(wards, JsonRequestBehavior.AllowGet);
        }


    }
}