using BanDoGiaDung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace BanDoGiaDung.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        // GET: Admin/Brand

        private readonly GiaDungDbContext  db = new GiaDungDbContext();

        public ActionResult Index(string search, string sortOrder, int? page)
        {
            ViewBag.Title = "Hãng Sản Phẩm";

            var brand = db.Brands.AsQueryable();

            ModelState.Remove("create_by");
            ModelState.Remove("update_by");

            /* =================== TÌM KIẾM =================== */

            // Tìm theo tên
            if (!string.IsNullOrEmpty(search))
            {
                brand = brand.Where(x => x.brand_name.Contains(search));
            }

            

            /* =================== SẮP XẾP =================== */

            switch (sortOrder)
            {
                case "name":
                    brand = brand.OrderBy(x => x.brand_name);
                    break;
                default:
                    brand = brand.OrderByDescending(x => x.brand_id); // mặc định
                    break;
            }

            /* =================== PHÂN TRANG =================== */

            int pageSize = 6;
            int pageNumber = page ?? 1;

            return View(brand.ToPagedList(pageNumber, pageSize));
        }

        // ==================== THÊM MỚI - GET ====================
        [HttpGet]
        public ActionResult Create()
        {
            // Check 1: Bắt buộc đăng nhập
            if (Session["UserID"] == null)
                return RedirectToAction("Logout", "Account", new { area = "" });

            ViewBag.Title = "Thêm Nhà Sản Xuất Mới";
            return View();
        }


        // ==================== THÊM MỚI - POST ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand model)
        {
            ModelState.Remove("create_by");
            ModelState.Remove("update_by");
            ModelState.Remove("create_at"); // Quan trọng: Bỏ qua lỗi ngày tạo
            ModelState.Remove("update_at"); // Quan trọng: Bỏ qua lỗi ngày sửa

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check 1: Bắt buộc đăng nhập
            if (Session["UserID"] == null)
                return RedirectToAction("Logout", "Account", new { area = "" });


            // Kiểm tratên nhà sản xuất trùng
            if (!string.IsNullOrEmpty(model.brand_name))
            {
                var exists = db.Brands.Any(x => x.brand_name == model.brand_name);
                if (exists)
                {
                    ModelState.AddModelError("brand_name", "Tên nhà sản xuất đã tồn tại!");
                    return View(model);
                }
            }

            if (Session["UserName"] == null)
            {
                ModelState.AddModelError("", "Bạn cần đăng nhập để thực hiện thao tác này!");
                return View(model);
            }

            model.create_at = DateTime.Now;
            model.create_by = Session["UserName"].ToString();   // ← Lưu USERNAME
            model.update_at = DateTime.Now;
            model.update_by = "";
            // hoặc để bằng "", miễn không null

            db.Configuration.ValidateOnSaveEnabled = false;

            db.Brands.Add(model);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Thêm nhà sản xuất mới thành công!";
            return RedirectToAction("Index");
        }

        // ==================== XÓA ====================
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var brand = db.Brands.Find(id);
                if (brand == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chương trình giảm giá!" });
                }

                db.Brands.Remove(brand);
                db.SaveChanges();

                return Json(new { success = true, message = "Xóa chương trình giảm giá thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}