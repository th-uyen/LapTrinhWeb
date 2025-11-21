using BanDoGiaDung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoGiaDung.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // Nhớ bật cái này khi chạy thật nha
    public class HomeController : Controller
    {
        private readonly GiaDungDbContext db = new GiaDungDbContext();

        // GET: Admin/Home/Index
        public ActionResult Index()
        {
            // 1. Tổng số sản phẩm (Total Product)
            ViewBag.TotalProduct = db.Products.Count();

            // 2. Tổng doanh thu (Total Revenue)
            // (Cộng tổng tiền của tất cả đơn hàng đã thanh toán/hoàn thành)
            // Giả sử Status = 3 là đã giao hàng/thanh toán
            ViewBag.TotalRevenue = db.Orders
                //.Where(o => o.Status == 3) 
                .Select(o => o.total) // Giả sử cột tổng tiền là TotalAmount
                .DefaultIfEmpty(0)
                .Sum();

            // 3. Tổng đơn hàng (Total Order)
            ViewBag.TotalOrder = db.Orders.Count();

            // 4. Tổng số lượng đã bán (Total Sold)
            // (Phải vào bảng OrderDetail để cộng dồn số lượng)
            //ViewBag.TotalSold = db.OrderDetails.Sum(d => d.Quantity);
            ViewBag.TotalSold = 100; // (Tạm thời fix cứng nếu bà chưa có bảng OrderDetail)


            // 5. Lấy 5 đơn hàng mới nhất để hiện bảng bên dưới
            var recentOrders = db.Orders.OrderByDescending(o => o.oder_date).Take(5).ToList();

            return View(recentOrders);
        }
    }
}