using BanDoGiaDung.Models;
using BanDoGiaDung.Models.Cart; // <-- Phải "using" cái thư mục Cart của bà
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoGiaDung.Controllers
{
    public class CartController : Controller
    {
        private GiaDungDbContext db = new GiaDungDbContext();

        // ==================== TRANG GIỎ HÀNG CHÍNH ====================
        // GET: /Cart/Index
        public ActionResult Index()
        {
            var cart = GetCart();
            return View(cart); // Trả List<CartItem> ra View
        }

        // ==================== THÊM VÀO GIỎ (BẰNG AJAX) ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }

            // === SỬA LỖI CHECK TỒN KHO (Đọc từ Product.cs) ===
            int stock = 0;
            // Chuyển "Hàng Tồn Kho" (string) sang (int)
            Int32.TryParse(product.quantity, out stock);

            if (stock <= 0)
            {
                return Json(new { success = false, message = "Sản phẩm đã hết hàng!" });
            }
            // === HẾT SỬA ===

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(x => x.Id == id);

            if (existingItem != null)
            {
                // Đã có $\rightarrow$ Tăng số lượng
                if (existingItem.Quantity + 1 > stock) // Check tồn kho
                {
                    return Json(new { success = false, message = "Vượt quá số lượng tồn kho!" });
                }
                existingItem.Quantity++;
            }
            else
            {
                // Chưa có $\rightarrow$ Thêm mới (Map từ Product sang CartItem)
                cart.Add(new CartItem
                {
                    Id = product.product_id,
                    Name = product.product_name,
                    Image = product.image, // Lấy từ product.image
                    Price = product.price, // Lấy từ product.price
                    Quantity = 1, // "Số lượng MUA" là 1
                });
            }

            SaveCart(cart);

            // Trả về JSON để AJAX biết là thành công
            return Json(new { success = true, message = "Đã thêm vào giỏ hàng!" });
        }

        // ==================== CẬP NHẬT GIỎ HÀNG (BẰNG AJAX) ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCart(int id, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return Json(new { success = false, message = "Lỗi! Không tìm thấy sản phẩm." });
            }

            // === SỬA LỖI CHECK TỒN KHO (Phải tìm lại Product) ===
            var product = db.Products.Find(id);
            int stock = 0;
            Int32.TryParse(product.quantity, out stock); // Chuyển string sang int

            if (quantity > stock)
            {
                // Trả về số lượng TỒN KHO (stock) nếu gõ lố
                return Json(new { success = false, message = "Vượt quá số lượng tồn kho! (Còn " + stock + ")", newQuantity = stock });
            }
            // === HẾT SỬA ===

            if (quantity <= 0)
            {
                cart.Remove(item); // Nếu số lượng = 0 thì xoá
            }
            else
            {
                item.Quantity = quantity;
            }

            SaveCart(cart);

            // Trả về giá mới và tổng tiền mới
            return Json(new
            {
                success = true,
                itemTotal = item.TotalPrice.ToString("N0"), // Dùng TotalPrice của CartItem
                cartTotal = GetTotal().ToString("N0")
            });
        }

        // ==================== XOÁ 1 SẢN PHẨM (BẰNG AJAX) ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return Json(new
            {
                success = true,
                cartTotal = GetTotal().ToString("N0")
            });
        }

        // ==================== XOÁ SẠCH GIỎ HÀNG ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return Json(new { success = true });
        }

        // ==================== LẤY GIỎ HÀNG MINI (CHO HEADER) ====================
        [ChildActionOnly]
        public PartialViewResult CartSummary()
        {
            var cart = GetCart();
            ViewBag.CartCount = cart.Sum(c => c.Quantity);
            ViewBag.CartTotal = GetTotal().ToString("N0");
            return PartialView("_CartSummary");
        }

        // Dùng để AJAX gọi (cập nhật giỏ hàng mini)
        public JsonResult GetCartSummaryJson()
        {
            var cart = GetCart();
            return Json(new
            {
                count = cart.Sum(c => c.Quantity),
                total = GetTotal().ToString("N0")
            }, JsonRequestBehavior.AllowGet);
        }


        // ==================== HÀM NỘI BỘ (PRIVATE) ====================
        private List<CartItem> GetCart()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        private void SaveCart(List<CartItem> cart)
        {
            Session["Cart"] = cart;
        }

        private double GetTotal()
        {
            var cart = GetCart();
            return cart.Sum(x => x.TotalPrice); // Dùng TotalPrice của CartItem
        }
    }
}