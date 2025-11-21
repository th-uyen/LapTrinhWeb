using BanDoGiaDung.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace BanDoGiaDung.Areas.Admin.Controllers
{
    public class DiscountController : Controller
    {
        // GET: Admin/Discount
        private readonly GiaDungDbContext db = new GiaDungDbContext();

        public ActionResult Index(string search, string code, string status, string sortOrder, int? page)
        {
            // Check 1: Bắt buộc đăng nhập
            if (Session["UserID"] == null)
                return RedirectToAction("Logout", "Account", new { area = "" });

            ViewBag.Title = "Chương trình giảm giá";

            var discounts = db.Discounts.AsQueryable();

            ModelState.Remove("create_by");
            ModelState.Remove("update_by");

            /* =================== TÌM KIẾM =================== */

            // Tìm theo tên
            if (!string.IsNullOrEmpty(search))
            {
                discounts = discounts.Where(x => x.discount_name.Contains(search));
            }

            // Tìm theo mã code
            if (!string.IsNullOrEmpty(code))
            {
                discounts = discounts.Where(x => x.discount_code.Contains(code));
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status))
            {
                DateTime now = DateTime.Now;

                if (status == "active")
                    discounts = discounts.Where(x => x.discount_star <= now && x.discount_end >= now);

                if (status == "expired")
                    discounts = discounts.Where(x => x.discount_end < now);

                if (status == "upcoming")
                    discounts = discounts.Where(x => x.discount_star > now);

                if (status == "soldout")
                    discounts = discounts.Where(x => x.quantity <= 0);
            }

            /* =================== SẮP XẾP =================== */

            switch (sortOrder)
            {
                case "name":
                    discounts = discounts.OrderBy(x => x.discount_name);
                    break;

                case "price":
                    discounts = discounts.OrderByDescending(x => x.discount_price);
                    break;

                case "quantity":
                    discounts = discounts.OrderByDescending(x => x.quantity);
                    break;

                default:
                    discounts = discounts.OrderByDescending(x => x.disscount_id); // mặc định
                    break;
            }

            /* =================== PHÂN TRANG =================== */

            int pageSize = 6;
            int pageNumber = page ?? 1;

            return View(discounts.ToPagedList(pageNumber, pageSize));
        }


        // ==================== THÊM MỚI - GET ====================
        [HttpGet]
        public ActionResult Create()
        {

            // Check 1: Bắt buộc đăng nhập
            if (Session["UserID"] == null)
                return RedirectToAction("Logout", "Account", new { area = "" });

            ViewBag.Title = "Thêm chương trình giảm giá";
            return View();
        }

        // ==================== THÊM MỚI - POST ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Discount model)
        {
            ModelState.Remove("create_by");
            ModelState.Remove("update_by");
            ModelState.Remove("create_at"); // Quan trọng: Bỏ qua lỗi ngày tạo
            ModelState.Remove("update_at"); // Quan trọng: Bỏ qua lỗi ngày sửa

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra ngày hợp lệ
            if (model.discount_star >= model.discount_end)
            {
                ModelState.AddModelError("", "Ngày kết thúc phải sau ngày bắt đầu!");
                return View(model);
            }

            // Kiểm tra mã giảm giá trùng
            if (!string.IsNullOrEmpty(model.discount_code))
            {
                var exists = db.Discounts.Any(x => x.discount_code == model.discount_code);
                if (exists)
                {
                    ModelState.AddModelError("discount_code", "Mã giảm giá đã tồn tại!");
                    return View(model);
                }
            }

            if (Session["UserName"] == null)
            {
                ModelState.AddModelError("", "Bạn cần đăng nhập để thực hiện thao tác này!");
                return View(model);
            }



            //model.create_at = DateTime.Now;
            //model.create_by = Session["UserName"].ToString();

            model.create_at = DateTime.Now;
            model.create_by = Session["UserName"].ToString();   // ← Lưu USERNAME
            model.update_at = DateTime.Now;
            model.update_by = "";
            // hoặc để bằng "", miễn không null

            db.Configuration.ValidateOnSaveEnabled = false;

            db.Discounts.Add(model);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Thêm chương trình giảm giá thành công!";
            return RedirectToAction("Index");
        }

        // ==================== CHỈNH SỬA - GET ====================
        [HttpGet]
        public ActionResult Edit(int id)
        {
            

            var discount = db.Discounts.Find(id);
            if (discount == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy chương trình giảm giá!";
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Chỉnh sửa chương trình giảm giá";
            return View(discount);
        }

        // ==================== CHỈNH SỬA - POST ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Discount model)
        {
            ModelState.Remove("create_by");
            ModelState.Remove("update_by");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var discount = db.Discounts.Find(model.disscount_id);
            if (discount == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy chương trình giảm giá!";
                return RedirectToAction("Index");
            }

            // Kiểm tra ngày hợp lệ
            if (model.discount_star >= model.discount_end)
            {
                ModelState.AddModelError("", "Ngày kết thúc phải sau ngày bắt đầu!");
                return View(model);
            }

            // Kiểm tra mã giảm giá trùng (trừ chính nó)
            if (!string.IsNullOrEmpty(model.discount_code))
            {
                var exists = db.Discounts.Any(x =>
                    x.discount_code == model.discount_code &&
                    x.disscount_id != model.disscount_id);

                if (exists)
                {
                    ModelState.AddModelError("discount_code", "Mã giảm giá đã tồn tại!");
                    return View(model);
                }
            }

            discount.discount_name = model.discount_name;
            discount.discount_star = model.discount_star;
            discount.discount_end = model.discount_end;
            discount.discount_price = model.discount_price;
            discount.quantity = model.quantity;
            discount.discount_code = model.discount_code;
            discount.update_at = DateTime.Now;
            discount.update_by = Session["UserName"].ToString();

            db.Configuration.ValidateOnSaveEnabled = false;

            db.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật chương trình giảm giá thành công!";
            return RedirectToAction("Index");
        }

        // ==================== XÓA ====================
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var discount = db.Discounts.Find(id);
                if (discount == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chương trình giảm giá!" });
                }

                db.Discounts.Remove(discount);
                db.SaveChanges();

                return Json(new { success = true, message = "Xóa chương trình giảm giá thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}